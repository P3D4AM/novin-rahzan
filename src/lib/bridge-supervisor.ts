/**
 * FKAttend bridge supervisor (v1.5.0) — full lifecycle management of the x86
 * FK623 bridge so the user NEVER runs START-BRIDGE.bat, keeps a console open,
 * copies a token or manages processes (docs/BRIDGE-SUPERVISION.md).
 *
 * Reality it is built on: HozorPlus runs on the SAME Windows machine that can
 * reach the device (START.bat → start-windows.js → this Next.js server), so
 * the bridge is a managed HIDDEN child process of this server process.
 *
 *   Next.js server (x64 Node)
 *     └─ supervisor (this file, globalThis-singleton)
 *          └─ child: %WINDIR%\SysWOW64\...powershell.exe -File fkattend-bridge.ps1
 *               └─ FK623Attend.dll (32-bit) ──TCP 4238──▶ NP756+
 *
 * What it guarantees:
 *   * START  — hidden (windowsHide), token via ENV (never argv/process list),
 *              readiness = GET /ping, then /health with token.
 *   * SINGLE INSTANCE — supervisor is a globalThis singleton; if the default
 *              port is occupied the child cannot bind (exit code 3) and the
 *              next port in 8765..8775 is tried.
 *   * RECOVERY — unexpected child exit or failing health checks invalidate the
 *              state; the next ensureBridge() restarts it (with backoff so a
 *              broken environment cannot cause a restart storm).
 *   * CLEAN SHUTDOWN — POST /shutdown (graceful) + taskkill /PID /T /F
 *              fallback; SIGINT/SIGTERM/beforeExit/exit hooks are registered
 *              so no orphaned bridge survives the server process.
 *   * SECRECY — the token is generated here (crypto.randomBytes) and lives in
 *              memory + the child's environment ONLY. It is never in argv,
 *              URLs, logs, API responses or the database. The log ring keeps
 *              the child's own console lines (which never contain the token
 *              or the network password by design).
 *
 * Environment overrides (tests / non-Windows development):
 *   FKBRIDGE_URL + FKBRIDGE_TOKEN — use an EXTERNAL bridge (e.g. the mock
 *       bridge) instead of spawning anything. Loopback-only, validated.
 *   FKBRIDGE_COMMAND — spawn this command (split by spaces) instead of the
 *       32-bit PowerShell (used by the supervisor test suite with
 *       `node scripts/mock-fkattend-bridge.js`).
 *   FKBRIDGE_DLLDIR — explicit DLL folder (else: <root>/tools/fkattend-bridge/bin,
 *       then tools/fkattend-bridge itself; FK623Attend.dll must be there).
 *   FKBRIDGE_PORT_BASE — first port to try (default 8765, then +1 … +10).
 *   FKBRIDGE_DISABLED=1 — refuse with a clear error (tests that must not spawn).
 */
import { spawn, spawnSync, type ChildProcess } from 'child_process'
import { randomBytes } from 'crypto'
import { existsSync } from 'fs'
import path from 'path'

const PORT_BASE = Number(process.env.FKBRIDGE_PORT_BASE || 8765)
const PORT_RANGE = 10
const READY_TIMEOUT_MS = 30_000
const HEALTH_INTERVAL_MS = 30_000
const RESTART_MIN_GAP_MS = 5_000

const LOCALHOST_HOSTS = new Set(['localhost', '127.0.0.1', '::1'])

/** endpoint = where a live provider should talk to the bridge */
export interface BridgeEndpoint {
  baseUrl: string
  token: string
}

export interface BridgeStatus {
  /** an external override is in effect (tests/dev) — no child is managed */
  external: boolean
  running: boolean
  pid: number | null
  port: number | null
  version: string | null
  dllDir: string | null
  dllFound: boolean
  restarts: number
  lastError: string | null
  logTail: string[]
  platformOk: boolean
}

interface SupervisorState {
  child: ChildProcess | null
  token: string
  port: number | null
  version: string | null
  dllDir: string | null
  startedAt: number
  restarts: number
  lastError: string | null
  stopping: boolean
  readyPromise: Promise<BridgeEndpoint> | null
  logRing: string[]
  healthTimer: NodeJS.Timeout | null
  lastStartAt: number
}

const G = globalThis as { __hozorplus_bridge_supervisor?: SupervisorState }

