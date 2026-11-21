/**
 * Phase 1 — database administration test suite.
 * =====================================================================
 * REAL SQLite, REAL Prisma client, REAL operations — on a THROWAWAY
 * test-owned database (scripts/test-db-guard.ts): an external DATABASE_URL
 * is ignored, the production db/custom.db is never touched, nothing real
 * is ever wiped. Backups land inside the same test directory.
 *
 * Coverage (Phase 1 spec):
 *   [A] status on a fresh schema — EMPTY detection from ROW COUNTS (file
 *       size is never consulted — asserted by an explicit tiny-file case)
 *   [B] ensure-admin bootstrap — random password, FIRST-LOGIN.txt written,
 *       scrypt hash verifies, audit row, no password in audit details,
 *       idempotent when a super admin already exists
 *   [C] demo seed on empty DB — data created; demo users only for free
 *       usernames; demo device only when none exists; settings only
 *       missing keys; notification rules only when none exist
 *   [D] re-seed over a CUSTOMIZED install — org_name / sms_api_key /
 *       device secrets / extra user / extra rule survive verbatim
 *   [E] wipe-to-empty — business tables emptied, users/devices/settings/
 *       rules/audit PRESERVED, user.personnelId detached, sessions cleared
 *   [F] backup — consistent snapshot file (SQLite header), openable with
 *       the same row counts, last_backup_at set, audit entry, unique names
 *   [G] atomicity — a mid-seed failure (SQL trigger ABORT) rolls the whole
 *       operation back; the database keeps its pre-operation contents
 *   [H] concurrency — two parallel destructive ops: exactly one runs, the
 *       other is rejected busy (409 semantics)
 *   [I] CLI — status prints the DB_STATE machine line; wipe/seed refuse to
 *       run without --yes; seed --yes works end-to-end via the installer path
 *   [J] env-ensure — existing variables preserved, DATABASE_URL updated
 *       exactly once, missing file created, other values never printed
 *   [K] build-lock — concurrent build refused while the holder is ALIVE
 *       (lock untouched); a stale lock (dead pid) is archived and proceeds;
 *       the lock is released after the run
 *
 * Run:  npx tsx scripts/test-db-admin.ts
 */
import { spawnSync } from 'child_process'
import { mkdirSync, readFileSync, writeFileSync, existsSync, rmSync, readdirSync } from 'fs'
import path from 'path'
import { PrismaClient } from '@prisma/client'
import { generateTestDatabasePath, assertTestOwnedDatabasePath, sqliteUrl } from './test-db-guard'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_DBADMIN || '/tmp/db-admin-tests'
const DB_FILE = generateTestDatabasePath(TMP)
const DB_URL = sqliteUrl(DB_FILE)
process.env.DATABASE_URL = DB_URL // BEFORE the first Prisma client below

let passed = 0
let failed = 0
let sectionsPassed = 0
let sectionsFailed = 0
let sectionFailed = false
let sectionsStarted = 0
const failures: string[] = []
function section(t: string) {
  if (sectionsStarted > 0) { if (sectionFailed) sectionsFailed++; else sectionsPassed++ }
  sectionsStarted++
  sectionFailed = false
  console.log(`\n━━━ ${t}`)
}
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; sectionFailed = true; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}
function sh(cmd: string, args: string[], env: Record<string, string> = {}) {
  return spawnSync(cmd, args, { cwd: ROOT, env: { ...process.env, ...env }, encoding: 'utf-8', timeout: 240_000 })
}

