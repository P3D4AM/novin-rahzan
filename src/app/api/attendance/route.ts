import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { addDays } from '@/lib/jalali'
import { parseDayParam } from '@/lib/time'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    // v1.6.0 (phase 1 stage 3): from/to are DATE-ONLY business days → UTC-midnight
    // markers (never timezone-shifted); identical values to the legacy parse for
    // valid input, clean 400 for junk.
    const from = parseDayParam(sp.get('from'))
    const to = parseDayParam(sp.get('to'))
    const departmentId = sp.get('departmentId') || ''
    const personId = sp.get('personId') || ''
    const status = sp.get('status') || ''
    if (!from || !to) throw new ApiError(400, 'بازه تاریخ نامعتبر')

    const where: Record<string, unknown> = {
      date: { gte: from, lte: to },
    }
    if (personId) where.personId = personId
    if (status) where.status = status
    if (departmentId) where.person = { departmentId }

    const records = await db.dailyAttendance.findMany({
      where,
      include: {
        person: {
          select: {
            id: true, firstName: true, lastName: true, personnelCode: true,
            department: { select: { name: true } },
            jobTitle: { select: { name: true } },
          },
        },
        shift: { select: { id: true, name: true, color: true } },
      },
      orderBy: [{ date: 'asc' }, { person: { personnelCode: 'asc' } }],
      take: 5000,
    })
    return NextResponse.json({ records })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    const from = parseDayParam(String(b.from))
    const to = parseDayParam(String(b.to))
    if (!from || !to || to < from) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const personIds: string[] = b.personIds?.length
      ? b.personIds
      : (await db.personnel.findMany({ where: { status: 'ACTIVE' }, select: { id: true } })).map((p) => p.id)
    const count = await recalcRange(personIds, from, addDays(to, 1))
    return NextResponse.json({ ok: true, count })
  } catch (err) {
    return fail(err)
  }
}
