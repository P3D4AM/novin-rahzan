import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole } from '@/lib/auth'
import { fail } from '@/lib/api'
import { rematchRawPunches } from '@/lib/raw-import'

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
    const { id: _id, createdAt, updatedAt, user: _user, ...rest } = b
    const p = await db.personnel.update({ where: { id }, data: { ...rest } })
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