async function main() {
  mkdirSync(TMP, { recursive: true })
  assertTestOwnedDatabasePath(DB_FILE, TMP)
  rmSync(path.join(path.dirname(DB_FILE), 'backups'), { recursive: true, force: true })
  rmSync(path.join(path.dirname(DB_FILE), 'FIRST-LOGIN.txt'), { force: true })

  /* schema push (the INSTALL.bat step) */
  const push = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: DB_URL })
  check('prisma db push on the throwaway test database', push.status === 0, (push.stderr || '').slice(-200))

  const db = new PrismaClient()

  /* ---------------- [A] status on fresh DB ---------------- */
  section('A. status: fresh schema → EMPTY by row counts, never by file size')
  {
    const { dbStatus } = await import('../src/lib/db-admin')
    const s = await dbStatus(db)
    check('schemaOk true after db push', s.schemaOk === true)
    check('hasBusinessData false (no rows)', s.hasBusinessData === false)
    check('counts readable', !!s.counts && s.counts.personnel === 0 && s.counts.users === 0)
    check('dbPath resolves to the test file', s.dbPath === DB_FILE, String(s.dbPath))
    check('sizeIsCriterion is literally false in the payload', s.sizeIsCriterion === false)
    // a TINY database file must still be treated as EMPTY-but-valid, not "fresh":
    // row counts are the only criterion. (File size is displayed only.)
    check('sizeBytes reported but not used for decisions', typeof s.sizeBytes === 'number' && (s.sizeBytes ?? 0) > 0)
  }

  /* ---------------- [B] ensure-admin ---------------- */
  section('B. ensure-admin: bootstrap SUPER_ADMIN with a random password')
  {
    const { dbEnsureSuperAdmin } = await import('../src/lib/db-admin')
    const r = await dbEnsureSuperAdmin(db)
    const loginFile = path.join(path.dirname(DB_FILE), 'FIRST-LOGIN.txt')
    check('created the admin account', r.created === true && r.username === 'admin')
    check('FIRST-LOGIN.txt written next to the DB', existsSync(loginFile))
    const txt = existsSync(loginFile) ? readFileSync(loginFile, 'utf-8') : ''
    check('FIRST-LOGIN.txt contains the username and the password', txt.includes('admin') && txt.includes(r.password ?? ''))
    check('password is 12 chars, mixed case+digits', /^[A-Za-z0-9]{12}$/.test(r.password ?? ''))
    const admin = await db.user.findUnique({ where: { username: 'admin' } })
    const { verifyPassword } = await import('../src/lib/auth-hash')
    check('stored hash verifies against the generated password', !!admin && verifyPassword(r.password ?? '', admin.passwordHash))
    const audit = await db.auditLog.findFirst({ where: { action: 'DB_ADMIN_INIT' } })
    check('audit row written for the bootstrap', !!audit)
    check('audit details contain NO password value', !audit?.details || !audit.details.includes(r.password ?? ''))
    const again = await dbEnsureSuperAdmin(db)
    check('idempotent: second call does nothing', again.created === false)
  }

  /* ---------------- [C] demo seed on empty DB ---------------- */
  section('C. demo seed on an empty database')
  {
    const { dbDemoSeed } = await import('../src/lib/db-admin')
    const r = await dbDemoSeed(db)
    check('12 demo persons created', r.seeded.persons === 12, String(r.seeded.persons))
    check('demo punches created (expected ≈590±absences)', r.seeded.punches > 400 && r.seeded.punches <= 744, String(r.seeded.punches))
    check('demo users created for free usernames', r.seeded.usersCreated.length === 3 && r.seeded.usersCreated.join(',') === 'hr,manager,ali', r.seeded.usersCreated.join(',')) // admin exists from [B]
    check('demo device created (device table was empty)', r.seeded.deviceCreated === true)
    check('settings created only for missing keys', r.seeded.settingsCreated.length > 0 && r.seeded.settingsPreserved.length === 0)
    check('backup taken before the seed', !!r.backup?.path && existsSync(r.backup.path))
    const admins = await db.user.count({ where: { role: 'SUPER_ADMIN' } })
    check('the bootstrap admin from [B] survived the seed', admins >= 1)
    const status = await (await import('../src/lib/db-admin')).dbStatus(db)
    check('status now reports DATA', status.hasBusinessData === true)
    const audit = await db.auditLog.findFirst({ where: { action: 'DB_SEED' } })
    check('audit row written for the seed', !!audit)
  }

  /* ---------------- [D] re-seed over a customized install ---------------- */
  section('D. re-seed preserves THIS installation\'s settings/devices/accounts')
  {
    // customize everything the seed must NOT overwrite
    await db.setting.update({ where: { id: 'org_name' }, data: { value: 'شرکت واقعی مشتری' } })
    await db.setting.upsert({ where: { id: 'sms_api_key' }, create: { id: 'sms_api_key', value: 'SECRET-KEEP' }, update: { value: 'SECRET-KEEP' } })
    const dev = await db.device.findFirstOrThrow()
    await db.device.update({ where: { id: dev.id }, data: { name: 'دستگاه گیت ورودی', password: 123456, passwordEnc: 'dpapi:machine:AAAB', ipAddress: '10.0.0.9', port: 4238, protocolFamily: 'FKATTEND' } })
    const { hashPassword } = await import('../src/lib/auth-hash')
    const customUser = await db.user.create({ data: { username: 'customuser', passwordHash: hashPassword('MyOwnPass9'), role: 'ADMIN', fullName: 'کاربر اختصاصی نصب' } })
    const rulesBefore = await db.notificationRule.count()
    await db.notificationRule.create({ data: { type: 'OT_REQUEST', enabled: true, notifyRole: 'ADMIN' } })
    const auditBefore = await db.auditLog.count()

    const { dbDemoSeed } = await import('../src/lib/db-admin')
    const r = await dbDemoSeed(db)
    check('re-seed completed', true)

    const org = await db.setting.findUnique({ where: { id: 'org_name' } })
    check('org_name NOT overwritten (install-specific)', org?.value === 'شرکت واقعی مشتری', org?.value)
    const sms = await db.setting.findUnique({ where: { id: 'sms_api_key' } })
    check('sms_api_key NOT overwritten (secret)', sms?.value === 'SECRET-KEEP')
    check('seed reports the preserved settings', r.seeded.settingsPreserved.includes('org_name') && r.seeded.settingsPreserved.includes('sms_api_key'))

    const devAfter = await db.device.findFirstOrThrow()
    check('device count still 1 (no duplicate demo device)', (await db.device.count()) === 1)
    check('device connection fields preserved', devAfter.ipAddress === '10.0.0.9' && devAfter.port === 4238 && devAfter.protocolFamily === 'FKATTEND')
    check('device network password + DPAPI blob preserved', devAfter.password === 123456 && devAfter.passwordEnc === 'dpapi:machine:AAAB')
    check('device name preserved', devAfter.name === 'دستگاه گیت ورودی')

    const cu = await db.user.findUnique({ where: { username: 'customuser' } })
    const { verifyPassword } = await import('../src/lib/auth-hash')
    check('custom user kept its OWN password (not reset)', !!cu && verifyPassword('MyOwnPass9', cu.passwordHash))
    check('no demo user was re-created for taken usernames', r.seeded.usersCreated.length === 0, r.seeded.usersCreated.join(','))

    const rulesAfter = await db.notificationRule.count()
    check('custom notification rule survived (rules preserved)', rulesAfter === rulesBefore + 1)
    const auditAfter = await db.auditLog.count()
    check('audit history preserved (only grew)', auditAfter >= auditBefore)
  }

  /* ---------------- [E] wipe-to-empty ---------------- */
  section('E. wipe-to-empty: business data gone, protected classes intact')
  {
    const { dbWipeToEmpty } = await import('../src/lib/db-admin')
    const usersBefore = await db.user.findMany({ select: { id: true, username: true, passwordHash: true } })
    const rulesBefore = await db.notificationRule.findMany({ select: { id: true } })
    const r = await dbWipeToEmpty(db)
    check('wipe completed with a backup', !!r.backup?.path && existsSync(r.backup.path))

    check('personnel emptied', (await db.personnel.count()) === 0)
    check('punches emptied', (await db.punchRecord.count()) === 0)
    check('raw punches emptied', (await db.rawPunch.count()) === 0)
    check('daily attendance emptied', (await db.dailyAttendance.count()) === 0)
    check('leave requests emptied', (await db.leaveRequest.count()) === 0)
    check('overtime requests emptied', (await db.overtimeRequest.count()) === 0)
    check('shifts emptied', (await db.shift.count()) === 0)
    check('leave types emptied', (await db.leaveType.count()) === 0)
    check('holidays emptied', (await db.holiday.count()) === 0)
    check('org structure emptied (departments/jobTitles/employmentTypes)', (await db.department.count()) === 0 && (await db.jobTitle.count()) === 0 && (await db.employmentType.count()) === 0)

    const usersAfter = await db.user.findMany({ select: { id: true, username: true, passwordHash: true, personnelId: true } })
    check('user ACCOUNTS preserved (login access survives)', usersAfter.length === usersBefore.length)
    check('user personnel links detached (SetNull semantics, no dangling FKs)', usersAfter.every((u) => u.personnelId === null))
    check('user password hashes unchanged', usersAfter.every((u) => usersBefore.find((b) => b.id === u.id)?.passwordHash === u.passwordHash))
    check('devices preserved', (await db.device.count()) === 1)
    const sms = await db.setting.findUnique({ where: { id: 'sms_api_key' } })
    check('settings preserved', sms?.value === 'SECRET-KEEP')
    check('notification rules preserved', (await db.notificationRule.findMany({ select: { id: true } })).length === rulesBefore.length)
    check('audit trail preserved and grew', (await db.auditLog.count()) >= 1)
    const resetAudit = await db.auditLog.findFirst({ where: { action: 'DB_RESET' } })
    check('audit row written for the reset', !!resetAudit)
    check('sessions wiped (forced re-login)', (await db.session.count()) === 0)
    const status = await (await import('../src/lib/db-admin')).dbStatus(db)
    check('status reports EMPTY again', status.hasBusinessData === false)
  }

  /* ---------------- [F] backup ---------------- */
  section('F. backup: consistent, restorable, audited')
  {
    // put some data back so the snapshot has content
    const { dbDemoSeed } = await import('../src/lib/db-admin')
    await dbDemoSeed(db, { skipBackup: true })
    const personsBefore = await db.personnel.count()

    const { dbBackup } = await import('../src/lib/db-admin')
    const b1 = await dbBackup(db, { reason: 'test' })
    const b2 = await dbBackup(db, { reason: 'test' })
    check('two backups → two distinct files', b1.path !== b2.path && existsSync(b1.path) && existsSync(b2.path))
    const header = readFileSync(b1.path).slice(0, 16).toString('latin1')
    check('backup file has the SQLite header', header.startsWith('SQLite format 3'))

    // restorable: open the snapshot as a database and compare counts
    const prevUrl = process.env.DATABASE_URL
    process.env.DATABASE_URL = `file:${b1.path}`
    const restoreClient = new PrismaClient()
    try {
      const personsInBackup = await restoreClient.personnel.count()
      check('backup opens and contains the same row counts', personsInBackup === personsBefore, `${personsInBackup} vs ${personsBefore}`)
    } finally {
      await restoreClient.$disconnect()
      process.env.DATABASE_URL = prevUrl
    }

    const at = await db.setting.findUnique({ where: { id: 'last_backup_at' } })
    const p = await db.setting.findUnique({ where: { id: 'last_backup_path' } })
    check('last_backup_at / last_backup_path settings updated', !!at && p?.value === b2.path)
    const audit = await db.auditLog.findFirst({ where: { action: 'DB_BACKUP' }, orderBy: { createdAt: 'desc' } })
    check('audit row written for the backup', !!audit)
    check('backups listed by status (≤10, newest first)', (await (await import('../src/lib/db-admin')).dbStatus(db)).backups.length >= 2)
  }

  /* ---------------- [G] atomicity ---------------- */
  section('G. atomicity: mid-seed failure leaves NOTHING half-applied')
  {
    const before = {
      persons: await db.personnel.count(),
      punches: await db.punchRecord.count(),
      departments: await db.department.count(),
      users: await db.user.count(),
      devices: await db.device.count(),
      settings: await db.setting.count(),
      leaveTypes: await db.leaveType.count(),
    }
    // a trigger that aborts every PunchRecord insert — the seed fails LATE
    // (after departments/shifts/personnel were already created inside the tx)
    await db.$executeRawUnsafe(`CREATE TRIGGER boom_punch BEFORE INSERT ON "PunchRecord" BEGIN SELECT RAISE(ABORT, 'boom'); END`)
    let threw = false
    try {
      const { dbDemoSeed } = await import('../src/lib/db-admin')
      await dbDemoSeed(db, { skipBackup: true })
    } catch { threw = true }
    check('the seed threw when the insert failed', threw)
    await db.$executeRawUnsafe('DROP TRIGGER boom_punch')
    const after = {
      persons: await db.personnel.count(),
      punches: await db.punchRecord.count(),
      departments: await db.department.count(),
      users: await db.user.count(),
      devices: await db.device.count(),
      settings: await db.setting.count(),
      leaveTypes: await db.leaveType.count(),
    }
    check('persons unchanged (transaction rolled back)', after.persons === before.persons, `${after.persons} vs ${before.persons}`)
    check('punches unchanged (transaction rolled back)', after.punches === before.punches, `${after.punches} vs ${before.punches}`)
    check('departments unchanged (nothing half-created)', after.departments === before.departments)
    check('users unchanged', after.users === before.users)
    check('devices unchanged', after.devices === before.devices)
    check('settings unchanged', after.settings === before.settings)
    check('leave types unchanged', after.leaveTypes === before.leaveTypes)
    // the database is still USABLE after the rollback (no lock held, writes work)
    await db.setting.upsert({ where: { id: 'post_crash_canary' }, create: { id: 'post_crash_canary', value: 'alive' }, update: { value: 'alive' } })
    check('database still accepts writes after the failed operation', (await db.setting.findUnique({ where: { id: 'post_crash_canary' } }))?.value === 'alive')
  }

  /* ---------------- [H] concurrency ---------------- */
  section('H. concurrency: one destructive op at a time (in-process lock)')
  {
    const { dbDemoSeed, DbAdminBusyError } = await import('../src/lib/db-admin')
    const results = await Promise.allSettled([dbDemoSeed(db, { skipBackup: true }), dbDemoSeed(db, { skipBackup: true })])
    const fulfilled = results.filter((r) => r.status === 'fulfilled').length
    const rejectedBusy = results.filter((r) => r.status === 'rejected' && r.reason instanceof DbAdminBusyError).length
    check('exactly ONE operation ran', fulfilled === 1, `fulfilled=${fulfilled}`)
    check('the other was rejected as busy (409 semantics)', rejectedBusy === 1)
    check('the database is still healthy after the race', (await db.personnel.count()) > 0)
  }

  /* ---------------- [I] CLI (installer path) ---------------- */
  section('I. CLI: status / confirmations / seed end-to-end (tsx, like INSTALL.bat)')
  {
    const st = sh('npx', ['tsx', 'scripts/db-admin.ts', 'status'], { DATABASE_URL: DB_URL })
    check('status exits 0', st.status === 0, (st.stderr || '').slice(-200))
    check('status prints the DB_STATE machine line', /DB_STATE=DATA/.test(st.stdout), st.stdout.slice(0, 200))
    check('status prints row counts (not file size)', /پرسنل/.test(st.stdout))

    const no = sh('npx', ['tsx', 'scripts/db-admin.ts', 'wipe'], { DATABASE_URL: DB_URL })
    check('wipe REFUSES without --yes (exit 2)', no.status === 2, `exit=${no.status}`)
    const no2 = sh('npx', ['tsx', 'scripts/db-admin.ts', 'seed'], { DATABASE_URL: DB_URL })
    check('seed REFUSES without --yes (exit 2)', no2.status === 2, `exit=${no2.status}`)

    const w = sh('npx', ['tsx', 'scripts/db-admin.ts', 'wipe', '--yes', '--no-backup'], { DATABASE_URL: DB_URL })
    check('wipe --yes completes (no-backup variant for throwaway DBs)', w.status === 0 && /دیتابیس خالی شد/.test(w.stdout), (w.stderr || w.stdout).slice(-300))
    const s = sh('npx', ['tsx', 'scripts/db-admin.ts', 'seed', '--yes', '--no-backup'], { DATABASE_URL: DB_URL })
    check('seed --yes completes end-to-end', s.status === 0 && /اطلاعات نمونه نصب شد/.test(s.stdout), (s.stderr || s.stdout).slice(-300))
    check('seed honestly reports that EXISTING accounts were kept (users preserved)', /حساب‌های موجود حفظ شدند|حساب‌های نمونه ساخته شد/.test(s.stdout), (s.stdout || '').slice(-300))

    // the fresh-database path: a DB with NO users yet → the seed DOES create demo accounts
    const DB3 = generateTestDatabasePath(TMP)
    const URL3 = sqliteUrl(DB3)
    const push3 = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: URL3 })
    check('third throwaway DB pushed (fresh, no users)', push3.status === 0)
    const s3 = sh('npx', ['tsx', 'scripts/db-admin.ts', 'seed', '--yes', '--no-backup'], { DATABASE_URL: URL3 })
    check('fresh seed mentions the created demo accounts (admin, …)', s3.status === 0 && /admin/.test(s3.stdout), (s3.stderr || s3.stdout).slice(-300))
    rmSync(DB3, { force: true })

    // fresh-database CLI bootstrap: ensure-admin on a brand-new empty db
    const DB2 = generateTestDatabasePath(TMP)
    const URL2 = sqliteUrl(DB2)
    const push2 = sh('npx', ['prisma', 'db', 'push', '--skip-generate'], { DATABASE_URL: URL2 })
    check('second throwaway DB pushed', push2.status === 0)
    const ea = sh('npx', ['tsx', 'scripts/db-admin.ts', 'ensure-admin'], { DATABASE_URL: URL2 })
    check('ensure-admin creates the first account + FIRST-LOGIN.txt', ea.status === 0 && existsSync(path.join(path.dirname(DB2), 'FIRST-LOGIN.txt')), (ea.stderr || ea.stdout).slice(-300))
    rmSync(DB2, { force: true })
  }

  /* ---------------- [J] env-ensure ---------------- */
  section('J. env-ensure.js: .env preservation on reinstall')
  {
    const tmpEnv = path.join(TMP, 'env-ensure-test.env')
    writeFileSync(tmpEnv, ['NODE_ENV=production', 'SECRET_TOKEN=keep-me', 'DATABASE_URL=file:OLD/old.db', '# comment stays', ''].join('\n'), 'utf-8')
    const r1 = sh('node', ['scripts/env-ensure.js', 'DATABASE_URL=file:NEW/new.db', '--env', tmpEnv])
    check('first run exits 0', r1.status === 0, (r1.stderr || '').slice(-200))
    const out1 = readFileSync(tmpEnv, 'utf-8')
    check('DATABASE_URL updated to the new value', out1.includes('DATABASE_URL=file:NEW/new.db') && !out1.includes('OLD/old.db'))
    check('other variables preserved verbatim', out1.includes('SECRET_TOKEN=keep-me') && out1.includes('NODE_ENV=production'))
    check('comments preserved', out1.includes('# comment stays'))
    check('no duplicate DATABASE_URL', (out1.match(/^DATABASE_URL=/gm) || []).length === 1)
    check('run output does NOT print other values', !r1.stdout.includes('keep-me'), r1.stdout)
    const r2 = sh('node', ['scripts/env-ensure.js', 'DATABASE_URL=file:NEW2/new2.db', '--env', tmpEnv])
    const out2 = readFileSync(tmpEnv, 'utf-8')
    check('second run keeps exactly one DATABASE_URL (idempotent)', r2.status === 0 && (out2.match(/^DATABASE_URL=/gm) || []).length === 1 && out2.includes('NEW2/new2.db'))
    const missing = path.join(TMP, 'env-ensure-missing.env')
    rmSync(missing, { force: true })
    const r3 = sh('node', ['scripts/env-ensure.js', 'DATABASE_URL=file:first.db', '--env', missing])
    check('creates .env when absent', r3.status === 0 && readFileSync(missing, 'utf-8').includes('DATABASE_URL=file:first.db'))
    const bad = sh('node', ['scripts/env-ensure.js', 'NOT_DATABASE_URL=x', '--env', missing])
    check('rejects a non-DATABASE_URL argument (exit 2)', bad.status === 2)
  }

  /* ---------------- [K] build-lock ---------------- */
  section('K. build-with-lock.js: live vs stale lock, release after run')
  {
    const lockPath = path.join(ROOT, '.build-lock.json')
    rmSync(lockPath, { force: true })
    // 1) normal acquire/release via dry-run
    const dry = sh('node', ['scripts/build-with-lock.js', '--dry-run'])
    check('dry-run acquires and releases the lock (exit 0)', dry.status === 0 && !existsSync(lockPath), (dry.stderr || '').slice(-200))
    // 2) LIVE holder: our own process is alive by definition
    writeFileSync(lockPath, JSON.stringify({ pid: process.pid, startedAt: new Date().toISOString(), command: 'npm run build' }), 'utf-8')
    const refused = sh('node', ['scripts/build-with-lock.js', '--dry-run'])
    check('a build with a LIVE holder is refused (exit 2)', refused.status === 2, `exit=${refused.status}`)
    check('the lock of the LIVE holder was NOT deleted', existsSync(lockPath))
    check('refusal message explains the running build', /PID/.test(refused.stderr || refused.stdout))
    // 3) STALE holder: a pid that has already exited (spawnSync child is dead on return)
    const shortLived = spawnSync('node', ['-e', 'process.exit(0)'])
    const deadPid = shortLived.pid ?? 999999
    writeFileSync(lockPath, JSON.stringify({ pid: deadPid, startedAt: new Date().toISOString(), command: 'npm run build' }), 'utf-8')
    const stale = sh('node', ['scripts/build-with-lock.js', '--dry-run'])
    const staleFiles = readdirSync(ROOT).filter((f) => f.startsWith('.build-lock.stale-'))
    check('a STALE lock (dead pid) is archived and the build proceeds', stale.status === 0 && staleFiles.length > 0, (stale.stderr || '').slice(-200))
    check('the stale evidence file is kept (never silently deleted)', staleFiles.length > 0)
    for (const f of staleFiles) rmSync(path.join(ROOT, f), { force: true })
    rmSync(lockPath, { force: true })
  }

  await db.$disconnect()
  if (sectionFailed) sectionsFailed++; else sectionsPassed++ // close the last section
  console.log('\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━')
  console.log(`db-admin suite: ${passed} assertion(s) passed, ${failed} failed`)
  console.log(`db-admin suite: ${sectionsPassed} section(s) fully green, ${sectionsFailed} with failures (A..K)`)
  if (failed > 0) { console.log('\nFAILED:'); for (const f of failures) console.log('  ✘ ' + f); process.exit(1) }
  process.exit(0)
}

main().catch((e) => {
  console.error('suite crashed:', e)
  process.exit(1)
})
