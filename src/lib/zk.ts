import ZKLib from 'node-zklib'
import { db } from '@/lib/db'

export interface ZkUser {
  uid: number
  userId?: string
  name?: string
  role?: number
  cardno?: number
  password?: string
}

export interface ZkLog {
  deviceUserId: number
  deviceUserIdStr?: string
  timestamp: Date
  state?: number
  type?: number
}

/** Hard timeout wrapper — prevents OS-level TCP hangs on unreachable devices */
function withTimeout<T>(promise: Promise<T>, ms: number, label: string): Promise<T> {
  return Promise.race([
    promise,
    new Promise<T>((_, reject) =>
      setTimeout(() => reject(new Error(`${label} — مهلت پاسخ دستگاه (${ms / 1000} ثانیه) به پایان رسید`)), ms)
    ),
  ])
}

/** Test connection and read basic info from a ZKTeco device */
export async function zkTest(ip: string, port: number, protocol: 'TCP' | 'UDP' = 'TCP') {
  const zk = new ZKLib(ip, port, 8000, 4000, protocol)
  try {
    await withTimeout(zk.createSocket(), 12000, 'اتصال به دستگاه')
    const info = await withTimeout(zk.getInfo(), 10000, 'دریافت اطلاعات')
    const time = await withTimeout(zk.getTime(), 10000, 'دریافت زمان')
    await zk.disconnect().catch(() => {})
    return {
      ok: true,
      info: {
        firmware: info.fwVersion ?? '',
        deviceName: (info as { deviceName?: string }).deviceName ?? '',
        serialNumber: info.serialNumber ?? '',
        userCounts: (info as { userCounts?: number }).userCounts ?? 0,
        logCounts: (info as { logCounts?: number }).logCounts ?? 0,
      },
      deviceTime: time,
    }
  } catch (err) {
    try { await zk.disconnect() } catch { /* noop */ }
    return { ok: false, error: err instanceof Error ? err.message : String(err) }
  }
}

/** Pull users from device */
export async function zkGetUsers(ip: string, port: number, protocol: 'TCP' | 'UDP' = 'TCP'): Promise<ZkUser[]> {
  const zk = new ZKLib(ip, port, 8000, 5000, protocol)
  await withTimeout(zk.createSocket(), 12000, 'اتصال به دستگاه')
  try {
    const users = await withTimeout(zk.getUsers(), 15000, 'دریافت کاربران')
    return (users?.data ?? []) as ZkUser[]
  } finally {
    try { await zk.disconnect() } catch { /* noop */ }
  }
}

/** Pull attendance logs from device */
export async function zkGetLogs(ip: string, port: number, protocol: 'TCP' | 'UDP' = 'TCP'): Promise<ZkLog[]> {
  const zk = new ZKLib(ip, port, 8000, 5000, protocol)
  await withTimeout(zk.createSocket(), 12000, 'اتصال به دستگاه')
  try {
    const logs = await withTimeout(zk.getAttendances(), 45000, 'دریافت ترددها')
    const list = (logs?.data ?? []) as Array<Record<string, unknown>>
    return list.map((l) => {
      const t = (l.recordTime ?? l.time ?? l.timestamp) as Date | string | undefined
      return {
        deviceUserId: Number(l.deviceUserId ?? l.uid ?? l.userSn ?? 0),
        timestamp: t ? new Date(t) : new Date(),
        state: Number(l.state ?? 0),
        type: Number(l.type ?? 0),
      }
    })
  } finally {
    try { await zk.disconnect() } catch { /* noop */ }
  }
}

/** Import device logs into PunchRecords. Returns counts. */
export async function importDeviceLogs(deviceId: string) {
  const device = await db.device.findUnique({ where: { id: deviceId } })
  if (!device) throw new Error('دستگاه یافت نشد')
  if (!device.ipAddress) throw new Error('آدرس IP دستگاه تنظیم نشده است')

  const logs = await zkGetLogs(device.ipAddress, device.port, device.protocol as 'TCP' | 'UDP')
  const persons = await db.personnel.findMany({ select: { id: true, deviceUserId: true, personnelCode: true } })

  // map device uid -> person (by deviceUserId or personnelCode)
  const byUid = new Map<number, string>()
  for (const p of persons) {
    if (p.deviceUserId != null) byUid.set(p.deviceUserId, p.id)
    const code = Number(p.personnelCode)
    if (!Number.isNaN(code)) byUid.set(code, p.id)
  }

  let created = 0
  let skipped = 0
  let unmatched = 0

  for (const log of logs) {
    const personId = byUid.get(log.deviceUserId)
    if (!personId) {
      unmatched++
      continue
    }
    const exists = await db.punchRecord.findFirst({
      where: { personId, timestamp: log.timestamp, source: 'DEVICE' },
      select: { id: true },
    })
    if (exists) {
      skipped++
      continue
    }
    const direction = log.state === 1 ? 'OUT' : log.state === 0 ? 'IN' : 'AUTO'
    await db.punchRecord.create({
      data: {
        personId,
        timestamp: log.timestamp,
        direction,
        source: 'DEVICE',
        deviceId,
        deviceUserId: log.deviceUserId,
        rawPayload: JSON.stringify(log),
      },
    })
    created++
  }

  await db.device.update({
    where: { id: deviceId },
    data: { lastSyncAt: new Date(), lastSyncCount: created, status: 'ONLINE' },
  })
  await db.deviceLog.create({
    data: {
      deviceId,
      level: 'SUCCESS',
      message: `همگام‌سازی انجام شد: ${created} رکورد جدید، ${skipped} تکراری، ${unmatched} بدون تطبیق`,
    },
  })

  return { created, skipped, unmatched, total: logs.length }
}
