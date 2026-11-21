#!/usr/bin/env node
/**
 * HozorPlus v1.8.1 — native dependency verification, comprehensive rewrite.
 * =====================================================================
 * WHY THIS REWRITE EXISTS (v1.8.0 install failure, root cause):
 *   The old verifier only recognised Prisma engine files whose name matched
 *   /libquery_engine/. On a REAL Windows install with Prisma 6.x the engine
 *   file is named `query_engine-windows.dll.node` (no `lib` prefix — the
 *   `libquery_engine-*` naming only applies to Linux targets). A perfectly
 *   successful install was therefore reported as FAILED. Additionally
 *   INSTALL.bat ran this verifier BEFORE `prisma generate`, so even the
 *   generated-client check could not pass. Both defects are fixed here and
 *   in INSTALL.bat (v1.8.1: generate runs first, then this verifier).
 *
 * WHAT IT VERIFIES (per REAL platform + architecture, not a fixed filename):
 *   Prisma (CRITICAL — the app cannot talk to SQLite without it):
 *     * the query engine file expected for {platform, arch}:
 *         win32  → query_engine-windows.dll.node   (also legacy libquery_engine-windows*.dll.node)
 *         linux  → libquery_engine-<target>.so.node (debian/rhel/musl/arch/… any distro target)
 *         darwin → libquery_engine-darwin[-arm64].dylib.node
 *       …with an ARCH guard: an arm64-named engine never satisfies x64.
 *     * the engine is looked for in BOTH real locations:
 *         node_modules/.prisma/client/  (copied there by `prisma generate` — required at runtime)
 *         node_modules/@prisma/engines/  (shipped by the npm package — required by the CLI)
 *     * the generated client (index.js / default.js) at the REAL generator
 *       output path parsed from prisma/schema.prisma (`output = "..."`, default
 *       node_modules/.prisma/client).
 *     * a found file must be NON-EMPTY and READABLE — a 0-byte engine means a
 *       truncated download and is reported as such.
 *     * version compatibility between prisma CLI / @prisma/client / @prisma/engines
 *       / the generated client (mismatch = WARNING, not a hard failure).
 *   esbuild binary (CRITICAL for the toolchain: `npx tsx scripts/db-admin.ts`
 *   runs during INSTALL.bat), Next.js SWC binding (warning — WASM fallback),
 *   sharp platform lib (warning — used by next/image), npm scripts policy.
 *
 * MESSAGES: every failure names the EXACT missing item and its expected
 * location/pattern. Errors are NOT funnelled into a single "run prisma
 * generate" sentence — a missing engine file, an empty engine file, an
 * unreadable file, a wrong-architecture engine and a missing generated
 * client each get their own precise Persian message.
 *
 * Exit codes: 0 = ok or warnings only; 1 = at least one critical item missing.
 *
 * CLI:   node scripts/verify-native.js [--root <path>] [--json]
 * API (used by scripts/test-verify-native.ts):
 *   const { verify } = require('./verify-native')
 *   verify({ root, platform, arch, skipNpm }) →
 *     { ok, critical, platform, arch, root, lines: [{level,text}], prisma: {…} }
 */
'use strict'
const fs = require('fs')
const path = require('path')
const { spawnSync } = require('child_process')

/* ------------------------------------------------------------------ */
/* Prisma engine naming — derived from the REAL file layout of Prisma  */
/* 6.x per platform. `lib` prefix exists ONLY on Unix targets.         */
/* ------------------------------------------------------------------ */

/**
 * Patterns for the query-engine file of a given platform. Each pattern is a
 * RegExp tested against directory entry names.
 * @param {string} platform win32 | linux | darwin
 * @param {string} arch x64 | arm64
 */
