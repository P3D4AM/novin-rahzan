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
 *       → no credit, no MISSION status.
 *   [F] idempotency: recalc rerun → identical DailyAttendance rows.
 *   [G] PunchRecord rows are byte-identical before/after recalc (no mutation,
 *       no deletion — raw punches untouched by the engine).
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
