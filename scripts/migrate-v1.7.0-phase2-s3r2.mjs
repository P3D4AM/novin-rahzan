#!/usr/bin/env node
/**
 * HozorPlus v1.7.0 (Phase 2 Stage S3 review fixes) — migration: dated
 * default-shift history.
 *
 * Adds:
 *   * TABLE DefaultShiftHistory (personId, effectiveFrom, shiftId, note,
 *     createdAt) + index (personId, effectiveFrom) — the DATED history of the
 *     personnel DEFAULT shift (Phase 2 S3 review fix 1: changing today's
 *     default must never rewrite historical recalculation; the engine now
 *     resolves the default AS OF each day from these rows).
 *   * ONE SNAPSHOT ROW per person that currently has a defaultShiftId but no
 *     history rows yet — effectiveFrom = TODAY (Tehran day), shiftId = the
 *     current default. This row is a FACT recorded at migration time ("as of
 *     migration day, the default was X"); NO back-dated rows are invented.
 *     Days BEFORE the earliest entry resolve to the earliest entry's shift
 *     (the documented backward-extension rule — docs/TIME-POLICIES.md), which
 *     reproduces the pre-upgrade behavior exactly, so legacy rows never get
 *     rewritten by the upgrade itself.
 *
 * Rules honoured (same family as migrate-v1.7.0-phase2-s3.mjs):
 *   * INCREMENTAL — adds the table and snapshot rows ONLY; no existing
 *     table/column/row/index is touched, nothing is ever dropped on `up`.
 *   * IDEMPOTENT — safe to run any number of times (table-existence guard;
 *     the snapshot insert only targets persons with defaultShiftId NOT NULL
 *     and ZERO history rows, so a re-run is a no-op).
 *   * REVERSIBLE (documented) — `down --confirm` drops exactly the new table.
 *     The dated history is LOST (re-created as a fresh snapshot by the next
 *     `up`); Personnel.defaultShiftId itself is never touched in either
 *     direction. Punch data (RawPunch/PunchRecord) is NEVER touched.
 *   * COMPATIBLE — `prisma db push` on the same DB converges with no drift
 *     (schema.prisma contains the same table; verified by the test suite).
 *   * The previous S3 migration (migrate-v1.7.0-phase2-s3.mjs) and its
 *     columns/data are untouched; this migration can run before or after it.
 *
 * Usage:
 *   node scripts/migrate-v1.7.0-phase2-s3r2.mjs            # up (default)
 *   node scripts/migrate-v1.7.0-phase2-s3r2.mjs status
 *   node scripts/migrate-v1.7.0-phase2-s3r2.mjs down --confirm
 */
import { createRequire } from 'module'
import { existsSync, readFileSync } from 'fs'
import { fileURLToPath } from 'url'
import path from 'path'
import { randomUUID } from 'crypto'

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

const CREATE_TABLE_DDL = `CREATE TABLE "DefaultShiftHistory" (
    "id" TEXT NOT NULL PRIMARY KEY,
    "personId" TEXT NOT NULL,
    "effectiveFrom" DATETIME NOT NULL,
    "shiftId" TEXT,
    "note" TEXT,
    "createdAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "DefaultShiftHistory_personId_fkey" FOREIGN KEY ("personId") REFERENCES "Personnel" ("id") ON DELETE CASCADE ON UPDATE CASCADE
)`
const CREATE_INDEX_DDL = `CREATE INDEX "DefaultShiftHistory_personId_effectiveFrom_idx" ON "DefaultShiftHistory"("personId", "effectiveFrom")`
const SNAPSHOT_NOTE = 'v1.7.0-s3r2 migration snapshot'

/** UTC-midnight marker of TODAY's Tehran calendar day (matches src/lib/time.ts dayMarkerOf). */
function tehranTodayMarker() {
  // en-CA numeric format is YYYY-MM-DD (unambiguous, unlike en-US MM/DD/YYYY)
  const iso = new Intl.DateTimeFormat('en-CA', {
    timeZone: 'Asia/Tehran', year: 'numeric', month: '2-digit', day: '2-digit',
  }).format(new Date())
  const [y, m, d] = iso.split('-').map(Number)
  return new Date(Date.UTC(y, m - 1, d))
}

