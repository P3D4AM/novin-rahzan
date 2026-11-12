#!/usr/bin/env node
/**
 * np-probe.js — ابزار تشخیص پروتکل دستگاه‌های حضور و غیاب (مستقل، بدون نصب)
 *
 * کشف کلیدی v1.2.5: DLL فروشنده (SBPCCOMM) موقع وصل شدن TCP اول یک «ورود»
 * می‌فرستد — فرمان کلاسیک 0x52 با «رمز شبکه» داخل فریم (متنی، بدون رمزنگاری):
 *
 *    55 AA | mid | 79 19 | 52 00 | رمز(۴بایت LE) | 00 00 | cksum   → RESULT (status≠0 = ورود موفق)
 *
 * تا وقتی وارد نشده‌اید، دستگاه NP فرمان‌های دیگر را با همان پاسخ ردِ ۱۰بایتی
 * (aa 55 … 10 00 …) برمی‌گرداند و بقیه را ساکت رها می‌کند — دقیقاً حالتی که تا
 * امروز دیدیم! این ابزار همان ورود را مثل نرم‌افزار فروشنده اجرا می‌کند.
 *
 * استفاده (در ویندوز، هر جا Node نصب باشد):
 *   node np-probe.js 192.168.1.75 4238 12345
 *                     └─ IP      └─ پورت └─ رمز شبکه (اختیاری، پیش‌فرض 0)
 *
 * گزینه‌ها (متغیر محیطی):
 *   HTTP_PORT=14460   پورت API HTTP دستگاه‌های جدید (پیش‌فرض 14460 — با 0 خاموش)
 *   WAIT=1500         مهلت هر تلاش (میلی‌ثانیه)
 *   MID=1             شماره دستگاه (machine id)
 *
 * خروجی کامل را برای پشتیبانی کپی کنید.
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
function xor(buf, key) { const o = Buffer.from(buf); for (let i = 0; i < o.length; i++) o[i] ^= key[i % 4]; return o }

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
/** فرمان ورود 0x52 — رمز شبکه داخل arg2 (مسیر DLL فروشنده) */
function loginFrame(pw) { return classicFrame(0x52, 0, Number(pw) >>> 0) }
function extFrame(cmd, arg = 0) {
  const f = Buffer.alloc(12)
  f.writeUInt16LE(0xaa55, 0)
  f.writeUInt16LE(MID, 2)
  f.writeUInt32LE(cmd >>> 0, 4)
  f.writeUInt16LE(arg, 8)
  f.writeUInt16LE((sum16(f, 2, 10) + 0xff) & 0xffff, 10)
  return f
}

/* ---------- تحلیل پاسخ‌ها ---------- */

function isRefusalAck(buf) { // aa 55 | mid | dword=0x10 | cksum فریم ما
  return buf && buf.length === 10 && buf[0] === 0xaa && buf[1] === 0x55 && buf.readUInt32LE(4) === 0x10
}
/** RESULT 14 بایتی: AA 55 | mid | ?(2) | status(2) | ret(4) | cksum — با رد شدنِ اختیاریِ ACK 8B اولی */
function parseResult(buf) {
  if (!buf || buf.length < 14) return null
  let off = 0
  if (buf[0] === 0x5a && buf[1] === 0xa5 && buf.length >= 8 + 14) off = 8 // skip classic ACK
  const f = buf.subarray(off, off + 14)
  if (f[0] !== 0xaa || f[1] !== 0x55) return null
  if (f.readUInt16LE(12) !== sum16(f, 0, 12)) return null
  return { status: f.readUInt16LE(6), ret: f.readUInt32LE(8) >>> 0, mid: f.readUInt16LE(2) }
}
function looksLikeFrame(buf) {
  if (!buf || buf.length < 2) return false
  return (buf[0] === 0x5a && buf[1] === 0xa5) || (buf[0] === 0xaa && buf[1] === 0x55)
    || (buf[0] === 0xa5 && buf[1] === 0x5a) || (buf[0] === 0x55 && buf[1] === 0xaa)
}

const results = []

/* ---------- تلاش تک‌فریمی (اتتصال تازه) ---------- */
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
      if (!connected && got.length === 0) verdict = `خطای اتصال ${errCode ? '(' + errCode + ')' : ''}`
      else if (got.length === 0) verdict = 'سکوت (بی‌پاسخ)'
      else if (isRefusalAck(got)) verdict = 'ردِ ۱۰ بایتی code=0x10 (فریم خوانده شد ولی پذیرفته نشد)'
      else if (looksLikeFrame(got)) verdict = '★ پاسخِ فریم‌دار!'
      else if (key && looksLikeFrame(xor(got, key))) verdict = '★★ پاسخِ رمزشده با همین کلید!'
      else verdict = got.length + ' بایت پاسخ'
      results.push({ label, got, verdict })
      console.log(`  » ${hex(frame)}`)
      console.log(`  « ${got.length ? hex(got) : '—'}   [${verdict}]`)
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

