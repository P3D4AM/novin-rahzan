#!/usr/bin/env node
/**
 * HozorPlus v1.8.0 (Phase 3 stage 2) — migration: full per-run download
 * accounting columns on DeviceSyncRun.
 *
 * Adds:
 *   * recordsAnnounced        INTEGER NULL  — the device-side record count
 *     when the transport can query it (SBXPC classic/EXT read a count before
 *     the data phase). NULL = the protocol cannot report a count (the FK623
 *     SDK has NO count export — an honest NULL, never a fabricated number).
 *   * recordsInvalidTimestamps INTEGER NOT NULL DEFAULT 0 — records the
 *     transport dropped BEFORE delivery because the timestamp could not be
 *     parsed.
 *   * abnormalEnd              BOOLEAN NOT NULL DEFAULT 0 — the transport read
 *     ended with an UNEXPECTED code (truncated history). Runs with this flag
 *     close PARTIAL so a short historical download is SEEN instead of being
 *     silently presented as complete.
 *
 * Purpose: diagnosing "asked for 90 days, got ~30 records per person" with
 * real numbers — announced vs delivered vs in-window vs stored vs duplicate.
 *
 * Rules honoured (same family as the v1.7.0 migrations):
 *   * INCREMENTAL — adds the three columns ONLY; no existing column/row/index
 *     is touched, nothing is dropped on `up`.
 *   * IDEMPOTENT — column-existence guards; re-runs are no-ops.
 *   * REVERSIBLE (documented) — `down --confirm` drops exactly the three new
 *     columns; all existing rows and punch data are untouched either way.
 *   * COMPATIBLE — `prisma db push` from the current schema converges with no
 *     drift (schema.prisma carries the same columns; verified by the suite).
 *
 * Usage:
 *   node scripts/migrate-v1.8.0-phase3.mjs            # up (default)
 *   node scripts/migrate-v1.8.0-phase3.mjs status
 *   node scripts/migrate-v1.8.0-phase3.mjs down --confirm
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

const ADD_COLUMNS = [
  { table: 'DeviceSyncRun', name: 'recordsAnnounced', ddl: 'INTEGER' },
  { table: 'DeviceSyncRun', name: 'recordsInvalidTimestamps', ddl: 'INTEGER NOT NULL DEFAULT 0' },
  { table: 'DeviceSyncRun', name: 'abnormalEnd', ddl: 'BOOLEAN NOT NULL DEFAULT 0' },
]

async function columnsOf(table) {
  return (await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)).map((c) => c.name)
}

async function up() {
  console.log(`[migrate v1.8.0-phase3] up — database: ${dbUrl}`)
  const existing = await columnsOf('DeviceSyncRun').catch(() => null)
  if (!existing) {
    console.log('[migrate v1.8.0-phase3] DeviceSyncRun table not found — a FRESH database gets these columns from prisma db push; nothing to migrate.')
    return
  }
  for (const col of ADD_COLUMNS) {
    if (existing.includes(col.name)) {
      console.log(`[migrate v1.8.0-phase3] ${col.table}.${col.name} already present`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "${col.table}" ADD COLUMN "${col.name}" ${col.ddl}`)
    console.log(`[migrate v1.8.0-phase3] added ${col.table}.${col.name}`)
  }
  // historical runs keep NULL announced / 0 counters — they predate the
  // counters; NO row is back-filled with invented numbers.
  await status()
}

async function status() {
  const cols = await columnsOf('DeviceSyncRun').catch(() => null)
  if (!cols) {
    console.log('[migrate v1.8.0-phase3] status: DeviceSyncRun table does not exist yet')
    return
  }
  for (const col of ADD_COLUMNS) {
    console.log(`[migrate v1.8.0-phase3] ${col.table}.${col.name} = ${cols.includes(col.name) ? 'present' : 'ABSENT'}`)
  }
  const runs = await db.$queryRawUnsafe('SELECT COUNT(*) AS n FROM "DeviceSyncRun"').catch(() => [{ n: 0 }])
  console.log(`[migrate v1.8.0-phase3] DeviceSyncRun rows: ${Number(runs[0]?.n ?? 0)} (untouched)`)
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.8.0-phase3] rollback drops exactly the three new accounting columns;')
    console.error('[migrate v1.8.0-phase3] all existing rows (incl. punch data) are untouched. Re-run with:')
    console.error('  node scripts/migrate-v1.8.0-phase3.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.8.0-phase3] down — database: ${dbUrl}`)
  for (const col of ADD_COLUMNS) {
    const cols = await columnsOf(col.table)
    if (!cols.includes(col.name)) {
      console.log(`[migrate v1.8.0-phase3] ${col.table}.${col.name} already absent`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "${col.table}" DROP COLUMN "${col.name}"`)
    console.log(`[migrate v1.8.0-phase3] dropped ${col.table}.${col.name}`)
  }
  await status()
}

const cmd = process.argv[2] ?? 'up'
if (cmd === 'up') await up()
else if (cmd === 'status') await status()
else if (cmd === 'down') await down()
else {
  console.error(`[migrate v1.8.0-phase3] unknown command "${cmd}" — use up | status | down --confirm`)
  process.exitCode = 2
}

await db.$disconnect()
