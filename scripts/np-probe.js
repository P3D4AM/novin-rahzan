#!/usr/bin/env node
/**
 * np-probe.js — ابزار تشخیص پروتکل دستگاه‌های حضور و غیاب (مستقل، بدون نصب)
 *
 * همهٔ گویش‌های ممکن را روی دستگاه واقعی امتحان می‌کند و پاسخ خام (hex / JSON)
 * هر تلاش را چاپ می‌کند تا دقیقاً معلوم شود دستگاه چه صحبت می‌کند:
 *
 *   ۱) API HTTP روی پورت 14460 — پروتکل رسمی دستگاه‌های NP جدید (VisibleLight)
 *      که نرم‌افزار خود فروشنده (PacketParserStandard.dll) استفاده می‌کند:
 *      POST /getDeviceKey, /getLogCount, /getDeviceTime, /getDeviceInfo, /getOEM
 *      با بدنهٔ {"pass": رمز شبکه}
 *   ۲) پروتکل باینری کلاسیک ۱۶ بایتی (SBXPC) — ساده و رمزشده با رمز
 *   ۳) پروتکل EXT ۱۲ بایتی — ساده و رمزشده با همهٔ فرمول‌های کلید شناخته‌شده
 *   ۴) کلاسیک/EXT روی UDP
 *
 * استفاده (در ویندوز، هر جا Node نصب باشد — حتی بیرون از پوشهٔ برنامه):
 *   node np-probe.js 192.168.1.75 4238 12345
 *                     └─ IP      └─ پورت └─ رمز شبکه (اختیاری، پیش‌فرض 0)
 *
 * گزینه‌ها (متغیر محیطی):
 *   HTTP_PORT=14460  پورت API HTTP (پیش‌فرض 14460 — با 0 غیرفعال می‌شود)
 *   WAIT=1500        مهلت هر تلاش (میلی‌ثانیه)
 *   MID=1           شماره دستگاه (machine id)
 *
 * خروجی کامل را برای پشتیبانی کپی کنید — از روی فریم‌های hex دقیقاً می‌گوییم
 * دستگاه شما چه گویشی دارد و همان را فعال می‌کنیم.
 */
const net = require('net')
const http = require('http')
const dgram = require('dgram')

const IP = process.argv[2] || '192.168.1.75'
const PORT = Number(process.argv[3] || 4238)
const PW = Number(process.argv[4] || 0)
const HTTP_PORT = Number(process.env.HTTP_PORT ?? 14460)
const WAIT = Number(process.env.WAIT || 1500)
const MID = Number(process.env.MID || 1)
const SALT = 0x25b84

function hex(b) { return Buffer.from(b).toString('hex').replace(/(..)/g, '$1 ').trim() }
function sum16(b, from, to) { let s = 0; for (let i = from; i < to; i++) s += b[i]; return s & 0xffff }

function keyLE32(pw, salt = 0) {
  const k = (Number(pw) + salt) >>> 0
  return Buffer.from([k & 0xff, (k >>> 8) & 0xff, (k >>> 16) & 0xff, (k >>> 24) & 0xff])
}
function keyASCII(pw) {
  const s = String(pw)
  return Buffer.from([s.charCodeAt(0) || 0, s.charCodeAt(1) || 0, s.charCodeAt(2) || 0, s.charCodeAt(3) || 0])
}
function xor(buf, key) { const o = Buffer.from(buf); for (let i = 0; i < o.length; i++) o[i] ^= key[i % 4]; return o }
function xorRange(buf, key, from, toExclusive) { // فقط بخشی از فریم رمز می‌شود
  const o = Buffer.from(buf)
  for (let i = from; i < toExclusive; i++) o[i] ^= key[i % 4]
  return o
}

/* ---------- فریم‌سازها ---------- */

function classicFrame(cmd, arg1 = 0, arg2 = 0) {
  const f = Buffer.alloc(16)
  f.writeUInt16LE(0xaa55, 0)
  f.writeUInt16LE(MID, 2)
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
  f.writeUInt16LE(MID, 2)
  f.writeUInt32LE(cmd >>> 0, 4)
  f.writeUInt16LE(arg, 8)
  f.writeUInt16LE((sum16(f, 2, 10) + 0xff) & 0xffff, 10)
  return f
}

