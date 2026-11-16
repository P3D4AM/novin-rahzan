/**
 * FKAttend device driver (v1.5.0) — talks to the real NP756+ through the
 * **supervised x86 FK623 bridge** (tools/fkattend-bridge/fkattend-bridge.ps1,
 * 32-bit PowerShell + FK623Attend.dll — the exact method that recorded the
 * successful port-4238 connection) and feeds the Phase 1 RawPunch pipeline
 * through the shared runRawSync orchestrator.
 *
 * Architecture (docs/BRIDGE-SUPERVISION.md — x86 DLLs NEVER load inside the
 * x64 Node/Next process; the bridge is a MANAGED HIDDEN CHILD of this server):
 *
 *   HozorPlus (x64 Node) ──localhost HTTP + auto-token──▶ x86 Bridge (child)
 *        └─ FK623Attend.dll (32-bit) ──TCP 4238──▶ NP756+
 *
 * v1.5.0: the user no longer configures ANYTHING about the bridge. The URL
 * and the token come from the bridge supervisor (ensureBridge) — started,
 * restarted and stopped by the app itself. There is no bridgeUrl/bridgeToken
 * on the Device row anymore, nothing to paste, nothing to keep running.
 *
 * Bridge contract: tools/fkattend-bridge/CONTRACT.md (v2)
 *   GET  /ping           → liveness (no token — readiness probe only)
 *   POST /health          → status (token)
 *   POST /device/test     → READ-ONLY connect probe (no keypad lock, no reads)
 *   POST /device/read-logs → full log pull (keypad locked, re-enabled in finally)
 *   POST /shutdown        → graceful stop (token)
 *
 * The bridge deliberately exposes NO destructive endpoint (no empty/delete/
 * set-time). Device settings (IP/port/timeout/password/license/machine) all
 * come from the Device row — nothing is hardcoded here. The bridge endpoint
 * is supervisor-internal (localhost by construction).
 */
import { db } from '@/lib/db'
import { runRawSync, RawPunchProvider, RawPunchRead } from '@/lib/raw-sync'
import type { RawPunchInput } from '@/lib/raw-import'
import { withDeviceSyncLock, SyncBusyError } from '@/lib/sync-mutex'
import { tehranWallClockToUTC } from '@/lib/sbxpc'
import { getDevicePassword } from '@/lib/device-secret-store'
import { ensureBridge, invalidateBridgeForRestart } from '@/lib/bridge-supervisor'

export interface FkBridgeConfig {
  bridgeUrl: string
  bridgeToken: string | null
  machineNumber: number
  ipAddress: string
  port: number
  connTimeoutMs: number
  password: number | null
  passwordEnc?: string | null
  sdkLicense: number
}

export interface FkAttendTestResult {
  ok: boolean
  dialect: 'FKATTEND'
  bridgeUrl: string
  /** total round-trip including bridge → device connect */
  latencyMs?: number
  serialNumber?: string | null
  deviceTime?: string | null
  serialMatch?: boolean | null
  error?: string
  /** diagnostics echoed by the bridge (never contains the password) */
  meta?: Record<string, unknown>
}

/** bridge JSON record (see CONTRACT.md v2 — workCode is OPTIONAL: the
 * confirmed FK623Attend.dll signature of FK_GetGeneralLogData does NOT return
 * one, so the bridge omits it entirely; a fake value is never fabricated) */
interface FkBridgeRecord {
  enrollNumber: number
  verifyMode: number
  inOutMode: number
  workCode?: number
  /** device wall clock "YYYY-MM-DD HH:MM:SS" (Tehran local) */
  timestamp: string
}

interface FkBridgeReadResponse {
  ok: boolean
  serialNumber?: string | null
  deviceTime?: string | null
  latencyMs?: number
  endCode?: number
  records?: FkBridgeRecord[]
  error?: string
  code?: string
  /** v2: true when the read ended with an UNEXPECTED code (never silently
   * treated as a complete read downstream) */
  endCodeAbnormal?: boolean
  /** echo of the request parameters (for config-propagation tests; NO password) */
  meta?: { machine?: number; ip?: string; port?: number; timeoutMs?: number; license?: number }
}

const LOCALHOST_HOSTS = new Set(['localhost', '127.0.0.1', '::1'])

