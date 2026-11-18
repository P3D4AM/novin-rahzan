import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError, hasRole } from '@/lib/auth'
import { fail } from '@/lib/api'
import { parseDayParam } from '@/lib/time'
import { recalcRange } from '@/lib/attendance'

/** Phase 2 S3 (min_excess): recompute the workday after an overtime request
 * decision so payableOvertimeMinutes reflects the change immediately. */
async function recalcOvertimeDay(personId: string, date: Date) {
  const day = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate()))
  await recalcRange([personId], day, day)
}

export async function GET(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const status = sp.get('status') || ''
    const where: Record<string, unknown> = {}
    if (status) where.status = status
    if (user.role === 'EMPLOYEE') where.personId = user.personId ?? 'none'
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
    const personId = hasRole(user, 'ADMIN') && b.personId ? b.personId : user.personId
    if (!personId) throw new ApiError(400, 'کاربر به پرسنلی متصل نیست')
    if (!b.date || !b.minutes) throw new ApiError(400, 'تاریخ و مدت الزامی است')
    // v1.6.0 (phase 1 stage 3): explicit date-only contract (YYYY-MM-DD or marker ISO)
    // — parsed as a UTC-midnight marker, never timezone-shifted
    const day = parseDayParam(String(b.date))
    if (!day) throw new ApiError(400, 'تاریخ نامعتبر است')
    const item = await db.overtimeRequest.create({
      data: { personId, date: day, minutes: Number(b.minutes), reason: b.reason || null, status: 'PENDING' },
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
    // Phase 2 S3: optional minutes edit — the request amount is part of the
    // payable calculation (min(actual, approved)), so a value change must be
    // validated server-side and followed by a recalc of that workday.
    const data: Record<string, unknown> = { status: b.status, approverId: user.id, decidedAt: new Date() }
    if (b.minutes !== undefined) {
      const minutes = Number(b.minutes)
      if (!Number.isInteger(minutes) || minutes <= 0) throw new ApiError(400, 'دقیقه‌های درخواست باید عدد صحیح مثبت باشد')
      data.minutes = minutes
    }
    const item = await db.overtimeRequest.update({
      where: { id: b.id },
      data,
    })
    // recompute the affected workday (approve / reject / amount change)
    await recalcOvertimeDay(item.personId, item.date)
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
    if (!hasRole(user, 'ADMIN') && user.personId !== item.personId) throw new ApiError(403, 'اجازه حذف ندارید')
    await db.overtimeRequest.delete({ where: { id } })
    // Phase 2 S3: a deleted request no longer contributes approved minutes —
    // recompute the workday so payableOvertimeMinutes stays truthful.
    await recalcOvertimeDay(item.personId, item.date)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
