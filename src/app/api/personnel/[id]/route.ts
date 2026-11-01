import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole } from '@/lib/auth'
import { fail } from '@/lib/api'

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
    await requireRole('ADMIN')
    const { id } = await ctx.params
    const b = await req.json()
    const { id: _id, createdAt, updatedAt, user, ...rest } = b
    const p = await db.personnel.update({ where: { id }, data: { ...rest } })
    return NextResponse.json({ personnel: p })
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
