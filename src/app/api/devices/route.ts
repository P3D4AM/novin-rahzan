import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail, errorMessage } from '@/lib/api'
import { zkTest, zkGetUsers, importDeviceLogs } from '@/lib/zk'
import { getEbioStatus } from '@/lib/ebio'
import { npTest } from '@/lib/sbxpc'
import { importNpDeviceLogs } from '@/lib/np-sync'
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
    const type = b.type === 'NOVIN_NP' ? 'NOVIN_NP' : 'ZKTECO'
    if (type === 'NOVIN_NP' && !b.serialNumber?.trim())
      throw new ApiError(400, 'برای دستگاه‌های نوین پرداز (NP) شماره سریال الزامی است — مانند نرم‌افزار اصلی، پیام‌های دستگاه با سریال مطابقت داده می‌شوند')
    const device = await db.device.create({
      data: {
        name: b.name, type, ipAddress: b.ipAddress || null, port: Number(b.port || 5005),
        protocol: b.protocol || 'TCP', location: b.location || null, enabled: b.enabled ?? true,
        serialNumber: b.serialNumber?.trim() || null,
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
    const type = b.type === 'NOVIN_NP' ? 'NOVIN_NP' : 'ZKTECO'
    if (type === 'NOVIN_NP' && !b.serialNumber?.trim())
      throw new ApiError(400, 'برای دستگاه‌های نوین پرداز (NP) شماره سریال الزامی است')
    const device = await db.device.update({
      where: { id: b.id },
      data: {
        name: b.name, type, ipAddress: b.ipAddress || null, port: Number(b.port || 5005), protocol: b.protocol,
        location: b.location || null, enabled: b.enabled, syncIntervalMin: Number(b.syncIntervalMin || 0),
        serialNumber: b.serialNumber?.trim() || null,
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

/** device actions: test | sync | users | gateway */
export async function PATCH(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.action) throw new ApiError(400, 'عملیات الزامی است')

    if (b.action === 'gateway') {
      return NextResponse.json({ result: getEbioStatus() })
    }

    if (!b.id) throw new ApiError(400, 'شناسه و عملیات الزامی است')
    const device = await db.device.findUnique({ where: { id: b.id } })
    if (!device) throw new ApiError(404, 'دستگاه یافت نشد')

    /* ---------------- Novin Pardaz NP devices (pull mode) ---------------- */
    if (device.type === 'NOVIN_NP') {
      if (b.action === 'test') {
        if (!device.ipAddress?.trim()) {
          throw new ApiError(400,
            'برای تست اتصال، آدرس IP دستگاه را در پرونده دستگاه وارد کنید (مثل 192.168.1.201). ' +
            'دستگاه‌های NP روی پورت 5005 هم به نرم‌افزار وصل می‌شوند (پوش لحظه‌ای) و هم از همان پورت پاسخ فرمان‌های دانلود را می‌دهند.')
        }
        const t = await npTest(device.ipAddress.trim(), device.port || 5005)
        const serialMatch = t.ok && t.serialNumber && device.serialNumber
          ? t.serialNumber.trim().toLowerCase() === device.serialNumber.trim().toLowerCase()
          : null
        await db.device.update({
          where: { id: device.id },
          data: {
            status: t.ok ? 'ONLINE' : 'OFFLINE',
            ...(t.ok && t.serialNumber && !device.serialNumber ? { serialNumber: t.serialNumber } : {}),
          },
        })
        await db.deviceLog.create({
          data: {
            deviceId: device.id,
            level: t.ok ? 'SUCCESS' : 'ERROR',
            message: t.ok
              ? `اتصال مستقیم برقرار شد — سریال: ${t.serialNumber}${t.deviceTime ? ` · ساعت دستگاه: ${new Date(t.deviceTime).toLocaleString('fa-IR')}` : ''}${serialMatch === false ? ' (⚠️ با سریال ثبت‌شده در پرونده دستگاه یکی نیست)' : ''}`
              : `خطای اتصال مستقیم: ${t.error}${t.detail ? ` [${t.detail}]` : ''}`,
          },
        })
        return NextResponse.json({ result: { ...t, serialMatch } })
      }

      if (b.action === 'sync') {
        const daysBack = typeof b.daysBack === 'number' && b.daysBack > 0 ? b.daysBack : null
        const result = await importNpDeviceLogs(device.id, { daysBack })
        await emitRealtime('device-sync', { deviceId: device.id, ...result })
        return NextResponse.json({ result })
      }

      throw new ApiError(400, 'برای دستگاه‌های نوین پرداز فقط «تست اتصال» و «دانلود ترددها» پشتیبانی می‌شود')
    }

    /* ---------------- ZKTeco & compatible ---------------- */

    if (b.action === 'test') {
      if (!device.ipAddress) throw new ApiError(400, 'آدرس IP تنظیم نشده است')
      const t = await zkTest(device.ipAddress, device.port, device.protocol as 'TCP' | 'UDP')
      await db.device.update({ where: { id: device.id }, data: { status: t.ok ? 'ONLINE' : 'OFFLINE', serialNumber: t.ok ? (t.info?.serialNumber || device.serialNumber) : device.serialNumber } })
      await db.deviceLog.create({ data: { deviceId: device.id, level: t.ok ? 'SUCCESS' : 'ERROR', message: t.ok ? `اتصال موفق - فریمور: ${t.info?.firmware} - کاربران: ${t.info?.userCounts} - ترددها: ${t.info?.logCounts}` : `خطای اتصال: ${t.error ?? errorMessage(t)}` } })
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
