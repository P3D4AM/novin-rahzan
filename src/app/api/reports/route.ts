import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { startOfUTCDay, addDays, monthRange, toJalali, faDigits } from '@/lib/jalali'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const type = sp.get('type') || 'daily'
    const from = startOfUTCDay(new Date(sp.get('from') || ''))
    const to = startOfUTCDay(new Date(sp.get('to') || ''))
    const departmentId = sp.get('departmentId') || ''
    if (isNaN(from.getTime()) || isNaN(to.getTime())) throw new ApiError(400, 'بازه تاریخ نامعتبر')

    const personWhere: Record<string, unknown> = {}
    if (departmentId) personWhere.departmentId = departmentId
    const persons = await db.personnel.findMany({
      where: { status: 'ACTIVE', ...personWhere },
      include: { department: { select: { name: true } }, jobTitle: { select: { name: true } } },
      orderBy: { personnelCode: 'asc' },
    })
    const personIds = persons.map((p) => p.id)

    const records = await db.dailyAttendance.findMany({
      where: { personId: { in: personIds }, date: { gte: from, lte: to } },
      include: { shift: { select: { name: true } }, leaveType: { select: { name: true } } },
      orderBy: { date: 'asc' },
    })
    const byPerson = new Map<string, typeof records>()
    for (const r of records) {
      const arr = byPerson.get(r.personId) ?? []
      arr.push(r)
      byPerson.set(r.personId, arr)
    }

    const overtimeReqs = await db.overtimeRequest.findMany({
      where: { personId: { in: personIds }, date: { gte: from, lte: to }, status: 'APPROVED' },
    })

    if (sp.get('export') === 'xlsx') {
      return exportXlsx(type, persons, byPerson, overtimeReqs, from, to)
    }

    // summary per person
    const summary = persons.map((p) => {
      const rs = byPerson.get(p.id) ?? []
      const present = rs.filter((r) => r.status === 'PRESENT' || r.status === 'INCOMPLETE').length
      const absent = rs.filter((r) => r.status === 'ABSENT').length
      const leave = rs.filter((r) => r.status === 'LEAVE').length
      const worked = rs.reduce((a, r) => a + r.workedMinutes, 0)
      const late = rs.reduce((a, r) => a + r.lateMinutes, 0)
      const ot = rs.reduce((a, r) => a + r.overtimeMinutes, 0)
      const leaveMin = rs.reduce((a, r) => a + r.leaveMinutes, 0)
      const otApproved = overtimeReqs.filter((o) => o.personId === p.id).reduce((a, o) => a + o.minutes, 0)
      return {
        personId: p.id, code: p.personnelCode, name: `${p.firstName} ${p.lastName}`,
        department: p.department?.name ?? '-', jobTitle: p.jobTitle?.name ?? '-',
        present, absent, leave, workedMinutes: worked, lateMinutes: late, overtimeMinutes: ot, leaveMinutes: leaveMin, otApproved,
      }
    })

    const daily = records.map((r) => {
      const p = persons.find((x) => x.id === r.personId)!
      return {
        date: r.date, personId: r.personId, code: p.personnelCode, name: `${p.firstName} ${p.lastName}`,
        department: p.department?.name ?? '-', shift: r.shift?.name ?? '-', status: r.status,
        firstIn: r.firstIn, lastOut: r.lastOut, workedMinutes: r.workedMinutes, lateMinutes: r.lateMinutes,
        earlyLeaveMinutes: r.earlyLeaveMinutes, overtimeMinutes: r.overtimeMinutes, leaveMinutes: r.leaveMinutes,
        leaveType: r.leaveType?.name ?? null,
      }
    })

    return NextResponse.json({ type, from, to, summary, daily })
  } catch (err) {
    return fail(err)
  }
}

function exportXlsx(type: string, persons: unknown[], byPerson: Map<string, unknown[]>, otReqs: unknown[], from: Date, to: Date) {
  // dynamic import to avoid bundling xlsx in every request
  return (async () => {
    const XLSX = await import('xlsx')
    const data: Record<string, unknown>[] = []
    const summaryRows = (persons as Array<{ personnelCode: string; firstName: string; lastName: string; department?: { name: string } }>).map((p) => {
      const rs = (byPerson.get((p as { id: string }).id) ?? []) as Array<{ status: string; workedMinutes: number; lateMinutes: number; overtimeMinutes: number; leaveMinutes: number }>
      return {
        'کد پرسنلی': p.personnelCode,
        'نام و نام خانوادگی': `${p.firstName} ${p.lastName}`,
        'دپارتمان': p.department?.name ?? '-',
        'روزهای حضور': rs.filter((r) => r.status === 'PRESENT' || r.status === 'INCOMPLETE').length,
        'روزهای غیبت': rs.filter((r) => r.status === 'ABSENT').length,
        'روزهای مرخصی': rs.filter((r) => r.status === 'LEAVE').length,
        'ساعت کارکرد': Math.round(rs.reduce((a, r) => a + r.workedMinutes, 0) / 6) / 10,
        'دقیقه تأخیر': rs.reduce((a, r) => a + r.lateMinutes, 0),
        'ساعت اضافه‌کاری': Math.round(rs.reduce((a, r) => a + r.overtimeMinutes, 0) / 6) / 10,
        'ساعت مرخصی': Math.round(rs.reduce((a, r) => a + r.leaveMinutes, 0) / 6) / 10,
      }
    })
    const wb = XLSX.utils.book_new()
    const ws = XLSX.utils.json_to_sheet(summaryRows)
    XLSX.utils.book_append_sheet(wb, ws, 'خلاصه ماهانه')
    const buf = XLSX.write(wb, { type: 'buffer', bookType: 'xlsx' }) as Buffer
    const res = new NextResponse(buf)
    res.headers.set('Content-Type', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet')
    res.headers.set('Content-Disposition', `attachment; filename="hozorplus-report-${type}.xlsx"`)
    return res
  })()
}


