#!/usr/bin/env node
/**
 * Mock Novin Pardaz NP-series device (NP756+) — SBXPC binary protocol SERVER.
 * Lets you test the HozorPlus "pull" channel (تست اتصال / دانلود ترددها)
 * WITHOUT real hardware. See scripts/SBXPC-PROTOCOL.md.
 *
 * Usage:
 *   node scripts/mock-sbxpc-device.js                          # classic dialect, 0.0.0.0:5005
 *   DIALECT=classiclogin node scripts/mock-sbxpc-device.js  # classic + 0x52 password LOGIN (real NP756+ behaviour!)
 *   DIALECT=ext         node scripts/mock-sbxpc-device.js      # 12-byte EXT dialect (plaintext)
 *   DIALECT=extenc      node scripts/mock-sbxpc-device.js      # password-encrypted EXT (real NP756+ behaviour)
 *   DIALECT=classicenc  node scripts/mock-sbxpc-device.js      # password-encrypted CLASSIC 16B frames
 *   DIALECT=http        node scripts/mock-sbxpc-device.js      # VisibleLight HTTP API (:14460) + binary stub
 *   DIALECT=result      node scripts/mock-sbxpc-device.js      # classic frames, no ACK
 *   DIALECT=rtlog-push  SERVER=127.0.0.1 TARGET_PORT=5005 node scripts/mock-sbxpc-device.js
 *                      # 756PLUS real-time PUSH simulator (v1.2.6): the DEVICE connects out
 *                      # to the "server" and sends RTLOG001-framed JSON punches, expecting
 *                      # {"log_id":N,"result":"OK"} replies — pair with:
 *                      #   node scripts/np-sniff.js --listen 5005
 *   PORT=4238 SERIAL=7561202505176 LOGS=500 USERS="1001,1002" node scripts/mock-sbxpc-device.js
 *   DIALECT=extenc PASSWORD=123456 node scripts/mock-sbxpc-device.js
 *   DIALECT=http PASSWORD=12345 node scripts/mock-sbxpc-device.js
 *
 * Dialects:
 *   classic (default)  cmd(16B) → ACK(8B 5AA5) → RESULT(14B AA55) → DATA(A55A)
 *   result             cmd(16B) → RESULT(14B AA55) → DATA(A55A)            [no ACK]
 *   ext                cmd(12B 55AA|mid|cmd4|arg2|cksum) → ACK(12B AA55|mid|dword4|cksum) → DATA(A55A)
 *                      PC→DEV ACK frames (AA 55 …) are accepted and ignored.
 *   classiclogin      EXACTLY what the real NP756+ does (v1.2.5 discovery): the vendor's
 *                      StartTcpX sends a classic LOGIN first — cmd 0x52 with the «رمز شبکه»
 *                      as arg2, plaintext — and the device replies with a 14B RESULT
 *                      (status≠0 = OK, status=0 = wrong password). Until you log in, EVERY
 *                      other classic command is refused with the 10-byte ack
 *                      AA 55|mid|0x10|cksum-echo and EXT/encrypted frames get silence.
 *   extenc             NP756+ real behaviour: classic 16B frames are refused with the
 *                      10-byte ack  AA 55|mid|0x10|cksum-of-refused-cmd ; plaintext EXT
 *                      frames are silently dropped; only frames XOR-encrypted with
 *                      key = LE32(PASSWORD + SALT) are honoured (SALT env, default 0 —
 *                      the DLL default). Data blocks are
 *                      [payload][trailer: 5A A5|mid|Σpayload(4)|?(2)|cksum(2)], encrypted.
 *   classicenc         hypothesis under test: the WHOLE classic exchange (16B commands
 *                      and all replies) is XOR-encrypted with key=LE32(PASSWORD+SALT).
 *                      Plaintext classic frames get the 10-byte refusal ack.
 *   http               "VisibleLight" generation: HTTP JSON API on HTTP_PORT (14460) —
 *                      POST /getDeviceKey {"pass":N} → {"result":true,"data":"…"}
 *                      (exactly what PacketParserStandard.dll speaks); the binary port
 *                      is a legacy stub that refuses classic frames like the real NP756+.
 *
 * Frames (LE):
 *   PC→DEV COMMAND (classic, 16B): 55 AA | mid(2) | 79 19 | cmd(2) | arg2(4) | arg1(2) | cksum(2)
 *   PC→DEV COMMAND (EXT, 12B):     55 AA | mid(2) | cmd(4) | arg(2) | cksum(2)
 *   DEV→PC ACK (classic, 8B):      5A A5 | mid(2) | 1(2) | cksum(2)   cksum=b2+b3+b4+b5+0xFF
 *   DEV→PC ACK (EXT, 12B):         AA 55 | mid(2) | dword(4) | cksum(2)  cksum=Σ[0..9]
 *   DEV→PC RESULT (14B):           AA 55 | mid(2) | ?(2) | status(2) | ret(4) | cksum(2)
 *   PC↔DEV DATA:                   A5 5A | mid(2) | data | cksum(2)
 *   DEV→PC BIG DATA:               5A A5 | mid(2) | data(≤1020) | cksum(2)
 */
