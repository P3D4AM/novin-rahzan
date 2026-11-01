#!/usr/bin/env node
/**
 * Mock Novin Pardaz NP-series device (NP756+) — SBXPC binary protocol SERVER.
 * Lets you test the HozorPlus "pull" channel (تست اتصال / دانلود ترددها)
 * WITHOUT real hardware. See scripts/SBXPC-PROTOCOL.md.
 *
 * Usage:
 *   node scripts/mock-sbxpc-device.js                          # 0.0.0.0:5005, serial 7561202505176
 *   PORT=5005 SERIAL=7561202505176 LOGS=500 USERS="1001,1002" node scripts/mock-sbxpc-device.js
 *
 * Frames (LE):
 *   PC→DEV COMMAND (16B): 55 AA | mid(2) | 79 19 | cmd(2) | arg2(4) | arg1(2) | cksum(2)
 *   DEV→PC ACK (8B):      5A A5 | mid(2) | ?(1) | 1(2) | cksum(2)     cksum=b2+b3+b4+b5+0xFF
 *   DEV→PC RESULT (14B):  AA 55 | mid(2) | ?(2) | status(2) | ret(4) | cksum(2)
 *   PC→DEV DATA:          A5 5A | mid(2) | data | cksum(2)
 *   DEV→PC DATA:          A5 5A | mid(2) | data | cksum(2)
 *   DEV→PC BIG DATA:      5A A5 | mid(2) | data(≤1020) | cksum(2)
 */
const net = require('net')

const PORT = Number(process.env.PORT || 5005)
const SERIAL = process.env.SERIAL || '7561202505176'
const LOG_COUNT = Number(process.env.LOGS || 500)
const USERS = (process.env.USERS || '1001,1002,1003,1004').split(',').map(Number)
const NOW_OFFSET_MIN = Number(process.env.NOW_OFFSET_MIN || 0) // simulate device clock drift

const EPOCH_2000 = Date.UTC(2000, 0, 1) / 1000

function makeLog(i) {
  // one punch per ~2 hours going back, cycling users; mode 1 = fingerprint
  const t = Math.floor(Date.now() / 1000) + NOW_OFFSET_MIN * 60 - i * 2 * 3600
  const userId = USERS[i % USERS.length]
  const mode = i % 4 === 3 ? 2 : 1 // occasionally "card" mode (2)
  const b = Buffer.alloc(8)
  b.writeUInt32LE(t - EPOCH_2000, 0)
  b.writeUInt16LE(userId, 4)
  b.writeUInt8(mode, 6)
  b.writeUInt8(0, 7)
  return b
}
const LOGS = Array.from({ length: LOG_COUNT }, (_, i) => makeLog(i))

function ack(mid) {
  const b = Buffer.alloc(8)
  b.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
  b.writeUInt16LE(mid, 2)
  b.writeUInt16LE(1, 4) // result = 1 (OK)
  b.writeUInt16LE((b[2] + b[3] + b[4] + b[5] + 0xff) & 0xffff, 6)
  return b
}

function result(mid, status, ret) {
  const b = Buffer.alloc(14)
  b.writeUInt16LE(0x55aa, 0) // bytes: AA 55
  b.writeUInt16LE(mid, 2)
  b.writeUInt16LE(0, 4)
  b.writeUInt16LE(status, 6)
  b.writeUInt32LE(ret >>> 0, 8)
  let sum = 0
  for (let i = 0; i < 12; i++) sum += b[i]
  b.writeUInt16LE(sum & 0xffff, 12)
  return b
}

function dataFrame(mid, payload) {
  const b = Buffer.alloc(payload.length + 6)
  b.writeUInt16LE(0x5aa5, 0) // bytes: A5 5A
  b.writeUInt16LE(mid, 2)
  payload.copy(b, 4)
  let sum = 0
  for (let i = 0; i < 4 + payload.length; i++) sum += b[i]
  b.writeUInt16LE(sum & 0xffff, 4 + payload.length)
  return b
}

