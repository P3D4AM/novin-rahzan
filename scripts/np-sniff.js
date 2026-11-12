#!/usr/bin/env node
/**
 * np-sniff.js — ابزار جامع ثبت ترافیک پروتکل دستگاه‌های حضور و غیاب (مستقل، بدون نصب)
 *
 * ═══ چرا این ابزار؟ ═══
 *   تحقیقات v1.2.6 روی نرم‌افزار NovinHozor نشان داد دستگاه NP756+ (756PLUS) اصلاً از
 *   «پورت کششی» (4238) داده نمی‌دهد — آن پورت فقط یک استاب قدیمی است که همه‌چیز را رد
 *   می‌کند. کانال واقعیِ ترددها «پوش بی‌درنگ» است: خودِ دستگاه به سرور وصل می‌شود و
 *   رکوردهای JSON می‌فرستد (پوشته در قالب RTLOG001/2/3 + طول ۴ بایتی — از دیسمبل
 *   RealSvrOcxTcp.ocx). این ابزار همان «سرور» را شبیه‌سازی می‌کند تا ترافیک واقعی
 *   دستگاه شما را ضبط کند.
 *
 * ═══ دو حالت کار ═══
 *
 * ۱) حالت شنود (پیشنهادی برای NP756+):
 *      node scripts/np-sniff.js --listen [port]
 *      پیش‌فرض پورت=5005. بعد:
 *        • در منوی دستگاه (تنظیمات شبکه/سرور) آدرس «سرور» را به IP همین کامپیوتر
 *          و پورت را همان پورت بالا بدهید — همان‌طور که قبلاً به نرم‌افزار
 *          NovinHozor می‌دادید.
 *        • یک تردد آزمایشی روی دستگاه بزنید (یا منتظر پوش‌های دوره‌ای بمانید).
 *        • همه‌ی فریم‌ها (hex + متن JSON/XML) همین‌جا و در فایل sniff-*.log ثبت می‌شود.
 *        • به فریم‌های شناسایی‌شده خودکار پاسخ OK می‌دهد تا دستگاه به پرکردن ادامه دهد
 *          (با REPLY=0 خاموش کنید؛ سبک پاسخ با REPLY_STYLE=v1|v3|raw تغییر می‌کند).
 *
 * ۲) حالت پروکسی (برای دیدن ترافیک نرم‌افزار سازنده ↔ دستگاه روی پورت کششی):
 *      node scripts/np-sniff.js <deviceIP> <devicePort> [listenPort]
 *      در نرم‌افزار NovinHozor IP دستگاه را به 127.0.0.1 (یا IP این کامپیوتر) تغییر دهید؛
 *      همه‌ی رفت‌وبرگشت‌ها بی‌هزینه رله و ثبت می‌شود.
 *
 * گزینه‌ها (متغیر محیطی):
 *   REPLY=1        پاسخ خودکار به فریم‌های RTLOG (پیش‌فرض 1)
 *   REPLY_STYLE=v1 پاسخ {"log_id":N,"result":"OK"} در پوشت RTLOG001 (پیش‌فرض)
 *            =v3   مثل v1 + "mode":"nothing"
 *            =raw  فقط JSON بدون پوشت
 *   PW=...         رمز — فقط برای پیش‌نمایش XOR فریم‌های نامشخص در حالت پروکسی
 *   EBIO=1        در حالت شنود، به پیام‌های XML (Ebio) هم پاسخ بدهد (پیش‌فرض 1)
 *
 * خروجی کامل را برای پشتیبانی بفرستید.
 */
'use strict';
const net = require('net');
const fs = require('fs');
const path = require('path');

/* ---------- آرگومان‌ها ---------- */
const args = process.argv.slice(2);
const LISTEN = args[0] === '--listen' || args.length === 0;
const devIp = LISTEN ? null : args[0];
const devPort = LISTEN ? null : parseInt(args[1], 10);
const listenPort = LISTEN
  ? (parseInt(args[1], 10) || 5005)
  : (parseInt(args[2], 10) || devPort);
const PW = Number(process.env.PW || 0);
const REPLY = Number(process.env.REPLY ?? 1) !== 0;
const REPLY_STYLE = (process.env.REPLY_STYLE || 'v1').toLowerCase();
const EBIO = Number(process.env.EBIO ?? 1) !== 0;

const RTLOG_MAGIC = ['RTLOG001', 'RTLOG002', 'RTLOG003'];