const net = require('net')
const http = require('http')

const PORT = Number(process.env.PORT || 5005)
const HTTP_PORT = Number(process.env.HTTP_PORT || 14460)
const SERIAL = process.env.SERIAL || '7561202505176'
const LOG_COUNT = Number(process.env.LOGS || 500)
const USERS = (process.env.USERS || '1001,1002,1003,1004').split(',').map(Number)
const NOW_OFFSET_MIN = Number(process.env.NOW_OFFSET_MIN || 0) // simulate device clock drift
const DIALECT = (process.env.DIALECT || 'classic').toLowerCase() // classic | classiclogin | result | ext | extenc | classicenc | http | rtlog-push
const PASSWORD = Number(process.env.PASSWORD || 0) // «رمز شبکه» for extenc/classicenc/http
const SALT = Number(process.env.SALT || 0) // crypto salt: 0 = DLL default; 0x25B84 = legacy variant
const EXT_CRYPTO_MAGIC = SALT

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

function sum16(b, to) {
  let s = 0
  for (let i = 0; i < to; i++) s += b[i]
  return s & 0xffff
}

function ack(mid) {
  const b = Buffer.alloc(8)
  b.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
  b.writeUInt16LE(mid, 2)
  b.writeUInt16LE(1, 4) // result = 1 (OK)
  b.writeUInt16LE((b[2] + b[3] + b[4] + b[5] + 0xff) & 0xffff, 6)
  return b
}

/** EXT 12-byte device ack: AA 55 | mid | dword | cksum = (Σ[2..9] + 0xFF) — SBPCCOMM64 rule */
function ackExt(mid, dword) {
  const b = Buffer.alloc(12)
  b.writeUInt16LE(0x55aa, 0) // bytes: AA 55
  b.writeUInt16LE(mid, 2)
  b.writeUInt32LE(dword >>> 0, 4)
  let s = 0
  for (let i = 2; i < 10; i++) s += b[i]
  b.writeUInt16LE((s + 0xff) & 0xffff, 10)
  return b
}

function result(mid, status, ret) {
  const b = Buffer.alloc(14)
  b.writeUInt16LE(0x55aa, 0) // bytes: AA 55
  b.writeUInt16LE(mid, 2)
  b.writeUInt16LE(0, 4)
  b.writeUInt16LE(status, 6)
  b.writeUInt32LE(ret >>> 0, 8)
  b.writeUInt16LE(sum16(b, 12), 12)
  return b
}

function dataFrame(mid, payload) {
  const b = Buffer.alloc(payload.length + 6)
  b.writeUInt16LE(0x5aa5, 0) // bytes: A5 5A
  b.writeUInt16LE(mid, 2)
  payload.copy(b, 4)
  b.writeUInt16LE(sum16(b, 4 + payload.length), 4 + payload.length)
  return b
}

function bigFrame(mid, payload) {
  const b = Buffer.alloc(payload.length + 6)
  b.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
  b.writeUInt16LE(mid, 2)
  payload.copy(b, 4)
  b.writeUInt16LE(sum16(b, 4 + payload.length), 4 + payload.length)
  return b
}

