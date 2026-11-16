#!/usr/bin/env node
/**
 * HozorPlus v1.3.1 — Phase 1 code-review corrections migration.
 *
 * Adds (INCREMENTAL — nothing existing is touched or dropped):
 *   * RejectedRecord table + indexes      (fix 4 — quarantine for malformed source records)
 *   * DeviceSyncLock table + index        (fix 7 — cross-process per-device sync lock)
 *   * DeviceSyncRun.recordsRejected / recordsOutOfWindow columns (fix 4 — full accounting)
 *   * RawPunch.matchNote column           (fix 1 — why a row stays UNMATCHED / ambiguous)
 *   * Device.passwordEnc column           (fix 6 — opt-in DPAPI at-rest protection storage)
 *
 * Rules honoured (same as migrate-v1.3.0-phase1.mjs):
 *   * INCREMENTAL & COMPATIBLE — every statement is IF NOT EXISTS / guarded;
 *     existing rows, columns and indexes are never modified or deleted.
 *     Existing device passwords stay exactly where they are (the plaintext
 *     column keeps working — protection is opt-in via a separate script).
 *   * REVERSIBLE — `down --confirm` removes exactly what `up` added.
 *     Device.passwordEnc is dropped EMPTY (protected values die with the
 *     column); the plaintext `password` column and every stored credential
 *     are untouched in both directions.
 *   * IDEMPOTENT — safe to run any number of times.
 *   * PARITY — the DDL is what `prisma db push` derives from the current
 *     prisma/schema.prisma (verified by the Phase 1 test suite).
 *
 * Usage:
 *   node scripts/migrate-v1.3.1-phase1-fixes.mjs            # up (default)
 *   node scripts/migrate-v1.3.1-phase1-fixes.mjs status
 *   node scripts/migrate-v1.3.1-phase1-fixes.mjs down --confirm
 */
import { createRequire } from 'module'
import { existsSync, readFileSync } from 'fs'
import { fileURLToPath } from 'url'
import path from 'path'

const require = createRequire(import.meta.url)
const { PrismaClient } = require('@prisma/client')

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '..')

