/**
 * NP-series device pull-sync: download attendance logs straight from the
 * device over the SBXPC binary protocol (see lib/sbxpc.ts), import them into
 * PunchRecord and keep an incremental bookmark (Device.lastLogAt) so later
 * syncs — manual or automatic — continue exactly where the previous one
 * stopped instead of re-importing old punches.
 */
import { db } from '@/lib/db'
import { SbxpcClient, NpLog } from '@/lib/sbxpc'
import { recalcDayForPerson } from '@/lib/attendance'
import { emitRealtime } from '@/lib/notify'
import { errorMessage } from '@/lib/api'

/** how far back a first-ever sync goes when nothing else is known */
export const DEFAULT_FIRST_SYNC_DAYS = 30

export interface NpSyncOptions {
  /** manual "download N days back" (null/undefined = use bookmark / default) */
  daysBack?: number | null
}

export interface NpSyncResult {
  created: number
  skipped: number
  unmatched: number
  total: number
  imported: number
  cutoff: string
  newBookmark: string | null
  deviceSerial?: string
  deviceTime?: string | null
  serialMismatch?: boolean
}

type DeviceRow = {
  id: string
  name: string
  ipAddress: string | null
  port: number
  serialNumber: string | null
  enabled: boolean
  lastLogAt: Date | null
}

/** best-effort IN/OUT from the vendor verify-mode ("IoMode") value */
function directionFromVerifyMode(v: number): 'IN' | 'OUT' | 'AUTO' {
  if (v === 1 || v === 4 || v === 10 || v === 11 || v === 12) return 'IN'
  if (v === 2 || v === 3 || v === 13 || v === 14 || v === 21) return 'OUT'
  return 'AUTO'
}

/** Pull + import logs from one NP device. */
export async function importNpDeviceLogs(deviceId: string, opts: NpSyncOptions = {}): Promise<NpSyncResult> {
  const device = await db.device.findUnique({ where: { id: deviceId } })
  if (!device) throw new Error('دستگاه یافت نشد')
  if (!device.ipAddress?.trim()) {
    throw new Error(
      'آدرس IP دستگاه تنظیم نشده است — برای دریافت ترددها، IP دستگاه (مثل 192.168.1.201) را در پرونده دستگاه وارد کنید. ' +
      'دستگاه‌های NP هم دریافت لحظه‌ای (پوش) دارند و هم دریافت مستقیم (دانلود)، هر دو با پورت 5005.',
    )
  }

  try {
    return await importNpInner(device as DeviceRow, opts)
  } catch (err) {
    await db.device.update({ where: { id: deviceId }, data: { status: 'OFFLINE' } }).catch(() => {})
    await db.deviceLog.create({
      data: { deviceId, level: 'ERROR', message: `خطای دریافت از دستگاه: ${errorMessage(err)}` },
    }).catch(() => {})
    throw err
  }
}