async function tableExists() {
  const rows = await db.$queryRawUnsafe(
    `SELECT COUNT(*) AS n FROM "sqlite_master" WHERE "type" = 'table' AND "name" = 'DefaultShiftHistory'`,
  )
  return Number(rows[0]?.n ?? 0) > 0
}

async function up() {
  console.log(`[migrate v1.7.0-s3r2] up — database: ${dbUrl}`)

  // Personnel table must exist (app db already initialized)
  const personnel = await db.$queryRawUnsafe(
    `SELECT COUNT(*) AS n FROM "sqlite_master" WHERE "type" = 'table' AND "name" = 'Personnel'`,
  )
  if (Number(personnel[0]?.n ?? 0) === 0) {
    throw new Error('Table Personnel not found — run the app\'s db setup (or the earlier migrations) first')
  }

  if (await tableExists()) {
    console.log('[migrate v1.7.0-s3r2] table DefaultShiftHistory already present')
  } else {
    await db.$executeRawUnsafe(CREATE_TABLE_DDL)
    console.log('[migrate v1.7.0-s3r2] created table DefaultShiftHistory')
    await db.$executeRawUnsafe(CREATE_INDEX_DDL)
    console.log('[migrate v1.7.0-s3r2] created index DefaultShiftHistory_personId_effectiveFrom_idx')
  }

  // Snapshot: one dated row per person that HAS a default today but NO history
  // rows (a fact recorded now — never a fabricated past-dated row). Persons
  // that already have history (created after the fix, re-run, etc.) are left
  // untouched. Persons with defaultShiftId NULL need nothing (their "no
  // default" state is already the resolution's null).
  const candidates = await db.$queryRawUnsafe(
    `SELECT "id" FROM "Personnel" WHERE "defaultShiftId" IS NOT NULL AND "id" NOT IN (SELECT "personId" FROM "DefaultShiftHistory")`,
  )
  const today = tehranTodayMarker()
  for (const row of candidates) {
    await db.$executeRawUnsafe(
      `INSERT INTO "DefaultShiftHistory" ("id","personId","effectiveFrom","shiftId","note") VALUES (${JSON.stringify(randomUUID())}, ${JSON.stringify(row.id)}, ${JSON.stringify(today.toISOString())}, (SELECT "defaultShiftId" FROM "Personnel" WHERE "id" = ${JSON.stringify(row.id)}), ${JSON.stringify(SNAPSHOT_NOTE)})`,
    )
  }
  console.log(`[migrate v1.7.0-s3r2] snapshot rows written (persons with a default, no history yet): ${candidates.length}`)
  await status()
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.7.0-s3r2] rollback drops the DefaultShiftHistory table (the dated history is')
    console.error('[migrate v1.7.0-s3r2] re-created as a fresh snapshot by the next up; Personnel.defaultShiftId is untouched).')
    console.error('[migrate v1.7.0-s3r2] punch data is untouched in either direction.')
    console.error('[migrate v1.7.0-s3r2] re-run with:  node scripts/migrate-v1.7.0-phase2-s3r2.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.7.0-s3r2] down — database: ${dbUrl}`)
  if (!(await tableExists())) {
    console.log('[migrate v1.7.0-s3r2] table DefaultShiftHistory already absent')
  } else {
    await db.$executeRawUnsafe(`DROP TABLE "DefaultShiftHistory"`)
    console.log('[migrate v1.7.0-s3r2] dropped table DefaultShiftHistory (cascade: index)')
  }
  await status()
}

async function status() {
  console.log(`[migrate v1.7.0-s3r2] status: DefaultShiftHistory = ${(await tableExists()) ? 'present' : 'absent'}`)
  try {
    const rows = await db.$queryRawUnsafe(`SELECT COUNT(*) AS n FROM "DefaultShiftHistory"`)
    console.log(`[migrate v1.7.0-s3r2] status: history rows = ${Number(rows[0]?.n ?? 0)}`)
  } catch {
    console.log('[migrate v1.7.0-s3r2] status: DefaultShiftHistory not queryable (table absent)')
  }
}

const cmd = process.argv[2] || 'up'
if (cmd === 'up') await up()
else if (cmd === 'down') await down()
else if (cmd === 'status') await status()
else {
  console.error(`unknown command: ${cmd} (use up | status | down --confirm)`)
  process.exitCode = 2
}

await db.$disconnect()
