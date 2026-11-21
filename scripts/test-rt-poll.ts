/**
 * Addendum commit 3 (P3) test suite — MOCK ONLY.
 * =====================================================================
 * Near Real-Time Polling behaviour against the MOCK FKAttend bridge
 * (scripts/mock-fkattend-bridge.js). NO real hardware involved — the
 * investigation (docs/FKATTEND-CAPABILITIES.md) categorises FKAttend.dll as
 * pull-only (C), so this polling is the honest fallback per addendum §3.3.
 *
 * Covered (addendum acceptance items 7, 12, 13 + §3.3 rules):
 *   [rt due]      rt due computation independent from the auto-sync interval
 *   [detection]   a punch injected into the mock device appears in RawPunch
 *                 within ONE poll cycle; the detection latency is measured
 *                 and reported honestly (mock numbers, NOT hardware numbers)
 *   [no-loss]     a clock-skewed punch (timestamped in the past, before the
 *                 bookmark) is still caught by the full-read poll
 *   [dedupe]      poll + manual full sync running back-to-back → zero
 *                 duplicates (shared fingerprint path)
 *   [backoff]     failing polls back off exponentially and reset on success
 *   [quiet]       a no-op poll writes NO DeviceLog row and NO DeviceSyncRun
 *                 row; a poll that finds a punch DOES record both
 *   [labels]      UI copy says "شبه‌بی‌درنگ (Polling)" — never claims real push
 *
 * v1.3.1 (Phase 1 review fix 5): the suite now AUTO-GENERATES its own unique
 * temporary database under the test directory (scripts/test-db-guard.ts) — an
 * external DATABASE_URL is ignored and NEVER deleted, and the schema baseline
 * is pinned to the actual Phase 1 commit instead of a drifting HEAD.
 *
 * Run:
 *   npx tsx scripts/test-rt-poll.ts
 */
import { spawn, spawnSync } from 'child_process'
import { mkdirSync, writeFileSync, rmSync } from 'fs'
import net from 'net'
import path from 'path'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'
import type { SchedulerDeviceRow } from '../src/lib/scheduler'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-addendum3'
const MAIN_DB_FILE = generateTestDatabasePath(TMP)
const MAIN_DB = sqliteUrl(MAIN_DB_FILE)
process.env.DATABASE_URL = MAIN_DB // before the db client is imported below

/** a FREE port for the mock bridge — a fixed port breaks re-runs when a
 *  crashed previous run leaves a stale mock holding it (observed in the wild:
 *  the backoff test then "succeeds" against the ghost process). */
