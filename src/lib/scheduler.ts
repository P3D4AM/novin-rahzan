/**
 * Device sync scheduler (addendum P2) — runs INSIDE the Next.js server
 * process, started once from src/instrumentation.ts at boot.
 *
 * Design rules (all of them are acceptance items):
 *   * SERVER-SIDE    — closing the browser never stops it; a page refresh
 *                      cannot create a second loop (singleton on globalThis,
 *                      also survives Next dev hot-reload).
 *   * DB-DRIVEN      — every tick re-reads device settings from the database:
 *                      interval changes apply live WITHOUT a server restart,
 *                      and after a restart the schedule resumes as saved
 *                      (nothing lives in memory but transient timers).
 *   * ONE PER DEVICE — every run goes through the per-device sync mutex
 *                      (src/lib/sync-mutex.ts) shared with manual "Sync Now"
 *                      and the near-real-time poller; a busy device is
 *                      skipped, never queued/interleaved.
 *   * NO TICK STORM  — an in-memory hold-off backs off failing devices
 *                      (default 120 s) so a dead device is not hammered on
 *                      every tick; the hold-off clears on the first success.
 *   * HISTORY        — every run already lands in DeviceSyncRun (Phase 1);
 *                      Device.lastSyncError keeps the sanitized last error
 *                      for the UI.
 *
 * NEAR REAL-TIME POLLING (addendum P3 — see docs/FKATTEND-CAPABILITIES.md):
 * the FKAttend SDK is PULL-ONLY (Category C) and the device's RTLOG push is
 * Category B (unproven on this device), so per addendum §3.3 each device can
 * enable an independent, honestly-labelled Polling loop:
 *   * separate interval (Device.rtPollSec, default 30s, validated 15..600)
 *   * FULL read + fingerprint dedupe — a poll can never miss a clock-skewed
 *     punch and can never duplicate an existing one
 *   * shares the per-device mutex with the scheduler + manual sync (a poll
 *     while a sync runs is skipped, never interleaved)
 *   * exponential backoff on consecutive failures (×2 up to ×10, reset on
 *     success), tracked in memory only
 *   * quiet mode: a no-op poll (0 new records, no error) writes NO DeviceLog
 *     row and NO DeviceSyncRun row — polls appear in the history exactly when
 *     they did something (new records / retries / failure), so history stays
 *     meaningful without a 30-second row flood.
 *
 * The tick is idempotent and single-flight: overlapping ticks are ignored.
 */
import { db } from '@/lib/db'
import { SyncBusyError } from '@/lib/sync-mutex'

export interface SchedulerDeviceRow {
  id: string
  name: string
  type: string
  protocolFamily: string | null
  enabled: boolean
  ipAddress: string | null
  syncIntervalMin: number
  lastSyncAt: Date | null
  /** near real-time polling (addendum P3) */
  rtEnabled?: boolean | null
  rtPollSec?: number | null
  rtLastPollAt?: Date | null
}

export interface DuePlan {
  autoSync: SchedulerDeviceRow[]
  rtPoll: SchedulerDeviceRow[]
  skipped: Array<{ id: string; reason: 'not-due' | 'manual-only' | 'disabled' | 'no-ip' }>
}

/** Pure due-computation — exported for tests. 0 interval = manual only. */
export function computeDue(devices: SchedulerDeviceRow[], now = Date.now()): DuePlan {
  const autoSync: SchedulerDeviceRow[] = []
  const rtPoll: SchedulerDeviceRow[] = []
  const skipped: DuePlan['skipped'] = []
  for (const d of devices) {
    if (!d.enabled) { skipped.push({ id: d.id, reason: 'disabled' }); continue }
    if (d.type === 'NOVIN_NP' && !d.ipAddress?.trim()) { skipped.push({ id: d.id, reason: 'no-ip' }); continue }
    if (!d.syncIntervalMin || d.syncIntervalMin <= 0) {
      skipped.push({ id: d.id, reason: 'manual-only' }) // auto-sync off (rt polling may still run)
    } else {
      const lastMs = d.lastSyncAt?.getTime() ?? null
      if (lastMs === null || now - lastMs >= d.syncIntervalMin * 60_000) autoSync.push(d)
      else skipped.push({ id: d.id, reason: 'not-due' })
    }
    // near real-time poll due? (independent of the auto-sync interval)
    if (d.rtEnabled) {
      const lastPoll = d.rtLastPollAt?.getTime() ?? null
      const intervalMs = Math.max(15, d.rtPollSec ?? 30) * 1000
      if (lastPoll === null || now - lastPoll >= intervalMs) rtPoll.push(d)
    }
  }
  return { autoSync, rtPoll, skipped }
}

