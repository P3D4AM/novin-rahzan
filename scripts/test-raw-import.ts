/**
 * Phase 1 (v1.3.0) test suite — MOCK ONLY.
 * =====================================================================
 * Everything in this file runs against MOCK data sources (in-memory
 * providers or scripts/mock-sbxpc-device.js over a local TCP socket).
 * NO real hardware (NP756+) is involved; hardware validation happens on the
 * Windows machine with the FKAttend bridge (Phase 2).
 *
 * Covered:
 *   [migration]  incremental up on a pre-1.3.0 database, idempotency,
 *                prisma db-push parity, rollback (down) + re-up
 *   [ingest]     basic import, unmatched retention, fingerprint dedupe,
 *                simultaneous identical punches (dupIndex), same-second
 *                distinct punches, transactional failure (nothing stored,
 *                bookmark frozen), legacy PunchRecord projection
 *   [orchestrator] sync history rows, retry with backoff, serial-mismatch
 *                guard (import stops), password redaction everywhere
 *   [rematch]    unmatched punches become MATCHED when personnel appears
 *   [e2e mock]   full importNpDeviceLogs() against mock-sbxpc-device.js
 *
 * Run (DATABASE_URL is REQUIRED — the suite refuses to touch db/custom.db):
 *   DATABASE_URL=file:/tmp/nr-test-phase1/rawtest.db npx tsx scripts/test-raw-import.ts
 */
import { spawn, spawnSync } from 'child_process'
import { rmSync, mkdirSync, writeFileSync } from 'fs'
import path from 'path'
import { db } from '../src/lib/db'
import { ingestRawPunches, rematchRawPunches, rawFingerprint } from '../src/lib/raw-import'
import { runRawSync, SerialMismatchError } from '../src/lib/raw-sync'
import type { RawPunchProvider } from '../src/lib/raw-sync'
import { withRetry } from '../src/lib/retry'
import { redactDeviceSecrets, sanitizeDeviceForApi, sanitizeResultForApi } from '../src/lib/device-secrets'
import { importNpDeviceLogs } from '../src/lib/np-sync'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-test-phase1'
const MAIN_DB = process.env.DATABASE_URL || ''
const MAIN_DB_FILE = MAIN_DB.replace(/^file:/, '')

const OLD_SCHEMA = `${TMP}/old-schema.prisma`

let passed = 0
let failed = 0
const failures: string[] = []

function section(t: string) {
  console.log(`\n\x1b[1m━━━ ${t}\x1b[0m`)
}
function check(name: string, cond: boolean, detail?: string) {
  if (cond) {
    passed++
    console.log(`  \x1b[32m✔\x1b[0m ${name}`)
  } else {
    failed++
    failures.push(name + (detail ? ` — ${detail}` : ''))
    console.log(`  \x1b[31m✘\x1b[0m ${name}${detail ? `  (${detail})` : ''}`)
  }
}
function sh(cmd: string, args: string[], env: Record<string, string>) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf8', timeout: 120_000 })
}

/** a provider that serves a fixed record list (MOCK) */
class ListProvider implements RawPunchProvider {
  readonly protocol = 'MOCK' as const
  constructor(private records: Parameters<typeof ingestRawPunches>[0], private serial?: string) {}
  async readAll() {
    return { records: this.records, deviceSerial: this.serial, deviceTime: new Date().toISOString() }
  }
}

/** a provider that fails N times before succeeding (MOCK — retry semantics) */
class FlakyProvider implements RawPunchProvider {
  readonly protocol = 'MOCK' as const
  failTimes: number
  calls = 0
  constructor(failTimes: number, private serial = '7561202505176') {
    this.failTimes = failTimes
  }
  async readAll() {
    this.calls++
    if (this.calls <= this.failTimes) throw new Error('مهلت پاسخ دستگاه به پایان رسید (ETIMEDOUT)')
    return { records: [], deviceSerial: this.serial, deviceTime: new Date().toISOString() }
  }
}

const T0 = Date.UTC(2026, 10, 1, 6, 30, 0) // 2026-11-01 06:30 UTC
const rec = (enroll: number | string, msAgo: number, extra: Partial<Parameters<typeof ingestRawPunches>[0][number]> = {}) => ({
  enrollNumber: enroll,
  punchTime: new Date(T0 - msAgo),
  inOutModeRaw: 1,
  verifyModeRaw: 1,
  workCode: '',
  rawPayload: JSON.stringify({ mock: true, enroll }),
  ...extra,
})

