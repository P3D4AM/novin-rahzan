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
 *   [form]       pure UI defaults module: new NP device → FKATTEND/4238,
 *                SBXPC → 5005, FKATTEND → 4238, ZKTeco → 4370, type-change
 *                harmonization (family/port always in sync), ZK payload never
 *                carries a contradictory NP family, legacy edit keeps stored
 *                family/port verbatim (SBXPC fallback preserved)
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
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'
import { validateDeviceSettings } from '../src/lib/device-validation'
import { sanitizeDeviceForApi } from '../src/lib/device-secrets'
import { fkWallClockToUTC } from '../src/lib/fkattend'
// device-form fix: the UI defaults/harmonization live in ONE pure module
import {
  newDeviceFormDefaults, harmonizeTypeSelection, harmonizeProtocolSelection,
  serverPayloadFamilyFor, suggestedPortForNpFamily, isNpProtocolFamily,
  NP_DEFAULT_FAMILY, DEFAULT_PORT_FKATTEND, DEFAULT_PORT_SBXPC, DEFAULT_PORT_ZKTECO,
} from '../src/lib/device-form-defaults'

// v1.3.1 (Phase 1 review fix 5): the suite AUTO-GENERATES its own unique
// temporary database under the test directory — an external DATABASE_URL is
// ignored and NEVER deleted; the schema baseline is pinned to the actual
// Phase 1 commit instead of a drifting HEAD. Db-dependent modules are
// imported dynamically AFTER DATABASE_URL points at the generated file.

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-addendum1'
const MAIN_DB_FILE = generateTestDatabasePath(TMP)
const MAIN_DB = sqliteUrl(MAIN_DB_FILE)
process.env.DATABASE_URL = MAIN_DB // before the db client is imported below

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
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(MAIN_DB, TMP)

  // db-dependent modules are imported AFTER DATABASE_URL points at the
  // auto-generated test database
  const { db } = await import('../src/lib/db')
  const { fkTestConnection, importFkAttendLogs, SyncBusyError } = await import('../src/lib/fkattend')
  const { SerialMismatchError } = await import('../src/lib/raw-sync')

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
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'FKATTEND', bridgeUrl: 'http://192.168.1.9:8765', bridgeToken: 'legacy-value' },
    { isCreate: true },
  )
  check('v1.5.0: legacy bridgeUrl/bridgeToken fields are IGNORED (bridge is internal)', v9.ok && (v9.data as unknown as Record<string, unknown>).bridgeUrl === undefined && (v9.data as unknown as Record<string, unknown>).bridgeToken === undefined)

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

  // device-form fix: editing a legacy device never migrates it — the stored
  // family/port are preserved verbatim; the SBXPC fallback for rows without
  // protocolFamily stays EXACTLY as before (backend fallback untouched).
  const v15 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'SBXPC', port: 6100 },
    { isCreate: false, existing: { protocolFamily: null, port: 6100 } },
  )
  check('edit of a legacy device keeps a custom port (6100) verbatim — no default rewrite', v15.ok && v15.data.port === 6100)
  const v16 = validateDeviceSettings(
    { name: 'x', type: 'NOVIN_NP', serialNumber: SERIAL, protocolFamily: 'SBXPC', port: 5005 },
    { isCreate: false, existing: { protocolFamily: null, port: null } },
  )
  check('legacy row without family/port → SBXPC/5005 (historical fallback preserved)', v16.ok && v16.data.protocolFamily === 'SBXPC' && v16.data.port === 5005)

  /* ================================================================
   * 1b) DEVICE FORM DEFAULTS & HARMONIZATION — pure UI-layer module
   * ================================================================ */
  section('FORM DEFAULTS — new-device trio, type/protocol/port harmonization')

  const nd = newDeviceFormDefaults()
  check('new-device default: NP + FKATTEND + port 4238 (approved NP756+ path)', nd.type === 'NOVIN_NP' && nd.protocolFamily === 'FKATTEND' && nd.port === 4238)
  check('new-device default transport is TCP', nd.protocol === 'TCP')
  check('NP_DEFAULT_FAMILY is FKATTEND and SBXPC remains a first-class choice', NP_DEFAULT_FAMILY === 'FKATTEND' && isNpProtocolFamily('SBXPC') && isNpProtocolFamily('FKATTEND'))
  check('suggested ports: FKATTEND 4238 / SBXPC 5005 / ZKTeco 4370', suggestedPortForNpFamily('FKATTEND') === 4238 && suggestedPortForNpFamily('SBXPC') === 5005 && DEFAULT_PORT_ZKTECO === 4370 && DEFAULT_PORT_FKATTEND === 4238 && DEFAULT_PORT_SBXPC === 5005)

  // protocol selection → suggested port (still freely editable afterwards)
  const hSbx = harmonizeProtocolSelection('SBXPC')
  check('selecting SBXPC suggests port 5005', hSbx.protocolFamily === 'SBXPC' && hSbx.port === 5005)
  const hFk = harmonizeProtocolSelection('FKATTEND')
  check('selecting FKATTEND suggests port 4238', hFk.protocolFamily === 'FKATTEND' && hFk.port === 4238)

  // type-change harmonization — the OLD bug reproduced NP+FKATTEND on port 5005
  const tNpFk = harmonizeTypeSelection('NOVIN_NP', 'FKATTEND')
  check('type → NP with prior FKATTEND keeps FKATTEND and suggests 4238 (old handler said 5005 — regression)', tNpFk.port === 4238 && tNpFk.protocolFamily === 'FKATTEND')
  const tNpSbx = harmonizeTypeSelection('NOVIN_NP', 'SBXPC')
  check('type → NP with prior SBXPC keeps the explicit choice and suggests 5005', tNpSbx.protocolFamily === 'SBXPC' && tNpSbx.port === 5005)
  const tNpInvalid = harmonizeTypeSelection('NOVIN_NP', 'whatever')
  check('type → NP with NO valid prior selection falls back to FKATTEND/4238', tNpInvalid.protocolFamily === 'FKATTEND' && tNpInvalid.port === 4238)
  const tZk = harmonizeTypeSelection('ZKTECO', 'FKATTEND')
  check('type → ZKTeco suggests port 4370 (protocolFamily kept as inert NP memory)', tZk.port === 4370 && tZk.protocolFamily === 'FKATTEND')
  // ZK → NP round-trip: the user's NP protocol choice survives
  const roundTrip = harmonizeTypeSelection('NOVIN_NP', harmonizeTypeSelection('ZKTECO', 'SBXPC').protocolFamily)
  check('NP → ZK → NP round-trip preserves an explicit SBXPC selection (port 5005 again)', roundTrip.protocolFamily === 'SBXPC' && roundTrip.port === 5005)

  // the module NEVER clamps or validates the port — after any selection the
  // user can type any port (the backend validator already proved 4238→4240 OK)
  check('harmonization results are plain suggestions — no port validation in the UI module', !('errors' in hFk) && typeof hFk.port === 'number' && typeof tZk.port === 'number')

  // payload sanitization — contradictory selections never reach the server
  check('ZK payload always carries the inert legacy family SBXPC (no ZK+FKATTEND contradiction)', serverPayloadFamilyFor('ZKTECO', 'FKATTEND') === 'SBXPC' && serverPayloadFamilyFor('ZKTECO', 'SBXPC') === 'SBXPC')
  check('NP payload carries the selected family (defensively normalized)', serverPayloadFamilyFor('NOVIN_NP', 'SBXPC') === 'SBXPC' && serverPayloadFamilyFor('NOVIN_NP', 'FKATTEND') === 'FKATTEND' && serverPayloadFamilyFor('NOVIN_NP', 'garbage') === 'FKATTEND')
  // only a type/protocol SELECTION may touch the port — the module exposes no
  // other port mutator, so unrelated form edits (name/IP/location/…) cannot
  // rewrite a custom port; the port-keep on edit is proven by v15 above.

  /* ---- secrets ---- */
  section('SECRETS — device row hygiene (v1.5.0: no stored bridge token at all)')
  const safe = sanitizeDeviceForApi({
    id: 'd1', name: 'X', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
    password: 4891489, bridgeToken: 'legacy-bridge-token', // v1.4.0 rows may still carry it — stripped defensively
    port: 4238, status: 'ONLINE', enabled: true, lastSyncCount: 0,
  })
  check('sanitizeDeviceForApi strips the password AND any legacy bridgeToken', safe.password === undefined && (safe as unknown as Record<string, unknown>).bridgeToken === undefined && safe.hasPassword === true)
  check('no secret value in the serialized row', !JSON.stringify(safe).includes('legacy-bridge-token') && !JSON.stringify(safe).includes('4891489'))

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
  // base = the v1.3.0 (Phase 1) schema — pinned to the Phase 1 commit, then
  // the real migration chain a user's DB would follow (fresh unique file —
  // nothing pre-existing is ever deleted)
  const { spawnSync } = await import('child_process')
  const { writeFileSync } = await import('fs')
  const sh = (cmd: string, args: string[], env: Record<string, string>) =>
    spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf8', timeout: 120_000 })
  const phase1Schema = sh('git', ['show', '3c254597:prisma/schema.prisma'], {})
  writeFileSync(`${TMP}/phase1-schema.prisma`, phase1Schema.stdout)
  sh('npx', ['prisma', 'db', 'push', '--schema', `${TMP}/phase1-schema.prisma`, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  const up14 = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.4.0 migration up (on a Phase-1 DB)', up14.status === 0 && /Device.rtPollSec: present/.test(up14.stdout))
  const up14b = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.4.0 migration idempotent', up14b.status === 0 && /already present/.test(up14b.stdout))
  const up15 = sh('node', ['scripts/migrate-v1.5.0-bridge-internal.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.5.0 migration up drops Device.bridgeUrl/bridgeToken', up15.status === 0 && /dropped Device\.(bridgeUrl|bridgeToken)/.test(up15.stdout))
  const up15b = sh('node', ['scripts/migrate-v1.5.0-bridge-internal.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.5.0 migration idempotent', up15b.status === 0 && /already absent/.test(up15b.stdout))
  // Phase 2 (S3) columns + dated default-shift history are part of the real
  // user chain since 2223ac75/068a90a1 — the parity check below compares
  // against the CURRENT schema, so the chain must include them (pre-existing
  // red at the handover HEAD otherwise: the suite pinned the chain at v1.5.0).
  const upS3 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.7.0 phase2 S3 migration up (isMission + payable/mission minutes)', upS3.status === 0 && /added .*(isMission|payableOvertimeMinutes|missionMinutes)|already present/.test(upS3.stdout))
  const upS3b = sh('node', ['scripts/migrate-v1.7.0-phase2-s3.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.7.0 S3 migration idempotent', upS3b.status === 0 && /already present/.test(upS3b.stdout))
  const upS3r2 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.7.0 phase2 S3r2 migration up (DefaultShiftHistory)', upS3r2.status === 0 && /created table DefaultShiftHistory|already present/.test(upS3r2.stdout))
  const upS3r2b = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.7.0 S3r2 migration idempotent', upS3r2b.status === 0 && /already present/.test(upS3r2b.stdout))
  // Phase 3 stage 2: DeviceSyncRun download-accounting columns join the chain
  const upP3 = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.8.0 phase3 migration up (announced/invalidTimestamps/abnormalEnd)', upP3.status === 0 && /recordsAnnounced = present/.test(upP3.stdout))
  const upP3b = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.8.0 phase3 migration idempotent', upP3b.status === 0 && /already present/.test(upP3b.stdout))
  // Phase 3 stage 6: tombstone + manual-override columns join the chain
  const upP4 = sh('node', ['scripts/migrate-v1.8.0-phase4.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.8.0 phase4 migration up (tombstone + manualOverride columns)', upP4.status === 0 && /deletedAt = present/.test(upP4.stdout) && /manualOverride = present/.test(upP4.stdout))
  const upP4b = sh('node', ['scripts/migrate-v1.8.0-phase4.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  check('v1.8.0 phase4 migration idempotent', upP4b.status === 0 && /already present/.test(upP4b.stdout))
  const parity = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: MAIN_DB })
  const parityOut = (parity.stdout || '') + (parity.stderr || '')
  check('prisma db push sees no drift (INSTALL.bat parity)', parity.status === 0 && /already in sync/i.test(parityOut))

  const p1 = await db.personnel.create({ data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 } })
  const device = await db.device.create({
    data: {
      name: 'دستگاه آزمایشی FK', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: SERIAL,
      connTimeoutMs: 3000, machineNumber: 1, sdkLicense: 1261,
    },
  })

  /* ================================================================
   * 4) FK E2E via the MOCK bridge
   * ================================================================ */
  const mock = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', FKBRIDGE_PORT: String(BRIDGE_PORT), FKBRIDGE_TOKEN: 'test-token', SERIAL, LOGS: '60', USERS: '1001,7777' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  const mockErr: string[] = []
  mock.stderr?.on('data', (d) => mockErr.push(String(d)))
  await new Promise((r) => setTimeout(r, 800))

  // v1.5.0: there are NO per-device bridge settings anymore. The bridge
  // endpoint comes from the supervisor; in this suite (and any mock/CI run)
  // it is the EXTERNAL override: FKBRIDGE_URL + FKBRIDGE_TOKEN → the mock
  // bridge. No child x86 process is spawned on non-Windows.
  const { resetBridgeSupervisorForTests } = await import('../src/lib/bridge-supervisor')
  const setBridgeOverride = (token: string) => {
    process.env.FKBRIDGE_URL = `http://127.0.0.1:${BRIDGE_PORT}`
    process.env.FKBRIDGE_TOKEN = token
  }
  setBridgeOverride('test-token')

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
  // wrong internal token (auth mismatch surfaces as a Persian service error)
  setBridgeOverride('wrong-token')
  const badTok = await fkTestConnection(device)
  check('wrong internal bridge auth → clear Persian error (no token jargon)', !badTok.ok && (badTok.error ?? '').includes('سرویس ارتباط دستگاه'))
  setBridgeOverride('test-token')
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
  check('v1.5.0 FK623: records carry NO workCode — never fabricated (empty + raw payload omits the key)', rawRows.every((r) => (r.workCode ?? '') === '') && rawRows.every((r) => !(r.rawPayload ?? '').includes('workCode')))
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
  // deterministic busy case: hold the per-device DB lock, then attempt a sync
  const { withDeviceSyncLock } = await import('../src/lib/sync-mutex')
  const hold = withDeviceSyncLock(device.id, device.name, () => new Promise((r) => setTimeout(() => r({ held: true }), 600)))
  let busyRejected = false
  try {
    await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  } catch (err) {
    busyRejected = err instanceof SyncBusyError
  }
  check('a sync attempt while the device lock is held is rejected with SyncBusyError', busyRejected)
  await hold
  // two genuinely concurrent syncs: at least one runs; every rejection is a
  // SyncBusyError (serialized — they never interleave; row count proves it)
  const rowsBeforeConcurrent = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })
  const [a, b] = await Promise.allSettled([
    importFkAttendLogs(device.id, { mode: 'MOCK', full: true }),
    importFkAttendLogs(device.id, { mode: 'MOCK', full: true }),
  ])
  const okCount = [a, b].filter((r) => r.status === 'fulfilled').length
  const busyCount = [a, b].filter((r) => r.status === 'rejected' && r.reason instanceof SyncBusyError).length
  check(
    'two concurrent syncs serialize: at least one runs, any rejection is SyncBusyError',
    okCount >= 1 && okCount + busyCount === 2,
    `ok=${okCount} busy=${busyCount}`,
  )
  check(
    'concurrent syncs add no duplicate rows (exclusion + unique key)',
    (await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })) === rowsBeforeConcurrent,
  )

  section('SERIAL MISMATCH GUARD')
  const devForeign = await db.device.create({
    data: {
      name: 'دستگاه سریال غلط', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: 'OTHER-SERIAL-999',
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
  check('bridge down → clear Persian error (not a crash)', !down.ok && /سرویس ارتباط دستگاه|ارتباط با/.test(down.error ?? ''))
  const errRun = await db.deviceLog.findMany({ where: { deviceId: device.id, level: 'ERROR' } })
  check('device logs carry no secrets after errors', errRun.every((l) => !/test-token|4891489/.test(l.message)))

  /* ---- restart bridge → recovery ---- */
  const mock2 = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', FKBRIDGE_PORT: String(BRIDGE_PORT), FKBRIDGE_TOKEN: 'test-token', SERIAL, LOGS: '60', USERS: '1001,7777' },
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

  // v1.5.0: leave no bridge override behind for other suites in this process
  delete process.env.FKBRIDGE_URL
  delete process.env.FKBRIDGE_TOKEN
  resetBridgeSupervisorForTests()

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
  rmSync(MAIN_DB_FILE, { force: true }) // own, validated, auto-generated file only
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('SUITE CRASHED:', err)
  const { db } = await import('../src/lib/db').catch(() => ({ db: null as never }))
  await db?.$disconnect?.()
  process.exit(1)
})