function prismaEnginePatterns(platform, arch) {
  if (platform === 'win32') {
    // Prisma 6.x on Windows (verified on a real user machine, Prisma 6.19.3):
    //   query_engine-windows.dll.node
    // very old Prisma also used libquery_engine-windows.dll.node — covered too.
    const pats = [
      /^query_engine-windows[^/]*\.dll\.node$/,
      /^libquery_engine-windows[^/]*\.dll\.node$/,
    ]
    return { patterns: pats, expected: 'query_engine-windows.dll.node' }
  }
  if (platform === 'linux') {
    // libquery_engine-debian-openssl-3.0.x.so.node, …-linux-musl-…, rhel, arch,
    // alpine, gentoo, nixos … — accept any distro target, arch-guarded later.
    return { patterns: [/^libquery_engine-[^/]*\.so\.node$/], expected: 'libquery_engine-<distro>.so.node (e.g. libquery_engine-debian-openssl-3.0.x.so.node)' }
  }
  if (platform === 'darwin') {
    // x64: libquery_engine-darwin.dylib.node
    // Apple Silicon: libquery_engine-darwin-arm64.dylib.node (plain darwin
    // also accepted on arm64 — Rosetta).
    return { patterns: [/^libquery_engine-darwin[^/]*\.dylib\.node$/], expected: 'libquery_engine-darwin.dylib.node (arm64: libquery_engine-darwin-arm64.dylib.node)' }
  }
  return { patterns: [], expected: `نامشخص برای پلتفرم ${platform}` }
}

/**
 * Arch guard: does this engine FILENAME look compatible with {arch}?
 * x64  → file must NOT carry an arm64/aarch64 (or other non-x64 arch) marker
 * arm64 → arm64/aarch64 marker preferred; an unmarked file is accepted with a
 *         note (cannot infer arch from every distro target name).
 */
function engineArchCompatible(name, arch) {
  const hasArmMarker = /arm64|aarch64/i.test(name)
  const hasOtherArch = /armv[67]|riscv64|ppc64|s390x|loong64|mips/i.test(name)
  if (arch === 'x64') return !hasArmMarker && !hasOtherArch
  if (arch === 'arm64') return hasArmMarker || (!hasArmMarker && !hasOtherArch)
  return true
}

function listDir(dir) {
  try { return fs.readdirSync(dir) } catch { return null }
}

function fileUsable(p) {
  try {
    const st = fs.statSync(p)
    if (!st.isFile()) return { ok: false, why: 'not-file' }
    if (st.size <= 0) return { ok: false, why: 'empty', size: st.size }
    fs.accessSync(p, fs.constants.R_OK)
    return { ok: true, size: st.size }
  } catch (e) {
    return { ok: false, why: 'unreadable', error: e.message }
  }
}

/** Find engine candidates in one directory for a platform/arch. */
function findEnginesIn(dir, platform, arch) {
  const names = listDir(dir)
  if (!names) return { dirExists: false, matched: [], wrongArch: [] }
  const { patterns } = prismaEnginePatterns(platform, arch)
  const matched = []
  const wrongArch = []
  for (const f of names) {
    if (patterns.some((re) => re.test(f))) {
      ;(engineArchCompatible(f, arch) ? matched : wrongArch).push(f)
    }
  }
  return { dirExists: true, matched, wrongArch, allNames: names }
}

/** Parse the generator output path out of prisma/schema.prisma (default .prisma/client). */
function generatorOutput(root) {
  const schemaPath = path.join(root, 'prisma', 'schema.prisma')
  let output = null
  let generatorFound = false
  try {
    const text = fs.readFileSync(schemaPath, 'utf-8')
    const m = text.match(/generator\s+client\s*\{[^}]*\}/s)
    if (m) {
      generatorFound = true
      const om = m[0].match(/output\s*=\s*"([^"]+)"/)
      if (om) output = om[1].replace(/\\(.)/g, '$1') // unescape \" and windows backslashes
    }
  } catch { /* schema unreadable → defaults below */ }
  if (!output) output = path.join('node_modules', '.prisma', 'client')
  return {
    generatorFound,
    schemaPath,
    output,
    outputAbs: path.isAbsolute(output) ? output : path.join(root, output),
  }
}

