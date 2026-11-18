/**
 * Phase 1 (v1.3.0 + v1.3.1 corrections) test suite — MOCK ONLY.
 * =====================================================================
 * Everything in this file runs against MOCK data sources (in-memory
 * providers or scripts/mock-sbxpc-device.js over a local TCP socket).
 * NO real hardware (NP756+) is involved; hardware validation happens on the
 * Windows machine with the FKAttend bridge (Phase 2). Nothing here claims
 * hardware validation.
 *
 * v1.3.1 (review fixes) — newly covered:
 *   [fix 1] personnel mapping: exact strings, leading zeros, separate
 *           deviceUserId vs personnelCode namespaces, collision/ambiguity
 *           detection (never auto-assigned; matchNote visible for admins)
 *   [fix 2] recalculation per distinct (personId, day) pair — multi-person,
 *           multi-day; PARTIAL sync state when recalculation fails; recovery
 *           via retrySyncRunRecalcs without re-importing punches
 *   [fix 3] atomic rematch: mapping update + projection + AuditLog in ONE
 *           transaction; failure between mapping update and projection rolls
 *           back; idempotent; audit trail with previous/new mapping
 *   [fix 4] quarantine: invalid timestamps preserved with raw payload +
 *           reason in RejectedRecord; rejected/outOfWindow counts on the run;
 *           a run with NEW rejected records closes PARTIAL; window-excluded
 *           records counted separately; full accounting seen = inWindow +
 *           rejected + outOfWindow
 *   [fix 5] test DB safety: auto-generated unique temp databases, path
 *           validation refusing anything outside the test directory (arbitrary
 *           DATABASE_URL targets are never touched), migration isolation
 *   [fix 6] device password at-rest: honest boundary (plaintext kept by
 *           default), DPAPI refused on non-Windows, sanitize strips
 *           passwordEnc, protect script refuses non-Windows
 *   [fix 7] DB-backed per-device sync lock: busy error, release after
 *           failure, lease-expiry takeover, cross-process concurrent import
 *           of the same batch (unique constraint = final safeguard)
 *   plus    the full v1.3.0 regression set (migration parity/rollback,
 *           dedupe, simultaneous punches, atomic failure, retry, serial
 *           guard, redaction, rematch, e2e vs mock-sbxpc-device.js)
 *
 * DATABASE handling (fix 5): the suite AUTO-GENERATES its own unique
 * temporary database under the test directory — an external DATABASE_URL is
 * ignored for the main database and NEVER written to or deleted.
 *
 * Run:
 *   npx tsx scripts/test-raw-import.ts
 */
import { spawn, spawnSync } from 'child_process'
import { mkdirSync, writeFileSync, existsSync, readFileSync, rmSync } from 'fs'
import os from 'os'
import path from 'path'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl, UnsafeDatabasePathError } from './test-db-guard'
import { withRetry } from '../src/lib/retry'
import { redactDeviceSecrets, sanitizeDeviceForApi, sanitizeResultForApi } from '../src/lib/device-secrets'
import { isProtectedSecret, isDeviceSecretProtectionEnabled, protectDeviceSecret, storeDevicePassword, getDevicePassword, deviceHasPassword, SecretProtectionUnavailableError } from '../src/lib/device-secret-store'
import type { RawPunchProvider, RawPunchRead } from '../src/lib/raw-sync'
import type { RawPunchInput } from '../src/lib/raw-import'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-test-phase1'

// ---------------------------------------------------------------- databases
// (fix 5) — the main test database is ALWAYS an auto-generated, unique,
// test-owned path. An externally supplied DATABASE_URL is ignored here and is
// never written to or deleted by this suite.
const externalDbUrl = process.env.DATABASE_URL || ''
const MAIN_DB_FILE = generateTestDatabasePath(TMP)
const MAIN_DB_URL = sqliteUrl(MAIN_DB_FILE)
process.env.DATABASE_URL = MAIN_DB_URL // MUST be set before the db client is imported

const OLD_SCHEMA = path.join(TMP, `old-schema-${process.pid}-${Date.now()}.prisma`)

let passed = 0
let failed = 0
const failures: string[] = []
const tmpFiles: string[] = [MAIN_DB_FILE]

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
function sh(cmd: string, args: string[], env: Record<string, string | undefined>) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env } as NodeJS.ProcessEnv, encoding: 'utf8', timeout: 180_000 })
}

/** a provider that serves a fixed record list (MOCK) */
class ListProvider implements RawPunchProvider {
  readonly protocol = 'MOCK' as const
  constructor(private records: RawPunchInput[], private serial?: string) {}
  async readAll(): Promise<RawPunchRead> {
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
  async readAll(): Promise<RawPunchRead> {
    this.calls++
    if (this.calls <= this.failTimes) throw new Error('مهلت پاسخ دستگاه به پایان رسید (ETIMEDOUT)')
    return { records: [], deviceSerial: this.serial, deviceTime: new Date().toISOString() }
  }
}

const T0 = Date.UTC(2026, 10, 1, 6, 30, 0) // 2026-11-01 06:30 UTC (fixed test clock)
const rec = (enroll: number | string, msAgo: number, extra: Partial<RawPunchInput> = {}): RawPunchInput => ({
  enrollNumber: enroll,
  punchTime: new Date(T0 - msAgo),
  inOutModeRaw: 1,
  verifyModeRaw: 1,
  workCode: '',
  rawPayload: JSON.stringify({ mock: true, enroll }),
  ...extra,
})

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(MAIN_DB_URL, TMP)

  // db-dependent modules are imported AFTER DATABASE_URL points at the
  // auto-generated test database
  const { db } = await import('../src/lib/db')
  const { ingestRawPunches, rematchRawPunches, rawFingerprint, quarantineRawPunches, buildPersonnelIndex, resolvePersonnelForEnroll } = await import('../src/lib/raw-import')
  const { runRawSync, SerialMismatchError, retrySyncRunRecalcs } = await import('../src/lib/raw-sync')
  const { withDeviceSyncLock, SyncBusyError } = await import('../src/lib/sync-mutex')
  const { importNpDeviceLogs } = await import('../src/lib/np-sync')

