import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { rematchRawPunches } from '@/lib/raw-import'
import { recordDefaultShiftChange } from '@/lib/default-shift'
import { parseDayParam, tehranTodayMarker } from '@/lib/time'

export async function GET(req: NextRequest) {
  try {
    const user = await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const q = sp.get('q')?.trim() || ''
    const departmentId = sp.get('departmentId') || ''
    const status = sp.get('status') || ''
    const all = sp.get('all') === '1'

    const where: Record<string, unknown> = {}
    if (q) {
      where.OR = [
        { firstName: { contains: q } },
        { lastName: { contains: q } },
        { personnelCode: { contains: q } },
        { nationalId: { contains: q } },
      ]
    }
    if (departmentId) where.departmentId = departmentId
    if (status) where.status = status

    // employees can only see basic list of colleagues
    const personnel = await db.personnel.findMany({
      where,
      include: {
        department: { select: { name: true } },
        jobTitle: { select: { name: true } },
        employmentType: { select: { name: true } },
        defaultShift: { select: { id: true, name: true, color: true } },
      },
      orderBy: [{ status: 'asc' }, { personnelCode: 'asc' }],
      ...(all ? {} : { take: 500 }),
    })
    const canSeeSensitive = user.role !== 'EMPLOYEE'
    return NextResponse.json({
      personnel: personnel.map((p) => ({
        ...p,
        nationalId: canSeeSensitive ? p.nationalId : undefined,
        phone: canSeeSensitive ? p.phone : undefined,
        annualLeaveDays: p.annualLeaveDays,
      })),
    })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('ADMIN')
    const b = await req.json()
    if (!b.personnelCode || !b.firstName || !b.lastName) throw new ApiError(400, 'کد پرسنلی، نام و نام خانوادگی الزامی است')
    const dup = await db.personnel.findFirst({ where: { OR: [{ personnelCode: b.personnelCode }, ...(b.pin ? [{ pin: b.pin }] : [])] } })
    if (dup) throw new ApiError(400, 'کد پرسنلی یا پین تکراری است')
    const { id, createdAt, updatedAt, defaultShiftEffectiveFrom, ...rest } = b
    const p = await db.personnel.create({ data: { ...rest } })
    // Phase 2 S3 review fix 1: the initial default shift gets a DATED history
    // row (baseline) — effective today, or the explicitly passed day marker —
    // so later changes never move the pre-effective-date past.
    if (p.defaultShiftId) {
      const effectiveFrom = parseDayParam(defaultShiftEffectiveFrom) ?? tehranTodayMarker()
      await recordDefaultShiftChange({ personId: p.id, shiftId: p.defaultShiftId, effectiveFrom, note: 'ایجاد پرسنل', previousShiftId: null })
    }
    // v1.3.0 (Phase 1): a new personnel member may match previously UNMATCHED
    // device punches (deviceUserId / personnelCode) — rematch them now.
    // v1.3.1 (fix 3): the acting admin is recorded in the AuditLog rows; a
    // rematch failure no longer hides behind a null (per-row failures are
    // reported and the rows stay UNMATCHED + recoverable).
    const rematch = await rematchRawPunches({ userId: user.id }).catch(() => null)
    return NextResponse.json({ personnel: p, ...(rematch ? { rematch } : {}) })
  } catch (err) {
    return fail(err)
  }
}