/* ---------- دنبالهٔ چندفریمی روی «یک» اتصال — برای ورود (login) و بعد فرمان ----------
 * steps: [{name, send, waitMs}] — هر قدم: ارسال، جمع‌کردن پاسخ تا ۳۰۰ms سکوت یا مهلت، بعد قدم بعد. */
function sequence(label, steps) {
  return new Promise((resolve) => {
    const s = new net.Socket()
    let stepIdx = -1
    let got = Buffer.alloc(0)
    let quiet = null   // fires when 300ms passed since last byte
    let limit = null   // step deadline
    let done = false
    const log = []
    const finish = () => {
      if (done) return
      done = true
      if (quiet) clearTimeout(quiet)
      if (limit) clearTimeout(limit)
      try { s.destroy() } catch { /* */ }
      results.push({ label, log })
      resolve()
    }
    const logStep = () => {
      const st = steps[stepIdx]
      let v = ''
      if (got.length === 0) v = 'سکوت'
      else if (isRefusalAck(got)) v = 'ردِ ۱۰ بایتی code=0x10'
      else if (looksLikeFrame(got)) v = '★ فریم معتبر'
      else v = got.length + ' بایت'
      log.push({ name: st.name, got: Buffer.from(got), verdict: v })
      console.log(`  [${st.name}] « ${got.length ? hex(got.subarray(0, 48)) + (got.length > 48 ? ' …' : '') : '—'}   [${v}]`)
      goNext()
    }
    const goNext = () => {
      if (done) return
      stepIdx++
      if (stepIdx >= steps.length) return finish()
      const st = steps[stepIdx]
      got = Buffer.alloc(0)
      console.log(`  [${st.name}] » ${hex(st.send)}`)
      s.write(st.send)
      limit = setTimeout(() => { if (quiet) clearTimeout(quiet); quiet = null; logStep() }, st.waitMs || WAIT)
    }
    s.setTimeout(4000)
    s.once('error', () => finish())
    s.once('timeout', () => finish())
    s.on('data', (c) => {
      got = Buffer.concat([got, c])
      if (quiet) clearTimeout(quiet)
      quiet = setTimeout(() => { quiet = null; if (limit) clearTimeout(limit); limit = null; logStep() }, 300)
    })
    s.connect(PORT, IP, () => goNext())
  })
}

/* ---------- تلاش UDP ---------- */
function udpAttempt(label, frame) {
  return new Promise((resolve) => {
    const s = dgram.createSocket('udp4')
    let got = Buffer.alloc(0)
    let done = false
    const finish = () => {
      if (done) return
      done = true
      try { s.close() } catch { /* */ }
      results.push({ label, got, verdict: got.length ? '★ پاسخ UDP!' : 'سکوت' })
      console.log(`  » ${hex(frame)}`)
      console.log(`  « ${got.length ? hex(got) : '—'}   [${got.length ? '★ پاسخ UDP' : 'سکوت'}]`)
      console.log()
      resolve()
    }
    s.on('error', () => finish())
    s.on('message', (c) => { got = Buffer.concat([got, c]); setTimeout(finish, 300) })
    s.send(frame, PORT, IP, (err) => { if (err) finish() })
    setTimeout(finish, WAIT + 1500)
  })
}

