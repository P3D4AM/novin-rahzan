import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { parseDayParam, parseInstantParam, tehranDayRangeInstantWindow } from '@/lib/time'
import { recalcDayForPerson } from '@/lib/attendance'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    // v1.6.0 (phase 1 stage 3): from/to are DATE-ONLY business days ('YYYY-MM-DD'
    // or marker ISO) → markers.
    // Phase 2 S5 (D1 retired): the punch-instant window is now the TRUE
    // Tehran-day range [Tehran midnight of from, Tehran midnight of to+1) —
    // the SAME window the attendance engine fetches punches over. The old
    // UTC-midnight marker window desynced reads from the engine by the +3:30
    // offset (Tehran 00:00–03:29 punches landed under the previous day's
    // filter; docs/API-DATE-CONTRACTS.md, docs/PHASE2-S5-AUDIT.md S5-A7).
    const from = parseDayParam(sp.get('from'))
    const to = parseDayParam(sp.get('to'))
    if ((sp.get('from') && !from) || (sp.get('to') && !to)) throw new ApiError(400, 'بازه تاریخ نامعتبر')
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
      ...(from && to ? { timestamp: tehranDayRangeInstantWindow(from, to) } : {}),
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
    // Phase 2 S5 (host-tz independence): timestamps are INSTANTS and must carry
    // an explicit timezone (full ISO with Z/±HH:MM) — timezone-less datetimes
    // parsed host-locally before, silently shifting the punch with the server's
    // timezone (docs/API-DATE-CONTRACTS.md, docs/PHASE2-S5-AUDIT.md S5-A3).
    const ts = parseInstantParam(String(b.timestamp))
    if (!ts) throw new ApiError(400, 'قالب زمان نامعتبر — زمان باید ISO کامل با منطقهٔ زمانی باشد (مثال: 2026-11-17T08:30:00+03:30)')
    const punch = await db.punchRecord.create({
      data: {
        personId: b.personId,
        timestamp: ts,
        direction: b.direction || 'AUTO',
        source: 'MANUAL',
        editReason: b.reason || null,
        latitude: b.latitude || null,
        longitude: b.longitude || null,
      },
    })
    await db.auditLog.create({ data: { userId: user.id, action: 'PUNCH_CREATE', entity: 'PunchRecord', entityId: punch.id, details: b.reason || '' } })
    await recalcDayForPerson(b.personId, ts, punch.direction)
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
    // Phase 2 S5: same strict instant contract as POST (full ISO with an
    // explicit timezone — never a host-local guess; see S5-A3).
    const ts = parseInstantParam(String(b.timestamp))
    if (!ts) throw new ApiError(400, 'قالب زمان نامعتبر — زمان باید ISO کامل با منطقهٔ زمانی باشد (مثال: 2026-11-17T08:30:00+03:30)')
    const punch = await db.punchRecord.update({
      where: { id: b.id },
      data: { timestamp: ts, direction: b.direction || old.direction, edited: true, editReason: b.reason || 'ویرایش دستی' },
    })
    await db.auditLog.create({ data: { userId: user.id, action: 'PUNCH_EDIT', entity: 'PunchRecord', entityId: b.id, details: `${old.timestamp.toISOString()} -> ${ts.toISOString()}` } })
    await recalcDayForPerson(old.personId, old.timestamp, old.direction)
    await recalcDayForPerson(punch.personId, punch.timestamp, punch.direction)
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
    await recalcDayForPerson(punch.personId, punch.timestamp, punch.direction)
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}


