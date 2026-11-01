import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireAuth } from '@/lib/auth'
import { fail } from '@/lib/api'
import { runAbsenceSweep } from '@/lib/notify'

export async function GET(req: NextRequest) {
  try {
    const user = await requireAuth()
    const sp = req.nextUrl.searchParams
    const all = sp.get('all') === '1'
    const where = all
      ? { OR: [{ userId: user.id }, { role: user.role }] }
      : { OR: [{ userId: user.id }, { role: user.role }], read: false }
    const items = await db.notification.findMany({ where, orderBy: { createdAt: 'desc' }, take: 100 })
    const unread = await db.notification.count({ where: { OR: [{ userId: user.id }, { role: user.role }], read: false } })
    return NextResponse.json({ items, unread })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    const user = await requireAuth()
    const b = await req.json().catch(() => ({}))
    if (b.id) {
      await db.notification.updateMany({ where: { id: b.id, OR: [{ userId: user.id }, { role: user.role }] }, data: { read: true } })
    } else {
      await db.notification.updateMany({ where: { OR: [{ userId: user.id }, { role: user.role }], read: false }, data: { read: true } })
    }
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}

/** run absence sweep manually */
export async function POST() {
  try {
    const { requireRole } = await import('@/lib/auth')
    await requireRole('ADMIN')
    const result = await runAbsenceSweep()
    return NextResponse.json(result)
  } catch (err) {
    return fail(err)
  }
}