/* ---------- EXT_SECURE (password-encrypted) helpers ---------- */

function keyFor(pw) {
  const k = (pw + EXT_CRYPTO_MAGIC) >>> 0
  return Buffer.from([k & 0xff, (k >>> 8) & 0xff, (k >>> 16) & 0xff, (k >>> 24) & 0xff])
}

function xor(buf, key) {
  const out = Buffer.from(buf)
  for (let i = 0; i < out.length; i++) out[i] ^= key[i % 4]
  return out
}

/** 10-byte "NP legacy refusal": AA 55 | mid | 0x10 | cksum-of-the-refused-classic-frame */
function npRefusalAck(mid, cmdCksum) {
  const b = Buffer.alloc(10)
  b.writeUInt16LE(0x55aa, 0) // bytes: AA 55
  b.writeUInt16LE(mid, 2)
  b.writeUInt32LE(0x10, 4)
  b.writeUInt16LE(cmdCksum, 8)
  return b
}

/** encrypted EXT data block: [payload][trailer 12B], each part encrypted separately */
function encBlock(payload, mid, key) {
  const tr = Buffer.alloc(12)
  tr.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
  tr.writeUInt16LE(mid, 2)
  tr.writeUInt32LE(sum16(payload, payload.length), 4)
  // trailer cksum = Σ[2..9] + 0xFF  (≡ Σ[0..9], since 5A+A5 = 0xFF)
  let s = 0
  for (let i = 2; i < 10; i++) s += tr[i]
  tr.writeUInt16LE((s + 0xff) & 0xffff, 10)
  return Buffer.concat([xor(payload, key), xor(tr, key)])
}