async function importNpInner(device: DeviceRow, opts: NpSyncOptions): Promise<NpSyncResult> {
  const client = new SbxpcClient(device.ipAddress!.trim(), device.port || 5005)
  let deviceSerial: string | undefined
  let deviceTime: string | null | undefined
  let serialMismatch = false

  try {
    await client.connect()
    deviceSerial = await client.getSerialNumber().catch(() => undefined)
    deviceTime = (await client.getDeviceTime())?.toISOString() ?? null

    if (deviceSerial && device.serialNumber && deviceSerial.trim().toLowerCase() !== device.serialNumber.trim().toLowerCase()) {
      serialMismatch = true
    }

    // vendor locks the keypad while reading (EnableDevice -1)
    await client.enableDevice(false).catch(() => {})
    const logs = await client.readAllLogs()
    await client.enableDevice(true).catch(() => {})

    const now = Date.now()
    let cutoffMs: number
    if (typeof opts.daysBack === 'number' && opts.daysBack > 0) {
      cutoffMs = now - opts.daysBack * 24 * 3600 * 1000
    } else if (device.lastLogAt) {
      // incremental: continue right after the newest log we already have
      cutoffMs = device.lastLogAt.getTime() - 60 * 1000 // 1-min overlap guard
    } else {
      cutoffMs = now - DEFAULT_FIRST_SYNC_DAYS * 24 * 3600 * 1000
    }

    const fresh = logs.filter((l) => Number.isFinite(l.ts) && l.ts >= cutoffMs && l.ts <= now + 24 * 3600 * 1000)

    // map device user id → personnel (deviceUserId or numeric personnelCode)
    const persons = await db.personnel.findMany({ select: { id: true, deviceUserId: true, personnelCode: true } })
    const byUid = new Map<number, string>()
    for (const p of persons) {
      if (p.deviceUserId != null) byUid.set(p.deviceUserId, p.id)
      const code = Number(p.personnelCode)
      if (!Number.isNaN(code) && !byUid.has(code)) byUid.set(code, p.id)
    }

    let created = 0
    let skipped = 0
    let unmatched = 0
    const touched = new Map<string, Set<string>>() // personId → local days needing recalc
    let maxTs: number | null = null

    for (const log of fresh) {
      if (maxTs === null || log.ts > maxTs) maxTs = log.ts
      const personId = byUid.get(log.userId)
      if (!personId) {
        unmatched++
        continue
      }
      const ts = new Date(log.ts)
      try {
        await db.punchRecord.create({
          data: {
            personId,
            timestamp: ts,
            direction: directionFromVerifyMode(log.verifyMode),
            source: 'DEVICE',
            deviceId: device.id,
            deviceUserId: log.userId,
            rawPayload: JSON.stringify({ proto: 'sbxpc', verifyMode: log.verifyMode, raw: log.raw }),
          },
        })
        created++
        const dayKey = new Date(ts.getTime() - 3.5 * 3600 * 1000).toISOString().slice(0, 10)
        if (!touched.has(dayKey)) touched.set(dayKey, new Set())
        touched.get(dayKey)!.add(personId)
      } catch {
        skipped++ // duplicate (same person+time+source) — already recorded
      }
    }

    // recalc attendance days that received new punches
    for (const [, personIds] of touched) {
      for (const personId of personIds) {
        await recalcDayForPerson(personId, new Date(maxTs ?? now)).catch(() => {})
      }
    }

    const bookmarkDate = maxTs !== null ? new Date(Math.max(maxTs, device.lastLogAt?.getTime() ?? 0)) : device.lastLogAt
    await db.device.update({
      where: { id: device.id },
      data: {
        status: 'ONLINE',
        lastSyncAt: new Date(),
        lastSyncCount: created,
        ...(bookmarkDate ? { lastLogAt: bookmarkDate } : {}),
        ...(deviceSerial && !device.serialNumber ? { serialNumber: deviceSerial } : {}),
      },
    })

    const summary: NpSyncResult = {
      created, skipped, unmatched,
      total: logs.length,
      imported: fresh.length,
      cutoff: new Date(cutoffMs).toISOString(),
      newBookmark: bookmarkDate ? bookmarkDate.toISOString() : null,
      deviceSerial,
      deviceTime,
      serialMismatch,
    }

    await db.deviceLog.create({
      data: {
        deviceId: device.id,
        level: 'SUCCESS',
        message:
          `دریافت از دستگاه انجام شد: ${logs.length} تردد در حافظه دستگاه، ${fresh.length} در بازه انتخابی، ` +
          `${created} رکورد جدید، ${skipped} تکراری، ${unmatched} بدون تطبیق` +
          (deviceSerial ? ` — سریال دستگاه: ${deviceSerial}` : '') +
          (serialMismatch ? ' (⚠️ با سریال ثبت‌شده در پرونده دستگاه یکی نیست)' : ''),
      },
    }).catch(() => {})

    if (created > 0) {
      await emitRealtime('device-sync', { deviceId: device.id, created, skipped, unmatched }).catch(() => {})
    }
    return summary
  } finally {
    client.disconnect()
  }
}

/** Automatically sync every enabled device whose interval is due (NP pull + ZK pull). */
export async function autoSyncDueDevices(): Promise<{ synced: string[]; errors: Array<{ name: string; error: string }> }> {
  const devices = await db.device.findMany({ where: { enabled: true, syncIntervalMin: { gt: 0 } } })
  const synced: string[] = []
  const errors: Array<{ name: string; error: string }> = []
  for (const d of devices) {
    const due = !d.lastSyncAt || Date.now() - d.lastSyncAt.getTime() >= d.syncIntervalMin * 60 * 1000
    if (!due) continue
    try {
      if (d.type === 'NOVIN_NP') {
        if (!d.ipAddress) continue
        await importNpDeviceLogs(d.id, {}) // incremental — continues from lastLogAt
      } else {
        const { importDeviceLogs } = await import('@/lib/zk')
        await importDeviceLogs(d.id)
      }
      synced.push(d.name)
    } catch (err) {
      errors.push({ name: d.name, error: errorMessage(err) })
    }
  }
  return { synced, errors }
}

/** kept for type-compat with tests */
export type { NpLog }
