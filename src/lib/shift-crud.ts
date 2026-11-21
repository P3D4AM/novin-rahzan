/**
 * Stage 7 review fix — the transactional shift CRUD core (create / update /
 * delete), extracted from /api/shifts so the PUT contract is unit-testable
 * against a throwaway SQLite DB (scripts/test-shift-crud.ts), exactly like
 * src/lib/bulk-ops.ts in stage 6. The route stays a thin ADMIN-guarded
 * wrapper over THESE functions; the shared validator (src/lib/shift-time.ts)
 * stays the ONE gate for every segment that reaches the database.
 *
 * The PUT contract — the two pre-existing risks this closes:
 *   1. `segments` ABSENT from the request body → ONLY scalar fields are
 *      updated; the existing segments are NOT deleted. (The old code ran
 *      `deleteMany` UNCONDITIONALLY before the update — a segment-less PUT
 *      silently destroyed the shift's segments.)
 *   2. `segments` PRESENT → validate FIRST, then delete + recreate inside
 *      ONE $transaction: if anything fails mid-way (validation, unique name
 *      conflict, unknown id, engine error) the previous segments survive
 *      untouched. (The old code deleted BEFORE updating — a failed update
 *      left the shift with NO segments at all.)
 *
 * breakMin contract (unified client / server / persistence): OPTIONAL.
 * Absent (undefined/null) is stored as 0 — the same `?? 0` as before on the
 * API side and the same Int @default(0) on the column; validateShiftSegments
 * now accepts absent too, so the UI gate and the server gate agree.
 */
import { PrismaClient } from '@prisma/client'
import { validateShiftSegments, ShiftSegmentInput } from './shift-time'

/** Validation failure → the route maps this to HTTP 400 (Persian message). */
export class ShiftValidationError extends Error {
  constructor(message: string) {
    super(message)
    this.name = 'ShiftValidationError'
  }
}

/** The scalar (non-segment) shift fields the form can write. */
export interface ShiftScalarInput {
  name?: string
  color?: string
  type?: string
  lateGraceMin?: number
  earlyGraceMin?: number
  overtimeEligible?: boolean
}

export interface ShiftCreateInput extends ShiftScalarInput {
  name: string
  segments: ShiftSegmentInput[]
}

/** Segment rows for prisma create — breakMin optional → 0 (unified contract). */
function segRows(segments: ShiftSegmentInput[]) {
  return segments.map((s, i) => ({
    order: i + 1,
    startMin: s.startMin,
    endMin: s.endMin,
    breakMin: s.breakMin ?? 0,
  }))
}

/** Scalar fields actually forwarded to prisma (undefined = "leave
 *  unchanged" on update); name is handled explicitly at each call site. */
function scalarData(patch: ShiftScalarInput) {
  return {
    color: patch.color,
    type: patch.type,
    lateGraceMin: patch.lateGraceMin,
    earlyGraceMin: patch.earlyGraceMin,
    overtimeEligible: patch.overtimeEligible,
  }
}

export async function createShift(db: PrismaClient, input: ShiftCreateInput) {
  if (!input.name || !input.name.trim()) throw new ShiftValidationError('نام شیفت الزامی است')
  const segv = validateShiftSegments(input.segments)
  if (!segv.ok) throw new ShiftValidationError(segv.error)
  return db.shift.create({
    data: {
      name: input.name.trim(),
      ...scalarData(input),
      segments: { create: segRows(input.segments) },
    },
    include: { segments: { orderBy: { order: 'asc' } } },
  })
}

/**
 * Update a shift. `segments === undefined` → segments untouched (risk 1);
 * `segments` present → validated first, then delete + recreate ATOMICALLY
 * inside one $transaction (risk 2).
 */
export async function updateShift(
  db: PrismaClient,
  id: string,
  patch: ShiftScalarInput,
  segments?: ShiftSegmentInput[],
) {
  if (!id) throw new ShiftValidationError('شناسه لازم است')
  if (segments !== undefined) {
    const segv = validateShiftSegments(segments)
    if (!segv.ok) throw new ShiftValidationError(segv.error)
  }
  if (patch.name !== undefined && !patch.name.trim()) throw new ShiftValidationError('نام شیفت الزامی است')
  return db.$transaction(async (tx) => {
    if (segments !== undefined) {
      await tx.shiftSegment.deleteMany({ where: { shiftId: id } })
    }
    return tx.shift.update({
      where: { id },
      data: {
        name: patch.name,
        ...scalarData(patch),
        ...(segments !== undefined ? { segments: { create: segRows(segments) } } : {}),
      },
      include: { segments: { orderBy: { order: 'asc' } } },
    })
  })
}

/** Delete a shift — usage check + segment removal + shift removal in ONE
 *  $transaction (a mid-way failure can no longer leave a segment-less shift). */
export async function deleteShift(db: PrismaClient, id: string) {
  if (!id) throw new ShiftValidationError('شناسه لازم است')
  const inUse = await db.personnel.count({ where: { defaultShiftId: id } })
  if (inUse > 0) throw new ShiftValidationError('این شیفت به پرسنل اختصاص یافته است')
  return db.$transaction(async (tx) => {
    await tx.shiftSegment.deleteMany({ where: { shiftId: id } })
    await tx.shift.delete({ where: { id } })
  })
}
