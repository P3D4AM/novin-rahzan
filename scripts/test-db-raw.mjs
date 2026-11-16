/**
 * v1.3.1 test helper — run raw SQL statements against DATABASE_URL.
 * Used by the migration data-preservation test to seed rows on a PRE-1.3.0
 * schema database (before the migration runs) and to verify them afterwards.
 *
 * Usage:
 *   DATABASE_URL=file:… node scripts/test-db-raw.mjs "INSERT …" "INSERT …"
 *   DATABASE_URL=file:… node scripts/test-db-raw.mjs --json "SELECT …"   → prints RAW_JSON <json>
 */
import { createRequire } from 'module'

const require = createRequire(import.meta.url)
const { PrismaClient } = require('@prisma/client')

async function main() {
  if (!process.env.DATABASE_URL) {
    console.error('DATABASE_URL is required')
    process.exit(2)
  }
  const args = process.argv.slice(2)
  const jsonMode = args[0] === '--json'
  const stmts = jsonMode ? args.slice(1) : args
  if (!stmts.length) {
    console.error('no statements given')
    process.exit(2)
  }
  const db = new PrismaClient()
  try {
    for (const s of stmts) {
      if (jsonMode) {
        const rows = await db.$queryRawUnsafe(s)
        // SQLite counts come back as BigInt — make them JSON-safe
        const safe = JSON.parse(JSON.stringify(rows, (_k, v) => (typeof v === 'bigint' ? Number(v) : v)))
        console.log('RAW_JSON ' + JSON.stringify(safe))
      } else {
        await db.$executeRawUnsafe(s)
      }
    }
    console.log('RAW_OK')
  } catch (err) {
    console.error('RAW_FAILED: ' + err.message)
    process.exit(1)
  } finally {
    await db.$disconnect()
  }
}

main()
