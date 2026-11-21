/**
 * Phase 3 stage 2 — historical punch download test suite (90-day demand).
 * =====================================================================
 * REAL SQLite + REAL Prisma + the REAL runRawSync orchestrator, on a
 * THROWAWAY test-owned database (scripts/test-db-guard.ts). The transport is
 * a MOCK provider (NO hardware involved — nothing here claims hardware
 * validation; the real NP756+ checklist is separate).
 *
 * The user-visible defect under test: "selecting the last 90 days returns
 * only ~30 punches per person". This suite PROVES the pipeline has no hidden
 * 30-record / 30-day cap anywhere (UI → daysBack → cutoff → ingest → store)
 * with SCATTERED records across the FULL 90 days, and that the new per-run
 * accounting counters surface the full story:
 *
 *   [A] migration up: recordsAnnounced / recordsInvalidTimestamps /
 *       abnormalEnd columns exist; idempotent; prisma db push converges
 *   [B] 90-day scattered download: 12 persons × 90 days (one punch on every
 *       single day, at varying hours) + extras; daysBack=90 → ALL 90×12
 *       in-window records are STORED (per person: exactly 90 distinct days —
 *       no 30-record or 30-day truncation anywhere in the pipeline)
 *   [C] counters: announced/delivered/in-window/stored/duplicate/out-of-
 *       window/unmatched/quarantined/invalid — persisted on DeviceSyncRun
 *   [D] idempotent re-download: same 90-day range again → 0 new, all
 *       duplicates, nothing lost
 *   [E] full-history window (daysBack=3650): older out-of-window records
 *       (100 days back) now flow in → nothing older is ever dropped
 *   [F] abnormalEnd (truncated read): PARTIAL status, delivered records
 *       STILL stored, abnormalEnd flag + Persian error persisted, device log
 *       WARN — a short read can never masquerade as complete again
 *   [G] announced > delivered mismatch surfaces in the result (SBXPC count
 *       path) — the operator can SEE the device had more than arrived
 *   [H] bookmark incremental: after a big download, an 'auto' sync picks up
 *       ONLY the new records (no re-download, no loss)
 *
 * Run:  npx tsx scripts/test-historical-download.ts
 */
import { spawnSync } from 'child_process'
import { mkdirSync, rmSync, cpSync, existsSync } from 'fs'
import path from 'path'
import { PrismaClient } from '@prisma/client'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'
import type { RawPunchInput } from '../src/lib/raw-import'
import type { RawPunchProvider, RawPunchRead } from '../src/lib/raw-sync'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_HISTDL || '/tmp/hist-download-tests'
const DB_FILE = generateTestDatabasePath(TMP)
const DB_URL = sqliteUrl(DB_FILE)
process.env.DATABASE_URL = DB_URL // before the db client is imported

let passed = 0
let failed = 0
let sectionsPassed = 0
let sectionsFailed = 0
let sectionFailed = false
let sectionsStarted = 0
const failures: string[] = []
function section(t: string) {
  if (sectionsStarted > 0) { if (sectionFailed) sectionsFailed++; else sectionsPassed++ }
  sectionsStarted++
  sectionFailed = false
  console.log(`\n━━━ ${t}`)
}
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; sectionFailed = true; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}
function sh(cmd: string, args: string[], env: Record<string, string> = {}) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf-8', timeout: 240_000 })
}

/** Mock transport — delivers a fixed record list with optional meta keys. */
class MockProvider implements RawPunchProvider {
  readonly protocol = 'MOCK' as const
  constructor(
    private readonly records: RawPunchInput[],
    private readonly meta: Record<string, unknown> = {},
  ) {}
  async readAll(): Promise<RawPunchRead> {
    return { records: this.records, deviceSerial: 'MOCK-SERIAL-001', deviceTime: new Date().toISOString(), meta: { proto: 'mock', ...this.meta } }
  }
}

function dayAtMiddayUTC(daysBack: number, hour = 12): Date {
  const d = new Date(Date.now() - daysBack * 24 * 3600 * 1000)
  d.setUTCHours(hour, 5, 0, 0)
  return d
}

