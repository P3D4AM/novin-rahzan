/**
 * v1.8.1 test suite — INSTALL.bat structure & semantics (order + exit codes).
 * =====================================================================
 * INSTALL.bat is a Windows batch script; on Linux it cannot be EXECUTED, so
 * this suite pins its STRUCTURE: the order of the stages, the exit-code gate
 * after every stage, the numbering, the per-stage distinct error messages and
 * the absence of anything that could delete the user's database or .env.
 * The REAL end-to-end execution (install → generate → verify → db push →
 * re-run with preserved data) is covered by scripts/test-installer-real.sh
 * (Linux sandbox / CI) and the GitHub Actions installer-matrix (windows-latest
 * runs the real thing).
 *
 * The v1.8.0 regression this suite pins down: verify-native ran BEFORE
 * `prisma generate`, and a verify failure printed the generic "Fix the npm
 * scripts policy" advice even when the real cause was a Windows engine
 * filename the verifier did not recognise.
 *
 * Run: npx tsx scripts/test-installer-bat.ts
 */
import * as fs from 'fs'
import * as path from 'path'

const bat = fs.readFileSync(path.resolve(__dirname, '..', 'INSTALL.bat'), 'utf-8')

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}
const idx = (s: string) => bat.indexOf(s)

section('ترتیب صحیح مراحل (اصلاح اصلی v1.8.1)')
{
  const npmInstall = idx('call npm install --no-audit --no-fund --ignore-scripts=false')
  const prismaGenerate = idx('call npx prisma generate')
  const verify = idx('node scripts\\verify-native.js')
  const dbPush = idx('call npx prisma db push --skip-generate')
  const envWrite = idx('node scripts\\env-ensure.js')
  const build = idx('node scripts\\build-with-lock.js')
  check('همهٔ مراحل کلیدی در فایل وجود دارند', [npmInstall, prismaGenerate, verify, dbPush, envWrite, build].every((i) => i >= 0))
  check('npm install قبل از prisma generate است', npmInstall >= 0 && npmInstall < prismaGenerate)
  check('prisma generate قبل از verify-native است (باگ ترتیب v1.8.0 برطرف شد)', prismaGenerate >= 0 && prismaGenerate < verify, `generate=${prismaGenerate}, verify=${verify}`)
  check('verify-native قبل از db push است', verify >= 0 && verify < dbPush)
  check('db push قبل از نوشتن .env است', dbPush < envWrite)
  check('بیلد آخرین مرحلهٔ اجرایی است', build > envWrite)
}

section('بررسی Exit Code در هر مرحله')
{
  const stages: [string, string][] = [
    ['npm install', 'call npm install --no-audit --no-fund --ignore-scripts=false'],
    ['prisma generate', 'call npx prisma generate'],
    ['verify-native', 'node scripts\\verify-native.js'],
    ['db push', 'call npx prisma db push --skip-generate'],
    ['env-ensure', 'node scripts\\env-ensure.js'],
    ['build', 'node scripts\\build-with-lock.js'],
  ]
  for (const [label, cmd] of stages) {
    const at = idx(cmd)
    // the exit-code gate must appear right after the command (before the next stage label)
    const after = bat.slice(at + cmd.length, at + cmd.length + 700)
    check(`مرحلهٔ ${label} دارای دروازهٔ Exit Code است`, at >= 0 && /if errorlevel 1/.test(after), after.slice(0, 120).replace(/\n/g, ' '))
  }
}

section('شماره‌گذاری مراحل — پیوسته و بدون تکرار')
{
  const nums = [...bat.matchAll(/\[(\d)\/8\]/g)].map((m) => m[1])
  check('هشت مرحله شماره‌دار وجود دارند', nums.length === 8, JSON.stringify(nums))
  const uniq = new Set(nums)
  check('شماره‌ها منحصربه‌فردند (v1.8.0 دو بار [2/8] داشت)', uniq.size === 8, JSON.stringify(nums))
  const sortedOk = nums.every((n, i) => parseInt(n, 10) === i + 1)
  check('شماره‌ها به‌ترتیب ۱..۸ هستند', sortedOk, JSON.stringify(nums))
}

section('پیام‌های خطای اختصاصی هر مرحله (نه پیام کلی)')
{
  check('پیام اختصاصی شکست npm install', bat.includes('npm install failed (stage 2/8)'))
  check('پیام اختصاصی شکست Prisma Generate', bat.includes('Prisma Client generation failed (stage 3/8)'))
  check('پیام اختصاصی شکست Native Verification', bat.includes('Native dependency verification failed (stage 4/8)'))
  check('پیام اختصاصی شکست db push', bat.includes('Database schema creation failed (stage 5/8)'))
  check('پیام اختصاصی شکست بیلد', bat.includes('Production build failed (stage 8/8)'))
  check('پیام اختصاصی نبودن npm (بررسی Node و npm)', bat.includes('npm was not found on this computer'))
  check('پیام کلی «Fix the npm scripts policy» حذف شده است', !bat.includes('Fix the npm scripts policy'))
  check('پیام شکست verify به علت واقعی ارجاع می‌دهد (خطوط [X])', bat.includes('The [X] lines above state the EXACT missing item'))
}

section('حفاظت از دیتابیس و .env کاربر')
{
  const destructive = [...bat.matchAll(/^\s*(del|erase|rd|rmdir|format)\s+/gim)]
  check('هیچ دستور حذف فایل/پوشه‌ای در INSTALL.bat نیست', destructive.length === 0, JSON.stringify(destructive.map((m) => m[0])))
  check('دیتابیس موجود به‌طور پیش‌فرض نگه داشته می‌شود', bat.includes('your data is KEPT - nothing is deleted'))
  check('بازنشانی دیتابیس فقط با تأیید صریح کاربر', bat.includes('Reset the database and load fresh DEMO data? [y/N]'))
  check('.env با env-ensure نوشته می‌شود (حافظ متغیرهای موجود)', bat.includes('env-ensure.js "DATABASE_URL=!DBURL!"'))
  check('env-ensure با حفظ متغیرها مستند شده است', bat.includes('existing variables and secrets are preserved'))
  check('db push با --skip-generate (تولید جدا در مرحلهٔ ۳)', bat.includes('db push --skip-generate'))
}

section('بررسی Node و npm قبل از نصب (مرحلهٔ ۱/۸)')
{
  check('بررسی وجود node', bat.includes('where node >nul'))
  check('بررسی نسخهٔ node ≥ 20', bat.includes('NODEMAJ! LSS 20'))
  check('بررسی وجود npm', bat.includes('where npm >nul'))
  check('نمایش نسخهٔ npm', bat.includes('npm -v'))
}

section('ارجاع به مستند allowScripts')
{
  check('INSTALL.bat به مستند allowScripts ارجاع می‌دهد', bat.includes('docs/INSTALLER-NPM-SCRIPTS.md'))
  check('از --dangerously-allow-all-scripts استفاده نمی‌شود', !bat.includes('--dangerously-allow-all-scripts'))
}

console.log('\n══════════════════════════════════════════')
console.log(`مجموعهٔ installer-bat: ${passed} تایید شد، ${failed} شکست خورد`)
if (failures.length) {
  console.log('شکست‌ها:')
  for (const f of failures) console.log('  ✘ ' + f)
}
console.log('══════════════════════════════════════════')
process.exit(failed > 0 ? 1 : 0)
