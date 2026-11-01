#!/usr/bin/env node
/**
 * HozorPlus — Windows / standalone launcher.
 * Starts, inside a single console:
 *   1. the realtime service  (socket.io on :3003, internal emit endpoint on 127.0.0.1:3004)
 *   2. the Next.js production web server (standalone build, :3000)
 * then opens the default browser. Closing this window (or Ctrl+C) stops both.
 */
const { spawn } = require('child_process')
const path = require('path')

const root = path.resolve(__dirname, '..')
const port = process.env.PORT || '3000'
const dbUrl =
  'file:' + path.join(root, 'db', 'custom.db').replace(/\\/g, '/').replace(/ /g, '%20')

const env = Object.assign({}, process.env, {
  NODE_ENV: 'production',
  PORT: port,
  HOSTNAME: process.env.HOSTNAME || '0.0.0.0',
  DATABASE_URL: dbUrl,
})

const children = []

function tagged(buf, name) {
  const lines = String(buf).split(/\r?\n/).filter(Boolean)
  for (const line of lines) {
    // keep lines that are already tagged by the child service
    process.stdout.write(line.startsWith('[') ? line + '\n' : `[${name}] ${line}\n`)
  }
}

function start(args, name) {
  const child = spawn(process.execPath, args, { cwd: root, env, stdio: ['ignore', 'pipe', 'pipe'] })
  child.stdout.on('data', (d) => tagged(d, name))
  child.stderr.on('data', (d) => tagged(d, name))
  children.push(child)
  return child
}

const rtPort = process.env.RT_PORT || '3003'

console.log('==========================================================')
console.log('  HozorPlus | حضور+  —  production server')
console.log('----------------------------------------------------------')
console.log(`  Web UI    : http://localhost:${port}`)
console.log(`  Realtime  : socket.io on port ${rtPort}`)
console.log('  Database  : ' + dbUrl)
console.log('  Keep this window OPEN — closing it stops the app.')
console.log('==========================================================')

const rt = start([path.join('mini-services', 'realtime', 'index.js')], 'realtime')
const app = start([path.join('.next', 'standalone', 'server.js')], 'web')

// open the browser once the server has had time to boot
const timer = setTimeout(() => {
  try {
    if (process.platform === 'win32') {
      spawn('cmd', ['/c', 'start', '', `http://localhost:${port}`], {
        detached: true,
        stdio: 'ignore',
      }).unref()
    } else {
      console.log(`[hozor] open http://localhost:${port} in your browser`)
    }
  } catch {
    /* ignore */
  }
}, 5000)
if (typeof timer.unref === 'function') timer.unref()

let exiting = false
function bye(code) {
  if (exiting) return
  exiting = true
  console.log('\n[hozor] shutting down ...')
  for (const child of children) {
    try {
      child.kill()
    } catch {
      /* ignore */
    }
  }
  setTimeout(() => process.exit(code == null ? 0 : code), 500).unref()
}

process.on('SIGINT', () => bye(0))
process.on('SIGTERM', () => bye(0))
rt.on('exit', (code) => {
  console.error(`[hozor] realtime service exited (${code})`)
  bye(code || 1)
})
app.on('exit', (code) => {
  console.error(`[hozor] web server exited (${code})`)
  bye(code == null ? 1 : code)
})
