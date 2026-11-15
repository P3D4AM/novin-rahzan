/**
 * Per-device sync mutex (addendum P2) — guarantees that at most ONE sync
 * operation runs per device at any moment, no matter who started it:
 * the manual "Sync Now" button, the automatic scheduler, or the near
 * real-time poller. A second attempt while one is running fails fast
 * with SyncBusyError instead of interleaving device reads.
 *
 * The map lives on globalThis so Next.js dev hot-reload cannot spawn
 * a second, independent lock table. All entries are always released
 * (finally) — a crashed run cannot deadlock the device.
 */
export class SyncBusyError extends Error {
  constructor(deviceName: string) {
    super(
      `همگام‌سازی دستگاه «${deviceName}» هنوز در حال اجراست — لطفاً چند لحظه بعد دوباره تلاش کنید ` +
      '(عملیات جدید نادیده گرفته شد تا با عملیات قبلی تداخل نکند)',
    )
    this.name = 'SyncBusyError'
  }
}

type GlobalWithLocks = typeof globalThis & { __hozorSyncLocks?: Map<string, Promise<unknown>> }
const g = globalThis as GlobalWithLocks
const locks: Map<string, Promise<unknown>> = (g.__hozorSyncLocks ??= new Map())

/** true while a sync operation is running for this device (scheduler/poller skip) */
export function isDeviceSyncing(deviceId: string): boolean {
  return locks.has(deviceId)
}

/**
 * Run `fn` as the ONLY sync operation for this device.
 * Throws SyncBusyError immediately if another sync is already running.
 */
export async function withDeviceSyncLock<T>(deviceId: string, deviceName: string, fn: () => Promise<T>): Promise<T> {
  if (locks.has(deviceId)) throw new SyncBusyError(deviceName)
  const p = (async () => {
    try {
      return await fn()
    } finally {
      locks.delete(deviceId)
    }
  })()
  locks.set(deviceId, p)
  return p
}
