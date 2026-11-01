import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

const TABLES = {
  dept: { model: db.department, label: 'دپارتمان' },
  job: { model: db.jobTitle, label: 'عنوان شغلی' },
  emptype: { model: db.employmentType, label: 'نوع استخدام' },
} as const

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const type = req.nextUrl.searchParams.get('type') || 'dept'
    const t = TABLES[type as keyof typeof TABLES]
    if (!t) throw new ApiError(400, 'نوع نامعتبر')
    const items = await t.model.findMany({ orderBy: { name: 'asc' } })
    return NextResponse.json({ items })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const { type, name, description } = await req.json()
    const t = TABLES[type as keyof typeof TABLES]
    if (!t || !name) throw new ApiError(400, 'نام الزامی است')
    const item = await t.model.create({ data: { name, ...(description ? { description } : {}) } })
    return NextResponse.json({ item })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const sp = req.nextUrl.searchParams
    const type = sp.get('type') || ''
    const id = sp.get('id') || ''
    const t = TABLES[type as keyof typeof TABLES]
    if (!t || !id) throw new ApiError(400, 'پارامتر نامعتبر')
    await t.model.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}
