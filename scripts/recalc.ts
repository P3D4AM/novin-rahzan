/* Recalculate DailyAttendance for all personnel over the last N days (default 35) */
import { PrismaClient } from '@prisma/client'
import { addDays } from '../src/lib/jalali'

const db = new PrismaClient()

async function main() {
  const days = Number(process.argv[2] || 35)
  const today = new Date()
  const start = addDays(new Date(Date.UTC(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate())), -days)
  const end = addDays(new Date(Date.UTC(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate())), 1)
  const persons = await db.personnel.findMany({ select: { id: true } })
  const { recalcRange } = await import('../src/lib/attendance')
  const count = await recalcRange(persons.map((p) => p.id), start, end)
  console.log(`✅ recalculated ${count} person-days for ${persons.length} persons (${days} days)`)
}

main().catch((e) => { console.error(e); process.exit(1) }).finally(() => db.$disconnect())
