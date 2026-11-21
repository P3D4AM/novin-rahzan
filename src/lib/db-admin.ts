/**
 * Phase 1 (v1.8.0-dev) — database administration core.
 * =====================================================================
 * Installer-safe, API-safe, CLI-safe database operations. NEVER used on a
 * "real" database by the test suite (tests own their throwaway sqlite files
 * via scripts/test-db-guard.ts).
 *
 * Hard rules implemented here (per the Phase 1 spec):
 *   * DB file size is NEVER a criterion for anything (explicitly banned).
 *   * Existing data is preserved by default; destructive operations require
 *     explicit confirmation and take a restorable backup FIRST.
 *   * Backups are consistent SQLite snapshots (VACUUM INTO), verified openable.
 *   * Wipe/reset preserves: User (login access survives — personnelId is
 *     detached), Device (connection settings + passwords/secrets), Setting,
 *     NotificationRule, AuditLog (accountability: history is appended, never
 *     erased). Everything business/transactional is wiped.
 *   * Every operation writes an AuditLog row (no secrets in details).
 *   * A single in-process lock makes concurrent destructive ops impossible.
 *   * Operations are transactional: a mid-operation failure rolls back —
 *     the database is never left half-wiped/half-seeded.
 *
 * The functions accept the Prisma client as a parameter so the Next.js API
 * route (shared src/lib/db client) and the installer CLI (own client on the
 * DATABASE_URL of the moment) share ONE implementation.
 */
import fs from 'fs'
import path from 'path'
import { randomBytes } from 'crypto'
import type { Prisma, PrismaClient } from '@prisma/client'

/** Prisma client or interactive transaction client. */
export type DbAdminClient = Prisma.TransactionClient | PrismaClient

export class DbAdminError extends Error {
  status: number
  constructor(status: number, message: string) {
    super(message)
    this.status = status
  }
}

/** Thrown when another admin operation is already running. */
export class DbAdminBusyError extends DbAdminError {
  constructor() {
    super(409, 'عملیات دیگری روی دیتابیس در حال اجراست؛ لطفاً چند لحظه بعد دوباره تلاش کنید')
  }
}

/* ------------------------------------------------------------------ */
/* DATABASE_URL helpers                                                */
/* ------------------------------------------------------------------ */

/** Extract the filesystem path from a Prisma SQLite URL ("file:…"). */
export function resolveSqliteFile(url: string | undefined): string | null {
  let p = String(url ?? '').trim()
  if (!p) return null
  if (p.startsWith('file:')) p = p.slice('file:'.length)
  else if (!p.endsWith('.db') && !p.includes('/')) return null // e.g. a postgres URL — unsupported
  if (p.startsWith('./') || p.startsWith('../')) p = path.resolve(p)
  else if (/^[a-zA-Z]:[\\/]/.test(p) || p.startsWith('/') || p.startsWith('\\\\')) p = p.replace(/%20/g, ' ')
  else p = path.resolve(p)
  return p || null
}

function isSqliteDatabase(client: DbAdminClient): boolean {
  const url = String(process.env.DATABASE_URL ?? '')
  return url.startsWith('file:') || /\.db$/.test(url)
}

function timestampedName(prefix: string): string {
  const d = new Date()
  const p = (n: number) => String(n).padStart(2, '0')
  const rand = randomBytes(2).toString('hex')
  return `${prefix}-${d.getUTCFullYear()}${p(d.getUTCMonth() + 1)}${p(d.getUTCDate())}-${p(d.getUTCHours())}${p(d.getUTCMinutes())}${p(d.getUTCSeconds())}-${rand}.db`
}

/* ------------------------------------------------------------------ */
/* Status                                                             */
/* ------------------------------------------------------------------ */

export interface BackupInfo { file: string; bytes: number; at: string }

export interface DbStatus {
  dbPath: string | null
  dbExists: boolean
  sizeBytes: number | null
  /** File size is INFORMATIONAL ONLY — never a criterion (Phase 1 rule). */
  sizeIsCriterion: false
  schemaOk: boolean
  hasBusinessData: boolean
  counts: {
    users: number
    superAdmins: number
    personnel: number
    punches: number
    rawPunches: number
    dailyAttendance: number
    leaveRequests: number
    overtimeRequests: number
    devices: number
    shifts: number
    auditLog: number
  } | null
  lastBackupAt: string | null
  lastBackupPath: string | null
  backupDir: string | null
  backups: BackupInfo[]
}

