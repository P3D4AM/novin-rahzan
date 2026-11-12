/* E2E (v1.2.5): all dialects incl. the LOGIN handshake (classiclogin — the real
 * NP756+ behaviour: cmd 0x52 with «رمز شبکه» before anything else works).
 * Usage: bun scripts/test-np-v125.mjs          (mocks started/stopped by the script) */
import { npTest, SbxpcClient } from '../src/lib/sbxpc.ts'
import { spawn } from 'node:child_process'

const BIN = 'node'
const MOCK = new URL('./mock-sbxpc-device.js', import.meta.url).pathname
let mockProc = null

async function startMock(env) {
  mockProc = spawn(BIN, [MOCK], { env: { ...process.env, LOGS: '500', ...env }, stdio: 'ignore' })
  await new Promise((r) => setTimeout(r, 700))
}
async function stopMock() {
  if (mockProc) { mockProc.kill('SIGKILL'); mockProc = null }
  await new Promise((r) => setTimeout(r, 300))
}

let failures = 0
const check = (cond, label) => {
  console.log(`  ${cond ? '✓' : '✗ FAIL'} ${label}`)
  if (!cond) failures++
}

async function testDialect(label, env, expect, { password, sync = true, syncThrows = false } = {}) {
  console.log(`\n===== ${label} =====`)
  await startMock(env)
  try {
    const t = await npTest('127.0.0.1', Number(env.PORT || 4238), 1, password)
    console.log(`  test: ok=${t.ok} dialect=${t.dialect} serial=${t.serialNumber} pw=${t.password}${t.logCount != null ? ` logs=${t.logCount}` : ''}${t.error ? ' err=' + t.error : ''}${t.note ? ' note=' + String(t.note).slice(0, 60) : ''}`)
    if (expect.expectFail) check(t.ok === false, 'test correctly failed (wrong password rejected)')
    else check(t.ok, 'test ok')
    if (expect.dialect) check(t.dialect === expect.dialect, `dialect = ${expect.dialect}`)
    if (expect.serialPrefix) check(String(t.serialNumber || '').startsWith(expect.serialPrefix), 'serial read')
    if (expect.password != null) check(t.password === expect.password, `password = ${expect.password}`)
    if (expect.logCount != null) check(t.logCount === expect.logCount, `logCount = ${expect.logCount}`)

    if (sync && t.ok) {
      const c = new SbxpcClient('127.0.0.1', Number(env.PORT || 4238), 1, t.password ?? null)
      try {
        await c.connect(4000)
        // reading the serial triggers probeDialect → the login happens there
        await c.getSerialNumber()
        if (syncThrows) {
          let threw = null
          try { await c.readAllLogs(30000) } catch (e) { threw = e }
          check(!!threw && /HTTP|کلاسیک رمزشده/.test(String(threw?.message)), 'readAllLogs → clear "next release" error')
        } else {
          const logs = await c.readAllLogs(30000)
          check(logs.length === 500, `logs = ${logs.length}/500`)
        }
      } finally { c.disconnect() }
    }
  } finally { await stopMock() }
}

const P = process.env.TPORT || 4238

// the headline: real NP756+ behaviour — login-gated classic
await testDialect('classiclogin · explicit password', { DIALECT: 'classiclogin', PASSWORD: '12345', PORT: P, HTTP_PORT: '0' },
  { dialect: 'CLASSIC', password: 12345, serialPrefix: '7561' }, { password: 12345 })
await testDialect('classiclogin · password auto-discovered', { DIALECT: 'classiclogin', PASSWORD: '123456', PORT: P, HTTP_PORT: '0' },
  { dialect: 'CLASSIC', password: 123456, serialPrefix: '7561' })
await testDialect('classiclogin · wrong configured password → graceful failure', { DIALECT: 'classiclogin', PASSWORD: '999000', PORT: P, HTTP_PORT: '0' },
  { expectFail: true }, { password: 4321, sync: false })

// regressions
await testDialect('classic', { DIALECT: 'classic', PORT: P }, { dialect: 'CLASSIC', serialPrefix: '7561' })
await testDialect('result (no ACK)', { DIALECT: 'result', PORT: P }, { dialect: 'RESULT_DIRECT', serialPrefix: '7561' })
await testDialect('ext (12B)', { DIALECT: 'ext', PORT: P }, { dialect: 'EXT', serialPrefix: '7561' })
await testDialect('extenc · salt=0', { DIALECT: 'extenc', PASSWORD: '123456', PORT: P, HTTP_PORT: '0' }, { dialect: 'EXT_SECURE', password: 123456, serialPrefix: '7561' })
await testDialect('extenc · salt=0x25B84', { DIALECT: 'extenc', PASSWORD: '9999', SALT: '0x25b84', PORT: P, HTTP_PORT: '0' }, { dialect: 'EXT_SECURE', password: 9999, serialPrefix: '7561' })
await testDialect('classicenc', { DIALECT: 'classicenc', PASSWORD: '12345', PORT: P, HTTP_PORT: '0' }, { dialect: 'CLASSIC_SECURE', password: 12345, serialPrefix: '7561' }, { password: 12345, syncThrows: true })
await testDialect('http / VisibleLight', { DIALECT: 'http', PASSWORD: '12345', PORT: P, HTTP_PORT: '14460' }, { dialect: 'HTTP_JSON', logCount: 500, serialPrefix: '7561' }, { password: 12345, syncThrows: true })

console.log(`\n${failures === 0 ? 'ALL PASS ✔' : failures + ' FAILURE(S) ✗'}`)
process.exit(failures === 0 ? 0 : 1)
