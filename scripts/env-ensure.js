#!/usr/bin/env node
/**
 * Phase 1 (v1.8.0-dev) — INSTALL.bat .env writer with PRESERVATION.
 * =====================================================================
 * The old installer wrote .env as a single line, DESTROYING every other
 * variable (secrets, overrides) the installation had. This helper merges:
 *   * existing keys/lines are preserved verbatim (order kept)
 *   * DATABASE_URL is updated (or appended) to the requested value
 *   * duplicate DATABASE_URL lines are collapsed to one
 *   * the write is atomic (temp file + rename) — no half-written .env
 *
 * Usage:  node scripts/env-ensure.js "DATABASE_URL=file:C:/HozorPlus/db/custom.db" [--env <path>]
 *         (--env targets a specific file — used by the test suite; the
 *          installer uses the project root .env)
 * Exit:   0 ok / 2 usage / 1 io error. Prints a per-key summary — values of
 *         OTHER keys are never printed.
 */
'use strict'
const fs = require('fs')
const path = require('path')

const root = path.resolve(__dirname, '..')
const argIdx = process.argv.indexOf('--env') // index in the FULL argv
const envPath = argIdx !== -1 && process.argv[argIdx + 1] ? path.resolve(process.argv[argIdx + 1]) : path.join(root, '.env')
const relEnv = argIdx >= 2 ? argIdx - 2 : -1 // same index in argv.slice(2)
const positional = process.argv.slice(2).filter((a, i) => a !== '--env' && !(relEnv !== -1 && i === relEnv + 1))

function main() {
  const arg = positional[0]
  if (!arg || !/^DATABASE_URL=/.test(arg)) {
    console.error('usage: node scripts/env-ensure.js "DATABASE_URL=<value>"')
    process.exit(2)
  }
  const newValue = arg.slice('DATABASE_URL='.length).trim()

  const lines = fs.existsSync(envPath)
    ? fs.readFileSync(envPath, 'utf-8').split(/\r?\n/)
    : []

  const seen = new Set() // other keys that survive
  const out = []
  let urlWritten = false
  for (const line of lines) {
    const t = line.trim()
    if (/^DATABASE_URL\s*=/.test(t)) {
      if (!urlWritten) { out.push(`DATABASE_URL=${newValue}`); urlWritten = true }
      continue // collapse duplicates
    }
    if (t === '') { if (out.length && out[out.length - 1] !== '') out.push(''); continue }
    out.push(line)
    const m = t.match(/^([A-Za-z_][A-Za-z0-9_]*)\s*=/)
    if (m) seen.add(m[1])
  }
  if (!urlWritten) {
    if (out.length && out[out.length - 1] !== '') out.push('')
    out.push(`DATABASE_URL=${newValue}`)
  }
  while (out.length && out[out.length - 1] === '') out.pop()

  const tmp = envPath + '.tmp'
  fs.writeFileSync(tmp, out.join('\n') + '\n', 'utf-8')
  fs.renameSync(tmp, envPath)

  console.log(`[env-ensure] DATABASE_URL=${newValue}`)
  console.log(`[env-ensure] preserved ${seen.size} existing variable(s)${seen.size ? ': ' + Array.from(seen).join(', ') : ''}`)
  console.log(`[env-ensure] written: ${envPath}`)
}

try { main() } catch (e) {
  console.error('[env-ensure] خطا:', e.message)
  process.exit(1)
}