const server = net.createServer((sock) => {
  let buf = Buffer.alloc(0)
  let pendingData = null // callback awaiting the PC's 10-byte data frame
  const ip = sock.remoteAddress
  console.log(`[mock-np] client connected: ${ip}`)

  const _rawWrite = sock.write.bind(sock) // true socket write (pre-encryption)
  // classicenc: encrypt every byte we send
  if (DIALECT === 'classicenc') {
    const key = keyFor(PASSWORD)
    sock.write = (b) => _rawWrite(xor(Buffer.from(b), key))
  }

  // classiclogin: per-connection login state (the session lives on the socket)
  let loggedIn = DIALECT !== 'classiclogin'

  /** classic command dispatcher — f is a DECRYPTED 16B command frame */
  function dispatchClassic(f) {
    const mid = f.readUInt16LE(2)
    const cmd = f.readUInt16LE(6)
    const arg2 = f.readUInt32LE(8)
    const arg1 = f.readUInt16LE(12)
    console.log(`[mock-np] CMD ${cmd.toString(16)} arg1=${arg1} arg2=${arg2} mid=${mid}`)
    if (DIALECT === 'classic' || DIALECT === 'classicenc' || DIALECT === 'classiclogin') sock.write(ack(mid))

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
  }

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

  /* ------------------------------------------------------------------ */
  /* EXT dialect (12-byte frames)                                        */
  /* ------------------------------------------------------------------ */
  function handleExtFrame(f) {
    if (f[0] !== 0x55 || f[1] !== 0xaa) return false
    const mid = f.readUInt16LE(2)
    const cmd = f.readUInt32LE(4)
    const arg = f.readUInt16LE(8)
    const expected = (mid & 0xff) + (mid >> 8) + (f[4] + f[5] + f[6] + f[7]) + f[8] + f[9] + 0xff
    if (f.readUInt16LE(10) !== (expected & 0xffff)) {
      // real NP756+ behaviour: answer a garbled frame with error code 0x10
      console.log('[mock-np] EXT bad checksum → error ack 0x10')
      sock.write(ackExt(mid, 0x10))
      return true
    }
    console.log(`[mock-np] EXT CMD ${cmd.toString(16)} arg=${arg} mid=${mid}`)

    switch (cmd) {
      case 0x113: { // GetSerialNumber
        sock.write(ackExt(mid, 1))
        const s = Buffer.alloc(32)
        s.write(SERIAL, 0, 'ascii')
        sock.write(dataFrame(mid, s))
        break
      }
      case 0x10e: { // GetDeviceTime
        sock.write(ackExt(mid, 1))
        const t = Buffer.alloc(4)
        t.writeUInt32LE(Math.floor(Date.now() / 1000) + NOW_OFFSET_MIN * 60 - EPOCH_2000, 0)
        sock.write(dataFrame(mid, t))
        break
      }
      case 0x10b:
      case 0x10c:
        sock.write(ackExt(mid, 1))
        break
      case 0x107: { // ReadAllGLogData
        if (arg === 0) {
          sock.write(ackExt(mid, LOGS.length))
        } else {
          sock.write(ackExt(mid, LOGS.length))
          const all = Buffer.concat(LOGS)
          for (let off = 0; off < all.length; off += 1020) {
            sock.write(bigFrame(mid, all.subarray(off, Math.min(off + 1020, all.length))))
          }
        }
        break
      }
      default:
        // unknown command → error code 0x10 (what real NP756+ devices do)
        sock.write(ackExt(mid, 0x10))
    }
    return true
  }

  /** PC→DEV EXT ack frame (AA 55 …) — accept and ignore */
  function isExtAckFrame(f) {
    return f.length >= 12 && f[0] === 0xaa && f[1] === 0x55
  }

  sock.on('data', (chunk) => {
    buf = Buffer.concat([buf, chunk])
    deliverPending()

    /* ---- EXT_SECURE dialect (password-encrypted, real NP756+) ---- */
    if (DIALECT === 'extenc') {
      const key = keyFor(PASSWORD)
      for (;;) {
        // (1) classic 16B frame → refuse (device only speaks encrypted EXT)
        if (buf.length >= 16) {
          const f = buf.subarray(0, 16)
          const mid = f.readUInt16LE(2)
          const mark = f.readUInt16LE(4)
          if (f[0] === 0x55 && f[1] === 0xaa && mark === 0x1979
            && f.readUInt16LE(14) === sum16(f, 14)) {
            buf = buf.subarray(16)
            console.log(`[mock-np] classic frame refused (cksum ${f.readUInt16LE(14).toString(16)}) → 10-byte error ack 0x10`)
            sock.write(npRefusalAck(mid, f.readUInt16LE(14)))
            continue
          }
        }
        // (2) encrypted 12B EXT frame
        if (buf.length >= 12) {
          const dec = xor(buf.subarray(0, 12), key)
          const isPcAck = dec[0] === 0xaa && dec[1] === 0x55
          const mid = dec.readUInt16LE(2)
          const cmd = dec.readUInt32LE(4)
          const arg = dec.readUInt16LE(8)
          const expected = ((() => { let s = 0; for (let i = 2; i < 10; i++) s += dec[i]; return s })() + 0xff) & 0xffff
          if (dec[0] === 0x55 && dec[1] === 0xaa && dec.readUInt16LE(10) === expected) {
            buf = buf.subarray(12)
            console.log(`[mock-np] ENC CMD ${cmd.toString(16)} arg=${arg} mid=${mid}`)
            switch (cmd) {
              case 0x113: { // GetSerialNumber
                sock.write(xor(ackExt(mid, 1), key))
                const s = Buffer.alloc(32)
                s.write(SERIAL, 0, 'ascii')
                sock.write(encBlock(s, mid, key))
                break
              }
              case 0x10e: { // GetDeviceTime
                sock.write(xor(ackExt(mid, 1), key))
                const t = Buffer.alloc(4)
                t.writeUInt32LE(Math.floor(Date.now() / 1000) + NOW_OFFSET_MIN * 60 - EPOCH_2000, 0)
                sock.write(encBlock(t, mid, key))
                break
              }
              case 0x10b:
              case 0x10c:
                sock.write(xor(ackExt(mid, 1), key))
                break
              case 0x107: { // ReadAllGLogData — one encrypted block + trailer
                if (arg === 0) {
                  sock.write(xor(ackExt(mid, LOGS.length), key))
                } else {
                  sock.write(xor(ackExt(mid, LOGS.length), key))
                  sock.write(encBlock(Buffer.concat(LOGS), mid, key))
                }
                break
              }
              default:
                sock.write(xor(ackExt(mid, 0x10), key))
            }
            continue
          }
          if (isPcAck) { // encrypted PC ack — swallow
            buf = buf.subarray(12)
            console.log('[mock-np] swallowed encrypted PC ack frame')
            continue
          }
        }
        // (3) nothing recognizable: ≥16 bytes → resync (drop); otherwise wait (silence)
        if (buf.length >= 16) {
          console.log('[mock-np] garbled encrypted frame — dropped (silence, like the real device)')
          buf = Buffer.alloc(0)
          continue
        }
        break
      }
      return
    }

    // EXT dialect: 12-byte command frames (and PC ack frames to swallow)
    if (DIALECT === 'ext') {
      for (;;) {
        if (buf.length < 12) break
        const f = buf.subarray(0, 12)
        buf = buf.subarray(12)
        if (isExtAckFrame(f)) { console.log('[mock-np] swallowed PC ack frame'); continue }
        handleExtFrame(f)
      }
      return
    }

    // classicenc: classic 16B frames arrive XOR-encrypted; plaintext ones are refused
    if (DIALECT === 'classicenc') {
      const key = keyFor(PASSWORD)
      for (;;) {
        if (buf.length < 16) break
        const dec = xor(buf.subarray(0, 16), key)
        const validEnc = dec[0] === 0x55 && dec[1] === 0xaa && dec.readUInt16LE(4) === 0x1979
          && dec.readUInt16LE(14) === sum16(dec, 14)
        const raw = buf.subarray(0, 16)
        const validPlain = raw[0] === 0x55 && raw[1] === 0xaa && raw.readUInt16LE(4) === 0x1979
          && raw.readUInt16LE(14) === sum16(raw, 14)
        buf = buf.subarray(16)
        if (validEnc) { dispatchClassic(dec) ; continue }
        if (validPlain) {
          console.log('[mock-np] plaintext classic frame — refused with 10-byte ack (device expects encryption)')
          _rawWrite(npRefusalAck(raw.readUInt16LE(2), raw.readUInt16LE(14)))
          continue
        }
        console.log('[mock-np] garbled encrypted classic frame — dropped (silence)')
      }
      return
    }

    // http dialect: binary port is a legacy stub — parse classic, always refuse
    if (DIALECT === 'http') {
      if (buf.length >= 16) {
        const raw = buf.subarray(0, 16)
        const validPlain = raw[0] === 0x55 && raw[1] === 0xaa && raw.readUInt16LE(4) === 0x1979
          && raw.readUInt16LE(14) === sum16(raw, 14)
        console.log('[mock-np] binary stub: classic frame → 10-byte refusal ack (HTTP-only device)')
        if (validPlain) sock.write(npRefusalAck(raw.readUInt16LE(2), raw.readUInt16LE(14)))
        else sock.write(npRefusalAck(1, 0))
        buf = Buffer.alloc(0)
      }
      return
    }

    // classic / result / classiclogin dialects: 16-byte command frames
    while (buf.length >= 16) {
      const f = buf.subarray(0, 16)
      buf = buf.subarray(16)
      if (f[0] !== 0x55 || f[1] !== 0xaa) continue
      const mid = f.readUInt16LE(2)
      const mark = f.readUInt16LE(4)
      if (mark !== 0x1979) { console.log(`[mock-np] bad company mark ${mark.toString(16)}`); continue }

      // ---- classiclogin: the LOGIN command (0x52, password in arg2) — the vendor's
      //      StartTcpX handshake; exactly what the real NP756+ expects ----
      if (DIALECT === 'classiclogin' && f.readUInt16LE(6) === 0x52) {
        const pw = f.readUInt32LE(8)
        if (pw === PASSWORD) {
          loggedIn = true
          console.log(`[mock-np] LOGIN 0x52 password=${pw} → RESULT status=1 (OK)`)
          sock.write(result(mid, 1, 1))
        } else {
          console.log(`[mock-np] LOGIN 0x52 password=${pw} (wrong, want ${PASSWORD}) → RESULT status=0`)
          sock.write(result(mid, 0, 0))
        }
        continue
      }

      // not logged in yet → the real device's 10-byte refusal ack (code 0x10)
      if (!loggedIn) {
        console.log(`[mock-np] CMD ${f.readUInt16LE(6).toString(16)} refused — not logged in → 10-byte ack 0x10`)
        sock.write(npRefusalAck(mid, f.readUInt16LE(14)))
        continue
      }

      dispatchClassic(f)
      deliverPending()
    }
  })
  sock.on('error', () => {})
  sock.on('close', () => console.log('[mock-np] client disconnected'))
})