/* ---------- لاگ ---------- */
const ts0 = new Date();
const stamp = `${ts0.getFullYear()}${String(ts0.getMonth() + 1).padStart(2, '0')}${String(ts0.getDate()).padStart(2, '0')}-${String(ts0.getHours()).padStart(2, '0')}${String(ts0.getMinutes()).padStart(2, '0')}${String(ts0.getSeconds()).padStart(2, '0')}`;
const LOGFILE = path.join(process.cwd(), `sniff-${stamp}.log`);
let logStream = null;
try { logStream = fs.createWriteStream(LOGFILE, { flags: 'a' }); } catch { /* */ }

function nowStr() {
  const d = new Date();
  return `${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}:${String(d.getSeconds()).padStart(2, '0')}.${String(d.getMilliseconds()).padStart(3, '0')}`;
}
function say(line) {
  console.log(line);
  if (logStream) { try { logStream.write(line + '\n'); } catch { /* */ } }
}
function hex(b) { return Buffer.from(b).toString('hex').replace(/(..)/g, '$1 ').trim() }
function textPreview(b) {
  const s = Buffer.from(b).toString('latin1').replace(/[^\x20-\x7e]/g, '·');
  return s.length > 220 ? s.slice(0, 220) + '…' : s;
}
function keyLE32(pw) {
  const k = Number(pw) >>> 0;
  return Buffer.from([k & 0xff, (k >>> 8) & 0xff, (k >>> 16) & 0xff, (k >>> 24) & 0xff]);
}
function xor(buf, key) { const o = Buffer.from(buf); for (let i = 0; i < o.length; i++) o[i] ^= key[i % 4]; return o }
function looksLikeXor(buf) {
  if (!PW || buf.length < 8) return null;
  const dec = xor(buf, keyLE32(PW));
  if ((dec[0] === 0x55 && dec[1] === 0xaa) || (dec[0] === 0xaa && dec[1] === 0x55)) return dec;
  return null;
}

/* ---------- تجزیه RTLOG ---------- */
/** جست‌وجوی فریم RTLOG در بافر — برمی‌گرداند [{magic,off,len,payloadOff}] */
function findRtlogFrames(buf) {
  const frames = [];
  let off = 0;
  while (off + 12 <= buf.length) {
    const asAscii = buf.toString('latin1', off, off + 8);
    const idx = RTLOG_MAGIC.indexOf(asAscii);
    if (idx >= 0) {
      const len = buf.readUInt32LE(off + 8);
      const payloadOff = off + 12;
      const end = payloadOff + len;
      if (len > 0 && len < 0x100000 && end <= buf.length) {
        frames.push({ magic: asAscii, off, len, payloadOff, end });
      }
      off += 8; // جلو برو و دوباره جست‌وجو (فریم بعدی)
    } else off++;
  }
  return frames;
}

/** ساخت فریم پاسخ RTLOG001/2/3 مثل OCX سازنده: magic + LE32(len+1) + payload + NUL */
function buildRtlogReply(magic, json) {
  const body = Buffer.from(json, 'ascii');
  const f = Buffer.alloc(8 + 4 + body.length + 1);
  f.write(magic, 0, 'ascii');
  f.writeUInt32LE(body.length + 1, 8);
  body.copy(f, 12);
  f[f.length - 1] = 0;
  return f;
}

/** استخراج log_id از JSON فریم (عدد یا رشته) */
function extractLogId(payload) {
  try {
    const s = payload.toString('utf8').trim().replace(/\0.*$/s, '');
    const m = s.match(/"log_id"\s*:\s*"?([0-9]+)"?/);
    return m ? m[1] : null;
  } catch { return null }
}

