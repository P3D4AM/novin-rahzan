/**
 * Device settings validation & normalization (addendum P1) — the single
 * place where per-device connection settings are checked BEFORE they reach
 * the database. Pure & synchronous → unit-testable without a server.
 *
 * Rules (all enforced in the BACKEND, never only in the browser):
 *   * protocolFamily (SBXPC | FKATTEND) is an EXPLICIT choice — it is never
 *     inferred from the port number. A device on port 4238 with family=SBXPC
 *     stays SBXPC; a device on 5005 with family=FKATTEND stays FKATTEND.
 *   * default port per family: FKATTEND → 4238 (NP756+), SBXPC → 5005,
 *     ZKTECO → 4370. The port stays freely editable afterwards.
 *   * syncIntervalMin: 0 (= manual only) or 1..1440 minutes (unrealistic /
 *     device-stressing values are rejected).
 *   * rtPollSec: 15..600 seconds (near real-time polling floor protects the
 *     device from a read loop that would keep its keypad locked).
 *   * network password semantics (Phase 1): undefined = keep the stored
 *     secret on edit; ''/null = clear; number = set (32-bit range).
 *   * v1.5.0: bridgeUrl/bridgeToken are GONE — the FKAttend bridge is fully
 *     internal (supervisor-managed). Any such field in a legacy payload is
 *     silently ignored (never written, never validated).
 */
export type ProtocolFamily = 'SBXPC' | 'FKATTEND'

export const PROTOCOL_FAMILIES: ProtocolFamily[] = ['SBXPC', 'FKATTEND']

/** default TCP port per family — NP756+ speaks FKAttend on 4238 */
export const DEFAULT_PORT_BY_FAMILY: Record<string, number> = {
  FKATTEND: 4238,
  SBXPC: 5005,
  ZKTECO: 4370,
}

const LOCALHOST_HOSTS = new Set(['localhost', '127.0.0.1', '::1', '[::1]'])
// kept for FKBRIDGE_URL validation in bridge-supervisor.ts (v1.5.0: the
// DEVICE no longer carries a bridge URL — it is internal)
export { LOCALHOST_HOSTS }

export interface DeviceSettingsInput {
  name?: unknown
  type?: unknown
  protocolFamily?: unknown
  ipAddress?: unknown
  port?: unknown
  protocol?: unknown
  location?: unknown
  enabled?: unknown
  syncIntervalMin?: unknown
  serialNumber?: unknown
  password?: unknown // number | string | null | undefined (undefined = keep)
  connTimeoutMs?: unknown
  machineNumber?: unknown
  sdkLicense?: unknown
  /** v1.5.0: accepted-but-IGNORED legacy fields (bridge is internal now) */
  bridgeUrl?: unknown
  bridgeToken?: unknown
  rtEnabled?: unknown
  rtPollSec?: unknown
}

export interface DeviceSettingsResult {
  name: string
  type: string
  protocolFamily: string
  ipAddress: string | null
  port: number
  protocol: string
  location: string | null
  enabled: boolean
  syncIntervalMin: number
  serialNumber: string | null
  connTimeoutMs: number
  machineNumber: number
  sdkLicense: number
  rtEnabled: boolean
  rtPollSec: number
  /** password data ONLY when it must be written; `undefined` = keep stored */
  password: number | null | undefined
}

export interface ExistingDeviceSettings {
  protocolFamily?: string | null
  port?: number | null
  password?: number | null
  type?: string | null
}

function intOrNull(v: unknown): number | null {
  if (v === null || v === undefined || v === '') return null
  const n = Number(v)
  return Number.isInteger(n) ? n : Number.isFinite(n) ? Math.round(n) : NaN
}

