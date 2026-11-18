import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET() {
  try {
    await requireRole('EMPLOYEE')
    const items = await db.leaveType.findMany({ orderBy: { name: 'asc' } })
    return NextResponse.json({ items })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.name) throw new ApiError(400, 'نام الزامی است')
    // Phase 2 S3 (approved_credit): mission is an EXPLICIT flag on the type —
    // never inferred from the Persian name.
    const item = await db.leaveType.create({ data: { name: b.name, paid: !!b.paid, color: b.color || '#f59e0b', requiresApproval: b.requiresApproval ?? true, countsTowardsBalance: b.countsTowardsBalance ?? false, isMission: !!b.isMission } })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    // Phase 2 S3: isMission is optional on edit — omitted keeps the current
    // value (Prisma undefined = no change); explicit false/true sets it.
    const data: Record<string, unknown> = { name: b.name, paid: !!b.paid, color: b.color, requiresApproval: b.requiresApproval, countsTowardsBalance: b.countsTowardsBalance }
    if (b.isMission !== undefined) data.isMission = !!b.isMission
    const item = await db.leaveType.update({ where: { id: b.id }, data })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    const inUse = await db.leaveRequest.count({ where: { leaveTypeId: id } })
    if (inUse) throw new ApiError(400, 'این نوع مرخصی درخواست ثبت‌شده دارد')
    await db.leaveType.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
