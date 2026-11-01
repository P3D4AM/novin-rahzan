import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError, hasRole } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const status = sp.get('status') || ''
    const where: Record<string, unknown> = {}
    if (status) where.status = status
    if (user.role === 'EMPLOYEE') where.personId = user.personnelId ?? 'none'
    const items = await db.overtimeRequest.findMany({
      where,
      include: { person: { select: { id: true, firstName: true, lastName: true, personnelCode: true } } },
      orderBy: { date: 'desc' },
      take: 500,
    })
    return NextResponse.json({ items })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const b = await req.json()
    const personId = hasRole(user, 'ADMIN') && b.personId ? b.personId : user.personnelId
    if (!personId) throw new ApiError(400, 'کاربر به پرسنلی متصل نیست')
    if (!b.date || !b.minutes) throw new ApiError(400, 'تاریخ و مدت الزامی است')
    const item = await db.overtimeRequest.create({
      data: { personId, date: startOfDay(b.date), minutes: Number(b.minutes), reason: b.reason || null, status: 'PENDING' },
    })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    const user = await requireRole('APPROVER')
    const b = await req.json()
    if (!b.id || !b.status) throw new ApiError(400, 'شناسه و وضعیت الزامی است')
    const item = await db.overtimeRequest.update({
      where: { id: b.id },
      data: { status: b.status, approverId: user.id, decidedAt: new Date() },
    })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const id = req.nextUrl.searchParams.get('id') || ''
    const item = await db.overtimeRequest.findUnique({ where: { id } })
    if (!item) throw new ApiError(404, 'یافت نشد')
    if (!hasRole(user, 'ADMIN') && user.personnelId !== item.personId) throw new ApiError(403, 'اجازه حذف ندارید')
    await db.overtimeRequest.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}

function startOfDay(s: string): Date {
  const d = new Date(s)
  return new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate()))
}