function state(): SupervisorState {
  if (!G.__hozorplus_bridge_supervisor) {
    G.__hozorplus_bridge_supervisor = {
      child: null,
      token: '',
      port: null,
      version: null,
      dllDir: null,
      startedAt: 0,
      restarts: 0,
      lastError: null,
      stopping: false,
      readyPromise: null,
      logRing: [],
      healthTimer: null,
      lastStartAt: 0,
    }
  }
  return G.__hozorplus_bridge_supervisor
}

function pushLog(line: string) {
  const s = state()
  const clean = String(line).trim()
  if (!clean) return
  s.logRing.push(clean.slice(0, 500))
  if (s.logRing.length > 200) s.logRing.splice(0, s.logRing.length - 200)
}

function logTail(): string[] {
  return state().logRing.slice(-20)
}

// ------------------------------------------------------------------ config

function resolveRoot(): string {
  // standalone server + start-windows.js both run with cwd = install root
  return process.cwd()
}

function resolveDllDir(): { dllDir: string | null; found: boolean; reason?: string } {
  const root = resolveRoot()
  const candidates = [
    process.env.FKBRIDGE_DLLDIR || null,
    path.join(root, 'tools', 'fkattend-bridge', 'bin'),
    path.join(root, 'tools', 'fkattend-bridge'),
  ].filter((p): p is string => Boolean(p))
  for (const dir of candidates) {
    if (existsSync(path.join(dir, 'FK623Attend.dll'))) {
      return { dllDir: dir, found: true }
    }
  }
  if (candidates.length > 0) {
    return {
      dllDir: candidates[0],
      found: false,
      reason: `FK623Attend.dll not found in any of: ${candidates.join(' ; ')}`,
    }
  }
  return { dllDir: null, found: false, reason: 'no DLL directory candidates' }
}

function resolveCommand(): { command: string; args: string[] } | { error: string } {
  if (process.env.FKBRIDGE_COMMAND) {
    const parts = process.env.FKBRIDGE_COMMAND.split(/\s+/).filter(Boolean)
    if (parts.length === 0) return { error: 'FKBRIDGE_COMMAND is empty' }
    return { command: parts[0], args: parts.slice(1) }
  }
  if (process.platform !== 'win32') {
    return {
      error:
        'سرویس ارتباط دستگاه FKAttend فقط روی ویندوز اجرا می‌شود — نرم‌افزار باید روی همان سیستم ویندوزی باشد که به دستگاه دسترسی شبکه دارد',
    }
  }
  const wow64 = path.join(process.env.WINDIR || 'C:\\Windows', 'SysWOW64', 'WindowsPowerShell', 'v1.0', 'powershell.exe')
  const sys32 = path.join(process.env.WINDIR || 'C:\\Windows', 'System32', 'WindowsPowerShell', 'v1.0', 'powershell.exe')
  // 32-bit OS has no SysWOW64 — the System32 PowerShell IS 32-bit there.
  const exe = existsSync(wow64) ? wow64 : existsSync(sys32) ? sys32 : null
  if (!exe) {
    return { error: 'PowerShell ویندوز یافت نشد — نصب ویندوز را بررسی کنید' }
  }
  return {
    command: exe,
    args: ['-NoProfile', '-ExecutionPolicy', 'Bypass', '-NonInteractive', '-WindowStyle', 'Hidden'],
  }
}

function bridgeScriptPath(): string {
  return path.join(resolveRoot(), 'tools', 'fkattend-bridge', 'fkattend-bridge.ps1')
}

// ------------------------------------------------------------ HTTP helpers

async function pingPort(port: number): Promise<boolean> {
  try {
    const res = await fetch(`http://127.0.0.1:${port}/ping`, {
      method: 'GET',
      signal: AbortSignal.timeout(400),
      cache: 'no-store',
    })
    return res.ok
  } catch {
    return false
  }
}

async function postJson<T>(baseUrl: string, token: string, pathSeg: string, body: Record<string, unknown>, timeoutMs: number): Promise<T> {
  const res = await fetch(new URL(pathSeg, baseUrl), {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', 'x-bridge-token': token },
    body: JSON.stringify(body ?? {}),
    signal: AbortSignal.timeout(timeoutMs),
    cache: 'no-store',
  })
  if (res.status === 401) throw new Error('BRIDGE_AUTH')
  if (!res.ok) throw new Error(`BRIDGE_HTTP_${res.status}`)
  return (await res.json()) as T
}

// ---------------------------------------------------------------- lifecycle

