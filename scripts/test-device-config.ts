/**
 * Addendum commit 1 (P0+P1) test suite — MOCK ONLY.
 * =====================================================================
 * Everything here runs against pure in-process validators and the MOCK
 * bridge (scripts/mock-fkattend-bridge.js). NO real hardware (NP756+) is
 * involved — hardware validation happens on the Windows machine with
 * tools/fkattend-bridge/CHECKLIST-WINDOWS.md.
 *
 * Covered (addendum acceptance items 1, 2, 13 and the P1 rules):
 *   [validation] default port 4238 for FKATTEND (but freely editable),
 *                protocol family is EXPLICIT (never derived from port),
 *                interval/timeout/poll ranges, localhost-only bridge URL,
 *                password/bridge-token keep semantics
 *   [secrets]    bridgeToken never leaves sanitizeDeviceForApi
 *   [fk e2e]     full import through the mock bridge → RawPunch (UNKNOWN
 *                direction, punchTimeRaw preserved, unmatched kept),
 *                dedupe on second run, concurrent sync → SyncBusyError,
 *                serial mismatch → import stops, port/IP/machine params
 *                propagate to the bridge (meta echo), test-connection
 *                success/failure/latency, bridge-down Persian error,
 *                failure injection + retry recovery
 *
 * Run (DATABASE_URL REQUIRED — refuses db/custom.db):
 *   DATABASE_URL=file:/tmp/nr-addendum1/addendum1.db npx tsx scripts/test-device-config.ts
 */
import { spawn } from 'child_process'
import { rmSync, mkdirSync } from 'fs'
import path from 'path'
import { db } from '../src/lib/db'
import { validateDeviceSettings } from '../src/lib/device-validation'
import { sanitizeDeviceForApi } from '../src/lib/device-secrets'
import { fkTestConnection, importFkAttendLogs, fkWallClockToUTC, SyncBusyError } from '../src/lib/fkattend'
import { SerialMismatchError } from '../src/lib/raw-sync'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-addendum1'
const MAIN_DB = process.env.DATABASE_URL || ''
const MAIN_DB_FILE = MAIN_DB.replace(/^file:/, '')

let passed = 0
let failed = 0
const failures: string[] = []

