/**
 * Device secret at-rest store — Phase 1 review fix 6 (v1.3.1).
 *
 * PROBLEM (accepted at v1.3.0, now made explicit): Device.password stores the
 * device «رمز شبکه» (network password) as a PLAINTEXT integer inside the
 * SQLite database. The API/log redaction (lib/device-secrets.ts) protects the
 * secret at the process boundary only — anyone who obtains the database FILE
 * reads the password. That is a documented, honestly-labelled gap — NOT
 * "complete credential protection".
 *
 * APPROACH (implemented here, OPT-IN — see docs/SECURITY-device-password-at-rest.md):
 *   * Windows DPAPI (CryptProtectData, LocalMachine scope) protects the value
 *     at rest. The encryption key never exists as a file beside the database
 *     and is never hardcoded in source: DPAPI derives it from the machine and
 *     (for LocalMachine scope) any account on that machine can be granted
 *     decryption. This is OS-provided crypto — no homemade scheme.
 *   * Storage format: Device.passwordEnc = "dpapi:machine:<base64 blob>",
 *     while Device.password is set to NULL. While passwordEnc is null, the
 *     plaintext column keeps working (backward compatibility).
 *   * Opt-in: the protection is applied per-write only when the environment
 *     variable HOZOR_DEVICE_SECRET_PROTECTION=1 AND the process runs on
 *     Windows. Otherwise everything behaves exactly as before (plaintext at
 *     rest, clearly documented gap). The one-time migration of existing
 *     credentials is a separate, explicit admin action
 *     (scripts/protect-device-secrets.ts) — never automatic, never silent.
 *
 * PROTECTION BOUNDARY (be honest about what this does and does not give):
 *   * Protects against: the database file being copied/stolen WITHOUT the
 *     machine (backup leak, file-share exposure, stolen db folder).
 *   * Does NOT protect against: code executing ON the machine as a local
 *     account (LocalMachine scope decrypts for any process on the box), a
 *     full machine compromise, or a malicious HozorPlus process itself.
 *
 * RECOVERY REQUIREMENTS (the operational trade-off an owner must accept
 * BEFORE opting in): a DPAPI LocalMachine blob can only be decrypted on the
 * SAME Windows machine it was protected on. A database restored to different
 * hardware has UNRECOVERABLE device passwords — they must be re-entered from
 * the device administration panel. If that is unacceptable, stay opted-out.
 */
import { spawn } from 'child_process'

/** marker prefix of the at-rest protected form */
export const PROTECTED_SECRET_PREFIX = 'dpapi:machine:'

/** env flag that turns protection on (per-write, Windows only) */
export const DEVICE_SECRET_PROTECTION_ENV = 'HOZOR_DEVICE_SECRET_PROTECTION'

export class SecretProtectionUnavailableError extends Error {
  constructor(reason: string) {
    super(`محافظت از رمز دستگاه در حالت فعلی ممکن نیست: ${reason}`)
    this.name = 'SecretProtectionUnavailableError'
  }
}

/** DPAPI exists on Windows only. */
export function deviceSecretProtectionPlatform(): boolean {
  return process.platform === 'win32'
}

/** true when the operator has opted in AND the platform can actually protect. */
export function isDeviceSecretProtectionEnabled(): boolean {
  return process.env[DEVICE_SECRET_PROTECTION_ENV] === '1' && deviceSecretProtectionPlatform()
}

/** does this value carry the protected at-rest format? */
export function isProtectedSecret(v: string | null | undefined): boolean {
  return typeof v === 'string' && v.startsWith(PROTECTED_SECRET_PREFIX)
}

/**
 * Run one PowerShell DPAPI operation. The secret bytes travel through the
 * environment (never a command-line argument — nothing secret ever lands in
 * a process list), and only the Base64 result is printed to stdout.
 */