async function pickFreePort(): Promise<number> {
  for (let i = 0; i < PORT_RANGE; i++) {
    const port = PORT_BASE + i
    const alive = await pingPort(port)
    if (!alive) return port
  }
  throw new Error(
    `هیچ پورت آزادی در بازهٔ ${PORT_BASE}–${PORT_BASE + PORT_RANGE - 1} برای سرویس ارتباط دستگاه پیدا نشد (پورت‌ها اشغال هستند)`,
  )
}

async function waitReady(port: number, child: ChildProcess): Promise<void> {
  const deadline = Date.now() + READY_TIMEOUT_MS
  while (Date.now() < deadline) {
    // fast-fail: the child itself died while starting (e.g. exit code 3 =
    // port already bound by someone else, 4 = DLL missing)
    if (child.exitCode != null) throw new Error(`CHILD_EXIT_${child.exitCode}`)
    if (await pingPort(port)) return
    await new Promise((r) => setTimeout(r, 300))
  }
  throw new Error('سرویس ارتباط دستگاه پاسخ‌گویی خود را شروع نکرد (timeout)')
}

interface BridgeHealth {
  ok: boolean
  version?: string
  dllDir?: string
  pid?: number
}

function killTree(pid: number | undefined): void {
  if (!pid) return
  if (process.platform === 'win32') {
    spawnSync('taskkill', ['/PID', String(pid), '/T', '/F'], { windowsHide: true })
  } else {
    try {
      process.kill(pid, 'SIGKILL')
    } catch { /* already dead */ }
  }
}

async function stopChild(s: SupervisorState): Promise<void> {
  s.stopping = true
  const child = s.child
  if (child?.pid && s.port) {
    try {
      await postJson(`http://127.0.0.1:${s.port}`, s.token, '/shutdown', {}, 3000)
    } catch {
      // graceful path failed (or never was up) → force-kill the tree
    }
    const exited = await new Promise<boolean>((resolve) => {
      const t = setTimeout(() => resolve(false), 3000)
      child.once('exit', () => { clearTimeout(t); resolve(true) })
      if (child.exitCode != null || child.killed) { clearTimeout(t); resolve(true) }
    })
    if (!exited) killTree(child.pid)
  } else if (child?.pid) {
    killTree(child.pid)
  }
  s.child = null
  s.stopping = false
}

function clearHealthTimer(s: SupervisorState) {
  if (s.healthTimer) {
    clearInterval(s.healthTimer)
    s.healthTimer = null
  }
}

function attachChild(s: SupervisorState, child: ChildProcess): void {
  s.child = child
  child.stdout?.on('data', (d: Buffer) => {
    for (const line of String(d).split(/\r?\n/)) pushLog(line)
  })
  child.stderr?.on('data', (d: Buffer) => {
    for (const line of String(d).split(/\r?\n/)) pushLog(`[stderr] ${line}`)
  })
  child.once('exit', (code, signal) => {
    if (s.child === child) {
      s.child = null
      clearHealthTimer(s)
      if (!s.stopping) {
        s.lastError = `سرویس ارتباط دستگاه به‌طور غیرمنتظره متوقف شد (کد ${code ?? 'null'}${signal ? ` / ${signal}` : ''}) — در اولین استفادهٔ بعدی راه‌اندازی مجدد می‌شود`
        pushLog(s.lastError)
      } else {
        pushLog(`bridge child exited (code ${code ?? 'null'})`)
      }
    }
  })
}

