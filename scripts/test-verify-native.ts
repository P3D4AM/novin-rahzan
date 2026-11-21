/**
 * v1.8.1 test suite — scripts/verify-native.js (Prisma engine verification).
 * =====================================================================
 * Everything here drives the REAL verifier module (the exact code INSTALL.bat
 * stage 4/8 runs) against constructed node_modules trees — fake TREES, real
 * CODE: no mocking of the verifier's logic. npm-policy checks are skipped
 * (skipNpm) so the suite is hermetic; the CLI-level section spawns the real
 * script as a child process exactly like INSTALL.bat does.
 *
 * The v1.8.0 regression this suite pins down: a HEALTHY Windows install
 * (Prisma 6.x, engine file `query_engine-windows.dll.node`) was reported as
 * broken because the old verifier only matched /libquery_engine/ names.
 *
 * Run: npx tsx scripts/test-verify-native.ts
 */
import * as fs from 'fs'
import * as path from 'path'
import * as os from 'os'
import { spawnSync } from 'child_process'

/* The verifier is a plain CommonJS script that runs on the user's machine with
 * bare `node` (no build step) — importing it via require keeps it outside
 * tsc's module graph while exercising the real runtime code. */
// eslint-disable-next-line @typescript-eslint/no-require-imports -- CJS sibling script, see comment above
const vn = require('../scripts/verify-native') as {
  verify: (o: {
    root?: string
    platform?: string
    arch?: string
    skipNpm?: boolean
    skipEsbuild?: boolean
    skipSharp?: boolean
    skipSwc?: boolean
  }) => VerifyResult
  prismaEnginePatterns: (platform: string, arch: string) => { patterns: RegExp[]; expected: string }
  engineArchCompatible: (name: string, arch: string) => boolean
}
type VerifyResult = {
  ok: boolean
  critical: boolean
  root: string
  platform: string
  arch: string
  lines: { level: 'ok' | 'warn' | 'bad'; text: string }[]
  prisma: { enginesDir: string; clientDir: string | null; engineFile: string | null; engineLocations: string[]; versionMismatch: boolean; missing: string[] }
}

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}
const badLines = (r: VerifyResult) => r.lines.filter((l) => l.level === 'bad').map((l) => l.text)
const badText = (r: VerifyResult) => badLines(r).join(' | ')

/* ---------- tree builder: a node_modules skeleton in a temp dir ---------- */
function mkTree(files: Record<string, string | Buffer>): string {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'vn-test-'))
  for (const [rel, content] of Object.entries(files)) {
    const p = path.join(root, rel)
    fs.mkdirSync(path.dirname(p), { recursive: true })
    fs.writeFileSync(p, content)
  }
  return root
}
const engine = (name: string, size = 4096) => Buffer.alloc(Math.max(size, 1), 7)
const schemaDefault = `generator client {\n  provider = "prisma-client-js"\n}\n\ndatasource db {\n  provider = "sqlite"\n  url      = env("DATABASE_URL")\n}\n`
const clientJS = 'module.exports = {} // generated client stub\n'
const pkgAt = (rel: string, version: string) => JSON.stringify({ name: path.basename(rel), version })

/* a HEALTHY Windows tree, byte-for-byte the structure the real user machine
 * had (Prisma 6.19.3, node_modules/.prisma/client + @prisma/engines) */
function healthyWinTree() {
  return mkTree({
    'package.json': JSON.stringify({ name: 't', version: '1.0.0', allowScripts: { prisma: true } }),
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.3'),
    'node_modules/@prisma/engines/README.md': 'engines',
    'node_modules/@prisma/engines/query_engine-windows.dll.node': engine('q'),
    'node_modules/@prisma/engines/schema-engine-windows.exe': engine('s'),
    'node_modules/.prisma/client/package.json': pkgAt('.prisma/client', '6.19.3'),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/default.js': clientJS,
    'node_modules/.prisma/client/query_engine-windows.dll.node': engine('q'),
  })
}