async function runPowerShellDpapi(base64: string, mode: 'protect' | 'unprotect'): Promise<string> {
  if (!deviceSecretProtectionPlatform()) {
    throw new SecretProtectionUnavailableError(
      'DPAPI فقط روی ویندوز در دسترس است — روی این سیستم، رمز دستگاه به‌صورت متن ساده ذخیره می‌شود (محدودیت مستندشده)',
    )
  }
  const body =
    mode === 'protect'
      ? '$in = [Convert]::FromBase64String($env:HOZOR_DPAPI_IN);' +
        '$out = [System.Security.Cryptography.ProtectedData]::Protect($in, $null, [System.Security.Cryptography.DataProtectionScope]::LocalMachine);' +
        '[Convert]::ToBase64String($out)'
      : '$in = [Convert]::FromBase64String($env:HOZOR_DPAPI_IN);' +
        '$out = [System.Security.Cryptography.ProtectedData]::Unprotect($in, $null, [System.Security.Cryptography.DataProtectionScope]::LocalMachine);' +
        '[Convert]::ToBase64String($out)'
  const script = ['$ErrorActionPreference = "Stop"', 'Add-Type -AssemblyName System.Security', body].join('\n')
  return await new Promise<string>((resolve, reject) => {
    const child = spawn(
      'powershell.exe',
      ['-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-Command', script],
      {
        env: { ...process.env, HOZOR_DPAPI_IN: base64 },
        windowsHide: true,
      },
    )
    let out = ''
    let err = ''
    const timer = setTimeout(() => child.kill(), 20_000)
    child.stdout?.on('data', (d) => { out += String(d) })
    child.stderr?.on('data', (d) => { err += String(d) })
    child.on('error', (e) => {
      clearTimeout(timer)
      reject(new SecretProtectionUnavailableError(`اجرای PowerShell ممکن نشد (${e.message})`))
    })
    child.on('close', (code) => {
      clearTimeout(timer)
      if (code !== 0) reject(new Error(`DPAPI ${mode} failed (exit ${code}): ${err.slice(0, 300)}`))
      else resolve(out.trim())
    })
  })
}

/** Protect a plaintext secret into the at-rest format (Windows only). */
export async function protectDeviceSecret(plain: string): Promise<string> {
  if (!plain) throw new Error('مقدار خالی قابل محافظت نیست')
  const b64 = Buffer.from(plain, 'utf8').toString('base64')
  const enc = await runPowerShellDpapi(b64, 'protect')
  return PROTECTED_SECRET_PREFIX + enc
}

/** Unprotect an at-rest value back to plaintext (Windows only). */
export async function unprotectDeviceSecret(enc: string): Promise<string> {
  if (!isProtectedSecret(enc)) throw new Error('مقدار در فرمت محافظت‌شده نیست')
  const b64 = enc.slice(PROTECTED_SECRET_PREFIX.length)
  const out = await runPowerShellDpapi(b64, 'unprotect')
  return Buffer.from(out, 'base64').toString('utf8')
}

/* ============================================================
 * Read/write helpers used by the device code paths
 * ============================================================
 * These keep every caller working regardless of whether protection is
 * enabled: the working value is always a plaintext number in memory, and the
 * at-rest representation (column) is decided in exactly ONE place.
 */

export type DeviceSecretRow = {
  password?: number | string | null
  passwordEnc?: string | null
}

/** The working network password (decrypted on demand). Never logged, never returned by APIs. */
export async function getDevicePassword(device: DeviceSecretRow): Promise<number | null> {
  if (device.passwordEnc) {
    const plain = await unprotectDeviceSecret(device.passwordEnc)
    const n = Number(plain)
    return plain !== '' && Number.isFinite(n) ? n : null
  }
  const p = device.password
  if (p == null || p === '') return null
  const n = Number(p)
  return Number.isFinite(n) ? n : null
}

/** Does this device have ANY stored network password (either representation)? */
export function deviceHasPassword(device: DeviceSecretRow): boolean {
  return (
    (device.password != null && device.password !== '') ||
    (device.passwordEnc != null && device.passwordEnc !== '')
  )
}

export interface StoredDevicePassword {
  password: number | null
  passwordEnc: string | null
  /** how the value ended up at rest — surfaced so callers can warn honestly */
  stored: 'protected' | 'plaintext'
}

/**
 * Decide the at-rest representation for a network password value.
 *   * protection enabled (env flag + Windows)  → DPAPI blob, password = null
 *   * otherwise                                 → plaintext column as before
 * `null` input clears both columns.
 */
export async function storeDevicePassword(plain: number | null): Promise<StoredDevicePassword> {
  if (plain == null) return { password: null, passwordEnc: null, stored: 'plaintext' }
  if (isDeviceSecretProtectionEnabled()) {
    try {
      const enc = await protectDeviceSecret(String(plain))
      return { password: null, passwordEnc: enc, stored: 'protected' }
    } catch {
      // protection was requested but is broken right now — NEVER lose the
      // credential, fall back to plaintext and say so (the caller logs a WARN)
      return { password: plain, passwordEnc: null, stored: 'plaintext' }
    }
  }
  return { password: plain, passwordEnc: null, stored: 'plaintext' }
}