function resolveDbUrl() {
  if (process.env.DATABASE_URL) return process.env.DATABASE_URL
  const envFile = path.join(root, '.env')
  if (existsSync(envFile)) {
    for (const line of readFileSync(envFile, 'utf8').split(/\r?\n/)) {
      const m = line.match(/^\s*DATABASE_URL\s*=\s*(.+)\s*$/)
      if (m) return m[1].trim().replace(/^["']|["']$/g, '')
    }
  }
  return 'file:' + path.join('db', 'custom.db')
}

const dbUrl = resolveDbUrl()
process.env.DATABASE_URL = dbUrl
const db = new PrismaClient()

const UP_STATEMENTS = [
  `CREATE TABLE IF NOT EXISTS "RejectedRecord" (
    "id" TEXT NOT NULL PRIMARY KEY,
    "deviceId" TEXT,
    "deviceSerial" TEXT NOT NULL DEFAULT '',
    "sourceProtocol" TEXT NOT NULL DEFAULT 'UNKNOWN',
    "syncRunId" TEXT,
    "enrollNumber" TEXT,
    "punchTimeRaw" TEXT,
    "rawPayload" TEXT,
    "rejectReason" TEXT NOT NULL,
    "sourceMeta" TEXT,
    "fingerprint" TEXT NOT NULL,
    "receivedAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "RejectedRecord_deviceId_fkey" FOREIGN KEY ("deviceId") REFERENCES "Device" ("id") ON DELETE SET NULL ON UPDATE CASCADE
  )`,
  `CREATE UNIQUE INDEX IF NOT EXISTS "RejectedRecord_fingerprint_key" ON "RejectedRecord"("fingerprint")`,
  `CREATE INDEX IF NOT EXISTS "RejectedRecord_deviceSerial_receivedAt_idx" ON "RejectedRecord"("deviceSerial", "receivedAt")`,
  `CREATE INDEX IF NOT EXISTS "RejectedRecord_rejectReason_idx" ON "RejectedRecord"("rejectReason")`,
  `CREATE TABLE IF NOT EXISTS "DeviceSyncLock" (
    "deviceId" TEXT NOT NULL PRIMARY KEY,
    "lockToken" TEXT NOT NULL,
    "acquiredAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "expiresAt" DATETIME NOT NULL
  )`,
  `CREATE INDEX IF NOT EXISTS "DeviceSyncLock_expiresAt_idx" ON "DeviceSyncLock"("expiresAt")`,
]

async function addColumn(table, column, ddl) {
  const cols = await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)
  const has = Array.isArray(cols) && cols.some((c) => c.name === column)
  if (has) {
    console.log(`[migrate v1.3.1] ${table}.${column} already present`)
    return false
  }
  await db.$executeRawUnsafe(`ALTER TABLE "${table}" ADD COLUMN "${column}" ${ddl}`)
  console.log(`[migrate v1.3.1] added ${table}.${column}`)
  return true
}

async function up() {
  console.log(`[migrate v1.3.1] up — database: ${dbUrl}`)
  for (const stmt of UP_STATEMENTS) {
    await db.$executeRawUnsafe(stmt)
  }
  // guarded ALTERs (SQLite has no ADD COLUMN IF NOT EXISTS)
  await addColumn('DeviceSyncRun', 'recordsRejected', 'INTEGER NOT NULL DEFAULT 0')
  await addColumn('DeviceSyncRun', 'recordsOutOfWindow', 'INTEGER NOT NULL DEFAULT 0')
  await addColumn('RawPunch', 'matchNote', 'TEXT')
  await addColumn('Device', 'passwordEnc', 'TEXT')
  await status()
}

const DOWN_STATEMENTS = [
  `DROP INDEX IF EXISTS "DeviceSyncLock_expiresAt_idx"`,
  `DROP TABLE IF EXISTS "DeviceSyncLock"`,
  `DROP INDEX IF EXISTS "RejectedRecord_rejectReason_idx"`,
  `DROP INDEX IF EXISTS "RejectedRecord_deviceSerial_receivedAt_idx"`,
  `DROP INDEX IF EXISTS "RejectedRecord_fingerprint_key"`,
  `DROP TABLE IF EXISTS "RejectedRecord"`,
]

async function dropColumn(table, column) {
  const cols = await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)
  const has = Array.isArray(cols) && cols.some((c) => c.name === column)
  if (!has) return
  try {
    await db.$executeRawUnsafe(`ALTER TABLE "${table}" DROP COLUMN "${column}"`)
    console.log(`[migrate v1.3.1] removed ${table}.${column}`)
  } catch (err) {
    console.warn(`[migrate v1.3.1] could not drop ${table}.${column} (SQLite < 3.35): ${err.message}`)
  }
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.3.1] rollback drops the RejectedRecord + DeviceSyncLock tables and the new columns.')
    console.error('[migrate v1.3.1] Device.password / Device.passwordEnc note: only the (opt-in, empty by default) passwordEnc column is dropped — stored credentials are NOT touched.')
    console.error('[migrate v1.3.1] re-run with:  node scripts/migrate-v1.3.1-phase1-fixes.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.3.1] down — database: ${dbUrl}`)
  for (const stmt of DOWN_STATEMENTS) {
    await db.$executeRawUnsafe(stmt)
  }
  await dropColumn('RawPunch', 'matchNote')
  await dropColumn('DeviceSyncRun', 'recordsRejected')
  await dropColumn('DeviceSyncRun', 'recordsOutOfWindow')
  await dropColumn('Device', 'passwordEnc')
  await status()
}

async function tableExists(name) {
  const rows = await db.$queryRawUnsafe(`SELECT name FROM sqlite_master WHERE type='table' AND name='${name}'`)
  return rows.length > 0
}

async function columnsOf(table) {
  try {
    return await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)
  } catch {
    return []
  }
}

async function status() {
  const has = async (t) => (await tableExists(t)) ? 'present' : 'MISSING'
  const col = async (t, c) => (Array.isArray(await columnsOf(t)) && (await columnsOf(t)).some((x) => x.name === c)) ? 'present' : 'MISSING'
  console.log(`[migrate v1.3.1] RejectedRecord table: ${await has('RejectedRecord')}`)
  console.log(`[migrate v1.3.1] DeviceSyncLock table: ${await has('DeviceSyncLock')}`)
  console.log(`[migrate v1.3.1] DeviceSyncRun.recordsRejected: ${await col('DeviceSyncRun', 'recordsRejected')}`)
  console.log(`[migrate v1.3.1] DeviceSyncRun.recordsOutOfWindow: ${await col('DeviceSyncRun', 'recordsOutOfWindow')}`)
  console.log(`[migrate v1.3.1] RawPunch.matchNote: ${await col('RawPunch', 'matchNote')}`)
  console.log(`[migrate v1.3.1] Device.passwordEnc: ${await col('Device', 'passwordEnc')}`)
}

async function main() {
  const cmd = process.argv[2] || 'up'
  try {
    if (cmd === 'up') await up()
    else if (cmd === 'down') await down()
    else if (cmd === 'status') {
      console.log(`[migrate v1.3.1] status — database: ${dbUrl}`)
      await status()
    } else {
      console.error('usage: migrate-v1.3.1-phase1-fixes.mjs [up|down --confirm|status]')
      process.exitCode = 2
    }
  } finally {
    await db.$disconnect()
  }
}

main().catch((err) => {
  console.error('[migrate v1.3.1] FAILED:', err.message)
  process.exit(1)
})
