/**
 * Addendum commit 4 (P4) test suite — MOCK ONLY.
 * =====================================================================
 * Live reports + notifications, verified at the SERVER level against the
 * MOCK FKAttend bridge and a MINI REALTIME SERVICE (a tiny :3004 /emit
 * listener that records every event the app would broadcast — the exact
 * socket.io path used in production).
 *
 * Covered (addendum acceptance items 8, 9, 10 + §4/§5 rules):
 *   [user notif]  matched punches → ONE aggregated notification per user
 *                 (only the punched person's own account — EMPLOYEE never
 *                 sees other people's punches), device name + time present,
 *                 direction NEVER claimed (UNKNOWN until verified)
 *   [admin notif] unmatched punches → ONE aggregated notification for
 *                 ADMIN + SUPER_ADMIN roles; EMPLOYEE role gets NOTHING
 *   [dedupe]      re-sync with 0 new records → ZERO new notifications
 *   [no target]   punches of personnel without a user account notify nobody
 *   [live events] on new punches the app emits 'device-sync' (counts only,
 *                 no person names → no privacy leak in the broadcast) and
 *                 'notification' events (one per notified user)
 *   [mark-read]   notification rows exist with read=false → markable
 *                 (the API already enforces per-user visibility)
 *   [unmatched]   unmatched punches preserved in RawPunch (nothing dropped)
 *
 * Run (DATABASE_URL REQUIRED — refuses db/custom.db):
 *   DATABASE_URL=file:/tmp/nr-addendum4/addendum4.db npx tsx scripts/test-live-notif.ts
 */
import { spawn, spawnSync } from 'child_process'
import { rmSync, mkdirSync, writeFileSync } from 'fs'
import http from 'http'
import path from 'path'
import { db } from '../src/lib/db'
import { importFkAttendLogs } from '../src/lib/fkattend'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_RAWTEST || '/tmp/nr-addendum4'
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
const BRIDGE_PORT = 8794

/** mini realtime service — records every /emit the app broadcasts */
const rtEvents: Array<{ event: string; payload: Record<string, unknown> }> = []

