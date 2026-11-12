#!/usr/bin/env node
/**
 * test-np-v126.mjs — e2e برای ابزارهای v1.2.6:
 *   ۱) np-sniff.js حالت شنود: شبیه‌ساز rtlog-push (دستگاه 756PLUS) → شنود → ۳ ACK
 *   ۲) np-sniff.js حالت پروکسی: نرم‌افزار ↔ دستگاه کلاسیک از تونل
 *   ۳) np-portscan.js: کشف پورت‌های باز روی 127.0.0.1
 *
 * اجرا: node scripts/test-np-v126.mjs   (یا bun)
 */
import { spawn } from 'node:child_process';
import net from 'node:net';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const DIR = path.dirname(fileURLToPath(import.meta.url));
const sleep = (ms) => new Promise((r) => setTimeout(r, ms));
const results = [];
function check(name, ok, info = '') {
  results.push({ name, ok });
  console.log(`  ${ok ? '✓' : '✗'} ${name}${info ? ' — ' + info : ''}`);
}

function run(cmd, args, env = {}, cwd = os.tmpdir()) {
  const child = spawn(cmd, args, {
    cwd,
    env: { ...process.env, ...env },
    stdio: ['ignore', 'pipe', 'pipe'],
  });
  child.out = '';
  child.err = '';
  child.stdout.on('data', (c) => { child.out += c.toString(); });
  child.stderr.on('data', (c) => { child.err += c.toString(); });
  return child;
}
const kill9 = (c) => { try { c.kill('SIGKILL'); } catch { /* */ } };
async function waitExit(c, ms) {
  return new Promise((resolve) => {
    const t = setTimeout(() => resolve('timeout'), ms);
    c.once('exit', (code) => { clearTimeout(t); resolve(code); });
  });
}
async function waitFor(c, needle, ms) {
  const t0 = Date.now();
  while (Date.now() - t0 < ms) {
    if (c.out.includes(needle)) return true;
    await sleep(60);
  }
  return false;
}

/* ═══ ۱) حالت شنود + rtlog-push ═══ */
async function testListen() {
  console.log('\n[۱] np-sniff --listen + شبیه‌ساز rtlog-push');
  const tmp = fs.mkdtempSync(path.join(os.tmpdir(), 'snp-listen-'));
  const sniffer = run('node', [path.join(DIR, 'np-sniff.js'), '--listen', '45901'], {}, tmp);
  check('شنود روی 45901 بالا آمد', await waitFor(sniffer, 'گوش دادن روی', 4000), sniffer.err.slice(0, 120));

  const mock = run('node', [path.join(DIR, 'mock-sbxpc-device.js')], {
    DIALECT: 'rtlog-push', SERVER: '127.0.0.1', TARGET_PORT: '45901',
    PUSH_COUNT: '3', PUSH_INTERVAL: '250', STRICT_ACK: '1', PORT: '45999',
  });

  const code = await waitExit(mock, 15000);
  check('شبیه‌ساز با موفقیت تمام شد (exit=0)', code === 0, `exit=${code}`);
  check('۳ تردد ارسال شد', mock.out.includes('RTLOG001') && mock.out.split('» RTLOG001').length === 4);
  check('۳ ACK معتبر برگشت', mock.out.includes('ACK های معتبر: 3/3'), mock.out.split('\n').filter(l => l.includes('ACK')).join(' | ').slice(0, 120));

  // فریم‌ها در خروجی شنود ثبت شده‌اند؟
  check('شنود فریم‌ها را ثبت کرد', sniffer.out.includes('فریم RTLOG001'), sniffer.out.slice(0, 200));
  check('شنود پاسخ OK فرستاد', sniffer.out.includes('{"log_id":"1000","result":"OK"'));

  // فایل لاگ
  await sleep(200);
  const logs = fs.readdirSync(tmp).filter((f) => f.startsWith('sniff-') && f.endsWith('.log'));
  check('فایل sniff-*.log ساخته شد', logs.length === 1, logs.join(','));
  if (logs.length) {
    const txt = fs.readFileSync(path.join(tmp, logs[0]), 'utf8');
    check('لاگ شامل RTLOG001 است', txt.includes('RTLOG001'));
    check('لاگ شامل JSON تردد است', txt.includes('"verify_mode"') && txt.includes('"io_time"'));
    check('لاگ شامل hex است', /[0-9a-f]{2} [0-9a-f]{2}/.test(txt));
  }

  // خاموش کردن + جمع‌بندی
  sniffer.kill('SIGINT');
  await sleep(400);
  check('جمع‌بندی چاپ شد', sniffer.out.includes('جمع‌بندی'));
  kill9(sniffer);
  try { fs.rmSync(tmp, { recursive: true, force: true }); } catch { /* */ }
}

