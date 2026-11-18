import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { datesBetween } from '@/lib/jalali'
import { parseDayParam } from '@/lib/time'
import { recalcRange } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    // v1.6.0 (phase 1 stage 3): explicit date-only day params (UTC-midnight markers, no tz shift)
    const from = parseDayParam(sp.get('from'))
    const to = parseDayParam(sp.get('to'))
    if (!from || !to) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const personId = sp.get('personId') || undefined
    const assignments = await db.shiftAssignment.findMany({
      where: { date: { gte: from, lte: to }, ...(personId ? { personId } : {}) },
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
    const from = parseDayParam(String(b.from))
    const to = parseDayParam(String(b.to))
    if (!from || !to || to < from) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const weekdays: number[] | null = b.weekdays?.length ? b.weekdays : null
    // Phase 2 S3 review fix 2: isActive=false gates NEW assignment selection —
    // an inactive shift stays resolvable for HISTORY but cannot be assigned to
    // new days anymore.
    if (b.shiftId) {
      const shift = await db.shift.findUnique({ where: { id: b.shiftId }, select: { isActive: true } })
      if (!shift || !shift.isActive) throw new ApiError(400, 'شیفت غیرفعال را نمی‌توان برای تخصیص جدید انتخاب کرد')
    }
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
    const date = parseDayParam(sp.get('date'))
    if (!personId || !date) throw new ApiError(400, 'پارامتر نامعتبر')
    await db.shiftAssignment.deleteMany({ where: { personId, date } })
    await recalcRange([personId], date, date)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
