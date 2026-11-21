/* Seed: Persian demo data for حضور+ attendance system.
 * Phase 1 (v1.8.0-dev): thin wrapper — the demo DATA now lives in
 * src/lib/demo-seed.ts (shared with the installer and the settings UI
 * "Database management" section, with preserve-on-conflict semantics).
 * This script keeps the historical `npm run seed` entry point.
 */
import { PrismaClient } from '@prisma/client'
import { dbDemoSeed } from '../src/lib/db-admin'

const db = new PrismaClient()

async function main() {
  console.log('🧺 seeding demo data (users/devices/settings of THIS install are preserved) …')
  const r = await dbDemoSeed(db)
  console.log(`✅ seed done: ${r.seeded.persons} persons, ${r.seeded.punches} punches`)
  if (r.backup) console.log(`🛟 backup before seed: ${r.backup.path}`)
}

main()
  .catch((e) => {
    console.error(e)
    process.exit(1)
  })
  .finally(() => db.$disconnect())
