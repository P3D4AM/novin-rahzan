import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { parseDayParam, parseJalaliStringToMarker, addDaysMarker } from '@/lib/time'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    // v1.6.0 (phase 1 stage 3): explicit date-only day params — UTC-midnight markers, never tz-shifted
    const from = parseDayParam(sp.get('from'))
    const to = parseDayParam(sp.get('to'))
    if ((sp.get('from') && !from) || (sp.get('to') && !to)) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const holidays = await db.holiday.findMany({ where: { ...(from && to ? { date: { gte: from, lte: to } } : {}) }, orderBy: { date: 'asc' } })
    return NextResponse.json({ holidays })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    // v1.6.0 (phase 1 stage 3): Jalali string → marker via the central module;
    // date-only values → UTC-midnight marker (no tz shift). Identical results to
    // the legacy path for valid input; junk now yields a clean 400 instead of NaN.
    let date = b.dateJalali ? parseJalaliStringToMarker(String(b.dateJalali)) : b.date ? parseDayParam(String(b.date)) : null
    if (!date) throw new ApiError(400, 'تاریخ نامعتبر')
    const holiday = await db.holiday.upsert({
      where: { date },
      create: { date, title: b.title || 'تعطیل', type: b.type || 'OFFICIAL' },
      update: { title: b.title || 'تعطیل', type: b.type || 'OFFICIAL' },
    })
    const persons = await db.personnel.findMany({ where: { status: 'ACTIVE' }, select: { id: true } })
    await recalcRange(persons.map((p) => p.id), date, addDaysMarker(date, 1))
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