/* ---------- تلاش HTTP ---------- */
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
          results.push({ label: `HTTP ${path}`, http: res.statusCode, body: buf, verdict: ok ? `★ HTTP ${res.statusCode}` : `HTTP ${res.statusCode}` })
          console.log(`  POST :${port}${path.padEnd(18)} → HTTP ${res.statusCode} · ${buf.slice(0, 160)}`)
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
  console.log(`  باینری: پورت ${PORT} · HTTP: پورت ${HTTP_PORT || 'خاموش'} · مهلت: ${WAIT}ms · mid=${MID}`)
  console.log('════════════════════════════════════════════════════════════════════')
  console.log()

  /* ---- [۱] ورود با «رمز شبکه» — فرمان 0x52 (مسیر DLL فروشنده؛ v1.2.5) ---- */
  console.log('[۱] ورود با رمز شبکه (فرمان 0x52) و بعد فرمان واقعی — روی یک اتصال:')
  console.log()
  await sequence(`L1 ورود(${PW}) + خواندن سریال`, [
    { name: `ورود 0x52 رمز=${PW}`, send: loginFrame(PW) },
    { name: 'بعد 0x113 سریال', send: classicFrame(0x113) },
  ])
  await sequence(`L2 ورود(${PW}) + ساعت دستگاه`, [
    { name: `ورود 0x52 رمز=${PW}`, send: loginFrame(PW) },
    { name: 'بعد 0x10e ساعت', send: classicFrame(0x10e, 0, 4) },
  ])
  if (PW !== 0) await sequence(`L3 ورود(0) + سریال`, [
    { name: 'ورود 0x52 رمز=0', send: loginFrame(0) },
    { name: 'بعد 0x113 سریال', send: classicFrame(0x113) },
  ])
  await sequence(`L4 ورود(999999 — رمز غلط، برای مقایسه) + سریال`, [
    { name: 'ورود 0x52 رمز=999999', send: loginFrame(999999) },
    { name: 'بعد 0x113 سریال', send: classicFrame(0x113) },
  ])
  await sequence('L5 EXT رمزشده 0x52 (واریانت EXT ورود)', [
    { name: 'EXT 0x52 رمزشده', send: xor(extFrame(0x52), keyLE32(PW)) },
  ])

  /* ---- [۲] API HTTP دستگاه‌های NP جدید (VisibleLight) ---- */
  if (HTTP_PORT > 0) {
    console.log(`[۲] API HTTP دستگاه‌های NP جدید (VisibleLight) روی پورت ${HTTP_PORT}:`)
    console.log()
    await httpAttempt(HTTP_PORT, '/getDeviceKey', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getLogCount', { pass: PW })
    await httpAttempt(HTTP_PORT, '/getDeviceTime', { pass: PW })
  }

  /* ---- [۳] گویش‌های باینری (خط پایه) ---- */
  console.log(`[۳] گویش‌های باینری روی پورت ${PORT} (هر تلاش = اتصال تازه، بدون ورود):`)
  console.log()
  const K = { dll: keyLE32(PW), salted: keyLE32(PW, SALT), onlySalt: keyLE32(0, SALT) }
  const e113 = extFrame(0x113)
  const c113 = classicFrame(0x113)
  const battery = [
    ['A1 classic 16B · GetSerialNumber · ساده', c113, null],
    ['A2 EXT 12B · GetSerialNumber · ساده', e113, null],
    ['B1 EXT رمزشده · کلید=LE32(رمز)', xor(e113, K.dll), K.dll],
    ['B2 EXT رمزشده · کلید=LE32(رمز+0x25B84)', xor(e113, K.salted), K.salted],
    ['C1 classic رمزشده 16B · کلید=LE32(رمز)', xor(c113, K.dll), K.dll],
  ]
  for (const [label, frame, key] of battery) {
    console.log(`── ${label}`)
    await attempt(label, frame, key)
  }

  /* ---- [۴] UDP ---- */
  console.log('[۴] فریم‌های کلیدی روی UDP:')
  console.log()
  await udpAttempt('U1 classic ساده', c113)
  await udpAttempt('U2 EXT رمزشده', xor(e113, K.dll))

  /* ---- جمع‌بندی ---- */
  console.log('───────────────────────── جمع‌بندی ─────────────────────────')
  let winner = null
  for (const r of results) {
    if (r.log) { // sequence result
      const login = r.log.find((l) => l.name.startsWith('ورود'))
      const after = r.log.find((l) => !l.name.startsWith('ورود'))
      if (login && after && parseResult(login.got) && parseResult(login.got).status !== 0 && after.verdict.startsWith('★')) {
        winner = winner || r
        console.log(` ★★★ «${r.label}»: ورود پذیرفته شد (status=${parseResult(login.got).status}) و فرمان بعدی هم جواب داد!`)
      } else if (login && parseResult(login.got)) {
        const st = parseResult(login.got).status
        console.log(` · «${r.label}»: پاسخ ورود = RESULT status=${st}${st === 0 ? ' (رمز پذیرفته نشد)' : ''}`)
      } else if (login && isRefusalAck(login.got)) {
        console.log(` · «${r.label}»: ورود → ردِ ۱۰ بایتی (code=0x10)`)
      } else if (login && login.got.length === 0) {
        console.log(` · «${r.label}»: ورود → سکوت`)
      }
    } else if (r.got && (r.verdict || '').startsWith('★')) {
      console.log(` ★ «${r.label}» → ${hex(r.got.subarray(0, 24))}${r.got.length > 24 ? ' …' : ''}`)
    } else if (r.http && r.http < 500 && r.body) {
      console.log(` ★ HTTP ${r.http} ${r.label} → ${String(r.body).slice(0, 60)}`)
    }
  }
  if (winner) {
    console.log('\n ★★★ نتیجه: دستگاه با «ورود رمز شبکه» (فرمان 0x52) باز می‌شود — پروتکل کلاسیک + ورود.')
    console.log('   این یعنی «تست اتصال» نسخهٔ v1.2.5 همین حالا باید موفق شود؛ نصب کنید و تست بگیرید.')
  }
  console.log()
  console.log('کل خروجی را کپی و برای پشتیبانی بفرستید. ✔')
}

main().catch((e) => { console.error('CRASH:', e); process.exit(1) })
