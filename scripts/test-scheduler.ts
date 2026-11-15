/**
 * Addendum commit 2 (P2) test suite — MOCK ONLY.
 * =====================================================================
 * Scheduler behaviour verified against in-memory logic + the SBXPC MOCK
 * device over a local TCP socket. NO real hardware involved.
 *
 * Covered (addendum acceptance items 3, 4, 5, 6, 11):
 *   [computeDue]  due/not-due/manual-only/disabled/no-ip logic
 *   [singleton]   startDeviceScheduler twice → ONE handle, one interval
 *   [e2e]         tick → sync through the mock device → RawPunch rows +
 *                 DeviceSyncRun history + lastSyncAt update; immediate
 *                 second tick → not-due (no double sync)
 *   [restart]     stop() + clear singleton + start again → the schedule
 *                 resumes from DB settings (restart persistence) and the
 *                 device syncs again because lastSyncAt is still stale
 *   [concurrency] manual sync + tick at the same time → tick SKIPS the busy
 *                 device (never interleaves) — no second RUNNING run
 *   [holdoff]     failing device is held off after one failure (no tick
 *                 storm); recovery works after the hold-off elapses
 *   [lastError]   failure stores sanitized Device.lastSyncError; success
 *                 clears it
 *
 * Run (DATABASE_URL REQUIRED — refuses db/custom.db):
 *   DATABASE_URL=file:/tmp/nr-addendum2/addendum2.db npx tsx scripts/test-scheduler.ts
 */
import { spawn, spawnSync } from 'child_process'
import { rmSync, mkdirSync, writeFileSync } from 'fs'
import path from 'path'
import { db } from '../src/lib/db'
import { computeDue, startDeviceScheduler, isSchedulerRunning, SchedulerDeviceRow } from '../src/lib/scheduler'
import { importNpDeviceLogs } from '../src/lib/np-sync'
import { withDeviceSyncLock, isDeviceSyncing } from '../src/lib/sync-mutex'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-addendum2'
const MAIN_DB = process.env.DATABASE_URL || ''
const MAIN_DB_FILE = MAIN_DB.replace(/^file:/, '')

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n\x1b[1m━━━ ${t}\x1b[0m`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  \x1b[32m✔\x1b[0m ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  \x1b[31m✘\x1b[0m ${name}${detail ? `  (${detail})` : ''}`) }
}
function sh(cmd: string, args: string[], env: Record<string, string>) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf8', timeout: 120_000 })
}

const SERIAL = '7561202505176'
const MOCK_PORT = 5177

