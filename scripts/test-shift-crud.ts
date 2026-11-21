/**
 * Stage 7 review fix test suite — /api/shifts PUT/POST/DELETE core on REAL
 * SQLite + REAL Prisma + the REAL CRUD lib (src/lib/shift-crud.ts).
 * =====================================================================
 * Throwaway test-owned database (scripts/test-db-guard.ts), same pattern as
 * scripts/test-bulk-ops.ts. No API server needed: the route is a thin
 * ADMIN-guarded wrapper over exactly these lib functions (verified by
 * reading route.ts; the guard itself is NOT exercised here — it needs a
 * session cookie, i.e. a running server).
 *
 *   [A] setup: fresh throwaway DB, schema pushed
 *   [B] createShift: valid (breakMin present/absent/null), night & 24h
 *       segments, name/segments validation errors
 *   [C] PUT without `segments` (risk 1): scalar fields updated, existing
 *       segments NOT deleted — the old code deleted them unconditionally
 *   [D] PUT with valid segments: atomic replace, order preserved
 *   [E] PUT with invalid segments: ShiftValidationError, DB untouched
 *   [F] PUT failing mid-transaction (RAISE trigger): previous segments
 *       SURVIVE (risk 2 — the old code deleted before updating)
 *   [G] PUT on a nonexistent id: clean failure, no side effects
 *   [H] PUT with empty segment list: validation error, nothing deleted
 *   [I] breakMin contract through the whole path: absent → stored 0;
 *       string/float/negative → rejected before the DB
 *   [J] DELETE: removes shift + segments; nonexistent id → clean error;
 *       atomic (RAISE trigger on the shift row) — no segment-less leftovers
 *   [K] teardown: DB accepts writes after triggers dropped
 *
 * Run:  npx tsx scripts/test-shift-crud.ts
 */
