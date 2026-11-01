import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { addDays, startOfUTCDay, tehranDayOf } from '@/lib/jalali'
import { recalcDayForPerson } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const from = sp.get('from') ? new Date(sp.get('from')!) : null
    const to = sp.get('to') ? new Date(sp.get('to')!) : null
    const personId = sp.get('personId') || undefined
    const source = sp.get('source') || undefined
    const q = sp.get('q')?.trim() || ''
    const page = Math.max(1, Number(sp.get('page') || 1))
    const pageSize = Math.min(200, Number(sp.get('pageSize') || 50))

    let personIds: string[] | undefined
    if (q) {
      const matched = await db.personnel.findMany({
        where: { OR: [{ firstName: { contains: q } }, { lastName: { contains: q } }, { personnelCode: { contains: q } }] },
        select: { id: true },
      })
      personIds = matched.map((m) => m.id)
      if (!personIds.length) return NextResponse.json({ punches: [], total: 0 })
    }

    const where: Record<string, unknown> = {
      ...(from && to ? { timestamp: { gte: from, lt: addDays(to, 1) } } : {}),
      ...(personId ? { personId } : {}),
      ...(personIds ? { personId: { in: personIds } } : {}),
      ...(source ? { source } : {}),
    }

    const total = await db.punchRecord.count({ where })
    const punches = await db.punchRecord.findMany({
      where,
      include: { person: { select: { id: true, firstName: true, lastName: true, personnelCode: true } } },
      orderBy: { timestamp: 'desc' },
      skip: (page - 1) * pageSize,
      take: pageSize,
    })
    return NextResponse.json({ punches, total })
  } catch (err) {
    return fail(err)
  }
}

/** manual punch (admin) or edit existing */
export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('ADMIN')
    const b = await req.json()
    if (!b.personId || !b.timestamp) throw new ApiError(400, 'پرسنل و زمان الزامی است')
    const punch = await db.punchRecord.create({
      data: {
        personId: b.personId,
        timestamp: new Date(b.timestamp),
        direction: b.direction || 'AUTO',
        source: 'MANUAL',
        editReason: b.reason || null,
        latitude: b.latitude || null,
        longitude: b.longitude || null,
      },
    })
    await db.auditLog.create({ data: { userId: user.id, action: 'PUNCH_CREATE', entity: 'PunchRecord', entityId: punch.id, details: b.reason || '' } })
    await recalcDayForPerson(b.personId, new Date(b.timestamp))
    return NextResponse.json({ punch })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    const user = await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id || !b.timestamp) throw new ApiError(400, 'شناسه و زمان الزامی است')
    const old = await db.punchRecord.findUnique({ where: { id: b.id } })
    if (!old) throw new ApiError(404, 'تردد یافت نشد')
    const punch = await db.punchRecord.update({
      where: { id: b.id },
      data: { timestamp: new Date(b.timestamp), direction: b.direction || old.direction, edited: true, editReason: b.reason || 'ویرایش دستی' },
    })
    await db.auditLog.create({ data: { userId: user.id, action: 'PUNCH_EDIT', entity: 'PunchRecord', entityId: b.id, details: `${old.timestamp.toISOString()} -> ${b.timestamp}` } })
    await recalcDayForPerson(old.personId, old.timestamp)
    await recalcDayForPerson(punch.personId, punch.timestamp)
    return NextResponse.json({ punch })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    const user = await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    const punch = await db.punchRecord.findUnique({ where: { id } })
    if (!punch) throw new ApiError(404, 'تردد یافت نشد')
    await db.punchRecord.delete({ where: { id } })
    await db.auditLog.create({ data: { userId: user.id, action: 'PUNCH_DELETE', entity: 'PunchRecord', entityId: id } })
    await recalcDayForPerson(punch.personId, punch.timestamp)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}


