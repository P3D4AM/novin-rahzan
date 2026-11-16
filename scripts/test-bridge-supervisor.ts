/**
 * Bridge-supervisor test suite — MOCK ONLY.
 * =====================================================================
 * Exercises src/lib/bridge-supervisor.ts against the MOCK bridge
 * (scripts/mock-fkattend-bridge.js spawned via FKBRIDGE_COMMAND — the exact
 * child-process shape the supervisor uses on Windows with the 32-bit
 * PowerShell + FK623Attend.dll). NO real hardware, no PowerShell, no DLL:
 * a stub FK623Attend.dll file stands in for the DLL-presence check.
 *
 * Covered (mission §7 + §13 "تست‌های Bridge با Mock"):
 *   [lifecycle]  start → /ping readiness → /health (token) ok; second
 *                ensureBridge returns the SAME child (single instance);
 *                crash (SIGKILL) → next ensureBridge restarts (new pid,
 *                restarts counter, works again); graceful stopBridge →
 *                child exits, port released, no orphan process survives.
 *   [security]   token NEVER appears in the child's command line (argv),
 *                is delivered via the environment (/proc/<pid>/environ on
 *                Linux), and never leaks into getBridgeStatus()/log ring.
 *                /health without token → 401; /ping is the only open
 *                endpoint and reveals nothing but liveness+pid.
 *   [recovery]   port fallback: a foreign process squatting the first port
 *                → the bridge lands on the NEXT port and works.
 *   [errors]     DLL missing → actionable Persian error, no child spawned;
 *                non-Windows without any override → clear Persian error;
 *                FKBRIDGE_DISABLED=1 → refuses.
 *
 * Run (no DATABASE_URL needed — the supervisor never touches the DB):
 *   npx tsx scripts/test-bridge-supervisor.ts
 */
import { spawn } from 'child_process'
import { mkdirSync, readFileSync, rmSync, writeFileSync } from 'fs'
import http from 'http'
import path from 'path'

const ROOT = path.resolve(__dirname, '..')
const TMP = process.env.TMPDIR_BRIDGE || '/tmp/nr-bridge-sup'
const PORT_BASE = Number(process.env.FKBRIDGE_PORT_BASE || 8891)

let passed = 0
let failed = 0
const failures: string[] = []

function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}

const sleep = (ms: number) => new Promise((r) => setTimeout(r, ms))

async function fetchOk(url: string, init?: RequestInit): Promise<{ ok: boolean; status: number; body: any }> {
  try {
    const res = await fetch(url, init)
    const text = await res.text()
    let body: any = null
    try { body = JSON.parse(text) } catch { body = text }
    return { ok: res.ok, status: res.status, body }
  } catch (err) {
    return { ok: false, status: 0, body: (err as Error)?.message }
  }
}

function procAlive(pid: number | null | undefined): boolean {
  if (!pid) return false
  try { process.kill(pid, 0); return true } catch { return false }
}

/** read a /proc file of a SAME-USER process (Linux sandbox only) */
function readProc(pid: number, file: 'cmdline' | 'environ'): string | null {
  try {
    return readFileSync(`/proc/${pid}/${file}`, 'utf8')
  } catch { return null }
}