/** Validate + normalize device settings. Returns {ok:false, error} on any rule violation. */
export function validateDeviceSettings(
  raw: DeviceSettingsInput,
  opts: { isCreate: boolean; existing?: ExistingDeviceSettings },
): { ok: true; data: DeviceSettingsResult } | { ok: false; error: string } {
  const existing = opts.existing ?? {}

  const name = String(raw.name ?? '').trim()
  if (!name) return { ok: false, error: 'نام دستگاه الزامی است' }

  const type = raw.type === 'NOVIN_NP' ? 'NOVIN_NP' : raw.type === 'IMPORT' ? 'IMPORT' : 'ZKTECO'

  // ---- protocol family: EXPLICIT only, never derived from the port -------
  let familyRaw = String(raw.protocolFamily ?? '').trim()
  if (!familyRaw && !opts.isCreate) familyRaw = String(existing.protocolFamily ?? '')
  if (!familyRaw) familyRaw = 'SBXPC' // safe default on create: legacy path keeps working
  if (!PROTOCOL_FAMILIES.includes(familyRaw as ProtocolFamily)) {
    return { ok: false, error: 'پروتکل دستگاه نامعتبر است — فقط SBXPC یا FKATTEND' }
  }
  const family = familyRaw as ProtocolFamily

  const serialNumber = String(raw.serialNumber ?? '').trim() || null
  if (type === 'NOVIN_NP' && !serialNumber) {
    return { ok: false, error: 'برای دستگاه‌های نوین پرداز (NP) شماره سریال الزامی است' }
  }

  // ---- port: explicit value, or family default (never the reverse) -------
  const port = intOrNull(raw.port)
  if (raw.port !== undefined && raw.port !== null && raw.port !== '' && (port === null || Number.isNaN(port))) {
    return { ok: false, error: 'پورت باید عدد باشد' }
  }
  let portFinal: number
  if (port !== null && !Number.isNaN(port)) {
    if (port < 1 || port > 65535) return { ok: false, error: 'پورت باید بین ۱ تا ۶۵۵۳۵ باشد' }
    portFinal = port
  } else if (!opts.isCreate && existing.port != null) {
    portFinal = existing.port
  } else {
    portFinal = DEFAULT_PORT_BY_FAMILY[type === 'NOVIN_NP' ? family : 'ZKTECO'] ?? 4370
  }

  const ipAddress = String(raw.ipAddress ?? '').trim() || null
  const protocol = raw.protocol === 'UDP' ? 'UDP' : 'TCP'
  const location = String(raw.location ?? '').trim() || null
  const enabled = raw.enabled === undefined ? true : Boolean(raw.enabled)

  // ---- automatic pull interval (backend range check) ---------------------
  const syncInterval = raw.syncIntervalMin === undefined || raw.syncIntervalMin === null || raw.syncIntervalMin === ''
    ? 0 // absent = manual only (checkbox off)
    : intOrNull(raw.syncIntervalMin)
  if (syncInterval === null || Number.isNaN(syncInterval)) {
    return { ok: false, error: 'فاصلهٔ دریافت خودکار باید عدد صحیح (دقیقه) باشد' }
  }
  if (syncInterval !== 0 && (syncInterval < 1 || syncInterval > 1440)) {
    return { ok: false, error: 'فاصلهٔ دریافت خودکار باید بین ۱ تا ۱۴۴۰ دقیقه باشد (۰ = فقط دستی)' }
  }

  // ---- FKAttend SDK parameters ------------------------------------------
  const connTimeoutMs = raw.connTimeoutMs === undefined || raw.connTimeoutMs === null || raw.connTimeoutMs === ''
    ? 5000
    : intOrNull(raw.connTimeoutMs)
  if (connTimeoutMs === null || Number.isNaN(connTimeoutMs) || connTimeoutMs < 1000 || connTimeoutMs > 60000) {
    return { ok: false, error: 'مهلت اتصال باید بین ۱۰۰۰ تا ۶۰۰۰۰ میلی‌ثانیه باشد' }
  }
  const machineNumber = raw.machineNumber === undefined || raw.machineNumber === null || raw.machineNumber === ''
    ? 1
    : intOrNull(raw.machineNumber)
  if (machineNumber === null || Number.isNaN(machineNumber) || machineNumber < 1 || machineNumber > 255) {
    return { ok: false, error: 'شمارهٔ دستگاه (SDK) باید بین ۱ تا ۲۵۵ باشد' }
  }
  const sdkLicense = raw.sdkLicense === undefined || raw.sdkLicense === null || raw.sdkLicense === ''
    ? 1261
    : intOrNull(raw.sdkLicense)
  if (sdkLicense === null || Number.isNaN(sdkLicense) || sdkLicense < 0 || sdkLicense > 2147483647) {
    return { ok: false, error: 'مقدار License SDK نامعتبر است' }
  }

  // ---- v1.5.0: bridge endpoint settings REMOVED --------------------------
  // The FKAttend bridge is fully internal (bridge-supervisor). Legacy
  // bridgeUrl/bridgeToken values in old payloads are ignored — the settings
  // simply never reach the database.

  // ---- near real-time polling (honest label; validated like everything) --
  const rtEnabled = Boolean(raw.rtEnabled)
  const rtPollSec = raw.rtPollSec === undefined || raw.rtPollSec === null || raw.rtPollSec === ''
    ? 30
    : intOrNull(raw.rtPollSec)
  if (rtPollSec === null || Number.isNaN(rtPollSec) || rtPollSec < 15 || rtPollSec > 600) {
    return { ok: false, error: 'فاصلهٔ Polling شبه‌بی‌درنگ باید بین ۱۵ تا ۶۰۰ ثانیه باشد' }
  }

  // ---- network password (Phase 1 semantics preserved) --------------------
  let password: number | null | undefined
  if (raw.password === undefined) {
    password = opts.isCreate ? null : (existing.password ?? null) // keep stored
  } else if (raw.password === null || raw.password === '') {
    password = null // explicit clear
  } else {
    const n = Number(raw.password)
    if (!Number.isInteger(n) || n < 0 || n > 2147483647) {
      return { ok: false, error: 'رمز شبکه باید عدد صحیح بین ۰ تا ۲۱۴۷۴۸۳۶۴۷ باشد' }
    }
    password = n
  }

  return {
    ok: true,
    data: {
      name,
      type,
      protocolFamily: family,
      ipAddress,
      port: portFinal,
      protocol,
      location,
      enabled,
      syncIntervalMin: syncInterval,
      serialNumber,
      connTimeoutMs,
      machineNumber,
      sdkLicense,
      rtEnabled,
      rtPollSec,
      password,
    },
  }
}