section('win32-x64 — ساختار واقعی ماشین کاربر (باگ v1.8.0)')
{
  const root = healthyWinTree()
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('نصب سالم → نتیجه ok (این همان کیسی است که v1.8.0 به‌اشتباه شکست می‌داد)', r.ok === true, `bad lines: ${badText(r)}`)
  check('هیچ قلم مفقودی نیست', r.prisma.missing.length === 0, JSON.stringify(r.prisma.missing))
  check('هر دو محل موتور تشخیص داده شدند', r.prisma.engineLocations.length === 2, JSON.stringify(r.prisma.engineLocations))
  check('فایل موتور = query_engine-windows.dll.node', r.prisma.engineFile === 'query_engine-windows.dll.node', String(r.prisma.engineFile))
  check('مسیر کلاینت تولیدشده = node_modules/.prisma/client', r.prisma.clientDir === path.join(root, 'node_modules', '.prisma', 'client'), String(r.prisma.clientDir))
  check('موتور schema هم گزارش می‌شود', r.lines.some((l) => l.text.includes('schema-engine-windows.exe')))
}

section('win32-x64 — کلاینت تولیدشده ناموجود (موتور سالم)')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/query_engine-windows.dll.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('شکست گزارش می‌شود', r.critical === true)
  check('دقیقاً قلم generated-client مفقود است', r.prisma.missing.includes('generated-client'), JSON.stringify(r.prisma.missing))
  check('پیام اختصاصی کلاینت (نه پیام کلی)', badText(r).includes('prisma generate') && badText(r).includes('index.js'))
  check('پیام، مسیر واقعی generator را می‌گوید', badText(r).includes('.prisma/client'))
}

section('win32-x64 — پوشهٔ engines موجود اما بدون فایل موتور')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/README.md': 'only docs',
    'node_modules/@prisma/engines/LICENSE': 'MIT',
    'node_modules/.prisma/client/index.js': clientJS,
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('شکست گزارش می‌شود', r.critical === true)
  check('دقیقاً قلم engine-file مفقود است', r.prisma.missing.includes('engine-file'), JSON.stringify(r.prisma.missing))
  check('پیام، نام مورد انتظار ویندوزی را می‌گوید', badText(r).includes('query_engine-windows.dll.node'), badText(r))
  check('پیام می‌گوید چه فایل‌هایی آنجا هست (نه فقط «generate اجرا کن»)', badText(r).includes('README.md'))
}

section('win32-x64 — فایل موتور خالی (صفر بایت)')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/query_engine-windows.dll.node': Buffer.alloc(0),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/query_engine-windows.dll.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('فایل خالی در @prisma/engines → شکست', r.critical === true)
  check('پیام «خالی/صفر بایت»', badText(r).includes('خالی'), badText(r))
}

section('win32-x64 — موتور فقط در @prisma/engines، در کلاینت کپی نشده')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/query_engine-windows.dll.node': engine('q'),
    'node_modules/.prisma/client/index.js': clientJS,
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('شکست گزارش می‌شود', r.critical === true)
  check('دقیقاً قلم engine-in-client مفقود است', r.prisma.missing.includes('engine-in-client'), JSON.stringify(r.prisma.missing))
  check('پیام می‌گوید generate کامل اجرا نشده', badText(r).includes('کامل اجرا نشده'))
}

section('linux-x64 — نام موتور debian (محیط واقعی توسعه)')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/libquery_engine-debian-openssl-3.0.x.so.node': engine('q'),
    'node_modules/@prisma/engines/schema-engine-debian-openssl-3.0.x': engine('s'),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/libquery_engine-debian-openssl-3.0.x.so.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'linux', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('پذیرش نام linux (پیشوند lib + .so.node)', r.ok === true, badText(r))
  check('هر دو محل موتور', r.prisma.engineLocations.length === 2, JSON.stringify(r.prisma.engineLocations))
}

