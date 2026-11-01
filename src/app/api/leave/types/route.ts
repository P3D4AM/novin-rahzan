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
    const item = await db.leaveType.create({ data: { name: b.name, paid: !!b.paid, color: b.color || '#f59e0b', requiresApproval: b.requiresApproval ?? true, countsTowardsBalance: b.countsTowardsBalance ?? false } })
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
    const item = await db.leaveType.update({ where: { id: b.id }, data: { name: b.name, paid: !!b.paid, color: b.color, requiresApproval: b.requiresApproval, countsTowardsBalance: b.countsTowardsBalance } })
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
