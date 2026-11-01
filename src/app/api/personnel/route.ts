import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'

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
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.personnelCode || !b.firstName || !b.lastName) throw new ApiError(400, 'کد پرسنلی، نام و نام خانوادگی الزامی است')
    const dup = await db.personnel.findFirst({ where: { OR: [{ personnelCode: b.personnelCode }, ...(b.pin ? [{ pin: b.pin }] : [])] } })
    if (dup) throw new ApiError(400, 'کد پرسنلی یا پین تکراری است')
    const { id, createdAt, updatedAt, ...rest } = b
    const p = await db.personnel.create({ data: { ...rest } })
    return NextResponse.json({ personnel: p })
  } catch (err) {
    return fail(err)
  }
}
