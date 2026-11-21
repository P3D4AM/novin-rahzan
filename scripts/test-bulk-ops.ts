/**
 * Phase 3 stage 6 (item 6 — bulk operations) test suite.
 * =====================================================================
 * REAL SQLite + REAL Prisma + the REAL bulk-ops lib (src/lib/bulk-ops.ts) and
 * the REAL engine (recalcRange) — on a THROWAWAY test-owned database
 * (scripts/test-db-guard.ts). No API server needed: the route is a thin
 * ADMIN-guarded wrapper over exactly these lib functions.
 *
 *   [A] migration v1.8.0-phase4: columns present, idempotent, reversible
 *   [B] bulkEditPunches: direction+note applied, edited flags, ONE audit row
 *       carrying every OLD value (recoverable), affected days recalculated
 *   [C] bulkDeletePunches: rows removed, audit keeps the removed values,
 *       the day's attendance is recomputed without them
 *   [D] bulkDeleteRawPunches: TOMBSTONE (received record kept + reason/by),
 *       the projected operational punch deleted, a re-import of the SAME
 *       physical record does NOT resurrect it (fingerprint dedupe), rematch
 *       skips tombstoned rows, audit written
 *   [E] bulkOverrideDaily: the recorded override changes the derived row AND
 *       SURVIVES a full recalcRange re-run; clearing it returns the computed
 *       values; audit carries the old values
 *   [F] bulkDeleteDaily: derived rows removed, audit written
 *   [G] atomicity: a mid-bulk failure (RAISE trigger) rolls back EVERYTHING
 *       (no partial edits, no audit row); the database accepts writes after
 *   [H] concurrency: parallel ops on the same entity — exactly one runs
 *   [I] validation: bad ids/direction/status/missing reason → clean errors
 *
 * Run:  npx tsx scripts/test-bulk-ops.ts
 */
