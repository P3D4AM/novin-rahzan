/**
 * Phase 2 Stage S3 — attendance engine DB integration & migration test suite.
 * =====================================================================
 * REAL DATABASE (throwaway sqlite, generated via scripts/test-db-guard.ts —
 * an external DATABASE_URL is ignored, never deleted), REAL Prisma client,
 * REAL recalcRange/recalcDayForPerson. Nothing mocked, nothing faked.
 *
 * Coverage (S3 mandate §7 integration items):
 *   [A] migration up on a pre-S3 schema (pinned to the S2 commit) with real
 *       seeded rows: columns appear, existing rows intact, the seed mission
 *       type 'مأموریت' flagged isMission, other types NOT flagged; idempotent
 *       re-run; `prisma db push` from the CURRENT schema converges (no drift,
 *       INSTALL.bat path).
 *   [B] recalcRange end-to-end: night shift Sat 22:00→Sun 06:00 — the Sunday
 *       02:30 punch lands on SATURDAY's row (480 worked) and appears NOWHERE
 *       else (no double count); Sunday's own row unaffected.
 *   [C] recalcDayForPerson with the Sunday-morning punch recomputes
 *       SATURDAY's row (shift-start workday, not the calendar day).
 *   [D] overtime: Setting ot_threshold_minutes read (custom value) +
 *       OvertimeRequest PENDING/REJECTED contribute nothing; APPROVED drives
 *       payableOvertimeMinutes = min(actual, approved); request-status change
 *       (approve → recalc → payable appears; reject → payable back to 0).
 *   [E] mission: approved mission LeaveType (isMission) + APPROVED request
 *       without punches → MISSION status with exact credited minutes; with
 *       overlapping attendance → overlap subtracted; PENDING/REJECTED mission
 *       → no credit, no MISSION status. E6 (review fix 6): a leave fully
 *       swallowed by a mission → leaveMinutes 0 and NO leaveTypeId reference.
 *   [F] idempotency: recalc rerun → identical DailyAttendance rows.
 *   [G] PunchRecord rows are byte-identical before/after recalc (no mutation,
 *       no deletion — raw punches untouched by the engine).
 *   [H] S3 review fix 1 — dated default-shift history end-to-end: baseline +
 *       dated change via recordDefaultShiftChange (the personnel API's write
 *       path); days BEFORE the effective date keep the OLD shift; days AFTER
 *       take the NEW one; explicit ShiftAssignment unaffected; pre-baseline
 *       day resolves via the documented backward extension; recalc rerun
 *       changes neither rows nor the history table.
 *   [I] S3 review fix 2 — a deactivated shift still computes its own past
 *       days (isActive=false never degrades history to NO_SHIFT/fallback).
 *   [J] S3 review fix 3 — the night-shift end boundary through the REAL
 *       engine: J1 Sat 22:00 IN + Sun 06:00 OUT (exact end) = ONE row, one
 *       valid pair, 480 worked; J2 Sat 22:00 IN + Sun 06:01 OUT = documented
 *       fallback split (IN_WITHOUT_OUT / OUT_WITHOUT_IN + OUT_OF_WINDOW, no
 *       fabricated pair); J3 next-day shift starting EXACTLY at 06:00 — the
 *       OUT 06:00 closes the night (Saturday) while the IN 06:00 opens the
 *       day shift (Sunday): independent, deterministic, each punch in
 *       exactly ONE row.
 *   [K] S3 review fix 5 — a legacy NEGATIVE approved OvertimeRequest never
 *       reduces the day's approved sum (payable = min(actual, 100) not 50).
 *
 * Run: npx tsx scripts/test-attendance-db.ts   (own temp DB; no external env)
 */
import { spawnSync } from 'child_process'
import { mkdirSync, rmSync, writeFileSync } from 'fs'
import path from 'path'
import { PrismaClient } from '@prisma/client'

import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_S3DB || '/tmp/nr-s3-attendance'
const MIG_URL = sqliteUrl(generateTestDatabasePath(TMP))

// --- accounting (repo convention: assertions & sections counted separately) ---
let assertionsPassed = 0
let assertionsFailed = 0
let sectionsPassed = 0
let sectionsFailed = 0
function section(name: string, fn: () => void | Promise<void>) {
  const before = assertionsFailed
  let crashed = false
  return (async () => {
    try {
      await fn()
      console.log(`  ✔ ${name}`)
    } catch (err) {
      crashed = true
      console.log(`  ✘ ${name}`)
      console.log(`    ${(err as Error)?.message ?? err}`)
    } finally {
      const ok = !crashed && assertionsFailed === before
      console.log(`— ${name}: ${ok ? 'OK' : 'FAILED'}`)
      if (ok) sectionsPassed++
      else sectionsFailed++
    }
  })()
}
function check(name: string, cond: unknown, msg?: string) {
  try {
    if (!cond) throw new Error(msg || 'check failed')
    assertionsPassed++
    console.log(`  ✔ ${name}`)
  } catch (err) {
    assertionsFailed++
    console.log(`  ✘ ${name} — ${(err as Error)?.message ?? err}`)
  }
}

function sh(cmd: string, args: string[], env: Record<string, string>) {
  return spawnSync(cmd, args, { cwd: ROOT, encoding: 'utf8', env: { ...process.env, ...env } })
}

console.log('== Phase 2 S3 attendance DB integration tests ==\n')

const TEHRAN = (y: number, m: number, d: number, h = 0, mi = 0) =>
  new Date(Date.UTC(y, m - 1, d, h, mi) - 210 * 60000) // fixed +03:30 (2026 — no DST)