async function main() {
  mkdirSync(TMP, { recursive: true })
  // a stub SDK dir: DLL-presence is a file check — no real DLL involved
  const sdkDir = path.join(TMP, 'bin')
  rmSync(sdkDir, { recursive: true, force: true })
  mkdirSync(sdkDir, { recursive: true })
  writeFileSync(path.join(sdkDir, 'FK623Attend.dll'), 'MOCK-ONLY STUB — not a real DLL (test suite)')

  const { ensureBridge, stopBridge, getBridgeStatus, resetBridgeSupervisorForTests } = await import('../src/lib/bridge-supervisor')

  // clean slate for every section
  const reset = () => {
    resetBridgeSupervisorForTests()
    delete process.env.FKBRIDGE_URL
    delete process.env.FKBRIDGE_TOKEN
    delete process.env.FKBRIDGE_COMMAND
    delete process.env.FKBRIDGE_DLLDIR
    process.env.FKBRIDGE_PORT_BASE = String(PORT_BASE)
    process.env.FKBRIDGE_DLLDIR = sdkDir // default for most sections
    process.env.FKBRIDGE_COMMAND = `node ${path.join('scripts', 'mock-fkattend-bridge.js')}`
    delete process.env.FKBRIDGE_DISABLED
  }

  /* ================================================================
   * 1) LIFECYCLE — start, readiness, single instance, restart, stop
   * ================================================================ */
  section('LIFECYCLE — start / readiness / single instance / crash-restart / stop')
  reset()

  let ep = await ensureBridge()
  check('ensureBridge starts the mock bridge', Boolean(ep.baseUrl && ep.token))
  let st = getBridgeStatus()
  check('status: running with pid/port, DLL found', st.running && st.pid != null && st.port === PORT_BASE && st.dllFound, JSON.stringify({ running: st.running, pid: st.pid, port: st.port, dllFound: st.dllFound }))

  const ping = await fetchOk(`${ep.baseUrl}/ping`)
  check('/ping answers unauthenticated liveness', ping.ok && ping.body?.ok === true && ping.body?.pid === st.pid)

  const health = await fetchOk(`${ep.baseUrl}/health`, { method: 'POST', headers: { 'x-bridge-token': ep.token } })
  check('/health with token returns version/dllDir', health.ok && health.body?.ok === true && typeof health.body?.version === 'string')

  const healthNoTok = await fetchOk(`${ep.baseUrl}/health`, { method: 'POST' })
  check('/health WITHOUT token → 401', healthNoTok.status === 401)

  const ep2 = await ensureBridge()
  const st2 = getBridgeStatus()
  check('second ensureBridge → SAME instance (no duplicate child)', ep2.baseUrl === ep.baseUrl && st2.pid === st.pid)

  // ---- security of the child transport (argv vs environ) -------------------
  const cmdline = readProc(st.pid!, 'cmdline')
  const environ = readProc(st.pid!, 'environ')
  check('token is NOT in the child command line', !cmdline || !cmdline.includes(ep.token), cmdline?.slice(0, 80))
  check('token IS transported via the child environment', Boolean(environ && environ.includes(ep.token)))
  check('status payload carries no token', !JSON.stringify(getBridgeStatus()).includes(ep.token))

  // ---- crash → restart ----------------------------------------------------
  const deadPid = st.pid
  process.kill(deadPid!, 'SIGKILL')
  await sleep(700)
  check('killing the child is detected (not running)', !procAlive(deadPid))
  ep = await ensureBridge() // must transparently restart
  st = getBridgeStatus()
  check('next ensureBridge RESTARTS the bridge (new pid)', st.running && st.pid != null && st.pid !== deadPid, `pid ${st.pid} vs dead ${deadPid}`)
  check('restart counter incremented', st.restarts >= 1, `restarts=${st.restarts}`)
  const ping2 = await fetchOk(`${ep.baseUrl}/ping`)
  check('restarted bridge answers /ping', ping2.ok && ping2.body?.ok === true)

  // ---- graceful stop -------------------------------------------------------
  const stopPid = st.pid
  await stopBridge()
  await sleep(300)
  check('stopBridge exits the child (no orphan)', !procAlive(stopPid))
  check('status after stop: not running, port released', !getBridgeStatus().running)
  const released = await fetchOk(`${ep.baseUrl}/ping`)
  check('port is released after stop', !released.ok || released.body?.pid !== stopPid)

  /* ================================================================
   * 2) PORT FALLBACK — foreign process squats the first port
   * ================================================================ */
  section('PORT FALLBACK — first port occupied by a foreign process')
  reset()
  const squatter = http.createServer((_, res) => { res.end('i am not a bridge') })
  await new Promise<void>((r) => squatter.listen(PORT_BASE, '127.0.0.1', () => r()))
  try {
    ep = await ensureBridge()
    st = getBridgeStatus()
    check('bridge lands on the NEXT free port', st.port === PORT_BASE + 1, `port=${st.port}`)
    const p = await fetchOk(`${ep.baseUrl}/ping`)
    check('fallback-port bridge answers /ping', p.ok && p.body?.ok === true)
    await stopBridge()
  } finally {
    squatter.close()
  }

  /* ================================================================
   * 3) ERRORS — DLL missing / non-Windows / disabled
   * ================================================================ */
  section('ERRORS — DLL missing, non-Windows, disabled')
  reset()
  // FKBRIDGE_COMMAND stays set so the (Linux) platform guard is skipped and
  // the DLL-presence check is reached — that is what we are testing here.
  process.env.FKBRIDGE_DLLDIR = path.join(TMP, 'no-dll-here') // exists, but no DLL inside
  mkdirSync(path.join(TMP, 'no-dll-here'), { recursive: true })
  let err: any = null
  try { await ensureBridge() } catch (e) { err = e }
  check('DLL missing → Persian actionable error, mentions FK623Attend.dll', Boolean(err) && /FK623Attend\.dll/.test(err?.message || ''), err?.message?.slice(0, 90))

  reset()
  delete process.env.FKBRIDGE_COMMAND
  delete process.env.FKBRIDGE_DLLDIR
  err = null
  try { await ensureBridge() } catch (e) { err = e }
  check('non-Windows without override → Persian platform error', Boolean(err) && /ویندوز/.test(err?.message || ''), err?.message?.slice(0, 90))

  reset()
  process.env.FKBRIDGE_DISABLED = '1'
  err = null
  try { await ensureBridge() } catch (e) { err = e }
  check('FKBRIDGE_DISABLED=1 → refuses with clear error', Boolean(err) && /خاموش/.test(err?.message || ''))
  delete process.env.FKBRIDGE_DISABLED

  /* ================================================================
   * 4) EXTERNAL OVERRIDE — mock/dev bridges via env (no spawn)
   * ================================================================ */
  section('EXTERNAL OVERRIDE — FKBRIDGE_URL/FKBRIDGE_TOKEN (mock tests)')
  reset()
  const ext = http.createServer((req, res) => {
    if (req.url === '/ping') { res.end('{"ok":true}'); return }
    if (req.headers['x-bridge-token'] === 'ext-token') res.end('{"ok":true,"version":"ext"}')
    else { res.statusCode = 401; res.end('{"ok":false}') }
  })
  const extPort = PORT_BASE + 5
  await new Promise<void>((r) => ext.listen(extPort, '127.0.0.1', () => r()))
  try {
    process.env.FKBRIDGE_URL = `http://127.0.0.1:${extPort}`
    process.env.FKBRIDGE_TOKEN = 'ext-token'
    ep = await ensureBridge()
    const stExt = getBridgeStatus()
    check('override is used verbatim (external, no managed child)', ep.baseUrl === `http://127.0.0.1:${extPort}` && stExt.external === true && stExt.pid == null, JSON.stringify({ external: stExt.external, pid: stExt.pid }))
    const bad = await fetchOk(`${ep.baseUrl}/health`, { method: 'POST', headers: { 'x-bridge-token': 'wrong' } })
    check('override endpoint still enforces its own auth (401 wrong token)', bad.status === 401)
  } finally {
    ext.close()
    delete process.env.FKBRIDGE_URL
    delete process.env.FKBRIDGE_TOKEN
  }

  /* ================================================================
   * 5) LOG RING — child console captured, token never logged
   * ================================================================ */
  section('LOG RING — child output captured, no secrets')
  reset()
  ep = await ensureBridge()
  st = getBridgeStatus()
  check('log ring captured the mock bridge startup lines', st.logTail.some((l) => l.includes('[mock-fk-bridge]')), JSON.stringify(st.logTail.slice(0, 2)))
  check('token never appears in the log ring', !st.logTail.join('\n').includes(ep.token))
  await stopBridge()

  /* ================================================================ */
  resetBridgeSupervisorForTests()
  delete process.env.FKBRIDGE_PORT_BASE
  delete process.env.FKBRIDGE_DLLDIR
  delete process.env.FKBRIDGE_COMMAND

  console.log('\n━━━ SUMMARY (bridge supervisor — MOCK)')
  console.log(`  MOCK tests     : ${passed} passed, ${failed} failed`)
  console.log('  HARDWARE tests : 0 (none run — supervisor+real FK623Attend.dll runs on the Windows machine; see tools/fkattend-bridge/CHECKLIST-WINDOWS.md)')
  if (failed > 0) {
    console.log('  failures:', failures)
    process.exitCode = 1
  }
}

main().catch((err) => {
  console.error('SUITE CRASHED:', err)
  process.exit(1)
})
