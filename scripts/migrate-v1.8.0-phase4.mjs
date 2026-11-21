#!/usr/bin/env node
/**
 * HozorPlus v1.8.0 (Phase 3 stage 6 — bulk operations) — migration: the
 * tombstone + manual-override columns.
 *
 * Adds:
 *   * RawPunch.deletedAt / deletedBy / deleteReason — the TOMBSTONE of an
 *     operational delete: the received device record is NEVER destroyed; the
 *     row stays (who/when/why) and is excluded from live views. The
 *     fingerprint dedupe still sees the tombstoned row → re-reading the same
 *     physical record can never resurrect a deleted punch.
 *   * DailyAttendance.manualOverride / overrideBy / overrideAt — a RECORDED
 *     administrative correction of the derived row that SURVIVES
 *     recalculation (recalcRange recomputes the day, then re-applies the
 *     override fields on top); clearing it returns the row to computed values.
 *
 * Rules honoured (same family as the v1.7.0/v1.8.0 migrations):
 *   * INCREMENTAL — adds the six columns ONLY; nothing dropped on `up`.
 *   * IDEMPOTENT — column-existence guards; re-runs are no-ops.
 *   * REVERSIBLE (documented) — `down --confirm` drops exactly the new
 *     columns; all rows and punch data are untouched either way.
 *   * COMPATIBLE — `prisma db push` from the current schema converges.
 *
 * Usage:
 *   node scripts/migrate-v1.8.0-phase4.mjs            # up (default)
 *   node scripts/migrate-v1.8.0-phase4.mjs status
 *   node scripts/migrate-v1.8.0-phase4.mjs down --confirm
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
  { table: 'RawPunch', name: 'deletedAt', ddl: 'DATETIME' },
  { table: 'RawPunch', name: 'deletedBy', ddl: 'TEXT' },
  { table: 'RawPunch', name: 'deleteReason', ddl: 'TEXT' },
  { table: 'DailyAttendance', name: 'manualOverride', ddl: 'TEXT' },
  { table: 'DailyAttendance', name: 'overrideBy', ddl: 'TEXT' },
  { table: 'DailyAttendance', name: 'overrideAt', ddl: 'DATETIME' },
]

async function columnsOf(table) {
  return (await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)).map((c) => c.name)
}

async function up() {
  console.log(`[migrate v1.8.0-phase4] up — database: ${dbUrl}`)
  for (const t of ['RawPunch', 'DailyAttendance']) {
    const cols = await columnsOf(t).catch(() => null)
    if (!cols) {
      console.log(`[migrate v1.8.0-phase4] ${t} table not found — a FRESH database gets these columns from prisma db push; nothing to migrate.`)
      continue
    }
    for (const col of ADD_COLUMNS.filter((c) => c.table === t)) {
      if (cols.includes(col.name)) {
        console.log(`[migrate v1.8.0-phase4] ${t}.${col.name} already present`)
        continue
      }
      await db.$executeRawUnsafe(`ALTER TABLE "${t}" ADD COLUMN "${col.name}" ${col.ddl}`)
      console.log(`[migrate v1.8.0-phase4] added ${t}.${col.name}`)
    }
  }
  // existing rows read as "live / not overridden" (NULL) — nothing back-filled
  await status()
}

async function status() {
  for (const t of ['RawPunch', 'DailyAttendance']) {
    const cols = await columnsOf(t).catch(() => null)
    if (!cols) { console.log(`[migrate v1.8.0-phase4] status: ${t} table does not exist yet`); continue }
    for (const col of ADD_COLUMNS.filter((c) => c.table === t)) {
      console.log(`[migrate v1.8.0-phase4] ${t}.${col.name} = ${cols.includes(col.name) ? 'present' : 'ABSENT'}`)
    }
    const n = await db.$queryRawUnsafe(`SELECT COUNT(*) AS n FROM "${t}"`).catch(() => [{ n: 0 }])
    console.log(`[migrate v1.8.0-phase4] ${t} rows: ${Number(n[0]?.n ?? 0)} (untouched)`)
  }
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.8.0-phase4] rollback drops exactly the six new columns;')
    console.error('[migrate v1.8.0-phase4] all existing rows (incl. punch data) are untouched. Re-run with:')
    console.error('  node scripts/migrate-v1.8.0-phase4.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.8.0-phase4] down — database: ${dbUrl}`)
  for (const col of ADD_COLUMNS) {
    const cols = await columnsOf(col.table).catch(() => null)
    if (!cols || !cols.includes(col.name)) {
      console.log(`[migrate v1.8.0-phase4] ${col.table}.${col.name} already absent`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "${col.table}" DROP COLUMN "${col.name}"`)
    console.log(`[migrate v1.8.0-phase4] dropped ${col.table}.${col.name}`)
  }
  await status()
}

const cmd = process.argv[2] ?? 'up'
if (cmd === 'up') await up()
else if (cmd === 'status') await status()
else if (cmd === 'down') await down()
else {
  console.error(`[migrate v1.8.0-phase4] unknown command "${cmd}" — use up | status | down --confirm`)
  process.exitCode = 2
}

await db.$disconnect()
