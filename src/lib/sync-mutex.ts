/**
 * Per-device sync mutex — guarantees that at most ONE sync operation runs per
 * device at any moment, no matter who started it: the manual "Sync Now" button,
 * the automatic scheduler, or the near real-time poller.
 *
 * v1.3.1 (Phase 1 review fix 7): the lock lives IN THE DATABASE (DeviceSyncLock
 * table), NOT in a process-local map. A process-local lock would only protect
 * one Node process and would falsely claim to protect the deployment — the
 * real deployment runs several processes against the same SQLite file
 * (server + scheduler + manual scripts), so mutual exclusion must be visible
 * to all of them.
 *
 * Semantics:
 *   * acquire — INSERT a lock row (deviceId = PK). If the row exists, the
 *     lease is checked: an EXPIRED lease is taken over atomically
 *     (conditional UPDATE — exactly one of N racing processes wins); a live
 *     lease fails fast with SyncBusyError.
 *   * lease   — every lock has an expiry (default 15 minutes, comfortably
 *     above withRetry + ingest transaction timeouts). A process that crashed
 *     mid-run can therefore never deadlock the device: once the lease
 *     expires, the next attempt takes the lock over (process recovery).
 *   * release — always in `finally`, and only by the owner (lockToken) —
 *     a released lock never outlives the operation that held it.
 *   * honesty — this lock serializes sync runs, it does NOT claim to make
 *     concurrent imports safe by itself: the database uniqueness constraint
 *     on RawPunch (fingerprint + dupIndex) remains the FINAL duplicate
 *     safeguard if two runs ever overlap anyway.
 */
import { randomUUID } from 'crypto'
import { Prisma } from '@prisma/client'
import { db } from '@/lib/db'

export class SyncBusyError extends Error {
  constructor(deviceName: string) {
    super(
      `همگام‌سازی دستگاه «${deviceName}» هنوز در حال اجراست — لطفاً چند لحظه بعد دوباره تلاش کنید ` +
      '(عملیات جدید نادیده گرفته شد تا با عملیات قبلی تداخل نکند)',
    )
    this.name = 'SyncBusyError'
  }
}

/** default lease: minutes a lock survives a crashed holder before takeover */
export const DEFAULT_SYNC_LOCK_LEASE_MS = 15 * 60_000

function isP2002(err: unknown): boolean {
  return err instanceof Prisma.PrismaClientKnownRequestError && err.code === 'P2002'
}

export interface DeviceSyncLockHandle {
  deviceId: string
  lockToken: string
}

/** Acquire the DB lock for a device (takeover allowed only after lease expiry).
 *  Throws SyncBusyError when another live sync holds the lock. */
export async function acquireDeviceSyncLock(
  deviceId: string,
  deviceName: string,
  leaseMs: number = DEFAULT_SYNC_LOCK_LEASE_MS,
): Promise<DeviceSyncLockHandle> {
  const lockToken = randomUUID()
  const expiresAt = new Date(Date.now() + leaseMs)
  try {
    await db.deviceSyncLock.create({ data: { deviceId, lockToken, expiresAt } })
    return { deviceId, lockToken }
  } catch (err) {
    if (!isP2002(err)) throw err
    // the row exists — someone may hold the lock. Sweep rows whose lease has
    // long expired, then try an ATOMIC conditional takeover: exactly one of
    // several racing processes gets count = 1.
    await db.deviceSyncLock.deleteMany({ where: { expiresAt: { lt: new Date(Date.now() - leaseMs) } } }).catch(() => {})
    const takeover = await db.deviceSyncLock.updateMany({
      where: { deviceId, expiresAt: { lt: new Date() } },
      data: { lockToken, expiresAt, acquiredAt: new Date() },
    })
    if (takeover.count === 0) throw new SyncBusyError(deviceName)
    return { deviceId, lockToken }
  }
}

/** Release a lock — only its owner may (lockToken match). Never throws. */
export async function releaseDeviceSyncLock(handle: DeviceSyncLockHandle): Promise<void> {
  await db.deviceSyncLock
    .deleteMany({ where: { deviceId: handle.deviceId, lockToken: handle.lockToken } })
    .catch(() => {})
}

/** true while a sync operation is running for this device (live lease only). */
export async function isDeviceSyncing(deviceId: string): Promise<boolean> {
  const row = await db.deviceSyncLock.findUnique({ where: { deviceId } })
  return !!row && row.expiresAt.getTime() > Date.now()
}

/**
 * Run `fn` as the ONLY sync operation for this device — across ALL processes.
 * Throws SyncBusyError immediately if another live sync is running. The lock
 * is released in `finally` after both success and failure; a crashed holder
 * is recovered by lease expiry, never manually.
 */
export async function withDeviceSyncLock<T>(
  deviceId: string,
  deviceName: string,
  fn: () => Promise<T>,
  opts: { leaseMs?: number } = {},
): Promise<T> {
  const handle = await acquireDeviceSyncLock(deviceId, deviceName, opts.leaseMs)
  try {
    return await fn()
  } finally {
    await releaseDeviceSyncLock(handle)
  }
}