function bigFrame(mid, payload) {
  const b = Buffer.alloc(payload.length + 6)
  b.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
  b.writeUInt16LE(mid, 2)
  payload.copy(b, 4)
  let sum = 0
  for (let i = 0; i < 4 + payload.length; i++) sum += b[i]
  b.writeUInt16LE(sum & 0xffff, 4 + payload.length)
  return b
}

const server = net.createServer((sock) => {
  let buf = Buffer.alloc(0)
  let pendingData = null // callback awaiting the PC's 10-byte data frame
  const ip = sock.remoteAddress
  console.log(`[mock-np] client connected: ${ip}`)

  const deliverPending = () => {
    if (pendingData && buf.length >= 10) {
      const frame = buf.subarray(0, 10)
      buf = buf.subarray(10)
      const cb = pendingData
      pendingData = null
      cb(frame)
      return true
    }
    return false
  }

  sock.on('data', (chunk) => {
    buf = Buffer.concat([buf, chunk])
    deliverPending()
    while (buf.length >= 16) {
      const f = buf.subarray(0, 16)
      buf = buf.subarray(16)
      if (f[0] !== 0x55 || f[1] !== 0xaa) continue
      const mid = f.readUInt16LE(2)
      const mark = f.readUInt16LE(4)
      const cmd = f.readUInt16LE(6)
      const arg2 = f.readUInt32LE(8)
      const arg1 = f.readUInt16LE(12)
      if (mark !== 0x1979) { console.log(`[mock-np] bad company mark ${mark.toString(16)}`); continue }
      console.log(`[mock-np] CMD ${cmd.toString(16)} arg1=${arg1} arg2=${arg2} mid=${mid}`)
      sock.write(ack(mid))

      switch (cmd) {
        case 0x113: { // GetSerialNumber
          sock.write(result(mid, 1, SERIAL.length))
          const s = Buffer.alloc(32)
          s.write(SERIAL, 0, 'ascii')
          sock.write(dataFrame(mid, s))
          break
        }
        case 0x10e: { // GetDeviceTime (packed seconds since 2000)
          sock.write(result(mid, 1, 4))
          const t = Buffer.alloc(4)
          t.writeUInt32LE(Math.floor(Date.now() / 1000) + NOW_OFFSET_MIN * 60 - EPOCH_2000, 0)
          sock.write(dataFrame(mid, t))
          break
        }
        case 0x10b: // EnableDevice(false)
        case 0x10c: // EnableDevice(true)
          sock.write(result(mid, 1, 1))
          break
        case 0x107: { // ReadAllGLogData
          if (arg1 === 0) {
            sock.write(result(mid, 1, LOGS.length))
          } else {
            // after the PC sends its 4-byte "1": reply with the count result,
            // stream the records as big-data chunks, wait for the PC's count
            // echo, then send the end marker (0x3e8).
            pendingData = () => {
              sock.write(result(mid, 1, LOGS.length))
              // protocol: chunks are exactly 0x3fc (1020) payload bytes except the
              // last one — records may straddle chunk boundaries (byte stream).
              const all = Buffer.concat(LOGS)
              for (let off = 0; off < all.length; off += 1020) {
                sock.write(bigFrame(mid, all.subarray(off, Math.min(off + 1020, all.length))))
              }
              pendingData = () => {
                console.log(`[mock-np] count echo received — sending end marker`)
                sock.write(result(mid, 1, 0x3e8))
              }
              deliverPending()
            }
          }
          break
        }
        case 0x118: // EmptyGeneralLogData
          sock.write(result(mid, 1, 1))
          break
        default:
          sock.write(result(mid, 1, 1))
      }
      deliverPending()
    }
  })
  sock.on('error', () => {})
  sock.on('close', () => console.log('[mock-np] client disconnected'))
})

server.listen(PORT, '0.0.0.0', () => {
  console.log(`[mock-np] SBXPC device simulator listening on 0.0.0.0:${PORT}`)
  console.log(`[mock-np] serial=${SERIAL} logs=${LOGS.length} users=[${USERS.join(',')}] clock-offset=${NOW_OFFSET_MIN}min`)
})
