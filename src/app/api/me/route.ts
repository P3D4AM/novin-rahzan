import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireAuth, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { onPunchRecorded } from '@/lib/notify'
import { recalcDayForPerson, recalcRange } from '@/lib/attendance'
import { tehranTodayMarker, jalaliOfInstant, jalaliMonthRange, tehranTodayWindow } from '@/lib/time'

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

    // v1.6.0 (phase 1 stage 3): Tehran today/month via the central tz-aware module
    // (server-machine timezone independence); marker & window conventions preserved.
    // Phase 2 S5: today's punch list uses the TRUE Tehran-day instant window
    // [Tehran midnight, next Tehran midnight) — was the UTC calendar day of the
    // Tehran date (displaced +3:30; Tehran 00:00–03:29 punches were missing and
    // the next day's early punches leaked in) — now identical to the kiosk route
    // and the engine's fallback day convention (docs/PHASE2-S5-AUDIT.md S5-A6).
    const today = tehranTodayMarker()
    const { jy, jm } = jalaliOfInstant(new Date())
    const mr = jalaliMonthRange(jy, jm)
    const todayWin = tehranTodayWindow()

    const [todayPunches, dailyToday, monthRecords, myLeaves, myOvertime, leaveTypes] = await Promise.all([
      db.punchRecord.findMany({ where: { personId: person.id, timestamp: { gte: todayWin.start, lt: todayWin.end } }, orderBy: { timestamp: 'asc' } }),
      db.dailyAttendance.findUnique({ where: { personId_date: { personId: person.id, date: today } } }),
      db.dailyAttendance.findMany({ where: { personId: person.id, date: { gte: mr.startMarker, lte: mr.endMarker } }, orderBy: { date: 'asc' } }),
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
    // Phase 2 S5: TRUE Tehran-day instant window — same convention as GET, the
    // kiosk route and the engine's fallback day attribution (the old
    // [tehranTodayMarker(), +1) window was the UTC calendar day displaced
    // +3:30 from Tehran midnight; docs/PHASE2-S5-AUDIT.md item S5-A6).
    const { start: dayStart, end: dayEnd } = tehranTodayWindow()
    const todayPunches = await db.punchRecord.findMany({ where: { personId: user.personId, timestamp: { gte: dayStart, lt: dayEnd } }, orderBy: { timestamp: 'desc' }, take: 1 })
    const direction = todayPunches.length === 0 ? 'IN' : todayPunches[0].direction === 'IN' ? 'OUT' : 'IN'
    const dup = await db.punchRecord.findFirst({ where: { personId: user.personId, timestamp: { gte: new Date(now.getTime() - 60000) } } })
    if (dup) throw new ApiError(429, 'ثبت مکرر! لطفاً یک دقیقه صبر کنید')
    const punch = await db.punchRecord.create({
      data: { personId: user.personId, timestamp: now, direction, source: 'WEB', latitude: b.latitude || null, longitude: b.longitude || null },
    })
    await recalcDayForPerson(user.personId, now, direction)
    await onPunchRecorded({ personId: user.personId, personName: user.fullName, timestamp: now, direction, source: 'WEB' })
    return NextResponse.json({ punch, direction })
  } catch (err) {
    return fail(err)
  }
}
