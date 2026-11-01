import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET() {
  try {
    await requireRole('ADMIN')
    const items = await db.notificationRule.findMany({ orderBy: { type: 'asc' } })
    return NextResponse.json({ items })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    const item = await db.notificationRule.update({
      where: { id: b.id },
      data: { enabled: b.enabled ?? undefined, thresholdMin: b.thresholdMin ?? undefined, notifyRole: b.notifyRole ?? undefined },
    })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}