/** POST JSON to the bridge with token auth + timeout; maps network errors to Persian. */
async function bridgePost<T>(cfg: { bridgeUrl: string; bridgeToken: string | null }, path: string, body: Record<string, unknown>, timeoutMs: number): Promise<T> {
  const u = new URL(cfg.bridgeUrl)
  if (!LOCALHOST_HOSTS.has(u.hostname.toLowerCase())) {
    throw new Error('آدرس Bridge فقط روی همین سیستم (localhost) مجاز است — تنظیمات دستگاه را اصلاح کنید')
  }
  const headers: Record<string, string> = { 'Content-Type': 'application/json' }
  if (cfg.bridgeToken) headers['x-bridge-token'] = cfg.bridgeToken
  let res: Response
  try {
    res = await fetch(new URL(path, u.origin), {
      method: 'POST',
      headers,
      body: JSON.stringify(body),
      signal: AbortSignal.timeout(timeoutMs),
      cache: 'no-store',
    })
  } catch (err) {
    const msg = (err as Error)?.name === 'TimeoutError' || (err as Error)?.message?.includes('abort')
      ? 'مهلت پاسخ سرویس ارتباط دستگاه به پایان رسید — لطفاً دوباره تلاش کنید'
      : 'ارتباط با سرویس ارتباط دستگاه برقرار نشد — لطفاً دوباره تلاش کنید'
    throw new Error(msg)
  }
  if (res.status === 401) {
    // a stale/foreign bridge instance is squatting our port → drop the state
    // so the NEXT ensureBridge starts a fresh one elsewhere
    invalidateBridgeForRestart()
    throw new Error('خطای داخلی سرویس ارتباط دستگاه — یک بار دیگر تلاش کنید؛ در صورت تکرار، راه‌اندازی مجدد نرم‌افزار را بررسی کنید')
  }
  if (!res.ok && res.status !== 200) {
    throw new Error(`خطای سرویس ارتباط دستگاه (HTTP ${res.status})`)
  }
  return (await res.json()) as T
}

/** wall-clock string "YYYY-MM-DD HH:MM:SS" (Tehran) → UTC instant (DST-correct via tz db) */
export function fkWallClockToUTC(ts: string): Date | null {
  const m = /^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$/.exec(String(ts ?? '').trim())
  if (!m) return null
  const ms = Date.UTC(Number(m[1]), Number(m[2]) - 1, Number(m[3]), Number(m[4]), Number(m[5]), Number(m[6]))
  if (!Number.isFinite(ms)) return null
  return tehranWallClockToUTC(ms)
}

/** FKAttend bridge wrapped as a RawPunchProvider (raw values preserved — no guessing). */
class FkAttendBridgeProvider implements RawPunchProvider {
  readonly protocol = 'FKATTEND' as const
  constructor(private readonly cfg: FkBridgeConfig) {}

  async readAll(): Promise<RawPunchRead> {
    const res = await bridgePost<FkBridgeReadResponse>(
      { bridgeUrl: this.cfg.bridgeUrl, bridgeToken: this.cfg.bridgeToken },
      '/device/read-logs',
      {
        machine: this.cfg.machineNumber,
        ip: this.cfg.ipAddress,
        port: this.cfg.port,
        timeoutMs: this.cfg.connTimeoutMs,
        protocolType: 0,
        password: this.cfg.password ?? 0,
        license: this.cfg.sdkLicense,
      },
      // device read may legitimately take a while (full history) — generous headroom
      this.cfg.connTimeoutMs + 120_000,
    )
    if (!res.ok) {
      throw new Error(res.error || 'دریافت ترددها از دستگاه از طریق Bridge ناموفق بود')
    }
    const records = Array.isArray(res.records) ? res.records : []
    const out: RawPunchInput[] = []
    let invalid = 0
    for (const r of records) {
      const t = fkWallClockToUTC(r.timestamp)
      if (!t || !Number.isFinite(t.getTime())) {
        invalid++
        continue
      }
      const hasWorkCode = r.workCode !== undefined && r.workCode !== null
      out.push({
        enrollNumber: String(r.enrollNumber),
        punchTime: t,
        punchTimeRaw: r.timestamp, // device wall clock preserved verbatim
        inOutModeRaw: Number.isFinite(r.inOutMode) ? r.inOutMode : 0,
        verifyModeRaw: Number.isFinite(r.verifyMode) ? r.verifyMode : 0,
        // v1.5.0: the FK623 signature returns NO workCode → empty string, and
        // the raw payload simply omits the key (never a fabricated value)
        workCode: hasWorkCode ? String(r.workCode) : '',
        rawPayload: JSON.stringify({
          proto: 'fkattend',
          enrollNumber: r.enrollNumber,
          verifyMode: r.verifyMode,
          inOutMode: r.inOutMode,
          ...(hasWorkCode ? { workCode: r.workCode } : {}),
          timestamp: r.timestamp,
        }),
      })
    }
    return {
      records: out,
      deviceSerial: res.serialNumber ?? undefined,
      deviceTime: res.deviceTime ?? null,
      meta: { endCode: res.endCode, endCodeAbnormal: res.endCodeAbnormal ?? false, bridgeLatencyMs: res.latencyMs, invalidTimestamps: invalid, echoed: res.meta },
    }
  }
}

type DeviceLike = {
  id: string
  name: string
  serialNumber: string | null
  machineNumber: number
  ipAddress: string | null
  port: number
  connTimeoutMs: number
  password: number | null
  passwordEnc: string | null
  sdkLicense: number
}