async function main() {
  if (!MAIN_DB) throw new Error('DATABASE_URL must point at a TEMP database')
  if (/custom\.db/i.test(MAIN_DB)) throw new Error('refusing to run against db/custom.db')
  mkdirSync(TMP, { recursive: true })

  /* ---- mini realtime listener on a dedicated test port ----
     (notify.ts honours HOZOR_RT_URL — set before the module loads) */
  process.env.HOZOR_RT_URL = 'http://127.0.0.1:3999/emit'
  await new Promise<void>((resolve) => {
    const srv = http.createServer((req, res) => {
      let body = ''
      req.on('data', (c) => (body += c))
      req.on('end', () => {
        try {
          const { event, payload } = JSON.parse(body)
          rtEvents.push({ event, payload })
        } catch { /* ignore */ }
        res.writeHead(200).end('{"ok":true}')
      })
    })
    srv.listen(3999, '127.0.0.1', resolve)
  })

  /* ---- DB ---- */
  rmSync(MAIN_DB_FILE, { force: true })
  rmSync(`${MAIN_DB_FILE}-journal`, { force: true })
  const phase1 = sh('git', ['show', 'HEAD:prisma/schema.prisma'], {})
  writeFileSync(`${TMP}/phase1-schema.prisma`, phase1.stdout)
  sh('npx', ['prisma', 'db', 'push', '--schema', `${TMP}/phase1-schema.prisma`, '--skip-generate'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.3.0-phase1.mjs', 'up'], { DATABASE_URL: MAIN_DB })
  sh('node', ['scripts/migrate-v1.4.0-phase2.mjs', 'up'], { DATABASE_URL: MAIN_DB })

  /* ---- users + personnel ---- */
  const { hashPassword } = await import('../src/lib/auth')
  const uAli = await db.user.create({ data: { username: 'ali', passwordHash: hashPassword('x1'), role: 'EMPLOYEE', fullName: 'علی محمدی' } })
  const uAdmin = await db.user.create({ data: { username: 'admin', passwordHash: hashPassword('x2'), role: 'ADMIN', fullName: 'مدیر سامانه' } })
  const uSuper = await db.user.create({ data: { username: 'super', passwordHash: hashPassword('x3'), role: 'SUPER_ADMIN', fullName: 'مدیر کل' } })
  const uOther = await db.user.create({ data: { username: 'other', passwordHash: hashPassword('x4'), role: 'EMPLOYEE', fullName: 'کارمند دیگر' } })
  // personnel WITH a linked user account (matched + notified)
  const pAli = await db.personnel.create({ data: { personnelCode: '1001', firstName: 'علی', lastName: 'محمدی', pin: '1001', deviceUserId: 1001 } })
  await db.user.update({ where: { id: uAli.id }, data: { personnelId: pAli.id } })
  // personnel WITHOUT a user account (matched, but nobody to notify)
  await db.personnel.create({ data: { personnelCode: '2002', firstName: 'رضا', lastName: 'کریمی', pin: '2002', deviceUserId: 2002 } })
  // enroll 7777 → unmatched

  const device = await db.device.create({
    data: {
      name: 'دستگاه اعلان', type: 'NOVIN_NP', protocolFamily: 'FKATTEND',
      ipAddress: '192.168.1.75', port: 4238, serialNumber: SERIAL,
      bridgeUrl: `http://127.0.0.1:${BRIDGE_PORT}`, bridgeToken: 'test-token',
    },
  })

  const mock = spawn('node', ['scripts/mock-fkattend-bridge.js'], {
    cwd: ROOT,
    env: { ...process.env, PORT: String(BRIDGE_PORT), TOKEN: 'test-token', SERIAL, LOGS: '6', USERS: '1001,2002,7777' },
    stdio: ['ignore', 'pipe', 'pipe'],
  })
  await new Promise((r) => setTimeout(r, 900))

  /* ================================================================
   * 1) import → notifications + live events
   * ================================================================ */
  section('NOTIFICATIONS ON IMPORT (mock bridge + mini realtime service)')
  const s1 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('6 records imported (2×1001, 2×2002, 2×7777)', s1.created === 6, `created=${s1.created}`)

  const notifs = await db.notification.findMany({ orderBy: { createdAt: 'asc' } })
  const userNotifs = notifs.filter((n) => n.userId === uAli.id)
  check('the punched employee (with linked account) gets EXACTLY ONE notification', userNotifs.length === 1, `count=${userNotifs.length}`)
  check('no OTHER (uninvolved) employee account receives anything', notifs.filter((n) => n.userId === uOther.id).length === 0)
  check('notification is aggregated (2 punches → one row)', (userNotifs[0]?.body ?? '').includes('۲ تردد'))
  check('device name present in the body', (userNotifs[0]?.body ?? '').includes('دستگاه اعلان'))
  check('time present in the body', (userNotifs[0]?.body ?? '').includes('آخرین ساعت'))
  check('direction NEVER claimed (no ورود/خروج wording)', !(userNotifs[0]?.body ?? '').includes('ورود') && !(userNotifs[0]?.body ?? '').includes('خروج'))


  const adminRoleNotifs = notifs.filter((n) => n.role === 'ADMIN' || n.role === 'SUPER_ADMIN')
  const employeeRoleNotifs = notifs.filter((n) => n.role === 'EMPLOYEE' || n.role === 'APPROVER')
  check('unmatched punches → ONE aggregated notification per admin role (ADMIN + SUPER_ADMIN)', adminRoleNotifs.length === 2, `count=${adminRoleNotifs.length}`)
  check('unmatched notification mentions the count and that records are KEPT', (adminRoleNotifs[0]?.body ?? '').includes('۲') && (adminRoleNotifs[0]?.body ?? '').includes('نگه‌داشته شد'))
  check('no EMPLOYEE-role broadcast ever (isolation)', employeeRoleNotifs.length === 0)
  const adminUserRows = notifs.filter((n) => n.userId === uAdmin.id || n.userId === uSuper.id)
  check('admin USERS also get their own row copies (existing notifyRoles pattern)', adminUserRows.length === 2)
  check('all notifications start unread (markable later)', notifs.every((n) => n.read === false))

  /* ---- notification visibility isolation (acceptance item 9, data level) ---- */
  const visibleToOtherEmployee = notifs.filter((n) => {
    // what /api/notifications would return for a different EMPLOYEE:
    // rows where userId = that user OR role = their role
    const isOwn = n.userId != null && n.userId !== uAli.id && n.userId !== uAdmin.id && n.userId !== uSuper.id
    const isRole = n.role === 'EMPLOYEE' || n.role === 'APPROVER'
    return isOwn || isRole
  })
  check('a DIFFERENT employee would see ZERO of these notifications (data-level isolation)', visibleToOtherEmployee.length === 0)

  /* ================================================================
   * 2) live events (the production socket.io chain)
   * ================================================================ */
  section('LIVE EVENTS (mini realtime service on :3004)')
  const syncEvents = rtEvents.filter((e) => e.event === 'device-sync')
  check("'device-sync' event emitted on new punches", syncEvents.length >= 1)
  const lastSync = syncEvents[syncEvents.length - 1]?.payload ?? {}
  check('device-sync payload carries counts only — NO person names (privacy in broadcast)', !JSON.stringify(lastSync).includes('علی') && !JSON.stringify(lastSync).includes('محمدی') && typeof lastSync.created === 'number')
  const notifEvents = rtEvents.filter((e) => e.event === 'notification')
  check("'notification' event emitted for the notified user (in-app + browser path)", notifEvents.some((e) => e.payload.userId === uAli.id))

  /* ================================================================
   * 3) dedupe — re-sync must not re-notify
   * ================================================================ */
  section('NOTIFICATION DEDUPE (acceptance item 8)')
  const before = await db.notification.count()
  const s2 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  const after = await db.notification.count()
  check('re-sync imports 0 new records', s2.created === 0)
  check('re-sync creates ZERO new notifications', after === before, `before=${before} after=${after}`)

  /* ================================================================
   * 4) new punch later → only THAT punch notifies
   * ================================================================ */
  section('INCREMENTAL NOTIFICATIONS (poll-like new punch)')
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/punch`, {
    method: 'POST', headers: { 'x-bridge-token': 'test-token' },
    body: JSON.stringify({ enroll: 1001, secondsAgo: 0 }),
  })
  await fetch(`http://127.0.0.1:${BRIDGE_PORT}/mock/punch`, {
    method: 'POST', headers: { 'x-bridge-token': 'test-token' },
    body: JSON.stringify({ enroll: 9999, secondsAgo: 0 }),
  })
  const s3 = await importFkAttendLogs(device.id, { mode: 'MOCK', full: true })
  check('2 new records imported (1 matched + 1 unmatched)', s3.created === 2, `created=${s3.created}`)
  const totalAli = await db.notification.count({ where: { userId: uAli.id } })
  check('the employee has exactly 2 notifications after the second import (1+1)', totalAli === 2, `total=${totalAli}`)
  const aliBodies = (await db.notification.findMany({ where: { userId: uAli.id } })).map((n) => n.body)
  check('the new notification describes exactly the new punch', aliBodies.some((b) => b.includes('یک تردد جدید')))
  const adminRoleTotal = await db.notification.count({ where: { role: 'ADMIN' } })
  check('admins have exactly 2 aggregated unmatched notifications after the second import', adminRoleTotal === 2, `total=${adminRoleTotal}`)
  const adminBodies = (await db.notification.findMany({ where: { role: 'ADMIN' } })).map((n) => n.body)
  check('the new admin notification says exactly «۱ تردد بدون تطبیق»', adminBodies.some((b) => b.includes('۱ تردد بدون تطبیق')))

  /* ================================================================
   * 5) unmatched retention + report pipeline (acceptance items 7, 10)
   * ================================================================ */
  section('REPORT PIPELINE + UNMATCHED RETENTION')
  const rawTotal = await db.rawPunch.count({ where: { deviceSerial: SERIAL, sourceProtocol: 'FKATTEND' } })
  check('all 8 raw punches stored', rawTotal === 8, `total=${rawTotal}`)
  const unmatched = await db.rawPunch.count({ where: { deviceSerial: SERIAL, matchStatus: 'UNMATCHED' } })
  check('unmatched punches preserved (2×7777 + 1×9999)', unmatched === 3, `unmatched=${unmatched}`)
  const aliPunches = await db.punchRecord.count({ where: { personId: pAli.id, source: 'DEVICE' } })
  check("Ali's PunchRecord projection exists (daily report pipeline fed)", aliPunches === 3, `punches=${aliPunches}`)
  const aliDaily = await db.dailyAttendance.findMany({ where: { personId: pAli.id } })
  check('daily attendance recalculated for the affected days (live report data)', aliDaily.length >= 1)
  check('daily rows never guess IN/OUT status from unverified direction', aliDaily.every((d) => d.status !== 'IN' && d.status !== 'OUT'))

  mock.kill('SIGTERM')

  console.log(`\n\x1b[1m━━━ SUMMARY (commit 4: P4)\x1b[0m`)
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log(`  HARDWARE tests : 0 (browser-notification UX + live refresh verified at server level only)`)
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