function section(t: string) { console.log(`\n\x1b[1m━━━ ${t}\x1b[0m`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  \x1b[32m✔\x1b[0m ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  \x1b[31m✘\x1b[0m ${name}${detail ? `  (${detail})` : ''}`) }
}

const SERIAL = '7561202505176'
const BRIDGE_PORT = 8791

async function main() {
  if (!MAIN_DB) throw new Error('DATABASE_URL must point at a TEMP database, e.g. file:/tmp/nr-addendum1/addendum1.db')
  if (/custom\.db/i.test(MAIN_DB)) throw new Error('refusing to run against db/custom.db — point DATABASE_URL at a temp database')
  mkdirSync(TMP, { recursive: true })

  /* ================================================================
   * 1) VALIDATION — per-device connection settings (P1)
   * ================================================================ */
  section('VALIDATION — default port, explicit family, ranges')

  const v1 = validateDeviceSettings(
    { name: 'دستگاه اصلی', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', ipAddress: '192.168.1.75' },
    { isCreate: true },
  )
  check('FKATTEND create without port → default 4238', v1.ok && v1.data.port === 4238, v1.ok ? `port=${v1.data.port}` : (v1 as { error: string }).error)

  const v2 = validateDeviceSettings(
    { name: 'دستگاه', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'SBXPC', port: 4238 },
    { isCreate: true },
  )
  check('family is EXPLICIT — port 4238 with family SBXPC stays SBXPC (never guessed from port)', v2.ok && v2.data.protocolFamily === 'SBXPC' && v2.data.port === 4238)

  const v3 = validateDeviceSettings(
    { name: 'دستگاه', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', port: 4240 },
    { isCreate: true },
  )
  check('port freely editable (4238 → 4240 on FKATTEND)', v3.ok && v3.data.port === 4240)

  const v4 = validateDeviceSettings({ name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'SBXPC' }, { isCreate: true })
  check('SBXPC create default port 5005 (legacy behavior preserved)', v4.ok && v4.data.port === 5005)

  const v5 = validateDeviceSettings({ name: 'x', type: 'ZKTECO' }, { isCreate: true })
  check('ZK device default port 4370', v5.ok && v5.data.port === 4370)

  const v6 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', syncIntervalMin: 2000 },
    { isCreate: true },
  )
  check('auto-sync interval 2000 min rejected (> 1440)', !v6.ok)

  const v7 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', syncIntervalMin: 5 },
    { isCreate: true },
  )
  check('auto-sync interval 5 accepted', v7.ok && v7.data.syncIntervalMin === 5)

  const v8 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', rtEnabled: true, rtPollSec: 5 },
    { isCreate: true },
  )
  check('rt polling 5s rejected (< 15 floor — protects the device)', !v8.ok)

  const v9 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', bridgeUrl: 'http://192.168.1.9:8765' },
    { isCreate: true },
  )
  check('non-localhost bridge URL rejected (security rule)', !v9.ok && (v9 as { error: string }).error.includes('همین سیستم'))

  const v10 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', bridgeUrl: 'http://127.0.0.1:8765', connTimeoutMs: 999 },
    { isCreate: true },
  )
  check('conn timeout < 1000ms rejected', !v10.ok)

  const v11 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', bridgeUrl: 'http://127.0.0.1:8765', connTimeoutMs: 8000, machineNumber: 7, sdkLicense: 1261, password: 123456 },
    { isCreate: true },
  )
  check('full FKATTEND profile validates', v11.ok && v11.data.connTimeoutMs === 8000 && v11.data.machineNumber === 7 && v11.data.password === 123456)

  const v12 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND' },
    { isCreate: false, existing: { protocolFamily: 'FKATTEND', port: 4238, password: 4891489 } },
  )
  check('edit without password keeps the stored secret', v12.ok && (v12.data.password === 4891489))

  const v13 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', password: '' },
    { isCreate: false, existing: { protocolFamily: 'FKATTEND', port: 4238, password: 4891489 } },
  )
  check('edit with empty password CLEARS it', v13.ok && v13.data.password === null)

  const v14 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL },
    { isCreate: false, existing: { protocolFamily: 'SBXPC', port: 5005 } },
  )
  check('edit without family keeps the stored family', v14.ok && v14.data.protocolFamily === 'SBXPC')

  /* ---- secrets ---- */
  section('SECRETS — bridge token handling')
  const safe = sanitizeDeviceForApi({
    id: 'd1', name: 'X', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
    password: 4891489, bridgeToken: 'secret-token-value',
    bridgeUrl: 'http://127.0.0.1:8765', port: 4238, status: 'ONLINE', enabled: true, lastSyncCount: 0,
  })
  check('sanitizeDeviceForApi strips bridgeToken AND password', safe.password === undefined && safe.bridgeToken === undefined && safe.hasPassword === true && safe.hasBridgeToken === true)
  check('no secret value in the serialized row', !JSON.stringify(safe).includes('secret-token-value') && !JSON.stringify(safe).includes('4891489'))

  /* ================================================================
   * 2) wall-clock conversion (shared with the SBXPC path)
   * ================================================================ */
  section('WALL CLOCK → UTC (DST-correct, shared helper)')
  const t1 = fkWallClockToUTC('2026-11-16 10:12:30') // Tehran, UTC+3:30 (no DST since 2022)
  check('winter wall clock → UTC 06:42:30', t1?.toISOString() === '2026-11-16T06:42:30.000Z', t1?.toISOString())
  const t2 = fkWallClockToUTC('2016-06-21 10:12:30') // Tehran DST era → UTC+4:30
  check('historical DST wall clock → UTC 05:42:30', t2?.toISOString() === '2016-06-21T05:42:30.000Z', t2?.toISOString())
  const t3 = fkWallClockToUTC('garbage')
  check('garbage timestamp → null (never an invented instant)', t3 === null)

  /* ================================================================
   * 3) prepare DB (Phase-1 schema + v1.4.0 migration) + device rows
   * ================================================================ */
  rmSync(MAIN_DB_FILE, { force: true })
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  const { spawnSync } = await import('child_process')
  const { writeFileSync } = await import('fs')
  const sh = (cmd: string, args: string[], env: Record<string, string>) =>
    spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf8', timeout: 120_000 })
  // base = the v1.3.0 (Phase 1) schema, as a real user's DB would look like
  const phase1Schema = sh('git', ['show', 'HEAD:prisma/schema.prisma'], {})
  writeFileSync(`${TMP}/phase1-schema.prisma`, phase1Schema.stdout)
  sh('npx', ['prisma', 'db', 'push', '--schema', `${TMP}/phase1-schema.prisma`, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  const up14 = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.4.0 migration up (on a Phase-1 DB)', up14.status === 0 && /Device.rtPollSec: present/.test(up14.stdout))
  const up14b = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.4.0 migration idempotent', up14b.status === 0 && /already present/.test(up14b.stdout))
  const parity = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: MAIN_DB })
  const parityOut = (parity.stdout || '') + (parity.stderr || '')
  check('prisma db push sees no drift (INSTALL.bat parity)', parity.status === 0 && /already in sync/i.test(parityOut))

  const p1 = await db.personnel.create({ data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 } })
  const device = await db.device.create({
    data: {
      name: 'دستگاه آزمایشی FK', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: SERIAL,
      connTimeoutMs: 3000, machineNumber: 1, sdkLicense: 1261,
      bridgeUrl: `http://127.0.0.1:${BRIDGE_PORT}`, bridgeToken: 'test-token',
    },
  })

  /* ================================================================
   * 4) FK E2E via the MOCK bridge
   * ================================================================ */
  const mock = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', SERIAL, LOGS: '60', USERS: '1001,7777' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  const mockErr: string[] = []
  mock.stderr?.on('data', (d) => mockErr.push(String(d)))
  await new Promise((r) => setTimeout(r, 800))

  section('TEST CONNECTION (read-only, via mock bridge)')
  const tRes = await fkTestConnection(device)
  check('connection ok', tRes.ok === true, JSON.stringify(tRes))
  check('serial reported and matches', tRes.serialNumber === SERIAL && tRes.serialMatch === true)
  check('latency measured', typeof tRes.latencyMs === 'number' && (tRes.latencyMs ?? 0) > 0)
  check('dialect = FKATTEND (explicit)', tRes.dialect === 'FKATTEND')
  check('device time reported (Tehran wall clock)', typeof tRes.deviceTime === 'string' && /^\d{4}-\d{2}-\d{2} /.test(tRes.deviceTime ?? ''))
  check('params propagate to bridge (machine/ip/port/license echo)', (tRes.meta as Record<string, unknown>)?.port === 4238 && (tRes.meta as Record<string, number>)?.machine === 1)

  /* ---- port change → new port propagates (acceptance item 1) ---- */
  const devPort2 = await db.device.update({ where: { id: device.id }, data: { port: 4240 } })
  const tRes2 = await fkTestConnection(devPort2)
  check('port 4238→4240 change reaches the bridge request (echo)', (tRes2.meta as Record<string, number>)?.port === 4240 && tRes2.ok)
  await db.device.update({ where: { id: device.id }, data: { port: 4238 } })

  section('FAILED CONNECTION (acceptance item 2)')
  // wrong token
  const badTok = await fkTestConnection({ ...device, bridgeToken: 'wrong' })
  check('wrong bridge token → clear Persian error', !badTok.ok && (badTok.error ?? '').includes('توکن'))
  // device unreachable via failure injection
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/flaky`, { method: 'POST', headers: { 'x-bridge-token': 'test-token' }, body: JSON.stringify({ failTimes: 1 }) })
  const badConn = await fkTestConnection(device)
  check('device connect failure surfaces the bridge error (code CONNECT_FAILED)', !badConn.ok && /CONNECT_FAILED|اتصال به دستگاه/.test(badConn.error ?? ''))
  check('failed test does not throw', badConn.ok === false && badConn.error != null)

  section('FULL IMPORT (P0) — RawPunch via mock bridge')
  const s1 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('60 records imported', s1.total === 60 && s1.created === 60, `total=${s1.total} created=${s1.created}`)
  const rawRows = await db.rawPunch.findMany({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })
  check('RawPunch rows stored with FKATTEND protocol', rawRows.length === 60)
  check('direction UNKNOWN for all (never guessed)', rawRows.every((r) => r.direction === 'UNKNOWN'))
  check('punchTimeRaw preserves the device wall clock', rawRows.every((r) => /^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/.test(r.punchTimeRaw)))
  check('inOutModeRaw/verifyModeRaw preserved', rawRows.every((r) => r.inOutModeRaw === 1 && r.verifyModeRaw >= 1 && r.verifyModeRaw <= 3))
  check('matched enroll 1001 linked to personnel', rawRows.find((r) => r.enrollNumber === '1001')?.personnelId === p1.id)
  check('unmatched enroll 7777 kept with UNMATCHED status', rawRows.filter((r) => r.enrollNumber === '7777').every((r) => r.matchStatus === 'UNMATCHED' && r.personnelId === null))
  const runRow = await db.deviceSyncRun.findFirst({ where: { deviceId: device.id }, orderBy: { startedAt: 'desc' } })
  check('DeviceSyncRun recorded with mode=MOCK and counts', runRow?.mode === 'MOCK' && runRow?.recordsNew === 60 && runRow?.recordsUnmatched === 30)
  const punches = await db.punchRecord.count({ where: { source: 'DEVICE', deviceId: device.id } })
  check('legacy PunchRecord projection created (30 matched only)', punches === 30)

  const s2 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('second import → all duplicates (idempotent fingerprints)', s2.created === 0 && s2.skipped === 60)
  check('no extra RawPunch rows', (await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })) === 60)

  section('CONCURRENT SYNC GUARD (acceptance item 6)')
  // start one slow import (bridge call takes ~real ms; hold it via a delayed
  // second attempt in parallel): use a manual Promise.all race
  const [a, b] = await Promise.allSettled([
    importFkAttendLogs(device.id, { mode: 'MOCK', full: true }),
    importFkAttendLogs(device.id, { mode: 'MOCK', full: true }),
  ])
  check('one of two concurrent syncs is rejected with SyncBusyError', (a.status === 'fulfilled' ? a.value : null) != null && (b.status === 'rejected' ? b.reason : null) instanceof SyncBusyError)

  section('SERIAL MISMATCH GUARD')
  const devForeign = await db.device.create({
    data: {
      name: 'دستگاه سریال غلط', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: 'OTHER-SERIAL-999',
      bridgeUrl: `http://127.0.0.1:${BRIDGE_PORT}`, bridgeToken: 'test-token',
    },
  })
  let mismatchThrew = false
  try { await importFkAttendLogs(devForeign.id, { mode: 'MOCK', full: true }) } catch (err) { mismatchThrew = err instanceof SerialMismatchError }
  check('bridge-reported serial ≠ profile serial → import stops before storing', mismatchThrew)
  check('no rows stored for the foreign device', (await db.rawPunch.count({ where: { deviceSerial: 'OTHER-SERIAL-999' } })) === 0)

  section('BRIDGE DOWN / NETWORK FLAP (acceptance item 11)')
  mock.kill('SIGTERM')
  await new Promise((r) => setTimeout(r, 400))
  const down = await fkTestConnection(device)
  check('bridge down → clear Persian error (not a crash)', !down.ok && /Bridge|اتصال رد شد|ارتباط با Bridge/.test(down.error ?? ''))
  const errRun = await db.deviceLog.findMany({ where: { deviceId: device.id, level: 'ERROR' } })
  check('device logs carry no secrets after errors', errRun.every((l) => !/test-token|4891489/.test(l.message)))

  /* ---- restart bridge → recovery ---- */
  const mock2 = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', SERIAL, LOGS: '60', USERS: '1001,7777' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  await new Promise((r) => setTimeout(r, 800))
  const tRes3 = await fkTestConnection(device)
  check('bridge restart → connection recovers', tRes3.ok === true)

  /* ---- flaky device + retry: import recovers (withRetry inside runRawSync) ----
     NOTE: mock2 regenerated its records after the restart (fresh timestamps),
     so the recovery import legitimately stores 60 NEW rows — the point of this
     check is that the run SUCCEEDS despite 2 injected consecutive failures. */
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/flaky`, { method: 'POST', headers: { 'x-bridge-token': 'test-token' }, body: JSON.stringify({ failTimes: 2 }) })
  const s3 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('import survives 2 injected consecutive failures via retry+backoff', s3.total === 60 && s3.created === 60, `total=${s3.total} created=${s3.created}`)
  const s4 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('recovery is stable — next import is fully idempotent', s4.created === 0 && s4.skipped === 60)

  mock2.kill('SIGTERM')

  /* ================================================================
   * summary — mock vs hardware explicitly separated
   * ================================================================ */
  console.log(`\n\x1b[1m━━━ SUMMARY (commit 1: P0+P1)\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  HARDWARE tests : 0 (none run — run tools/fkattend-bridge/CHECKLIST-WINDOWS.md on the Windows machine)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  await db.$disconnect()
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('SUITE CRASHED:', err)
  await db.$disconnect().catch(() => {})
  process.exit(1)
})
