import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole } from '@/lib/auth'
import { fail } from '@/lib/api'
import { rematchRawPunches } from '@/lib/raw-import'
import { recordDefaultShiftChange } from '@/lib/default-shift'
import { parseDayParam, tehranTodayMarker } from '@/lib/time'

export async function GET(_req: NextRequest, ctx: { params: Promise<{ id: string }> }) {
  try {
    const { id } = await ctx.params
    const p = await db.personnel.findUnique({
      where: { id },
      include: { department: true, jobTitle: true, employmentType: true, defaultShift: true, user: { select: { id: true, username: true, role: true } } },
    })
    if (!p) return NextResponse.json({ error: 'یافت نشد' }, { status: 404 })
    return NextResponse.json({ personnel: p })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest, ctx: { params: Promise<{ id: string }> }) {
  try {
    const user = await requireRole('ADMIN')
    const { id } = await ctx.params
    const b = await req.json()
    const { id: _id, createdAt, updatedAt, user: _user, defaultShiftEffectiveFrom, ...rest } = b
    const before = await db.personnel.findUnique({ where: { id }, select: { defaultShiftId: true } })
    const p = await db.personnel.update({ where: { id }, data: { ...rest } })
    // Phase 2 S3 review fix 1: a defaultShiftId CHANGE records its effective
    // date (optional `defaultShiftEffectiveFrom` day param, default TODAY) —
    // historical recalculation resolves the default AS OF each day from the
    // dated history, so changing today's default never moves days before the
    // effective date. Re-submitting the SAME value writes nothing (recalc
    // stability; an unchanged edit is not a change).
    if (before && 'defaultShiftId' in b && b.defaultShiftId !== before.defaultShiftId) {
      const effectiveFrom = parseDayParam(defaultShiftEffectiveFrom) ?? tehranTodayMarker()
      await recordDefaultShiftChange({
        personId: id,
        shiftId: p.defaultShiftId,
        effectiveFrom,
        note: 'ویرایش پرسنل',
        previousShiftId: before.defaultShiftId,
      })
    }
    // v1.3.0 (Phase 1): an edited code/deviceUserId may match UNMATCHED punches — rematch them
    // v1.3.1 (fix 3): the acting admin is recorded in the AuditLog rows
    const rematch = await rematchRawPunches({ userId: user.id }).catch(() => null)
    return NextResponse.json({ personnel: p, ...(rematch ? { rematch } : {}) })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(_req: NextRequest, ctx: { params: Promise<{ id: string }> }) {
  try {
    await requireRole('ADMIN')
    const { id } = await ctx.params
    await db.personnel.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