if (DIALECT !== 'rtlog-push') server.listen(PORT, '0.0.0.0', () => {
  console.log(`[mock-np] SBXPC device simulator listening on 0.0.0.0:${PORT}`)
  console.log(`[mock-np] dialect=${DIALECT}${['extenc', 'classicenc', 'http', 'classiclogin'].includes(DIALECT) ? ` password=${PASSWORD} salt=${SALT}` : ''} serial=${SERIAL} logs=${LOGS.length} users=[${USERS.join(',')}] clock-offset=${NOW_OFFSET_MIN}min`)
})

/* ------------------------------------------------------------------ */
/* "VisibleLight" HTTP JSON API (NP756P/NP761P generation) — mirrors  */
/* PacketParserStandard.dll: POST http://ip:14460/<endpoint>         */
/* with {"pass":N} → {"result":bool,"data":"<json-string|text>"}        */
/* ------------------------------------------------------------------ */
if (DIALECT === 'http') {
  const json = (res, code, obj) => {
    const b = Buffer.from(JSON.stringify(obj))
    res.writeHead(code, { 'Content-Type': 'application/json', 'Content-Length': b.length })
    res.end(b)
  }
  const readBody = (req, cb) => {
    let data = ''
    req.on('data', (c) => { data += c })
    req.on('end', () => { try { cb(JSON.parse(data || '{}')) } catch { cb({}) } })
  }
  const ok = (data) => JSON.stringify({ result: true, data: typeof data === 'string' ? data : JSON.stringify(data) })
  const fail = (why) => JSON.stringify({ result: false, data: JSON.stringify({ error: why }) })

  const httpServer = http.createServer((req, res) => {
    const passOk = (body) => Number(body.pass) === PASSWORD || String(body.pass) === String(PASSWORD)
    readBody(req, (body) => {
      console.log(`[mock-np] HTTP ${req.method} ${req.url} pass=${body.pass ?? '—'}`)
      if (!passOk(body)) return json(res, 200, JSON.parse(fail('password')))
      switch (req.url) {
        case '/getDeviceKey':
          return json(res, 200, JSON.parse(ok(SERIAL)))
        case '/getLogCount':
          return json(res, 200, JSON.parse(ok(JSON.stringify({ count: LOGS.length }))))
        case '/getDeviceTime':
          return json(res, 200, JSON.parse(ok(new Date(Date.now() + NOW_OFFSET_MIN * 60000).toISOString())))
        case '/getDeviceInfo':
          return json(res, 200, JSON.parse(ok(JSON.stringify({ model: 'NP756+', serial: SERIAL, users: USERS.length }))))
        case '/getOEM':
          return json(res, 200, JSON.parse(ok('NovinPardaz')))
        default:
          return json(res, 404, JSON.parse(fail('unknown endpoint')))
      }
    })
  })
  httpServer.listen(HTTP_PORT, '0.0.0.0', () => {
    console.log(`[mock-np] VisibleLight HTTP API listening on 0.0.0.0:${HTTP_PORT} (password=${PASSWORD})`)
  })
}

