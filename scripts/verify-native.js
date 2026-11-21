#!/usr/bin/env node
/**
 * Phase 1 (v1.8.0-dev) — native dependency verification after npm install.
 * =====================================================================
 * The npm `allowScripts`/`ignore-scripts` policy (user/global .npmrc) can
 * silently skip postinstall steps, leaving native packages broken. This
 * helper verifies the platform bindings the app actually needs and reports
 * in Persian. It also checks the live npm config and warns when scripts are
 * disabled.
 *
 *   * Prisma query engine (CRITICAL — the app cannot talk to SQLite without it)
 *   * esbuild binary (CRITICAL for the toolchain used by scripts)
 *   * Next.js SWC binding (warning — Next has a WASM fallback, slower)
 *   * sharp platform lib (warning — used by next/image in production)
 *
 * Exit code: 0 = ok or warnings only; 1 = a critical piece is missing.
 */
'use strict'
const fs = require('fs')
const path = require('path')
const { spawnSync } = require('child_process')

const root = path.resolve(__dirname, '..')
const nm = (p) => path.join(root, 'node_modules', p)
const exists = (p) => { try { fs.accessSync(p); return true } catch { return false } }

const platform = process.platform // win32 | linux | darwin
const arch = process.arch // x64 | arm64
let critical = false

function ok(msg) { console.log('  [OK]  ' + msg) }
function warn(msg) { console.log('  [!]   ' + msg) }
function bad(msg) { console.log('  [X]   ' + msg); critical = true }

console.log(`[verify-native] پلتفرم: ${platform}-${arch}`)

/* npm scripts policy ------------------------------------------------- */
try {
  const r = spawnSync(process.platform === 'win32' ? 'npm.cmd' : 'npm', ['config', 'get', 'ignore-scripts'], { encoding: 'utf-8', timeout: 30_000 })
  const v = String(r.stdout || '').trim()
  if (v === 'true') {
    bad('تنظیم npm «ignore-scripts=true» فعال است — اسکریپت‌های نصب اجرا نشده‌اند. با «npm config delete ignore-scripts» و سپس «npm install» اصلاح کنید.')
  } else {
    ok(`سیاست npm ignore-scripts = ${v || 'false'}`)
  }
} catch (e) {
  warn('بررسی تنظیم npm ممکن نشد: ' + e.message)
}

/* Prisma query engine (critical) -------------------------------------- */
const enginesDir = nm('@prisma/engines')
const prismaEngineOk = exists(enginesDir) && fs.readdirSync(enginesDir).some((f) => /libquery_engine/.test(f))
const generatedClient = nm('.prisma/client')
if (prismaEngineOk && exists(generatedClient)) ok('موتور کوئری Prisma موجود است')
else {
  bad('موتور کوئری Prisma یا کلاینت تولیدشده یافت نشد — «npx prisma generate» را اجرا کنید (نصب ناقص است).')
}

/* esbuild binary (critical for the toolchain) ------------------------ */
const esbuildPkgs = {
  win32: { x64: '@esbuild/win32-x64' },
  linux: { x64: '@esbuild/linux-x64', arm64: '@esbuild/linux-arm64' },
  darwin: { x64: '@esbuild/darwin-x64', arm64: '@esbuild/darwin-arm64' },
}
const esbuildPkg = esbuildPkgs[platform]?.[arch]
const esbuildMain = nm('esbuild/lib/main.js')
if (esbuildPkg && exists(nm(esbuildPkg))) ok(`باینری esbuild موجود است (${esbuildPkg})`)
else if (exists(esbuildMain)) warn('esbuild نصب است اما باینری پلتفرم یافت نشد — اگر خطای اجرا دیدید «npm install @esbuild/linux-x64 --force» (یا معادل پلتفرم) اجرا کنید.')
else { bad('esbuild نصب نشده است — «npm install» را دوباره اجرا کنید.'); critical = true }
if (!esbuildPkg && exists(esbuildMain)) warn(`پلتفرم ${platform}-${arch} در فهرست بررسی esbuild نیست`)

/* Next.js SWC (warning) ------------------------------------------------ */
const swcCandidates = [`@next/swc-${platform}-${arch}`, `@next/swc-${platform}-${arch}-gnu`, `@next/swc-${platform}-${arch}-msvc`]
const swcFound = swcCandidates.some((p) => exists(nm(p)))
if (swcFound) ok('بایندینگ SWC مربوط به Next موجود است')
else warn('بایندینگ بومی SWC یافت نشد — Next از حالت WASM (کندتر) استفاده خواهد کرد. اگر Build خیلی کند بود، نصب را با اسکریپت‌های فعال تکرار کنید.')

/* sharp (warning) ------------------------------------------------------ */
const sharpPkg = {
  win32: '@img/sharp-win32-x64',
  linux: '@img/sharp-libvips-linux-x64',
}[platform]
if (sharpPkg && exists(nm(sharpPkg))) ok(`کتابخانهٔ sharp برای پردازش تصویر موجود است (${sharpPkg})`)
else warn('sharp برای پلتفرم نصب نیست — پردازش تصویر (اگر استفاده شود) ممکن است کندتر باشد.')

if (critical) {
  console.log('[verify-native] نتیجه: وابستگی‌های حیاتی ناقص‌اند — نصب را اصلاح کنید.')
  process.exit(1)
}
console.log('[verify-native] نتیجه: وابستگی‌های حیاتی سالم‌اند (هشدارها اختیاری‌اند).')
process.exit(0)
