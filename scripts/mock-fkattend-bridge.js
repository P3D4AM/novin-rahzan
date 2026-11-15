#!/usr/bin/env node
/**
 * MOCK FKAttend bridge — test double for tools/fkattend-bridge/fkattend-bridge.ps1
 * (the real 32-bit PowerShell bridge that talks to the physical NP756+).
 *
 * Implements the exact JSON contract from tools/fkattend-bridge/CONTRACT.md:
 *   POST /health          — liveness (no token)
 *   POST /device/test     — read-only probe: serial, device time, latency
 *   POST /device/read-logs— full pull with endCode -7
 * plus MOCK-ONLY helpers used by the test suites:
 *   POST /mock/punch      — inject a new punch  { enroll, secondsAgo }
 *   POST /mock/flaky      — fail the next N device ops { failTimes }
 *   POST /mock/reset      — reset records + flakiness
 *
 * THIS IS A MOCK: no hardware is involved. Device-side records are generated
 * as Tehran wall-clock strings exactly like the real FK_GetGeneralLogData
 * output, so the HozorPlus-side conversion (wall clock → UTC) is exercised
 * for real. Records generated here are MOCK data — clearly marked in every
 * test that uses this server (DeviceSyncRun.mode = 'MOCK').
 *
 * Usage:
 *   PORT=8766 TOKEN=test-token SERIAL=7561202505176 LOGS=60 USERS=1001,7777 \
 *   node scripts/mock-fkattend-bridge.js
 */
const http = require('http')

const PORT = Number(process.env.PORT || 8766)
const TOKEN = process.env.TOKEN || 'test-token'
const SERIAL = process.env.SERIAL || '7561202505176'
const LOGS = Number(process.env.LOGS || 60)
const USERS = (process.env.USERS || '1001,7777').split(',').map(Number)
const DEVICE_PORT = Number(process.env.DEVICE_PORT || 4238) // echoed in meta for config-propagation tests
const FAIL_CONNECT = Number(process.env.FAIL_CONNECT || 0) // start already failing

// Tehran wall clock "now" (Iran abolished DST in 2022 → fixed +03:30 today;
// the HozorPlus conversion uses the full tz database and stays correct for
// historical DST eras — mock only needs "now" anyway)
function tehranWallClock(ms) {
  const t = new Date(ms + 3.5 * 3600 * 1000) // interpret as UTC after shifting
  const p = (n) => String(n).padStart(2, '0')
  return `${t.getUTCFullYear()}-${p(t.getUTCMonth() + 1)}-${p(t.getUTCDate())} ${p(t.getUTCHours())}:${p(t.getUTCMinutes())}:${p(t.getUTCSeconds())}`
}

/** @type {Array<{enrollNumber:number, verifyMode:number, inOutMode:number, workCode:number, timestamp:string}>} */
let records = []
let failTimes = FAIL_CONNECT
let opCount = 0

function regen() {
  records = []
  for (let i = 0; i < LOGS; i++) {
    const ms = Date.now() - i * 2 * 3600 * 1000
    records.push({
      enrollNumber: USERS[i % USERS.length],
      verifyMode: 1 + (i % 3),
      inOutMode: 1,
      workCode: 0,
      timestamp: tehranWallClock(ms),
    })
  }
}
regen()

function json(res, status, body) {
  const bytes = Buffer.from(JSON.stringify(body))
  res.writeHead(status, { 'Content-Type': 'application/json; charset=utf-8' })
  res.end(bytes)
}

function readBody(req) {
  return new Promise((resolve) => {
    let b = ''
    req.on('data', (c) => (b += c))
    req.on('end', () => {
      try { resolve(JSON.parse(b || '{}')) } catch { resolve({}) }
    })
  })
}

function metaEcho(b) {
  return {
    machine: Number(b.machine ?? 1),
    ip: String(b.ip ?? ''),
    port: Number(b.port ?? DEVICE_PORT),
    timeoutMs: Number(b.timeoutMs ?? 5000),
    license: Number(b.license ?? 1261),
  }
}

const server = http.createServer(async (req, res) => {
  const path = req.url.split('?')[0].replace(/\/$/, '')
  const t0 = Date.now()
  const isMockOnly = path.startsWith('/mock/')

  if (path === '/health') {
    return json(res, 200, { ok: true, version: 'mock-1.4.0', dllDir: '(mock)', uptimeSec: Math.round(process.uptime()), pid: process.pid, probeExtras: true })
  }

  const token = req.headers['x-bridge-token']
  if (token !== TOKEN) {
    return json(res, 401, { ok: false, error: 'توکن Bridge نامعتبر است', code: 'AUTH' })
  }

  const b = await readBody(req)

  if (path === '/device/test') {
    if (failTimes > 0) {
      failTimes--
      return json(res, 200, { ok: false, error: 'اتصال به دستگاه برقرار نشد (کد -101 از FK_ConnectNet) — MOCK failure injection', code: 'CONNECT_FAILED' })
    }
    opCount++
    // serial/time like the isolated probe would report
    return json(res, 200, {
      ok: true,
      serialNumber: SERIAL,
      deviceTime: tehranWallClock(Date.now()),
      latencyMs: Math.max(1, Math.round(Date.now() - t0) + 37),
      meta: metaEcho(b),
    })
  }

  if (path === '/device/read-logs') {
    if (failTimes > 0) {
      failTimes--
      return json(res, 200, { ok: false, error: 'اتصال به دستگاه برقرار نشد (کد -101 از FK_ConnectNet) — MOCK failure injection', code: 'CONNECT_FAILED' })
    }
    opCount++
    return json(res, 200, {
      ok: true,
      serialNumber: SERIAL,
      deviceTime: tehranWallClock(Date.now()),
      latencyMs: Math.max(1, Math.round(Date.now() - t0) + 120),
      connectMs: 42,
      endCode: -7, // normal end of reading — as observed on the real device
      records: [...records], // full memory dump; HozorPlus dedupes by fingerprint
      meta: metaEcho(b),
    })
  }

  if (isMockOnly) {
    if (path === '/mock/punch') {
      const enroll = Number(b.enroll ?? USERS[0])
      const secondsAgo = Number(b.secondsAgo ?? 0)
      records.push({
        enrollNumber: enroll,
        verifyMode: Number(b.verifyMode ?? 1),
        inOutMode: Number(b.inOutMode ?? 1),
        workCode: Number(b.workCode ?? 0),
        timestamp: tehranWallClock(Date.now() - secondsAgo * 1000),
      })
      return json(res, 200, { ok: true, total: records.length })
    }
    if (path === '/mock/flaky') {
      failTimes = Number(b.failTimes ?? 1)
      return json(res, 200, { ok: true, failTimes })
    }
    if (path === '/mock/reset') {
      regen()
      failTimes = 0
      return json(res, 200, { ok: true, total: records.length })
    }
    return json(res, 404, { ok: false, error: 'unknown mock endpoint' })
  }

  return json(res, 404, { ok: false, error: 'مسیر یافت نشد', code: 'NOT_FOUND' })
})

server.listen(PORT, '127.0.0.1', () => {
  console.log(`[mock-fk-bridge] MOCK ONLY (no hardware) — 127.0.0.1:${PORT} token=${TOKEN}`)
  console.log(`[mock-fk-bridge] serial=${SERIAL} records=${records.length} users=[${USERS.join(',')}] failTimes=${FAIL_CONNECT}`)
})
