import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { zkTest, zkGetUsers, importDeviceLogs } from '@/lib/zk'
import { emitRealtime } from '@/lib/notify'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const withLogs = sp.get('logs') === '1'
    const devices = await db.device.findMany({ orderBy: { name: 'asc' } })
    const logs = withLogs ? await db.deviceLog.findMany({ orderBy: { createdAt: 'desc' }, take: 50 }) : []
    return NextResponse.json({ devices, logs })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.name) throw new ApiError(400, 'نام دستگاه الزامی است')
    const device = await db.device.create({
      data: {
        name: b.name, type: b.type || 'ZKTECO', ipAddress: b.ipAddress || null, port: Number(b.port || 4370),
        protocol: b.protocol || 'TCP', location: b.location || null, enabled: b.enabled ?? true,
      },
    })
    return NextResponse.json({ device })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    const device = await db.device.update({
      where: { id: b.id },
      data: {
        name: b.name, ipAddress: b.ipAddress, port: Number(b.port || 4370), protocol: b.protocol,
        location: b.location, enabled: b.enabled, syncIntervalMin: Number(b.syncIntervalMin || 0),
      },
    })
    return NextResponse.json({ device })
  } catch (err) {
    return fail(err)
  }
}

export async function DELETE(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const id = req.nextUrl.searchParams.get('id') || ''
    await db.device.delete({ where: { id } })
    return NextResponse.json({ ok: true })
  } catch (err) {
    return fail(err)
  }
}

/** device actions: test | sync | users */
export async function PATCH(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id || !b.action) throw new ApiError(400, 'شناسه و عملیات الزامی است')
    const device = await db.device.findUnique({ where: { id: b.id } })
    if (!device) throw new ApiError(404, 'دستگاه یافت نشد')

    if (b.action === 'test') {
      if (!device.ipAddress) throw new ApiError(400, 'آدرس IP تنظیم نشده است')
      const t = await zkTest(device.ipAddress, device.port, device.protocol as 'TCP' | 'UDP')
      await db.device.update({ where: { id: device.id }, data: { status: t.ok ? 'ONLINE' : 'OFFLINE', serialNumber: t.ok ? (t.info?.serialNumber ?? device.serialNumber) : device.serialNumber } })
      await db.deviceLog.create({ data: { deviceId: device.id, level: t.ok ? 'SUCCESS' : 'ERROR', message: t.ok ? `اتصال موفق - فریمور: ${t.info?.firmware} - کاربران: ${t.info?.userCounts} - ترددها: ${t.info?.logCounts}` : `خطای اتصال: ${t.error}` } })
      return NextResponse.json({ result: t })
    }

    if (b.action === 'users') {
      if (!device.ipAddress) throw new ApiError(400, 'آدرس IP تنظیم نشده است')
      const users = await zkGetUsers(device.ipAddress, device.port, device.protocol as 'TCP' | 'UDP')
      await db.deviceLog.create({ data: { deviceId: device.id, level: 'INFO', message: `دریافت ${users.length} کاربر از دستگاه` } })
      return NextResponse.json({ users })
    }

    if (b.action === 'sync') {
      const result = await importDeviceLogs(device.id)
      await emitRealtime('device-sync', { deviceId: device.id, ...result })
      return NextResponse.json({ result })
    }

    throw new ApiError(400, 'عملیات نامعتبر')
  } catch (err) {
    return fail(err)
  }
}