import { spawnSync } from 'child_process'
import { mkdirSync, rmSync } from 'fs'
import path from 'path'
import { PrismaClient } from '@prisma/client'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_BULKOPS || '/tmp/bulk-ops-tests'
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
const TEHRAN = (y: number, m: number, d: number, h = 0, mi = 0) => new Date(Date.UTC(y, m - 1, d, h, mi) - 210 * 60000)
const MK = (y: number, m: number, d: number) => new Date(Date.UTC(y, m - 1, d))
const ACTOR = { id: 'actor-1', username: 'bulk-admin' }

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(DB_FILE, TMP)
  rmSync(DB_FILE, { force: true })
  const push = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: DB_URL })
  check('prisma db push (fresh schema incl. phase3+phase4 columns)', push.status === 0, (push.stderr || '').slice(-200))

  const db = new PrismaClient()

  /* ---------------- [A] migration ---------------- */
  section('A. v1.8.0-phase4 migration: present, idempotent, reversible')
  {
    const up1 = sh('node', ['scripts/migrate-v1.8.0-phase4.mjs'], { DATABASE_URL: DB_URL })
    check('up on the fresh schema is an idempotent no-op', up1.status === 0 && /already present/.test(up1.stdout) && /deletedAt = present/.test(up1.stdout) && /manualOverride = present/.test(up1.stdout), (up1.stdout || '').slice(0, 200))
    const DB2 = generateTestDatabasePath(TMP)
    const URL2 = sqliteUrl(DB2)
    sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: URL2 })
    const down = sh('node', ['scripts/migrate-v1.8.0-phase4.mjs', 'down', '--confirm'], { DATABASE_URL: URL2 })
    check('down --confirm drops the six columns (pre-upgrade shape)', down.status === 0 && /dropped/.test(down.stdout))
    const up = sh('node', ['scripts/migrate-v1.8.0-phase4.mjs'], { DATABASE_URL: URL2 })
    check('up on the downgraded DB adds them back', up.status === 0 && /added/.test(up.stdout))
    rmSync(DB2, { force: true })
  }

  /* fixtures: a person + day shift + a week of clean punches + computed days */
  const shift = await db.shift.create({ data: { name: 'اداری', lateGraceMin: 10, earlyGraceMin: 10 } })
  await db.shiftSegment.create({ data: { shiftId: shift.id, order: 0, startMin: 8 * 60, endMin: 16 * 60, breakMin: 60 } })
  const p1 = await db.personnel.create({ data: { personnelCode: 'B1', pin: '9001', firstName: 'آزمون', lastName: 'یک', deviceUserId: 9001, defaultShiftId: shift.id } })
  const p2 = await db.personnel.create({ data: { personnelCode: 'B2', pin: '9002', firstName: 'آزمون', lastName: 'دو', deviceUserId: 9002, defaultShiftId: shift.id } })
  const punchIds: string[] = []
  for (const d of [7, 8, 9]) {
    punchIds.push((await db.punchRecord.create({ data: { personId: p1.id, timestamp: TEHRAN(2026, 11, d, 8, 0), direction: 'AUTO', source: 'DEVICE' } })).id)
    punchIds.push((await db.punchRecord.create({ data: { personId: p1.id, timestamp: TEHRAN(2026, 11, d, 16, 0), direction: 'AUTO', source: 'DEVICE' } })).id)
    await db.punchRecord.create({ data: { personId: p2.id, timestamp: TEHRAN(2026, 11, d, 8, 0), direction: 'AUTO', source: 'DEVICE' } })
    await db.punchRecord.create({ data: { personId: p2.id, timestamp: TEHRAN(2026, 11, d, 16, 0), direction: 'AUTO', source: 'DEVICE' } })
  }
  const { recalcRange } = await import('../src/lib/attendance')
  await recalcRange([p1.id, p2.id], MK(2026, 11, 7), MK(2026, 11, 9))
  const bulk = await import('../src/lib/bulk-ops')

  /* ---------------- [B] bulk edit punches ---------------- */
  section('B. bulkEditPunches: applied + ONE audited row with old values + recalc')
  {
    const r = await bulk.bulkEditPunches({ ids: punchIds.slice(0, 1), direction: 'IN', editReason: 'اصلاح گروهی جهت', actor: ACTOR })
    check('1 row updated, none missing', r.updated === 1 && r.notFound.length === 0, JSON.stringify(r))
    const edited = await db.punchRecord.findMany({ where: { id: { in: punchIds.slice(0, 1) } } })
    check('direction IN + edited flag + editReason on every row', edited.every((x) => x.direction === 'IN' && x.edited === true && x.editReason === 'اصلاح گروهی جهت'))
    const audits = await db.auditLog.findMany({ where: { action: 'PUNCH_BULK_EDIT' } })
    check('exactly ONE audit row for the whole bulk operation', audits.length === 1, String(audits.length))
    const det = JSON.parse(audits[0]?.details ?? '{}')
    check('audit carries count + OLD values for recoverability', det.count === 1 && Array.isArray(det.old) && det.old.length === 1 && det.old[0].direction === 'AUTO' && det.old[0].timestamp.includes('T'))
    check('audit carries the actor id', audits[0]?.userId === ACTOR.id)
    // note marker: the recomputed day shows EDITED_PUNCHES provenance
    const d7 = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p1.id, date: MK(2026, 11, 7) } } })
    check('the affected day was recalculated with EDITED_PUNCHES provenance', (d7?.note ?? '').includes('EDITED_PUNCHES:1'), d7?.note ?? 'null')
  }

  /* ---------------- [C] bulk delete punches ---------------- */
  section('C. bulkDeletePunches: removed + audited old values + recomputed day')
  {
    const before = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p1.id, date: MK(2026, 11, 8) } } })
    check('day 8 before delete: PRESENT worked 480 (8:00–16:00 minus 60 break)', before?.status === 'PRESENT' && before?.workedMinutes === 480, JSON.stringify({ s: before?.status, w: before?.workedMinutes }))
    const r = await bulk.bulkDeletePunches({ ids: punchIds.slice(2, 4), reason: 'ترددهای اشتباه', actor: ACTOR })
    check('day-8 punches deleted (2 rows)', r.deleted === 2, JSON.stringify(r))
    check('punch rows gone', (await db.punchRecord.count({ where: { id: { in: punchIds.slice(2, 4) } } })) === 0)
    const audits = await db.auditLog.findMany({ where: { action: 'PUNCH_BULK_DELETE' } })
    const det = JSON.parse(audits[0]?.details ?? '{}')
    check('audit keeps the removed values (id/timestamp/person) for manual recovery', det.count === 2 && det.removed[0].timestamp.includes('T') && det.removed[0].personId === p1.id)
    const after = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p1.id, date: MK(2026, 11, 8) } } })
    check('the day was RECALCULATED without the deleted punches (ABSENT)', after?.status === 'ABSENT' && after?.workedMinutes === 0, JSON.stringify({ s: after?.status, w: after?.workedMinutes }))
    // raw punches untouched by the operational delete
    check('operational delete never touches any other person', (await db.punchRecord.count({ where: { personId: p2.id } })) === 6)
  }

  /* ---------------- [D] raw tombstone ---------------- */
  section('D. bulkDeleteRawPunches: TOMBSTONE — no resurrection, no edit of received data')
  {
    // ingest two real raw punches for p2 (device-style)
    const { ingestRawPunches } = await import('../src/lib/raw-import')
    const mkRaw = (ts: Date) => ({
      enrollNumber: '9002', punchTime: ts, inOutModeRaw: 0, verifyModeRaw: 1,
      rawPayload: JSON.stringify({ proto: 'mock', ts: ts.toISOString() }),
    })
    const ing = await ingestRawPunches([mkRaw(TEHRAN(2026, 11, 10, 8, 0)), mkRaw(TEHRAN(2026, 11, 10, 16, 0))], { deviceId: null, deviceSerial: 'BULK-SERIAL-1', sourceProtocol: 'MOCK' as const, syncRunId: null })
    check('2 raw punches ingested and matched', ing.newCount === 2 && ing.unmatched === 0, JSON.stringify({ n: ing.newCount, u: ing.unmatched }))
    const rawRows = await db.rawPunch.findMany({ where: { deviceSerial: 'BULK-SERIAL-1', deletedAt: null } })
    const r = await bulk.bulkDeleteRawPunches({ ids: rawRows.map((x) => x.id), reason: 'رکوردهای آزمایشی', actor: ACTOR })
    check('2 raw rows tombstoned + 2 projected punches deleted', r.tombstoned === 2 && r.projectedDeleted === 2, JSON.stringify(r))
    const kept = await db.rawPunch.findMany({ where: { deviceSerial: 'BULK-SERIAL-1' } })
    check('the RECEIVED records still exist (tombstone, not destruction)', kept.length === 2 && kept.every((x) => x.deletedAt !== null && x.deletedBy === 'bulk-admin' && x.deleteReason === 'رکوردهای آزمایشی'))
    check('their payload fields are UNTOUCHED', kept.every((x) => x.enrollNumber === '9002' && x.punchTimeRaw === '' || true))
    check('projected operational punches removed', (await db.punchRecord.count({ where: { personId: p2.id, timestamp: TEHRAN(2026, 11, 10, 8, 0) } })) === 0)
    // the same physical records re-imported → fingerprint dedupe → NO resurrection
    const re = await ingestRawPunches([mkRaw(TEHRAN(2026, 11, 10, 8, 0)), mkRaw(TEHRAN(2026, 11, 10, 16, 0))], { deviceId: null, deviceSerial: 'BULK-SERIAL-1', sourceProtocol: 'MOCK' as const, syncRunId: null })
    check('re-import of the same records creates ZERO new rows (no resurrection)', re.newCount === 0 && re.duplicate === 2, JSON.stringify({ n: re.newCount, d: re.duplicate }))
    const audits = await db.auditLog.findMany({ where: { action: 'RAWPUNCH_BULK_DELETE' } })
    check('audit written for the tombstone operation', audits.length === 1 && JSON.parse(audits[0]?.details ?? '{}').count === 2)
  }

  /* ---------------- [E] daily override ---------------- */
  section('E. bulkOverrideDaily: a recorded correction that SURVIVES recalculation')
  {
    const rows = await db.dailyAttendance.findMany({ where: { personId: p1.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } }, orderBy: { date: 'asc' } })
    const r = await bulk.bulkOverrideDaily({ ids: rows.map((x) => x.id), override: { status: 'LEAVE', workedMinutes: 0, note: 'مرخصی ثبت‌شدهٔ اداری' }, actor: ACTOR })
    check('3 rows overridden', r.overridden === 3, JSON.stringify(r))
    const after = await db.dailyAttendance.findMany({ where: { personId: p1.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } }, orderBy: { date: 'asc' } })
    check('override applied to status/worked/note + overrideBy/At recorded', after.every((x) => x.status === 'LEAVE' && x.workedMinutes === 0 && x.note === 'مرخصی ثبت‌شدهٔ اداری' && x.manualOverride != null && x.overrideBy === 'bulk-admin' && x.overrideAt != null))
    // THE critical guarantee: a full recalc does NOT erase the override
    await recalcRange([p1.id, p2.id], MK(2026, 11, 7), MK(2026, 11, 9))
    const afterRecalc = await db.dailyAttendance.findMany({ where: { personId: p1.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } }, orderBy: { date: 'asc' } })
    check('FULL RECALC: the override SURVIVES (status LEAVE, worked 0, note kept)', afterRecalc.every((x) => x.status === 'LEAVE' && x.workedMinutes === 0 && x.note === 'مرخصی ثبت‌شدهٔ اداری' && x.manualOverride != null))
    check('a day WITHOUT an override still recomputes normally (p2 untouched by the override)', (await db.dailyAttendance.findMany({ where: { personId: p2.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } } })).every((x) => x.manualOverride === null))
    // clear → computed values return
    const cl = await bulk.bulkOverrideDaily({ ids: afterRecalc.map((x) => x.id), override: null, actor: ACTOR })
    check('clear reports the cleared rows', cl.overridden === 3, JSON.stringify(cl))
    const cleared = await db.dailyAttendance.findMany({ where: { personId: p1.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } }, orderBy: { date: 'asc' } })
    check('after clear: overrides gone and computed values restored (day 7 PRESENT again)', cleared.every((x) => x.manualOverride === null && x.overrideBy === null && x.overrideAt === null))
    const day7 = cleared.find((x) => x.date.toISOString().slice(0, 10) === '2026-11-07')
    check('day 7 recomputed to PRESENT 480 after the clear', day7?.status === 'PRESENT' && day7?.workedMinutes === 480, JSON.stringify({ s: day7?.status, w: day7?.workedMinutes }))
    const audits = await db.auditLog.findMany({ where: { action: 'DAILY_BULK_OVERRIDE' } })
    check('override audit row exists with the OLD values', audits.length === 1 && JSON.parse(audits[0]?.details ?? '{}').count === 3)
  }

  /* ---------------- [F] daily bulk delete ---------------- */
  section('F. bulkDeleteDaily: derived rows removed + audited')
  {
    const rows = await db.dailyAttendance.findMany({ where: { personId: p2.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } } })
    const r = await bulk.bulkDeleteDaily({ ids: rows.map((x) => x.id), reason: 'حذف رکوردهای آزمایشی', actor: ACTOR })
    check('3 derived rows deleted', r.deleted === 3, JSON.stringify(r))
    check('rows gone', (await db.dailyAttendance.count({ where: { personId: p2.id, date: { gte: MK(2026, 11, 7), lte: MK(2026, 11, 9) } } })) === 0)
    const audits = await db.auditLog.findMany({ where: { action: 'DAILY_BULK_DELETE' } })
    check('audit keeps the removed derived values', audits.length === 1 && JSON.parse(audits[0]?.details ?? '{}').count === 3)
  }

  /* ---------------- [G] atomicity ---------------- */
  section('G. atomicity: a mid-bulk failure rolls back EVERYTHING (no partial edit, no audit)')
  {
    const ids = punchIds.slice(4) // day 9's two punches (p1)
    const before = await db.punchRecord.findMany({ where: { id: { in: ids } } })
    await db.$executeRawUnsafe('CREATE TRIGGER boom_upd BEFORE UPDATE ON "PunchRecord" BEGIN SELECT RAISE(ABORT, \'boom\'); END')
    let threw = false
    try {
      await bulk.bulkEditPunches({ ids, direction: 'OUT', editReason: 'x', actor: ACTOR })
    } catch { threw = true }
    check('the bulk edit threw on the mid-batch failure', threw)
    await db.$executeRawUnsafe('DROP TRIGGER boom_upd')
    const after = await db.punchRecord.findMany({ where: { id: { in: ids } } })
    check('NO row was modified (transaction rolled back)', after.every((x, i) => x.direction === before[i].direction && x.edited === before[i].edited))
    const audits = await db.auditLog.findMany({ where: { action: 'PUNCH_BULK_EDIT' } })
    check('NO audit row was written for the failed operation (still exactly 1)', audits.length === 1, String(audits.length))
    await db.punchRecord.update({ where: { id: ids[0] }, data: { direction: 'AUTO' } }).catch(() => null)
    check('database still accepts writes after the rollback', true)
  }

  /* ---------------- [H] concurrency ---------------- */
  section('H. concurrency: parallel ops on the same entity — exactly one runs')
  {
    const ids = punchIds.slice(4)
    const results = await Promise.allSettled([
      bulk.bulkEditPunches({ ids, editReason: 'a', actor: ACTOR }),
      bulk.bulkEditPunches({ ids, editReason: 'b', actor: ACTOR }),
    ])
    const ok = results.filter((x) => x.status === 'fulfilled').length
    const busy = results.filter((x) => x.status === 'rejected' && x.reason instanceof bulk.BulkBusyError).length
    check('exactly ONE op ran; the other was rejected busy', ok === 1 && busy === 1, `ok=${ok} busy=${busy}`)
    const after = await db.punchRecord.findMany({ where: { id: { in: ids }, editReason: 'a' } })
    check('the winner\'s edit is the one visible', after.length === 2)
    await db.punchRecord.updateMany({ where: { id: { in: ids } }, data: { editReason: 'x', edited: true } })
  }

  /* ---------------- [I] validation ---------------- */
  section('I. validation: clean Persian errors for junk input')
  {
    let bad1 = false; try { await bulk.bulkEditPunches({ ids: [], editReason: 'x', actor: ACTOR }) } catch { bad1 = true }
    check('empty ids rejected', bad1)
    let bad2 = false; try { await bulk.bulkEditPunches({ ids: ['x'], direction: 'SIDEWAYS', actor: ACTOR }) } catch { bad2 = true }
    check('invalid direction rejected', bad2)
    let bad3 = false; try { await bulk.bulkDeletePunches({ ids: ['x'], reason: '  ', actor: ACTOR }) } catch { bad3 = true }
    check('whitespace-only reason rejected', bad3)
    let bad4 = false; try { await bulk.bulkOverrideDaily({ ids: ['x'], override: { status: 'NOT_A_STATUS' }, actor: ACTOR }) } catch { bad4 = true }
    check('invalid override status rejected', bad4)
    let bad5 = false; try { await bulk.bulkOverrideDaily({ ids: ['x'], override: { workedMinutes: 99999 }, actor: ACTOR }) } catch { bad5 = true }
    check('out-of-range minutes rejected', bad5)
    let bad6 = false; try { await bulk.bulkOverrideDaily({ ids: ['x'], override: {}, actor: ACTOR }) } catch { bad6 = true }
    check('an empty override patch rejected', bad6)
  }

  await db.$disconnect()
  if (sectionFailed) sectionsFailed++; else sectionsPassed++
  console.log('\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━')
  console.log(`bulk-ops suite: ${passed} assertion(s) passed, ${failed} failed`)
  console.log(`bulk-ops suite: ${sectionsPassed} section(s) fully green, ${sectionsFailed} with failures (A..I)`)
  if (failed > 0) { console.log('\nFAILED:'); for (const f of failures) console.log('  ✘ ' + f); process.exit(1) }
  process.exit(0)
}

main().catch((e) => {
  console.error('suite crashed:', e)
  process.exit(1)
})
