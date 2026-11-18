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
      // Phase 2 S3 (min_excess): the overtime threshold Setting is consumed by
      // the calculation engine — an invalid value must be REJECTED here, never
      // silently stored (the engine also re-validates on read and falls back
      // to the documented default 30).
      if (id === 'ot_threshold_minutes') {
        const raw = String(value).trim()
        const n = Number(raw)
        if (!/^\d+$/.test(raw) || !Number.isInteger(n) || n < 0) {
          throw new ApiError(400, 'آستانه اضافه‌کاری باید عدد صحیح و نامنفی (دقیقه) باشد')
        }
      }
      await db.setting.upsert({ where: { id }, create: { id, value: String(value) }, update: { value: String(value) } })
    }
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
