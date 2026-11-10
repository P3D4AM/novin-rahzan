#!/usr/bin/env node
/**
 * np-probe.js — ابزار تشخیص پروتکل دستگاه‌های حضور و غیاب (بدون نیاز به نرم‌افزار)
 *
 * همهٔ گویش‌ها و واریانت‌های رمزنگاری ممکن را روی دستگاه واقعی امتحان می‌کند و
 * پاسخ خام (hex) هر تلاش را چاپ می‌کند تا دقیقاً معلوم شود دستگاه چه صحبت می‌کند.
 *
 * استفاده (در ویندوز، داخل پوشهٔ نصب HozorPlus):
 *   node scripts\np-probe.js 192.168.1.75 4238 12345
 *                     └─ IP دستگاه   └─ پورت   └─ رمز شبکه (اختیاری، پیش‌فرض 0)
 *
 * گزینه‌ها:
 *   HTTP_PORT=14460   پورت API HTTP دستگاه‌های VisibleLight (پیش‌فرض 14460، 0=غیرفعال)
 *   WAIT=1500         مهلت هر تلاش (میلی‌ثانیه)
 *
 * خروجی را کامل برای پشتیبانی کپی کنید — از روی فریم‌های hex می‌توانیم پروتکل را
 * دقیقاً سازگار کنیم.
 */
const net = require('net')
const http = require('http')

const IP = process.argv[2] || '192.168.1.75'
const PORT = Number(process.argv[3] || 4238)
const PW = Number(process.argv[4] || 0)
const HTTP_PORT = Number(process.env.HTTP_PORT ?? 14460)
const WAIT = Number(process.env.WAIT || 1500)

function hex(b) { return b.toString('hex').replace(/(..)/g, '$1 ').trim() }
function sum16(b, from, to) { let s = 0; for (let i = from; i < to; i++) s += b[i]; return s & 0xffff }

function keyLE32(pw, salt = 0) {
  const k = (pw + salt) >>> 0
  return Buffer.from([k & 0xff, (k >>> 8) & 0xff, (k >>> 16) & 0xff, (k >>> 24) & 0xff])
}
function keyASCII(pw) {
  const s = String(pw)
  return Buffer.from([s.charCodeAt(0) || 0, s.charCodeAt(1) || 0, s.charCodeAt(2) || 0, s.charCodeAt(3) || 0])
}
function xor(buf, key) { const o = Buffer.from(buf); for (let i = 0; i < o.length; i++) o[i] ^= key[i % 4]; return o }

/* ---------- فریم‌سازها ---------- */

function classicFrame(cmd, arg1 = 0, arg2 = 0) {
  const f = Buffer.alloc(16)
  f.writeUInt16LE(0xaa55, 0)
  f.writeUInt16LE(1, 2)
  f.writeUInt16LE(0x1979, 4)
  f.writeUInt16LE(cmd, 6)
  f.writeUInt32LE(arg2 >>> 0, 8)
  f.writeUInt16LE(arg1, 12)
  f.writeUInt16LE(sum16(f, 0, 14), 14)
  return f
}
function extFrame(cmd, arg = 0) {
  const f = Buffer.alloc(12)
  f.writeUInt16LE(0xaa55, 0)
  f.writeUInt16LE(1, 2)
  f.writeUInt32LE(cmd >>> 0, 4)
  f.writeUInt16LE(arg, 8)
  f.writeUInt16LE((sum16(f, 2, 10) + 0xff) & 0xffff, 10)
  return f
}

/* ---------- تلاش TCP (هر تلاش روی اتصال تازه) ---------- */

const results = []

function attempt(label, frame) {
  return new Promise((resolve) => {
    const s = new net.Socket()
    let got = Buffer.alloc(0)
    let done = false
    const finish = (note) => {
      if (done) return
      done = true
      try { s.destroy() } catch { /* */ }
      results.push({ label, sent: frame, got, note })
      const status = got.length === 0 ? 'سکوت (بی‌پاسخ)' : `${got.length} بایت پاسخ`
      console.log(`  ${label.padEnd(46)} » ${hex(frame.subarray(0, 16))}${frame.length > 16 ? ' …' : ''}`)
      console.log(`  ${''.padEnd(46)} « ${got.length ? hex(got.subarray(0, 32)) + (got.length > 32 ? ' …' : '') : '—'}   [${status}]`)
      resolve()
    }
    s.setTimeout(4000)
    s.once('error', () => finish('خطای اتصال'))
    s.once('timeout', () => finish('timeout'))
    s.connect(PORT, IP, () => {
      s.write(frame)
      s.on('data', (c) => { got = Buffer.concat([got, c]) })
      setTimeout(() => finish('پاسخ'), WAIT)
    })
  })
}

/* ---------- تلاش HTTP (دستگاه‌های VisibleLight) ---------- */