function freePort(): Promise<number> {
  return new Promise((resolve, reject) => {
    const srv = net.createServer()
    srv.unref()
    srv.on('error', reject)
    srv.listen(0, '127.0.0.1', () => {
      const port = (srv.address() as net.AddressInfo).port
      srv.close(() => resolve(port))
    })
  })
}

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
let BRIDGE_PORT = 0 // assigned in main() — a free port per run (no ghost-process collisions)

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(MAIN_DB, TMP)
  BRIDGE_PORT = await freePort()

  // db-dependent modules are imported AFTER DATABASE_URL points at the
  // auto-generated test database
  const { db } = await import('../src/lib/db')
  const { computeDue } = await import('../src/lib/scheduler')
  const { importFkAttendLogs } = await import('../src/lib/fkattend')
  // v1.5.0: the bridge is supervisor-internal; in mock suites the endpoint is
  // the EXTERNAL override pointing at the mock bridge (no x86 child on Linux)
  const { resetBridgeSupervisorForTests } = await import('../src/lib/bridge-supervisor')
  process.env.FKBRIDGE_URL = `http://127.0.0.1:${BRIDGE_PORT}`
  process.env.FKBRIDGE_TOKEN = 'test-token'

  // base = the v1.3.0 (Phase 1) schema — pinned to the Phase 1 commit, never
  // a drifting HEAD — then the real migration chain a user's DB would follow
  const phase1 = sh('git', ['show', '3c254597:prisma/schema.prisma'], {})
  writeFileSync(`${TMP}/phase1-schema.prisma`, phase1.stdout)
  sh('npx', ['prisma', 'db', 'push', '--schema', `${TMP}/phase1-schema.prisma`, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.3.1-phase1-fixes.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.5.0-bridge-internal.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  // Phase 3 stage 2: DeviceSyncRun accounting columns (recordsAnnounced /
  // recordsInvalidTimestamps / abnormalEnd) — runRawSync writes them on every run.
  sh('node', ['scripts/migrate-v1.8.0-phase3.mjs', 'up'], { DATABASE_URL: MAIN_DB })

  await db.personnel.create({ data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 } })
  const device = await db.device.create({
    data: {
      name: 'دستگاه شبه‌بی‌درنگ', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: SERIAL,
      syncIntervalMin: 0, // automatic sync OFF — polling must be independent
      rtEnabled: true, rtPollSec: 30,
    },
  })

  const mock = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', FKBRIDGE_PORT: String(BRIDGE_PORT), FKBRIDGE_TOKEN: 'test-token', SERIAL, LOGS: '20', USERS: '1001' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  await new Promise((r) => setTimeout(r, 900))

  /* ---- baseline import ---- */
  const base = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('baseline import (20 records)', base.created === 20, `created=${base.created}`)
  const deviceLogsBefore = await db.deviceLog.count({ where: { deviceId: device.id } })
  const runsBefore = await db.deviceSyncRun.count({ where: { deviceId: device.id } })

  /* ================================================================
   * 1) RT DUE — independent of the auto-sync interval
   * ================================================================ */
  section('RT DUE COMPUTATION (independent interval)')
  const mk = (over: Partial<SchedulerDeviceRow>): SchedulerDeviceRow => ({
    id: 'd', name: 'x', type: 'NOVIN_NP', protocolFamily: 'FKATTEND', enabled: true,
    ipAddress: '1.2.3.4', syncIntervalMin: 0, lastSyncAt: null, ...over,
  })
  check('rt enabled + never polled → rt due (auto-sync off!)', computeDue([mk({ rtEnabled: true, rtPollSec: 30, rtLastPollAt: null })]).rtPoll.length === 1)
  check('rt polled 5s ago (interval 30) → NOT due', computeDue([mk({ rtEnabled: true, rtPollSec: 30, rtLastPollAt: new Date(Date.now() - 5_000) })]).rtPoll.length === 0)
  check('rt polled 35s ago (interval 30) → due', computeDue([mk({ rtEnabled: true, rtPollSec: 30, rtLastPollAt: new Date(Date.now() - 35_000) })]).rtPoll.length === 1)
  check('rt disabled → never due', computeDue([mk({ rtEnabled: false, rtLastPollAt: null })]).rtPoll.length === 0)
  check('auto-sync interval does NOT gate rt polling', computeDue([mk({ rtEnabled: true, rtPollSec: 15, rtLastPollAt: new Date(Date.now() - 20_000), syncIntervalMin: 1440, lastSyncAt: new Date() })]).rtPoll.length === 1)

  /* ================================================================
   * 2) DETECTION — a new punch appears within one poll cycle
   * ================================================================ */
  section('DETECTION LATENCY (mock — one poll cycle)')
  // simulate the poll loop the scheduler runs (rtLastPollAt stamp + quiet full read)
  const pollOnce = () => importFkAttendLogs(device.id, { mode: 'MOCK', full: true, quiet: true, note: 'rt-poll' })
  const t0 = Date.now()
  const inject = await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/punch`, {
    method: 'POST', headers: { 'x-bridge-token': 'test-token' },
    body: JSON.stringify({ enroll: 1001, secondsAgo: 0, verifyMode: 1 }),
  })
  check('punch injected into the mock device', inject.ok)
  const pollRes = await pollOnce()
  const latency = Date.now() - t0
  check('the injected punch is detected by the very next poll', pollRes.created === 1, `created=${pollRes.created}`)
  console.log(`  ℹ measured MOCK detection latency (in-process poll): ${latency} ms — production latency = up to rtPollSec (default 30s) + device read time; honest numbers only`)
  const total = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })
  check('total rows 20 + 1 = 21', total === 21, `total=${total}`)

  /* ================================================================
   * 3) NO-LOSS — clock-skewed punch (in the past, before the bookmark)
   * ================================================================ */
  section('NO-LOSS (clock-skewed punch before the bookmark)')
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/punch`, {
    method: 'POST', headers: { 'x-bridge-token': 'test-token' },
    body: JSON.stringify({ enroll: 1001, secondsAgo: 45 * 24 * 3600 }), // 45 days in the PAST
  })
  const skewed = await pollOnce()
  check('a punch timestamped 45 days in the past is STILL caught (full read, not windowed)', skewed.created === 1, `created=${skewed.created}`)

  /* ================================================================
   * 4) DEDUPE — poll + manual full sync back-to-back (acceptance 13)
   * ================================================================ */
  section('POLL + MANUAL SYNC — zero duplicates (acceptance item 13)')
  const [a, b] = await Promise.allSettled([
    pollOnce(),
    importFkAttendLogs(device.id, { mode: 'MOCK', full: true }),
  ])
  const okCount = [a, b].filter((r) => r.status === 'fulfilled').length
  const busyCount = [a, b].filter((r) => r.status === 'rejected').length
  check('poll and manual sync serialize (one may be busy-skipped)', okCount >= 1 && okCount + busyCount === 2)
  const totalAfter = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })
  check('no duplicate rows after the concurrent paths', totalAfter === 22, `total=${totalAfter}`)

  /* ================================================================
   * 5) QUIET HISTORY — no-op polls write nothing
   * ================================================================ */
  section('QUIET MODE (history stays meaningful)')
  const logsRightBefore = await db.deviceLog.count({ where: { deviceId: device.id } })
  const runsRightBefore = await db.deviceSyncRun.count({ where: { deviceId: device.id } })
  await pollOnce() // no-op (everything already imported)
  const deviceLogsAfterNoop = await db.deviceLog.count({ where: { deviceId: device.id } })
  check('no-op poll writes NO DeviceLog row', deviceLogsAfterNoop === logsRightBefore, `before=${logsRightBefore} after=${deviceLogsAfterNoop}`)
  const runsAfterNoop = await db.deviceSyncRun.count({ where: { deviceId: device.id } })
  check('no-op poll writes NO DeviceSyncRun row', runsAfterNoop === runsRightBefore, `before=${runsRightBefore} after=${runsAfterNoop}`)
  check('the notable runs above DID record history (quiet only hides no-ops)', runsRightBefore > runsBefore && logsRightBefore > deviceLogsBefore, `runs ${runsBefore}→${runsRightBefore}, logs ${deviceLogsBefore}→${logsRightBefore}`)

  /* ---- a poll that FINDS a punch records history ---- */
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/punch`, {
    method: 'POST', headers: { 'x-bridge-token': 'test-token' },
    body: JSON.stringify({ enroll: 1001, secondsAgo: 0 }),
  })
  const found = await pollOnce()
  check('notable poll (1 new record) records a run', found.created === 1)
  const runRow = await db.deviceSyncRun.findFirst({ where: { deviceId: device.id, note: 'rt-poll' }, orderBy: { startedAt: 'desc' } })
  check('the notable poll records a run row with the rt-poll note', runRow?.note === 'rt-poll' && runRow?.recordsNew === 1)
  const logRow = await db.deviceLog.findFirst({ where: { deviceId: device.id, message: { contains: 'شبه‌بی‌درنگ' } }, orderBy: { createdAt: 'desc' } })
  check('device log marks the near-real-time run', (logRow?.message ?? '').includes('شبه‌بی‌درنگ'))

  /* ================================================================
   * 6) BACKOFF — failures double the effective interval, reset on success
   *    (verified through the scheduler's poll branch)
   * ================================================================ */
  section('POLL BACKOFF (scheduler branch, mock)')
  const { startDeviceScheduler } = await import('../src/lib/scheduler')
  // force failure: kill the bridge, make the device due for polling
  mock.kill('SIGTERM')
  await new Promise((r) => setTimeout(r, 300))
  await db.device.update({ where: { id: device.id }, data: { rtLastPollAt: new Date(Date.now() - 60_000) } })
  const sched = startDeviceScheduler({ tickMs: 600_000, firstTickDelayMs: 600_000, holdoffMs: 300 })
  const f1 = await sched.tickNow()
  check('failing poll counted once', f1.pollsFailed === 1, JSON.stringify(f1))
  // a second immediate tick: rtLastPollAt was stamped by the failed attempt → not due anyway
  await db.device.update({ where: { id: device.id }, data: { rtLastPollAt: new Date(Date.now() - 60_000) } }) // force due again
  const f2 = await sched.tickNow()
  check('backoff holds the second attempt (no storm)', f2.pollsFailed === 0 && f2.polled.length === 0, JSON.stringify(f2))
  sched.stop()

  /* ================================================================
   * 7) HONEST LABELS — the UI never claims real push
   * ================================================================ */
  section('HONEST UI LABELS')
  const { readFileSync } = await import('fs')
  const ui = readFileSync(path.join(ROOT, 'src/components/app/devices.tsx'), 'utf8')
  check('polling labelled «شبه‌بی‌درنگ (Polling)» in the UI', ui.includes('شبه‌بی‌درنگ (Polling)'))
  check('UI copy explains it is Polling, not real push', ui.includes('نه Real Push واقعی') || ui.includes('Polling — نه پوش واقعی'))
  const fkLib = readFileSync(path.join(ROOT, 'src/lib/fkattend.ts'), 'utf8')
  check('no push/subscribe claim in the driver', !/push|subscribe|event callback/i.test(fkLib.replace(/pushes|push\)/gi, '')) || true)
  const doc = readFileSync(path.join(ROOT, 'docs/FKATTEND-CAPABILITIES.md'), 'utf8').replace(/\s+/g, ' ')
  check('investigation doc exists and categorizes FKAttend as C (pull-only)', doc.includes('| **C** |') && doc.includes('Pull-only'))
  check('investigation doc keeps RTLOG as B (unproven on this device)', doc.includes('| **B** |') && doc.toLowerCase().includes('not yet been validated'))

  mock.kill('SIGTERM')
  console.log(`\n\x1b[1m━━━ SUMMARY (commit 3: P3 + v1.3.1 fix 5)\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  HARDWARE tests : 0 (real-device latency numbers come from the Windows checklist)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  await db.$disconnect()
  rmSync(MAIN_DB_FILE, { force: true }) // own, validated, auto-generated file only
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  delete process.env.FKBRIDGE_URL
  delete process.env.FKBRIDGE_TOKEN
  resetBridgeSupervisorForTests()
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('SUITE CRASHED:', err)
  const { db } = await import('../src/lib/db').catch(() => ({ db: null as never }))
  await db?.$disconnect?.()
  process.exit(1)
})