export async function dbStatus(client: PrismaClient): Promise<DbStatus> {
  const dbPath = resolveSqliteFile(process.env.DATABASE_URL)
  const dbExists = !!dbPath && fs.existsSync(dbPath)
  const sizeBytes = dbExists && dbPath ? fs.statSync(dbPath).size : null
  const backupDir = dbPath ? path.join(path.dirname(dbPath), 'backups') : null

  let counts: DbStatus['counts'] = null
  let schemaOk = false
  try {
    const [users, superAdmins, personnel, punches, rawPunches, dailyAttendance, leaveRequests, overtimeRequests, devices, shifts, auditLog] =
      await Promise.all([
        client.user.count(), client.user.count({ where: { role: 'SUPER_ADMIN' } }),
        client.personnel.count(), client.punchRecord.count(), client.rawPunch.count(),
        client.dailyAttendance.count(), client.leaveRequest.count(), client.overtimeRequest.count(),
        client.device.count(), client.shift.count(), client.auditLog.count(),
      ])
    counts = { users, superAdmins, personnel, punches, rawPunches, dailyAttendance, leaveRequests, overtimeRequests, devices, shifts, auditLog }
    schemaOk = true
  } catch {
    // P2021/P2022 — schema not created yet (or unreadable): report, never guess.
    schemaOk = false
  }

  const hasBusinessData = !!(counts && (counts.personnel > 0 || counts.punches > 0 || counts.rawPunches > 0 ||
    counts.dailyAttendance > 0 || counts.leaveRequests > 0 || counts.overtimeRequests > 0 || counts.shifts > 0))

  let lastBackupAt: string | null = null
  let lastBackupPath: string | null = null
  if (schemaOk) {
    const at = await client.setting.findUnique({ where: { id: 'last_backup_at' } }).catch(() => null)
    const p = await client.setting.findUnique({ where: { id: 'last_backup_path' } }).catch(() => null)
    lastBackupAt = at?.value ?? null
    lastBackupPath = p?.value ?? null
  }

  const backups: BackupInfo[] = []
  if (backupDir && fs.existsSync(backupDir)) {
    for (const f of fs.readdirSync(backupDir).filter((x) => x.endsWith('.db')).sort().reverse().slice(0, 10)) {
      const full = path.join(backupDir, f)
      try {
        const st = fs.statSync(full)
        backups.push({ file: full, bytes: st.size, at: st.mtime.toISOString() })
      } catch { /* unreadable entry — skip */ }
    }
  }

  return { dbPath, dbExists, sizeBytes, sizeIsCriterion: false, schemaOk, hasBusinessData, counts, lastBackupAt, lastBackupPath, backupDir, backups }
}

/* ------------------------------------------------------------------ */
/* Backup                                                             */
/* ------------------------------------------------------------------ */

export interface BackupResult { path: string; bytes: number }

/**
 * Consistent, restorable snapshot of the SQLite database (VACUUM INTO).
 * Runs OUTSIDE any transaction by design (SQLite requirement) and is safe to
 * run while the app is up.
 */