/* ---------- نتیجه‌گیری هوشمند ---------- */
/** آیا با XOR کلید، بافر به یک فریم شناخته‌شده (5A A5 / AA 55 / A5 5A) تبدیل می‌شود؟ */
function looksLikeFrame(buf, key) {
  if (!buf || buf.length < 2) return false
  const d = key ? xor(buf, key) : Buffer.from(buf)
  return d[0] === 0x5a && d[1] === 0xa5 ? true
    : d[0] === 0xaa && d[1] === 0x55 ? true
    : d[0] === 0xa5 && d[1] === 0x5a ? true
    : d[0] === 0x55 && d[1] === 0xaa ? true
    : false
}
function isRefusalAck(buf) { // پاسخ رد ۱۰ بایتی NP: aa 55 | mid | dword=0x10 | cksum فریم ما
  return buf && buf.length === 10 && buf[0] === 0xaa && buf[1] === 0x55 && buf.readUInt32LE(4) === 0x10
}

const results = []

/* ---------- تلاش TCP (هر تلاش روی اتصال تازه) ---------- */
function attempt(label, frame, key) {
  return new Promise((resolve) => {
    const s = new net.Socket()
    let got = Buffer.alloc(0)
    let done = false
    let connected = false
    let errCode = ''
    const finish = () => {
      if (done) return
      done = true
      try { s.destroy() } catch { /* */ }
      let verdict = ''
      if (!connected && got.length === 0) verdict = `خطای اتصال ${errCode ? '(' + errCode + ')' : ''} — پورت بسته یا IP اشتباه`
      else if (got.length === 0) verdict = 'سکوت (بی‌پاسخ)'
      else if (isRefusalAck(got)) verdict = 'ردِ ۱۰ بایتی (فریم خوانده شد ولی پذیرفته نشد)'
      else if (looksLikeFrame(got, null)) verdict = '★ پاسخِ فریم‌دار!'
      else if (key && looksLikeFrame(got, key)) verdict = '★★ پاسخِ رمزشده با همین کلید!!'
      else verdict = got.length + ' بایت پاسخ'
      results.push({ label, sent: frame, got, key, verdict })
      console.log(`  » ${hex(frame)}`)
      console.log(`  « ${got.length ? hex(got) : '—'}   [${verdict}]`)
      if (key && got.length && looksLikeFrame(got, key)) console.log(`  🔓 نمای رمزگشایی‌شده با کلید: ${hex(xor(got, key))}`)
      console.log()
      resolve()
    }
    s.setTimeout(4000)
    s.once('error', (e) => { if (!connected) errCode = e.code || e.message; finish() })
    s.once('timeout', () => finish())
    s.connect(PORT, IP, () => {
      connected = true
      s.write(frame)
      s.on('data', (c) => { got = Buffer.concat([got, c]) })
      setTimeout(finish, WAIT)
    })
  })
}

/* ---------- تلاش UDP ---------- */
function udpAttempt(label, frame, key) {
  return new Promise((resolve) => {
    const s = dgram.createSocket('udp4')
    let got = Buffer.alloc(0)
    let done = false
    const finish = () => {
      if (done) return
      done = true
      try { s.close() } catch { /* */ }
      const verdict = got.length === 0 ? 'سکوت (بی‌پاسخ)' : '★ پاسخ UDP!'
      results.push({ label, sent: frame, got, key, verdict })
      console.log(`  » ${hex(frame)}`)
      console.log(`  « ${got.length ? hex(got) : '—'}   [${verdict}]`)
      console.log()
      resolve()
    }
    s.on('error', () => finish())
    s.on('message', (c) => { got = Buffer.concat([got, c]); setTimeout(finish, 300) })
    s.send(frame, PORT, IP, (err) => { if (err) finish() })
    setTimeout(finish, WAIT + 1500)
  })
}