import { spawnSync } from 'child_process'
import { mkdirSync, rmSync } from 'fs'
import path from 'path'
import { PrismaClient } from '@prisma/client'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'
import { createShift, updateShift, deleteShift, ShiftValidationError } from '../src/lib/shift-crud'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_SHIFTCRUD || '/tmp/shift-crud-tests'
const DB_FILE = generateTestDatabasePath(TMP)
const DB_URL = sqliteUrl(DB_FILE)
process.env.DATABASE_URL = DB_URL // before the db client is imported

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}
function sh(cmd: string, args: string[], env: Record<string, string> = {}) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf-8', timeout: 240_000 })
}
async function expectValidationError(name: string, fn: () => Promise<unknown>) {
  try {
    await fn()
    check(name, false, 'no error thrown')
  } catch (err) {
    check(name, err instanceof ShiftValidationError, `threw ${(err as Error)?.name ?? String(err)} instead of ShiftValidationError`)
  }
}

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(DB_FILE, TMP)
  rmSync(DB_FILE, { force: true })
  const push = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: DB_URL })
  check('prisma db push (fresh schema on the throwaway DB)', push.status === 0, (push.stderr || '').slice(-200))

  const db = new PrismaClient()

  /* ---------------- [B] createShift ---------------- */
  section('B. createShift — validation + breakMin contract')

  const s1 = await createShift(db, {
    name: 'اداری صبح', color: '#10b981', type: 'FIXED', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
    segments: [{ startMin: 480, endMin: 990, breakMin: 60 }],
  })
  check('created with segments (include returns them ordered)', s1.segments.length === 1 && s1.segments[0].startMin === 480 && s1.segments[0].endMin === 990 && s1.segments[0].breakMin === 60)
  check('segment order starts at 1', s1.segments[0].order === 1)

  const s2 = await createShift(db, {
    name: 'شب امنیت', color: '#8b5cf6',
    segments: [{ startMin: 1320, endMin: 360 }], // night shift, breakMin ABSENT
  })
  check('night shift 22:00→06:00 created, breakMin ABSENT → stored as 0 (unified contract)', s2.segments[0].breakMin === 0 && s2.segments[0].startMin === 1320 && s2.segments[0].endMin === 360)

  const s3 = await createShift(db, {
    name: '۲۴ ساعته',
    segments: [{ startMin: 480, endMin: 480, breakMin: null }], // 24h, breakMin null
  })
  check('24h shift (start == end) created, breakMin null → stored as 0', s3.segments[0].breakMin === 0 && s3.segments[0].startMin === 480 && s3.segments[0].endMin === 480)

  const s4 = await createShift(db, {
    name: 'چندبخشی',
    segments: [
      { startMin: 480, endMin: 720, breakMin: 0 },
      { startMin: 780, endMin: 1020, breakMin: 15 },
    ],
  })
  check('split shift created, two segments in order', s4.segments.length === 2 && s4.segments[0].order === 1 && s4.segments[1].order === 2 && s4.segments[1].breakMin === 15)

  await expectValidationError("createShift without a name → ShiftValidationError ('نام شیفت الزامی است')", () =>
    createShift(db, { name: '   ', segments: [{ startMin: 480, endMin: 720 }] }))
  await expectValidationError('createShift without segments → ShiftValidationError', () =>
    createShift(db, { name: 'X', segments: [] as never }))
  await expectValidationError("createShift with '25:99' minutes (1500 > 1439) → ShiftValidationError", () =>
    createShift(db, { name: 'X', segments: [{ startMin: 1500, endMin: 720, breakMin: 0 }] }))
  await expectValidationError("createShift with breakMin '-5' → ShiftValidationError", () =>
    createShift(db, { name: 'X', segments: [{ startMin: 480, endMin: 720, breakMin: -5 }] }))
  await expectValidationError("createShift with breakMin '5' (string) → ShiftValidationError", () =>
    createShift(db, { name: 'X', segments: [{ startMin: 480, endMin: 720, breakMin: '5' as unknown as number }] }))

  /* ---------------- [C] PUT without segments (risk 1) ---------------- */
  section('C. PUT WITHOUT segments — existing segments are NOT deleted (risk 1)')

  const before = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  check('precondition: s1 has its 1 segment', before.length === 1)
  const updatedNoSegs = await updateShift(db, s1.id, { lateGraceMin: 15 }, undefined)
  check('scalar field updated (lateGraceMin 10 → 15)', updatedNoSegs.lateGraceMin === 15)
  const after = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  check('RISK 1 FIXED: segments SURVIVED a segments-less PUT (old code deleted them)', after.length === 1 && after[0].startMin === 480 && after[0].breakMin === 60)
  const nameOnly = await updateShift(db, s1.id, { name: 'اداری صبح (ویرایش)' }, undefined)
  check('name-only PUT works, segments still intact', nameOnly.name === 'اداری صبح (ویرایش)' && (await db.shiftSegment.count({ where: { shiftId: s1.id } })) === 1)

  /* ---------------- [D] PUT with valid segments ---------------- */
  section('D. PUT WITH valid segments — atomic replace')

  const replaced = await updateShift(db, s1.id, { overtimeEligible: false }, [
    { startMin: 540, endMin: 1080, breakMin: 45 },
    { startMin: 1140, endMin: 120, breakMin: 0 },
  ])
  check('PUT with segments: replaced 1 → 2 segments', replaced.segments.length === 2)
  check('PUT with segments: new values in order (09:00, 19:00 + night 19:00→02:00)', replaced.segments[0].startMin === 540 && replaced.segments[0].breakMin === 45 && replaced.segments[1].startMin === 1140 && replaced.segments[1].endMin === 120)
  const rawRows = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  check('raw DB rows match (order 1,2 — old row gone, no duplicates)', rawRows.length === 2 && rawRows[0].order === 1 && rawRows[1].order === 2)

  /* ---------------- [E] PUT with INVALID segments ---------------- */
  section('E. PUT WITH invalid segments — rejected, DB untouched')

  const before2 = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  await expectValidationError("PUT with '25:99'-style segment (endMin 1440) → ShiftValidationError", () =>
    updateShift(db, s1.id, { name: 'Z' }, [{ startMin: 480, endMin: 1440, breakMin: 0 }]))
  const after2 = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  check('invalid PUT: no row added, none deleted (validation runs BEFORE any delete)', after2.length === before2.length && after2[0].startMin === before2[0].startMin)
  const stillSame = await db.shift.findUnique({ where: { id: s1.id } })
  check('invalid PUT: scalar fields untouched too (name not applied)', stillSame?.name === 'اداری صبح (ویرایش)')

  /* ---------------- [F] PUT failing mid-transaction (risk 2) ---------------- */
  section('F. PUT failing MID-TRANSACTION — previous segments SURVIVE (risk 2)')

  await db.$executeRawUnsafe('CREATE TRIGGER boom_shift_upd BEFORE UPDATE ON "Shift" BEGIN SELECT RAISE(ABORT, \'boom\'); END')
  const before3 = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  let boomCaught = false
  try {
    await updateShift(db, s1.id, { lateGraceMin: 99 }, [{ startMin: 0, endMin: 1, breakMin: 0 }])
  } catch {
    boomCaught = true
  }
  check('the poisoned update failed (RAISE trigger fired inside the transaction)', boomCaught)
  const after3 = await db.shiftSegment.findMany({ where: { shiftId: s1.id }, orderBy: { order: 'asc' } })
  check('RISK 2 FIXED: the deleteMany ROLLED BACK — previous segments intact (old code left the shift with NO segments)', after3.length === before3.length && after3[0].startMin === before3[0].startMin && after3[0].breakMin === before3[0].breakMin)
  const scalarIntact = await db.shift.findUnique({ where: { id: s1.id } })
  check('mid-transaction failure: scalar fields untouched as well', scalarIntact?.lateGraceMin !== 99)
  await db.$executeRawUnsafe('DROP TRIGGER boom_shift_upd')
  check('trigger dropped, DB usable again', true)

  /* ---------------- [G] PUT on a nonexistent id ---------------- */
  section('G. PUT on a nonexistent id')

  const ghostBefore = await db.shiftSegment.count()
  let ghostErr: Error | null = null
  try {
    await updateShift(db, 'shift-does-not-exist', { name: 'G' }, [{ startMin: 480, endMin: 720, breakMin: 0 }])
  } catch (err) {
    ghostErr = err as Error
  }
  check('nonexistent id: update fails (prisma P2025-class error, not a silent no-op)', ghostErr !== null && !(ghostErr instanceof ShiftValidationError))
  check('nonexistent id: no segments were left orphaned by the aborted delete', (await db.shiftSegment.count()) === ghostBefore)

  /* ---------------- [H] PUT with an empty segment list ---------------- */
  section('H. PUT with an EMPTY segment list')

  await expectValidationError('PUT with segments: [] → ShiftValidationError (حداقل یک بازه)', () =>
    updateShift(db, s1.id, {}, []))
  check('empty-list PUT: existing segments NOT deleted', (await db.shiftSegment.count({ where: { shiftId: s1.id } })) === 2)

  /* ---------------- [I] breakMin through the whole path ---------------- */
  section('I. breakMin — end-to-end contract (validation → storage)')

  const s5 = await createShift(db, { name: 'بدون وقفه', segments: [{ startMin: 480, endMin: 990 }] })
  check('create with breakMin ABSENT: validator passes, stored as 0', s5.segments[0].breakMin === 0)
  const s5upd = await updateShift(db, s5.id, {}, [{ startMin: 480, endMin: 990, breakMin: null }])
  check('PUT with breakMin null: passes, stored as 0', s5upd.segments[0].breakMin === 0)
  await expectValidationError('PUT with breakMin 480.5 (float) → ShiftValidationError', () =>
    updateShift(db, s5.id, {}, [{ startMin: 480, endMin: 990, breakMin: 480.5 }]))
  await expectValidationError('PUT with breakMin NaN → ShiftValidationError', () =>
    updateShift(db, s5.id, {}, [{ startMin: 480, endMin: 990, breakMin: NaN }]))
  await expectValidationError('PUT with breakMin larger than the span → ShiftValidationError', () =>
    updateShift(db, s5.id, {}, [{ startMin: 480, endMin: 510, breakMin: 31 }]))
  const stored5 = await db.shiftSegment.findFirst({ where: { shiftId: s5.id } })
  check('after all rejected PUTs the stored segment is the last VALID one (breakMin 0, 08:00→16:30)', stored5?.breakMin === 0 && stored5?.startMin === 480 && stored5?.endMin === 990)

  /* ---------------- [J] DELETE ---------------- */
  section('J. DELETE — removes shift + segments, clean failures')

  const delShift = await createShift(db, { name: 'حذفی', segments: [{ startMin: 60, endMin: 120, breakMin: 0 }] })
  await deleteShift(db, delShift.id)
  check('deleted: shift gone', (await db.shift.count({ where: { id: delShift.id } })) === 0)
  check('deleted: its segments gone (no orphans)', (await db.shiftSegment.count({ where: { shiftId: delShift.id } })) === 0)

  await expectValidationError("DELETE with empty id → ShiftValidationError ('شناسه لازم است')", () =>
    deleteShift(db, ''))
  let ghostDel: Error | null = null
  try {
    await deleteShift(db, 'shift-does-not-exist')
  } catch (err) {
    ghostDel = err as Error
  }
  check('DELETE nonexistent id: fails cleanly (prisma error)', ghostDel !== null)

  // atomic delete: the shift row is poisoned so the second step fails
  const poisoned = await createShift(db, { name: 'مسموم', segments: [{ startMin: 60, endMin: 120, breakMin: 0 }] })
  await db.$executeRawUnsafe('CREATE TRIGGER boom_shift_del BEFORE DELETE ON "Shift" BEGIN SELECT RAISE(ABORT, \'boom\'); END')
  let delBoom = false
  try {
    await deleteShift(db, poisoned.id)
  } catch {
    delBoom = true
  }
  check('poisoned delete failed (RAISE fired after the segment deleteMany)', delBoom)
  const survivedSegs = await db.shiftSegment.count({ where: { shiftId: poisoned.id } })
  check('atomic delete: segment deleteMany ROLLED BACK with the failed shift delete (no segment-less shift left behind)', survivedSegs === 1)
  await db.$executeRawUnsafe('DROP TRIGGER boom_shift_del')
  await deleteShift(db, poisoned.id)
  check('after dropping the trigger the delete succeeds', (await db.shift.count({ where: { id: poisoned.id } })) === 0)

  /* ---------------- [K] teardown ---------------- */
  section('K. teardown')

  const finalWrite = await createShift(db, { name: 'نهایی', segments: [{ startMin: 0, endMin: 1439, breakMin: 0 }] })
  check('DB accepts ordinary writes after all triggers dropped', finalWrite.id.length > 0)

  console.log('\n━━━ SUMMARY (Stage 7 review — /api/shifts CRUD core, REAL SQLite)')
  console.log(`  DB (real SQLite + real Prisma) tests : ${passed} passed, ${failed} failed`)
  console.log(`  DB file (throwaway, test-owned)     : ${DB_FILE}`)
  console.log(`  NOT RUN HERE                        : requireRole ADMIN/EMPLOYEE guard (needs a running server + session cookie) — route.ts is a verified thin wrapper; UI dialog rendering (manual visual checklist); real Windows/hardware environment`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  await db.$disconnect()
  process.exit(failed > 0 ? 1 : 0)
}

main().catch((err) => {
  console.error('SUITE CRASHED:', err)
  process.exit(1)
})
