/**
 * Device secret hygiene — Phase 1 (Security).
 *
 * The device «رمز شبکه» (network password) is a secret at rest in Device.password.
 * It is used internally for the SBXPC/EXT_SECURE dialects, but it must NEVER be:
 *   * returned by any API response (GET /api/devices, /api/bootstrap, test results)
 *   * written into DeviceLog / DeviceSyncRun.error / notes / error texts
 *   * echoed into realtime payloads
 *
 * This module centralises the two guards every code path goes through:
 *   * sanitizeDeviceForApi() — strips the password from a device row and replaces it
 *     with a boolean hasPassword indicator (UI copy keeps working).
 *   * redactDeviceSecrets()  — last-resort scrubber for text that may contain a
 *     password VALUE (historical DeviceLog rows written by v1.2.x did include it).
 *     Applied on every write to DeviceLog and on every read of log rows via API.
 */

import { deviceHasPassword } from '@/lib/device-secret-store'

export type DeviceWithSecrets = {
  password?: number | string | null
  passwordEnc?: string | null
  bridgeToken?: string | null
} & Record<string, unknown>

/** legacy alias (Phase 1) */
export type DeviceWithPassword = DeviceWithSecrets

export type SafeDeviceRow = Omit<DeviceWithSecrets, 'password' | 'passwordEnc' | 'bridgeToken'> & {
  hasPassword: boolean
  hasProtectedPassword: boolean
  hasBridgeToken: boolean
}

/** Strip the device secrets (network password — plaintext or protected-at-rest —
 *  and the bridge token) from a row before it leaves the process boundary. */
export function sanitizeDeviceForApi(device: DeviceWithSecrets): SafeDeviceRow {
  const { password, passwordEnc, bridgeToken, ...rest } = device
  return {
    ...rest,
    hasPassword: deviceHasPassword({ password, passwordEnc }),
    hasProtectedPassword: passwordEnc != null && passwordEnc !== '',
    hasBridgeToken: bridgeToken != null && bridgeToken !== '',
  }
}

/** Strip the password from an arbitrary result object (npTest / bridge responses). */
export function sanitizeResultForApi<T extends Record<string, unknown>>(result: T): T & { passwordFound?: boolean } {
  const clone: Record<string, unknown> = { ...result }
  const hadPassword = clone.password != null
  delete clone.password
  return { ...(clone as T), passwordFound: hadPassword }
}

/**
 * Redact secret VALUES from free text (device log messages, error strings).
 * Historical v1.2.x rows contain e.g. " · رمز شبکه: 123456" — this replaces the
 * VALUE while keeping the label, so old rows become safe to display.
 */
const SECRET_PATTERNS: Array<{ re: RegExp; replacement: string }> = [
  { re: /(رمز\s*شبکه\s*[:：]?\s*)\d{3,}/g, replacement: '$1●●●●' },
  { re: /(رمز\s*(?:دستگاه|اتصال)?\s*[:：]?\s*)\d{3,}/g, replacement: '$1●●●●' },
  { re: /(pass(?:word)?\s*[:=]\s*)\d{3,}/gi, replacement: '$1***' },
]

export function redactDeviceSecrets(text: string): string {
  let out = text
  for (const { re, replacement } of SECRET_PATTERNS) out = out.replace(re, replacement)
  return out
}
