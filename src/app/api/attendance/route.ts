import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { addDays, startOfUTCDay } from '@/lib/jalali'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const from = new Date(sp.get('from') || '')
    const to = new Date(sp.get('to') || '')
    const departmentId = sp.get('departmentId') || ''
    const personId = sp.get('personId') || ''
    const status = sp.get('status') || ''
    if (isNaN(from.getTime()) || isNaN(to.getTime())) throw new ApiError(400, 'بازه تاریخ نامعتبر')

    const where: Record<string, unknown> = {
      date: { gte: startOfUTCDay(from), lte: startOfUTCDay(to) },
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
    const from = startOfUTCDay(new Date(b.from))
    const to = startOfUTCDay(new Date(b.to))
    if (isNaN(from.getTime()) || isNaN(to.getTime()) || to < from) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const personIds: string[] = b.personIds?.length
      ? b.personIds
      : (await db.personnel.findMany({ where: { status: 'ACTIVE' }, select: { id: true } })).map((p) => p.id)
    const count = await recalcRange(personIds, from, addDays(to, 1))
    return NextResponse.json({ ok: true, count })
  } catch (err) {
    return fail(err)
  }
}