/* ═══ ۲) حالت پروکسی ═══ */
async function testProxy() {
  console.log('\n[۲] np-sniff حالت پروکسی (تونل نرم‌افزار ↔ دستگاه کلاسیک)');
  const mock = run('node', [path.join(DIR, 'mock-sbxpc-device.js')], { DIALECT: 'classic', PORT: '45902' });
  check('دستگاه شبیه‌ساز بالا آمد', await waitFor(mock, 'listening on 0.0.0.0:45902', 4000));

  const tmp = fs.mkdtempSync(path.join(os.tmpdir(), 'snp-proxy-'));
  const sniffer = run('node', [path.join(DIR, 'np-sniff.js'), '127.0.0.1', '45902', '45903'], {}, tmp);
  check('پروکسی روی 45903 بالا آمد', await waitFor(sniffer, 'تونل برقرار', 100) || await waitFor(sniffer, 'تونل: 0.0.0.0:45903', 4000), sniffer.err.slice(0, 120));

  // «نرم‌افزار» آزمایشی: فرمان کلاسیک 0x113 (سریال) از طریق تونل
  const MID = 1;
  const f = Buffer.alloc(16);
  f.writeUInt16LE(0xaa55, 0); f.writeUInt16LE(MID, 2); f.writeUInt16LE(0x1979, 4);
  f.writeUInt16LE(0x113, 6); f.writeUInt32LE(0, 8); f.writeUInt16LE(0, 12);
  let s = 0; for (let i = 0; i < 14; i++) s += f[i]; f.writeUInt16LE(s & 0xffff, 14);

  const reply = await new Promise((resolve) => {
    const sock = net.connect(45903, '127.0.0.1');
    const got = Buffer.alloc(0);
    let acc = Buffer.alloc(0);
    sock.on('data', (c) => {
      acc = Buffer.concat([acc, c]);
      if (acc.length >= 8 + 14 + 6 + 32) { sock.destroy(); resolve(acc); }
    });
    sock.on('connect', () => sock.write(f));
    sock.on('error', () => resolve(acc));
    setTimeout(() => { try { sock.destroy(); } catch { /* */ } resolve(acc); }, 3000);
    void got;
  });
  check('پاسخ ACK (5a a5) از تونل برگشت', reply.length >= 8 && reply[0] === 0x5a && reply[1] === 0xa5, `${reply.length}B`);
  const serialIdx = reply.indexOf(Buffer.from('7561202505176', 'ascii'));
  check('سریال (داده A5 5A) از تونل برگشت', serialIdx >= 0);

  await sleep(300);
  check('پروکسی ترافیک را لاگ کرد', sniffer.out.includes('» (16B)') && sniffer.out.includes('«'));
  const logs = fs.readdirSync(tmp).filter((x) => x.startsWith('sniff-'));
  check('فایل لاگ پروکسی ساخته شد', logs.length === 1, logs.join(','));

  kill9(sniffer); kill9(mock);
  try { fs.rmSync(tmp, { recursive: true, force: true }); } catch { /* */ }
}

/* ═══ ۳) اسکنر پورت ═══ */
async function testPortscan() {
  console.log('\n[۳] np-portscan روی 127.0.0.1 (پورت‌های باز محلی)');
  const mock = run('node', [path.join(DIR, 'mock-sbxpc-device.js')], { DIALECT: 'classic', PORT: '45912' });
  await waitFor(mock, 'listening on 0.0.0.0:45912', 4000);
  const scanner = run('node', [path.join(DIR, 'np-portscan.js'), '127.0.0.1'], {
    PORTS: '45912,45913,1', TIMEOUT: '300', CONC: '3',
  });
  const code = await waitExit(scanner, 12000);
  check('اسکنر تمام شد', code === 0 || code === null || code === 0, `exit=${code}`);
  check('پورت باز 45912 کشف شد', scanner.out.includes(':45912'));
  check('پورت بسته 45913 گزارش نشد', !scanner.out.includes(':45913'));
  check('پورت 1 گزارش نشد', !scanner.out.includes(':1 ') && !/\s:1\b/.test(scanner.out));
  check('پیشنهاد پوش چاپ شد', scanner.out.includes('np-sniff.js --listen'));
  kill9(scanner); kill9(mock);
}

/* ═══ اجرا ═══ */
const t0 = Date.now();
await testListen();
await testProxy();
await testPortscan();
const failed = results.filter((r) => !r.ok);
console.log(`\n════════ نتیجه: ${results.length - failed.length}/${results.length} تست سبز (${((Date.now() - t0) / 1000).toFixed(1)}s) ════════`);
process.exit(failed.length ? 1 : 0);