/** dispatch by the EXPLICIT protocol family (P1 rule) */
async function runOneAutoSync(d: SchedulerDeviceRow): Promise<void> {
  if (d.type === 'NOVIN_NP' && d.protocolFamily === 'FKATTEND') {
    const { importFkAttendLogs } = await import('@/lib/fkattend')
    await importFkAttendLogs(d.id, {})
  } else if (d.type === 'NOVIN_NP') {
    const { importNpDeviceLogs } = await import('@/lib/np-sync')
    await importNpDeviceLogs(d.id, {})
  } else {
    const { importDeviceLogs } = await import('@/lib/zk')
    await importDeviceLogs(d.id)
  }
}

/**
 * One near-real-time poll: FULL read + fingerprint dedupe (addendum §3.3 —
 * "only new records, safely"; the full read also survives device clock skew),
 * quiet history (a no-op poll records nothing) and the shared per-device
 * mutex. `quiet`/`note` are honoured by runRawSync.
 */
async function runOneRtPoll(d: SchedulerDeviceRow): Promise<void> {
  const opts = { full: true, quiet: true, note: 'rt-poll' } as const
  if (d.type === 'NOVIN_NP' && d.protocolFamily === 'FKATTEND') {
    const { importFkAttendLogs } = await import('@/lib/fkattend')
    await importFkAttendLogs(d.id, opts)
  } else if (d.type === 'NOVIN_NP') {
    const { importNpDeviceLogs } = await import('@/lib/np-sync')
    await importNpDeviceLogs(d.id, opts)
  } else {
    const { importDeviceLogs } = await import('@/lib/zk')
    await importDeviceLogs(d.id)
  }
}

export interface SchedulerHandle {
  stop(): void
  /** run one tick immediately (tests + first-boot promptness) */
  tickNow(): Promise<{ ran: string[]; polled: string[]; busy: number; held: number; failed: number; pollsFailed: number }>
}

export interface SchedulerOptions {
  /** tick period — default 30s (settings changes apply within one tick) */
  tickMs?: number
  /** initial delay before the first tick — default 5s (server settles) */
  firstTickDelayMs?: number
  /** failure hold-off — default 120s */
  holdoffMs?: number
}

type GlobalWithScheduler = typeof globalThis & {
  __hozorSchedulerHandle?: SchedulerHandle
  __hozorSchedulerHoldoff?: Map<string, number> // deviceId → next allowed attempt (epoch ms)
  __hozorRtBackoff?: Map<string, { fails: number; nextAt: number }> // rt-poll backoff per device
}

const g = globalThis as GlobalWithScheduler

