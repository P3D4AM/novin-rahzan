#!/usr/bin/env node
/**
 * Phase 1 (v1.8.0-dev) — serialized `npm run build` with a LIVE build lock.
 * =====================================================================
 * INSTALL.bat runs `next build`; running two builds at once corrupts .next.
 * Rules implemented (installer defect list):
 *   * a lock file (.build-lock.json) records the WRAPPER's pid + start time
 *   * a second build while the holder is ALIVE → refused (exit 2) and the
 *     lock is NEVER touched
 *   * a lock whose holder is DEAD (leftover from a crashed run) → stale:
 *     renamed to .build-lock.stale-<ts> (evidence kept, never silently
 *     deleted) and the build proceeds
 *   * the lock is removed ONLY by its own holder after the child exits
 *
 * Usage:  node scripts/build-with-lock.js [npm args…]   (default: run build)
 */
'use strict'
const fs = require('fs')
const path = require('path')
const { spawn } = require('child_process')

const root = path.resolve(__dirname, '..')
const lockPath = path.join(root, '.build-lock.json')

function readLock() {
  try { return JSON.parse(fs.readFileSync(lockPath, 'utf-8')) } catch { return null }
}

function isAlive(pid) {
  if (!pid || !Number.isInteger(pid)) return false
  try { process.kill(pid, 0); return true } catch (e) { return e.code === 'EPERM' } // EPERM = alive but protected
}

function isoNow() { return new Date().toISOString() }

function acquire() {
  const existing = readLock()
  if (existing && isAlive(existing.pid)) {
    console.error(`[build-lock] یک Build دیگر همین الان در حال اجراست (PID ${existing.pid}، شروع: ${existing.startedAt}).`)
    console.error('[build-lock] این اجرا متوقف شد؛ قفل دست نخورد. تا پایان Build دیگر منتظر بمانید.')
    process.exit(2)
  }
  if (existing) {
    const stale = path.join(root, `.build-lock.stale-${Date.now()}.json`)
    try { fs.renameSync(lockPath, stale) } catch {}
    console.warn(`[build-lock] قفل باقی‌مانده از اجرای قطع‌شده یافت شد (PID ${existing.pid}) — به ${path.basename(stale)} منتقل شد و Build ادامه می‌یابد.`)
  }
  const mine = { pid: process.pid, startedAt: isoNow(), command: 'npm run build' }
  fs.writeFileSync(lockPath, JSON.stringify(mine, null, 2) + '\n', 'utf-8')
  return mine
}

function release(mine) {
  const cur = readLock()
  if (cur && cur.pid === mine.pid) {
    try { fs.unlinkSync(lockPath) } catch {}
  }
}

function main() {
  const args = process.argv.slice(2)
  const dryRun = args.includes('--dry-run') // test hook: no real npm build, just hold the lock briefly
  const mine = acquire()
  console.log(`[build-lock] قفل Build گرفته شد (PID ${mine.pid}) — شروع ${mine.startedAt}`)
  if (dryRun) {
    // hold the lock for a moment so concurrency tests can probe it, then release
    setTimeout(() => {
      release(mine)
      console.log('[build-lock] dry-run پایان یافت — قفل آزاد شد.')
      process.exit(0)
    }, 1500)
    return
  }
  const npmCmd = process.platform === 'win32' ? 'npm.cmd' : 'npm'
  const child = spawn(npmCmd, ['run', 'build', ...args], { cwd: root, stdio: 'inherit', shell: false })
  child.on('error', (e) => {
    console.error('[build-lock] اجرای npm ناموفق:', e.message)
    release(mine)
    process.exit(1)
  })
  child.on('close', (code) => {
    release(mine)
    console.log(`[build-lock] Build پایان یافت (کد ${code}) — قفل آزاد شد.`)
    process.exit(code ?? 1)
  })
}

main()