/* ═══════════════ حالت ۱: شنود ═══════════════ */
function runListen() {
  let connSeq = 0;
  const stats = { conns: 0, rtlog: 0, ebio: 0, other: 0, replied: 0, logIds: [] };

  const server = net.createServer((sock) => {
    const id = ++connSeq;
    stats.conns++;
    const peer = `${sock.remoteAddress}:${sock.remotePort}`;
    say(`\n[${nowStr()}] ✚ اتصال #${id} از ${peer}`);
    let buf = Buffer.alloc(0);

    const replyRtlog = (frame) => {
      if (!REPLY) return;
      const logId = extractLogId(buf.subarray(frame.payloadOff, frame.end));
      const json = logId
        ? `{"log_id":"${logId}","result":"OK"${REPLY_STYLE === 'v3' ? ',"mode":"nothing"' : ''}}`
        : `{"result":"OK"}`;
      let wire;
      if (REPLY_STYLE === 'raw') wire = Buffer.from(json, 'ascii');
      else wire = buildRtlogReply(frame.magic, json);
      try { sock.write(wire); stats.replied++; } catch { /* */ }
      say(`[${nowStr()}] #${id} » پاسخ ${frame.magic} → ${json}`);
    };

    sock.on('data', (chunk) => {
      buf = Buffer.concat([buf, chunk]);
      // ۱) فریم‌های RTLOG
      let consumed = 0;
      for (const frame of findRtlogFrames(buf)) {
        const payload = buf.subarray(frame.payloadOff, frame.end);
        stats.rtlog++;
        const logId = extractLogId(payload);
        if (logId) stats.logIds.push(logId);
        say(`\n[${nowStr()}] #${id} « فریم ${frame.magic} (payload=${frame.len}B) از ${peer}`);
        say(`   متن: ${payload.toString('utf8').replace(/\0.*$/s, '').trim()}`);
        say(`   hex : ${hex(buf.subarray(frame.off, frame.end)).slice(0, 400)}`);
        replyRtlog(frame);
        consumed = Math.max(consumed, frame.end);
      }
      if (consumed) buf = buf.subarray(consumed);
      // ۲) پیام XML (خانواده Ebio)
      if (EBIO && buf.length && /^\s*</.test(buf.toString('latin1').slice(0, 64))) {
        const text = buf.toString('utf8').trim();
        stats.ebio++;
        say(`\n[${nowStr()}] #${id} « پیام XML (Ebio) از ${peer}`);
        say(`   متن: ${text.slice(0, 600)}`);
        const m = text.match(/<TransID>([^<]*)<\/TransID>/i);
        const ev = (text.match(/<Event>([^<]*)<\/Event>/i) || [])[1] || '';
        const replyType = /keepalive/i.test(ev) ? 'KeptAlive' : 'UploadedLog';
        if (REPLY) {
          const xml = `<?xml version="1.0"?><Message><Request>${replyType}</Request><TransID>${m ? m[1] : '-1'}</TransID></Message>`;
          try { sock.write(Buffer.from(xml, 'ascii')); stats.replied++; } catch { /* */ }
          say(`[${nowStr()}] #${id} » پاسخ Ebio → ${xml}`);
        }
        buf = Buffer.alloc(0);
      }
      // ۳) هر چیز دیگر — خاموش ثبت شود
      if (buf.length) {
        stats.other++;
        const x = looksLikeXor(buf);
        say(`\n[${nowStr()}] #${id} « نامشخص (${buf.length}B) از ${peer}`);
        say(`   hex : ${hex(buf).slice(0, 400)}`);
        say(`   متن : ${textPreview(buf)}`);
        if (x) say(`   XOR(LE32(${PW})): ${hex(x).slice(0, 120)}  ← فریم SBXPC رمزشده!`);
        // بافر را نگه می‌داریم تا اگر نیمه‌فریم RTLOG بود کامل شود؛ فقط اگر خیلی بزرگ شد خالی کن
        if (buf.length > 0x10000) buf = buf.subarray(-2048);
      }
    });
    sock.on('error', (e) => say(`\n[${nowStr()}] #${id} ⚠ خطا: ${e.message}`));
    sock.on('close', () => say(`\n[${nowStr()}] ✖ اتصال #${id} بسته شد (${peer})`));
  });

  server.on('error', (e) => {
    console.error(`✗ نمی‌توان روی پورت ${listenPort} گوش داد: ${e.message}`);
    console.error('  (اگر پورت اشغال است، پورت دیگری بدهید: node scripts/np-sniff.js --listen 5006)');
    process.exit(1);
  });
  server.listen(listenPort, '0.0.0.0', () => {
    say('════════════════════════════════════════════════════════════════════');
    say('  np-sniff — حالت شنود (پوش بی‌درنگ دستگاه)');
    say(`  گوش دادن روی: 0.0.0.0:${listenPort}`);
    say(`  پاسخ خودکار: ${REPLY ? `روشن (سبک ${REPLY_STYLE})` : 'خاموش'} · Ebio XML: ${EBIO ? 'روشن' : 'خاموش'}`);
    say(`  فایل لاگ: ${LOGFILE}`);
    say('──────────────────────────────────────────────────────────────────');
    say('  مراحل:');
    say('   ۱) این پنجره را باز نگه دارید (فایروال ویندوز اجازه دسترسی بدهد).');
    say('   ۲) در منوی دستگاه، تنظیمات «سرور/آدرس مقصد پوش» را به IP همین');
    say(`      کامپیوتر و پورت ${listenPort} تغییر دهید (قبلاً به NovinHozor می‌دادید).`);
    say('   ۳) یک تردد آزمایشی روی دستگاه ثبت کنید و منتظر بمانید.');
    say('   ۴) بعد از دریافت چند فریم، Ctrl+C بزنید و فایل لاگ را برای');
    say('      پشتیبانی بفرستید (مخصوصاً بخش «hex» اولین فریم).');
    say('══════════════════════════════════════════════════════════════════');
  });

  process.on('SIGINT', () => {
    say('\n───────────────────────── جمع‌بندی ─────────────────────────');
    say(` · اتصال‌ها: ${stats.conns} · فریم RTLOG: ${stats.rtlog} · XML(Ebio): ${stats.ebio} · نامشخص: ${stats.other}`);
    say(` · پاسخ‌های ارسالی: ${stats.replied}`);
    if (stats.logIds.length) say(` · log_id های دیده‌شده: ${stats.logIds.slice(0, 20).join(', ')}${stats.logIds.length > 20 ? ' …' : ''}`);
    if (stats.rtlog === 0 && stats.ebio === 0) {
      say(' · هنوز هیچ فریمی دریافت نشده — دستگاه به این پورت وصل نمی‌شود.');
      say('   راهنما: پورت را همان عددی بگذارید که دستگاه الان برای سرور تنظیم شده،');
      say('   یا پورت دستگاه را عوض کنید. (برای کشف پورت فعلی: np-portscan.js)');
    }
    say(` · فایل کامل: ${LOGFILE} — برای پشتیبانی بفرستید. ✔`);
    try { logStream && logStream.end(); } catch { /* */ }
    process.exit(0);
  });
}