async function startBridge(): Promise<BridgeEndpoint> {
  const s = state()
  if (s.startedAt && Date.now() - s.lastStartAt < RESTART_MIN_GAP_MS && s.restarts > 0) {
    const wait = RESTART_MIN_GAP_MS - (Date.now() - s.lastStartAt)
    await new Promise((r) => setTimeout(r, wait))
  }
  s.lastStartAt = Date.now()

  // DLL presence is the most actionable failure on Windows → checked FIRST;
  // on non-Windows (no FKBRIDGE_COMMAND) the platform itself is the root cause.
  if (process.platform !== 'win32' && !process.env.FKBRIDGE_COMMAND) {
    throw new Error(
      'سرویس ارتباط دستگاه FKAttend فقط روی ویندوز اجرا می‌شود — نرم‌افزار باید روی همان سیستم ویندوزی باشد که به دستگاه دسترسی شبکه دارد',
    )
  }
  const dll = resolveDllDir()
  if (!dll.found) {
    throw new Error(
      'فایل‌های SDK دستگاه (FK623Attend.dll و DLLهای همراه) یافت نشد — آن‌ها را از پکیج SDK نوین‌پرداز در پوشهٔ tools\\fkattend-bridge\\bin کنار نرم‌افزار کپی کنید',
    )
  }

  const cmd = resolveCommand()
  if ('error' in cmd) throw new Error(cmd.error)

  const script = bridgeScriptPath()
  if (!process.env.FKBRIDGE_COMMAND && !existsSync(script)) {
    throw new Error('فایل سرویس ارتباط دستگاه (tools\\fkattend-bridge\\fkattend-bridge.ps1) در نصب یافت نشد — نصب نرم‌افزار را بررسی کنید')
  }

  // up to 3 attempts: a port that is occupied by a NON-bridge process answers
  // no /ping, our child then fails to bind (exit code 3) → try the next port
  const wasRestart = s.startedAt !== 0 // not the first start of this lifetime
  let lastErr: Error | null = null
  for (let attempt = 0; attempt < 3; attempt++) {
    const port = await pickFreePort()
    const token = randomBytes(32).toString('hex')
    const args = [...cmd.args]
    if (!process.env.FKBRIDGE_COMMAND) args.push('-File', script, '-Port', String(port))

    pushLog(`supervisor: starting bridge on port ${port}`)
    const child = spawn(cmd.command, args, {
      cwd: process.env.FKBRIDGE_COMMAND ? resolveRoot() : path.dirname(script),
      env: {
        ...process.env,
        FKBRIDGE_TOKEN: token,
        FKBRIDGE_PORT: String(port),
        FKBRIDGE_DLLDIR: dll.dllDir ?? '',
        FKBRIDGE_PROBE_EXTRAS: '0',
      },
      windowsHide: true, // no console window — invisible to the user
      stdio: ['ignore', 'pipe', 'pipe'],
    })
    attachChild(s, child)
    s.token = token
    s.port = port
    s.startedAt = Date.now()

    try {
      await waitReady(port, child)
      const health = await postJson<BridgeHealth>(`http://127.0.0.1:${port}`, token, '/health', {}, 5000)
      if (!health?.ok) throw new Error('BRIDGE_NOT_OK')
      s.version = health.version ?? null
      s.dllDir = health.dllDir ?? dll.dllDir
      s.lastError = null
      if (wasRestart) s.restarts++
    } catch (err) {
      // startup failed on this port: clean up the child, decide retry vs abort
      const tail = logTail()
      const msg = (err as Error)?.message || 'نامشخص'
      await stopChild(s)
      s.port = null
      if (msg === 'CHILD_EXIT_3' && attempt < 2) {
        pushLog(`supervisor: port ${port} busy (child exit 3) — trying the next port`)
        lastErr = new Error('پورت اشغال بود')
        continue
      }
      if (msg === 'CHILD_EXIT_4') {
        throw new Error('فایل FK623Attend.dll در مسیر SDK یافت نشد — پوشهٔ tools\\fkattend-bridge\\bin را بررسی کنید')
      }
      const detail = msg === 'BRIDGE_AUTH'
        ? 'پاسخ سلامت سرویس معتبر نبود'
        : msg === 'BRIDGE_NOT_OK' ? 'پاسخ سلامت سرویس ناموفق بود' : msg
      throw new Error(`راه‌اندازی سرویس ارتباط دستگاه ناموفق بود (${detail})${tail.length ? `\nآخرین پیام‌های سرویس:\n${tail.join('\n')}` : ''}`)
    }

    // periodic health watch — two consecutive failures invalidate the state so
    // the next ensureBridge() restarts it instead of hammering a dead bridge
    let misses = 0
    clearHealthTimer(s)
    s.healthTimer = setInterval(() => {
      const cur = state()
      if (!cur.child || cur.stopping) return
      pingPort(cur.port ?? 0)
        .then((alive) => {
          if (alive) misses = 0
          else if (++misses >= 2) {
            misses = 0
            cur.lastError = 'سرویس ارتباط دستگاه پاسخ‌گویی خود را از دست داد — در استفادهٔ بعدی راه‌اندازی مجدد می‌شود'
            pushLog(cur.lastError)
            // mark dead WITHOUT killing here (the child may just be busy) —
            // the next ensureBridge() will detect child==null and restart.
            cur.child = null
            clearHealthTimer(cur)
          }
        })
        .catch(() => { /* probe errors count as misses only via alive=false */ })
    }, HEALTH_INTERVAL_MS)
    if (typeof (s.healthTimer as unknown as { unref?: () => void }).unref === 'function') {
      ;(s.healthTimer as unknown as { unref: () => void }).unref()
    }

    return { baseUrl: `http://127.0.0.1:${port}`, token }
  }
  throw new Error(lastErr?.message || 'راه‌اندازی سرویس ارتباط دستگاه ناموفق بود')
}