/* ------------------------------------------------------------------ */
/* rtlog-push — شبیه‌ساز پوش بی‌درنگ دستگاه 756PLUS (کشف v1.2.6)      */
/* دستگاهِ واقعی خودش به سرور TCP وصل می‌شود و رکوردهای JSON را در      */
/* پوشت RTLOG001 (magic 8B + طول LE32 4B + payload) می‌فرستد و منتظر     */
/* پاسخ {"log_id":N,"result":"OK"} می‌ماند — همان چیزی که np-sniff.js   */
/* با حالت --listen ضبط و پاسخ می‌دهد.                                  */
/* ------------------------------------------------------------------ */
if (DIALECT === 'rtlog-push') {
  const SERVER = process.env.SERVER || '127.0.0.1'
  const TARGET_PORT = Number(process.env.TARGET_PORT || PORT)
  const PUSH_COUNT = Number(process.env.PUSH_COUNT || 3)
  const PUSH_INTERVAL = Number(process.env.PUSH_INTERVAL || 350)

  const pad = (n) => String(n).padStart(2, '0')
  function punchJson(i) {
    const t = new Date(Date.now() - i * 3600e3)
    const ioTime = `${t.getFullYear()}${pad(t.getMonth() + 1)}${pad(t.getDate())}${pad(t.getHours())}${pad(t.getMinutes())}${pad(t.getSeconds())}`
    return JSON.stringify({
      log_id: String(1000 + i),
      user_id: USERS[i % USERS.length],
      fk_device_id: '1',
      io_mode: '1',
      io_time: ioTime,
      serial_number: SERIAL,
      verify_mode: 'fp',
      device_port: TARGET_PORT,
      is_support_string_id: '0',
    })
  }
  function rtlogFrame(json) {
    const body = Buffer.from(json, 'ascii')
    const f = Buffer.alloc(8 + 4 + body.length + 1)
    f.write('RTLOG001', 0, 'ascii')
    f.writeUInt32LE(body.length + 1, 8)
    body.copy(f, 12)
    return f
  }

  let acks = 0
  let rbuf = Buffer.alloc(0)
  const sock = net.connect(TARGET_PORT, SERVER, async () => {
    console.log(`[mock-np] rtlog-push: وصل شد به ${SERVER}:${TARGET_PORT} — ارسال ${PUSH_COUNT} تردد`)
    for (let i = 0; i < PUSH_COUNT; i++) {
      const j = punchJson(i)
      sock.write(rtlogFrame(j))
      console.log(`[mock-np] » RTLOG001 ${j}`)
      await new Promise((r) => setTimeout(r, PUSH_INTERVAL))
    }
    setTimeout(() => {
      console.log(`[mock-np] rtlog-push پایان — ACK های معتبر: ${acks}/${PUSH_COUNT}`)
      process.exit(0)
    }, 700)
  })
  sock.on('data', (c) => {
    rbuf = Buffer.concat([rbuf, c])
    // پاسخ‌های پوشت‌دار RTLOG
    while (rbuf.length >= 12 && ['RTLOG001', 'RTLOG002', 'RTLOG003'].includes(rbuf.toString('latin1', 0, 8))) {
      const len = rbuf.readUInt32LE(8)
      if (rbuf.length < 12 + len) break
      const payload = rbuf.subarray(12, 12 + len).toString('utf8').replace(/\0.*$/s, '')
      console.log(`[mock-np] « پاسخ: ${payload}`)
      if (/"result"\s*:\s*"OK"/.test(payload)) acks++
      rbuf = rbuf.subarray(12 + len)
    }
    // پاسخ‌های خام JSON بدون پوشت
    if (rbuf.length && !['RTLOG001', 'RTLOG002', 'RTLOG003'].includes(rbuf.toString('latin1', 0, 8))) {
      const raw = rbuf.toString('latin1')
      console.log(`[mock-np] « پاسخ خام: ${raw}`)
      if (/"result"\s*:\s*"OK"/.test(raw)) acks++
      rbuf = Buffer.alloc(0)
    }
  })
  sock.on('error', (e) => { console.error('[mock-np] خطای اتصال push:', e.message); process.exit(1) })
  process.on('exit', () => { if (process.env.STRICT_ACK && acks < PUSH_COUNT) process.exitCode = 2 })
}
