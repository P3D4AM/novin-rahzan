/**
 * NP-series device pull-sync: download attendance logs straight from the
 * device over the SBXPC binary protocol (see lib/sbxpc.ts) and store them
 * through the RawPunch integrity pipeline (lib/raw-sync.ts + lib/raw-import.ts).
 *
 * v1.3.0 (Phase 1) changes — all six historical defects fixed:
 *   1. every record is stored in RawPunch (unmatched rows are KEPT, no longer
 *      dropped by a moving bookmark),
 *   2. dedupe is fingerprint-based (device+enroll+raw tuple+dupIndex) instead
 *      of relying on the legacy unique key,
 *   3. a serial mismatch STOPS the import before anything is stored,
 *   4. punch direction is never guessed — RawPunch.direction stays UNKNOWN and
 *      the legacy projection uses AUTO (undetermined),
 *   5. EnableDevice(true) (keypad re-enable) is guaranteed in `finally`,
 *   6. every run is recorded in DeviceSyncRun (sync history) with retry counts.
 */
import { SbxpcClient, NpLog } from '@/lib/sbxpc'
import { runRawSync, RawPunchProvider, RawPunchRead, RawSyncSummary } from '@/lib/raw-sync'
import { db } from '@/lib/db'

/** how far back a first-ever sync goes when nothing else is known (re-exported for compat) */
export { DEFAULT_FIRST_SYNC_DAYS } from '@/lib/raw-sync'

export interface NpSyncOptions {
  /** manual "download N days back" (null/undefined = use bookmark / default) */
  daysBack?: number | null
  /** what this call actually talks to — HARDWARE (default), MOCK (tests)… */
  mode?: 'HARDWARE' | 'MOCK' | 'DRY_RUN'
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
  password: number | null
  enabled: boolean
  lastLogAt: Date | null
}

/** SbxpcClient wrapped as a RawPunchProvider (records kept raw — no guessing). */
class SbxpcProvider implements RawPunchProvider {
  readonly protocol = 'SBXPC' as const
  private client: SbxpcClient | null = null

  constructor(private readonly device: { ipAddress: string; port: number; password: number | null }) {}

  async readAll(): Promise<RawPunchRead> {
    const c = this.client ?? new SbxpcClient(this.device.ipAddress, this.device.port || 5005, 1, this.device.password ?? null)
    this.client = c
    await c.connect()
    try {
      const serial = await c.getSerialNumber().catch(() => undefined)
      const time = await c.getDeviceTime().catch(() => null)
      // vendor locks the keypad while reading (EnableDevice -1) — re-enable is
      // guaranteed in `finally` even if the read throws (v1.3.0 fix #5)
      await c.enableDevice(false).catch(() => {})
      const logs = await c.readAllLogs()
      return {
        records: logs.map(toRawInput),
        deviceSerial: serial,
        deviceTime: time ? time.toISOString() : null,
        meta: { proto: 'sbxpc', logCount: logs.length },
      }
    } finally {
      await c.enableDevice(true).catch(() => {})
    }
  }

  disconnect() {
    try {
      this.client?.disconnect()
    } catch {
      /* ignore */
    }
  }
}

/** NpLog → RawPunchInput (raw values preserved; punchTimeRaw unknown for this dialect) */
function toRawInput(l: NpLog) {
  return {
    enrollNumber: String(l.userId),
    punchTime: new Date(l.ts),
    inOutModeRaw: 0, // the classic SBXPC log frame carries no separate in/out byte
    verifyModeRaw: l.verifyMode,
    rawPayload: JSON.stringify({ proto: 'sbxpc', verifyMode: l.verifyMode, raw: l.raw }),
  }
}

/** Pull + import logs from one NP device (SBXPC dialect, port 5005). */
export async function importNpDeviceLogs(deviceId: string, opts: NpSyncOptions = {}): Promise<NpSyncResult> {
  const device = await db.device.findUnique({ where: { id: deviceId } })
  if (!device) throw new Error('دستگاه یافت نشد')
  if (!device.ipAddress?.trim()) {
    throw new Error(
      'آدرس IP دستگاه تنظیم نشده است — برای دریافت ترددها، IP دستگاه (مثل 192.168.1.201) را در پرونده دستگاه وارد کنید. ' +
      'دستگاه‌های NP هم دریافت لحظه‌ای (پوش) دارند و هم دریافت مستقیم (دانلود)، هر دو با پورت 5005.',
    )
  }

  const provider = new SbxpcProvider({
    ipAddress: device.ipAddress.trim(),
    port: device.port || 5005,
    password: device.password ?? null,
  })
  try {
    const summary: RawSyncSummary = await runRawSync({
      deviceId: device.id,
      provider,
      mode: opts.mode ?? 'HARDWARE',
      daysBack: opts.daysBack ?? null,
      retries: 2,
    })
    return {
      created: summary.newCount,
      skipped: summary.duplicate,
      unmatched: summary.unmatched,
      total: summary.seen,
      imported: summary.inWindow,
      cutoff: summary.cutoff,
      newBookmark: summary.newBookmark,
      deviceSerial: summary.deviceSerial,
      deviceTime: summary.deviceTime,
      serialMismatch: false,
    }
  } finally {
    provider.disconnect()
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
        await importNpDeviceLogs(d.id, {}) // incremental — continues from rawCursorAt
      } else {
        const { importDeviceLogs } = await import('@/lib/zk')
        await importDeviceLogs(d.id)
      }
      synced.push(d.name)
    } catch (err) {
      errors.push({ name: d.name, error: (err as Error)?.message ?? String(err) })
    }
  }
  return { synced, errors }
}

/** kept for type-compat with tests */
export type { NpLog }
