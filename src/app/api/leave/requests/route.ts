import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError, hasRole } from '@/lib/auth'
import { fail } from '@/lib/api'
import { onLeaveRequest, notifyUser } from '@/lib/notify'
import { recalcRange } from '@/lib/attendance'
import { startOfUTCDay, addDays } from '@/lib/jalali'

export async function GET(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const status = sp.get('status') || ''
    const personId = sp.get('personId') || ''
    const mine = sp.get('mine') === '1'
    const where: Record<string, unknown> = {}
    if (status) where.status = status
    if (personId) where.personId = personId
    if (mine || user.role === 'EMPLOYEE') where.personId = user.personnelId ?? 'none'
    const items = await db.leaveRequest.findMany({
      where,
      include: {
        person: { select: { id: true, firstName: true, lastName: true, personnelCode: true } },
        leaveType: { select: { id: true, name: true, color: true, paid: true } },
      },
      orderBy: { createdAt: 'desc' },
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
    if (!b.leaveTypeId || !b.startDateTime || !b.endDateTime) throw new ApiError(400, 'نوع مرخصی و بازه زمانی الزامی است')
    const start = new Date(b.startDateTime)
    const end = new Date(b.endDateTime)
    if (end <= start) throw new ApiError(400, 'پایان مرخصی باید بعد از شروع باشد')
    const durationMinutes = Math.round((end.getTime() - start.getTime()) / 60000)
    const type = await db.leaveType.findUnique({ where: { id: b.leaveTypeId } })
    if (!type) throw new ApiError(404, 'نوع مرخصی یافت نشد')

    // overlap check
    const overlap = await db.leaveRequest.findFirst({
      where: { personId, status: { in: ['PENDING', 'APPROVED'] }, startDateTime: { lt: end }, endDateTime: { gt: start } },
    })
    if (overlap) throw new ApiError(400, 'با درخواست مرخصی دیگری هم‌پوشانی دارد')

    const item = await db.leaveRequest.create({
      data: {
        personId, leaveTypeId: b.leaveTypeId, kind: b.kind || (durationMinutes < 1440 ? 'HOURLY' : 'DAILY'),
        startDateTime: start, endDateTime: end, durationMinutes,
        reason: b.reason || null,
        status: type.requiresApproval && !hasRole(user, 'ADMIN') ? 'PENDING' : hasRole(user, 'ADMIN') ? 'APPROVED' : 'PENDING',
        ...(hasRole(user, 'ADMIN') ? { approverId: user.id, decidedAt: new Date() } : {}),
      },
    })
    await onLeaveRequest(item.id)
    if (item.status === 'APPROVED') {
      await recalcRange([personId], startOfUTCDay(start), startOfUTCDay(end))
    }
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
    const req0 = await db.leaveRequest.findUnique({ where: { id: b.id } })
    if (!req0) throw new ApiError(404, 'درخواست یافت نشد')
    if (req0.status !== 'PENDING' && b.status !== 'CANCELLED') throw new ApiError(400, 'این درخواست قبلاً بررسی شده است')
    const item = await db.leaveRequest.update({
      where: { id: b.id },
      data: { status: b.status, approverId: user.id, decidedAt: new Date(), decisionNote: b.note || null },
    })
    // notify requester
    const req2 = await db.user.findFirst({ where: { personnelId: req0.personId } })
    if (req2) {
      const msg = b.status === 'APPROVED' ? 'درخواست مرخصی شما تأیید شد' : b.status === 'REJECTED' ? 'درخواست مرخصی شما رد شد' : 'درخواست مرخصی شما لغو شد'
      await notifyUser(req2.id, 'نتیجه درخواست مرخصی', msg, b.status)
    }
    await recalcRange([req0.personId], startOfUTCDay(req0.startDateTime), startOfUTCDay(addDays(req0.endDateTime, 1)))
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const id = req.nextUrl.searchParams.get('id') || ''
    const item = await db.leaveRequest.findUnique({ where: { id } })
    if (!item) throw new ApiError(404, 'یافت نشد')
    const canDelete = hasRole(user, 'ADMIN') || (user.personnelId === item.personId && item.status === 'PENDING')
    if (!canDelete) throw new ApiError(403, 'اجازه حذف ندارید')
    await db.leaveRequest.delete({ where: { id } })
    await recalcRange([item.personId], startOfUTCDay(item.startDateTime), startOfUTCDay(item.endDateTime))
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
