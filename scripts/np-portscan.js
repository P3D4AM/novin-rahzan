#!/usr/bin/env node
/**
 * np-portscan.js — اسکن پورت‌های دستگاه حضور و غیاب (مستقل، بدون نصب)
 *
 * با کشف v1.2.6 معلوم شد دستگاه NP756+ علاوه بر پورت کششی (4238 — استاب)،
 * یک یا چند سرویس دیگر هم دارد؛ مهم‌ترین‌ها:
 *   5005  = پوش بی‌درنگ (RTLOG / Ebio) — همان که ترددها را می‌فرستد
 *   7005  = پوش Hysoon/سیگنال-آر در برخی نصب‌ها
 *   14460 = API HTTP نسل «VisibleLight»
 * این ابگر پورت‌های باز دستگاه را پیدا می‌کند تا دقیقاً ببینیم چه سرویس‌هایی دارد.
 *
 * استفاده:
 *   node scripts/np-portscan.js 192.168.1.75            ← اسکن پورت‌های رایج
 *   node scripts/np-portscan.js 192.168.1.75 --full     ← همهٔ ۶۵۵۳۵ پورت (~۱ دقیقه)
 * گزینه‌ها:
 *   TIMEOUT=400   مهلت هر اتصال (میلی‌ثانیه)
 *   CONC=200      همزمانی
 *
 * خروجی کامل را برای پشتیبانی بفرستید.
 */
'use strict';
const net = require('net');

const IP = process.argv[2] || '192.168.1.75';
const FULL = process.argv.includes('--full');
const TIMEOUT = Number(process.env.TIMEOUT || 400);
const CONC = Number(process.env.CONC || 200);

const COMMON = [
  21, 22, 23, 69, 80, 443,                       // عمومی
  4238,                                          // استاب کششی SBXPC
  4370,                                          // ZKTeco push
  5005, 7005,                                    // پوش بی‌درنگ (RTLOG / Ebio / Hysoon)
  5010, 5011, 5555, 5577, 6789,
  8000, 8008, 8080, 8081, 8443, 8886, 9100,
  10001, 1433, 14460, 20000, 30000,              // VisibleLight / SQL / …
];

const MEANING = {
  23: 'تلنت — کنسول مدیریتی (اگر باز باشد خیلی ارزشمند است)',
  80: 'وب — رابط مدیریت؟',
  443: 'وب (HTTPS)',
  4238: 'استاب قدیمی SBXPC (همان که همه‌چیز را رد می‌کند)',
  4370: 'ZKTeco push',
  5005: '★ پوش بی‌درنگ RTLOG/Ebio — کانال واقعی ترددها',
  7005: '★ پوش Hysoon/هاب — در برخی نصب‌ها همین است',
  14460: '★ API HTTP نسل VisibleLight',
  5010: 'Anviz', 5011: 'Anviz',
  5555: 'سرویس دستگاه', 5577: 'سرویس دستگاه',
  8080: 'وب — رابط مدیریت؟', 8081: 'وب',
  10001: 'سرویس دستگاه', 20000: 'سرویس', 30000: 'سرویس',
};

const ports = process.env.PORTS
  ? process.env.PORTS.split(',').map((p) => parseInt(p, 10)).filter((p) => p > 0)
  : (FULL ? Array.from({ length: 65535 }, (_, i) => i + 1) : COMMON);
const openPorts = [];

console.log('════════════════════════════════════════════════════════════════════');
console.log(`  np-portscan — اسکن ${IP}  (${FULL ? 'همهٔ پورت‌ها' : `${ports.length} پورت رایج`} · مهلت ${TIMEOUT}ms)`);
console.log('════════════════════════════════════════════════════════════════════');

function probe(port) {
  return new Promise((resolve) => {
    const s = new net.Socket();
    s.setTimeout(TIMEOUT);
    let banner = Buffer.alloc(0);
    let done = false;
    const settle = (result) => {
      if (done) return;
      done = true;
      try { s.destroy(); } catch { /* */ }
      resolve(result);
    };
    s.once('error', () => settle(null));
    s.once('timeout', () => settle(null));
    s.connect(port, IP, () => {
      // اتصال باز — مهلت را برای پنجره‌ی بنر افزایش بده تا پورت باز غلط تشخیص داده نشود
      s.setTimeout(TIMEOUT + 700);
      // اتصال باز — صبر برای بنر خودآگاه، بعد یک GET آزمایشی
      let t2 = null;
      const t = setTimeout(() => {
        if (done) return;
        try { s.write('GET / HTTP/1.0\r\n\r\n'); } catch { /* */ }
        t2 = setTimeout(() => settle({ port, banner }), 350);
      }, 250);
      s.on('data', (c) => {
        banner = Buffer.concat([banner, c]);
        if (banner.length > 160) { clearTimeout(t); if (t2) clearTimeout(t2); settle({ port, banner }); }
      });
      s.once('close', () => { clearTimeout(t); if (t2) clearTimeout(t2); settle({ port, banner }); });
    });
  });
}

let idx = 0;
async function worker() {
  while (idx < ports.length) {
    const p = ports[idx++];
    const r = await probe(p);
    if (r) openPorts.push(r);
  }
}

(async () => {
  const workers = Array.from({ length: Math.min(CONC, ports.length) }, () => worker());
  await Promise.all(workers);
  console.log();
  if (openPorts.length === 0) {
    console.log('· هیچ پورت باز پیدا نشد. دستگاه روشن است؟ IP درست است؟');
    console.log('  (اگر فایروال کامپیوتر شما restrictive است، TIMEOUT را بیشتر بگذارید: TIMEOUT=1500)');
  } else {
    console.log(`── پورت‌های باز: ${openPorts.length} ──────────────────────────────`);
    for (const { port, banner } of openPorts.sort((a, b) => a.port - b.port)) {
      const b = banner.length
        ? banner.toString('latin1').replace(/[^\x20-\x7e\n\r]/g, '·').split('\n')[0].slice(0, 80)
        : '—';
      console.log(`  :${String(port).padEnd(6)} ${MEANING[port] || 'سرویس ناشناخته'}`);
      console.log(`         بنر: ${b}`);
      if (banner.length) console.log(`         hex  : ${banner.subarray(0, 32).toString('hex').replace(/(..)/g, '$1 ')}`);
    }
    console.log('──────────────────────────────────────────');
    const push = openPorts.find((o) => o.port === 5005 || o.port === 7005);
    if (push) {
      console.log(`★ پورت ${push.port} باز است — همان کانال پوش بی‌درنگ است! برای ضبط ترافیک:`);
      console.log(`    node scripts/np-sniff.js --listen ${push.port}`);
      console.log('    و بعد آدرس «سرور» دستگاه را به IP همین کامپیوتر تغییر دهید.');
    } else {
      console.log('· پورت 5005/7005 باز نیست — پوش دستگاه فعلاً به جای دیگری می‌رود یا خاموش است.');
      console.log('  اگر پورت باز دیگری دیدید، خروجی را برای پشتیبانی بفرستید.');
      if (openPorts.length) {
        console.log('  برای ضبط ترافیک هر پورت مشکوکِ باز:');
        console.log('    node scripts/np-sniff.js --listen <پورت>');
      }
    }
  }
  console.log();
  console.log('کل خروجی را کپی و برای پشتیبانی بفرستید. ✔');
  process.exit(0);
})().catch((e) => { console.error('CRASH:', e); process.exit(1); });
