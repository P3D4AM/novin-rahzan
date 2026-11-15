#!/usr/bin/env node
/**
 * HozorPlus v1.4.0 (addendum) — Phase 2 migration: per-device connection,
 * automatic-sync and near-real-time-polling columns on Device.
 *
 * Rules honoured (same as migrate-v1.3.0-phase1.mjs):
 *   * INCREMENTAL — 11 new columns on Device ONLY; no existing table/column/
 *     row/index is touched, nothing is ever dropped on `up`.
 *   * IDEMPOTENT  — safe to run any number of times (column-existence guards).
 *   * REVERSIBLE  — `down --confirm` drops exactly these 11 columns
 *     (RawPunch/DeviceSyncRun from v1.3.0 stay untouched; DOWN never loses
 *     punch data — only the new settings columns go away).
 *   * COMPATIBLE — `prisma db push` on the same DB converges with no drift
 *     (verified by the test suite; INSTALL.bat path keeps working).
 *
 * Usage:
 *   node scripts/migrate-v1.4.0-phase2.mjs            # up (default)
 *   node scripts/migrate-v1.4.0-phase2.mjs status
 *   node scripts/migrate-v1.4.0-phase2.mjs down --confirm
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

// (column, DDL fragment) — additive only
const NEW_COLUMNS = [
  ["protocolFamily", `TEXT NOT NULL DEFAULT 'SBXPC'`],
  ['connTimeoutMs', `INTEGER NOT NULL DEFAULT 5000`],
  ['machineNumber', `INTEGER NOT NULL DEFAULT 1`],
  ['sdkLicense', `INTEGER NOT NULL DEFAULT 1261`],
  ['bridgeUrl', `TEXT`],
  ['bridgeToken', `TEXT`],
  ['lastSyncError', `TEXT`],
  ['rtEnabled', `BOOLEAN NOT NULL DEFAULT 0`],
  ['rtPollSec', `INTEGER NOT NULL DEFAULT 30`],
  ['rtLastPollAt', `DATETIME`],
]

async function columnsOf(table) {
  try {
    return (await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)).map((c) => c.name)
  } catch {
    return []
  }
}

async function up() {
  console.log(`[migrate v1.4.0-phase2] up — database: ${dbUrl}`)
  const existing = await columnsOf('Device')
  if (existing.length === 0) throw new Error('Device table not found — run migrate-v1.3.0-phase1.mjs first')
  for (const [name, ddl] of NEW_COLUMNS) {
    if (existing.includes(name)) {
      console.log(`[migrate v1.4.0-phase2] Device.${name} already present`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "Device" ADD COLUMN "${name}" ${ddl}`)
    console.log(`[migrate v1.4.0-phase2] added Device.${name}`)
  }
  await status()
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.4.0-phase2] rollback drops ONLY the 11 new Device settings columns (raw punch data is untouched).')
    console.error('[migrate v1.4.0-phase2] re-run with:  node scripts/migrate-v1.4.0-phase2.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.4.0-phase2] down — database: ${dbUrl}`)
  for (const [name] of NEW_COLUMNS) {
    const cols = await columnsOf('Device')
    if (!cols.includes(name)) continue
    try {
      await db.$executeRawUnsafe(`ALTER TABLE "Device" DROP COLUMN "${name}"`)
      console.log(`[migrate v1.4.0-phase2] removed Device.${name}`)
    } catch (err) {
      console.warn(`[migrate v1.4.0-phase2] could not drop Device.${name} (SQLite < 3.35): ${err.message}`)
    }
  }
  await status()
}

async function status() {
  const cols = await columnsOf('Device')
  for (const [name] of NEW_COLUMNS) {
    console.log(`[migrate v1.4.0-phase2] Device.${name}: ${cols.includes(name) ? 'present' : 'MISSING'}`)
  }
}

async function main() {
  const cmd = process.argv[2] || 'up'
  try {
    if (cmd === 'up') await up()
    else if (cmd === 'down') await down()
    else if (cmd === 'status') {
      console.log(`[migrate v1.4.0-phase2] status — database: ${dbUrl}`)
      await status()
    } else {
      console.error('usage: migrate-v1.4.0-phase2.mjs [up|down --confirm|status]')
      process.exitCode = 2
    }
  } finally {
    await db.$disconnect()
  }
}

main().catch((err) => {
  console.error('[migrate v1.4.0-phase2] FAILED:', err.message)
  process.exit(1)
})