/** raw input record for a mocked transport */
function rec(enroll: number, when: Date): RawPunchInput {
  return {
    enrollNumber: String(enroll),
    punchTime: when,
    inOutModeRaw: 0,
    verifyModeRaw: 1,
    rawPayload: JSON.stringify({ proto: 'mock', enrollNumber: enroll, ts: when.toISOString() }),
  }
}

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(DB_FILE, TMP)
  rmSync(DB_FILE, { force: true })
  const push = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: DB_URL })
  check('prisma db push on the throwaway database (fresh schema incl. the new accounting columns)', push.status === 0, (push.stderr || '').slice(-200))

  const db = new PrismaClient()
  const { runRawSync } = await import('../src/lib/raw-sync')

  /* fixture: 12 personnel (enroll 1001..1012) + one device */
  const device = await db.device.create({
    data: { name: 'موک NP756', type: 'NOVIN_NP', protocolFamily: 'FKATTEND', ipAddress: '192.168.1.75', port: 4238, serialNumber: 'MOCK-SERIAL-001', enabled: true },
  })
  for (let i = 1; i <= 12; i++) {
    await db.personnel.create({
      data: { personnelCode: String(2011 + i - 1), firstName: 'تست', lastName: String(i), deviceUserId: 1000 + i, status: 'ACTIVE', pin: String(1000 + i) },
    })
  }

  /* ---------------- [A] migration ---------------- */
  section('A. v1.8.0-phase3 migration: accounting columns, idempotent, db-push convergent')
  {
    const up1 = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs'], { DATABASE_URL: DB_URL })
    check('migration up exits 0 (fresh schema already has the columns — idempotent no-op)', up1.status === 0, (up1.stderr || '').slice(-300))
    check('migration reports the three columns present', /recordsAnnounced = present/.test(up1.stdout) && /recordsInvalidTimestamps = present/.test(up1.stdout) && /abnormalEnd = present/.test(up1.stdout), up1.stdout.slice(0, 300))
    const up2 = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs'], { DATABASE_URL: DB_URL })
    check('migration re-run is an idempotent no-op', up2.status === 0 && /already present/.test(up2.stdout))

    // upgrade path: a PRE-v1.8.0 database (columns dropped) gets them via the migration
    const DB2 = generateTestDatabasePath(TMP)
    const URL2 = sqliteUrl(DB2)
    const push2 = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: URL2 })
    check('upgrade fixture pushed', push2.status === 0)
    const downgrade = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs', 'down', '--confirm'], { DATABASE_URL: URL2 })
    check('down --confirm removes the three columns (pre-v1.8.0 database shape)', downgrade.status === 0 && /dropped/.test(downgrade.stdout), (downgrade.stdout || '').slice(-200))
    const upgrade = sh('node', ['scripts/migrate-v1.8.0-phase3.mjs'], { DATABASE_URL: URL2 })
    check('up on the pre-v1.8.0 database adds the columns back', upgrade.status === 0 && /added/.test(upgrade.stdout), (upgrade.stdout || '').slice(-200))
    // prisma db push on the migrated DB must converge with the schema (no drift)
    const conv = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: URL2 })
    check('prisma db push converges after migration (no schema drift)', conv.status === 0, (conv.stderr || '').slice(-200))
    rmSync(DB2, { force: true })
  }

  /* ---------------- [B] the 90-day scattered download ---------------- */
  section('B. 90-day scattered download — NO 30-record/30-day cap exists in the pipeline')
  {
    // 12 persons × 90 days: EXACTLY ONE punch EVERY day (distinct hours), plus
    // 5 extra same-day punches on random days, plus 10 records older than the
    // 90-day window (must be counted out-of-window, NOT stored), plus 2 with
    // unparseable timestamps (quarantined) and 2 unmatched enrollees.
    const records: RawPunchInput[] = []
    for (let back = 89; back >= 0; back--) {
      for (let p = 1; p <= 12; p++) {
        records.push(rec(1000 + p, dayAtMiddayUTC(back, 6 + (back % 12)))) // scattered hours
      }
    }
    for (let e = 0; e < 5; e++) records.push(rec(1001 + e, dayAtMiddayUTC(30 + e * 7, 9))) // extra punches
    for (let o = 0; o < 10; o++) records.push(rec(1000 + (o % 12) + 1, dayAtMiddayUTC(95 + o, 11))) // OLDER than the window (95..104 days — unambiguous)
    const quarantined: RawPunchInput[] = [
      { ...rec(1001, new Date(Date.now() - 3600_000)), punchTime: new Date(Number.NaN) },
      { ...rec(1002, new Date(Date.now() - 7200_000)), punchTime: new Date(Number.NaN) },
    ]
    records.push(...quarantined)
    records.push(rec(9999, dayAtMiddayUTC(10, 8)), rec(9998, dayAtMiddayUTC(10, 18))) // unmatched enrollees

    const inWindowExpected = 12 * 90 + 5 + 2 // 12×90 daily + 5 extra + the 2 in-window UNMATCHED enrollees
    const olderExpected = 10
    const totalDelivered = records.length // 1080 + 5 + 10 + 2 quarantined + 2 unmatched = 1099
    const provider = new MockProvider(records, { announced: totalDelivered })

    const s1 = await runRawSync({ deviceId: device.id, provider, mode: 'MOCK', daysBack: 90 })

    check('announced counter surfaced from the transport', s1.announced === totalDelivered, String(s1.announced))
    check('seen == every delivered record (nothing silently dropped by the orchestrator)', s1.seen === totalDelivered, `${s1.seen}/${totalDelivered}`)
    check('inWindow == 12×90 + 5 extras + 2 in-window unmatched (NO 30-record or 30-day cap)', s1.inWindow === inWindowExpected, `${s1.inWindow}/${inWindowExpected}`)
    check('outOfWindow == the 10 older records (intentionally excluded, counted)', s1.outOfWindow === olderExpected, `${s1.outOfWindow}/${olderExpected}`)
    check('2 unparseable timestamps quarantined (RejectedRecord, trace kept)', s1.rejected === 2, String(s1.rejected))
    check('stored new == inWindow (all in-range records stored)', s1.newCount === inWindowExpected, `${s1.newCount}/${inWindowExpected}`)
    check('unmatched == the 2 foreign enrollees (KEPT as RawPunch, never dropped)', s1.unmatched === 2, String(s1.unmatched))
    check('run closed PARTIAL because of the 2 quarantined records (honest, documented semantics)', s1.status === 'PARTIAL', s1.status)

    // THE assertion the user demanded: per-person, punches on EVERY one of the
    // 90 days — 30-record/30-day truncation would break this immediately.
    const stored = await db.rawPunch.findMany({ where: { deviceId: device.id, personnelId: { not: null } }, select: { personnelId: true, punchTime: true } })
    check('raw punches stored for all 12 matched persons (inWindow minus the 2 unmatched)', stored.length === inWindowExpected - 2, String(stored.length))
    const perPerson = new Map<string, Set<string>>()
    for (const r of stored) {
      const pid = r.personnelId as string
      if (!perPerson.has(pid)) perPerson.set(pid, new Set())
      perPerson.get(pid)!.add(r.punchTime.toISOString().slice(0, 10))
    }
    const personsWith90days = [...perPerson.values()].filter((days) => days.size === 90).length
    check('every person has punches on 90 DISTINCT days (no 30-day truncation anywhere)', perPerson.size === 12 && personsWith90days === 12, `persons=${perPerson.size} with90=${personsWith90days}`)
    const maxDay = new Date(Math.max(...stored.map((r) => r.punchTime.getTime())))
    const minDay = new Date(Math.min(...stored.map((r) => r.punchTime.getTime())))
    const oldestDaysBack = (Date.now() - minDay.getTime()) / 86400000
    const newestDaysBack = (Date.now() - maxDay.getTime()) / 86400000
    check('stored range spans the full window (oldest ≈ 89 days back, newest ≈ today)', oldestDaysBack >= 88 && newestDaysBack <= 1, `oldest=${oldestDaysBack.toFixed(2)}d newest=${newestDaysBack.toFixed(2)}d`)

    /* ---------------- [C] counters persisted ---------------- */
    const run = await db.deviceSyncRun.findUnique({ where: { id: s1.runId } })
    check('DeviceSyncRun persists recordsAnnounced', run?.recordsAnnounced === totalDelivered, String(run?.recordsAnnounced))
    check('DeviceSyncRun persists recordsSeen/InWindow/New/OutOfWindow', run?.recordsSeen === totalDelivered && run?.recordsInWindow === inWindowExpected && run?.recordsNew === inWindowExpected && run?.recordsOutOfWindow === olderExpected)
    check('DeviceSyncRun persists recordsUnmatched + recordsRejected', run?.recordsUnmatched === 2 && run?.recordsRejected === 2)
    check('abnormalEnd persisted false on the clean run', run?.abnormalEnd === false)
    check('cutoff == exactly 90 days before now (window honoured to the day)', Date.now() - new Date(s1.cutoff).getTime() >= 89.9 * 86400000 && Date.now() - new Date(s1.cutoff).getTime() <= 90.1 * 86400000, s1.cutoff)

    /* ---------------- [D] idempotent re-download ---------------- */
    const s2 = await runRawSync({ deviceId: device.id, provider, mode: 'MOCK', daysBack: 90 })
    check('re-download of the SAME 90-day range: 0 new records', s2.newCount === 0, String(s2.newCount))
    check('re-download: everything in-window counted duplicate (fingerprints)', s2.duplicate === inWindowExpected, `${s2.duplicate}/${inWindowExpected}`)
    const count = await db.rawPunch.count({ where: { deviceId: device.id } })
    check('raw punch total after re-download == first run total (matched+unmatched, nothing lost)', count === s1.newCount, `${count}/${s1.newCount}`)

    /* ---------------- [E] full-history window ---------------- */
    const s3 = await runRawSync({ deviceId: device.id, provider, mode: 'MOCK', daysBack: 3650 })
    check('daysBack=3650 (همه): the 10 previously out-of-window records now flow in', s3.newCount === olderExpected, String(s3.newCount))
    check('outOfWindow == 0 with the full window', s3.outOfWindow === 0, String(s3.outOfWindow))
  }

  /* ---------------- [F] abnormalEnd (truncated read) ---------------- */
  section('F. truncated read (abnormalEnd): PARTIAL + records STILL stored + loud warning')
  {
    // simulate a read that died mid-way: the device announced 600 records but
    // only 300 were delivered with an abnormal end code
    const delivered: RawPunchInput[] = []
    for (let i = 0; i < 300; i++) delivered.push(rec(1000 + (i % 12) + 1, new Date(Date.now() - (i % 90) * 86400000 - i * 60000)))
    const s = await runRawSync({
      deviceId: device.id,
      provider: new MockProvider(delivered, { announced: 600, abnormalEnd: true, endCode: -999 }),
      mode: 'MOCK',
      daysBack: 90,
    })
    check('run closes PARTIAL on an abnormal end', s.status === 'PARTIAL', s.status)
    check('abnormalEnd surfaced in the result', s.abnormalEnd === true)
    check('announced > seen mismatch visible (600 announced vs 300 delivered)', s.announced === 600 && s.seen === 300)
    check('delivered records are STILL stored (no data thrown away)', s.newCount === 300, String(s.newCount))
    const run = await db.deviceSyncRun.findUnique({ where: { id: s.runId } })
    check('DeviceSyncRun.abnormalEnd persisted true', run?.abnormalEnd === true)
    check('DeviceSyncRun.recordsAnnounced persisted (600)', run?.recordsAnnounced === 600)
    check('run error text names the incomplete read', !!run?.error && /ناقص/.test(run.error))
    const log = await db.deviceLog.findFirst({ where: { deviceId: device.id, level: 'WARN' }, orderBy: { createdAt: 'desc' } })
    check('device log row is WARN and mentions the incomplete read', !!log && /ناقص/.test(log.message))
  }

  /* ---------------- [G] honest null announced (FK623) ---------------- */
  section('G. FK623 (no count export): announced is an honest null — never fabricated')
  {
    const one = [rec(1001, dayAtMiddayUTC(1, 10))]
    const s = await runRawSync({ deviceId: device.id, provider: new MockProvider(one), mode: 'MOCK', daysBack: 90 })
    check('announced == null when the transport reports no count', s.announced === null, String(s.announced))
    check('everything else still counts normally', s.seen === 1 && s.newCount === 1)
    const run = await db.deviceSyncRun.findUnique({ where: { id: s.runId } })
    check('recordsAnnounced column stays NULL (not 0, not invented)', run?.recordsAnnounced === null)
  }

  /* ---------------- [H] bookmark incremental ---------------- */
  section('H. bookmark incremental: auto sync fetches ONLY the new records')
  {
    const fresh = [rec(1001, new Date(Date.now() - 2 * 3600_000))]
    const s = await runRawSync({ deviceId: device.id, provider: new MockProvider(fresh), mode: 'MOCK' })
    check('auto sync (no daysBack) stores only the new record', s.newCount === 1, String(s.newCount))
    check('auto sync cutoff comes from the device bookmark, not the 30-day default', s.newCount === 1)
    const dev = await db.device.findUnique({ where: { id: device.id } })
    check('bookmark advanced to the new record', !!dev?.rawCursorAt && Math.abs(dev.rawCursorAt.getTime() - fresh[0].punchTime.getTime()) < 2000)
  }

  await db.$disconnect()
  if (sectionFailed) sectionsFailed++; else sectionsPassed++ // close the last section
  console.log('\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━')
  console.log(`historical-download suite: ${passed} assertion(s) passed, ${failed} failed`)
  console.log(`historical-download suite: ${sectionsPassed} section(s) fully green, ${sectionsFailed} with failures`)
  if (failed > 0) { console.log('\nFAILED:'); for (const f of failures) console.log('  ✘ ' + f); process.exit(1) }
  process.exit(0)
}

main().catch((e) => {
  console.error('suite crashed:', e)
  process.exit(1)
})