/** Start the scheduler singleton. Calling it twice returns the SAME handle. */
export function startDeviceScheduler(opts: SchedulerOptions = {}): SchedulerHandle {
  const existing = g.__hozorSchedulerHandle
  if (existing) return existing

  const tickMs = Math.max(1_000, opts.tickMs ?? 30_000)
  const firstTickDelayMs = Math.max(0, opts.firstTickDelayMs ?? 5_000)
  const holdoffMs = Math.max(0, opts.holdoffMs ?? 120_000)
  const holdoff = (g.__hozorSchedulerHoldoff ??= new Map<string, number>())
  const rtBackoff = (g.__hozorRtBackoff ??= new Map<string, { fails: number; nextAt: number }>())

  let timer: ReturnType<typeof setInterval> | null = null
  let ticking = false // single-flight: overlapping ticks are ignored

  async function tick(): Promise<{ ran: string[]; polled: string[]; busy: number; held: number; failed: number; pollsFailed: number }> {
    if (ticking) return { ran: [], polled: [], busy: 0, held: 0, failed: 0, pollsFailed: 0 }
    ticking = true
    const ran: string[] = []
    const polled: string[] = []
    let busy = 0
    let held = 0
    let failed = 0
    let pollsFailed = 0
    try {
      const devices = await db.device.findMany({ orderBy: { name: 'asc' } })
      const { autoSync, rtPoll } = computeDue(devices as SchedulerDeviceRow[])
      const now = Date.now()

      // ---- automatic pull (addendum P2) ----------------------------------
      for (const d of autoSync) {
        const nextAllowed = holdoff.get(d.id) ?? 0
        if (nextAllowed > now) {
          held++
          continue // recent failure — backed off (protects the device + log)
        }
        try {
          // NOTE: no outer lock here on purpose — the sync entry points
          // (importNpDeviceLogs / importFkAttendLogs) already acquire the
          // per-device mutex internally; a SyncBusyError from them means a
          // manual sync / poller is running → skip, next tick retries.
          await runOneAutoSync(d)
          ran.push(d.name)
          holdoff.delete(d.id) // healthy again
        } catch (err) {
          if (err instanceof SyncBusyError) {
            busy++ // manual sync / poller is running — fine, next tick retries
            continue
          }
          failed++
          holdoff.set(d.id, Date.now() + holdoffMs)
          console.error(`[scheduler] ${d.name}: sync failed — holding off ${Math.round(holdoffMs / 1000)}s (${(err as Error)?.message ?? String(err)})`)
        }
      }

      // ---- near real-time polling (addendum P3) -------------------------
      for (const d of rtPoll) {
        const intervalMs = Math.max(15, d.rtPollSec ?? 30) * 1000
        const back = rtBackoff.get(d.id)
        if (back && back.nextAt > Date.now()) continue // exponential backoff
        try {
          await runOneRtPoll(d)
          polled.push(d.name)
          rtBackoff.delete(d.id) // healthy → backoff resets
        } catch (err) {
          if (err instanceof SyncBusyError) continue // scheduler/manual sync owns the device right now
          pollsFailed++
          const fails = (rtBackoff.get(d.id)?.fails ?? 0) + 1
          const factor = Math.min(2 ** fails, 10) // ×2..×10
          rtBackoff.set(d.id, { fails, nextAt: Date.now() + intervalMs * factor })
          console.error(`[scheduler] ${d.name}: rt-poll failed ×${fails} — backoff ${Math.round(intervalMs * factor / 1000)}s (${(err as Error)?.message ?? String(err)})`)
        } finally {
          // the poll attempt happened (success OR failure) → advance the stamp
          await db.device.update({ where: { id: d.id }, data: { rtLastPollAt: new Date() } }).catch(() => {})
        }
      }
    } catch (err) {
      console.error('[scheduler] tick error:', (err as Error)?.message ?? String(err))
    } finally {
      ticking = false
    }
    return { ran, polled, busy, held, failed, pollsFailed }
  }

  const boot = () => {
    timer = setInterval(() => {
      void tick()
    }, tickMs)
    if (typeof timer.unref === 'function') timer.unref()
  }
  const delay = setTimeout(boot, firstTickDelayMs)
  if (typeof delay.unref === 'function') delay.unref()

  const handle: SchedulerHandle = {
    stop() {
      if (timer) clearInterval(timer)
      g.__hozorSchedulerHandle = undefined
    },
    tickNow: () => tick(),
  }
  g.__hozorSchedulerHandle = handle
  console.log(`[scheduler] device sync scheduler started (tick ${Math.round(tickMs / 1000)}s, holdoff ${Math.round(holdoffMs / 1000)}s)`)
  return handle
}

/** true while a scheduler singleton is alive (tests + instrumentation) */
export function isSchedulerRunning(): boolean {
  return g.__hozorSchedulerHandle != null
}
