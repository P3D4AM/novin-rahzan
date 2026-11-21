import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail, badRequest } from '@/lib/api'
// Phase 3, item 9 + Stage 7 review fix: the SAME pure validator gates the
// form and the API — an invalid segment can no longer be stored even by a
// direct API call. The transactional CRUD core lives in src/lib/shift-crud.ts
// (PUT: no segments ⇒ existing segments untouched; delete+recreate atomic —
// unit-tested with REAL SQLite in scripts/test-shift-crud.ts).
import { createShift, updateShift, deleteShift, ShiftValidationError } from '@/lib/shift-crud'

export async function GET() {
  try {
    await requireRole('EMPLOYEE')
    const shifts = await db.shift.findMany({ include: { segments: { orderBy: { order: 'asc' } } }, orderBy: { name: 'asc' } })
    return NextResponse.json({ shifts })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    const shift = await createShift(db, {
      name: b.name,
      color: b.color,
      type: b.type,
      lateGraceMin: b.lateGraceMin,
      earlyGraceMin: b.earlyGraceMin,
      overtimeEligible: b.overtimeEligible,
      segments: b.segments,
    })
    return NextResponse.json({ shift })
  } catch (err) {
    if (err instanceof ShiftValidationError) return badRequest(err.message)
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    // Stage 7 review fix: `segments` ABSENT (undefined) in the body ⇒ the
    // existing segments are NOT deleted — only the scalar fields are updated.
    // `segments` present ⇒ validated first, then delete + recreate inside ONE
    // $transaction (src/lib/shift-crud.ts).
    const shift = await updateShift(
      db,
      b.id,
      {
        name: b.name,
        color: b.color,
        type: b.type,
        lateGraceMin: b.lateGraceMin,
        earlyGraceMin: b.earlyGraceMin,
        overtimeEligible: b.overtimeEligible,
      },
      b.segments,
    )
    return NextResponse.json({ shift })
  } catch (err) {
    if (err instanceof ShiftValidationError) return badRequest(err.message)
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    await deleteShift(db, id)
    return NextResponse.json({ ok: true })
  } catch (err) {
    if (err instanceof ShiftValidationError) return badRequest(err.message)
    return fail(err)
  }
}
