#!/usr/bin/env node
/**
 * HozorPlus v1.3.0 — Phase 1 migration: RawPunch + DeviceSyncRun (+ Device.rawCursorAt).
 *
 * Rules honoured (Blueprint «Data Integrity & Security»):
 *   * INCREMENTAL — only new tables / one new nullable column are added.
 *     No existing table, column, row or index is touched or dropped.
 *   * REVERSIBLE — `node scripts/migrate-v1.3.0-phase1.mjs down --confirm`
 *     removes exactly what `up` added (RawPunch/DeviceSyncRun rows are dropped
 *     with the tables — existing PunchRecord data is NOT affected).
 *   * IDEMPOTENT — safe to run any number of times (IF NOT EXISTS + guards).
 *   * COMPATIBLE — the DDL below is byte-identical to what `prisma db push`
 *     derives from prisma/schema.prisma, so INSTALL.bat (db push) and this
 *     script converge on the same schema (verified in the Phase 1 tests).
 *
 * Usage:
 *   node scripts/migrate-v1.3.0-phase1.mjs            # up (default)
 *   node scripts/migrate-v1.3.0-phase1.mjs status    # what exists right now
 *   node scripts/migrate-v1.3.0-phase1.mjs down --confirm   # rollback
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
  `CREATE TABLE IF NOT EXISTS "RawPunch" (
    "id" TEXT NOT NULL PRIMARY KEY,
    "deviceSerial" TEXT NOT NULL,
    "sourceProtocol" TEXT NOT NULL DEFAULT 'UNKNOWN',
    "enrollNumber" TEXT NOT NULL,
    "punchTime" DATETIME NOT NULL,
    "punchTimeRaw" TEXT NOT NULL DEFAULT '',
    "inOutModeRaw" INTEGER NOT NULL DEFAULT 0,
    "verifyModeRaw" INTEGER NOT NULL DEFAULT 0,
    "workCode" TEXT NOT NULL DEFAULT '',
    "sourceRecordId" TEXT NOT NULL DEFAULT '',
    "rawFingerprint" TEXT NOT NULL,
    "dupIndex" INTEGER NOT NULL DEFAULT 0,
    "deviceId" TEXT,
    "personnelId" TEXT,
    "matchStatus" TEXT NOT NULL DEFAULT 'UNMATCHED',
    "direction" TEXT NOT NULL DEFAULT 'UNKNOWN',
    "rawPayload" TEXT,
    "syncRunId" TEXT,
    "importedAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "RawPunch_deviceId_fkey" FOREIGN KEY ("deviceId") REFERENCES "Device" ("id") ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT "RawPunch_personnelId_fkey" FOREIGN KEY ("personnelId") REFERENCES "Personnel" ("id") ON DELETE SET NULL ON UPDATE CASCADE
)`,
  `CREATE TABLE IF NOT EXISTS "DeviceSyncRun" (
    "id" TEXT NOT NULL PRIMARY KEY,
    "deviceId" TEXT,
    "deviceSerial" TEXT NOT NULL DEFAULT '',
    "sourceProtocol" TEXT NOT NULL DEFAULT 'UNKNOWN',
    "mode" TEXT NOT NULL DEFAULT 'HARDWARE',
    "status" TEXT NOT NULL DEFAULT 'RUNNING',
    "startedAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "finishedAt" DATETIME,
    "recordsSeen" INTEGER NOT NULL DEFAULT 0,
    "recordsInWindow" INTEGER NOT NULL DEFAULT 0,
    "recordsNew" INTEGER NOT NULL DEFAULT 0,
    "recordsDuplicate" INTEGER NOT NULL DEFAULT 0,
    "recordsMatched" INTEGER NOT NULL DEFAULT 0,
    "recordsUnmatched" INTEGER NOT NULL DEFAULT 0,
    "bookmarkAt" DATETIME,
    "retries" INTEGER NOT NULL DEFAULT 0,
    "error" TEXT,
    "note" TEXT,
    CONSTRAINT "DeviceSyncRun_deviceId_fkey" FOREIGN KEY ("deviceId") REFERENCES "Device" ("id") ON DELETE SET NULL ON UPDATE CASCADE
)`,
  `CREATE INDEX IF NOT EXISTS "RawPunch_punchTime_idx" ON "RawPunch"("punchTime")`,
  `CREATE INDEX IF NOT EXISTS "RawPunch_deviceSerial_punchTime_idx" ON "RawPunch"("deviceSerial", "punchTime")`,
  `CREATE INDEX IF NOT EXISTS "RawPunch_personnelId_idx" ON "RawPunch"("personnelId")`,
  `CREATE INDEX IF NOT EXISTS "RawPunch_matchStatus_idx" ON "RawPunch"("matchStatus")`,
  `CREATE UNIQUE INDEX IF NOT EXISTS "RawPunch_deviceSerial_sourceProtocol_enrollNumber_rawFingerprint_dupIndex_key" ON "RawPunch"("deviceSerial", "sourceProtocol", "enrollNumber", "rawFingerprint", "dupIndex")`,
  `CREATE INDEX IF NOT EXISTS "DeviceSyncRun_deviceSerial_startedAt_idx" ON "DeviceSyncRun"("deviceSerial", "startedAt")`,
  `CREATE INDEX IF NOT EXISTS "DeviceSyncRun_deviceId_idx" ON "DeviceSyncRun"("deviceId")`,
]

const DOWN_STATEMENTS = [
  `DROP INDEX IF EXISTS "DeviceSyncRun_deviceId_idx"`,
  `DROP INDEX IF EXISTS "DeviceSyncRun_deviceSerial_startedAt_idx"`,
  `DROP INDEX IF EXISTS "RawPunch_deviceSerial_sourceProtocol_enrollNumber_rawFingerprint_dupIndex_key"`,
  `DROP INDEX IF EXISTS "RawPunch_matchStatus_idx"`,
  `DROP INDEX IF EXISTS "RawPunch_personnelId_idx"`,
  `DROP INDEX IF EXISTS "RawPunch_deviceSerial_punchTime_idx"`,
  `DROP INDEX IF EXISTS "RawPunch_punchTime_idx"`,
  `DROP TABLE IF EXISTS "DeviceSyncRun"`,
  `DROP TABLE IF EXISTS "RawPunch"`,
]

async function tableExists(name) {
  const rows = await db.$queryRawUnsafe(`SELECT name FROM sqlite_master WHERE type='table' AND name=${'?'}`)
  return rows.length > 0
}

async function columnsOf(table) {
  return db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)
}

async function deviceHasRawCursor() {
  try {
    const cols = await columnsOf('Device')
    return Array.isArray(cols) && cols.some((c) => c.name === 'rawCursorAt')
  } catch {
    return false
  }
}

async function up() {
  console.log(`[migrate v1.3.0-phase1] up — database: ${dbUrl}`)
  for (const stmt of UP_STATEMENTS) {
    await db.$executeRawUnsafe(stmt)
  }
  if (!(await deviceHasRawCursor())) {
    await db.$executeRawUnsafe(`ALTER TABLE "Device" ADD COLUMN "rawCursorAt" DATETIME`)
    console.log('[migrate v1.3.0-phase1] added Device.rawCursorAt')
  } else {
    console.log('[migrate v1.3.0-phase1] Device.rawCursorAt already present')
  }
  await status()
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.3.0-phase1] rollback drops the RawPunch + DeviceSyncRun tables (all raw punch rows are lost).')
    console.error('[migrate v1.3.0-phase1] re-run with:  node scripts/migrate-v1.3.0-phase1.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.3.0-phase1] down — database: ${dbUrl}`)
  for (const stmt of DOWN_STATEMENTS) {
    await db.$executeRawUnsafe(stmt)
  }
  if (await deviceHasRawCursor()) {
    try {
      await db.$executeRawUnsafe(`ALTER TABLE "Device" DROP COLUMN "rawCursorAt"`)
      console.log('[migrate v1.3.0-phase1] removed Device.rawCursorAt')
    } catch (err) {
      console.warn('[migrate v1.3.0-phase1] could not drop Device.rawCursorAt (SQLite < 3.35): ' + err.message)
    }
  }
  await status()
}

async function status() {
  const rawTable = (await db.$queryRawUnsafe(`SELECT name FROM sqlite_master WHERE type='table' AND name='RawPunch'`)).length > 0
  const syncTable = (await db.$queryRawUnsafe(`SELECT name FROM sqlite_master WHERE type='table' AND name='DeviceSyncRun'`)).length > 0
  const cursor = await deviceHasRawCursor()
  let rawRows = 0
  if (rawTable) {
    const r = await db.$queryRawUnsafe('SELECT COUNT(*) AS n FROM "RawPunch"')
    rawRows = Number(r[0].n)
  }
  console.log(`[migrate v1.3.0-phase1] RawPunch table: ${rawTable ? 'present' : 'MISSING'} (${rawRows} rows)`)
  console.log(`[migrate v1.3.0-phase1] DeviceSyncRun table: ${syncTable ? 'present' : 'MISSING'}`)
  console.log(`[migrate v1.3.0-phase1] Device.rawCursorAt column: ${cursor ? 'present' : 'MISSING'}`)
}

async function main() {
  const cmd = process.argv[2] || 'up'
  try {
    if (cmd === 'up') await up()
    else if (cmd === 'down') await down()
    else if (cmd === 'status') {
      console.log(`[migrate v1.3.0-phase1] status — database: ${dbUrl}`)
      await status()
    } else {
      console.error('usage: migrate-v1.3.0-phase1.mjs [up|down --confirm|status]')
      process.exitCode = 2
    }
  } finally {
    await db.$disconnect()
  }
}

main().catch((err) => {
  console.error('[migrate v1.3.0-phase1] FAILED:', err.message)
  process.exit(1)
})