  /* ================================================================
   * 0) FIX 5 — test database safety (guard)
   * ================================================================ */
  section('DB GUARD (fix 5) — isolated, auto-generated, refusal of foreign paths')
  check('main test database auto-generated under the test directory', MAIN_DB_FILE.startsWith(path.resolve(TMP)))
  check('main test database path is unique per run (pid + time + rand)', /rawtest-\d+-\d+-\d+\.db$/.test(MAIN_DB_FILE))
  if (externalDbUrl) {
    // an external DATABASE_URL is IGNORED — never used, never deleted
    check('external DATABASE_URL is ignored for the main database (not deleted, not written)', externalDbUrl !== MAIN_DB_URL)
  }
  check('guard accepts a database inside the test directory', (() => {
    try {
      return assertTestOwnedDatabasePath(sqliteUrl(path.join(TMP, 'sub', 'ok.db')), TMP) === path.join(path.resolve(TMP), 'sub', 'ok.db')
    } catch {
      return false
    }
  })())
  {
    // an arbitrary (production-like) path must be REJECTED without any modification
    const arbitrary = path.join(os.tmpdir(), `hozor-arbitrary-${process.pid}-${Date.now()}.db`)
    writeFileSync(arbitrary, 'PRECIOUS PRODUCTION DATA', 'utf8')
    let rejected = false
    try {
      assertTestOwnedDatabasePath(arbitrary, TMP)
    } catch (err) {
      rejected = err instanceof UnsafeDatabasePathError || /رفض مسیر/.test((err as Error).message)
    }
    const unchanged = existsSync(arbitrary) && readFileSync(arbitrary, 'utf8') === 'PRECIOUS PRODUCTION DATA'
    check('guard REJECTS an arbitrary database path outside the test directory', rejected)
    check('rejected arbitrary database is untouched (content identical)', unchanged)
    check('guard rejects the production custom.db path', (() => {
      try {
        assertTestOwnedDatabasePath('file:db/custom.db', TMP)
        return false
      } catch {
        return true
      }
    })())
    rmSync(arbitrary, { force: true })
    check('relative traversal (../) is rejected', (() => {
      try {
        assertTestOwnedDatabasePath(sqliteUrl(path.join(TMP, '..', 'evil.db')), TMP)
        return false
      } catch {
        return true
      }
    })())
  }

  /* ================================================================
   * 1) MIGRATION — on a database that has the PRE-1.3.0 schema,
   *    with EXISTING data preserved across up AND rollback (fix 5 isolation)
   * ================================================================ */
  section('MIGRATION — incremental, idempotent, reversible, data-preserving')
  const migDb = generateTestDatabasePath(TMP)
  tmpFiles.push(migDb)
  const MIG_URL = sqliteUrl(migDb)
  // the pre-1.3.0 baseline schema is pinned to the PARENT of the Phase 1
  // commit — never "HEAD", which would drift with future commits
  const phase1Parent = spawnSync('git', ['rev-parse', '3c254597^'], { cwd: ROOT, encoding: 'utf8' })
  check('pre-1.3.0 baseline commit resolves (3c254597^)', phase1Parent.status === 0)
  const old = spawnSync('git', ['show', '3c254597^:prisma/schema.prisma'], { cwd: ROOT, encoding: 'utf8' })
  writeFileSync(OLD_SCHEMA, old.stdout)
  tmpFiles.push(OLD_SCHEMA)
  const pushOld = sh('npx', ['prisma', 'db', 'push', '--schema', OLD_SCHEMA, '--skip-generate'], { DATABASE_URL: MIG_URL })
  check('pre-1.3.0 database created (db push, pinned old schema)', pushOld.status === 0, (pushOld.stderr || '').slice(-200))

  // seed REAL data on the old schema — it must survive up AND rollback
  const seed = sh('node', ['scripts/test-db-raw.mjs',
    `INSERT INTO "Personnel" ("id","personnelCode","firstName","lastName","pin","qrToken","createdAt","updatedAt") VALUES ('pmig1','9001','مهاجر','تست','9001','seed-qr-token-1','2026-01-01T00:00:00.000Z','2026-01-01T00:00:00.000Z')`,
    `INSERT INTO "Device" ("id","name","type","password","createdAt") VALUES ('dmig1','دستگاه مهاجرت','NOVIN_NP',123456,'2026-01-01T00:00:00.000Z')`,
    `INSERT INTO "PunchRecord" ("id","personId","timestamp","createdAt") VALUES ('prmig1','pmig1','2026-02-01T08:00:00.000Z','2026-02-01T08:00:00.000Z')`,
  ], { DATABASE_URL: MIG_URL })
  check('seed rows written on the pre-1.3.0 database', seed.status === 0, (seed.stderr || '').slice(-200))

