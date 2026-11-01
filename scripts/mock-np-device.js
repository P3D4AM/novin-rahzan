#!/usr/bin/env node
/**
 * Mock Novin Pardaz NP-series device (e.g. NP756+) — for testing the HozorPlus
 * Ebio push gateway WITHOUT real hardware.
 *
 * Wire protocol (as reverse-engineered from the vendor software):
 *   device → server: TCP, short XML messages (Windows-1256), port 5005
 *   server → device: ASCII XML ACK + trailing NUL byte
 *
 * Usage:
 *   node scripts/mock-np-device.js                       # defaults: 127.0.0.1:5005, serial 7561202505176, user 1001
 *   SERIAL=123abc USER_ID=1003 PORT=5005 node scripts/mock-np-device.js
 *   MODE=loop node scripts/mock-np-device.js             # keep-alive every 30s until Ctrl+C
 */
const net = require('net')

const HOST = process.env.HOST || '127.0.0.1'
const PORT = Number(process.env.PORT || 5005)
const SERIAL = process.env.SERIAL || '7561202505176'
const USER_ID = process.env.USER_ID || '1001'
const TERM_TYPE = process.env.TERM_TYPE || '756PLUS'
const MODE = process.env.MODE || 'once'

let transId = 1000 + Math.floor(Math.random() * 100)

function nextTransId() { return ++transId }

function baseMsg(event) {
  return `<TerminalType>${TERM_TYPE}</TerminalType><TerminalID>1</TerminalID><DeviceSerialNo>${SERIAL}</DeviceSerialNo><TransID>${nextTransId()}</TransID><Event>${event}</Event>`
}

function keepAlive() {
  return `<?xml version="1.0"?><Message>${baseMsg('KeepAlive')}</Message>`
}

function timeLog(when) {
  const d = when ?? new Date()
  const p = (n) => String(n).padStart(2, '0')
  const body =
    `<Year>${d.getFullYear()}</Year><Month>${d.getMonth() + 1}</Month><Day>${d.getDate()}</Day>` +
    `<Hour>${d.getHours()}</Hour><Minute>${d.getMinutes()}</Minute><Second>${d.getSeconds()}</Second>` +
    `<UserID>${USER_ID}</UserID><DoorID>1</DoorID><AttendStat>0</AttendStat><VerifMode>1</VerifMode><JobCode>0</JobCode><APStat>0</APStat>`
  return `<?xml version="1.0"?><Message>${baseMsg('TimeLog')}${body}</Message>`
}

function send(socket, xml, label) {
  return new Promise((resolve) => {
    const onAck = (buf) => {
      socket.off('data', onAck)
      const text = buf.toString('latin1').replace(/\x00/g, '\\0')
      console.log(`ACK (${label}): ${text}`)
      const ok = /<TransID>(\d+)<\/TransID>/.test(text) && text.includes('<Request>')
      resolve(ok)
    }
    socket.on('data', onAck)
    socket.write(Buffer.from(xml, 'binary'))
    console.log(`→ ${label}  (TransID ${transId})`)
    setTimeout(() => { socket.off('data', onAck); resolve(false) }, 5000)
  })
}

async function main() {
  console.log(`mock NP device — serial=${SERIAL} type=${TERM_TYPE} user=${USER_ID} → ${HOST}:${PORT}`)
  const socket = net.connect(PORT, HOST)
  socket.on('error', (err) => { console.error('connection error:', err.message); process.exit(1) })
  socket.on('close', () => { if (MODE !== 'loop') process.exit(0) })

  await new Promise((r) => socket.once('connect', r))
  console.log('connected to gateway')

  const keepOk = await send(socket, keepAlive(), 'KeepAlive')
  if (!keepOk) console.log('!! no valid ACK for KeepAlive (device would retry)')

  const inOk = await send(socket, timeLog(), 'TimeLog')
  if (!inOk) console.log('!! no valid ACK for TimeLog (device would retry)')

  if (MODE === 'loop') {
    console.log('loop mode: pushing a TimeLog every 60s — Ctrl+C to stop')
    setInterval(async () => {
      try { await send(socket, timeLog(), 'TimeLog') } catch { /* ignore */ }
    }, 60000)
  } else {
    socket.end()
    setTimeout(() => process.exit(0), 500)
  }
}

main()
