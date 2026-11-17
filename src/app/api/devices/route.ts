import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail, errorMessage } from '@/lib/api'
import { zkTest, zkGetUsers, importDeviceLogs } from '@/lib/zk'
import { getEbioStatus } from '@/lib/ebio'
import { npTest } from '@/lib/sbxpc'
import { importNpDeviceLogs } from '@/lib/np-sync'
import { emitRealtime } from '@/lib/notify'
import { sanitizeDeviceForApi, sanitizeResultForApi, redactDeviceSecrets } from '@/lib/device-secrets'
import { getDevicePassword, storeDevicePassword } from '@/lib/device-secret-store'
import { validateDeviceSettings } from '@/lib/device-validation'
import { formatTehranDateTime } from '@/lib/time'
import { fkTestConnection, importFkAttendLogs, SyncBusyError } from '@/lib/fkattend'

export async function GET(req: NextRequest) {
  try {
    await requireRole('EMPLOYEE')
    const sp = req.nextUrl.searchParams
    const withLogs = sp.get('logs') === '1'
    const withSyncs = sp.get('syncs') === '1'
    // v1.5.0: ?bridge=1 — STATUS-ONLY view of the internal FKAttend bridge
    // service (never starts anything, never exposes the token or any secret).
    // Lets the devices UI show the service state the user no longer manages.
    const withBridge = sp.get('bridge') === '1'
    const devices = await db.device.findMany({ orderBy: { name: 'asc' } })
    // v1.3.0 (Phase 1 — Security): the device password is a secret at rest;
    // it must never leave the process boundary. UI copy uses hasPassword.
    const logs = withLogs
      ? (await db.deviceLog.findMany({ orderBy: { createdAt: 'desc' }, take: 50 })).map((l) => ({
          ...l,
          message: redactDeviceSecrets(l.message), // v1.2.x rows may contain a password VALUE
        }))
      : []
    // v1.4.0 (addendum P2): sync history — sanitized (error/note never carry secrets)
    const syncs = withSyncs
      ? (await db.deviceSyncRun.findMany({ orderBy: { startedAt: 'desc' }, take: 20 })).map((r) => ({
          ...r,
          error: r.error ? redactDeviceSecrets(r.error) : null,
          note: r.note ? redactDeviceSecrets(r.note) : null,
        }))
      : []
    let bridge: import('@/lib/bridge-supervisor').BridgeStatus | undefined
    if (withBridge) {
      try {
        const { getBridgeStatus } = await import('@/lib/bridge-supervisor')
        bridge = getBridgeStatus()
      } catch { /* status is best-effort; the devices list still loads */ }
    }
    return NextResponse.json({ devices: devices.map(sanitizeDeviceForApi), logs, syncs, ...(bridge ? { bridge } : {}) })
  } catch (err) {
    return fail(err)
  }
}

export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    // v1.4.0 (addendum P1): ALL connection settings go through the shared backend
    // validator — port defaults per protocol family (FKATTEND → 4238), explicit
    // family selection, interval/timeout ranges. v1.5.0: bridge settings are
    // INTERNAL (supervisor-managed) — legacy bridgeUrl/bridgeToken in the payload
    // are ignored and never stored.
    const v = validateDeviceSettings(b, { isCreate: true })
    if (!v.ok) throw new ApiError(400, v.error)
    const d = v.data
    // v1.3.1 (fix 6): the at-rest representation of the network password is
    // decided in ONE place — storeDevicePassword (DPAPI when opted in on
    // Windows, plaintext column otherwise; the gap stays documented).
    const stored = await storeDevicePassword(d.password ?? null)
    const device = await db.device.create({
      data: {
        name: d.name, type: d.type, protocolFamily: d.protocolFamily,
        ipAddress: d.ipAddress, port: d.port, protocol: d.protocol,
        location: d.location, enabled: d.enabled, serialNumber: d.serialNumber,
        connTimeoutMs: d.connTimeoutMs, machineNumber: d.machineNumber, sdkLicense: d.sdkLicense,
        syncIntervalMin: d.syncIntervalMin, rtEnabled: d.rtEnabled, rtPollSec: d.rtPollSec,
        password: stored.password,
        passwordEnc: stored.passwordEnc,
      },
    })
    if (stored.stored === 'plaintext' && d.password != null) {
      // the operator asked for protection but the platform cannot provide it
      // — stored plaintext on purpose (never lose a credential); make it visible
      await db.deviceLog.create({
        data: {
          deviceId: device.id,
          level: 'WARN',
          message: 'رمز شبکه بدون محافظت در پایگاه‌داده ذخیره شد — محافظت DPAPI فقط روی Windows و با فعال‌بودن HOZOR_DEVICE_SECRET_PROTECTION فعال می‌شود (محدودیت مستندشده)',
        },
      }).catch(() => {})
    }
    return NextResponse.json({ device: sanitizeDeviceForApi(device) })
  } catch (err) {
    return fail(err)
  }
}