/** ensure a healthy bridge exists (starts or restarts it when needed). */
export async function ensureBridge(): Promise<BridgeEndpoint> {
  const s = state()
  if (process.env.FKBRIDGE_DISABLED === '1') {
    throw new Error('سرویس ارتباط دستگاه با FKBRIDGE_DISABLED=1 خاموش شده است')
  }

  // external override (mock tests / non-Windows dev) — validated loopback only
  if (process.env.FKBRIDGE_URL && process.env.FKBRIDGE_TOKEN) {
    const host = new URL(process.env.FKBRIDGE_URL).hostname.toLowerCase()
    if (!LOCALHOST_HOSTS.has(host)) {
      throw new Error('FKBRIDGE_URL فقط روی همین سیستم (localhost) مجاز است')
    }
    return { baseUrl: new URL(process.env.FKBRIDGE_URL).origin, token: process.env.FKBRIDGE_TOKEN }
  }

  if (s.child && s.port && s.token && !s.stopping) {
    if (await pingPort(s.port)) {
      return { baseUrl: `http://127.0.0.1:${s.port}`, token: s.token }
    }
    // dead but not noticed yet → clean up, then restart below
    const deadPid = s.child.pid
    s.child = null
    clearHealthTimer(s)
    killTree(deadPid)
  }

  if (s.readyPromise) return s.readyPromise
  const p = startBridge()
    .catch((err) => {
      s.readyPromise = null
      throw err
    })
    .then((res) => {
      s.readyPromise = null
      return res
    })
  s.readyPromise = p
  return p
}

/** stop the managed bridge (graceful → force). Safe when not running. */
export async function stopBridge(): Promise<void> {
  const s = state()
  await stopChild(s)
  clearHealthTimer(s)
  s.port = null
  s.token = ''
  s.version = null
}

/**
 * Authentication trouble at runtime (stale/foreign bridge on our port):
 * drop the current state so the NEXT ensureBridge() starts fresh elsewhere.
 */
export function invalidateBridgeForRestart(): void {
  const s = state()
  s.child = null
  clearHealthTimer(s)
  s.port = null
  s.token = ''
}

/** status-only view for the API — NEVER contains the token. */
export function getBridgeStatus(): BridgeStatus {
  const s = state()
  const dll = resolveDllDir()
  const externalActive = Boolean(process.env.FKBRIDGE_URL && process.env.FKBRIDGE_TOKEN)
  return {
    external: externalActive,
    running: externalActive || Boolean(s.child && s.port && !s.stopping),
    pid: s.child?.pid ?? null,
    port: s.port,
    version: s.version,
    dllDir: dll.dllDir,
    dllFound: dll.found,
    restarts: s.restarts,
    lastError: s.lastError,
    logTail: logTail(),
    platformOk: process.platform === 'win32' || Boolean(process.env.FKBRIDGE_COMMAND || externalActive),
  }
}

/** register process shutdown hooks ONCE — no orphaned bridge survives us. */
export function registerBridgeShutdownHooks(): void {
  const g = globalThis as { __hozorplus_bridge_hooks?: boolean }
  if (g.__hozorplus_bridge_hooks) return
  g.__hozorplus_bridge_hooks = true

  const hardStop = () => {
    const s = state()
    if (s.child?.pid) killTree(s.child.pid)
    clearHealthTimer(s)
  }
  const softStop = () => { void stopBridge().catch(() => hardStop()) }

  process.once('SIGINT', softStop)
  process.once('SIGTERM', softStop)
  process.on('exit', hardStop) // synchronous only — taskkill/spawnSync
}

/** does any enabled FKATTEND device exist? (lazy db import — server only) */
export async function hasFkAttendDevice(): Promise<boolean> {
  try {
    const { db } = await import('@/lib/db')
    const n = await db.device.count({ where: { protocolFamily: 'FKATTEND', enabled: true } })
    return n > 0
  } catch {
    return false
  }
}

/** TEST-ONLY: wipe the singleton so a suite can start from a clean slate. */
export function resetBridgeSupervisorForTests(): void {
  const s = state()
  if (s.child?.pid) killTree(s.child.pid)
  clearHealthTimer(s)
  G.__hozorplus_bridge_supervisor = undefined
}
