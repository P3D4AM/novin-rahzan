import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

export async function GET() {
  try {
    await requireRole('EMPLOYEE')
    const items = await db.setting.findMany()
    const map: Record<string, string> = {}
    for (const s of items) map[s.id] = s.value
    return NextResponse.json({ settings: map })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.settings || typeof b.settings !== 'object') throw new ApiError(400, 'داده نامعتبر')
    for (const [id, value] of Object.entries(b.settings)) {
      await db.setting.upsert({ where: { id }, create: { id, value: String(value) }, update: { value: String(value) } })
    }
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