export async function dbBackup(client: PrismaClient, opts?: { reason?: string }): Promise<BackupResult> {
  if (!isSqliteDatabase(client)) throw new DbAdminError(400, 'پشتیبان‌گیری فقط برای دیتابیس SQLite پشتیبانی می‌شود')
  const dbPath = resolveSqliteFile(process.env.DATABASE_URL)
  if (!dbPath || !fs.existsSync(dbPath)) throw new DbAdminError(400, 'فایل دیتابیس یافت نشد')
  const backupDir = path.join(path.dirname(dbPath), 'backups')
  fs.mkdirSync(backupDir, { recursive: true })
  const target = path.join(backupDir, timestampedName('hozorplus-backup'))
  // escape single quotes for the SQL string literal
  const sqlPath = target.replace(/'/g, "''")
  await client.$executeRawUnsafe(`VACUUM INTO '${sqlPath}'`)
  const bytes = fs.statSync(target).size
  // verify the snapshot is a REAL sqlite database before calling it a backup
  const fd = fs.openSync(target, 'r')
  try {
    const header = Buffer.alloc(16)
    fs.readSync(fd, header, 0, 16, 0)
    if (header.slice(0, 15).toString('latin1') !== 'SQLite format 3') {
      throw new DbAdminError(500, 'پشتیبان ایجاد شد اما فایل خروجی SQLite معتبر نیست — عملیات لغو شد')
    }
  } finally {
    fs.closeSync(fd)
  }
  await client.setting.upsert({ where: { id: 'last_backup_at' }, create: { id: 'last_backup_at', value: new Date().toISOString() }, update: { value: new Date().toISOString() } })
  await client.setting.upsert({ where: { id: 'last_backup_path' }, create: { id: 'last_backup_path', value: target }, update: { value: target } })
  await client.auditLog.create({ data: { action: 'DB_BACKUP', entity: 'Database', details: JSON.stringify({ path: target, bytes, reason: opts?.reason ?? '' }) } })
  return { path: target, bytes }
}

/* ------------------------------------------------------------------ */
/* Wipe (reset to empty)                                              */
/* ------------------------------------------------------------------ */

export interface WipeCounts {
  punchRecord: number; rawPunch: number; rejectedRecord: number; dailyAttendance: number
  leaveRequest: number; overtimeRequest: number; shiftAssignment: number; defaultShiftHistory: number
  notification: number; deviceLog: number; deviceSyncRun: number; deviceSyncLock: number; session: number
  personnel: number; shiftSegment: number; shiftRotation: number; shift: number; leaveType: number
  holiday: number; department: number; jobTitle: number; employmentType: number
}

/** The tables wiped inside the transaction, children first (FK-safe order). */
async function wipeBusinessData(tx: DbAdminClient): Promise<WipeCounts> {
  // detach user→personnel links explicitly (independent of FK enforcement)
  await tx.user.updateMany({ data: { personnelId: null } })
  const punchRecord = await tx.punchRecord.deleteMany()
  const rawPunch = await tx.rawPunch.deleteMany()
  const rejectedRecord = await tx.rejectedRecord.deleteMany()
  const dailyAttendance = await tx.dailyAttendance.deleteMany()
  const leaveRequest = await tx.leaveRequest.deleteMany()
  const overtimeRequest = await tx.overtimeRequest.deleteMany()
  const shiftAssignment = await tx.shiftAssignment.deleteMany()
  const defaultShiftHistory = await tx.defaultShiftHistory.deleteMany()
  const notification = await tx.notification.deleteMany()
  const deviceLog = await tx.deviceLog.deleteMany()
  const deviceSyncRun = await tx.deviceSyncRun.deleteMany()
  const deviceSyncLock = await tx.deviceSyncLock.deleteMany()
  const session = await tx.session.deleteMany() // force re-login (fresh state)
  const personnel = await tx.personnel.deleteMany()
  const shiftSegment = await tx.shiftSegment.deleteMany()
  const shiftRotation = await tx.shiftRotation.deleteMany()
  const shift = await tx.shift.deleteMany()
  const leaveType = await tx.leaveType.deleteMany()
  const holiday = await tx.holiday.deleteMany()
  const department = await tx.department.deleteMany()
  const jobTitle = await tx.jobTitle.deleteMany()
  const employmentType = await tx.employmentType.deleteMany()
  return {
    punchRecord: punchRecord.count, rawPunch: rawPunch.count, rejectedRecord: rejectedRecord.count,
    dailyAttendance: dailyAttendance.count, leaveRequest: leaveRequest.count, overtimeRequest: overtimeRequest.count,
    shiftAssignment: shiftAssignment.count, defaultShiftHistory: defaultShiftHistory.count, notification: notification.count,
    deviceLog: deviceLog.count, deviceSyncRun: deviceSyncRun.count, deviceSyncLock: deviceSyncLock.count,
    session: session.count, personnel: personnel.count, shiftSegment: shiftSegment.count,
    shiftRotation: shiftRotation.count, shift: shift.count, leaveType: leaveType.count, holiday: holiday.count,
    department: department.count, jobTitle: jobTitle.count, employmentType: employmentType.count,
  }
}

export interface WipeResult { backup: BackupResult | null; wiped: WipeCounts; auditId: string }

/**
 * Reset to an EMPTY database (schema intact — this is NOT "drop the schema").
 * Login access is preserved (User rows survive), so are devices, settings and
 * the audit trail. Takes a backup first unless explicitly disabled (tests).
 */
export async function dbWipeToEmpty(client: PrismaClient, opts?: { skipBackup?: boolean; reason?: string }): Promise<WipeResult> {
  return withDbAdminLock(async () => {
    const backup = opts?.skipBackup ? null : await dbBackup(client, { reason: opts?.reason ?? 'reset-to-empty' })
    const wiped = await client.$transaction(async (tx) => wipeBusinessData(tx), { timeout: 300_000, maxWait: 60_000 })
    const audit = await client.auditLog.create({
      data: { action: 'DB_RESET', entity: 'Database', details: JSON.stringify({ backup: backup?.path ?? null, wipedTotals: { personnel: wiped.personnel, punchRecord: wiped.punchRecord, rawPunch: wiped.rawPunch } }) },
    })
    return { backup, wiped, auditId: audit.id }
  })
}

/* ------------------------------------------------------------------ */
/* Demo seed                                                          */
/* ------------------------------------------------------------------ */

export interface DemoSeedResult {
  backup: BackupResult | null
  wiped: WipeCounts
  seeded: import('./demo-seed').SeedResult
  auditId: string
}

/**
 * Install demo data: backup → ONE transaction [wipe business data → create
 * demo data] → audit. A failure anywhere rolls back to the pre-operation
 * state (never half-seeded). Protected classes (users/devices/settings/
 * rules/audit) are preserved by the wipe and honoured by the seed — see
 * src/lib/demo-seed.ts.
 */
export async function dbDemoSeed(client: PrismaClient, opts?: { skipBackup?: boolean; reason?: string }): Promise<DemoSeedResult> {
  return withDbAdminLock(async () => {
    const backup = opts?.skipBackup ? null : await dbBackup(client, { reason: opts?.reason ?? 'demo-seed' })
    const result = await client.$transaction(
      async (tx) => {
        const wiped = await wipeBusinessData(tx)
        const seeded = await (await import('./demo-seed')).seedDemoData(tx)
        return { wiped, seeded }
      },
      { timeout: 600_000, maxWait: 60_000 },
    )
    const audit = await client.auditLog.create({
      data: {
        action: 'DB_SEED', entity: 'Database',
        details: JSON.stringify({
          backup: backup?.path ?? null,
          persons: result.seeded.persons, punches: result.seeded.punches,
          usersCreated: result.seeded.usersCreated, deviceCreated: result.seeded.deviceCreated,
          settingsPreserved: result.seeded.settingsPreserved,
        }),
      },
    })
    return { backup, wiped: result.wiped, seeded: result.seeded, auditId: audit.id }
  })
}

/* ------------------------------------------------------------------ */
/* Bootstrap admin (fresh installs)                                   */
/* ------------------------------------------------------------------ */

export interface AdminBootstrapResult { created: boolean; username: string; password?: string; passwordFile?: string }

/**
 * Guarantee at least one SUPER_ADMIN account exists. On a fresh empty install
 * this creates `admin` with a RANDOM one-time password that is written to
 * FIRST-LOGIN.txt next to the database (never into AuditLog/logs).
 */
export async function dbEnsureSuperAdmin(client: PrismaClient): Promise<AdminBootstrapResult> {
  const existing = await client.user.count({ where: { role: 'SUPER_ADMIN', active: true } })
  if (existing > 0) return { created: false, username: '' }

  // reuse the same hash as the app (scrypt salted) — never a plaintext column
  const { hashPassword } = await import('./auth-hash')
  const password = randomPassword(12)
  const dbPath = resolveSqliteFile(process.env.DATABASE_URL)
  const file = dbPath ? path.join(path.dirname(dbPath), 'FIRST-LOGIN.txt') : null
  await client.user.create({
    data: { username: 'admin', passwordHash: hashPassword(password), role: 'SUPER_ADMIN', fullName: 'مدیر ارشد سیستم', active: true },
  })
  if (file) {
    const d = new Date()
    fs.writeFileSync(file, [
      'HozorPlus — ورود اولیه (اولین نصب)',
      '=====================================',
      'نام کاربری: admin',
      `رمز عبور:  ${password}`,
      '',
      `تاریخ ایجاد: ${d.toISOString()}`,
      '',
      'این رمز یک‌بار تولید شده است و در هیچ لاگ دیگری ذخیره نمی‌شود.',
      'پس از اولین ورود، رمز را از «تنظیمات → کاربران» تغییر کنید',
      'و سپس این فایل را حذف کنید.',
      '',
    ].join('\n'), 'utf-8')
  }
  await client.auditLog.create({ data: { action: 'DB_ADMIN_INIT', entity: 'User', details: JSON.stringify({ username: 'admin', role: 'SUPER_ADMIN', passwordFile: file }) } })
  return { created: true, username: 'admin', password, passwordFile: file ?? undefined }
}

function randomPassword(len: number): string {
  const alphabet = 'ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789'
  const bytes = randomBytes(len)
  let out = ''
  for (let i = 0; i < len; i++) out += alphabet[bytes[i] % alphabet.length]
  return out
}

/* ------------------------------------------------------------------ */
/* In-process operation lock                                          */
/* ------------------------------------------------------------------ */

let opLock = false

/** Serialize destructive operations inside this process (single-server app). */
export async function withDbAdminLock<T>(fn: () => Promise<T>): Promise<T> {
  if (opLock) throw new DbAdminBusyError()
  opLock = true
  try {
    return await fn()
  } finally {
    opLock = false
  }
}