/* ═══════════════ حالت ۲: پروکسی ═══════════════ */
function runProxy() {
  let connSeq = 0;
  const server = net.createServer((client) => {
    const id = ++connSeq;
    const peer = `${client.remoteAddress}:${client.remotePort}`;
    say(`\n[${nowStr()}] ✚ #${id} نرم‌افزار از ${peer} وصل شد → تونل به ${devIp}:${devPort}`);
    const dev = net.connect({ host: devIp, port: devPort });

    dev.on('connect', () => {
      say(`[${nowStr()}] #${id} تونل برقرار شد`);
      client.on('data', (c) => {
        say(`[${nowStr()}] #${id} » (${c.length}B) ${hex(c).slice(0, 200)}`);
        const x = looksLikeXor(c);
        if (x) say(`          XOR(LE32(${PW})): ${hex(x).slice(0, 200)}`);
        dev.write(c);
      });
      dev.on('data', (c) => {
        say(`[${nowStr()}] #${id} « (${c.length}B) ${hex(c).slice(0, 200)}`);
        const x = looksLikeXor(c);
        if (x) say(`          XOR(LE32(${PW})): ${hex(x).slice(0, 200)}`);
        client.write(c);
      });
    });
    const kill = (why) => { try { client.destroy(); dev.destroy(); } catch { /* */ } say(`[${nowStr()}] ✖ #${id} ${why}`); };
    client.on('error', (e) => kill(`خطای سمت نرم‌افزار: ${e.message}`));
    dev.on('error', (e) => kill(`خطای اتصال به دستگاه: ${e.message}`));
    client.on('close', () => kill('سمت نرم‌افزار بسته شد'));
    dev.on('close', () => kill('سمت دستگاه بسته شد'));
  });
  server.on('error', (e) => { console.error(`✗ ${e.message}`); process.exit(1); });
  server.listen(listenPort, '0.0.0.0', () => {
    say('════════════════════════════════════════════════════════════════════');
    say('  np-sniff — حالت پروکسی (نرم‌افزار ↔ دستگاه)');
    say(`  تونل: 0.0.0.0:${listenPort} → ${devIp}:${devPort}`);
    say(`  فایل لاگ: ${LOGFILE}`);
    say('──────────────────────────────────────────────────────────────────');
    say('  ۱) این پنجره را باز نگه دارید.');
    say('  ۲) در NovinHozor آدرس دستگاه را به 127.0.0.1 (یا IP این کامپیوتر)');
    say(`     تغییر دهید — پورت همان ${listenPort} بماند.`);
    say('  ۳) مثل همیشه به دستگاه وصل شوید؛ همه‌ی فریم‌ها ثبت می‌شود.');
    say('  ۴) Ctrl+C → فایل لاگ را برای پشتیبانی بفرستید.');
    say('══════════════════════════════════════════════════════════════════');
  });
  process.on('SIGINT', () => {
    say(`\n · فایل کامل: ${LOGFILE} — برای پشتیبانی بفرستید. ✔`);
    try { logStream && logStream.end(); } catch { /* */ }
    process.exit(0);
  });
}

if (LISTEN) runListen();
else if (!devIp || !devPort) {
  console.error('استفاده: node scripts/np-sniff.js --listen [port]   یا   node scripts/np-sniff.js <deviceIP> <devicePort> [listenPort]');
  process.exit(1);
} else runProxy();