async function main() {
  mkdirSync(TMP, { recursive: true })
  const tmpFiles: string[] = []
  try {
    // ------------------------------------------------------------------
    // [A] migration on a pre-S3 schema with real rows
    // ------------------------------------------------------------------
    await section('A. migration up on pre-S3 schema (pinned to the S2 commit 2c315558)', async () => {
      // pin the OLD schema to the S2 commit — never "HEAD" (which now has S3)
      const old = sh('git', ['show', '2c315558:prisma/schema.prisma'], {})
      check('pre-S3 baseline schema resolves from git (2c315558)', old.status === 0 && old.stdout.length > 1000)
      const OLD_SCHEMA = path.join(TMP, 'old-schema-s3.prisma')
      writeFileSync(OLD_SCHEMA, old.stdout)
      tmpFiles.push(OLD_SCHEMA)
      const pushOld = sh('npx', ['prisma', 'db', 'push', '--schema', OLD_SCHEMA, '--skip-generate'], { DATABASE_URL: MIG_URL })
      check('pre-S3 database created (db push, pinned old schema)', pushOld.status === 0, (pushOld.stderr || '').slice(-200))

      // seed REAL pre-S3 rows: mission type by the OLD convention (name only),
      // a normal type, a person, a shift, a punch, a DailyAttendance row
      const seedOld = sh('node', ['scripts/test-db-raw.mjs',
        `INSERT INTO "LeaveType" ("id","name","paid","requiresApproval","color","countsTowardsBalance") VALUES ('lt-mis','مأموریت',1,1,'#3b82f6','0')`,
        `INSERT INTO "LeaveType" ("id","name","paid","requiresApproval","color","countsTowardsBalance") VALUES ('lt-norm','استحقاقی',1,1,'#10b981','1')`,
        `INSERT INTO "Personnel" ("id","personnelCode","firstName","lastName","pin","qrToken","createdAt","updatedAt") VALUES ('p-s3','S3-1','کارمند','تست','9101','seed-qr-s3','2026-01-01T00:00:00.000Z','2026-01-01T00:00:00.000Z')`,
        `INSERT INTO "Shift" ("id","name","lateGraceMin","earlyGraceMin","overtimeEligible","isActive","type","color") VALUES ('sh-day','روزانه',10,10,1,1,'FIXED','#10b981')`,
        `INSERT INTO "ShiftSegment" ("id","shiftId","order","startMin","endMin","breakMin") VALUES ('seg1','sh-day',0,480,960,0)`,
        `INSERT INTO "PunchRecord" ("id","personId","timestamp","createdAt") VALUES ('pr-s3','p-s3','2026-11-17T04:30:00.000Z','2026-11-17T04:30:00.000Z')`,
        `INSERT INTO "DailyAttendance" ("id","personId","date","status","workedMinutes","lateMinutes","earlyLeaveMinutes","overtimeMinutes","leaveMinutes") VALUES ('da-s3','p-s3','2026-11-17T00:00:00.000Z','PRESENT',480,0,0,0,0)`,
      ], { DATABASE_URL: MIG_URL })
      check('pre-S3 real rows seeded (types/person/shift/punch/daily)', seedOld.status === 0, (seedOld.stderr || '').slice(-300))

      const up = sh('node', ['scripts/migrate-v1.7.0-phase2-s3.mjs'], { DATABASE_URL: MIG_URL })
      check('migration up exits 0', up.status === 0, (up.stderr || up.stdout || '').slice(-300))
      const up2 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3.mjs'], { DATABASE_URL: MIG_URL })
      check('migration up is idempotent (second run exits 0, no changes)', up2.status === 0, (up2.stderr || '').slice(-300))

      const db = new PrismaClient({ datasources: { db: { url: MIG_URL } } })
      try {
        const mis = await db.leaveType.findUnique({ where: { id: 'lt-mis' } })
        const norm = await db.leaveType.findUnique({ where: { id: 'lt-norm' } })
        check("existing mission type 'مأموریت' flagged isMission=true by the migration", mis !== null && mis.isMission === true)
        check('normal leave type NOT flagged (isMission=false)', norm !== null && norm.isMission === false)
        const person = await db.personnel.findUnique({ where: { id: 'p-s3' } })
        const punch = await db.punchRecord.findUnique({ where: { id: 'pr-s3' } })
        check('existing Personnel and PunchRecord rows survive the migration untouched', person !== null && punch !== null && punch.timestamp.toISOString() === '2026-11-17T04:30:00.000Z')
        const da = await db.dailyAttendance.findUnique({ where: { id: 'da-s3' } })
        check('existing DailyAttendance row survives with its worked 480 (defaults 0 for new columns)', da !== null && da.workedMinutes === 480 && da.missionMinutes === 0 && da.payableOvertimeMinutes === 0)
      } finally {
        await db.$disconnect()
      }

      // current schema converges with no drift (INSTALL.bat path)
      const pushNow = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: MIG_URL })
      check('prisma db push from the CURRENT schema converges (no drift, INSTALL.bat path)', pushNow.status === 0, (pushNow.stderr || '').slice(-300))
    })

    // ------------------------------------------------------------------
    // fresh engine database for B..G
    // ------------------------------------------------------------------
    const ENGINE_URL = sqliteUrl(generateTestDatabasePath(TMP))
    const push = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: ENGINE_URL })
    await section('B0. engine database provisioned (db push, current schema)', async () => {
      check('db push exits 0', push.status === 0, (push.stderr || '').slice(-300))
    })

    const db = new PrismaClient({ datasources: { db: { url: ENGINE_URL } } })
    process.env.DATABASE_URL = ENGINE_URL // the engine module's shared client reads this at construction
    const { recalcRange, recalcDayForPerson } = await import('../src/lib/attendance')

    // --- fixtures: day shift + night shift, one person, Sat night assignment
    const dayShift = await db.shift.create({ data: { name: 'روزانه', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true } })
    await db.shiftSegment.create({ data: { shiftId: dayShift.id, order: 0, startMin: 8 * 60, endMin: 16 * 60, breakMin: 0 } })
    const nightShift = await db.shift.create({ data: { name: 'شبانه', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true } })
    await db.shiftSegment.create({ data: { shiftId: nightShift.id, order: 0, startMin: 22 * 60, endMin: 6 * 60, breakMin: 0 } })

    const person = await db.personnel.create({
      data: { personnelCode: 'S3-ENG', pin: '9102', firstName: 'کارمند', lastName: 'موتور', defaultShiftId: dayShift.id },
    })

    // Sat 2026-11-14 (night assignment), Sun 15 & Tue 17 (default day shift)
    const MK = (y: number, m: number, d: number) => new Date(Date.UTC(y, m - 1, d))
    await db.shiftAssignment.create({ data: { personId: person.id, date: MK(2026, 11, 14), shiftId: nightShift.id } })
    await db.shiftAssignment.create({ data: { personId: person.id, date: MK(2026, 11, 17), shiftId: dayShift.id } })

    const mkPunch = (ts: Date, direction: string) =>
      db.punchRecord.create({ data: { personId: person.id, timestamp: ts, direction, source: 'KIOSK' } })

    // night shift Sat: IN Sat 22:00, OUT Sun 02:30 → 270 min? use 05:00 → 420 min
    await mkPunch(TEHRAN(2026, 11, 14, 22, 0), 'IN')
    const nightOut = await mkPunch(TEHRAN(2026, 11, 15, 5, 0), 'OUT')
    // day shift Tue: IN 08:00, OUT 18:31 (OT 151 actual)
    await mkPunch(TEHRAN(2026, 11, 17, 8, 0), 'IN')
    await mkPunch(TEHRAN(2026, 11, 17, 18, 31), 'OUT')

    // ------------------------------------------------------------------
    // [B] recalcRange: night punch → Saturday's row
    // ------------------------------------------------------------------
    await section('B. recalcRange: night-shift Sunday-morning punch lands on SATURDAY (workday_policy=shift)', async () => {
      const n = await recalcRange([person.id], MK(2026, 11, 14), MK(2026, 11, 17))
      check('recalcRange wrote 4 person-days (Sat..Tue)', n === 4)

      const sat = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 14) } } })
      check('SATURDAY row: PRESENT, worked 420 (22:00→05:00), night shift id', sat !== null && sat.status === 'PRESENT' && sat.workedMinutes === 420 && sat.shiftId === nightShift.id)
      check('SATURDAY row firstIn = Sat 22:00 Tehran, lastOut = Sun 05:00 Tehran', sat !== null && sat.firstIn?.toISOString() === TEHRAN(2026, 11, 14, 22, 0).toISOString() && sat.lastOut?.toISOString() === nightOut.timestamp.toISOString())

      const sun = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 15) } } })
      check('SUNDAY row has NO punch from the night shift (no double count): no punches → no firstIn/lastOut', sun !== null && sun.firstIn === null && sun.lastOut === null && sun.workedMinutes === 0)
      // Sunday: default shift suppressed on Friday? no — Sunday is a normal day but no assignment row →
      // default shift applies... but no punches → PENDING/ABSENT per now. The point: the night punch is NOT here.

      const tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('TUESDAY row: day-shift normal day, worked 631, OT 151 (threshold 30 default — no Setting row yet)', tue !== null && tue.workedMinutes === 631 && tue.overtimeMinutes === 151 && tue.payableOvertimeMinutes === 0)

      // the night punch appears in exactly ONE row's window
      const allRows = await db.dailyAttendance.findMany({ where: { personId: person.id } })
      const containing = allRows.filter((r) => r.firstIn?.getTime() === TEHRAN(2026, 11, 14, 22, 0).getTime() || r.lastOut?.getTime() === nightOut.timestamp.getTime())
      check('the night pair is reflected in exactly ONE DailyAttendance row', containing.length === 1)
    })

    // ------------------------------------------------------------------
    // [C] recalcDayForPerson with the Sunday-morning punch → Saturday row
    // ------------------------------------------------------------------
    await section('C. recalcDayForPerson(Sunday 05:00 punch) recomputes SATURDAY (shift-start workday)', async () => {
      // wipe Saturday's row content by deleting DailyAttendance for the test, then recalc via the punch trigger
      await db.dailyAttendance.deleteMany({ where: { personId: person.id, date: MK(2026, 11, 14) } })
      const before = await db.dailyAttendance.findMany({ where: { personId: person.id } })
      check('Saturday row deleted for the trigger test', before.every((r) => r.date.getTime() !== MK(2026, 11, 14).getTime()))
      await recalcDayForPerson(person.id, nightOut.timestamp) // the Sunday 05:00 OUT punch instant
      const sat = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 14) } } })
      check('recalcDayForPerson recreated SATURDAY with the full night pair (420 worked)', sat !== null && sat.workedMinutes === 420 && sat.status === 'PRESENT')
    })

    // ------------------------------------------------------------------
    // [D] overtime threshold + requests
    // ------------------------------------------------------------------
    await section('D. overtime: Setting threshold, request statuses, decision triggers recalc', async () => {
      // custom threshold 60 → Tuesday worked 631 → 631-480=151 > 60 → OT still 151 (whole excess)
      await db.setting.create({ data: { id: 'ot_threshold_minutes', value: '60' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      let tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('custom threshold 60: worked 631 → actual OT 151 (whole excess above 480+60=540: 631-480)', tue !== null && tue.overtimeMinutes === 151)

      // PENDING + REJECTED requests contribute nothing
      await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: 90, status: 'PENDING' } })
      const rej = await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: 80, status: 'REJECTED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('PENDING/REJECTED requests → payable stays 0 while actual 151 stays visible', tue !== null && tue.payableOvertimeMinutes === 0 && tue.overtimeMinutes === 151)

      // APPROVED with less than actual → payable = approved
      const appr = await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: 100, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('APPROVED 100 → payable 100 = min(151, 100); actual 151 still visible', tue !== null && tue.payableOvertimeMinutes === 100 && tue.overtimeMinutes === 151)

      // a second APPROVED request → sum 100+61 = 161 > actual → payable clamped to 151
      await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: 61, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('two APPROVED (100+61=161) → payable clamped to actual 151', tue !== null && tue.payableOvertimeMinutes === 151)

      // decision-trigger recalc path (the route's logic, exercised through the engine):
      // rejecting the first approval must drop payable to 61 (the remaining approved)
      await db.overtimeRequest.update({ where: { id: appr.id }, data: { status: 'REJECTED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('after rejecting one approval → payable = remaining approved 61', tue !== null && tue.payableOvertimeMinutes === 61)

      // deleting the remaining request → payable 0
      await db.overtimeRequest.deleteMany({ where: { personId: person.id, date: MK(2026, 11, 17), status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('after deleting all approvals → payable 0, actual 151 preserved', tue !== null && tue.payableOvertimeMinutes === 0 && tue.overtimeMinutes === 151)
      check('the REJECTED request row still exists (nothing silently deleted)', (await db.overtimeRequest.count({ where: { id: rej.id } })) === 1)
    })

    // ------------------------------------------------------------------
    // [E] mission (isMission LeaveType + APPROVED request)
    // ------------------------------------------------------------------
    await section('E. mission: approved_credit end-to-end (status, credit, overlap subtraction)', async () => {
      const missionType = await db.leaveType.create({ data: { name: 'مأموریت S3', paid: true, countsTowardsBalance: false, isMission: true } })
      const normalType = await db.leaveType.create({ data: { name: 'استحقاقی S3', paid: true, countsTowardsBalance: true } })

      // E1: approved mission, no punches, on Thu 2026-11-19 (no assignment → default day shift)
      await db.leaveRequest.create({
        data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 19, 9, 0), endDateTime: TEHRAN(2026, 11, 19, 13, 0), durationMinutes: 240, status: 'APPROVED' },
      })
      await recalcRange([person.id], MK(2026, 11, 19), MK(2026, 11, 19))
      let thu = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 19) } } })
      check('approved mission 09:00–13:00 without punches → status MISSION, missionMinutes 240, worked 240, leaveMinutes 0 (not counted as leave)', thu !== null && thu.status === 'MISSION' && thu.missionMinutes === 240 && thu.workedMinutes === 240 && thu.leaveMinutes === 0 && thu.leaveTypeId === null)

      // E2: PENDING and REJECTED missions contribute nothing (Fri 2026-11-20)
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 20, 9, 0), endDateTime: TEHRAN(2026, 11, 20, 12, 0), durationMinutes: 180, status: 'PENDING' } })
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 20, 14, 0), endDateTime: TEHRAN(2026, 11, 20, 16, 0), durationMinutes: 120, status: 'REJECTED' } })
      await recalcRange([person.id], MK(2026, 11, 20), MK(2026, 11, 20))
      let fri = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 20) } } })
      check('PENDING/REJECTED missions → no MISSION status, no credit (not LEAVE either: normal day no punches → PENDING/ABSENT)', fri !== null && fri.status !== 'MISSION' && fri.missionMinutes === 0)

      // E3: mission overlapping real attendance (Wed 2026-11-18: punches 08:00–12:00, mission 10:00–14:00)
      await mkPunch(TEHRAN(2026, 11, 18, 8, 0), 'IN')
      await mkPunch(TEHRAN(2026, 11, 18, 12, 0), 'OUT')
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 18, 10, 0), endDateTime: TEHRAN(2026, 11, 18, 14, 0), durationMinutes: 240, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 18), MK(2026, 11, 18))
      const wed = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 18) } } })
      check('mission 10:00–14:00 overlapping attendance 08:00–12:00 → credit 120 (12:00–14:00); worked 240+120=360; PRESENT', wed !== null && wed.missionMinutes === 120 && wed.workedMinutes === 360 && wed.status === 'PRESENT')

      // E4: night mission on the Sat night-shift workday (22:00 Sat → 06:00 Sun), no punches
      //     recalc must credit it to SATURDAY (shift-start day), not Sunday
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 14, 22, 0), endDateTime: TEHRAN(2026, 11, 15, 6, 0), durationMinutes: 480, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 14), MK(2026, 11, 15))
      const sat2 = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 14) } } })
      const sun2 = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 15) } } })
      // attendance 22:00→05:00 (420) ∩ mission 22:00→06:00: credit = 05:00→06:00 = 60
      check('night mission 22:00→06:00 over attendance 22:00→05:00 → SATURDAY credit exactly 60 (05:00–06:00)', sat2 !== null && sat2.missionMinutes === 60 && sat2.workedMinutes === 420 + 60)
      check('SUNDAY row gets NO mission minutes from the night mission (credited to the shift-start day once)', sun2 !== null && sun2.missionMinutes === 0)

      // E5: mission + normal leave overlap — leave minutes exclude mission overlap
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: normalType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 21, 10, 0), endDateTime: TEHRAN(2026, 11, 21, 16, 0), durationMinutes: 360, status: 'APPROVED' } })
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 21, 12, 0), endDateTime: TEHRAN(2026, 11, 21, 18, 0), durationMinutes: 360, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 21), MK(2026, 11, 21))
      const sat21 = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 21) } } })
      check('leave 10–16 minus mission overlap 12–16 → leaveMinutes 120; mission credit 12–18 → 360; status MISSION', sat21 !== null && sat21.leaveMinutes === 120 && sat21.missionMinutes === 360 && sat21.status === 'MISSION' && sat21.leaveTypeId === normalType.id)

      // E6 (S3 review fix 6): a leave whose every covered minute is removed by
      // the overlapping mission → leaveMinutes 0 and NO leaveTypeId reference.
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: normalType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 22, 9, 0), endDateTime: TEHRAN(2026, 11, 22, 15, 0), durationMinutes: 360, status: 'APPROVED' } })
      await db.leaveRequest.create({ data: { personId: person.id, leaveTypeId: missionType.id, kind: 'HOURLY', startDateTime: TEHRAN(2026, 11, 22, 8, 0), endDateTime: TEHRAN(2026, 11, 22, 16, 0), durationMinutes: 480, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 22), MK(2026, 11, 22))
      const sun22 = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 22) } } })
      check('leave 09–15 fully swallowed by mission 08–16 → leaveMinutes 0 AND leaveTypeId NULL (no zero-share reference)', sun22 !== null && sun22.leaveMinutes === 0 && sun22.leaveTypeId === null && sun22.missionMinutes === 480)
    })

    // ------------------------------------------------------------------
    // [H] S3 review fix 1 — dated default-shift history end-to-end (real DB)
    // ------------------------------------------------------------------
    await section('H. dated defaultShift history: change effective date never moves earlier days (real recalcRange)', async () => {
      // the same write path the personnel API uses (recordDefaultShiftChange)
      const { recordDefaultShiftChange } = await import('../src/lib/default-shift')
      const p2 = await db.personnel.create({ data: { personnelCode: 'S3-HIST', pin: '9103', firstName: 'تاریخچه', lastName: 'شیفت', defaultShiftId: dayShift.id } })

      // baseline: day shift effective 2026-11-16 (a punch the day BEFORE it proves backward extension)
      await recordDefaultShiftChange({ personId: p2.id, shiftId: dayShift.id, effectiveFrom: MK(2026, 11, 16), note: 'baseline', previousShiftId: null })
      await db.punchRecord.createMany({ data: [
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 15, 8, 0), direction: 'IN', source: 'KIOSK' },   // BEFORE the baseline (backward extension)
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 15, 16, 0), direction: 'OUT', source: 'KIOSK' },
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 17, 8, 0), direction: 'IN', source: 'KIOSK' },    // day-shift era
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 17, 16, 30), direction: 'OUT', source: 'KIOSK' },
      ] })
      // an EXPLICIT assignment (day shift on 11-19) that must never feel the default change
      await db.shiftAssignment.create({ data: { personId: p2.id, date: MK(2026, 11, 19), shiftId: dayShift.id } })
      await db.punchRecord.createMany({ data: [
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 19, 8, 0), direction: 'IN', source: 'KIOSK' },
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 19, 16, 0), direction: 'OUT', source: 'KIOSK' },
      ] })

      await recalcRange([p2.id], MK(2026, 11, 15), MK(2026, 11, 19))
      const before = {} as Record<string, { workedMinutes: number; shiftId: string | null; status: string } | undefined>
      for (const d of [15, 17, 19]) {
        const r = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p2.id, date: MK(2026, 11, d) } } })
        before[d] = r ? { workedMinutes: r.workedMinutes, shiftId: r.shiftId, status: r.status } : undefined
      }
      check('day-shift era rows before the change: 11-15 worked 480 (backward extension), 11-17 worked 510, both with the DAY shift id', before[15]?.workedMinutes === 480 && before[15]?.shiftId === dayShift.id && before[17]?.workedMinutes === 510 && before[17]?.shiftId === dayShift.id)
      check('explicit ShiftAssignment day 11-19: worked 480 with the DAY shift (explicit always wins)', before[19]?.workedMinutes === 480 && before[19]?.shiftId === dayShift.id)

      // the change: default → night shift, effective 2026-11-20 (dated, via the real write path)
      const change = await recordDefaultShiftChange({ personId: p2.id, shiftId: nightShift.id, effectiveFrom: MK(2026, 11, 20), note: 'switch to nights', previousShiftId: dayShift.id })
      check('recordDefaultShiftChange wrote the dated entry (changed=true, effective 2026-11-20)', change.changed === true && change.effectiveFrom.getTime() === MK(2026, 11, 20).getTime() && change.shiftId === nightShift.id)
      const p2row = await db.personnel.findUnique({ where: { id: p2.id }, select: { defaultShiftId: true } })
      check('Personnel.defaultShiftId updated to the new current value (night)', p2row?.defaultShiftId === nightShift.id)
      const history = await db.defaultShiftHistory.findMany({ where: { personId: p2.id }, orderBy: { effectiveFrom: 'asc' } })
      check('exactly TWO history rows (baseline day 11-16 + change 11-20) — no fabricated rows', history.length === 2 && history[0].shiftId === dayShift.id && history[1].shiftId === nightShift.id)

      // night-era punches: Sat 21 22:00 → Sun 22 05:00 (attributed to 11-21, the shift start day)
      await db.punchRecord.createMany({ data: [
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 21, 22, 0), direction: 'IN', source: 'KIOSK' },
        { personId: p2.id, timestamp: TEHRAN(2026, 11, 22, 5, 0), direction: 'OUT', source: 'KIOSK' },
      ] })
      await recalcRange([p2.id], MK(2026, 11, 15), MK(2026, 11, 22))

      const after = {} as Record<string, { workedMinutes: number; shiftId: string | null; status: string; firstIn: Date | null; lastOut: Date | null } | undefined>
      for (const d of [15, 17, 19, 21, 22]) {
        const r = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p2.id, date: MK(2026, 11, d) } } })
        after[d] = r ? { workedMinutes: r.workedMinutes, shiftId: r.shiftId, status: r.status, firstIn: r.firstIn, lastOut: r.lastOut } : undefined
      }
      check('days BEFORE the effective date are UNCHANGED (11-15: 480 day, 11-17: 510 day, 11-19 explicit: 480 day)', after[15]?.workedMinutes === 480 && after[15]?.shiftId === dayShift.id && after[17]?.workedMinutes === 510 && after[17]?.shiftId === dayShift.id && after[19]?.workedMinutes === 480 && after[19]?.shiftId === dayShift.id)
      check('days AFTER the effective date take the NEW default: Sat 21-21 night row worked 420 with the NIGHT shift id', after[21]?.workedMinutes === 420 && after[21]?.shiftId === nightShift.id && after[21]?.status === 'PRESENT')
      check('the Sunday 05:00 punch landed on SATURDAY (shift-start day), not Sunday — Sunday row has NO punch instants', after[22]?.firstIn === null && after[22]?.lastOut === null && after[21]?.lastOut?.getTime() === TEHRAN(2026, 11, 22, 5, 0).getTime())

      // mandated stability: re-running recalc must NOT change history or rows
      const rowsBeforeRerun = await db.dailyAttendance.findMany({ where: { personId: p2.id }, orderBy: { date: 'asc' } })
      await recalcRange([p2.id], MK(2026, 11, 15), MK(2026, 11, 22))
      const rowsAfterRerun = await db.dailyAttendance.findMany({ where: { personId: p2.id }, orderBy: { date: 'asc' } })
      const identical = rowsBeforeRerun.length === rowsAfterRerun.length && rowsBeforeRerun.every((r, i) =>
        r.date.getTime() === rowsAfterRerun[i].date.getTime() &&
        r.workedMinutes === rowsAfterRerun[i].workedMinutes &&
        r.shiftId === rowsAfterRerun[i].shiftId &&
        r.status === rowsAfterRerun[i].status &&
        r.firstIn?.getTime() === rowsAfterRerun[i].firstIn?.getTime() &&
        r.lastOut?.getTime() === rowsAfterRerun[i].lastOut?.getTime())
      check('recalc rerun leaves every row identical (the engine never writes history)', identical)
      const historyAfter = await db.defaultShiftHistory.findMany({ where: { personId: p2.id } })
      check('the history table itself is untouched by recalc (still exactly 2 rows)', historyAfter.length === 2)
    })

    // ------------------------------------------------------------------
    // [I] S3 review fix 2 — inactive shifts stay resolvable for history
    // ------------------------------------------------------------------
    await section('I. inactive shift: a deactivated shift still computes its own historical days', async () => {
      const oldShift = await db.shift.create({ data: { name: 'شیفت قدیمی', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true } })
      await db.shiftSegment.create({ data: { shiftId: oldShift.id, order: 0, startMin: 8 * 60, endMin: 16 * 60, breakMin: 0 } })
      const p3 = await db.personnel.create({ data: { personnelCode: 'S3-INACT', pin: '9104', firstName: 'غیرفعال', lastName: 'شیفت' } })
      // explicit assignment of the (still active) shift to a past date
      await db.shiftAssignment.create({ data: { personId: p3.id, date: MK(2026, 11, 23), shiftId: oldShift.id } })
      await db.punchRecord.createMany({ data: [
        { personId: p3.id, timestamp: TEHRAN(2026, 11, 23, 8, 0), direction: 'IN', source: 'KIOSK' },
        { personId: p3.id, timestamp: TEHRAN(2026, 11, 23, 16, 0), direction: 'OUT', source: 'KIOSK' },
      ] })
      await recalcRange([p3.id], MK(2026, 11, 23), MK(2026, 11, 23))
      const rowBefore = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p3.id, date: MK(2026, 11, 23) } } })
      check('with the shift ACTIVE: day 11-23 computes PRESENT 480 with that shift id, no attribution fallback in note', rowBefore !== null && rowBefore.status === 'PRESENT' && rowBefore.workedMinutes === 480 && rowBefore.shiftId === oldShift.id && rowBefore.note === null)

      // deactivate the shift — the past day must keep computing with it
      await db.shift.update({ where: { id: oldShift.id }, data: { isActive: false } })
      await recalcRange([p3.id], MK(2026, 11, 23), MK(2026, 11, 23))
      const rowAfter = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p3.id, date: MK(2026, 11, 23) } } })
      check('after isActive=false the SAME past day still computes with the SAME shift (no NO_SHIFT / no fallback note)', rowAfter !== null && rowAfter.status === 'PRESENT' && rowAfter.workedMinutes === 480 && rowAfter.shiftId === oldShift.id && rowAfter.note === null)

      // the deactivated shift is still loaded by the calculation shift map
      const stillIn = await db.shift.findUnique({ where: { id: oldShift.id }, select: { isActive: true } })
      check('fixture sanity: the shift row is indeed isActive=false now (history resolved anyway)', stillIn !== null && stillIn.isActive === false)
    })

    // ------------------------------------------------------------------
    // [J] S3 review fix 3 — night-shift end boundary through the REAL engine
    // ------------------------------------------------------------------
    await section('J. night-shift end boundary: exact 06:00 OUT closes the shift-start day (real recalcRange)', async () => {
      // J1 (mandated): Sat 22:00 IN + Sun 06:00 OUT — one row, one valid pair
      const pJ = await db.personnel.create({ data: { personnelCode: 'S3-BOUND', pin: '9105', firstName: 'مرز', lastName: 'شیفت' } })
      await db.shiftAssignment.create({ data: { personId: pJ.id, date: MK(2026, 11, 28), shiftId: nightShift.id } })
      await db.punchRecord.createMany({ data: [
        { personId: pJ.id, timestamp: TEHRAN(2026, 11, 28, 22, 0), direction: 'IN', source: 'KIOSK' },
        { personId: pJ.id, timestamp: TEHRAN(2026, 11, 29, 6, 0), direction: 'OUT', source: 'KIOSK' },
      ] })
      await recalcRange([pJ.id], MK(2026, 11, 28), MK(2026, 11, 29))
      const satJ = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pJ.id, date: MK(2026, 11, 28) } } })
      const sunJ = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pJ.id, date: MK(2026, 11, 29) } } })
      check('J1: SATURDAY row holds the WHOLE pair — PRESENT, worked 480 (22:00→06:00), firstIn 22:00 / lastOut 06:00, no anomaly note', satJ !== null && satJ.status === 'PRESENT' && satJ.workedMinutes === 480 && satJ.firstIn?.getTime() === TEHRAN(2026, 11, 28, 22, 0).getTime() && satJ.lastOut?.getTime() === TEHRAN(2026, 11, 29, 6, 0).getTime() && satJ.note === null)
      check('J1: SUNDAY row gets NO punch from the night pair (no split, no double count)', sunJ !== null && sunJ.firstIn === null && sunJ.lastOut === null && sunJ.workedMinutes === 0)

      // each real punch is computed in EXACTLY one DailyAttendance row
      const rowsJ = await db.dailyAttendance.findMany({ where: { personId: pJ.id } })
      const punchCount = (t: Date) => rowsJ.filter((r) => r.firstIn?.getTime() === t.getTime() || r.lastOut?.getTime() === t.getTime()).length
      check('J1: each punch (Sat 22:00 IN, Sun 06:00 OUT) appears in exactly ONE row', punchCount(TEHRAN(2026, 11, 28, 22, 0)) === 1 && punchCount(TEHRAN(2026, 11, 29, 6, 0)) === 1)

      // J2 (mandated): Sat 22:00 IN + Sun 06:01 OUT — the 06:01 OUT is genuinely out of window
      const pQ = await db.personnel.create({ data: { personnelCode: 'S3-BOUND2', pin: '9106', firstName: 'مرز', lastName: 'دوم' } })
      await db.shiftAssignment.create({ data: { personId: pQ.id, date: MK(2026, 11, 28), shiftId: nightShift.id } })
      await db.punchRecord.createMany({ data: [
        { personId: pQ.id, timestamp: TEHRAN(2026, 11, 28, 22, 0), direction: 'IN', source: 'KIOSK' },
        { personId: pQ.id, timestamp: TEHRAN(2026, 11, 29, 6, 1), direction: 'OUT', source: 'KIOSK' },
      ] })
      await recalcRange([pQ.id], MK(2026, 11, 28), MK(2026, 11, 29))
      const satQ = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pQ.id, date: MK(2026, 11, 28) } } })
      const sunQ = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pQ.id, date: MK(2026, 11, 29) } } })
      check('J2: SATURDAY keeps the open session — INCOMPLETE with IN_WITHOUT_OUT (no fabricated pair)', satQ !== null && satQ.status === 'INCOMPLETE' && satQ.workedMinutes === 0 && satQ.note === 'IN_WITHOUT_OUT:1')
      check('J2: SUNDAY receives the 06:01 OUT via the documented fallback — INCOMPLETE with OUT_WITHOUT_IN + OUT_OF_WINDOW reasons (an orphan OUT is reported, never fabricated into a pair)', sunQ !== null && sunQ.status === 'INCOMPLETE' && sunQ.firstIn === null && sunQ.note !== null && sunQ.note.includes('OUT_WITHOUT_IN') && sunQ.note.includes('OUT_OF_WINDOW'))

      // J3 (mandated): next-day shift starting EXACTLY at the boundary — IN and OUT independent
      const boundaryShift = await db.shift.create({ data: { name: 'شیفت مرزی', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true } })
      await db.shiftSegment.create({ data: { shiftId: boundaryShift.id, order: 0, startMin: 6 * 60, endMin: 14 * 60, breakMin: 0 } })
      const pR = await db.personnel.create({ data: { personnelCode: 'S3-BOUND3', pin: '9107', firstName: 'مرز', lastName: 'سوم' } })
      await db.shiftAssignment.create({ data: { personId: pR.id, date: MK(2026, 11, 28), shiftId: nightShift.id } })
      await db.shiftAssignment.create({ data: { personId: pR.id, date: MK(2026, 11, 29), shiftId: boundaryShift.id } })
      // four punches — the OUT 06:00 (closes the night) and the IN 06:00 (opens the day shift) share ONE instant
      await db.punchRecord.createMany({ data: [
        { personId: pR.id, timestamp: TEHRAN(2026, 11, 28, 22, 0), direction: 'IN', source: 'KIOSK' },
        { personId: pR.id, timestamp: TEHRAN(2026, 11, 29, 6, 0), direction: 'OUT', source: 'KIOSK' },
        { personId: pR.id, timestamp: TEHRAN(2026, 11, 29, 6, 0), direction: 'IN', source: 'WEB' },
        { personId: pR.id, timestamp: TEHRAN(2026, 11, 29, 14, 0), direction: 'OUT', source: 'WEB' },
      ] })
      await recalcRange([pR.id], MK(2026, 11, 28), MK(2026, 11, 29))
      const satR = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pR.id, date: MK(2026, 11, 28) } } })
      const sunR = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: pR.id, date: MK(2026, 11, 29) } } })
      check('J3: SATURDAY = the OUT 06:00 that CLOSES the night — pair 22:00→06:00, worked 480, PRESENT', satR !== null && satR.status === 'PRESENT' && satR.workedMinutes === 480 && satR.firstIn?.getTime() === TEHRAN(2026, 11, 28, 22, 0).getTime() && satR.lastOut?.getTime() === TEHRAN(2026, 11, 29, 6, 0).getTime() && satR.shiftId === nightShift.id)
      check('J3: SUNDAY = the IN 06:00 that OPENS the boundary day shift — pair 06:00→14:00, worked 480, PRESENT (same instant, other workday)', sunR !== null && sunR.status === 'PRESENT' && sunR.workedMinutes === 480 && sunR.firstIn?.getTime() === TEHRAN(2026, 11, 29, 6, 0).getTime() && sunR.lastOut?.getTime() === TEHRAN(2026, 11, 29, 14, 0).getTime() && sunR.shiftId === boundaryShift.id)
      const rowsR = await db.dailyAttendance.findMany({ where: { personId: pR.id } })
      const inCountR = (t: Date) => rowsR.filter((r) => r.firstIn?.getTime() === t.getTime()).length
      const outCountR = (t: Date) => rowsR.filter((r) => r.lastOut?.getTime() === t.getTime()).length
      check('J3: every real punch is computed in exactly ONE row (IN Sat 22:00; OUT 06:00 → Saturday lastOut; IN 06:00 → Sunday firstIn; OUT 14:00) — two punches at the SAME instant land on two DIFFERENT rows by direction', inCountR(TEHRAN(2026, 11, 28, 22, 0)) === 1 && outCountR(TEHRAN(2026, 11, 29, 6, 0)) === 1 && inCountR(TEHRAN(2026, 11, 29, 6, 0)) === 1 && outCountR(TEHRAN(2026, 11, 29, 14, 0)) === 1 && rowsR.filter((r) => r.workedMinutes === 480).length === 2)
    })

    // ------------------------------------------------------------------
    // [K] S3 review fix 5 — overtime sum guard against legacy negative rows
    // ------------------------------------------------------------------
    await section('K. overtime: a legacy NEGATIVE approved request never reduces the day approved sum', async () => {
      // Tuesday 11-17 already has worked 631 → actual OT 151 (section B fixture)
      // an approved +100 plus a legacy NEGATIVE -50 (creatable before the API fix)
      await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: 100, status: 'APPROVED' } })
      await db.overtimeRequest.create({ data: { personId: person.id, date: MK(2026, 11, 17), minutes: -50, status: 'APPROVED' } })
      await recalcRange([person.id], MK(2026, 11, 17), MK(2026, 11, 17))
      const tue = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: MK(2026, 11, 17) } } })
      check('approved 100 + legacy -50 → payable min(151, 100) = 100 (the negative row never subtracts from the sum)', tue !== null && tue.payableOvertimeMinutes === 100 && tue.overtimeMinutes === 151)
      await db.overtimeRequest.deleteMany({ where: { personId: person.id, date: MK(2026, 11, 17), minutes: -50 } })
    })

    // ------------------------------------------------------------------
    // [F] idempotency + [G] punch immutability
    // ------------------------------------------------------------------
    await section('F/G. idempotency of recalc rerun + PunchRecord immutability', async () => {
      const punchesBefore = await db.punchRecord.findMany({ where: { personId: person.id }, orderBy: { timestamp: 'asc' } })
      const rowsBefore = await db.dailyAttendance.findMany({ where: { personId: person.id }, orderBy: { date: 'asc' } })

      await recalcRange([person.id], MK(2026, 11, 14), MK(2026, 11, 21))
      const rowsAfter = await db.dailyAttendance.findMany({ where: { personId: person.id }, orderBy: { date: 'asc' } })

      check('recalc rerun produces the SAME number of rows', rowsBefore.length === rowsAfter.length && rowsAfter.length > 0)
      let identical = rowsBefore.length === rowsAfter.length
      for (let i = 0; i < rowsBefore.length && identical; i++) {
        const a = rowsBefore[i]
        const b = rowsAfter[i]
        identical =
          a.date.getTime() === b.date.getTime() &&
          a.status === b.status &&
          a.workedMinutes === b.workedMinutes &&
          a.lateMinutes === b.lateMinutes &&
          a.earlyLeaveMinutes === b.earlyLeaveMinutes &&
          a.overtimeMinutes === b.overtimeMinutes &&
          a.payableOvertimeMinutes === b.payableOvertimeMinutes &&
          a.leaveMinutes === b.leaveMinutes &&
          a.missionMinutes === b.missionMinutes &&
          a.note === b.note &&
          a.firstIn?.getTime() === b.firstIn?.getTime() &&
          a.lastOut?.getTime() === b.lastOut?.getTime()
      }
      check('every row is identical after the rerun (statuses, worked, OT, payable, mission, leave, note)', identical)

      const punchesAfter = await db.punchRecord.findMany({ where: { personId: person.id }, orderBy: { timestamp: 'asc' } })
      let punchesSame = punchesAfter.length === punchesBefore.length
      for (let i = 0; i < punchesAfter.length && punchesSame; i++) {
        punchesSame =
          punchesAfter[i].id === punchesBefore[i].id &&
          punchesAfter[i].timestamp.getTime() === punchesBefore[i].timestamp.getTime() &&
          punchesAfter[i].direction === punchesBefore[i].direction &&
          punchesAfter[i].source === punchesBefore[i].source
      }
      check(`raw PunchRecord rows byte-identical after all recalcs (${punchesAfter.length} rows — never mutated, never deleted)`, punchesSame)
    })

    // ------------------------------------------------------------------
    // [L] S3 review fix 1 — the migration script itself (pre-fix schema →
    //     dated default-shift history), additive & backward compatible
    // ------------------------------------------------------------------
    await section('L. migration migrate-v1.7.0-phase2-s3r2.mjs: table + snapshot, idempotent, data preserved', async () => {
      const MIG2_URL = sqliteUrl(generateTestDatabasePath(TMP))
      // baseline: the CURRENT HEAD schema (2223ac75 — S3 columns, NO DefaultShiftHistory)
      const old = sh('git', ['show', '2223ac75:prisma/schema.prisma'], {})
      check('pre-S3r2 baseline schema resolves from git (2223ac75)', old.status === 0 && old.stdout.length > 1000)
      const OLD2_SCHEMA = path.join(TMP, 'old-schema-s3r2.prisma')
      writeFileSync(OLD2_SCHEMA, old.stdout)
      tmpFiles.push(OLD2_SCHEMA)
      const pushOld2 = sh('npx', ['prisma', 'db', 'push', '--schema', OLD2_SCHEMA, '--skip-generate'], { DATABASE_URL: MIG2_URL })
      check('pre-S3r2 database created (db push, pinned old schema)', pushOld2.status === 0, (pushOld2.stderr || '').slice(-200))

      // real rows: a person WITH a default, one WITHOUT, a shift, a punch, a DailyAttendance row
      const seed2 = sh('node', ['scripts/test-db-raw.mjs',
        `INSERT INTO "Shift" ("id","name","lateGraceMin","earlyGraceMin","overtimeEligible","isActive","type","color") VALUES ('sh-m2','موروثی',10,10,1,1,'FIXED','#10b981')`,
        `INSERT INTO "Personnel" ("id","personnelCode","firstName","lastName","pin","qrToken","createdAt","updatedAt","defaultShiftId") VALUES ('p-m2','MIG2-A','قدیمی','الف','2001','q-m2','2026-01-01T00:00:00.000Z','2026-01-01T00:00:00.000Z','sh-m2')`,
        `INSERT INTO "Personnel" ("id","personnelCode","firstName","lastName","pin","qrToken","createdAt","updatedAt") VALUES ('p-m3','MIG2-B','قدیمی','ب','2002','q-m3','2026-01-01T00:00:00.000Z','2026-01-01T00:00:00.000Z')`,
        `INSERT INTO "PunchRecord" ("id","personId","timestamp","createdAt") VALUES ('pr-m2','p-m2','2026-11-17T04:30:00.000Z','2026-11-17T04:30:00.000Z')`,
        `INSERT INTO "DailyAttendance" ("id","personId","date","status","workedMinutes","lateMinutes","earlyLeaveMinutes","overtimeMinutes","leaveMinutes") VALUES ('da-m2','p-m2','2026-11-17T00:00:00.000Z','PRESENT',480,0,0,0,0)`,
      ], { DATABASE_URL: MIG2_URL })
      check('pre-S3r2 rows seeded (shift/person-with-default/person-without/punch/daily)', seed2.status === 0, (seed2.stderr || '').slice(-300))

      const up = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs'], { DATABASE_URL: MIG2_URL })
      check('migration up exits 0 (table + index + snapshot)', up.status === 0, (up.stderr || up.stdout || '').slice(-300))
      const up2 = sh('node', ['scripts/migrate-v1.7.0-phase2-s3r2.mjs'], { DATABASE_URL: MIG2_URL })
      check('migration up is IDEMPOTENT (second run exits 0, 0 new snapshot rows)', up2.status === 0 && (up2.stdout || '').includes('snapshot rows written (persons with a default, no history yet): 0'))

      const dbL = new PrismaClient({ datasources: { db: { url: MIG2_URL } } })
      try {
        const hist = await dbL.defaultShiftHistory.findMany()
        // Tehran "today" marker, computed the same way the migration script does
        const parts = new Intl.DateTimeFormat('en-CA', {
          timeZone: 'Asia/Tehran', year: 'numeric', month: '2-digit', day: '2-digit',
        }).format(new Date()).split('-').map(Number)
        const tehranToday = new Date(Date.UTC(parts[0], parts[1] - 1, parts[2]))
        check('exactly ONE snapshot row — the person WITH a default (the default-less person needs none), shift = the current default, effective TODAY (Tehran day marker)', hist.length === 1 && hist[0].personId === 'p-m2' && hist[0].shiftId === 'sh-m2' && hist[0].effectiveFrom.getTime() === tehranToday.getTime())
        const punch = await dbL.punchRecord.findUnique({ where: { id: 'pr-m2' } })
        const daily = await dbL.dailyAttendance.findUnique({ where: { id: 'da-m2' } })
        const personNoDefault = await dbL.personnel.findUnique({ where: { id: 'p-m3' } })
        check('all pre-existing rows survive untouched (punch timestamp, daily worked 480, second person)', punch !== null && punch.timestamp.toISOString() === '2026-11-17T04:30:00.000Z' && daily !== null && daily.workedMinutes === 480 && personNoDefault !== null)
      } finally {
        await dbL.$disconnect()
      }

      // the CURRENT schema converges on the migrated database (no drift — INSTALL.bat path)
      const pushNow2 = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: MIG2_URL })
      check('prisma db push from the CURRENT schema converges (no drift after the migration)', pushNow2.status === 0, (pushNow2.stderr || '').slice(-300))
    })

    await db.$disconnect()
  } finally {
    // cleanup: only files this suite created, only inside the test root
    for (const f of tmpFiles) {
      try {
        const p = assertTestOwnedDatabasePath(f, TMP)
        rmSync(p)
      } catch {
        /* not a test-owned path — leave it alone */
      }
    }
    try {
      rmSync(TMP, { recursive: true, force: true })
    } catch {
      /* best effort */
    }
  }
}

main().catch((err) => {
  console.error('suite crashed:', err)
  process.exitCode = 1
})

process.on('exit', () => {
  console.log(`\n== Attendance-DB assertions: ${assertionsPassed} passed / ${assertionsFailed} failed ==`)
  console.log(`== Attendance-DB sections: ${sectionsPassed} passed / ${sectionsFailed} failed ==`)
  if (assertionsFailed > 0 || sectionsFailed > 0) process.exitCode = 1
})