async function deviceToBridgeConfig(device: DeviceLike): Promise<FkBridgeConfig> {
  if (!device.ipAddress?.trim()) {
    throw new Error('آدرس IP دستگاه تنظیم نشده است — IP دستگاه (مثل 192.168.1.75) را در پرونده دستگاه وارد کنید')
  }
  // v1.5.0: the bridge endpoint is INTERNAL — started/restarted/stopped by the
  // supervisor. No user-facing URL or token exists anymore.
  const ep = await ensureBridge()
  return {
    bridgeUrl: ep.baseUrl,
    bridgeToken: ep.token,
    machineNumber: device.machineNumber || 1,
    ipAddress: device.ipAddress.trim(),
    port: device.port || 4238,
    connTimeoutMs: device.connTimeoutMs || 5000,
    // v1.3.1 (fix 6): the working password may live in the protected column —
    // getDevicePassword decrypts inside the process when needed
    password: (await getDevicePassword(device)) ?? 0,
    sdkLicense: device.sdkLicense ?? 1261,
  }
}

/** READ-ONLY connection test through the bridge (no keypad lock, no data read). */
export async function fkTestConnection(device: DeviceLike): Promise<FkAttendTestResult> {
  const cfg = await deviceToBridgeConfig(device)
  const serialOnFile = device.serialNumber?.trim() ?? ''
  const t0 = Date.now()
  try {
    const res = await bridgePost<FkBridgeReadResponse>(
      { bridgeUrl: cfg.bridgeUrl, bridgeToken: cfg.bridgeToken },
      '/device/test',
      {
        machine: cfg.machineNumber,
        ip: cfg.ipAddress,
        port: cfg.port,
        timeoutMs: cfg.connTimeoutMs,
        protocolType: 0,
        password: device.password ?? 0,
        license: cfg.sdkLicense,
      },
      cfg.connTimeoutMs + 30_000,
    )
    const latencyMs = res.latencyMs ?? Date.now() - t0
    const serial = res.serialNumber ?? null
    const serialMatch = serial && serialOnFile
      ? serial.trim().toLowerCase() === serialOnFile.trim().toLowerCase()
      : null
    return {
      ok: Boolean(res.ok),
      dialect: 'FKATTEND',
      bridgeUrl: cfg.bridgeUrl,
      latencyMs,
      serialNumber: serial,
      deviceTime: res.deviceTime ?? null,
      serialMatch,
      error: res.ok ? undefined : res.error,
      meta: res.meta as Record<string, unknown> | undefined,
    }
  } catch (err) {
    return {
      ok: false,
      dialect: 'FKATTEND',
      bridgeUrl: cfg.bridgeUrl,
      error: (err as Error)?.message ?? String(err),
    }
  }
}

export interface FkSyncOptions {
  daysBack?: number | null
  mode?: 'HARDWARE' | 'MOCK' | 'DRY_RUN'
  /** read the FULL history the device reports (fingerprint dedupe makes it safe) */
  full?: boolean
  /** quiet history (no-op runs write nothing) — rt-poll */
  quiet?: boolean
  /** provenance note for the DeviceSyncRun row */
  note?: string
}

/** UI-compatible sync result (same shape as the SBXPC NpSyncResult) */
export interface FkSyncResult {
  created: number
  skipped: number
  unmatched: number
  total: number
  imported: number
  /** v1.3.1 (fix 4): malformed records quarantined (new) and window-excluded records */
  rejected?: number
  outOfWindow?: number
  /** v1.3.1 (fix 2): distinct (personId, day) pairs whose recalculation failed */
  recalcFailed?: number
  cutoff: string
  newBookmark: string | null
  deviceSerial?: string
  deviceTime?: string | null
  serialMismatch?: boolean
  runId?: string
  mode?: string
}

/** Pull + import logs from one FKATTEND-family device (through the bridge). */
export async function importFkAttendLogs(deviceId: string, opts: FkSyncOptions = {}): Promise<FkSyncResult> {
  const device = await db.device.findUnique({ where: { id: deviceId } })
  if (!device) throw new Error('دستگاه یافت نشد')
  const cfg = await deviceToBridgeConfig(device)
  const provider = new FkAttendBridgeProvider(cfg)
  const summary = await withDeviceSyncLock(device.id, device.name, () =>
    runRawSync({
      deviceId: device.id,
      provider,
      mode: opts.mode ?? 'HARDWARE',
      daysBack: opts.daysBack ?? null,
      full: opts.full ?? false,
      quiet: opts.quiet ?? false,
      note: opts.note,
      retries: 2,
    }),
  )
  return {
    created: summary.newCount,
    skipped: summary.duplicate,
    unmatched: summary.unmatched,
    total: summary.seen,
    imported: summary.inWindow,
    rejected: summary.rejected,
    outOfWindow: summary.outOfWindow,
    recalcFailed: summary.recalcFailed,
    cutoff: summary.cutoff,
    newBookmark: summary.newBookmark,
    deviceSerial: summary.deviceSerial,
    deviceTime: summary.deviceTime,
    runId: summary.runId,
    mode: summary.mode,
  }
}

/** keep the busy-error importable for API layers */
export { SyncBusyError }