/* ------------------------------------------------------------------ */
/* Main verification — pure-ish: takes {root, platform, arch} so tests  */
/* can drive FAKE trees and FAKE platforms without touching process.*   */
/* ------------------------------------------------------------------ */

/**
 * @param {{root?: string, platform?: string, arch?: string, skipNpm?: boolean, skipEsbuild?: boolean, skipSharp?: boolean, skipSwc?: boolean}} opts
 */
function verify(opts) {
  const root = path.resolve(opts.root || path.resolve(__dirname, '..'))
  const platform = opts.platform || process.platform
  const arch = opts.arch || process.arch
  const nm = (p) => path.join(root, 'node_modules', p)

  const R = {
    ok: true, critical: false,
    root, platform, arch,
    lines: [],
    prisma: { enginesDir: nm('@prisma/engines'), clientDir: null, engineFile: null, engineLocations: [], versionMismatch: false, missing: [] },
    esbuild: {}, swc: {}, sharp: {},
  }
  const ok = (msg) => R.lines.push({ level: 'ok', text: msg })
  const warn = (msg) => R.lines.push({ level: 'warn', text: msg })
  const bad = (msg) => { R.lines.push({ level: 'bad', text: msg }); R.critical = true; R.ok = false }

  ok(`پلتفرم: ${platform}-${arch}`)

  /* ---- npm scripts policy ------------------------------------------- */
  if (!opts.skipNpm) {
    try {
      const r = spawnSync(process.platform === 'win32' ? 'npm.cmd' : 'npm', ['config', 'get', 'ignore-scripts'], { encoding: 'utf-8', timeout: 30_000 })
      const v = String(r.stdout || '').trim()
      if (v === 'true') {
        bad('تنظیم npm «ignore-scripts=true» فعال است — اسکریپت‌های نصب اجرا نشده‌اند. با «npm config delete ignore-scripts» و سپس «npm install» اصلاح کنید.')
      } else {
        ok(`سیاست npm ignore-scripts = ${v || 'false'}`)
      }
      // npm >= 11: the allowScripts field in package.json decides which
      // dependency install scripts are trusted (advisory in 11, enforced in 12).
      try {
        const pkg = JSON.parse(fs.readFileSync(path.join(root, 'package.json'), 'utf-8'))
        if (pkg.allowScripts && Object.keys(pkg.allowScripts).length) {
          const allowed = Object.keys(pkg.allowScripts).filter((k) => pkg.allowScripts[k])
          ok(`allowScripts در package.json ثبت شده است (${allowed.length} بستهٔ مجاز)`)
        } else if (process.versions.npm && parseInt(process.versions.npm, 10) >= 12) {
          warn('npm ≥ 12 بدون فیلد allowScripts — اسکریپت‌های نصب وابسته‌ها به‌صورت پیش‌فرض مسدود می‌شوند.')
        }
      } catch { /* no package.json → skip */ }
    } catch (e) {
      warn('بررسی تنظیم npm ممکن نشد: ' + e.message)
    }
  }

  /* ---- Prisma query engine + generated client (CRITICAL) ------------ */
  const gen = generatorOutput(root)
  R.prisma.clientDir = gen.outputAbs
  const enginesDir = nm('@prisma/engines')
  const clientDir = gen.outputAbs
  const { expected } = prismaEnginePatterns(platform, arch)

  // location 1: the npm package @prisma/engines
  const eng = findEnginesIn(enginesDir, platform, arch)
  // location 2: the generated client dir (engine copied by `prisma generate`)
  const cli = findEnginesIn(clientDir, platform, arch)

  const enginesPkgExists = fs.existsSync(path.join(enginesDir, 'package.json'))

  // -- generated client presence (precise, separate from the engine)
  const clientIndex = path.join(clientDir, 'index.js')
  const clientDefault = path.join(clientDir, 'default.js')
  const clientHasEntry = fs.existsSync(clientIndex) || fs.existsSync(clientDefault)
  if (clientHasEntry) {
    ok(`کلاینت تولیدشدهٔ Prisma موجود است (${gen.output}${path.sep}index.js)`)
  } else if (gen.generatorFound === false && !fs.existsSync(gen.schemaPath)) {
    warn(`prisma/schema.prisma پیدا نشد — مسیر کلاینت از پیش‌فرض فرض شد: ${gen.output}`)
  } else {
    bad(`کلاینت تولیدشدهٔ Prisma در مسیر واقعی generator یافت نشد: ${gen.output} (فایل index.js/default.js موجود نیست) — «npx prisma generate» باید اجرا شود.`)
    R.prisma.missing.push('generated-client')
  }

  // -- engine in the generated client dir (required at runtime)
  let engineFileInClient = null
  if (cli.dirExists) {
    if (cli.matched.length) {
      const usable = fileUsable(path.join(clientDir, cli.matched[0]))
      if (usable.ok) { engineFileInClient = cli.matched[0]; R.prisma.engineLocations.push(`.prisma/client/${cli.matched[0]} (${usable.size} bytes)`) }
      else if (usable.why === 'empty') bad(`فایل موتور Prisma در پوشهٔ کلاینت تولیدشده موجود اما «خالی/صفر بایت» است: ${cli.matched[0]} — نصب ناقص/قطع‌شده؛ «npm install» را دوباره اجرا کنید.`)
      else bad(`فایل موتور Prisma در پوشهٔ کلاینت تولیدشده قابل خواندن نیست: ${cli.matched[0]} (${usable.error}).`)
    } else if (cli.wrongArch.length) {
      bad(`موتور Prisma یافت‌شده در پوشهٔ کلاینت برای معماری دیگری است (${cli.wrongArch[0]} در حالی که معماری فعلی ${arch} است) — وابستگی اشتباه نصب شده است.`)
      R.prisma.missing.push('engine-arch')
    }
  }

  // -- engine in @prisma/engines (the npm package location)
  let engineFileInEngines = null
  if (!enginesPkgExists) {
    bad('بستهٔ @prisma/engines در node_modules وجود ندارد — «npm install» ناقص اجرا شده است.')
    R.prisma.missing.push('engines-package')
  } else if (!eng.dirExists) {
    bad(`پوشهٔ بستهٔ @prisma/engines یافت نشد (${enginesDir}) — نصب npm ناقص است.`)
    R.prisma.missing.push('engines-dir')
  } else if (eng.matched.length) {
    const usable = fileUsable(path.join(enginesDir, eng.matched[0]))
    if (usable.ok) { engineFileInEngines = eng.matched[0]; R.prisma.engineLocations.push(`@prisma/engines/${eng.matched[0]} (${usable.size} bytes)`) }
    else if (usable.why === 'empty') bad(`فایل موتور Prisma در @prisma/engines موجود اما «خالی/صفر بایت» است: ${eng.matched[0]} — دانلود/کپی ناقص؛ «npm install» را دوباره اجرا کنید.`)
    else bad(`فایل موتور Prisma در @prisma/engines قابل خواندن نیست: ${eng.matched[0]} (${usable.error}).`)
  } else if (eng.wrongArch.length) {
    bad(`موتور Prisma یافت‌شده در @prisma/engines برای معماری دیگری است (${eng.wrongArch[0]}؛ معماری فعلی ${arch}) — وابستگی اشتباه نصب شده است.`)
    R.prisma.missing.push('engine-arch')
  } else {
    // dir exists, package exists, but NO engine file at all — the v1.8.0 bug
    // class: report what IS there so the user can see the real situation.
    const present = (eng.allNames || []).slice(0, 8)
    bad(`فایل موتور کوئری Prisma برای پلتفرم ${platform}-${arch} در @prisma/engines یافت نشد. نام مورد انتظار: «${expected}». فایل‌های موجود در پوشه: ${present.length ? present.join('، ') : '(هیچ)'}`)
    R.prisma.missing.push('engine-file')
  }

  R.prisma.engineFile = engineFileInClient || engineFileInEngines || null

  if (clientHasEntry && engineFileInClient && engineFileInEngines) {
    ok(`موتور کوئری Prisma موجود و سالم است — ${R.prisma.engineLocations.join(' و ')}`)
  } else if (clientHasEntry && engineFileInEngines && !engineFileInClient) {
    // engine only in the npm package, not copied into the generated client →
    // `prisma generate` did not finish its engine copy step
    bad(`موتور در @prisma/engines موجود است اما در پوشهٔ کلاینت تولیدشده (${gen.output}) کپی نشده است — «npx prisma generate» کامل اجرا نشده؛ دوباره اجرا کنید.`)
    R.prisma.missing.push('engine-in-client')
  } else if (!clientHasEntry && (engineFileInClient || engineFileInEngines)) {
    // already flagged by the client-entry check above
  }

  // -- Prisma CLI ↔ Client ↔ engines version compatibility (warning-level)
  try {
    const vPkg = (p) => { try { return JSON.parse(fs.readFileSync(nm(p), 'utf-8')).version } catch { return null } }
    const versions = {
      'prisma (CLI)': vPkg('prisma/package.json'),
      '@prisma/client': vPkg('@prisma/client/package.json'),
      '@prisma/engines': vPkg('@prisma/engines/package.json'),
      'generated client': (() => { try { return JSON.parse(fs.readFileSync(path.join(clientDir, 'package.json'), 'utf-8')).version } catch { return null } })(),
    }
    const present = Object.entries(versions).filter(([, v]) => v)
    const majMin = (v) => v.split('.').slice(0, 2).join('.')
    const first = present.length ? majMin(present[0][1]) : null
    const mismatched = present.filter(([, v]) => majMin(v) !== first)
    if (present.length >= 2 && mismatched.length) {
      R.prisma.versionMismatch = true
      warn(`نسخه‌های Prisma هم‌راستا نیستند: ${present.map(([k, v]) => `${k}=${v}`).join('، ')} — اختلاف نسخهٔ major/minor می‌تواند خطای «engine does not match» تولید کند.`)
    } else if (present.length >= 2) {
      ok(`نسخه‌های Prisma سازگارند (${present.map(([k, v]) => `${k}=${v}`).join('، ')})`)
    }
  } catch { /* version check best-effort */ }

  // -- schema engine (needed by `prisma db push` — the next INSTALL step)
  if (enginesPkgExists && eng.dirExists) {
    const schemaEngine = (eng.allNames || []).find((f) => /^schema-engine-/.test(f))
    if (schemaEngine) ok(`موتور Schema (برای prisma db push) موجود است: ${schemaEngine}`)
    else warn('موتور schema-engine در @prisma/engines یافت نشد — مرحلهٔ «prisma db push» ممکن است با خطا مواجه شود.')
  }

  /* ---- esbuild binary (CRITICAL for tsx) ---------------------------- */
  if (!opts.skipEsbuild) {
    const esbuildPkgs = {
      win32: { x64: '@esbuild/win32-x64', arm64: '@esbuild/win32-arm64' },
      linux: { x64: '@esbuild/linux-x64', arm64: '@esbuild/linux-arm64' },
      darwin: { x64: '@esbuild/darwin-x64', arm64: '@esbuild/darwin-arm64' },
    }
    const esbuildPkg = esbuildPkgs[platform]?.[arch]
    const esbuildMain = nm('esbuild/lib/main.js')
    R.esbuild = { pkg: esbuildPkg, mainExists: fs.existsSync(esbuildMain) }
    if (esbuildPkg && fs.existsSync(nm(esbuildPkg))) {
      // platform binary layout differs: linux/darwin put it in <pkg>/bin/esbuild,
      // the windows package puts esbuild.exe at the PACKAGE ROOT (verified on a
      // real windows-latest CI runner — that asymmetry was the last red leg).
      const binCandidates = [
        path.join(nm(esbuildPkg), 'bin', platform === 'win32' ? 'esbuild.exe' : 'esbuild'),
        path.join(nm(esbuildPkg), 'esbuild.exe'),
      ]
      const usable = binCandidates.map(fileUsable).find((u) => u && u.ok)
      if (usable) ok(`باینری esbuild موجود و سالم است (${esbuildPkg}، ${usable.size} bytes)`)
      else bad(`بستهٔ ${esbuildPkg} نصب است اما فایل باینری آن خالی/غیرقابل‌خواندن است — «npm install» را دوباره اجرا کنید.`)
    } else if (R.esbuild.mainExists) {
      warn('esbuild نصب است اما بستهٔ باینری پلتفرم یافت نشد — اگر خطای اجرا دیدید «npm install @esbuild/' + `${platform}-x64` + ' --force» (یا معادل پلتفرم) اجرا کنید.')
    } else {
      bad('esbuild نصب نشده است — «npm install» را دوباره اجرا کنید.')
    }
    if (!esbuildPkg && R.esbuild.mainExists) warn(`پلتفرم ${platform}-${arch} در فهرست بررسی esbuild نیست`)
  }

  /* ---- Next.js SWC binding (warning) -------------------------------- */
  if (!opts.skipSwc) {
    const swcCandidates = [`@next/swc-${platform}-${arch}`, `@next/swc-${platform}-${arch}-gnu`, `@next/swc-${platform}-${arch}-msvc`]
    const swcFound = swcCandidates.some((p) => fs.existsSync(nm(p)))
    R.swc = { found: swcFound }
    if (swcFound) ok('بایندینگ SWC مربوط به Next موجود است')
    else warn('بایندینگ بومی SWC یافت نشد — Next از حالت WASM (کندتر) استفاده خواهد کرد.')
  }

  /* ---- sharp (warning) ------------------------------------------------ */
  if (!opts.skipSharp) {
    const sharpPkg = {
      win32: { x64: '@img/sharp-win32-x64', arm64: '@img/sharp-win32-arm64' },
      linux: { x64: '@img/sharp-linux-x64', arm64: '@img/sharp-linux-arm64' },
      darwin: { x64: '@img/sharp-darwin-x64', arm64: '@img/sharp-darwin-arm64' },
    }[platform]?.[arch]
    R.sharp = { pkg: sharpPkg, found: !!(sharpPkg && fs.existsSync(nm(sharpPkg))) }
    if (R.sharp.found) ok(`کتابخانهٔ sharp برای پردازش تصویر موجود است (${sharpPkg})`)
    else warn('sharp برای پلتفرم نصب نیست — پردازش تصویر (اگر استفاده شود) ممکن است کندتر باشد.')
  }

  if (R.critical) R.ok = false
  return R
}

/* ------------------------------------------------------------------ */
/* CLI                                                                  */
/* ------------------------------------------------------------------ */

function main() {
  const argv = process.argv.slice(2)
  const rootIdx = argv.indexOf('--root')
  const root = rootIdx !== -1 && argv[rootIdx + 1] ? path.resolve(argv[rootIdx + 1]) : undefined
  const json = argv.includes('--json')
  const R = verify({ root })

  if (json) {
    console.log(JSON.stringify(R, null, 2))
  } else {
    for (const l of R.lines) {
      const tag = l.level === 'ok' ? '  [OK]  ' : l.level === 'warn' ? '  [!]   ' : '  [X]   '
      console.log(tag + l.text)
    }
    if (R.critical) console.log('[verify-native] نتیجه: وابستگی‌های حیاتی ناقص‌اند — پیام‌های [X] بالا علت دقیق هر مورد را مشخص می‌کنند.')
    else console.log('[verify-native] نتیجه: وابستگی‌های حیاتی سالم‌اند (هشدارها اختیاری‌اند).')
  }
  process.exit(R.critical ? 1 : 0)
}

if (require.main === module) main()

module.exports = { verify, prismaEnginePatterns, engineArchCompatible, findEnginesIn, generatorOutput, fileUsable }
