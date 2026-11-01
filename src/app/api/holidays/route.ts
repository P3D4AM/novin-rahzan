import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { startOfUTCDay, parseJalaliString, addDays } from '@/lib/jalali'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const from = sp.get('from') ? new Date(sp.get('from')!) : undefined
    const to = sp.get('to') ? new Date(sp.get('to')!) : undefined
    const holidays = await db.holiday.findMany({ where: { ...(from && to ? { date: { gte: startOfUTCDay(from), lte: startOfUTCDay(to) } } : {}) }, orderBy: { date: 'asc' } })
    return NextResponse.json({ holidays })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    let date = b.dateJalali ? parseJalaliString(b.dateJalali) : b.date ? new Date(b.date) : null
    if (!date || isNaN(date.getTime())) throw new ApiError(400, 'تاریخ نامعتبر')
    date = startOfUTCDay(date)
    const holiday = await db.holiday.upsert({
      where: { date },
      create: { date, title: b.title || 'تعطیل', type: b.type || 'OFFICIAL' },
      update: { title: b.title || 'تعطیل', type: b.type || 'OFFICIAL' },
    })
    const persons = await db.personnel.findMany({ where: { status: 'ACTIVE' }, select: { id: true } })
    await recalcRange(persons.map((p) => p.id), date, addDays(date, 1))
    return NextResponse.json({ holiday })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    const h = await db.holiday.findUnique({ where: { id } })
    if (!h) throw new ApiError(404, 'یافت نشد')
    await db.holiday.delete({ where: { id } })
    const persons = await db.personnel.findMany({ where: { status: 'ACTIVE' }, select: { id: true } })
    await recalcRange(persons.map((p) => p.id), h.date, h.date)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
