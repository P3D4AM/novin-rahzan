import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireAuth, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { onPunchRecorded } from '@/lib/notify'
import { recalcDayForPerson, recalcRange } from '@/lib/attendance'
import { tehranDayOf, addDays, toJalali, monthRange, startOfUTCDay } from '@/lib/jalali'

export async function GET() {
  try {
    const user = await requireAuth()
    if (!user.personId) return NextResponse.json({ error: 'کاربر به پرونده پرسنلی متصل نیست' }, { status: 400 })
    const person = await db.personnel.findUnique({
      where: { id: user.personId },
      include: {
        department: { select: { name: true } },
        jobTitle: { select: { name: true } },
        employmentType: { select: { name: true } },
        defaultShift: { include: { segments: true } },
      },
    })
    if (!person) return NextResponse.json({ error: 'پرونده پرسنلی یافت نشد' }, { status: 404 })

    const today = tehranDayOf(new Date())
    const { jy, jm } = toJalali(new Date())
    const mr = monthRange(jy, jm)

    const [todayPunches, dailyToday, monthRecords, myLeaves, myOvertime, leaveTypes] = await Promise.all([
      db.punchRecord.findMany({ where: { personId: person.id, timestamp: { gte: today, lt: addDays(today, 1) } }, orderBy: { timestamp: 'asc' } }),
      db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: today } } }),
      db.dailyAttendance.findMany({ where: { personId: person.id, date: { gte: mr.start, lte: mr.end } }, orderBy: { date: 'asc' } }),
      db.leaveRequest.findMany({ where: { personId: person.id }, include: { leaveType: { select: { name: true, color: true } } }, orderBy: { createdAt: 'desc' }, take: 30 }),
      db.overtimeRequest.findMany({ where: { personId: person.id }, orderBy: { date: 'desc' }, take: 30 }),
      db.leaveType.findMany(),
    ])

    const usedLeaveDays = monthRecords.reduce((a, r) => a + (r.leaveMinutes > 0 && r.leaveTypeId ? r.leaveMinutes / 480 : 0), 0)
    return NextResponse.json({
      person, todayPunches, dailyToday, monthRecords, myLeaves, myOvertime, leaveTypes,
      leaveBalance: { annual: person.annualLeaveDays, usedThisMonth: Math.round(usedLeaveDays * 10) / 10 },
    })
  } catch (err) {
    return fail(err)
  }
}

/** self punch from phone/web */
export async function POST(req: NextRequest) {
  try {
    const user = await requireAuth()
    if (!user.personId) throw new ApiError(400, 'کاربر به پرونده پرسنلی متصل نیست')
    const b = await req.json().catch(() => ({}))
    const now = new Date()
    const dayStart = tehranDayOf(now)
    const dayEnd = addDays(dayStart, 1)
    const todayPunches = await db.punchRecord.findMany({ where: { personId: user.personId, timestamp: { gte: dayStart, lt: dayEnd } }, orderBy: { timestamp: 'desc' }, take: 1 })
    const direction = todayPunches.length === 0 ? 'IN' : todayPunches[0].direction === 'IN' ? 'OUT' : 'IN'
    const dup = await db.punchRecord.findFirst({ where: { personId: user.personId, timestamp: { gte: new Date(now.getTime() - 60000) } } })
    if (dup) throw new ApiError(429, 'ثبت مکرر! لطفاً یک دقیقه صبر کنید')
    const punch = await db.punchRecord.create({
      data: { personId: user.personId, timestamp: now, direction, source: 'WEB', latitude: b.latitude || null, longitude: b.longitude || null },
    })
    await recalcDayForPerson(user.personId, now)
    await onPunchRecorded({ personId: user.personId, personName: user.fullName, timestamp: now, direction, source: 'WEB' })
    return NextResponse.json({ punch, direction })
  } catch (err) {
    return fail(err)
  }
}

void startOfUTCDay