section('linux-x64 — نام موتور musl/alpine هم معتبر است')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/libquery_engine-linux-musl-openssl-3.0.x.so.node': engine('q'),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/libquery_engine-linux-musl-openssl-3.0.x.so.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'linux', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('پذیرش target موتور musl', r.ok === true, badText(r))
}

section('linux-x64 — فقط موتور arm64 موجود (معماری اشتباه)')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/libquery_engine-debian-openssl-3.0.x-arm64.so.node': engine('q'),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/libquery_engine-debian-openssl-3.0.x-arm64.so.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'linux', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('موتور arm64 روی x64 رد می‌شود', r.critical === true)
  check('پیام عدم تطابق معماری', badText(r).includes('معماری'), badText(r))
  check('قلم engine-arch ثبت می‌شود', r.prisma.missing.includes('engine-arch'), JSON.stringify(r.prisma.missing))
}

section('darwin — x64 و arm64')
{
  const mk = (file: string, arch: 'x64' | 'arm64') => mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    [`node_modules/@prisma/engines/${file}`]: engine('q'),
    'node_modules/.prisma/client/index.js': clientJS,
    [`node_modules/.prisma/client/${file}`]: engine('q'),
  })
  const rx = vn.verify({ root: mk('libquery_engine-darwin.dylib.node', 'x64'), platform: 'darwin', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('darwin-x64 با libquery_engine-darwin.dylib.node پذیرفته می‌شود', rx.ok === true, badText(rx))
  const ra = vn.verify({ root: mk('libquery_engine-darwin-arm64.dylib.node', 'arm64'), platform: 'darwin', arch: 'arm64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('darwin-arm64 با نام -arm64 پذیرفته می‌شود', ra.ok === true, badText(ra))
  const rr = vn.verify({ root: mk('libquery_engine-darwin.dylib.node', 'arm64'), platform: 'darwin', arch: 'arm64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('darwin-arm64 با نام سادهٔ darwin هم پذیرفته می‌شود (Rosetta)', rr.ok === true, badText(rr))
  const rw = vn.verify({ root: mk('libquery_engine-darwin-arm64.dylib.node', 'x64'), platform: 'darwin', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('darwin-x64 با موتور arm64 رد می‌شود', rw.critical === true)
}

section('بستهٔ @prisma/engines کلاً نصب نیست')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/.prisma/client/index.js': clientJS,
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('شکست گزارش می‌شود', r.critical === true)
  check('قلم engines-package مفقود است', r.prisma.missing.includes('engines-package'), JSON.stringify(r.prisma.missing))
  check('پیام npm install ناقص (نه prisma generate)', badText(r).includes('npm install'))
}

section('ساختار صحیح کلاینت + سازگاری نسخه‌ها')
{
  const root = healthyWinTree()
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('نسخه‌های هم‌راستا → بدون هشدار نسخه', r.prisma.versionMismatch === false)
  check('خط ok سازگاری نسخه‌ها چاپ می‌شود', r.lines.some((l) => l.level === 'ok' && l.text.includes('سازگارند')))

  const mismatchRoot = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/query_engine-windows.dll.node': engine('q'),
    'node_modules/prisma/package.json': pkgAt('prisma', '6.19.2'),
    'node_modules/@prisma/client/package.json': pkgAt('@prisma/client', '7.1.0'),
    'node_modules/.prisma/client/package.json': pkgAt('.prisma/client', '7.1.0'),
    'node_modules/.prisma/client/index.js': clientJS,
    'node_modules/.prisma/client/query_engine-windows.dll.node': engine('q'),
  })
  const rm = vn.verify({ root: mismatchRoot, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('اختلاف major نسخه‌ها → هشدار (نه شکست سخت)', rm.prisma.versionMismatch === true && rm.critical === false, `critical=${rm.critical}`)
  check('هشدار «engine does not match» می‌دهد', rm.lines.some((l) => l.level === 'warn' && l.text.includes('نسخه')))
}

section('مسیر سفارشی output در schema')
{
  const root = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': `generator client {\n  provider = "prisma-client-js"\n  output   = "custom-generated/client"\n}\n`,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/@prisma/engines/query_engine-windows.dll.node': engine('q'),
    'custom-generated/client/index.js': clientJS,
    'custom-generated/client/query_engine-windows.dll.node': engine('q'),
  })
  const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
  check('کلاینت در مسیر سفارشی پیدا می‌شود', r.ok === true, badText(r))
  check('مسیر clientDir همان output سفارشی است', r.prisma.clientDir === path.join(root, 'custom-generated', 'client'), String(r.prisma.clientDir))
}

section('فایل غیرقابل‌خواندن (فقط POSIX)')
{
  if (process.platform === 'win32') {
    check('روی Windows قابل آزمایش نیست — SKIP صادقانه', true)
  } else {
    const root = healthyWinTree()
    const eng = path.join(root, 'node_modules', '@prisma', 'engines', 'query_engine-windows.dll.node')
    const cli = path.join(root, 'node_modules', '.prisma', 'client', 'query_engine-windows.dll.node')
    fs.chmodSync(eng, 0o000)
    fs.chmodSync(cli, 0o000)
    const r = vn.verify({ root, platform: 'win32', arch: 'x64', skipNpm: true, skipEsbuild: true, skipSharp: true, skipSwc: true })
    check('فایل غیرقابل‌خواندن → شکست', r.critical === true)
    check('پیام «قابل خواندن نیست»', badText(r).includes('قابل خواندن نیست'), badText(r))
    fs.chmodSync(eng, 0o644)
    fs.chmodSync(cli, 0o644)
  }
}

section('الگوها — واحدی، مستقل از درخت')
{
  const wp = vn.prismaEnginePatterns('win32', 'x64')
  check('نام مورد انتظار ویندوز = query_engine-windows.dll.node', wp.expected === 'query_engine-windows.dll.node', wp.expected)
  check('query_engine-windows.dll.node الگوی win32 را ارضا می‌کند', wp.patterns.some((re) => re.test('query_engine-windows.dll.node')))
  check('libquery_engine-debian…so.node الگوی win32 را ارضا نمی‌کند', !wp.patterns.some((re) => re.test('libquery_engine-debian-openssl-3.0.x.so.node')))
  check('libquery_engine-windows.dll.node (قدیمی) هم ارضا می‌شود', wp.patterns.some((re) => re.test('libquery_engine-windows.dll.node')))
  const lp = vn.prismaEnginePatterns('linux', 'x64')
  check('الگوی linux فقط libquery_engine-…so.node می‌پذیرد', lp.patterns.some((re) => re.test('libquery_engine-debian-openssl-3.0.x.so.node')) && !lp.patterns.some((re) => re.test('query_engine-windows.dll.node')))
  const dp = vn.prismaEnginePatterns('darwin', 'x64')
  check('الگوی darwin فقط …dylib.node می‌پذیرد', dp.patterns.some((re) => re.test('libquery_engine-darwin.dylib.node')) && !dp.patterns.some((re) => re.test('libquery_engine-debian-openssl-3.0.x.so.node')))
  check('engineArchCompatible: x64 موتور arm64 را رد می‌کند', vn.engineArchCompatible('libquery_engine-debian-openssl-3.0.x-arm64.so.node', 'x64') === false)
  check('engineArchCompatible: arm64 موتور arm64 را می‌پذیرد', vn.engineArchCompatible('libquery_engine-debian-openssl-3.0.x-arm64.so.node', 'arm64') === true)
  check('engineArchCompatible: arm64 موتور بدون نشان معماری را می‌پذیرد', vn.engineArchCompatible('libquery_engine-darwin.dylib.node', 'arm64') === true)
}

section('CLI — دقیقاً همان فراخوانی INSTALL.bat (فرآیند واقعی)')
{
  // The spawned CLI inherits the REAL process.platform — the healthy tree is
  // therefore built for the CURRENT platform (linux here, win32 on Windows CI).
  const p = process.platform
  const engineName = p === 'win32' ? 'query_engine-windows.dll.node'
    : p === 'darwin' ? 'libquery_engine-darwin.dylib.node'
      : 'libquery_engine-debian-openssl-3.0.x.so.node'
  const schemaEngineName = p === 'win32' ? 'schema-engine-windows.exe' : 'schema-engine-debian-openssl-3.0.x'
  const esbuildPkgName = (p === 'win32' ? '@esbuild/win32-x64' : p === 'darwin' ? '@esbuild/darwin-x64' : '@esbuild/linux-x64')
  const esbuildBinName = p === 'win32' ? 'esbuild.exe' : 'esbuild'
  const healthyForPlatform = mkTree({
    'package.json': JSON.stringify({ name: 't', version: '1.0.0', allowScripts: { prisma: true, esbuild: true } }),
    'prisma/schema.prisma': schemaDefault,
    [`node_modules/@prisma/engines/package.json`]: pkgAt('@prisma/engines', '6.19.2'),
    [`node_modules/@prisma/engines/${engineName}`]: engine('q'),
    [`node_modules/@prisma/engines/${schemaEngineName}`]: engine('s'),
    'node_modules/.prisma/client/index.js': clientJS,
    [`node_modules/.prisma/client/${engineName}`]: engine('q'),
    [`node_modules/${esbuildPkgName}/package.json`]: pkgAt(esbuildPkgName, '0.28.2'),
    [`node_modules/${esbuildPkgName}/bin/${esbuildBinName}`]: engine('b'),
    'node_modules/esbuild/lib/main.js': clientJS,
  })
  const g = spawnSync(process.execPath, [path.resolve(__dirname, 'verify-native.js'), '--root', healthyForPlatform], { encoding: 'utf-8' })
  check('درخت سالم (پلتفرم واقعی) → exit code 0', g.status === 0, `status=${g.status}\n${g.stdout}`)
  check('گزارش فارسی چاپ می‌شود', g.stdout.includes('موتور کوئری Prisma موجود و سالم است'), g.stdout)

  const badForPlatform = mkTree({
    'package.json': '{"name":"t","version":"1.0.0"}',
    'prisma/schema.prisma': schemaDefault,
    'node_modules/@prisma/engines/package.json': pkgAt('@prisma/engines', '6.19.2'),
    'node_modules/.prisma/client/index.js': clientJS,
    [`node_modules/${esbuildPkgName}/package.json`]: pkgAt(esbuildPkgName, '0.28.2'),
    [`node_modules/${esbuildPkgName}/bin/${esbuildBinName}`]: engine('b'),
    'node_modules/esbuild/lib/main.js': clientJS,
  })
  const b = spawnSync(process.execPath, [path.resolve(__dirname, 'verify-native.js'), '--root', badForPlatform], { encoding: 'utf-8' })
  check('درخت ناقص (بدون موتور) → exit code 1', b.status === 1, `status=${b.status}`)
  check('خروجی، علت دقیق را چاپ می‌کند', b.stdout.includes('فایل موتور کوئری Prisma') && b.stdout.includes('یافت نشد'), b.stdout)

  const j = spawnSync(process.execPath, [path.resolve(__dirname, 'verify-native.js'), '--root', healthyForPlatform, '--json'], { encoding: 'utf-8' })
  let parsed: { ok?: boolean; critical?: boolean } | null = null
  try { parsed = JSON.parse(j.stdout) } catch { parsed = null }
  check('حالت --json خروجی JSON معتبر می‌دهد', j.status === 0 && parsed !== null && parsed.ok === true && parsed.critical === false)
}

console.log('\n══════════════════════════════════════════')
console.log(`مجموعهٔ verify-native: ${passed} تایید شد، ${failed} شکست خورد`)
if (failures.length) {
  console.log('شکست‌ها:')
  for (const f of failures) console.log('  ✘ ' + f)
}
console.log('══════════════════════════════════════════')
process.exit(failed > 0 ? 1 : 0)