async function main() {
  if (!MAIN_DB) throw new Error('DATABASE_URL must be set to a TEMP database, e.g. file:/tmp/nr-test-phase1/rawtest.db')
  if (/custom\.db/i.test(MAIN_DB)) throw new Error('refusing to run against db/custom.db — point DATABASE_URL at a temp database')
  mkdirSync(TMP, { recursive: true })

  /* ================================================================
   * 1) MIGRATION — on a database that has the PRE-1.3.0 schema
   * ================================================================ */
  section('MIGRATION — incremental, idempotent, reversible')
  rmSync(`${TMP}/mig.db`, { force: true })
  rmSync(`${TMP}/mig.db-journal`, { force: true })
  const old = spawnSync('git', ['show', 'HEAD:prisma/schema.prisma'], { cwd: ROOT, encoding: 'utf8' })
  writeFileSync(OLD_SCHEMA, old.stdout)
  const pushOld = sh('npx', ['prisma', 'db', 'push', '--schema', OLD_SCHEMA, '--skip-generate'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('pre-1.3.0 database created (db push, old schema)', pushOld.status === 0, (pushOld.stderr || '').slice(-200))

  const up1 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('migration up applied', up1.status === 0 && /RawPunch table: present/.test(up1.stdout), (up1.stderr || '').slice(-200))
  const up2 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('migration up is idempotent (2nd run ok)', up2.status === 0 && /RawPunch table: present/.test(up2.stdout))
  // v1.4.0 upgrade on top — the parity check runs against the CURRENT schema
  const up14 = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('v1.4.0 upgrade applies on the v1.3.0 database', up14.status === 0 && /Device.rtPollSec: present/.test(up14.stdout))

  const parity = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: `file:${TMP}/mig.db` })
  const parityOut = (parity.stdout || '') + (parity.stderr || '')
  check(
    'prisma db push sees NO drift after migration (parity with INSTALL.bat path)',
    parity.status === 0 && /already in sync/i.test(parityOut),
    parityOut.slice(-200).replace(/\n/g, ' '),
  )

  sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'down', '--confirm'], { DATABASE_URL: `file:${TMP}/mig.db` })
  const down1 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'down', '--confirm'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('rollback (down --confirm) removes the new objects', down1.status === 0 && /RawPunch table: MISSING/.test(down1.stdout))
  const downGuard = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'down'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('rollback requires --confirm', downGuard.status === 2)
  const up3 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: `file:${TMP}/mig.db` })
  check('re-up after rollback works', up3.status === 0 && /RawPunch table: present/.test(up3.stdout))

  /* ================================================================
   * 2) prepare the MAIN test database (old schema + migration)
   * ================================================================ */
  rmSync(MAIN_DB_FILE, { force: true })
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  sh('npx', ['prisma', 'db', 'push', '--schema', OLD_SCHEMA, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  const mainUp = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  if (mainUp.status !== 0) throw new Error('main db migration failed: ' + mainUp.stderr)
  // v1.4.0 upgrade on top (as a real v1.3.0 installation would do)
  const mainUp14 = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  if (mainUp14.status !== 0) throw new Error('main db v1.4.0 migration failed: ' + mainUp14.stderr)

  // seed: two personnel + one device
  const p1 = await db.personnel.create({
    data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 },
  })
  const p2 = await db.personnel.create({
    data: { personnelCode: '2002', firstName: 'رضا', lastName: 'کریمی', pin: '2002' },
  })
  const device = await db.device.create({
    data: { name: 'دستگاه آزمایشی', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5111, serialNumber: '7561202505176' },
  })
  const SERIAL = '7561202505176'

  /* ================================================================
   * 3) INGEST — storage, dedupe, unmatched, simultaneous punches
   * ================================================================ */
  section('INGEST — basic import, unmatched retention, projection')
  const batch1 = [
    rec(1001, 3600_000), // matched via deviceUserId
    rec('2002', 7200_000), // matched via numeric personnelCode
    rec(9999, 1800_000), // UNMATCHED — must be stored, never dropped
    { ...rec(1001, 60_000), verifyModeRaw: 4 },
  ]
  const r1 = await ingestRawPunches(batch1, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('all 4 valid records counted', r1.seen === 4 && r1.invalid === 0)
  check('4 new rows stored', r1.newCount === 4, `newCount=${r1.newCount}`)
  check('3 matched / 1 unmatched', r1.matched === 3 && r1.unmatched === 1, `matched=${r1.matched} unmatched=${r1.unmatched}`)
  check('legacy PunchRecord projection = 3 (matched only)', r1.punchRecordsCreated === 3, `created=${r1.punchRecordsCreated}`)
  const raw1 = await db.rawPunch.findMany({ where: { deviceSerial: SERIAL } })
  check('unmatched row kept with UNMATCHED status', raw1.some((r) => r.enrollNumber === '9999' && r.matchStatus === 'UNMATCHED' && r.personnelId === null))
  check('matched rows carry personnelId', raw1.find((r) => r.enrollNumber === '1001')?.personnelId === p1.id)
  check('enroll 2002 matched via numeric personnelCode', raw1.find((r) => r.enrollNumber === '2002')?.personnelId === p2.id)
  check('direction is UNKNOWN for every row (never guessed)', raw1.every((r) => r.direction === 'UNKNOWN'))
  check('raw in/out + verify values preserved', raw1.every((r) => r.inOutModeRaw === 1 && [1, 4].includes(r.verifyModeRaw)))
  check('bookmark advanced to newest punch', (await db.device.findUnique({ where: { id: device.id } }))?.rawCursorAt?.getTime() === T0 - 60_000)
  const punch1 = await db.punchRecord.findMany({ where: { source: 'DEVICE' } })
  check('projection direction = AUTO (legacy undetermined vocabulary)', punch1.every((p) => p.direction === 'AUTO'))
  check('projection rawPayload present', punch1.every((p) => p.rawPayload != null))

  section('INGEST — dedupe (fingerprint identity)')
  const r2 = await ingestRawPunches(batch1, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('re-ingest of the same batch → 0 new, 4 duplicates', r2.newCount === 0 && r2.duplicate === 4, `new=${r2.newCount} dup=${r2.duplicate}`)
  check('no extra RawPunch rows after re-ingest', (await db.rawPunch.count({ where: { deviceSerial: SERIAL } })) === 4)
  check('no extra PunchRecord rows after re-ingest', (await db.punchRecord.count({ where: { source: 'DEVICE' } })) === 3)
  check('bookmark unchanged (same max)', (await db.device.findUnique({ where: { id: device.id } }))?.rawCursorAt?.getTime() === T0 - 60_000)

  section('INGEST — simultaneous identical punches (dupIndex)')
  const simultaneous = [rec(1001, 10_000), rec(1001, 10_000), rec(1001, 10_000)]
  const r3 = await ingestRawPunches(simultaneous, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('3 identical same-second punches → 3 rows (dupIndex 0,1,2)', r3.newCount === 3, `new=${r3.newCount}`)
  const dupRows = await db.rawPunch.findMany({
    where: { deviceSerial: SERIAL, punchTime: new Date(T0 - 10_000) },
    orderBy: { dupIndex: 'asc' },
  })
  check('dupIndex sequence is 0,1,2', dupRows.map((r) => r.dupIndex).join(',') === '0,1,2', dupRows.map((r) => r.dupIndex).join(','))
  check('projection keeps only ONE legacy row for the same person+second', (await db.punchRecord.count({ where: { personId: p1.id, timestamp: new Date(T0 - 10_000), source: 'DEVICE' } })) === 1)
  const r3b = await ingestRawPunches(simultaneous, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('re-ingest of identical trio → 0 new (all duplicates)', r3b.newCount === 0 && r3b.duplicate === 3)

  section('INGEST — same second, different raw values stay distinct')
  const distinct = [
    rec(1001, 5000, { verifyModeRaw: 1 }),
    rec(1001, 5000, { verifyModeRaw: 2 }),
  ]
  const r4 = await ingestRawPunches(distinct, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('two same-second punches with different verify → 2 rows', r4.newCount === 2)
  const fpA = rawFingerprint(rec(1001, 5000, { verifyModeRaw: 1 }), SERIAL, 'MOCK')
  const fpB = rawFingerprint(rec(1001, 5000, { verifyModeRaw: 2 }), SERIAL, 'MOCK')
  check('fingerprints differ when raw values differ', fpA !== fpB)

  section('INGEST — atomic failure: nothing stored, bookmark frozen')
  const cursorBefore = (await db.device.findUnique({ where: { id: device.id } }))?.rawCursorAt?.getTime()
  const rowsBefore = await db.rawPunch.count({ where: { deviceSerial: SERIAL } })
  let ingestThrew = false
  try {
    // bogus deviceId → FK violation inside the transaction → full rollback
    await ingestRawPunches([rec(1001, 1000)], { deviceId: 'nonexistent-device-id', deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  } catch {
    ingestThrew = true
  }
  check('transactional failure propagates', ingestThrew)
  check('no rows stored by the failed run', (await db.rawPunch.count({ where: { deviceSerial: SERIAL } })) === rowsBefore)
  check('bookmark did NOT advance on failure', (await db.device.findUnique({ where: { id: device.id } }))?.rawCursorAt?.getTime() === cursorBefore)

  /* ================================================================
   * 4) ORCHESTRATOR — sync history, retry, serial guard, secrets
   * ================================================================ */
  section('ORCHESTRATOR — sync history + retry')
  const dev2 = await db.device.create({
    data: { name: 'دستگاه دوم', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5112, serialNumber: 'SN-222' },
  })
  const flaky = new FlakyProvider(2, 'SN-222')
  const s1 = await runRawSync({ deviceId: dev2.id, provider: flaky, mode: 'MOCK', full: true, retries: 2 })
  check('flaky provider succeeds after 2 retries', s1.status === 'SUCCESS' && s1.retries === 2, `retries=${s1.retries}`)
  const run1 = await db.deviceSyncRun.findFirst({ where: { deviceId: dev2.id }, orderBy: { startedAt: 'desc' } })
  check('DeviceSyncRun row exists with mode=MOCK and retries=2', run1?.mode === 'MOCK' && run1?.retries === 2 && run1?.status === 'SUCCESS')

  const alwaysFail = new FlakyProvider(99, 'SN-222')
  let syncThrew = false
  try {
    await runRawSync({ deviceId: dev2.id, provider: alwaysFail, mode: 'MOCK', retries: 1 })
  } catch {
    syncThrew = true
  }
  check('permanently failing sync throws after retries', syncThrew)
  const run2 = await db.deviceSyncRun.findFirst({ where: { deviceId: dev2.id }, orderBy: { startedAt: 'desc' } })
  check('failed run recorded as FAILED with sanitized error', run2?.status === 'FAILED' && !/password|رمز\s*شبکه\s*:\s*\d/i.test(run2?.error ?? ''))
  check('failed run does not advance the bookmark', run2?.bookmarkAt == null)
  check('device status OFFLINE after failure', (await db.device.findUnique({ where: { id: dev2.id } }))?.status === 'OFFLINE')

  section('ORCHESTRATOR — serial mismatch guard')
  const dev3 = await db.device.create({
    data: { name: 'دستگاه سوم', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5113, serialNumber: 'SN-333' },
  })
  const foreign = new ListProvider([rec(1001, 1000)], 'SN-FOREIGN')
  let mismatchThrew = false
  try {
    await runRawSync({ deviceId: dev3.id, provider: foreign, mode: 'MOCK', full: true })
  } catch (err) {
    mismatchThrew = err instanceof SerialMismatchError
  }
  check('serial mismatch throws SerialMismatchError', mismatchThrew)
  check('NO rows stored on serial mismatch', (await db.rawPunch.count({ where: { deviceSerial: 'SN-FOREIGN' } })) === 0)
  const run3 = await db.deviceSyncRun.findFirst({ where: { deviceId: dev3.id }, orderBy: { startedAt: 'desc' } })
  check('mismatch run recorded FAILED', run3?.status === 'FAILED')
  const logs3 = await db.deviceLog.findMany({ where: { deviceId: dev3.id, level: 'ERROR' } })
  check('mismatch produces an ERROR device log', logs3.length > 0)

  section('SECURITY — password redaction')
  check(
    'historical log message with password is redacted',
    !/4891489/.test(redactDeviceSecrets('اتصال مستقیم برقرار شد — سریال: X · رمز شبکه: 4891489 · ترددها: 2944')),
  )
  check(
    'redaction keeps the label but not the value',
    /رمز شبکه: ●●●●/.test(redactDeviceSecrets('رمز شبکه: 4891489')),
  )
  const safeDev = sanitizeDeviceForApi({
    id: 'd1', name: 'X', password: 4891489, serialNumber: 'S', port: 5005, enabled: true, status: 'ONLINE', type: 'NOVIN_NP', protocol: 'TCP', location: null, ipAddress: null, lastLogAt: null, lastSyncAt: null, lastSyncCount: 0, syncIntervalMin: 0, createdAt: new Date(), rawCursorAt: null,
  })
  check('sanitizeDeviceForApi strips the password, exposes hasPassword', safeDev.password === undefined && safeDev.hasPassword === true && !JSON.stringify(safeDev).includes('4891489'))
  const safeRes = sanitizeResultForApi({ ok: true, serialNumber: 'S', password: 123456, note: '' })
  check('sanitizeResultForApi replaces password with passwordFound', safeRes.password === undefined && safeRes.passwordFound === true)
  const safeNone = sanitizeResultForApi({ ok: true, serialNumber: 'S' })
  check('passwordFound=false when no password', safeNone.passwordFound === false)

  section('RETRY — withRetry semantics')
  let counter = 0
  const outcome = await withRetry(
    async () => {
      counter++
      if (counter < 3) throw new Error('x')
      return 'ok'
    },
    { retries: 3, baseDelayMs: 10 },
  )
  check('withRetry returns the value after retries', outcome.value === 'ok' && outcome.retries === 2 && counter === 3)
  let never = false
  try {
    await withRetry(async () => { throw new Error('y') }, { retries: 1, baseDelayMs: 1 })
  } catch {
    never = true
  }
  check('withRetry rethrows after exhausting attempts', never)

  /* ================================================================
   * 5) REMATCH — unmatched punches become usable
   * ================================================================ */
  section('REMATCH — unmatched → matched when personnel appears')
  const p3 = await db.personnel.create({
    data: { personnelCode: '9999', firstName: 'مهدی', lastName: 'نوری', pin: '9999' },
  })
  const rem = await rematchRawPunches()
  const remRow = await db.rawPunch.findFirst({ where: { enrollNumber: '9999' } })
  check('unmatched punch rematched to the new personnel', remRow?.matchStatus === 'MATCHED' && remRow?.personnelId === p3.id)
  check('rematch created the legacy projection', rem.punchRecordsCreated >= 1)
  const rem2 = await rematchRawPunches()
  check('rematch is idempotent (second call matches nothing)', rem2.matched === 0)
  check('rematch does not duplicate the projection', (await db.punchRecord.count({ where: { personId: p3.id, source: 'DEVICE' } })) === 1)

  /* ================================================================
   * 6) E2E (MOCK DEVICE) — importNpDeviceLogs over TCP
   * ================================================================ */
  section('E2E MOCK DEVICE — importNpDeviceLogs vs mock-sbxpc-device.js')
  const MOCK_PORT = 5111
  const mock = spawn('node', ['scripts/mock-sbxpc-device.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(MOCK_PORT), SERIAL, LOGS: '60', USERS: '1001,7777' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  let mockReady = false
  const mockOut: string[] = []
  mock.stdout?.on('data', (d) => { mockOut.push(String(d)); if (String(d).includes('listening')) mockReady = true })
  mock.stderr?.on('data', (d) => mockOut.push(String(d)))
  await new Promise((r) => setTimeout(r, 1500))
  const mockDevice = await db.device.update({
    where: { id: device.id },
    data: { ipAddress: '127.0.0.1', port: MOCK_PORT },
  })
  const e2e1 = await importNpDeviceLogs(mockDevice.id, { daysBack: 30, mode: 'MOCK' })
  check('mock pull sync imports all 60 logs', e2e1.total === 60 && e2e1.imported === 60, `total=${e2e1.total} imported=${e2e1.imported}`)
  check('mock sync created 60 raw rows', e2e1.created === 60, `created=${e2e1.created}`)
  check('mock sync reports unmatched users (7777)', e2e1.unmatched === 30, `unmatched=${e2e1.unmatched}`)
  check('mock sync reports matched users (1001)', e2e1.created - e2e1.unmatched === 30)
  const e2e2 = await importNpDeviceLogs(mockDevice.id, { daysBack: 30, mode: 'MOCK' })
  check('second mock sync → everything is a duplicate (0 new)', e2e2.created === 0 && e2e2.skipped === 60, `created=${e2e2.created} skipped=${e2e2.skipped}`)
  const runsMock = await db.deviceSyncRun.findMany({ where: { deviceId: mockDevice.id } })
  check('two DeviceSyncRun rows recorded with mode=MOCK', runsMock.length === 2 && runsMock.every((r) => r.mode === 'MOCK'))
  const mockRaw = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'SBXPC' } })
  check('60 SBXPC raw rows with serial identity', mockRaw === 60, `count=${mockRaw}`)
  const successLog = await db.deviceLog.findFirst({ where: { deviceId: mockDevice.id, level: 'SUCCESS' }, orderBy: { createdAt: 'desc' } })
  check('device log mentions Mock mode marker', (successLog?.message ?? '').includes('[آزمایشی — Mock]'))
  check('device log carries no password', !/رمز\s*شبکه\s*[:：]?\s*\d{3,}/.test(successLog?.message ?? ''))
  check('serial mismatch flag false (serials agree)', e2e1.serialMismatch === false)
  mock.kill('SIGTERM')

  /* ================================================================
   * SUMMARY — mock vs hardware clearly separated
   * ================================================================ */
  console.log(`\n\x1b[1m━━━ SUMMARY\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  HARDWARE tests : 0 (none run — requires the NP756+ on the Windows machine, Phase 2)`)
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
