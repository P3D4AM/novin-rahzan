#!/usr/bin/env node
/**
 * HozorPlus v1.7.0 (Phase 2 Stage S3) — migration: the three approved policies
 * get their storage columns.
 *
 * Adds:
 *   * LeaveType.isMission        BOOLEAN NOT NULL DEFAULT 0
 *     (mission_policy = approved_credit — a mission is a LeaveType flagged
 *     isMission; NEVER detected by Persian name matching in CODE, but this
 *     MIGRATION must flag the existing mission type created by earlier seeds,
 *     and that type is identified by its exact name 'مأموریت' — the only
 *     marker existing data has.)
 *   * DailyAttendance.payableOvertimeMinutes  INTEGER NOT NULL DEFAULT 0
 *     (overtime_policy = min_excess — payable = min(actual, approved);
 *     overtimeMinutes keeps its existing actual-OT meaning.)
 *   * DailyAttendance.missionMinutes          INTEGER NOT NULL DEFAULT 0
 *     (approved mission time credited as valid work time, attendance overlap
 *     subtracted).
 *
 * Rules honoured (same family as migrate-v1.4.0/v1.5.0):
 *   * INCREMENTAL — adds the three columns ONLY; no existing table/column/
 *     row/index is touched, nothing is ever dropped on `up`.
 *   * IDEMPOTENT  — safe to run any number of times (column-existence guards;
 *     the mission flag update is a no-op when already set).
 *   * REVERSIBLE (documented) — `down --confirm` drops exactly the three new
 *     columns. DailyAttendance rows lose the payable/mission minutes (they are
 *     derivable by a re-run of recalc) and LeaveType loses the mission flags
 *     (restored on the next `up`, which re-marks 'مأموریت'). Punch data
 *     (RawPunch/PunchRecord) is NEVER touched by either direction.
 *   * COMPATIBLE — `prisma db push` on the same DB converges with no drift
 *     (schema.prisma contains the same columns; verified by the test suite).
 *
 * Usage:
 *   node scripts/migrate-v1.7.0-phase2-s3.mjs            # up (default)
 *   node scripts/migrate-v1.7.0-phase2-s3.mjs status
 *   node scripts/migrate-v1.7.0-phase2-s3.mjs down --confirm
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
  { table: 'LeaveType', name: 'isMission', ddl: 'BOOLEAN NOT NULL DEFAULT 0' },
  { table: 'DailyAttendance', name: 'payableOvertimeMinutes', ddl: 'INTEGER NOT NULL DEFAULT 0' },
  { table: 'DailyAttendance', name: 'missionMinutes', ddl: 'INTEGER NOT NULL DEFAULT 0' },
]

/** The existing seed's mission type name — the ONLY marker old data has. */
const MISSION_TYPE_NAME = 'مأموریت'

async function columnsOf(table) {
  try {
    return (await db.$queryRawUnsafe(`PRAGMA table_info("${table}")`)).map((c) => c.name)
  } catch {
    return []
  }
}

async function up() {
  console.log(`[migrate v1.7.0-phase2-s3] up — database: ${dbUrl}`)
  for (const col of ADD_COLUMNS) {
    const cols = await columnsOf(col.table)
    if (cols.length === 0) throw new Error(`Table ${col.table} not found — run the app's db setup (or the earlier migrations) first`)
    if (cols.includes(col.name)) {
      console.log(`[migrate v1.7.0-phase2-s3] ${col.table}.${col.name} already present`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "${col.table}" ADD COLUMN "${col.name}" ${col.ddl}`)
    console.log(`[migrate v1.7.0-phase2-s3] added ${col.table}.${col.name}`)
  }

  // Mark the existing mission type (identified by its exact seed name — old
  // data has no other marker; new code NEVER matches by name).
  const flagged = await db.$executeRawUnsafe(
    `UPDATE "LeaveType" SET "isMission" = 1 WHERE "name" = ${JSON.stringify(MISSION_TYPE_NAME)} AND "isMission" = 0`,
  )
  console.log(`[migrate v1.7.0-phase2-s3] existing LeaveType '${MISSION_TYPE_NAME}' rows flagged isMission: ${flagged}`)
  const missions = await db.$queryRawUnsafe(`SELECT COUNT(*) AS n FROM "LeaveType" WHERE "isMission" = 1`)
  console.log(`[migrate v1.7.0-phase2-s3] LeaveTypes flagged as mission: ${Number(missions[0]?.n ?? 0)}`)
  await status()
}

async function down() {
  if (process.argv[3] !== '--confirm') {
    console.error('[migrate v1.7.0-phase2-s3] rollback drops the three new columns (payable/mission minutes are')
    console.error('re-derivable by a recalc run; the isMission flags are re-set by the next up).')
    console.error('[migrate v1.7.0-phase2-s3] raw punch data is untouched in either direction.')
    console.error('[migrate v1.7.0-phase2-s3] re-run with:  node scripts/migrate-v1.7.0-phase2-s3.mjs down --confirm')
    process.exitCode = 2
    return
  }
  console.log(`[migrate v1.7.0-phase2-s3] down — database: ${dbUrl}`)
  for (const col of ADD_COLUMNS) {
    const cols = await columnsOf(col.table)
    if (!cols.includes(col.name)) {
      console.log(`[migrate v1.7.0-phase2-s3] ${col.table}.${col.name} already absent`)
      continue
    }
    await db.$executeRawUnsafe(`ALTER TABLE "${col.table}" DROP COLUMN "${col.name}"`)
    console.log(`[migrate v1.7.0-phase2-s3] dropped ${col.table}.${col.name}`)
  }
  await status()
}

async function status() {
  for (const col of ADD_COLUMNS) {
    const cols = await columnsOf(col.table)
    console.log(`[migrate v1.7.0-phase2-s3] status: ${col.table}.${col.name} = ${cols.includes(col.name) ? 'present' : 'absent'}`)
  }
  try {
    const rows = await db.$queryRawUnsafe(`SELECT COUNT(*) AS n FROM "LeaveType" WHERE "isMission" = 1`)
    console.log(`[migrate v1.7.0-phase2-s3] status: mission LeaveTypes flagged = ${Number(rows[0]?.n ?? 0)}`)
  } catch {
    console.log('[migrate v1.7.0-phase2-s3] status: LeaveType.isMission not queryable (column absent)')
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