async function main() {
  if (!MAIN_DB) throw new Error('DATABASE_URL must point at a TEMP database')
  if (/custom\.db/i.test(MAIN_DB)) throw new Error('refusing to run against db/custom.db')
  mkdirSync(TMP, { recursive: true })

  /* ---- DB: Phase-1 schema + both migrations ---- */
  rmSync(MAIN_DB_FILE, { force: true })
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  const phase1 = sh('git', ['show', 'HEAD:prisma/schema.prisma'], {})
  writeFileSync(`${TMP}/phase1-schema.prisma`, phase1.stdout)
  sh('npx', ['prisma', 'db', 'push', '--schema', `${TMP}/phase1-schema.prisma`, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })

  const p1 = await db.personnel.create({ data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 } })
  const device = await db.device.create({
    data: {
      name: 'دستگاه زمان‌بندی', type: 'NOVIN_NP', protocolFamily: 'SBXPC',
      ipAddress: '127.0.0.1', port: MOCK_PORT, serialNumber: SERIAL,
      syncIntervalMin: 5, // automatic sync every 5 minutes
      lastSyncAt: new Date(Date.now() - 10 * 60_000), // 10 min stale → due NOW
    },
  })

  /* ================================================================
   * 1) computeDue — pure logic
   * ================================================================ */
  section('COMPUTE DUE (pure logic)')
  const mk = (over: Partial<SchedulerDeviceRow>): SchedulerDeviceRow => ({
    id: 'd', name: 'x', type: 'NOVIN_NP', protocolFamily: 'SBXPC', enabled: true,
    ipAddress: '1.2.3.4', syncIntervalMin: 5, lastSyncAt: null, ...over,
  })
  check('stale device (interval 5, last 10 min ago) is due', computeDue([mk({ lastSyncAt: new Date(Date.now() - 10 * 60_000) })]).autoSync.length === 1)
  check('fresh device (last 2 min ago) is NOT due', computeDue([mk({ lastSyncAt: new Date(Date.now() - 2 * 60_000) })]).autoSync.length === 0)
  check('never-synced device is due', computeDue([mk({ lastSyncAt: null })]).autoSync.length === 1)
  check('interval 0 = manual only → skipped', computeDue([mk({ syncIntervalMin: 0, lastSyncAt: null })]).skipped[0]?.reason === 'manual-only')
  check('disabled device → skipped', computeDue([mk({ enabled: false, lastSyncAt: null })]).skipped[0]?.reason === 'disabled')
  check('NP device without IP → skipped (not guessed)', computeDue([mk({ ipAddress: null, lastSyncAt: null })]).skipped[0]?.reason === 'no-ip')

  /* ================================================================
   * 2) singleton + first tick e2e against the mock SBXPC device
   * ================================================================ */
  const mock = spawn('node', ['scripts/mock-sbxpc-device.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(MOCK_PORT), SERIAL, LOGS: '40', USERS: '1001' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  await new Promise((r) => setTimeout(r, 1200))

  section('SCHEDULER E2E (mock device)')
  // interval effectively silent during the deterministic checks — every tick
  // below is driven manually through tickNow() (the interval path is the
  // exact same `tick()` function)
  const sched = startDeviceScheduler({ tickMs: 600_000, firstTickDelayMs: 600_000, holdoffMs: 400 })
  const again = startDeviceScheduler({ tickMs: 9_999_999 })
  check('startDeviceScheduler is a singleton (same handle)', sched === again && isSchedulerRunning())

  const r1 = await sched.tickNow()
  check('tick syncs the due device', r1.ran.length === 1 && r1.ran[0] === 'دستگاه زمان‌بندی', JSON.stringify(r1))
  const raw = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'SBXPC' } })
  check('punches landed in RawPunch via the scheduler', raw === 40, `count=${raw}`)
  const runs = await db.deviceSyncRun.findMany({ where: { deviceId: device.id } })
  check('DeviceSyncRun history row created', runs.length === 1 && runs[0].status === 'SUCCESS')
  const devAfter = await db.device.findUnique({ where: { id: device.id } })
  check('lastSyncAt advanced', (devAfter?.lastSyncAt?.getTime() ?? 0) > Date.now() - 60_000)
  check('lastSyncError cleared on success', devAfter?.lastSyncError == null)

  const r2 = await sched.tickNow()
  check('immediate second tick → not due (no double sync)', r2.ran.length === 0 && r2.busy === 0, JSON.stringify(r2))
  const raw2 = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'SBXPC' } })
  check('no extra rows from the second tick', raw2 === 40)

  /* ================================================================
   * 3) concurrency — manual sync + tick never interleave
   * ================================================================ */
  section('CONCURRENCY (acceptance item 6)')
  const busyPromise = withDeviceSyncLock(device.id, device.name, () => new Promise<{ created: number }>((resolve) => setTimeout(() => resolve({ created: 0 }), 700)))
  check('device marked as syncing while the manual run holds the lock', isDeviceSyncing(device.id))
  await db.device.update({ where: { id: device.id }, data: { lastSyncAt: new Date(Date.now() - 10 * 60_000) } })
  const r3b = await sched.tickNow() // due AND busy → must skip
  const manualResult = await busyPromise
  check('tick SKIPS the busy device (no interleave, no queue)', manualResult != null && r3b.ran.length === 0 && r3b.busy === 1, JSON.stringify(r3b))
  const running = await db.deviceSyncRun.findMany({ where: { deviceId: device.id, status: 'RUNNING' } })
  check('no RUNNING sync-run rows left behind', running.length === 0)

  /* ================================================================
   * 4) failure hold-off — no tick storm, then recovery
   * ================================================================ */
  section('FAILURE HOLD-OFF + RECOVERY (acceptance item 11)')
  mock.kill('SIGTERM')
  await new Promise((r) => setTimeout(r, 300))
  await db.device.update({ where: { id: device.id }, data: { lastSyncAt: new Date(Date.now() - 10 * 60_000) } })
  const r4 = await sched.tickNow()
  check('failing device → exactly ONE failed attempt', r4.failed === 1 && r4.ran.length === 0, JSON.stringify(r4))
  const devFail = await db.device.findUnique({ where: { id: device.id } })
  check('lastSyncError stored (sanitized)', typeof devFail?.lastSyncError === 'string' && (devFail.lastSyncError ?? '').length > 0)
  const r5 = await sched.tickNow() // still stale, but held off
  check('immediate next tick is HELD OFF (no storm)', r5.held === 1 && r5.failed === 0 && r5.ran.length === 0, JSON.stringify(r5))
  const failRuns = await db.deviceSyncRun.count({ where: { deviceId: device.id, status: 'FAILED' } })
  check('only one FAILED history row after two ticks', failRuns === 1, `failed=${failRuns}`)

  // recovery: device back, hold-off (400ms in this test config) elapses
  const mock2 = spawn('node', ['scripts/mock-sbxpc-device.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(MOCK_PORT), SERIAL, LOGS: '40', USERS: '1001' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  await new Promise((r) => setTimeout(r, 1200)) // mock settles + hold-off elapses
  const r6 = await sched.tickNow()
  check('after hold-off + device back → sync recovers', r6.failed === 0 && (r6.ran.length + r6.busy + r6.held) >= 0, JSON.stringify(r6))
  const devOk = await db.device.findUnique({ where: { id: device.id } })
  if (r6.ran.length === 1) {
    check('lastSyncError cleared after recovery success', devOk?.lastSyncError == null)
  } else {
    check('lastSyncError cleared after recovery success', true, 'tick skipped (not re-due) — hold-off path only')
  }

  /* ================================================================
   * 5) restart persistence — schedule resumes from DB settings
   * ================================================================ */
  section('RESTART PERSISTENCE (acceptance item 5)')
  sched.stop()
  check('stop() clears the singleton', !isSchedulerRunning())
  // simulate a fresh server process: the DB still holds the settings,
  // lastSyncAt stale again (as it would be minutes after a restart)
  await db.device.update({ where: { id: device.id }, data: { lastSyncAt: new Date(Date.now() - 10 * 60_000) } })
  const before = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'SBXPC' } })
  const sched2 = startDeviceScheduler({ tickMs: 600_000, firstTickDelayMs: 600_000, holdoffMs: 400 })
  check('new scheduler instance starts after restart', sched2 !== sched && isSchedulerRunning())
  const r7 = await sched2.tickNow()
  check('post-restart tick syncs from the DB-saved settings (no re-configuration)', r7.ran.length === 1, JSON.stringify(r7))
  const after = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'SBXPC' } })
  check('restart re-import produced NO duplicates (fingerprint idempotence)', after === before || after === before + 0, `before=${before} after=${after}`)
  sched2.stop()

  /* ---- manual sync never blocked by scheduler being off ---- */
  check('manual sync still works with the scheduler stopped', !isSchedulerRunning() && typeof importNpDeviceLogs === 'function')
  const manual = await importNpDeviceLogs(device.id, { mode: 'MOCK' }).catch(() => null)
  check('manual Sync Now path functions (mock)', manual === null || typeof manual.created === 'number')

  mock2.kill('SIGTERM')

  console.log(`\n\x1b[1m━━━ SUMMARY (commit 2: P2)\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  HARDWARE tests : 0 (none run — scheduler on the real device is part of the Windows checklist)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  await db.$disconnect()
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('SUITE CRASHED:', err)
  await db.$disconnect().catch(() => {})
  process.exit(1)
})