export async function PUT(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const b = await req.json()
    if (!b.id) throw new ApiError(400, 'شناسه لازم است')
    const existing = await db.device.findUnique({ where: { id: b.id } })
    if (!existing) throw new ApiError(404, 'دستگاه یافت نشد')
    // v1.4.0: full backend validation on edit too. The password follows the
    // Phase 1 semantics: field absent = keep the stored secret (the edit
    // form starts empty because GET never echoes secrets). v1.5.0: bridge
    // settings are internal — legacy bridge fields are ignored.
    const v = validateDeviceSettings(b, {
      isCreate: false,
      existing: {
        protocolFamily: existing.protocolFamily,
        port: existing.port,
        password: existing.password,
        type: existing.type,
      },
    })
    if (!v.ok) throw new ApiError(400, v.error)
    const d = v.data
    // v1.3.1 (fix 6): an explicitly provided password goes through the single
    // at-rest decision point (DPAPI when opted in on Windows). An absent field
    // keeps BOTH stored columns untouched (Phase 1 "keep" semantics).
    const passwordData =
      d.password !== undefined
        ? await storeDevicePassword(d.password)
        : null
    const device = await db.device.update({
      where: { id: b.id },
      data: {
        name: d.name, type: d.type, protocolFamily: d.protocolFamily,
        ipAddress: d.ipAddress, port: d.port, protocol: d.protocol,
        location: d.location, enabled: d.enabled, serialNumber: d.serialNumber,
        connTimeoutMs: d.connTimeoutMs, machineNumber: d.machineNumber, sdkLicense: d.sdkLicense,
        syncIntervalMin: d.syncIntervalMin, rtEnabled: d.rtEnabled, rtPollSec: d.rtPollSec,
        ...(passwordData ? { password: passwordData.password, passwordEnc: passwordData.passwordEnc } : {}),
      },
    })
    if (passwordData && passwordData.stored === 'plaintext' && d.password != null) {
      await db.deviceLog.create({
        data: {
          deviceId: device.id,
          level: 'WARN',
          message: 'رمز شبکه بدون محافظت در پایگاه‌داده ذخیره شد — محافظت DPAPI فقط روی Windows و با فعال‌بودن HOZOR_DEVICE_SECRET_PROTECTION فعال می‌شود (محدودیت مستندشده)',
        },
      }).catch(() => {})
    }
    return NextResponse.json({ device: sanitizeDeviceForApi(device) })
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
      /* v1.4.0 (addendum P0/P1): dispatch by the EXPLICITLY selected protocol family.
         FKATTEND — official SDK via the x86 bridge (read-only, port 4238 default). */
      if (device.protocolFamily === 'FKATTEND') {
        if (b.action === 'test') {
          const t = await fkTestConnection(device)
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
              message: redactDeviceSecrets(
                t.ok
                  ? `اتصال از طریق Bridge FKAttend برقرار شد — پروتکل: FKAttend · پورت ${device.port} · سریال: ${t.serialNumber || '— (بدون پروب سریال)'} · زمان پاسخ: ${t.latencyMs ?? '?'} میلی‌ثانیه${t.deviceTime ? ` · ساعت دستگاه: ${t.deviceTime}` : ''}${t.serialMatch === false ? ' (⚠️ با سریال ثبت‌شده در پرونده دستگاه یکی نیست)' : ''}`
                  : `خطای اتصال از طریق Bridge FKAttend: ${t.error ?? 'نامشخص'}`,
              ),
            },
          })
          // the password/bridge token never leave the process — only indicators
          return NextResponse.json({ result: sanitizeResultForApi(t as unknown as Record<string, unknown>) })
        }
        if (b.action === 'sync') {
          const daysBack = typeof b.daysBack === 'number' && b.daysBack > 0 ? b.daysBack : null
          const full = b.full === true
          try {
            const result = await importFkAttendLogs(device.id, { daysBack, full })
            await emitRealtime('device-sync', { deviceId: device.id, ...result })
            return NextResponse.json({ result })
          } catch (err) {
            if (err instanceof SyncBusyError) throw new ApiError(409, err.message)
            throw err
          }
        }
        throw new ApiError(400, 'برای دستگاه‌های FKAttend فقط «تست اتصال» و «دانلود ترددها» پشتیبانی می‌شود')
      }

      if (b.action === 'test') {
        if (!device.ipAddress?.trim()) {
          throw new ApiError(400,
            'برای تست اتصال، آدرس IP دستگاه را در پرونده دستگاه وارد کنید (مثل 192.168.1.201). ' +
            'دستگاه‌های NP روی پورت 5005 هم به نرم‌افزار وصل می‌شوند (پوش لحظه‌ای) و هم از همان پورت پاسخ فرمان‌های دانلود را می‌دهند.')
        }
        // v1.3.1 (fix 6): the working password may live in the protected
        // column — one read helper decides (decrypts only inside the process)
        const workingPassword = await getDevicePassword(device)
        const t = await npTest(device.ipAddress.trim(), device.port || 5005, 1, workingPassword)
        const serialMatch = t.ok && t.serialNumber && device.serialNumber
          ? t.serialNumber.trim().toLowerCase() === device.serialNumber.trim().toLowerCase()
          : null
        const dialectFa: Record<string, string> = {
          CLASSIC: 'پروتکل کلاسیک (۱۶بایتی)',
          RESULT_DIRECT: 'پروتکل کلاسیک بدون ACK',
          EXT: 'پروتکل ۱۲بایتی (EXT)',
          EXT_SECURE: 'پروتکل رمزشدهٔ NP (با «رمز شبکه»)',
          CLASSIC_SECURE: 'پروتکل کلاسیک رمزشده (با «رمز شبکه»)',
          HTTP_JSON: 'API HTTP — نسل جدید NP (VisibleLight)',
        }
        // remember a newly discovered «رمز شبکه» so syncs reuse it — through
        // the single at-rest decision point, and only when NOTHING is stored yet
        const discovered = t.ok && t.password != null && device.password == null && device.passwordEnc == null
          ? await storeDevicePassword(t.password)
          : null
        await db.device.update({
          where: { id: device.id },
          data: {
            status: t.ok ? 'ONLINE' : 'OFFLINE',
            ...(t.ok && t.serialNumber && !device.serialNumber ? { serialNumber: t.serialNumber } : {}),
            ...(discovered ? { password: discovered.password, passwordEnc: discovered.passwordEnc } : {}),
          },
        })
        await db.deviceLog.create({
          data: {
            deviceId: device.id,
            level: t.ok ? 'SUCCESS' : 'ERROR',
            message: redactDeviceSecrets(
              t.ok
                ? `اتصال مستقیم برقرار شد — سریال: ${t.serialNumber || '—'}${t.dialect ? ` · ${dialectFa[t.dialect] ?? t.dialect}` : ''}${t.password != null ? ' · رمز شبکه: تنظیم شد (مقدار ذخیره شد و نمایش داده نمی‌شود)' : ''}${t.logCount != null ? ` · ترددهای ذخیره‌شده روی دستگاه: ${t.logCount}` : ''}${t.deviceTime ? ` · ساعت دستگاه: ${formatTehranDateTime(new Date(t.deviceTime), { fa: true })}` : ''}${t.note ? ` · ${t.note}` : ''}${serialMatch === false ? ' (⚠️ با سریال ثبت‌شده در پرونده دستگاه یکی نیست)' : ''}`
                : `خطای اتصال مستقیم: ${t.error}${t.detail ? ` [${t.detail}]` : ''}${t.note ? ` · ${t.note}` : ''}${t.raw ? ` · فریم‌های خام:\n${t.raw}` : ''}`,
            ),
          },
        })
        // v1.3.0: the password VALUE never leaves the API — only a boolean indicator
        return NextResponse.json({ result: { ...sanitizeResultForApi(t as unknown as Record<string, unknown>), serialMatch } })
      }

      if (b.action === 'sync') {
        const daysBack = typeof b.daysBack === 'number' && b.daysBack > 0 ? b.daysBack : null
        try {
          const result = await importNpDeviceLogs(device.id, { daysBack })
          await emitRealtime('device-sync', { deviceId: device.id, ...result })
          return NextResponse.json({ result })
        } catch (err) {
          if (err instanceof SyncBusyError) throw new ApiError(409, err.message)
          throw err
        }
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