function httpAttempt(port, path, body) {
  return new Promise((resolve) => {
    const data = Buffer.from(JSON.stringify(body))
    const req = http.request(
      { host: IP, port, path, method: 'POST', timeout: 3500,
        headers: { 'Content-Type': 'application/json', 'Content-Length': data.length } },
      (res) => {
        let buf = ''
        res.on('data', (c) => { buf += c })
        res.on('end', () => {
          console.log(`  POST :${port}${path.padEnd(16)} → HTTP ${res.statusCode} · ${buf.slice(0, 160)}`)
          results.push({ label: `HTTP ${path}`, http: res.statusCode, body: buf })
          resolve()
        })
      })
    req.on('timeout', () => { req.destroy(); console.log(`  POST :${port}${path.padEnd(16)} → timeout`); resolve() })
    req.on('error', (e) => { console.log(`  POST :${port}${path.padEnd(16)} → ${e.code || e.message}`); resolve() })
    req.end(data)
  })
}

async function main() {
  console.log('╔══════════════════════════════════════════════════════════════════════════╗')
  console.log(`║  np-probe — تشخیص پروتکل دستگاه  ${IP}:${PORT}  رمز=${PW}`)
  console.log('╚══════════════════════════════════════════════════════════════════════════╝\n')

  /* ---- بخش ۱: API HTTP (دستگاه‌های NP756P/NP761P «VisibleLight») ---- */
  if (HTTP_PORT > 0) {
    console.log(`[۱] امتحان API_HTTP روی پورت ${HTTP_PORT} (پروتکل دستگاه‌های جدید NP):`)
    await httpAttempt(HTTP_PORT, '/getDeviceKey', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getLogCount', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getDeviceTime', { pass: PW })
    console.log()
  }

  /* ---- بخش ۲: پروتکل باینری روی پورت تنظیم‌شده ---- */
  console.log(`[۲] امتحان گویش‌های باینری روی پورت ${PORT}:`)
  const SALT = 0x25b84
  const variants = [
    ['classic 16B · GetSerialNumber (خط پایه)', classicFrame(0x113)],
    ['EXT 12B ساده · GetSerialNumber (خط پایه)', extFrame(0x113)],
    ['EXT رمزشده · key=LE32(رمز)        [پیش‌فرض DLL]', xor(extFrame(0x113), keyLE32(PW))],
    [`EXT رمزشده · key=LE32(رمز+${SALT})   [واریانت قدیمی]`, xor(extFrame(0x113), keyLE32(PW, SALT))],
    [`EXT رمزشده · key=LE32(${SALT})        [نمک تنها]`, xor(extFrame(0x113), keyLE32(0, SALT))],
    ['EXT رمزشده · key=LE32(رمز^نمک)', xor(extFrame(0x113), keyLE32(PW ^ SALT))],
    ['EXT رمزشده · key=ASCII(رمز)', xor(extFrame(0x113), keyASCII(PW))],
    ['EXT رمزشده · key=LE32(رمز+0x1979)', xor(extFrame(0x113), keyLE32(PW, 0x1979))],
    ['EXT رمزشده · key=LE32(0x1979)', xor(extFrame(0x113), keyLE32(0, 0x1979))],
    ['classic رمزشده 16B · key=LE32(رمز)', xor(classicFrame(0x113), keyLE32(PW))],
    [`classic رمزشده 16B · key=LE32(رمز+${SALT})`, xor(classicFrame(0x113), keyLE32(PW, SALT))],
    ['EXT رمزشده · key=LE32(رمز) · GetDeviceTime', xor(extFrame(0x10e, 4), keyLE32(PW))],
    ['classic · GetDeviceTime (خط پایه)', classicFrame(0x10e, 0, 4)],
  ]
  for (const [label, frame] of variants) {
    await attempt(label, frame)
  }

  /* ---- جمع‌بندی ---- */
  console.log('\n───────────────── جمع‌بندی ─────────────────')
  const resp = results.filter((r) => r.got && r.got.length > 0)
  if (resp.length === 0) {
    console.log(' هیچ پاسخی از هیچ گویشی دریافت نشد — دستگاه روی این IP/پورت پاسخ نمی‌دهد.')
  } else {
    for (const r of resp) {
      console.log(` ✓ ${r.label} → ${hex(r.got.subarray(0, 24))}${r.got.length > 24 ? ' …' : ''}`)
    }
    const enc = resp.find((r) => r.got.length >= 12 && r.got[0] === 0xaa && r.got[1] === 0x55 && r.label.startsWith('EXT رمزشده'))
    if (enc) console.log(`\n ★ برنده: «${enc.label}» — این گویش را برای پشتیبانی بفرستید تا فعالش کنیم!`)
    const httpOk = results.filter((r) => r.http && r.http < 500)
    if (httpOk.length) console.log(`\n ★ دستگاه API HTTP دارد (${httpOk.map((r) => r.label).join(', ')}) — مسیر HTTP قابل استفاده است!`)
  }
  console.log('\nکل خروجی را کپی و برای پشتیبانی بفرستید. ✔')
}

main().catch((e) => { console.error('CRASH:', e); process.exit(1) })
