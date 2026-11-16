#!/usr/bin/env node
/**
 * HozorPlus v1.5.0 — migration: the FKAttend bridge becomes INTERNAL.
 *
 * Drops Device.bridgeUrl / Device.bridgeToken — the two user-managed bridge
 * settings from v1.4.0. The bridge is now started/restarted/stopped by the app
 * itself (src/lib/bridge-supervisor.ts) with a per-launch memory-only token;
 * there is nothing for the user to configure, paste or keep running anymore
 * (docs/BRIDGE-SUPERVISION.md).
 *
 * Rules honoured (same family as migrate-v1.4.0-phase2.mjs):
 *   * INCREMENTAL — removes ONLY the two obsolete bridge columns; every other
 *     table/column/row/index is untouched. Punch data (RawPunch/DeviceSyncRun/
 *     RejectedRecord) is NOT affected in any way.
 *   * IDEMPOTENT  — safe to run any number of times (column-existence guards).
 *   * HONEST ROLLBACK — `down --confirm` re-adds the two columns EMPTY: the
 *     dropped values are obsolete internal pointers (a supervised bridge no
 *     longer reads them), so they are NOT restored — this is documented, not
 *     hidden. Raw data is never touched by either direction.
 *   * COMPATIBLE — `prisma db push` on the same DB converges with no drift
 *     (schema.prisma no longer contains the columns; verified by tests).
 *
 * Usage:
 *   node scripts/migrate-v1.5.0-bridge-internal.mjs            # up (default)
 *   node scripts/migrate-v1.5.0-bridge-internal.mjs status
 *   node scripts/migrate-v1.5.0-bridge-internal.mjs down --confirm
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

const DROP_COLUMNS = ['bridgeUrl', 'bridgeToken']

async function columnsOf(table) {
  try {
    return (await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)).map((c) => c.name)
  } catch {
    return []
  }
}

async function up() {
  console.log(`[migrate v1.5.0-bridge-internal] up — database: ${dbUrl}`)
  const existing = await columnsOf('Device')
  if (existing.length === 0) throw new Error('Device table not found — run the earlier migrations first')
  for (const name of DROP_COLUMNS) {
    if (!existing.includes(name)) {
      console.log(`[migrate v1.5.0-bridge-internal] Device.${name} already absent`)
      continue
    }
    // honest record of what is being removed before it goes
    try {
      const rows = await db.$queryRawUnsafe(
        `SELECT COUNT(*) AS total, SUM(CASE WHEN "${name}" IS NOT NULL AND "${name}" <> '' THEN 1 ELSE 0 END) AS nonEmpty FROM "Device"`,
      )
      const total = Number(rows[0]?.total ?? 0)
      const nonEmpty = Number(rows[0]?.nonEmpty ?? 0)
      console.log(`[migrate v1.5.0-bridge-internal] dropping Device.${name} (devices: ${total}, with a stored value: ${nonEmpty} — obsolete internal setting, not restorable by design)`)
    } catch { /* count is informational only */ }
    await db.$executeRawUnsafe(`ALTER TABLE "Device" DROP COLUMN "${name}"`)
    console.log(`[migrate v1.5.0-bridge-internal] dropped Device.${name}`)
  }
  await status()
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.5.0-bridge-internal] rollback re-adds the two bridge columns EMPTY (the dropped values were')
    console.error('obsolete internal pointers — a supervised bridge never reads them; raw punch data is untouched).')
    console.error('[migrate v1.5.0-bridge-internal] re-run with:  node scripts/migrate-v1.5.0-bridge-internal.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.5.0-bridge-internal] down — database: ${dbUrl}`)
  const cols = await columnsOf('Device')
  const ddl = { bridgeUrl: `TEXT`, bridgeToken: `TEXT` }
  for (const name of DROP_COLUMNS) {
    if (cols.includes(name)) {
      console.log(`[migrate v1.5.0-bridge-internal] Device.${name} already present`)
      continue
    }
    try {
      await db.$executeRawUnsafe(`ALTER TABLE "Device" ADD COLUMN "${name}" ${ddl[name]}`)
      console.log(`[migrate v1.5.0-bridge-internal] re-added Device.${name} (empty — values not restorable by design)`)
    } catch (err) {
      console.warn(`[migrate v1.5.0-bridge-internal] could not re-add Device.${name} (SQLite < 3.35): ${err.message}`)
    }
  }
  await status()
}

async function status() {
  const cols = await columnsOf('Device')
  for (const name of DROP_COLUMNS) {
    console.log(`[migrate v1.5.0-bridge-internal] Device.${name}: ${cols.includes(name) ? 'present (pre-v1.5.0)' : 'absent (v1.5.0 internal bridge)'}`)
  }
}

async function main() {
  const cmd = process.argv[2] || 'up'
  try {
    if (cmd === 'up') await up()
    else if (cmd === 'down') await down()
    else if (cmd === 'status') {
      console.log(`[migrate v1.5.0-bridge-internal] status — database: ${dbUrl}`)
      await status()
    } else {
      console.error('usage: migrate-v1.5.0-bridge-internal.mjs [up|down --confirm|status]')
      process.exitCode = 2
    }
  } finally {
    await db.$disconnect()
  }
}

main().catch((err) => {
  console.error('[migrate v1.5.0-bridge-internal] FAILED:', err.message)
  process.exit(1)
})
