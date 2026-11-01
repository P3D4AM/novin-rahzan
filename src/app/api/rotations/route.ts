import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { datesBetween, startOfUTCDay, faWeekday } from '@/lib/jalali'
import { recalcRange } from '@/lib/attendance'

export async function GET() {
  try {
    await requireRole('EMPLOYEE')
    const rotations = await db.shiftRotation.findMany({ orderBy: { name: 'asc' } })
    return NextResponse.json({ rotations })
  } catch (err) {
    return fail(err)
  }
}

/** Apply a rotation: POST { rotationId, personIds, from, to } */
export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    const rotation = await db.shiftRotation.findUnique({ where: { id: b.rotationId } })
    if (!rotation) throw new ApiError(404, 'چرخش یافت نشد')
    const pattern = JSON.parse(rotation.pattern) as Array<{ shiftId?: string; off?: boolean; days: number }>
    const cycleLen = pattern.reduce((a, s) => a + s.days, 0)
    if (!cycleLen) throw new ApiError(400, 'الگوی چرخش خالی است')
    const from = startOfUTCDay(new Date(b.from))
    const to = startOfUTCDay(new Date(b.to))
    if (isNaN(from.getTime()) || isNaN(to.getTime()) || to < from) throw new ApiError(400, 'بازه تاریخ نامعتبر')
    const personIds: string[] = b.personIds || []
    if (!personIds.length) throw new ApiError(400, 'پرسنل انتخاب نشده است')

    // build day -> assignment map for one cycle starting at 'from'
    const cycle: Array<{ shiftId: string | null }> = []
    for (const step of pattern) {
      for (let i = 0; i < step.days; i++) cycle.push({ shiftId: step.off ? null : step.shiftId ?? null })
    }

    let count = 0
    for (const d of datesBetween(from, to)) {
      const idx = datesBetween(from, d).length - 1
      const slot = cycle[idx % cycleLen]
      for (const personId of personIds) {
        await db.shiftAssignment.upsert({
          where: { personId_date: { personId, date: d } },
          create: { personId, date: d, shiftId: slot.shiftId, note: rotation.name },
          update: { shiftId: slot.shiftId, note: rotation.name },
        })
        count++
      }
    }
    await recalcRange(personIds, from, to)
    return NextResponse.json({ ok: true, count, cycleLength: cycleLen })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.name || !b.pattern) throw new ApiError(400, 'نام و الگو الزامی است')
    const rotation = await db.shiftRotation.upsert({
      where: { id: b.id || '_new_' },
      create: { id: undefined, name: b.name, pattern: JSON.stringify(b.pattern), isActive: true },
      update: { name: b.name, pattern: JSON.stringify(b.pattern) },
    })
    return NextResponse.json({ rotation })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    if (!id) throw new ApiError(400, 'شناسه لازم است')
    await db.shiftRotation.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}

void faWeekday