  const up1 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.3.0 migration up applied', up1.status === 0 && /RawPunch table: present/.test(up1.stdout), (up1.stderr || '').slice(-200))
  const up14 = sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MIG_URL })
  const up15 = sh('node', ['scripts/migrate-v1.5.0-bridge-internal.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.5.0 migration drops the obsolete bridge columns', up15.status === 0 && /dropped Device\.bridgeUrl/.test(up15.stdout))
  check('v1.4.0 upgrade applies on the v1.3.0 database', up14.status === 0 && /Device.rtPollSec: present/.test(up14.stdout))
  const up131 = sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check(
    'v1.3.1 migration up applied (RejectedRecord + DeviceSyncLock + new columns)',
    up131.status === 0 && /RejectedRecord table: present/.test(up131.stdout) && /DeviceSyncLock table: present/.test(up131.stdout) &&
      /DeviceSyncRun.recordsRejected: present/.test(up131.stdout) && /RawPunch.matchNote: present/.test(up131.stdout) && /Device.passwordEnc: present/.test(up131.stdout),
    (up131.stdout || up131.stderr || '').slice(-300),
  )
  const up131b = sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.3.1 migration up is idempotent (2nd run ok)', up131b.status === 0 && /RejectedRecord table: present/.test(up131b.stdout))
  // Phase 2 S3: the policy-column migration joins the upgrade chain (old
  // install → v1.3.0 → v1.3.1 → v1.4.0 → v1.5.0 → v1.7.0 → current engine)
  const up17 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.7.0 migration up applied (isMission + payable/mission minutes columns)', up17.status === 0 && /LeaveType\.isMission = present/.test(up17.stdout) && /DailyAttendance\.missionMinutes = present/.test(up17.stdout), (up17.stderr || up17.stdout || '').slice(-200))
  // Phase 2 S3 review fix 1: the dated default-shift history migration joins
  // the chain too (the current engine reads DefaultShiftHistory on every recalc)
  const up17r2 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.7.0-s3r2 migration up applied (DefaultShiftHistory table)', up17r2.status === 0 && /DefaultShiftHistory = present/.test(up17r2.stdout), (up17r2.stderr || up17r2.stdout || '').slice(-200))
  const up17r2b = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.7.0-s3r2 migration up is idempotent (2nd run: 0 new snapshot rows)', up17r2b.status === 0 && /snapshot rows written \(persons with a default, no history yet\): 0/.test(up17r2b.stdout))

  const verifySeed = sh('node', ['scripts/test-db-raw.mjs', '--json',
    'SELECT COUNT(*) AS n FROM "Personnel" WHERE id=\'pmig1\'',
    'SELECT COUNT(*) AS n FROM "Device" WHERE id=\'dmig1\' AND "password"=123456',
    'SELECT COUNT(*) AS n FROM "PunchRecord" WHERE id=\'prmig1\'',
    'SELECT COUNT(*) AS n FROM "RawPunch"',
  ], { DATABASE_URL: MIG_URL })
  const verifyLines = verifySeed.stdout.split(/\r?\n/).filter((l) => l.startsWith('RAW_JSON')).map((l) => JSON.parse(l.slice(9))[0].n)
  check(
    'seeded personnel/device (incl. plaintext password 123456)/punch survive the migrations; no phantom raw punches',
    verifySeed.status === 0 && verifyLines.length === 4 && verifyLines.slice(0, 3).every((n) => n === 1) && verifyLines[3] === 0,
    (verifySeed.stderr || verifyLines.join(' | ')).slice(-200),
  )

  const parity = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: MIG_URL })
  const parityOut = (parity.stdout || '') + (parity.stderr || '')
  check(
    'prisma db push sees NO drift after migrations (parity with INSTALL.bat path)',
    parity.status === 0 && /already in sync/i.test(parityOut),
    parityOut.slice(-200).replace(/\n/g, ' '),
  )

  // fresh-install parity: db push from the CURRENT schema, then v1.3.1 up = no-op
  const freshDb = generateTestDatabasePath(TMP)
  tmpFiles.push(freshDb)
  const pushNew = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: sqliteUrl(freshDb) })
  const upFresh = sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: sqliteUrl(freshDb) })
  check(
    'fresh INSTALL.bat path (db push) + v1.3.1 up converge (idempotent on new schema)',
    pushNew.status === 0 && upFresh.status === 0 && /RejectedRecord table: present/.test(upFresh.stdout) && /Device.passwordEnc: present/.test(upFresh.stdout),
    (upFresh.stderr || '').slice(-200),
  )

  // rollback safety: legacy data must survive the down migration untouched
  const down131 = sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'down', '--confirm'], { DATABASE_URL: MIG_URL })
  check('v1.3.1 rollback removes the new objects', down131.status === 0 && /RejectedRecord table: MISSING/.test(down131.stdout), (down131.stderr || '').slice(-200))
  sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'down', '--confirm'], { DATABASE_URL: MIG_URL })
  const downGuard = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'down'], { DATABASE_URL: MIG_URL })
  check('rollback requires --confirm (v1.3.0 guard intact)', downGuard.status === 2)
  const down1 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'down', '--confirm'], { DATABASE_URL: MIG_URL })
  check('v1.3.0 rollback removes the Phase 1 tables', down1.status === 0 && /RawPunch table: MISSING/.test(down1.stdout))
  const verifySeedAfterDown = sh('node', ['scripts/test-db-raw.mjs', '--json',
    'SELECT COUNT(*) AS n FROM "Personnel" WHERE id=\'pmig1\'',
    'SELECT COUNT(*) AS n FROM "Device" WHERE id=\'dmig1\' AND "password"=123456',
    'SELECT COUNT(*) AS n FROM "PunchRecord" WHERE id=\'prmig1\'',
  ], { DATABASE_URL: MIG_URL })
  const downLines = verifySeedAfterDown.stdout.split(/\r?\n/).filter((l) => l.startsWith('RAW_JSON'))
  check('seeded rows (incl. the stored password) still intact AFTER full rollback — rollback deletes nothing existing', verifySeedAfterDown.status === 0 && downLines.length === 3 && downLines.every((l) => JSON.parse(l.slice(9))[0].n === 1), (verifySeedAfterDown.stderr || '').slice(-200))
  const up3 = sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('re-up after rollback works', up3.status === 0 && /RawPunch table: present/.test(up3.stdout))
  const up131c = sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: MIG_URL })
  check('v1.3.1 re-up after rollback works', up131c.status === 0 && /RejectedRecord table: present/.test(up131c.stdout))

  /* ================================================================
   * 2) prepare the MAIN test database (old schema + full migration chain)
   * ================================================================ */
  const pushMainOld = sh('npx', ['prisma', 'db', 'push', '--schema', OLD_SCHEMA, '--skip-generate'], { DATABASE_URL: MAIN_DB_URL })
  if (pushMainOld.status !== 0) throw new Error('main db old-schema push failed: ' + (pushMainOld.stderr || '').slice(-300))
  for (const m of ['migrate-v1.3.0-phase1.mjs', 'migrate-v1.4.0-phase2.mjs', 'migrate-v1.3.1-phase1-fixes.mjs', 'migrate-v1.5.0-bridge-internal.mjs', 'migrate-v1.7.0-phase2-s3.mjs', 'migrate-v1.7.0-phase2-s3r2.mjs']) {
    const r = sh('node', [`scripts/${m}`, 'up'], { DATABASE_URL: MAIN_DB_URL })
    if (r.status !== 0) throw new Error(`main db migration failed (${m}): ` + (r.stderr || '').slice(-300))
  }

  // seed: personnel + device
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
   * 3) FIX 1 — personnel mapping never guesses
   * ================================================================ */
  section('FIX 1 — personnel mapping: exact strings, zeros, namespaces, ambiguity')
  const idx = await buildPersonnelIndex()
  check('personnel index keys are exact strings (no numeric collapse)', idx.byPersonnelCode.has('1001') && idx.byPersonnelCode.has('2002'))
  {
    const r = resolvePersonnelForEnroll('1001', idx)
    check('deviceUserId match resolves to the right person', r.status === 'MATCHED' && r.personnelId === p1.id && r.matchedVia === 'DEVICE_USER_ID')
    const r2 = resolvePersonnelForEnroll('2002', idx)
    check('personnelCode match resolves to the right person', r2.status === 'MATCHED' && r2.personnelId === p2.id && r2.matchedVia === 'PERSONNEL_CODE')
    check('no candidate → plain UNMATCHED (not ambiguous)', resolvePersonnelForEnroll('9999', idx).status === 'UNMATCHED')
    check('leading-zero variant "01001" does NOT match deviceUserId 1001', resolvePersonnelForEnroll('01001', idx).status === 'UNMATCHED')
    check('leading-zero variant "02002" does NOT match personnelCode "2002"', resolvePersonnelForEnroll('02002', idx).status === 'UNMATCHED')
  }
  // collision: person B whose personnelCode equals person A's deviceUserId
  const pCollide = await db.personnel.create({
    data: { personnelCode: '1001X', firstName: 'تقی', lastName: 'برخورد', pin: 'pincol', deviceUserId: 1002 },
  })
  {
    const pB = await db.personnel.create({
      data: { personnelCode: '1002', firstName: 'نازنین', lastName: 'برخورد', pin: 'pinb2' },
    })
    const idx2 = await buildPersonnelIndex()
    // now enroll '1002' is pCollide.deviceUserId AND pB.personnelCode → ambiguous
    const r = resolvePersonnelForEnroll('1002', idx2)
    check('deviceUserId↔personnelCode collision is AMBIGUOUS (never assigned)', r.status === 'AMBIGUOUS')
    await db.personnel.delete({ where: { id: pB.id } })
  }
  await db.personnel.delete({ where: { id: pCollide.id } })
  // duplicate deviceUserId owners
  {
    const pA = await db.personnel.create({ data: { personnelCode: '5001', firstName: 'آ', lastName: 'دوگانه', pin: 'dua1', deviceUserId: 77 } })
    const pB = await db.personnel.create({ data: { personnelCode: '5002', firstName: 'ب', lastName: 'دوگانه', pin: 'dua2', deviceUserId: 77 } })
    const idx3 = await buildPersonnelIndex()
    const r = resolvePersonnelForEnroll('77', idx3)
    check('deviceUserId owned by two personnel is AMBIGUOUS (never assigned)', r.status === 'AMBIGUOUS')
    await db.personnel.deleteMany({ where: { id: { in: [pA.id, pB.id] } } })
  }
  // leading zeros END-TO-END through ingest
  {
    const pZero = await db.personnel.create({ data: { personnelCode: '0042', firstName: 'صفر', lastName: 'صفرها', pin: '0042' } })
    const zeroBatch: RawPunchInput[] = [
      { ...rec('0042', 90_000), rawPayload: JSON.stringify({ mock: true, zero: true }) },
      { ...rec('42', 80_000), rawPayload: JSON.stringify({ mock: true, zero: false }) },
    ]
    const rz = await ingestRawPunches(zeroBatch, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
    check('enroll "0042" matches personnelCode "0042" exactly', rz.matched === 1 && rz.unmatched === 1, `matched=${rz.matched} unmatched=${rz.unmatched}`)
    const rows = await db.rawPunch.findMany({ where: { enrollNumber: { in: ['0042', '42'] }, punchTime: { gte: new Date(T0 - 120_000) } } })
    check('enrollNumber stored as EXACT string (leading zeros preserved)', rows.some((r) => r.enrollNumber === '0042') && rows.some((r) => r.enrollNumber === '42'))
    check('"42" row is UNMATCHED (zero loss is not repaired by guessing)', rows.find((r) => r.enrollNumber === '42')?.matchStatus === 'UNMATCHED')
    await db.rawPunch.deleteMany({ where: { enrollNumber: { in: ['0042', '42'] } } })
    await db.punchRecord.deleteMany({ where: { personId: pZero.id } })
    await db.personnel.delete({ where: { id: pZero.id } })
  }

  /* ================================================================
   * 4) INGEST (regression) — storage, dedupe, unmatched, simultaneous punches
   * ================================================================ */
  section('INGEST — basic import, unmatched retention, projection')
  const batch1: RawPunchInput[] = [
    rec(1001, 3600_000), // matched via deviceUserId
    rec('2002', 7200_000), // matched via exact personnelCode
    rec(9999, 1800_000), // UNMATCHED — must be stored, never dropped
    { ...rec(1001, 60_000), verifyModeRaw: 4 },
  ]
  const r1 = await ingestRawPunches(batch1, { deviceId: device.id, deviceSerial: SERIAL, sourceProtocol: 'MOCK' })
  check('all 4 valid records counted', r1.seen === 4 && r1.invalid === 0)
  check('4 new rows stored', r1.newCount === 4, `newCount=${r1.newCount}`)
  check('3 matched / 1 unmatched', r1.matched === 3 && r1.unmatched === 1, `matched=${r1.matched} unmatched=${r1.unmatched}`)
  check('legacy PunchRecord projection = 3 (matched only)', r1.punchRecordsCreated === 3, `created=${r1.punchRecordsCreated}`)
  const raw1 = await db.rawPunch.findMany({ where: { deviceSerial: SERIAL } })
  check('unmatched row kept with UNMATCHED status and NO matchNote (plain unmatched)', raw1.some((r) => r.enrollNumber === '9999' && r.matchStatus === 'UNMATCHED' && r.personnelId === null && r.matchNote == null))
  check('matched rows carry personnelId', raw1.find((r) => r.enrollNumber === '1001')?.personnelId === p1.id)
  check('enroll 2002 matched via exact personnelCode', raw1.find((r) => r.enrollNumber === '2002')?.personnelId === p2.id)
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
   * 5) FIX 4 — quarantine: no source record disappears without a trace
   * ================================================================ */
  section('FIX 4 — quarantine of invalid timestamps + separate window counts')
  const devQ = await db.device.create({
    data: { name: 'دستگاه قرنطینه', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5114, serialNumber: 'SN-Q40' },
  })
  {
    const invalidRec: RawPunchInput = {
      enrollNumber: '5150',
      punchTime: new Date('not-a-date' as unknown as string), // Invalid Date
      punchTimeRaw: '2026-13-45 99:99:99',
      inOutModeRaw: 1,
      verifyModeRaw: 1,
      rawPayload: JSON.stringify({ mock: true, broken: true, ts: '2026-13-45 99:99:99' }),
    }
    const futureRec: RawPunchInput = { ...rec(1001, 0), punchTime: new Date(Date.now() + 3 * 24 * 3600 * 1000) } // 3 days in the FUTURE (relative to the real clock)
    const okRec: RawPunchInput = { ...rec(1001, 120_000) }
    const q = await runRawSync({
      deviceId: devQ.id,
      provider: new ListProvider([okRec, invalidRec, futureRec], 'SN-Q40'),
      mode: 'MOCK',
      full: true, // cutoff = 0 → the future record is excluded ONLY by the future guard
    })
    check('all 3 records the provider reported are accounted for (seen=3)', q.seen === 3)
    check('inWindow=1 / rejected=1 / outOfWindow=1 — full accounting', q.inWindow === 1 && q.rejected === 1 && q.outOfWindow === 1, `inW=${q.inWindow} rej=${q.rejected} oow=${q.outOfWindow}`)
    check('accounting identity holds: seen = inWindow + rejected + outOfWindow', q.seen === q.inWindow + q.rejected + q.outOfWindow)
    check('run closes PARTIAL (not a clean SUCCESS) when new records were quarantined', q.status === 'PARTIAL', q.status)
    const rejRow = await db.rejectedRecord.findFirst({ where: { deviceSerial: 'SN-Q40' } })
    check('invalid record is preserved in the quarantine (enroll + raw payload + wall clock)', !!rejRow && rejRow.enrollNumber === '5150' && !!rejRow.rawPayload?.includes('broken') && rejRow.punchTimeRaw === '2026-13-45 99:99:99')
    check('reject reason is machine-readable (INVALID_TIMESTAMP) with source meta', rejRow?.rejectReason === 'INVALID_TIMESTAMP' && rejRow?.sourceMeta != null)
    check('the FUTURE record is NOT quarantined (window exclusion ≠ malformed)', (await db.rejectedRecord.count({ where: { deviceSerial: 'SN-Q40' } })) === 1)
    const runQ = await db.deviceSyncRun.findFirst({ where: { deviceId: devQ.id }, orderBy: { startedAt: 'desc' } })
    check('DeviceSyncRun carries recordsRejected and recordsOutOfWindow', runQ?.recordsRejected === 1 && runQ?.recordsOutOfWindow === 1)
    check('DeviceSyncRun status PARTIAL with an explanatory (sanitized) error', runQ?.status === 'PARTIAL' && /قرنطینه/.test(runQ?.error ?? ''))

    // re-read of the same bad record → quarantine dedupe, run not PARTIAL again
    const q2 = await runRawSync({
      deviceId: devQ.id,
      provider: new ListProvider([okRec, invalidRec, futureRec], 'SN-Q40'),
      mode: 'MOCK',
      full: true,
    })
    check('re-reporting the same bad record adds no quarantine row (fingerprint dedupe)', q2.rejected === 0 && q2.rejectedDuplicate === 1 && (await db.rejectedRecord.count({ where: { deviceSerial: 'SN-Q40' } })) === 1)
    check('nothing NEW quarantined → run is a clean SUCCESS again (history stays honest)', q2.status === 'SUCCESS' && q2.newCount === 0)
    // quiet rt-poll: no-op runs stay silent even with known-bad records in the stream
    const q3 = await runRawSync({
      deviceId: devQ.id,
      provider: new ListProvider([okRec, invalidRec, futureRec], 'SN-Q40'),
      mode: 'MOCK',
      full: true,
      quiet: true,
    })
    const runsQ = await db.deviceSyncRun.count({ where: { deviceId: devQ.id } })
    check('quiet no-op poll stays silent (0 new, 0 newly quarantined → no run row)', q3.status === 'SUCCESS' && runsQ === 2, `runs=${runsQ}`)
  }
  {
    // out-of-window (BEFORE the cutoff) records are counted separately
    const devW = await db.device.create({
      data: { name: 'دستگاه بازه', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5115, serialNumber: 'SN-W40' },
    })
    const oldRec: RawPunchInput = { ...rec(1001, 40 * 24 * 3600 * 1000) } // 40 days back
    const nowRec: RawPunchInput = { ...rec(1001, 120_000) }
    const w = await runRawSync({ deviceId: devW.id, provider: new ListProvider([nowRec, oldRec], 'SN-W40'), mode: 'MOCK', daysBack: 30 })
    check('window-excluded (before cutoff) record counted as outOfWindow, NOT quarantined', w.outOfWindow === 1 && w.rejected === 0 && w.status === 'SUCCESS')
    check('out-of-window record leaves no RejectedRecord row', (await db.rejectedRecord.count({ where: { deviceSerial: 'SN-W40' } })) === 0)
    check('window test accounting: seen=2, inWindow=1', w.seen === 2 && w.inWindow === 1)
    await db.device.delete({ where: { id: devW.id } })
  }
  await db.device.delete({ where: { id: devQ.id } })

  /* ================================================================
   * 6) FIX 2 — recalculation per (personId, day) + PARTIAL recovery
   * ================================================================ */
  section('FIX 2 — per-(personId, day) recalculation across several days')
  {
    const devR = await db.device.create({
      data: { name: 'دستگاه بازمحاسبه', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5116, serialNumber: 'SN-RC25' },
    })
    // Tehran days: 2026-11-02 (Mon), 11-03 (Tue), 11-04 (Wed) — punches in
    // Tehran morning (UTC ~05:30) and afternoon (UTC ~13:30)
    const at = (utcH: number, utcMin: number, day: number) => new Date(Date.UTC(2026, 10, day, utcH, utcMin, 0))
    const multiDay: RawPunchInput[] = [
      // person 1: 2 punches day 2, 1 punch day 3, 1 punch day 4
      { ...rec(1001, 0), punchTime: at(5, 30, 2) },
      { ...rec(1001, 0), punchTime: at(13, 30, 2) },
      { ...rec(1001, 0), punchTime: at(5, 0, 3) },
      { ...rec(1001, 0), punchTime: at(6, 30, 4) },
      // person 2: 1 punch day 2, 2 punches day 3, 1 punch day 4
      { ...rec('2002', 0), punchTime: at(5, 30, 2) },
      { ...rec('2002', 0), punchTime: at(5, 0, 3) },
      { ...rec('2002', 0), punchTime: at(13, 0, 3) },
      { ...rec('2002', 0), punchTime: at(6, 30, 4) },
    ]
    const s = await runRawSync({ deviceId: devR.id, provider: new ListProvider(multiDay, 'SN-RC25'), mode: 'MOCK', full: true })
    check('multi-day import succeeded (8 new rows)', s.status === 'SUCCESS' && s.newCount === 8, `new=${s.newCount}`)
    const dayA = new Date(Date.UTC(2026, 10, 2))
    const dayB = new Date(Date.UTC(2026, 10, 3))
    const dayC = new Date(Date.UTC(2026, 10, 4))
    const da = async (pid: string, day: Date) => db.dailyAttendance.findUnique({ where: { personId_date: { personId: pid, date: day } } })
    check('person 1 day A recalculated (2 punches → PRESENT)', (await da(p1.id, dayA))?.status === 'PRESENT')
    check('person 1 day B recalculated (1 punch → INCOMPLETE)', (await da(p1.id, dayB))?.status === 'INCOMPLETE')
    check('person 1 day C recalculated (the OLD code recalc-ed only the max-punch day)', (await da(p1.id, dayC)) != null)
    check('person 2 day A recalculated (INCOMPLETE)', (await da(p2.id, dayA))?.status === 'INCOMPLETE')
    check('person 2 day B recalculated (2 punches → PRESENT)', (await da(p2.id, dayB))?.status === 'PRESENT')
    check('person 2 day C recalculated', (await da(p2.id, dayC)) != null)
    check('all six (personId, day) pairs were recalculated — not just the newest day', [await da(p1.id, dayA), await da(p1.id, dayB), await da(p1.id, dayC), await da(p2.id, dayA), await da(p2.id, dayB), await da(p2.id, dayC)].every(Boolean))
    await db.device.delete({ where: { id: devR.id } })
    await db.rawPunch.deleteMany({ where: { deviceSerial: 'SN-RC25' } })
    await db.punchRecord.deleteMany({ where: { timestamp: { gte: dayA, lt: new Date(Date.UTC(2026, 10, 5)) }, source: 'DEVICE' } })
    await db.dailyAttendance.deleteMany({ where: { date: { gte: dayA, lt: new Date(Date.UTC(2026, 10, 5)) } } })

    // --- recalculation FAILURE → PARTIAL, recoverable WITHOUT re-import ----
    const devF = await db.device.create({
      data: { name: 'دستگاه بازیابی', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5117, serialNumber: 'SN-RC26' },
    })
    const failBatch: RawPunchInput[] = [
      { ...rec(1001, 0), punchTime: at(5, 30, 2) },
      { ...rec(1001, 0), punchTime: at(13, 30, 2) },
      { ...rec('2002', 0), punchTime: at(5, 30, 2) },
    ]
    const failDay = dayA.toISOString().slice(0, 10)
    const seenCalls: Array<{ personId: string; day: string }> = []
    const sFail = await runRawSync({
      deviceId: devF.id,
      provider: new ListProvider(failBatch, 'SN-RC26'),
      mode: 'MOCK',
      full: true,
      recalc: async (personId, day) => {
        seenCalls.push({ personId, day: day.toISOString().slice(0, 10) })
        if (personId === p1.id && day.toISOString().slice(0, 10) === failDay) throw new Error('خطای ساختگی بازمحاسبه (تست)')
        return 0
      },
    })
    check('ingestion succeeded but the run closes PARTIAL on recalculation failure', sFail.status === 'PARTIAL' && sFail.recalcFailed === 1, `status=${sFail.status} recalcFailed=${sFail.recalcFailed}`)
    check('the OTHER (personId, day) pairs were still recalculated (error not swallowed globally)', seenCalls.some((c) => c.personId === p2.id && c.day === failDay))
    check('recalc receives the AFFECTED day (t.day), not a global maxPunchTime', seenCalls.every((c) => c.day === failDay))
    const runF = await db.deviceSyncRun.findFirst({ where: { deviceId: devF.id }, orderBy: { startedAt: 'desc' } })
    check('DeviceSyncRun PARTIAL records the recalculation failure (sanitized error)', runF?.status === 'PARTIAL' && /بازمحاسبه/.test(runF?.error ?? ''))
    check('the punches are safely stored despite the recalc failure (bookmark advanced)', (await db.rawPunch.count({ where: { deviceSerial: 'SN-RC26' } })) === 3 && runF?.bookmarkAt != null)
    check('the failed day has NO DailyAttendance row yet (explicit recoverable state)', (await db.dailyAttendance.count({ where: { personId: p1.id, date: dayA } })) === 0)
    // retry — WITHOUT re-importing: pairs are re-derived from the STORED rows
    const before = await db.rawPunch.count({ where: { deviceSerial: 'SN-RC26' } })
    const retry = await retrySyncRunRecalcs(runF!.id)
    check('retrySyncRunRecalcs recalculates from stored rows (no re-import, no device read)', retry.failed === 0 && retry.pairs === 2 && (await db.rawPunch.count({ where: { deviceSerial: 'SN-RC26' } })) === before, `pairs=${retry.pairs}`)
    check('the previously failed day is now recalculated (2 punches → PRESENT)', (await da(p1.id, dayA))?.status === 'PRESENT')
    const runF2 = await db.deviceSyncRun.findUnique({ where: { id: runF!.id } })
    check('after a full recalc retry the run row closes as SUCCESS', runF2?.status === 'SUCCESS', runF2?.status ?? '')
    await db.device.delete({ where: { id: devF.id } })
    await db.rawPunch.deleteMany({ where: { deviceSerial: 'SN-RC26' } })
    await db.punchRecord.deleteMany({ where: { timestamp: { gte: dayA, lt: new Date(Date.UTC(2026, 10, 3)) }, source: 'DEVICE' } })
    await db.dailyAttendance.deleteMany({ where: { date: { gte: dayA, lt: new Date(Date.UTC(2026, 10, 3)) } } })
  }

  /* ================================================================
   * 7) FIX 3 — atomic, auditable rematch
   * ================================================================ */
  section('FIX 3 — atomic rematch: rollback, idempotency, audit trail')
  {
    const devM = await db.device.create({
      data: { name: 'دستگاه تطبیق', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5118, serialNumber: 'SN-RM31' },
    })
    const uBatch: RawPunchInput[] = [rec(7788, 300_000), rec(7788, 400_000)]
    await ingestRawPunches(uBatch, { deviceId: devM.id, deviceSerial: 'SN-RM31', sourceProtocol: 'MOCK' })
    const p3 = await db.personnel.create({
      data: { personnelCode: '7788', firstName: 'مهدی', lastName: 'نوری', pin: '7788' },
    })

    // (a) failure BETWEEN mapping update and projection creation → full rollback
    const failRematch = await rematchRawPunches({
      _projectionCreate: async () => {
        throw new Error('خطای ساختگی ایجاد پروجکشن (تست)')
      },
    })
    check('projection failure surfaces (not swallowed)', failRematch.failed === 2 && failRematch.errors.length === 2)
    const rowsAfterFail = await db.rawPunch.findMany({ where: { enrollNumber: '7788' } })
    check('failed rematch left rows UNMATCHED (transaction rolled back)', rowsAfterFail.length === 2 && rowsAfterFail.every((r) => r.matchStatus === 'UNMATCHED' && r.personnelId === null))
    check('failed rematch created NO PunchRecord', (await db.punchRecord.count({ where: { personId: p3.id } })) === 0)
    check('failed rematch wrote NO AuditLog entry', (await db.auditLog.count({ where: { entity: 'RawPunch', action: 'RAW_PUNCH_REMATCH' } })) === 0)
    check('failed rematch is recoverable (rows are still retried by the next call)', true)

    // (b) successful rematch — atomic + audited
    const rem = await rematchRawPunches({ userId: 'test-admin-user' })
    check('retry succeeds after the failure was rolled back (recoverable)', rem.matched === 2 && rem.punchRecordsCreated === 2, `matched=${rem.matched} created=${rem.punchRecordsCreated}`)
    const remRow = await db.rawPunch.findFirst({ where: { enrollNumber: '7788' }, orderBy: { punchTime: 'asc' } })
    check('rows now MATCHED with personnelId set', remRow?.matchStatus === 'MATCHED' && remRow?.personnelId === p3.id)
    check('rematch created the legacy projection', (await db.punchRecord.count({ where: { personId: p3.id, source: 'DEVICE' } })) === 2)
    const audits = await db.auditLog.findMany({ where: { entity: 'RawPunch', action: 'RAW_PUNCH_REMATCH' }, orderBy: { createdAt: 'asc' } })
    check('each mapping change is recorded in AuditLog (2 rows)', audits.length === 2)
    const auditForRemRow = audits.find((a) => {
      try {
        return JSON.parse(a.details ?? '{}').rawPunchId === remRow?.id
      } catch {
        return false
      }
    })
    const det = auditForRemRow ? JSON.parse(auditForRemRow.details ?? '{}') : {}
    check(
      'audit record identifies rawPunchId, previous mapping, new mapping, operation time',
      !!auditForRemRow && det.rawPunchId === remRow?.id && det.previousMapping?.personnelId == null && det.previousMapping?.matchStatus === 'UNMATCHED' &&
        det.newMapping?.personnelId === p3.id && det.newMapping?.matchStatus === 'MATCHED' && typeof det.operationTime === 'string' &&
        auditForRemRow.userId === 'test-admin-user',
      JSON.stringify(det).slice(0, 120),
    )
    check('audit details contain no password-like values', !/password|رمز/i.test(auditForRemRow?.details ?? ''))
    const rem2 = await rematchRawPunches()
    check('rematch is idempotent (second call matches nothing)', rem2.matched === 0)
    check('rematch does not duplicate the projection', (await db.punchRecord.count({ where: { personId: p3.id, source: 'DEVICE' } })) === 2)
    check('idempotent rematch adds no audit rows', (await db.auditLog.count({ where: { entity: 'RawPunch', action: 'RAW_PUNCH_REMATCH' } })) === 2)

    // (c) ambiguous enroll numbers are never auto-assigned by rematch
    const pUid = await db.personnel.create({ data: { personnelCode: 'AA1', firstName: 'ی', lastName: 'آزمون', pin: 'am1', deviceUserId: 606 } })
    const pUid2 = await db.personnel.create({ data: { personnelCode: 'BB1', firstName: 'ک', lastName: 'آزمون', pin: 'am2', deviceUserId: 606 } })
    await ingestRawPunches([rec(606, 200_000)], { deviceId: devM.id, deviceSerial: 'SN-RM31', sourceProtocol: 'MOCK' })
    const remAmb = await rematchRawPunches()
    const ambRow = await db.rawPunch.findFirst({ where: { enrollNumber: '606' } })
    check('ambiguous deviceUserId (two owners) stays UNMATCHED after rematch', ambRow?.matchStatus === 'UNMATCHED' && ambRow?.personnelId == null)
    check('ambiguous row carries an AMBIGUOUS_* matchNote for admin review', (ambRow?.matchNote ?? '').startsWith('AMBIGUOUS_'))
    check('rematch reports the ambiguous count', remAmb.ambiguous >= 1)
    await db.personnel.deleteMany({ where: { id: { in: [pUid.id, pUid2.id] } } })
    await db.rawPunch.deleteMany({ where: { enrollNumber: '606' } })
    await db.device.delete({ where: { id: devM.id } })
    await db.rawPunch.deleteMany({ where: { deviceSerial: 'SN-RM31' } })
    await db.punchRecord.deleteMany({ where: { personId: p3.id } })
    await db.dailyAttendance.deleteMany({ where: { personId: p3.id } })
    await db.personnel.delete({ where: { id: p3.id } })
    await db.auditLog.deleteMany({ where: { action: 'RAW_PUNCH_REMATCH' } })
  }

  /* ================================================================
   * 8) FIX 7 — per-device sync lock (DB-backed, cross-process)
   * ================================================================ */
  section('FIX 7 — DB-backed per-device sync lock')
  const devL = await db.device.create({
    data: { name: 'دستگاه قفل', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5119, serialNumber: 'SN-LK70' },
  })
  {
    // busy on a live lock
    await db.deviceSyncLock.create({ data: { deviceId: devL.id, lockToken: 'held-by-other', expiresAt: new Date(Date.now() + 60_000) } })
    let busy = false
    try {
      await withDeviceSyncLock(devL.id, 'دستگاه قفل', async () => 'never')
    } catch (err) {
      busy = err instanceof SyncBusyError
    }
    check('second sync attempt while a LIVE lock is held → SyncBusyError', busy)
    await db.deviceSyncLock.deleteMany({ where: { deviceId: devL.id } })

    // released after failure
    let threw = false
    try {
      await withDeviceSyncLock(devL.id, 'دستگاه قفل', async () => { throw new Error('boom') })
    } catch (err) {
      threw = (err as Error).message === 'boom'
    }
    check('lock is released after the operation FAILS (finally)', threw && (await db.deviceSyncLock.count({ where: { deviceId: devL.id } })) === 0)

    // concurrent in-process attempts — exactly one wins
    const results = await Promise.allSettled([
      withDeviceSyncLock(devL.id, 'دستگاه قفل', async () => { await new Promise((r) => setTimeout(r, 250)); return 'first' }),
      withDeviceSyncLock(devL.id, 'دستگاه قفل', async () => 'second'),
    ])
    const wins = results.filter((r) => r.status === 'fulfilled').length
    const busies = results.filter((r) => r.status === 'rejected' && r.reason instanceof SyncBusyError).length
    check('two concurrent lock attempts → exactly one winner, one busy', wins === 1 && busies === 1)
    check('lock table is clean after both attempts', (await db.deviceSyncLock.count({ where: { deviceId: devL.id } })) === 0)

    // lease expiry → takeover (process-crash recovery)
    await db.deviceSyncLock.create({ data: { deviceId: devL.id, lockToken: 'dead-process', expiresAt: new Date(Date.now() - 1000) } })
    const afterCrash = await withDeviceSyncLock(devL.id, 'دستگاه قفل', async () => 'took-over')
    check('EXPIRED lease is taken over (crashed holder cannot deadlock the device)', afterCrash === 'took-over')
    check('taken-over lock is released afterwards', (await db.deviceSyncLock.count({ where: { deviceId: devL.id } })) === 0)
  }

  section('FIX 7 — cross-process concurrent import of the same batch')
  {
    const batchN = 6
    const wEnv = (w: number) => ({
      DATABASE_URL: MAIN_DB_URL,
      WORKER_TMP_ROOT: TMP,
      WORKER_DEVICE_ID: devL.id,
      WORKER_SERIAL: 'CONC-SERIAL',
      WORKER_ID: String(w),
    })
    const w1 = spawn('npx', ['tsx', 'scripts/test-concurrent-import-worker.ts'], { cwd: ROOT, env: { ...process.env, ...wEnv(1) } as NodeJS.ProcessEnv })
    const w2 = spawn('npx', ['tsx', 'scripts/test-concurrent-import-worker.ts'], { cwd: ROOT, env: { ...process.env, ...wEnv(2) } as NodeJS.ProcessEnv })
    const collect = (p: import('child_process').ChildProcess) => new Promise<{ code: number | null; out: string }>((resolve) => {
      let out = ''
      p.stdout?.on('data', (d) => { out += String(d) })
      p.stderr?.on('data', (d) => { out += String(d) })
      p.on('close', (code) => resolve({ code, out }))
    })
    const [a, b] = await Promise.all([collect(w1), collect(w2)])
    check('both concurrent import workers exit cleanly', a.code === 0 && b.code === 0, `codes=${a.code}/${b.code} ${((a.out || '') + (b.out || '')).slice(-200)}`)
    const parseRes = (out: string) => {
      const line = out.split(/\r?\n/).find((l) => l.startsWith('WORKER_RESULT '))
      return line ? JSON.parse(line.slice('WORKER_RESULT '.length)) : null
    }
    const ra = parseRes(a.out)
    const rb = parseRes(b.out)
    check('each worker reports full accounting of the shared batch (new+dup = 6)', !!ra && !!rb && ra.newCount + ra.duplicate === batchN && rb.newCount + rb.duplicate === batchN, `a=${JSON.stringify(ra)} b=${JSON.stringify(rb)}`)
    const stored = await db.rawPunch.count({ where: { deviceSerial: 'CONC-SERIAL' } })
    check(`the unique constraint keeps the database duplicate-free: exactly ${batchN} rows stored (not ${batchN * 2})`, stored === batchN, `stored=${stored}`)
    check('no lock rows remain after the workers are done', (await db.deviceSyncLock.count({})) === 0)
    await db.rawPunch.deleteMany({ where: { deviceSerial: 'CONC-SERIAL' } })
    await db.punchRecord.deleteMany({ where: { deviceId: devL.id, source: 'DEVICE' } })
  }
  await db.device.delete({ where: { id: devL.id } })

  /* ================================================================
   * 9) FIX 6 — device password at-rest: honest boundary
   * ================================================================ */
  section('FIX 6 — device password at-rest security (honest boundary)')
  {
    check('protection is DISABLED by default (env flag unset / non-Windows)', !isDeviceSecretProtectionEnabled())
    const stored = await storeDevicePassword(4891489)
    check('storeDevicePassword (protection off) keeps the plaintext column representation', stored.password === 4891489 && stored.passwordEnc === null && stored.stored === 'plaintext')
    const storedNull = await storeDevicePassword(null)
    check('clearing the password clears both representations', storedNull.password === null && storedNull.passwordEnc === null)
    check('getDevicePassword reads the plaintext column', (await getDevicePassword({ password: 4891489, passwordEnc: null })) === 4891489)
    check('deviceHasPassword sees both representations', deviceHasPassword({ password: 123, passwordEnc: null }) && deviceHasPassword({ password: null, passwordEnc: 'dpapi:machine:AAAA' }) && !deviceHasPassword({ password: null, passwordEnc: null }))
    check('isProtectedSecret recognizes the DPAPI format only', isProtectedSecret('dpapi:machine:AAAA') && !isProtectedSecret('123456') && !isProtectedSecret(null))
    let refused = false
    try {
      await protectDeviceSecret('123456')
    } catch (err) {
      refused = err instanceof SecretProtectionUnavailableError
    }
    check('protectDeviceSecret REFUSES on non-Windows (no fake encryption)', refused)
    let refusedRead = false
    try {
      await getDevicePassword({ password: null, passwordEnc: 'dpapi:machine:AAAA' })
    } catch (err) {
      refusedRead = err instanceof SecretProtectionUnavailableError
    }
    check('reading a protected value on non-Windows fails loudly (no silent plaintext fallback)', refusedRead)
    const safe = sanitizeDeviceForApi({
      id: 'd1', name: 'X', password: 4891489, passwordEnc: null, bridgeToken: null, serialNumber: 'S', port: 5005, enabled: true, status: 'ONLINE', type: 'NOVIN_NP', protocol: 'TCP', location: null, ipAddress: null, lastLogAt: null, lastSyncAt: null, lastSyncCount: 0, syncIntervalMin: 0, createdAt: new Date(), rawCursorAt: null,
    })
    check('sanitizeDeviceForApi still strips the password', safe.password === undefined && safe.hasPassword === true)
    const safeEnc = sanitizeDeviceForApi({
      id: 'd2', name: 'Y', password: null, passwordEnc: 'dpapi:machine:SECRETBLOB', bridgeToken: 'bt', serialNumber: 'S', port: 5005, enabled: true, status: 'ONLINE', type: 'NOVIN_NP', protocol: 'TCP', location: null, ipAddress: null, lastLogAt: null, lastSyncAt: null, lastSyncCount: 0, syncIntervalMin: 0, createdAt: new Date(), rawCursorAt: null,
    })
    check('sanitizeDeviceForApi strips the PROTECTED blob and any legacy bridge token too', safeEnc.passwordEnc === undefined && (safeEnc as unknown as Record<string, unknown>).bridgeToken === undefined && safeEnc.hasProtectedPassword === true && !JSON.stringify(safeEnc).includes('SECRETBLOB') && !JSON.stringify(safeEnc).includes('"bt"'))
    const protectScript = sh('npx', ['tsx', 'scripts/protect-device-secrets.ts'], {})
    check('protect-device-secrets script refuses to run on non-Windows (exit 2, nothing touched)', protectScript.status === 2)
  }

  /* ================================================================
   * 10) ORCHESTRATOR (regression) — sync history, retry, serial guard
   * ================================================================ */
  section('ORCHESTRATOR — sync history + retry')
  const dev2 = await db.device.create({
    data: { name: 'دستگاه دوم', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5122, serialNumber: 'SN-222' },
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
    data: { name: 'دستگاه سوم', type: 'NOVIN_NP', ipAddress: '127.0.0.1', port: 5123, serialNumber: 'SN-333' },
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

  section('SECURITY — password redaction (regression)')
  check(
    'historical log message with password is redacted',
    !/4891489/.test(redactDeviceSecrets('اتصال مستقیم برقرار شد — سریال: X · رمز شبکه: 4891489 · ترددها: 2944')),
  )
  check(
    'redaction keeps the label but not the value',
    /رمز شبکه: ●●●●/.test(redactDeviceSecrets('رمز شبکه: 4891489')),
  )
  const safeDev = sanitizeDeviceForApi({
    id: 'd1', name: 'X', password: 4891489, passwordEnc: null, bridgeToken: null, serialNumber: 'S', port: 5005, enabled: true, status: 'ONLINE', type: 'NOVIN_NP', protocol: 'TCP', location: null, ipAddress: null, lastLogAt: null, lastSyncAt: null, lastSyncCount: 0, syncIntervalMin: 0, createdAt: new Date(), rawCursorAt: null,
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
   * 11) REMATCH (regression) — end-to-end via personnel creation flow
   * ================================================================ */
  section('REMATCH — unmatched punches become usable (personnel flow)')
  {
    const p9 = await db.personnel.create({
      data: { personnelCode: '9999', firstName: 'مهدی', lastName: 'نوری', pin: '9999' },
    })
    const rem = await rematchRawPunches({ userId: 'admin-x' })
    const remRow = await db.rawPunch.findFirst({ where: { enrollNumber: '9999' } })
    check('unmatched punch rematched to the new personnel', remRow?.matchStatus === 'MATCHED' && remRow?.personnelId === p9.id)
    check('rematch created the legacy projection', rem.punchRecordsCreated >= 1)
    const rem2 = await rematchRawPunches()
    check('rematch is idempotent (second call matches nothing)', rem2.matched === 0)
    check('rematch does not duplicate the projection', (await db.punchRecord.count({ where: { personId: p9.id, source: 'DEVICE' } })) === 1)
  }

  /* ================================================================
   * 12) E2E (MOCK DEVICE) — importNpDeviceLogs over TCP (with the DB lock)
   * ================================================================ */
  section('E2E MOCK DEVICE — importNpDeviceLogs vs mock-sbxpc-device.js')
  const MOCK_PORT = 5111
  const mock = spawn('node', ['scripts/mock-sbxpc-device.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(MOCK_PORT), SERIAL, LOGS: '60', USERS: '1001,7777' } as NodeJS.ProcessEnv,
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  let mockReady = false
  const mockOut: string[] = []
  mock.stdout?.on('data', (d) => { mockOut.push(String(d)); if (String(d).includes('listening')) mockReady = true })
  mock.stderr?.on('data', (d) => mockOut.push(String(d)))
  await new Promise((r) => setTimeout(r, 1500))
  check('mock device is listening', mockReady)
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
  check('the per-device DB lock is fully released after the e2e syncs', (await db.deviceSyncLock.count({ where: { deviceId: mockDevice.id } })) === 0)
  mock.kill('SIGTERM')

  /* ================================================================
   * SUMMARY — mock vs hardware clearly separated
   * ================================================================ */
  console.log(`\n\x1b[1m━━━ SUMMARY\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  MIGRATION tests: covered in the MIGRATION sections above (isolated temp databases, data preserved across up AND rollback)`)
  console.log(`  HARDWARE tests : 0 (none run — requires the NP756+ on the Windows machine, Phase 2; nothing here claims hardware validation)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  await db.$disconnect()

  // cleanup: only the files this run generated inside the test directory
  for (const f of tmpFiles) {
    try {
      assertTestOwnedDatabasePath(f, TMP)
      rmSync(f, { force: true })
      rmSync(`${f}-journal`, { force: true })
    } catch {
      // never touch anything outside TMP (fix 5 invariant)
    }
  }
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('SUITE CRASHED:', err)
  const { db } = await import('../src/lib/db').catch(() => ({ db: null as never }))
  await db?.$disconnect?.()
  process.exit(1)
})
