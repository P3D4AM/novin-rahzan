import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError, hashPassword } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET() {
  try {
    await requireRole('ADMIN')
    const users = await db.user.findMany({
      select: { id: true, username: true, role: true, fullName: true, active: true, personnelId: true, createdAt: true },
      orderBy: { username: 'asc' },
    })
    const logs = await db.auditLog.findMany({ orderBy: { createdAt: 'desc' }, take: 100 })
    return NextResponse.json({ users, logs })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.username || !b.password || !b.role) throw new ApiError(400, 'نام کاربری، رمز و نقش الزامی است')
    const dup = await db.user.findUnique({ where: { username: b.username } })
    if (dup) throw new ApiError(400, 'نام کاربری تکراری است')
    const user = await db.user.create({
      data: {
        username: b.username, passwordHash: hashPassword(b.password), role: b.role,
        fullName: b.fullName || b.username, personnelId: b.personnelId || null, active: true,
      },
      select: { id: true, username: true, role: true, fullName: true, active: true, personnelId: true },
    })
    return NextResponse.json({ user })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    const data: Record<string, unknown> = {}
    if (b.password) data.passwordHash = hashPassword(b.password)
    if (b.role) data.role = b.role
    if (b.fullName) data.fullName = b.fullName
    if (b.personnelId !== undefined) data.personnelId = b.personnelId || null
    if (b.active !== undefined) data.active = !!b.active
    const user = await db.user.update({ where: { id: b.id }, data, select: { id: true, username: true, role: true, fullName: true, active: true, personnelId: true } })
    return NextResponse.json({ user })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    const me = await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    if (id === me.id) throw new ApiError(400, 'نمی‌توانید خودتان را حذف کنید')
    await db.session.deleteMany({ where: { userId: id } })
    await db.user.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
