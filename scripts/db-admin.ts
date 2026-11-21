/**
 * Phase 1 (v1.8.0-dev) — installer CLI for database administration.
 * =====================================================================
 * Used by INSTALL.bat (step: database). Safe by construction:
 *   * NEVER resets or seeds based on database file size — data presence is
 *     detected from real row counts only.
 *   * `status` is read-only and prints one machine-readable line
 *     (DB_STATE=EMPTY|DATA|NO_SCHEMA|NO_FILE) the installer can parse.
 *   * `wipe` / `seed` need --yes (explicit confirmation) and ALWAYS take a
 *     backup first (unless --no-backup for throwaway test databases).
 *   * `ensure-admin` bootstraps the initial SUPER_ADMIN on empty installs
 *     (random password written to FIRST-LOGIN.txt — never to logs).
 *
 * Usage:
 *   npx tsx scripts/db-admin.ts status
 *   npx tsx scripts/db-admin.ts backup
 *   npx tsx scripts/db-admin.ts wipe --yes [--no-backup]
 *   npx tsx scripts/db-admin.ts seed --yes [--no-backup]
 *   npx tsx scripts/db-admin.ts ensure-admin
 */
import { PrismaClient } from '@prisma/client'

const db = new PrismaClient()

function fa(n: number): string {
  return new Intl.NumberFormat('en-US').format(n) // keep latin digits in the console (log grep-ability)
}

async function main() {
  const cmd = process.argv[2] ?? ''
  const args = new Set(process.argv.slice(3))
  const yes = args.has('--yes')

  if (cmd === 'status') {
    const { dbStatus } = await import('../src/lib/db-admin')
    const s = await dbStatus(db)
    const state = !s.dbExists ? 'NO_FILE' : !s.schemaOk ? 'NO_SCHEMA' : s.hasBusinessData ? 'DATA' : 'EMPTY'
    console.log(`DB_STATE=${state}`)
    console.log(`  فایل دیتابیس: ${s.dbPath ?? 'نامشخص'}${s.dbExists ? ` (${fa(s.sizeBytes ?? 0)} بایت — فقط اطلاعات، معیار تشخیص نیست)` : ' (وجود ندارد)'}`)
    if (s.counts) {
      console.log(`  پرسنل: ${fa(s.counts.personnel)} | ترددها: ${fa(s.counts.punches)} | ترددهای خام: ${fa(s.counts.rawPunches)} | کارکرد روزانه: ${fa(s.counts.dailyAttendance)}`)
      console.log(`  کاربران: ${fa(s.counts.users)} (مدیر ارشد: ${fa(s.counts.superAdmins)}) | دستگاه‌ها: ${fa(s.counts.devices)} | شیفت‌ها: ${fa(s.counts.shifts)}`)
    } else {
      console.log('  اسکیمای دیتابیس ساخته نشده است.')
    }
    console.log(`  آخرین پشتیبان: ${s.lastBackupAt ?? '—'}`)
    if (s.backups.length) console.log(`  پشتیبان‌های موجود: ${fa(s.backups.length)} فایل در ${s.backupDir}`)
    if (state === 'DATA') console.log('  ⚠ دیتابیس موجود است — نصب به‌صورت پیش‌فرض داده‌های آن را حفظ می‌کند.')
    process.exit(0)
  }

  if (cmd === 'backup') {
    const { dbBackup } = await import('../src/lib/db-admin')
    const r = await dbBackup(db, { reason: 'cli' })
    console.log(`✅ پشتیبان ایجاد شد: ${r.path} (${fa(r.bytes)} بایت)`)
    process.exit(0)
  }

  if (cmd === 'wipe') {
    if (!yes) {
      console.error('برای پاک‌سازی، تأیید صریح لازم است: npx tsx scripts/db-admin.ts wipe --yes')
      process.exit(2)
    }
    const { dbWipeToEmpty } = await import('../src/lib/db-admin')
    const r = await dbWipeToEmpty(db, { reason: 'installer', skipBackup: args.has('--no-backup') })
    if (r.backup) console.log(`🛟 پشتیبان قبل از پاک‌سازی: ${r.backup.path}`)
    console.log(`✅ دیتابیس خالی شد: پرسنل ${fa(r.wiped.personnel)}، تردد ${fa(r.wiped.punchRecord)}، کارکرد ${fa(r.wiped.dailyAttendance)} حذف شدند`)
    console.log('   کاربران، دستگاه‌ها، تنظیمات و سابقهٔ ممیزی حفظ شدند.')
    process.exit(0)
  }

  if (cmd === 'seed') {
    if (!yes) {
      console.error('برای نصب اطلاعات نمونه، تأیید صریح لازم است: npx tsx scripts/db-admin.ts seed --yes')
      process.exit(2)
    }
    const { dbDemoSeed } = await import('../src/lib/db-admin')
    const r = await dbDemoSeed(db, { reason: 'installer', skipBackup: args.has('--no-backup') })
    if (r.backup) console.log(`🛟 پشتیبان قبل از نصب نمونه: ${r.backup.path}`)
    console.log(`✅ اطلاعات نمونه نصب شد: ${fa(r.seeded.persons)} پرسنل، ${fa(r.seeded.punches)} تردد`)
    if (r.seeded.usersCreated.length) console.log(`   حساب‌های نمونه ساخته شد: ${r.seeded.usersCreated.join(', ')}`)
    else console.log('   حساب کاربری جدیدی ساخته نشد (حساب‌های موجود حفظ شدند).')
    if (r.seeded.settingsPreserved.length) console.log(`   تنظیمات موجود حفظ شد: ${r.seeded.settingsPreserved.join(', ')}`)
    process.exit(0)
  }

  if (cmd === 'ensure-admin') {
    const { dbEnsureSuperAdmin } = await import('../src/lib/db-admin')
    const r = await dbEnsureSuperAdmin(db)
    if (!r.created) {
      console.log(`✔ حساب مدیر ارشد موجود است — نیازی به حساب اولیه نیست.`)
      process.exit(0)
    }
    console.log(`✅ حساب مدیر ارشد اولیه ساخته شد (نام کاربری: ${r.username})`)
    console.log(`   رمز اولیه در فایل «${r.passwordFile}» نوشته شده است — پس از اولین ورود آن را تغییر کنید.`)
    process.exit(0)
  }

  console.error('فرمان نامعتبر. استفاده: status | backup | wipe --yes | seed --yes | ensure-admin')
  process.exit(2)
}

main()
  .catch((e) => {
    console.error('خطا:', e instanceof Error ? e.message : e)
    process.exit(1)
  })
  .finally(() => db.$disconnect())
