import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET() {
  try {
    await requireRole('EMPLOYEE')
    const shifts = await db.shift.findMany({ include: { segments: { orderBy: { order: 'asc' } } }, orderBy: { name: 'asc' } })
    return NextResponse.json({ shifts })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.name) throw new ApiError(400, 'نام شیفت الزامی است')
    if (!b.segments?.length) throw new ApiError(400, 'حداقل یک بازه زمانی لازم است')
    const { id, segments, ...rest } = b
    const shift = await db.shift.create({
      data: {
        ...rest,
        segments: {
          create: segments.map((s: { startMin: number; endMin: number; breakMin?: number }, i: number) => ({
            order: i + 1, startMin: s.startMin, endMin: s.endMin, breakMin: s.breakMin ?? 0,
          })),
        },
      },
      include: { segments: true },
    })
    return NextResponse.json({ shift })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    const { id, segments, ...rest } = b
    await db.shiftSegment.deleteMany({ where: { shiftId: id } })
    const shift = await db.shift.update({
      where: { id },
      data: {
        ...rest,
        ...(segments
          ? {
              segments: {
                create: segments.map((s: { startMin: number; endMin: number; breakMin?: number }, i: number) => ({
                  order: i + 1, startMin: s.startMin, endMin: s.endMin, breakMin: s.breakMin ?? 0,
                })),
              },
            }
          : {}),
      },
      include: { segments: { orderBy: { order: 'asc' } } },
    })
    return NextResponse.json({ shift })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    if (!id) throw new ApiError(400, 'شناسه لازم است')
    const inUse = await db.personnel.count({ where: { defaultShiftId: id } })
    if (inUse > 0) throw new ApiError(400, 'این شیفت به پرسنل اختصاص یافته است')
    await db.shiftSegment.deleteMany({ where: { shiftId: id } })
    await db.shift.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
