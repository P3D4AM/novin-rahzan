import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { datesBetween, startOfUTCDay } from '@/lib/jalali'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const from = new Date(sp.get('from') || '')
    const to = new Date(sp.get('to') || '')
    if (isNaN(from.getTime()) || isNaN(to.getTime())) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const personId = sp.get('personId') || undefined
    const assignments = await db.shiftAssignment.findMany({
      where: { date: { gte: startOfUTCDay(from), lte: startOfUTCDay(to) }, ...(personId ? { personId } : {}) },
      include: { shift: { select: { id: true, name: true, color: true } } },
    })
    return NextResponse.json({ assignments })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    // b: { personIds: string[], from: date, to: date, shiftId?: string|null (null=تعطیلی), weekdays?: number[] }
    const personIds: string[] = b.personIds || []
    if (!personIds.length || !b.from || !b.to) throw new ApiError(400, 'انتخاب پرسنل و بازه تاریخ الزامی است')
    const from = startOfUTCDay(new Date(b.from))
    const to = startOfUTCDay(new Date(b.to))
    if (to < from) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const weekdays: number[] | null = b.weekdays?.length ? b.weekdays : null
    let count = 0
    for (const d of datesBetween(from, to)) {
      if (weekdays && !weekdays.includes(d.getUTCDay())) continue
      for (const personId of personIds) {
        await db.shiftAssignment.upsert({
          where: { personId_date: { personId, date: d } },
          create: { personId, date: d, shiftId: b.shiftId ?? null },
          update: { shiftId: b.shiftId ?? null },
        })
        count++
      }
    }
    await recalcRange(personIds, from, to)
    return NextResponse.json({ ok: true, count })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const sp = req.nextUrl.searchParams
    const personId = sp.get('personId') || ''
    const date = new Date(sp.get('date') || '')
    if (!personId || isNaN(date.getTime())) throw new ApiError(400, 'پارامتر نامعتبر')
    await db.shiftAssignment.deleteMany({ where: { personId, date: startOfUTCDay(date) } })
    await recalcRange([personId], date, date)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
