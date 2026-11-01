/** Test SbxpcClient against the mock device (run with tsx). */
import { SbxpcClient, npTest } from '../src/lib/sbxpc'

const HOST = process.env.HOST || '127.0.0.1'
const PORT = Number(process.env.PORT || 5005)

async function main() {
  const mode = process.argv[2] || 'full'

  if (mode === 'test') {
    const r = await npTest(HOST, PORT)
    console.log('npTest:', JSON.stringify(r, null, 2))
    return
  }

  const c = new SbxpcClient(HOST, PORT)
  await c.connect()
  console.log('connected:', c.isConnected())

  const serial = await c.getSerialNumber()
  console.log('serial:', JSON.stringify(serial))

  const time = await c.getDeviceTime()
  console.log('device time:', time?.toISOString(), '| local now:', new Date().toISOString())

  await c.enableDevice(false)
  console.log('EnableDevice(false) ok')
  await c.enableDevice(true)
  console.log('EnableDevice(true) ok')

  const t0 = Date.now()
  const logs = await c.readAllLogs()
  const dt = Date.now() - t0
  console.log(`read ${logs.length} logs in ${dt}ms`)
  console.log('first:', JSON.stringify(logs[0]), '→', new Date(logs[0].ts).toISOString())
  console.log('last :', JSON.stringify(logs[logs.length - 1]), '→', new Date(logs[logs.length - 1].ts).toISOString())
  const modes = new Map<number, number>()
  for (const l of logs) modes.set(l.verifyMode, (modes.get(l.verifyMode) ?? 0) + 1)
  console.log('verify modes:', [...modes.entries()])

  c.disconnect()
  console.log('disconnected:', !c.isConnected())
}

main().catch((err) => { console.error('FAIL:', err); process.exit(1) })