/* ---------- تلاش HTTP (پروتکل دستگاه‌های NP جدید / VisibleLight) ---------- */
function httpAttempt(port, path, body) {
  return new Promise((resolve) => {
    const data = Buffer.from(JSON.stringify(body))
    const t = setTimeout(() => { req.destroy(); console.log(`  POST :${port}${path.padEnd(18)} → timeout`); console.log(); resolve() }, 3500)
    const req = http.request(
      { host: IP, port, path, method: 'POST', headers: { 'Content-Type': 'application/json', 'Content-Length': data.length } },
      (res) => {
        let buf = ''
        res.on('data', (c) => { buf += c })
        res.on('end', () => {
          clearTimeout(t)
          const ok = res.statusCode < 500 && buf.length > 0
          results.push({ label: `HTTP ${path}`, http: res.statusCode, body: buf, verdict: ok ? `★ HTTP ${res.statusCode} — API فعال!` : `HTTP ${res.statusCode}` })
          console.log(`  POST :${port}${path.padEnd(18)} → HTTP ${res.statusCode} · ${buf.slice(0, 200)}`)
          console.log()
          resolve()
        })
      })
    req.on('error', (e) => { clearTimeout(t); console.log(`  POST :${port}${path.padEnd(18)} → ${e.code || e.message}`); console.log(); resolve() })
    req.end(data)
  })
}

async function main() {
  console.log('════════════════════════════════════════════════════════════════════')
  console.log(`  np-probe — تشخیص پروتکل دستگاه ${IP}:${PORT}  (رمز=${PW})`)
  console.log(`  باینری: پورت ${PORT} · HTTP: پورت ${HTTP_PORT || 'خاموش'} · مهلت هر تلاش: ${WAIT}ms`)
  console.log('════════════════════════════════════════════════════════════════════')
  console.log()

  /* ---- [۱] API HTTP — پروتکل رسمی دستگاه‌های NP جدید (پورت 14460) ---- */
  if (HTTP_PORT > 0) {
    console.log('[۱] API HTTP دستگاه‌های NP جدید (VisibleLight) روی پورت ' + HTTP_PORT + ':')
    console.log()
    await httpAttempt(HTTP_PORT, '/getDeviceKey', { pass: PW })
    if (PW) await httpAttempt(HTTP_PORT, '/getDeviceKey', { pass: String(PW) })
    await httpAttempt(HTTP_PORT, '/getLogCount', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getDeviceTime', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getDeviceInfo', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getOEM', { pass: PW })
  }

  /* ---- [۲] پروتکل باینری روی پورت تنظیم‌شده ---- */
  console.log(`[۲] گویش‌های باینری روی پورت ${PORT} (هر تلاش = اتصال تازه):`)
  console.log()
  const K = {
    dll: keyLE32(PW),                    // پیش‌فرض واقعی DLL (اسلات نمک صفر است)
    salted: keyLE32(PW, SALT),           // واریانت نسل قبلی
    onlySalt: keyLE32(0, SALT),           // فقط «نشان شرکت» بدون رمز
    ascii: keyASCII(PW),                 // رمز به‌صورت ASCII
  }

  const c113 = classicFrame(0x113)
  const e113 = extFrame(0x113)
  const c10e = classicFrame(0x10e, 0, 4)

  const battery = [
    ['A1 classic 16B · GetSerialNumber · ساده            (خط پایه)', c113, null],
    ['A2 classic 16B · GetDeviceTime · ساده              (خط پایه)', c10e, null],
    ['A3 EXT 12B · GetSerialNumber · ساده                (خط پایه)', e113, null],
    ['B1 EXT رمزشده · کلید=LE32(رمز)                       [پیش‌فرض DLL]', xor(e113, K.dll), K.dll],
    ['B2 EXT رمزشده · کلید=LE32(رمز+0x25B84)              [واریانت قدیمی]', xor(e113, K.salted), K.salted],
    ['B3 EXT رمزشده · کلید=LE32(رمز) · فقط ۱۰ بایت اول    (cksum ساده)', xorRange(e113, K.dll, 0, 10), K.dll],
    ['B4 EXT رمزشده · کلید=ASCII(رمز)', xor(e113, K.ascii), K.ascii],
    ['B5 EXT رمزشده · کلید=LE32(0x25B84)                  [نمکِ تنها]', xor(e113, K.onlySalt), K.onlySalt],
    ['C1 classic رمزشده 16B · کلید=LE32(رمز)              ★ تازه', xor(c113, K.dll), K.dll],
    ['C2 classic رمزشده 16B · کلید=LE32(رمز+0x25B84)      ★ تازه', xor(c113, K.salted), K.salted],
    ['C3 classic رمزشده · فقط ۱۴ بایت اول (cksum ساده) · کلید=LE32(رمز)', xorRange(c113, K.dll, 0, 14), K.dll],
    ['C4 classic رمزشده · magic ساده (رمز از بایت ۲) · کلید=LE32(رمز)', xorRange(c113, K.dll, 2, 16), K.dll],
    ['C5 classic رمزشده · GetDeviceTime · کلید=LE32(رمز)', xor(c10e, K.dll), K.dll],
    ['C6 classic رمزشده 16B · کلید=LE32(0x25B84)', xor(c113, K.onlySalt), K.onlySalt],
  ]
  for (const [label, frame, key] of battery) {
    console.log(`── ${label}`)
    await attempt(label, frame, key)
  }

  /* ---- [۳] UDP (بعضی دستگاه‌ها فقط UDP جواب می‌دهند) ---- */
  console.log('[۳] همان فریم‌های کلیدی روی UDP:')
  console.log()
  console.log('── U1 classic ساده روی UDP')
  await udpAttempt('U1', c113, null)
  console.log('── U2 EXT رمزشده · کلید=LE32(رمز) روی UDP')
  await udpAttempt('U2', xor(e113, K.dll), K.dll)
  console.log('── U3 classic رمزشده · کلید=LE32(رمز) روی UDP')
  await udpAttempt('U3', xor(c113, K.dll), K.dll)

  /* ---- جمع‌بندی ---- */
  console.log('───────────────────────── جمع‌بندی ─────────────────────────')
  const httpOk = results.filter((r) => r.http && r.http < 500 && r.body && r.body.length)
  const binWin = results.filter((r) => r.got && r.got.length && (r.verdict || '').startsWith('★'))
  const binAny = results.filter((r) => r.got && r.got.length)
  if (httpOk.length) {
    console.log(` ★★ API HTTP دستگاه فعال است! (${httpOk.map((r) => `HTTP ${r.http} ${r.label}`).join(' · ')})`)
    console.log('   → دستگاه شما با پروتکل HTTP (پورت 14460) کار می‌کند؛ همین خروجی را بفرستید تا اتصال را روی همین پروتکل فعال کنیم.')
  }
  if (binWin.length) {
    for (const r of binWin) console.log(` ★ برنده: «${r.label}» → ${hex(r.got.subarray(0, 24))}${r.got.length > 24 ? ' …' : ''}`)
    console.log('   → این گویش را برای پشتیبانی بفرستید تا فعالش کنیم!')
  }
  if (binAny.length && !binWin.length) {
    for (const r of binAny) console.log(` · پاسخ بدون ★ از «${r.label}»: ${hex(r.got.subarray(0, 24))}${r.got.length > 24 ? ' …' : ''}`)
  }
  if (!httpOk.length && !binWin.length && !binAny.length) {
    console.log(' هیچ پاسخی از هیچ گویشی دریافت نشد — دستگاه روی این IP/پورت پاسخ نمی‌دهد.')
    console.log('   (IP و پورت را از صفحهٔ دستگاه‌ها دقیق چک کنید؛ پورت HTTP را با HTTP_PORT=14460 امتحان کنید)')
  }
  console.log()
  console.log('کل خروجی را کپی و برای پشتیبانی بفرستید. ✔')
}

main().catch((e) => { console.error('CRASH:', e); process.exit(1) })
