/**
 * SBXPC binary protocol client — Novin Pardaz NP-series devices (NP756+, NP756,
 * NP746, NP757, NP770, NP620, NP290, …)
 *
 * Reverse-engineered from the vendor's own NovinHozor software
 * (SBXPCDLL64.dll + SBPCCOMM64.dll). See scripts/SBXPC-PROTOCOL.md.
 *
 * Real-world devices come in (at least) SIX wire "dialects", so this client
 * PROBES the device on connect and adapts automatically:
 *
 *  CLASSIC        cmd(16B, 55AA) → ACK(8B, 5AA5) → RESULT(14B, AA55) → DATA(A55A)
 *  RESULT_DIRECT  cmd(16B, 55AA) → RESULT(14B, AA55) → DATA(A55A)      [no ACK]
 *  EXT            cmd(12B, 55AA) → ACK(12B, AA55) → DATA …            [SBPCCOMM *_EXT path]
 *  EXT_SECURE     cmd(12B, XOR-encrypted) → ACK(12B, enc) → DATA …     [NP756+ & newer]
 *                 key = LE32(password + salt), frame[j] ^= key[j%4]
 *                 ("رمز شبکه" in the device menu; auto-probed against common defaults)
 *                 The DLL's salt slot is BSS → default salt = 0 (key = LE32(password));
 *                 some SDK generations use 0x25B84 — the client tries both (v1.2.3).
 *  CLASSIC_SECURE classic 16B exchange with every frame XOR-encrypted with the
 *                 same key (v1.2.4 hypothesis — probed when EXT_SECURE fails).
 *  HTTP_JSON      the "VisibleLight" generation (NP756P/NP761P…): a JSON/HTTP API
 *                 on port 14460 — POST /getDeviceKey, /getLogCount, /getLogInfo,
 *                 /getDeviceTime, /getDeviceInfo, /getOEM with {"pass":N} bodies,
 *                 responses {"result":bool,"data":"…"} (exactly what the vendor's
 *                 PacketParserStandard.dll speaks — v1.2.4). The binary port of these
 *                 devices is a legacy stub that refuses classic frames (10-byte ack)
 *                 and drops everything else — the fingerprint seen on real NP756+.
 *
 * Every exchange can be dumped as raw hex in errors/details, so unrecognized
 * device flavours can be diagnosed from the logs without physical access.
 *
 * Frame formats (all little-endian):
 *   PC → DEV COMMAND (classic, 16B): 55 AA | mid(2) | 79 19 | cmd(2) | arg2(4) | arg1(2) | cksum(2)
 *     cksum = (Σ bytes[0..13]) & 0xFFFF
 *   PC → DEV COMMAND (EXT, 12B):     55 AA | mid(2) | cmd(4) | arg(2)=0 | cksum(2)
 *     cksum = (Σ bytes[2..9] + 0xFF) & 0xFFFF
 *   PC → DEV ACK (EXT, 12B):         AA 55 | mid(2) | cmd(4) | arg(2)=0 | cksum(2)
 *   DEV → PC ACK (classic, 8B):      5A A5 | mid(2) | result(2)==1 | cksum(2)
 *     cksum = (mid_lo + mid_hi + r0 + r1 + 0xFF) & 0xFFFF
 *   DEV → PC ACK (EXT, 12B):         AA 55 | mid(2) | dword(4) | cksum(2)
 *     cksum = (Σ bytes[2..9] + 0xFF) & 0xFFFF  [corrected in v1.2.2 from SBPCCOMM64 0x1db0]
 *   EXT_SECURE: the 12B command / 12B ack frames and the data blocks are
 *     XOR-encrypted with the 4-byte key LE32(password + salt); encryption
 *     restarts at byte 0 of EVERY frame. Data blocks (SBPCCOMM _EXT):
 *     [payload][trailer: 5A A5 | mid(2) | Σpayload(4) | ?(2) | cksum(2)]
 *     with trailer cksum = (Σ trailer[2..9] + 0xFF) & 0xFFFF.
 *     salt=0 is the DLL default (the slot is zero-initialized BSS; only
 *     ChangeCompanyMark_EXT ever writes it — nobody in the vendor stack calls
 *     that). 0x25B84 appeared in v1.2.2 due to a misread of file padding; it is
 *     kept as a fallback key variant because some firmware generations use it.
 *   DEV → PC "NP legacy refusal" (10B): AA 55 | mid(2) | dword(4) | cksum_of_refused_cmd(2)
 *     — observed on real NP756+ units: frame received & checksummed correctly,
 *     but the classic protocol was refused (device only speaks encrypted EXT).
 *   DEV → PC RESULT (14B):           AA 55 | mid(2) | ?(2) | status(2) | retCode(4) | cksum(2)
 *     cksum = (Σ bytes[0..11]) & 0xFFFF
 *   DATA both ways:                  A5 5A | mid(2) | payload | cksum(2)
 *     cksum = (Σ bytes[0..4+len)) & 0xFFFF
 *   DEV → PC BIG DATA (chunked):     5A A5 | mid(2) | chunk(≤1020) | cksum(2)
 */
import * as net from 'net'

const COMPANY_MARK = 0x1979
const BIG_CHUNK = 0x3fc // 1020 — max payload per data frame
const DEFAULT_TIMEOUT = 8000

/** SBPCCOMM64.dll — the crypto "company mark" (salt) added to the device "رمز شبکه".
 *  The DLL's slot for it is zero-initialized BSS, so the DEFAULT is 0
 *  (key = LE32(password)); 0x25B84 is kept as a fallback variant. */
const EXT_CRYPTO_MAGIC = 0x25b84

/** salt values tried for EXT_SECURE (in order) — see EXT_CRYPTO_MAGIC note */
const EXT_SALTS: number[] = [0, EXT_CRYPTO_MAGIC]

/** common device defaults for the «رمز شبکه» — tried in order when none is configured */
const PASSWORD_CANDIDATES = [0, 123456, 12345, 1234, 123, 1, 9999, 65535]

/** verify-mode lookup tables extracted from SBXPCDLL64 */
const MODE_TABLE_OLD = [4, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 15, 23, 24, 25, 26]
const MODE_TABLE_NEW = [23, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 4, 5, 6, 7, 22]

export type SbxDialect = 'CLASSIC' | 'RESULT_DIRECT' | 'EXT' | 'EXT_SECURE' | 'CLASSIC_SECURE' | 'HTTP_JSON'

/** port of the "VisibleLight" HTTP JSON API (vendor constant — PacketParserStandard.dll) */
export const NP_HTTP_PORT = 14460

/** XOR key for the EXT_SECURE dialect: LE32(password + salt) */
export function extKeyFor(password: number, salt = 0): Buffer {
  const k = (Math.trunc(password) + salt) >>> 0
  return Buffer.from([k & 0xff, (k >>> 8) & 0xff, (k >>> 16) & 0xff, (k >>> 24) & 0xff])
}

/** XOR a whole buffer with the repeating 4-byte key (returns a copy) */
export function xorCrypt(frame: Buffer, key: Buffer): Buffer {
  const out = Buffer.from(frame)
  for (let i = 0; i < out.length; i++) out[i] ^= key[i % 4]
  return out
}

export interface NpLog {
  /** device user id (enroll number) */
  userId: number
  /** UTC milliseconds of the punch (converted from device Tehran wall-clock) */
  ts: number
  /** decoded verify mode (vendor "IoMode") */
  verifyMode: number
  /** raw record bytes (hex) for auditing */
  raw: string
}

export class SbxpcError extends Error {
  constructor(message: string, public detail?: string) {
    super(message)
    this.name = 'SbxpcError'
  }
}

function hex(buf: Buffer): string {
  return buf.toString('hex').replace(/(..)/g, '$1 ').trim()
}

function sum16(buf: Buffer, from: number, toExclusive: number): number {
  let s = 0
  for (let i = from; i < toExclusive; i++) s += buf[i]
  return s & 0xffff
}

/** Tehran wall-clock (what the device clock shows) → correct UTC instant */
function tehranWallClockToUTC(msSinceEpochLocal: number): Date {
  const asUtc = new Date(msSinceEpochLocal).getTime() // interpret numbers as UTC
  if (!Number.isFinite(asUtc)) return new Date(NaN)
  try {
    const dtf = new Intl.DateTimeFormat('en-US', {
      timeZone: 'Asia/Tehran', hour12: false,
      year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit',
    })
    const parts = dtf.formatToParts(new Date(asUtc))
    const get = (t: string) => Number(parts.find((p) => p.type === t)?.value ?? 0)
    const tehranAsUtc = Date.UTC(get('year'), get('month') - 1, get('day'), get('hour') % 24, get('minute'), get('second'))
    const offset = tehranAsUtc - asUtc
    const ts = new Date(asUtc - offset)
    return Number.isFinite(ts.getTime()) ? ts : new Date(asUtc - 3.5 * 3600 * 1000)
  } catch {
    return new Date(asUtc - 3.5 * 3600 * 1000)
  }
}

/** seconds since 2000-01-01 (device wall clock) → UTC Date */
export function npTimestamp(sec: number): Date {
  return tehranWallClockToUTC(Date.UTC(2000, 0, 1) + sec * 1000)
}

/* ---------------------------------------------------------------- */
/* frame parsers (pure — exported for tests)                        */
/* ---------------------------------------------------------------- */

export interface ClassicAck { result: number; mid: number }
export function parseClassicAck(f: Buffer): ClassicAck | null {
  if (f.length < 8 || f[0] !== 0x5a || f[1] !== 0xa5) return null
  const mid = f.readUInt16LE(2)
  const expected = (f[2] + f[3] + f[4] + f[5] + 0xff) & 0xffff
  if (f.readUInt16LE(6) !== expected) return null
  return { result: f.readUInt16LE(4), mid }
}

export interface ExeResult { status: number; retCode: number; mid: number }
export function parseExeResult(f: Buffer): ExeResult | null {
  if (f.length < 14 || f[0] !== 0xaa || f[1] !== 0x55) return null
  const mid = f.readUInt16LE(2)
  if (f.readUInt16LE(12) !== sum16(f, 0, 12)) return null
  return { status: f.readUInt16LE(6), retCode: f.readUInt32LE(8) >>> 0, mid }
}

export interface ExtAck { dword: number; mid: number }
export function parseExtAck(f: Buffer): ExtAck | null {
  if (f.length < 12 || f[0] !== 0xaa || f[1] !== 0x55) return null
  const mid = f.readUInt16LE(2)
  // checksum covers bytes 2..9 (+0xFF) — SBPCCOMM64.dll 0x1db0 ack builder
  if (f.readUInt16LE(10) !== ((sum16(f, 2, 10) + 0xff) & 0xffff)) return null
  return { dword: f.readUInt32LE(4) >>> 0, mid }
}

/**
 * 10-byte "NP legacy refusal" ack — AA 55 | mid(2) | dword(4) | cksum_of_the_refused_command(2).
 * Real NP756+ units answer a (correctly checksummed) classic 16B frame with this:
 * "frame received, but I only speak the password-encrypted EXT dialect".
 * Distinguished from a plain 12-byte EXT ack by length + own-checksum validity.
 */
export interface NpErrAck { mid: number; dword: number; cmdCksum: number }
export function parseNpErrAck(f: Buffer): NpErrAck | null {
  if (f.length < 10 || f[0] !== 0xaa || f[1] !== 0x55) return null
  if (f.length >= 12 && parseExtAck(f.subarray(0, 12)) !== null) return null
  return { mid: f.readUInt16LE(2), dword: f.readUInt32LE(4) >>> 0, cmdCksum: f.readUInt16LE(8) }
}

/**
 * EXT_SECURE data block: [payload][trailer 12B: 5A A5 | mid(2) | Σpayload(4) | ?(2) | cksum(2)].
 * Returns the payload when both trailer checksums validate. `f` must be DECRYPTED.
 */
export function parseExtTrailer(f: Buffer, payloadLen: number): Buffer | null {
  if (f.length < payloadLen + 12) return null
  const tr = f.subarray(payloadLen, payloadLen + 12)
  if (tr[0] !== 0x5a || tr[1] !== 0xa5) return null
  if (tr.readUInt16LE(10) !== ((sum16(tr, 2, 10) + 0xff) & 0xffff)) return null
  if (tr.readUInt32LE(4) !== sum16(f, 0, payloadLen)) return null
  return f.subarray(0, payloadLen)
}

/** A5 5A data frame of a KNOWN payload length */
export function parseDataFrame(f: Buffer, payloadLen: number): Buffer | null {
  const total = payloadLen + 6
  if (f.length < total || f[0] !== 0xa5 || f[1] !== 0x5a) return null
  if (f.readUInt16LE(4 + payloadLen) !== sum16(f, 0, 4 + payloadLen)) return null
  return f.subarray(4, 4 + payloadLen)
}

/* ---------------------------------------------------------------- */
/* HTTP_JSON dialect — "VisibleLight" NP generation (port 14460)      */
/* (protocol copied from the vendor's PacketParserStandard.dll)      */
/* ---------------------------------------------------------------- */

export interface HttpApiEnvelope {
  result: boolean
  data: string
}

/** POST one JSON request to the device HTTP API; null = no HTTP device / no answer */
export async function npHttpPost(
  ip: string,
  port: number,
  endpoint: string,
  body: Record<string, unknown>,
  timeoutMs = 2600,
): Promise<HttpApiEnvelope | null> {
  const ctl = new AbortController()
  const timer = setTimeout(() => ctl.abort(), timeoutMs)
  try {
    const res = await fetch(`http://${ip}:${port}${endpoint}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
      signal: ctl.signal,
    })
    const text = (await res.text()).slice(0, 8192)
    try {
      const j = JSON.parse(text) as { result?: unknown; data?: unknown }
      if (typeof j.result !== 'boolean') return null
      return { result: j.result, data: typeof j.data === 'string' ? j.data : JSON.stringify(j.data ?? '') }
    } catch {
      return null
    }
  } catch {
    return null
  } finally {
    clearTimeout(timer)
  }
}

/** parse the /getLogCount "data" payload defensively ({"count":N} | {"logCount":N} | plain N) */
export function parseHttpLogCount(data: string): number | null {
  const s = data.trim()
  try {
    const j = JSON.parse(s)
    if (typeof j === 'number') return Math.trunc(j)
    if (j && typeof j === 'object') {
      for (const k of ['count', 'logCount', 'log_count', 'total', 'logs']) {
        const v = (j as Record<string, unknown>)[k]
        if (typeof v === 'number') return Math.trunc(v)
        if (typeof v === 'string' && /^\d+$/.test(v.trim())) return Number(v.trim())
      }
    }
  } catch { /* not JSON */ }
  if (/^\d+$/.test(s)) return Number(s)
  return null
}

/** parse a device clock string defensively (ISO | "yyyy-MM-dd HH:mm:ss" | epoch) */
export function parseHttpDate(data: string): Date | null {
  const s = data.trim()
  if (/^\d{10}$/.test(s)) return new Date(Number(s) * 1000) // epoch seconds
  if (/^\d{13}$/.test(s)) return new Date(Number(s))
  const d = new Date(s.replace(' ', 'T'))
  return Number.isFinite(d.getTime()) ? d : null
}

/* ---------------------------------------------------------------- */
/* client                                                            */
/* ---------------------------------------------------------------- */

export interface ProbeInfo {
  dialect: SbxDialect | null
  serialNumber?: string
  deviceTime?: Date | null
  /** raw bytes of every exchange, for diagnostics */
  exchanges: Array<{ dir: 'out' | 'in'; hex: string }>
  note?: string
  /** EXT_SECURE: the «رمز شبکه» that the device accepted */
  password?: number
  /** EXT_SECURE: the salt variant the device accepted (0 = DLL default) */
  salt?: number
  /** HTTP_JSON: number of stored logs (from /getLogCount) */
  logCount?: number
}

// (shared mutable client state lives on the class; nothing else here)

export class SbxpcClient {
  private socket: net.Socket | null = null
  private buf: Buffer = Buffer.alloc(0)
  private closed = false
  /** detected on probe; null until then */
  dialect: SbxDialect | null = null
  probeInfo: ProbeInfo | null = null

  constructor(
    private readonly ip: string,
    private readonly port: number,
    private machineId = 1,
    /** «رمز شبکه» of the device — required by (and auto-probed for) the EXT_SECURE dialect */
    password?: number | null,
  ) {
    this.password = password ?? null
    this.hadConfiguredPassword = password != null
  }

  /** EXT_SECURE network password («رمز شبکه»); null = unknown/not set */
  password: number | null = null
  /** EXT_SECURE salt variant (0 = DLL default; EXT_CRYPTO_MAGIC = legacy guess) */
  private extSalt = EXT_SALTS[0]
  /** true when a password was explicitly configured (not auto-probed) */
  private readonly hadConfiguredPassword: boolean = false
  /** EXT_SECURE probe: bytes that followed the accepted ack (may hold the serial block) */
  private extProbeTail: Buffer = Buffer.alloc(0)
  /** v1.2.5: the device demands the classic 0x52 login before any command */
  private needsLogin = false
  /** v1.2.5: this connection is logged in (login state lives per TCP session) */
  private loggedIn = false

  /* ---------------------------------------------------------------- */
  /* low-level framing                                                 */
  /* ---------------------------------------------------------------- */

  private cmdHeader(cmd: number, arg1: number, arg2: number): Buffer {
    const h = Buffer.alloc(16)
    h.writeUInt16LE(0xaa55, 0) // bytes: 55 AA
    h.writeUInt16LE(this.machineId, 2)
    h.writeUInt16LE(COMPANY_MARK, 4) // bytes: 79 19
    h.writeUInt16LE(cmd, 6)
    h.writeUInt32LE(arg2 >>> 0, 8)
    h.writeUInt16LE(arg1 & 0xffff, 12)
    h.writeUInt16LE(sum16(h, 0, 14), 14)
    return h
  }

  /** EXT 12-byte command frame: 55 AA | mid(2) | cmd(4) | arg(2)=0 | cksum(2) */
  private cmdFrameExt(cmd: number, arg = 0): Buffer {
    const f = Buffer.alloc(12)
    f.writeUInt16LE(0xaa55, 0) // bytes: 55 AA
    f.writeUInt16LE(this.machineId, 2)
    f.writeUInt32LE(cmd >>> 0, 4)
    f.writeUInt16LE(arg & 0xffff, 8)
    f.writeUInt16LE((sum16(f, 2, 10) + 0xff) & 0xffff, 10)
    return f
  }

  /** EXT 12-byte ack frame (PC → device): AA 55 | mid(2) | cmd(4) | arg(2)=0 | cksum(2) */
  private ackFrameExt(cmd: number, arg = 0): Buffer {
    const f = Buffer.alloc(12)
    f.writeUInt16LE(0x55aa, 0) // bytes: AA 55
    f.writeUInt16LE(this.machineId, 2)
    f.writeUInt32LE(cmd >>> 0, 4)
    f.writeUInt16LE(arg & 0xffff, 8)
    f.writeUInt16LE((sum16(f, 2, 10) + 0xff) & 0xffff, 10)
    return f
  }

  /* ---- EXT_SECURE (password-encrypted NP dialect) ------------------ */

  private key(): Buffer {
    return extKeyFor(this.password ?? 0, this.extSalt)
  }

  /** encrypted EXT command — the whole 12B frame XOR key(password + 0x25B84) */
  private cmdFrameExtEnc(cmd: number, arg = 0): Buffer {
    return xorCrypt(this.cmdFrameExt(cmd, arg), this.key())
  }

  /** encrypted EXT ack (PC → device) */
  private ackFrameExtEnc(cmd: number, arg = 0): Buffer {
    return xorCrypt(this.ackFrameExt(cmd, arg), this.key())
  }

  /** encrypted data block: payload + 12B trailer, both XOR-encrypted */
  private dataBlockExtEnc(payload: Buffer): Buffer {
    const tr = Buffer.alloc(12)
    tr.writeUInt16LE(0xa55a, 0) // bytes: 5A A5
    tr.writeUInt16LE(this.machineId, 2)
    tr.writeUInt32LE(sum16(payload, 0, payload.length), 4)
    // tr[8..9] stay 0 (the vendor SDK leaves them uninitialized)
    tr.writeUInt16LE((sum16(tr, 2, 10) + 0xff) & 0xffff, 10)
    return Buffer.concat([xorCrypt(payload, this.key()), xorCrypt(tr, this.key())])
  }

  private dataFrame(payload: Buffer): Buffer {
    const f = Buffer.alloc(payload.length + 6)
    f.writeUInt16LE(0x5aa5, 0) // bytes: A5 5A
    f.writeUInt16LE(this.machineId, 2)
    payload.copy(f, 4)
    f.writeUInt16LE(sum16(f, 0, 4 + payload.length), 4 + payload.length)
    return f
  }

  private write(buf: Buffer) {
    if (!this.socket || this.closed) throw new SbxpcError('اتصال به دستگاه برقرار نیست')
    this.socket.write(buf)
  }

  private record(dir: 'out' | 'in', buf: Buffer) {
    if (this.probeInfo) this.probeInfo.exchanges.push({ dir, hex: hex(buf.subarray(0, 128)) })
  }

  /** read exactly n bytes (with timeout) */
  private readN(n: number, timeoutMs: number): Promise<Buffer> {
    return new Promise((resolve, reject) => {
      const timer = setTimeout(() => {
        cleanup()
        reject(new SbxpcError(`دستگاه پاسخ نداد (سقف انتظار ${Math.round(timeoutMs / 1000)} ثانیه)`, `buffer=${hex(this.buf)}`))
      }, timeoutMs)
      const cleanup = () => {
        clearTimeout(timer)
        this.socket?.off('data', onData)
        this.socket?.off('close', onClose)
        this.socket?.off('error', onClose)
      }
      const onData = (chunk: Buffer) => {
        this.buf = Buffer.concat([this.buf, chunk])
        if (this.buf.length >= n) {
          const out = this.buf.subarray(0, n)
          this.buf = this.buf.subarray(n)
          cleanup()
          resolve(out)
        }
      }
      const onClose = () => {
        cleanup()
        reject(new SbxpcError('اتصال دستگاه در میانهٔ عملیات قطع شد'))
      }
      if (!this.socket || this.closed) {
        cleanup()
        reject(new SbxpcError('اتصال به دستگاه برقرار نیست'))
        return
      }
      this.socket.on('data', onData)
      this.socket.on('close', onClose)
      this.socket.on('error', onClose)
      // maybe already buffered
      onData(Buffer.alloc(0))
    })
  }

  /**
   * Drain everything the device has to say: first chunk within `firstTimeoutMs`,
   * then keep reading while bytes keep arriving within `idleMs` of each other.
   */
  private readAvailable(firstTimeoutMs: number, idleMs = 400, maxBytes = 64 * 1024): Promise<Buffer> {
    return new Promise((resolve, reject) => {
      const chunks: Buffer[] = []
      let total = 0
      let settled = false
      let idleTimer: NodeJS.Timeout | null = null
      const firstTimer = setTimeout(() => finish(), firstTimeoutMs)

      const cleanup = () => {
        clearTimeout(firstTimer)
        if (idleTimer) clearTimeout(idleTimer)
        this.socket?.off('data', onData)
        this.socket?.off('close', onFail)
        this.socket?.off('error', onFail)
      }
      const finish = () => {
        if (settled) return
        settled = true
        cleanup()
        resolve(Buffer.concat(chunks))
      }
      const onFail = () => {
        if (settled) return
        if (total > 0) { finish(); return }
        settled = true
        cleanup()
        reject(new SbxpcError('اتصال دستگاه در میانهٔ عملیات قطع شد'))
      }
      const armIdle = () => {
        if (idleTimer) clearTimeout(idleTimer)
        idleTimer = setTimeout(finish, idleMs)
      }
      const onData = (chunk: Buffer) => {
        if (settled || chunk.length === 0) return
        armIdle()
        chunks.push(chunk)
        total += chunk.length
        if (total >= maxBytes) finish()
      }

      if (!this.socket || this.closed) {
        clearTimeout(firstTimer)
        reject(new SbxpcError('اتصال به دستگاه برقرار نیست'))
        return
      }
      this.socket.on('data', onData)
      this.socket.on('close', onFail)
      this.socket.on('error', onFail)

      if (this.buf.length > 0) {
        const pre = this.buf
        this.buf = Buffer.alloc(0)
        onData(pre)
      }
    })
  }

  async connect(timeoutMs = DEFAULT_TIMEOUT): Promise<void> {
    await new Promise<void>((resolve, reject) => {
      const s = new net.Socket()
      s.setTimeout(timeoutMs)
      const onFail = (err: unknown) => {
        s.destroy()
        reject(err instanceof Error ? err : new Error(String(err)))
      }
      s.once('error', onFail)
      s.once('timeout', () => onFail(new SbxpcError('مهلت اتصال به دستگاه به پایان رسید — دستگاه خاموش است یا IP/پورت اشتباه است')))
      s.connect(this.port, this.ip, () => {
        s.setTimeout(0)
        s.off('error', onFail)
        this.socket = s
        this.closed = false
        this.buf = Buffer.alloc(0)
        resolve()
      })
    })
    // socket error handling from now on: mark closed
    this.socket!.on('error', () => { this.closed = true })
    this.socket!.on('close', () => { this.closed = true })
  }

  /** fresh TCP connection — device-side parser state resets on disconnect */
  async resetConnection(timeoutMs = 6000): Promise<void> {
    this.disconnect()
    this.loggedIn = false // v1.2.5: the login session lives on the socket
    await new Promise((r) => setTimeout(r, 250))
    await this.connect(timeoutMs)
    if (this.needsLogin && this.password != null) {
      await this.ensureLogin()
    }
  }

  isConnected(): boolean {
    return !!this.socket && !this.closed
  }

  disconnect() {
    try { this.socket?.end() } catch { /* ignore */ }
    try { this.socket?.destroy() } catch { /* ignore */ }
    this.socket = null
    this.closed = true
  }

  /* ---------------------------------------------------------------- */
  /* dialect probe                                                     */
  /* ---------------------------------------------------------------- */

  /**
   * Probe the device with GetSerialNumber (0x113) in multiple dialects and
   * record which one it speaks. Safe to call once right after connect.
   */
  async probeDialect(): Promise<ProbeInfo> {
    if (this.probeInfo) return this.probeInfo
    const info: ProbeInfo = { dialect: null, exchanges: [] }
    this.probeInfo = info

    /* ---- attempt 1: CLASSIC 16-byte command ---- */
    const classicCmd = this.cmdHeader(0x113, 0, 0)
    this.write(classicCmd)
    this.record('out', classicCmd)
    const r1 = await this.readAvailable(6000)
    this.record('in', r1)
    if (r1.length === 0) {
      // a newer NP unit may ignore plaintext frames entirely — try encrypted EXT once
      const silent = await this.tryExtSecure()
      if (silent != null) return this.finishExtSecure(silent, info)
      throw new SbxpcError(
        'دستگاه هیچ پاسخی نداد — IP/پورت را بررسی کنید و مطمئن شوید پورتِ «ارتباط» دستگاه (نه پورت سرور) وارد شده است',
        'probe: no bytes',
      )
    }

    // CLASSIC: 5A A5 ack + result + data, possibly all in one drain
    const ack = parseClassicAck(r1.subarray(0, 8))
    if (ack) {
      this.dialect = 'CLASSIC'
      info.dialect = 'CLASSIC'
      if (ack.mid !== this.machineId && ack.mid !== 0xffff) this.machineId = ack.mid
      const serial = this.trySerialFromTail(r1.subarray(8))
      if (serial) info.serialNumber = serial
      info.deviceTime = await this.safeGetDeviceTime()
      return info
    }

    /** NP756+ refusal of the classic protocol → the device demands a LOGIN first
     *  (v1.2.5 discovery: the vendor's StartTcpX sends classic cmd 0x52 with the
     *  «رمز شبکه» in arg2 before anything else — see SBXPC-PROTOCOL.md) */
    let sawNpErr: NpErrAck | null = null

    if (r1[0] === 0xaa && r1[1] === 0x55) {
      /* ---- attempt 2a: RESULT_DIRECT (result 14B + 38B data) ---- */
      const res = r1.length >= 14 ? parseExeResult(r1.subarray(0, 14)) : null
      if (res) {
        let tail = r1.subarray(14)
        if (tail.length === 0) {
          // data may follow after a beat
          const more = await this.readAvailable(2500)
          if (more.length > 0) this.record('in', more)
          tail = Buffer.concat([tail, more])
        }
        const serial = this.trySerialFromTail(tail)
        if (serial) {
          this.dialect = 'RESULT_DIRECT'
          info.dialect = 'RESULT_DIRECT'
          info.serialNumber = serial
          info.deviceTime = await this.safeGetDeviceTime()
          return info
        }
      }

      sawNpErr = parseNpErrAck(r1)
    }

    /* ---- attempt 2b (v1.2.5): the LOGIN handshake — classic cmd 0x52 with the
       «رمز شبکه» in arg2, plaintext, exactly like the vendor's StartTcpX.
       Until you log in, the device refuses every command with the 10-byte
       code-0x10 ack and drops everything else. */
    if (sawNpErr) {
      const found = await this.tryClassicLogin()
      if (found != null) {
        // logged in — the classic protocol should answer now (same connection!)
        const classicCmd2 = this.cmdHeader(0x113, 0, 0)
        try {
          this.write(classicCmd2)
          this.record('out', classicCmd2)
        } catch { /* fall through to the other probes */ }
        const r2 = await this.readAvailable(6000).catch(() => Buffer.alloc(0))
        this.record('in', r2)
        const done = this.finishClassicProbe(r2, info)
        if (done) {
          info.password = found
          info.deviceTime = await this.safeGetDeviceTime()
          info.note = `ورود با «رمز شبکه» انجام شد (فرمان 0x52) — رمز: ${found}`
          return info
        }
      }

      /* ---- attempt 2c (v1.2.2-): password-encrypted EXT dialect ----
         some NP generations refuse classic frames and only accept frames
         XOR-encrypted with key(«رمز شبکه» + salt). */
      const foundExt = await this.tryExtSecure()
      if (foundExt != null) return this.finishExtSecure(foundExt, info)

      /* ---- v1.2.4: the "VisibleLight" HTTP JSON API (NP756P/NP761P generation) —
         the vendor's own stack talks to these face devices over HTTP :14460,
         and their binary port is exactly such a refusing stub ---- */
      const http = await this.tryHttpApi(info)
      if (http) return http

      /* ---- v1.2.4: CLASSIC_SECURE hypothesis — the whole classic exchange
         XOR-encrypted with the «رمز شبکه» key ---- */
      const classicSec = await this.tryClassicSecure(info)
      if (classicSec) return classicSec

      info.note = this.hadConfiguredPassword
        ? 'دستگاه NP با رمزنگاری شبکه شناسایی شد ولی «رمز شبکه» ثبت‌شده پذیرفته نشد — رمز را از منوی دستگاه (تنظیمات ← ارتباط ← رمز شبکه) بررسی کنید'
        : 'دستگاه NP با رمزنگاری شبکه شناسایی شد ولی هیچ‌یک از رمزهای رایج پذیرفته نشد — «رمز شبکه» دستگاه را از منوی دستگاه (تنظیمات ← ارتباط) پیدا و در پرونده دستگاه وارد کنید'
      info.note += ' — اگر رمز درست است، خروجی ابزار scripts/np-probe.js را برای پشتیبانی بفرستید'
      return info
    }

    /* ---- attempt 3: plain EXT (12-byte frames) — reconnect for a clean
       device-side parser state (our classic frame may have half-consumed) ---- */
    if (!sawNpErr) {
      try { await this.resetConnection() } catch { /* keep old socket if reconnect fails */ }
      const extCmd = this.cmdFrameExt(0x113)
      this.write(extCmd)
      this.record('out', extCmd)
      const r2 = await this.readAvailable(6000)
      this.record('in', r2)
      if (r2.length >= 12) {
        const ack2 = parseExtAck(r2.subarray(0, 12))
        if (ack2) {
          this.dialect = 'EXT'
          info.dialect = 'EXT'
          if (ack2.mid !== this.machineId && ack2.mid !== 0xffff) this.machineId = ack2.mid
          let rest = r2.subarray(12)
          if (rest.length === 0) {
            const more = await this.readAvailable(2500)
            if (more.length > 0) this.record('in', more)
            rest = Buffer.concat([rest, more])
          }
          const serial = this.trySerialFromTail(rest)
          if (serial) {
            try { this.write(this.ackFrameExt(0x113)) } catch { /* ignore */ }
            info.serialNumber = serial
          } else {
            info.note =
              ack2.dword === 0x10
                ? 'دستگاه گویش ۱۲بایتی (EXT) پروتکل را صحبت می‌کند ولی فرمان «خواندن سریال» را نپذیرفت (کد 16) — احتمالاً این فرمان روی این مدل پشتیبانی نمی‌شود'
                : `دستگاه با فریم ۱۲بایتی (EXT) پاسخ داد (کد ${ack2.dword})`
          }
          info.deviceTime = await this.safeGetDeviceTime()
          return info
        }
      }
    }

    /* ---- attempt 4: final fallback — encrypted EXT with common passwords ---- */
    {
      const found = await this.tryExtSecure()
      if (found != null) return this.finishExtSecure(found, info)
    }

    /* ---- attempt 5: try GetDeviceTime (0x10e) classic — harmless ---- */
    {
      try { await this.resetConnection() } catch { /* ignore */ }
      const tcmd = this.cmdHeader(0x10e, 0, 4)
      this.write(tcmd)
      this.record('out', tcmd)
      const r3 = await this.readAvailable(6000)
      this.record('in', r3)
      const res3 = r3.length >= 14 ? parseExeResult(r3.subarray(0, 14)) : null
      if (res3) {
        this.dialect = 'RESULT_DIRECT'
        info.dialect = 'RESULT_DIRECT'
        const data = r3.length >= 14 + 10 ? parseDataFrame(r3.subarray(14), 4) : null
        if (data) info.deviceTime = npTimestamp(data.readUInt32LE(0))
        info.note = 'دستگاه پاسخ می‌دهد ولی فرمان «خواندن سریال» را برنگرداند'
        return info
      }
    }

    /* ---- nothing recognized — but the device IS alive ---- */
    /* v1.2.4: last resort — maybe it is an HTTP_JSON device after all */
    const http2 = await this.tryHttpApi(info)
    if (http2) return http2
    info.note = 'دستگاه پاسخ داد ولی فریم‌های آن با هیچ‌یک از گویش‌های شناخته‌شدهٔ پروتکل مطابقت نداشت'
    return info
  }

  /* ---------------------------------------------------------------- */
  /* v1.2.5 — the classic LOGIN handshake (cmd 0x52, «رمز شبکه» in arg2)  */
  /* Reverse-engineered from SBPCCOMM64.dll vtable[2]: StartTcpX calls    */
  /* devObj->login(password) right after the TCP connect — classic frame, */
  /* PLAINTEXT, password in arg2 → 14B RESULT reply (status≠0 = OK).       */
  /* ---------------------------------------------------------------- */

  /**
   * Send the login frame and parse the RESULT. Returns true when logged in.
   * The reply may be [optional 8B ACK][14B RESULT] — status (word @+6) ≠ 0 means OK.
   * The 10-byte code-0x10 refusal (exactly 10 bytes) means the login was rejected.
   */
  private async classicLogin(pw: number, timeoutMs = 5000): Promise<boolean> {
    const f = this.cmdHeader(0x52, 0, pw)
    try {
      this.write(f)
      this.record('out', f)
    } catch { return false }
    const r = await this.readAvailable(timeoutMs).catch(() => Buffer.alloc(0))
    this.record('in', r)
    if (r.length === 0) return false // silence → wrong/absent login
    // parse [optional 8B classic ACK] + 14B RESULT first (AA 55 frames can be both!)
    let off = 0
    if (r[0] === 0x5a && r[1] === 0xa5 && r.length >= 8 + 14) off = 8 // skip classic ACK
    const res = r.length >= off + 14 ? parseExeResult(r.subarray(off, off + 14)) : null
    if (res) return res.status !== 0
    // an exact 10-byte frame is the NP refusal — login rejected
    if (r.length === 10 && parseNpErrAck(r)) return false
    return false
  }

  /**
   * Try the login against the configured password (or common defaults),
   * each on a FRESH connection. On success the connection stays logged in.
   * Returns the accepted password, or null.
   */
  private async tryClassicLogin(): Promise<number | null> {
    const configuredPw = this.password
    const passwords = this.password != null ? [this.password] : PASSWORD_CANDIDATES
    for (const pw of passwords) {
      this.password = pw
      try {
        await this.resetConnection(4000)
      } catch {
        this.password = configuredPw
        return null // device unreachable
      }
      if (await this.classicLogin(pw)) {
        this.needsLogin = true
        this.loggedIn = true
        return pw
      }
    }
    this.password = configuredPw
    return null
  }

  /** re-login after a reconnect if this device demands it (v1.2.5) */
  private async ensureLogin(): Promise<void> {
    if (!this.needsLogin || this.loggedIn || this.password == null) return
    if (await this.classicLogin(this.password, 4000)) this.loggedIn = true
  }

  /**
   * Recognize a classic 0x113 probe reply (used by the post-login retry):
   * CLASSIC (5A A5 ack + serial) or RESULT_DIRECT (AA 55 result + serial).
   * Fills `info` and returns true when recognized.
   */
  private finishClassicProbe(r: Buffer, info: ProbeInfo): boolean {
    if (r.length === 0) return false
    const ack = parseClassicAck(r.subarray(0, 8))
    if (ack) {
      this.dialect = 'CLASSIC'
      info.dialect = 'CLASSIC'
      if (ack.mid !== this.machineId && ack.mid !== 0xffff) this.machineId = ack.mid
      const serial = this.trySerialFromTail(r.subarray(8))
      if (serial) info.serialNumber = serial
      return true
    }
    if (r[0] === 0xaa && r[1] === 0x55) {
      const res = r.length >= 14 ? parseExeResult(r.subarray(0, 14)) : null
      if (res) {
        const serial = this.trySerialFromTail(r.subarray(14))
        if (serial) {
          this.dialect = 'RESULT_DIRECT'
          info.dialect = 'RESULT_DIRECT'
          info.serialNumber = serial
          return true
        }
      }
    }
    return false
  }

  /** finalize an EXT_SECURE probe success */
  private async finishExtSecure(pw: number, info: ProbeInfo): Promise<ProbeInfo> {
    this.dialect = 'EXT_SECURE'
    info.dialect = 'EXT_SECURE'
    info.password = pw
    info.salt = this.extSalt
    // the probe drain may already carry the serial block
    const serial = this.trySerialFromTailEnc(this.extProbeTail)
    if (serial) {
      info.serialNumber = serial
      try { this.write(this.ackFrameExtEnc(0x113)) } catch { /* ignore */ }
    }
    // swallow whatever the probe exchange left unread, then read the clock
    await this.readAvailable(500).catch(() => Buffer.alloc(0))
    info.deviceTime = await this.safeGetDeviceTime()
    info.note = `گویش رمزشدهٔ NP — «رمز شبکه»=${pw}${this.extSalt ? ` · نمک=${this.extSalt}` : ''}`
    return info
  }

  /**
   * Try the password-encrypted EXT dialect against the configured password
   * (or a list of common device defaults) × the known salt variants.
   * Each attempt runs on a FRESH connection — the device-side parser state
   * resets on disconnect. Returns the accepted password, or null.
   */
  private async tryExtSecure(): Promise<number | null> {
    const configuredPw = this.password
    const passwords = this.password != null ? [this.password] : PASSWORD_CANDIDATES
    for (const salt of EXT_SALTS) {
      for (const pw of passwords) {
        this.password = pw
        this.extSalt = salt
        try {
          await this.resetConnection(4000)
        } catch {
          // device unreachable — abort the whole search
          this.password = configuredPw
          this.extSalt = EXT_SALTS[0]
          return null
        }
        const cmd = this.cmdFrameExtEnc(0x113)
        try {
          this.write(cmd)
          this.record('out', cmd)
        } catch { continue }
        const r = await this.readAvailable(2200).catch(() => Buffer.alloc(0))
        this.record('in', r)
        if (r.length < 12) continue // silence → wrong key
        const dec = xorCrypt(r.subarray(0, 12), extKeyFor(pw, salt))
        const ack = parseExtAck(dec)
        if (!ack) continue
        if (ack.mid !== this.machineId && ack.mid !== 0xffff) this.machineId = ack.mid
        this.extProbeTail = r.subarray(12)
        return pw
      }
    }
    // sweep failed — restore the configured password so later probes use it
    this.password = configuredPw
    this.extSalt = EXT_SALTS[0]
    return null
  }

  /* ---------------------------------------------------------------- */
  /* v1.2.4 — CLASSIC_SECURE (hypothesis): the whole classic exchange    */
  /* XOR-encrypted with the «رمز شبکه» key                              */
  /* ---------------------------------------------------------------- */

  /** send one encrypted classic command; return the DECRYPTED reply drain (or null) */
  private async classicSecureExchange(
    cmd: number, arg1: number, arg2: number, pw: number, salt: number, timeoutMs: number,
    dataLen = 32,
  ): Promise<Buffer | null> {
    const key = extKeyFor(pw, salt)
    const frame = xorCrypt(this.cmdHeader(cmd, arg1, arg2), key)
    try {
      this.write(frame)
      this.record('out', frame)
    } catch { return null }
    const r = await this.readAvailable(timeoutMs).catch(() => Buffer.alloc(0))
    this.record('in', r)
    if (r.length === 0) return null
    return this.decryptClassicReply(r, key, dataLen)
  }

  /**
   * Turn a (possibly encrypted) classic reply drain into plaintext frames.
   * The device restarts the XOR phase at byte 0 of EVERY frame, so the frames
   * are decrypted one at a time: ACK(8B 5AA5) → RESULT(14B AA55) → DATA(6+len).
   * `dataLen` is the expected payload of the data frame (0x113 → 32, 0x10e → 4);
   * the result's retCode is NOT trusted for framing (devices disagree on it).
   * Also accepts a fully plaintext reply.
   */
  private decryptClassicReply(drain: Buffer, key: Buffer, dataLen = 32): Buffer | null {
    // plaintext reply?
    if (drain[0] === 0x5a && drain[1] === 0xa5) return drain
    if (drain[0] === 0xaa && drain[1] === 0x55 && drain.length >= 14 && parseExeResult(drain.subarray(0, 14))) return drain

    const parts: Buffer[] = []
    let off = 0
    for (let guard = 0; guard < 8 && off + 2 <= drain.length; guard++) {
      const h0 = drain[off] ^ key[0]
      const h1 = drain[off + 1] ^ key[1]
      let flen = 0
      if (h0 === 0x5a && h1 === 0xa5) flen = 8
      else if (h0 === 0xaa && h1 === 0x55) flen = 14
      else if (h0 === 0xa5 && h1 === 0x5a) flen = dataLen + 6
      else return null
      if (off + flen > drain.length) return parts.length ? Buffer.concat(parts) : null
      parts.push(xorCrypt(drain.subarray(off, off + flen), key))
      off += flen
    }
    const plain = Buffer.concat(parts)
    return plain.length > 0 ? plain : null
  }

  /** probe CLASSIC_SECURE: encrypted classic 0x113 → valid ACK/RESULT ⇒ dialect confirmed */
  private async tryClassicSecure(info: ProbeInfo): Promise<ProbeInfo | null> {
    // last-resort hypothesis — a short candidate list keeps the probe snappy
    const configuredPw = this.password
    const passwords = this.password != null ? [this.password] : [12345, 123456, 0]
    for (const salt of EXT_SALTS) {
      for (const pw of passwords) {
        this.password = pw
        this.extSalt = salt
        try {
          await this.resetConnection(4000)
        } catch {
          this.password = configuredPw
          this.extSalt = EXT_SALTS[0]
          return null
        }
        const plain = await this.classicSecureExchange(0x113, 0, 0, pw, salt, 2600, 32)
        if (!plain) continue
        const ack = plain.length >= 8 ? parseClassicAck(plain.subarray(0, 8)) : null
        const res = plain.length >= 14 ? parseExeResult(plain.subarray(8, 22)) : null
        if (!ack && !res) continue
        this.dialect = 'CLASSIC_SECURE'
        info.dialect = 'CLASSIC_SECURE'
        info.password = pw
        info.salt = salt
        if (ack && ack.mid !== this.machineId && ack.mid !== 0xffff) this.machineId = ack.mid
        const serial = this.trySerialFromTail(plain.subarray(ack ? 8 : 14))
        if (serial) info.serialNumber = serial
        // read the clock through the same encrypted classic path
        const t = await this.classicSecureExchange(0x10e, 0, 4, pw, salt, 2600, 4)
        if (t) {
          // the drain starts with the optional 8B ACK — the RESULT frame follows it
          const ackT = parseClassicAck(t.subarray(0, 8))
          const off = ackT ? 8 : 0
          const tres = t.length >= off + 14 ? parseExeResult(t.subarray(off, off + 14)) : null
          const tdata = tres && t.length >= off + 14 + 10 ? parseDataFrame(t.subarray(off + 14, off + 24), 4) : null
          if (tdata) info.deviceTime = npTimestamp(tdata.readUInt32LE(0))
        }
        info.note = `گویش کلاسیکِ رمزشده — «رمز شبکه»=${pw}${salt ? ` · نمک=${salt}` : ''}`
        return info
      }
    }
    this.password = configuredPw
    this.extSalt = EXT_SALTS[0]
    return null
  }

  /* ---------------------------------------------------------------- */
  /* v1.2.4 — HTTP_JSON dialect ("VisibleLight" generation, :14460)      */
  /* ---------------------------------------------------------------- */

  /**
   * Probe the device HTTP JSON API (port 14460 — vendor constant).
   * On success fills dialect/serial/time/logCount into `info` and returns it.
   */
  async tryHttpApi(info: ProbeInfo): Promise<ProbeInfo | null> {
    const pw = this.password ?? 0
    // some firmwares want the pass as a string, some as a number — try both
    for (const pass of [pw, String(pw)]) {
      const key = await npHttpPost(this.ip, NP_HTTP_PORT, '/getDeviceKey', { pass })
      if (!key || !key.result) continue
      this.dialect = 'HTTP_JSON'
      info.dialect = 'HTTP_JSON'
      info.password = pw
      const serial = key.data.trim()
      if (serial) info.serialNumber = serial
      const time = await npHttpPost(this.ip, NP_HTTP_PORT, '/getDeviceTime', { pass })
      if (time?.result) info.deviceTime = parseHttpDate(time.data)
      const cnt = await npHttpPost(this.ip, NP_HTTP_PORT, '/getLogCount', { pass })
      if (cnt?.result) info.logCount = parseHttpLogCount(cnt.data) ?? undefined
      info.note =
        `دستگاه با API HTTP (پورت ${NP_HTTP_PORT}) پاسخ داد — نسل جدید NP («VisibleLight»). ` +
        `تست اتصال موفق؛ خواندن ترددها روی همین پروتکل در نسخهٔ بعدی فعال می‌شود — ` +
        `برای تسریع، خروجی scripts/np-probe.js را برای پشتیبانی بفرستید`
      return info
    }
    return null
  }

  /** try to read a 32-byte serial out of [result(14B)] + [data frame(38B)] */
  private trySerialFromTail(tail: Buffer): string | null {
    if (tail.length === 0) return null
    let off = 0
    if (tail.length >= 14 && parseExeResult(tail.subarray(0, 14))) off = 14
    const need = off + 38
    if (tail.length < need) return null
    const payload = parseDataFrame(tail.subarray(off), 32)
    if (!payload) return null
    const s = payload.toString('ascii').replace(/[\0\r\n\t ]+$/g, '').trim()
    return s || null
  }

  /**
   * EXT_SECURE serial tail — the device answers the encrypted ACK followed by
   * the serial data as an encrypted block. Frame formats seen across firmware
   * generations are all accepted: classic A5 5A data frame (plain or encrypted)
   * and the SBPCCOMM _EXT [payload][trailer] block (encrypted).
   */
  private trySerialFromTailEnc(tail: Buffer): string | null {
    if (tail.length === 0 || this.password == null) return null
    const key = extKeyFor(this.password, this.extSalt)
    let off = 0
    if (tail.length >= 14 && parseExeResult(tail.subarray(0, 14))) off = 14
    const t = tail.subarray(off)
    const clean = (b: Buffer | null) => (b ? b.toString('ascii').replace(/[\0\r\n\t ]+$/g, '').trim() || null : null)
    // variant 1: encrypted classic A5 5A data frame (38B)
    if (t.length >= 38) {
      const p = parseDataFrame(xorCrypt(t.subarray(0, 38), key), 32)
      const s = clean(p)
      if (s) return s
    }
    // variant 2: plaintext classic A5 5A data frame
    if (t.length >= 38) {
      const s = clean(parseDataFrame(t.subarray(0, 38), 32))
      if (s) return s
    }
    // variant 3: encrypted [payload(32)][trailer(12)] block
    if (t.length >= 44) {
      const s = clean(parseExtTrailer(xorCrypt(t.subarray(0, 44), key), 32))
      if (s) return s
    }
    return null
  }

  private async safeGetDeviceTime(): Promise<Date | null> {
    try { return await this.getDeviceTime() } catch { return null }
  }

  /* ---------------------------------------------------------------- */
  /* protocol operations                                               */
  /* ---------------------------------------------------------------- */

  /** send a command in the active dialect and drain the immediate response */
  private async sendCmd(cmd: number, arg1: number, arg2: number, timeoutMs = DEFAULT_TIMEOUT): Promise<Buffer> {
    if (this.dialect === 'EXT_SECURE') {
      const f = this.cmdFrameExtEnc(cmd, arg1)
      this.write(f)
    } else if (this.dialect === 'EXT') {
      const f = this.cmdFrameExt(cmd)
      this.write(f)
    } else {
      const f = this.cmdHeader(cmd, arg1, arg2)
      this.write(f)
    }
    return this.readAvailable(timeoutMs)
  }

  /** consume an 8-byte ACK (classic dialect only) */
  private async expectClassicAck(timeoutMs = DEFAULT_TIMEOUT): Promise<void> {
    const ack = await this.readN(8, timeoutMs)
    const a = parseClassicAck(ack)
    if (!a) {
      throw new SbxpcError('پاسخ دستگاه قابل تشخیص نیست (ACK نامعتبر)', `ack=${hex(ack)}`)
    }
    if (a.mid !== this.machineId && a.mid !== 0xffff) this.machineId = a.mid
    if (a.result !== 1) {
      throw new SbxpcError(`دستگاه فرمان را نپذیرفت (کد ${a.result})`, `ack=${hex(ack)}`)
    }
  }

  private async expectResult(timeoutMs = DEFAULT_TIMEOUT): Promise<{ status: number; retCode: number }> {
    if (this.dialect === 'CLASSIC') await this.expectClassicAck(timeoutMs)
    const r = await this.readN(14, timeoutMs)
    const res = parseExeResult(r)
    if (!res) {
      throw new SbxpcError('فرمت پاسخ دستگاه نامعتبر است', `result=${hex(r)}`)
    }
    if (res.mid !== this.machineId && res.mid !== 0xffff) this.machineId = res.mid
    return { status: res.status, retCode: res.retCode }
  }

  /** cmd 0x113 — device serial number (the one the push gateway matches by) */
  async getSerialNumber(): Promise<string> {
    if (!this.dialect) await this.probeDialect()
    if (this.probeInfo?.serialNumber) return this.probeInfo.serialNumber
    if (this.dialect === 'HTTP_JSON') {
      const key = await npHttpPost(this.ip, NP_HTTP_PORT, '/getDeviceKey', { pass: this.password ?? 0 })
      if (key?.result) return key.data.trim()
      throw new SbxpcError('دستگاه فقط API HTTP دارد — خواندن سریال روی این نسل در نسخهٔ بعدی کامل می‌شود')
    }

    if (this.dialect === 'EXT_SECURE') {
      const drain = await this.sendCmd(0x113, 0, 0)
      if (drain.length < 12) throw new SbxpcError('دستگاه فرمان سریال را نپذیرفت', `recv=${hex(drain)}`)
      const ack = parseExtAck(xorCrypt(drain.subarray(0, 12), this.key()))
      if (!ack) throw new SbxpcError('دستگاه فرمان سریال را نپذیرفت (ACK رمزشده نامعتبر)', `recv=${hex(drain)}`)
      const serial = this.trySerialFromTailEnc(drain.subarray(12))
      if (!serial) throw new SbxpcError('دستگاه شماره سریال را برنگرداند', `recv=${hex(drain)}`)
      try { this.write(this.ackFrameExtEnc(0x113)) } catch { /* ignore */ }
      if (this.probeInfo) this.probeInfo.serialNumber = serial
      return serial
    }

    if (this.dialect === 'EXT') {
      const drain = await this.sendCmd(0x113, 0, 0)
      const ack = drain.length >= 12 ? parseExtAck(drain.subarray(0, 12)) : null
      if (!ack) throw new SbxpcError('دستگاه فرمان سریال را نپذیرفت', `recv=${hex(drain)}`)
      const serial = this.trySerialFromTail(drain.subarray(12))
      if (!serial) throw new SbxpcError('دستگاه شماره سریال را برنگرداند', `recv=${hex(drain)}`)
      try { this.write(this.ackFrameExt(0x113)) } catch { /* ignore */ }
      if (this.probeInfo) this.probeInfo.serialNumber = serial
      return serial
    }

    const drain = await this.sendCmd(0x113, 0, 0)
    // buffer may contain [ack][result][data] — normalize
    let off = 0
    const first = parseClassicAck(drain.subarray(0, 8))
    if (first && this.dialect === 'CLASSIC') {
      if (first.result !== 1) throw new SbxpcError(`دستگاه فرمان را نپذیرفت (کد ${first.result})`, `ack=${hex(drain.subarray(0, 8))}`)
      off = 8
    }
    const res = parseExeResult(drain.subarray(off, off + 14))
    if (!res || res.status === 0) {
      throw new SbxpcError('دستگاه شماره سریال را برنگرداند', `recv=${hex(drain)}`)
    }
    const data = parseDataFrame(drain.subarray(off + 14), 32)
    if (!data) throw new SbxpcError('دادهٔ سریال دریافت نشد', `recv=${hex(drain)}`)
    const s = data.toString('ascii').replace(/[\0\r\n\t ]+$/g, '').trim()
    if (this.probeInfo) this.probeInfo.serialNumber = s
    return s
  }

  /** cmd 0x10e — device clock */
  async getDeviceTime(): Promise<Date | null> {
    try {
      if (!this.dialect) await this.probeDialect()

      if (this.dialect === 'HTTP_JSON') {
        const t = await npHttpPost(this.ip, NP_HTTP_PORT, '/getDeviceTime', { pass: this.password ?? 0 })
        return t?.result ? parseHttpDate(t.data) : null
      }

      if (this.dialect === 'EXT_SECURE') {
        const drain = await this.sendCmd(0x10e, 0, 4)
        if (drain.length < 12) return null
        const ack = parseExtAck(xorCrypt(drain.subarray(0, 12), this.key()))
        if (!ack) return null
        const tail = drain.subarray(12)
        const key = this.key()
        let data: Buffer | null = null
        // classic A5 5A frame (encrypted or plain)
        if (tail.length >= 10) {
          data = parseDataFrame(xorCrypt(tail.subarray(0, 10), key), 4)
            ?? parseDataFrame(tail.subarray(0, 10), 4)
        }
        // SBPCCOMM _EXT [payload(4)][trailer(12)] block
        if (!data && tail.length >= 16) {
          data = parseExtTrailer(xorCrypt(tail.subarray(0, 16), key), 4)
        }
        if (!data) return null
        try { this.write(this.ackFrameExtEnc(0x10e)) } catch { /* ignore */ }
        return npTimestamp(data.readUInt32LE(0))
      }

      if (this.dialect === 'EXT') {
        const drain = await this.sendCmd(0x10e, 0, 4)
        const ack = drain.length >= 12 ? parseExtAck(drain.subarray(0, 12)) : null
        if (!ack) return null
        const data = drain.length >= 12 + 10 ? parseDataFrame(drain.subarray(12), 4) : null
        if (!data) return null
        try { this.write(this.ackFrameExt(0x10e)) } catch { /* ignore */ }
        return npTimestamp(data.readUInt32LE(0))
      }

      this.write(this.cmdHeader(0x10e, 0, 4))
      const drain = await this.readAvailable(DEFAULT_TIMEOUT)
      let off = 0
      const first = parseClassicAck(drain.subarray(0, 8))
      if (first && this.dialect === 'CLASSIC') off = 8
      const res = parseExeResult(drain.subarray(off, off + 14))
      if (!res || res.status === 0) return null
      const data = parseDataFrame(drain.subarray(off + 14), 4)
      if (!data) return null
      return npTimestamp(data.readUInt32LE(0))
    } catch {
      return null // non-fatal
    }
  }

  /** cmd 0x10b/0x10c — lock/unlock the device keypad during operations */
  async enableDevice(enabled: boolean): Promise<void> {
    if (this.dialect === 'EXT_SECURE') {
      // best-effort; not all EXT devices support it
      const cmd = enabled ? 0x10c : 0x10b
      this.write(this.cmdFrameExtEnc(cmd))
      await this.readAvailable(3000).catch(() => Buffer.alloc(0))
      return
    }
    if (this.dialect === 'EXT') {
      // best-effort; not all EXT devices support it
      const cmd = enabled ? 0x10c : 0x10b
      this.write(this.cmdFrameExt(cmd))
      await this.readAvailable(3000).catch(() => Buffer.alloc(0))
      return
    }
    this.write(this.cmdHeader(enabled ? 0x10c : 0x10b, 0, 0))
    if (this.dialect === 'CLASSIC') await this.expectClassicAck()
    await this.readN(14, DEFAULT_TIMEOUT)
  }

  /**
   * cmd 0x107 — read ALL attendance logs stored in the device
   * (vendor ReadAllGLogData flow, adapted per dialect).
   */
  async readAllLogs(timeoutMs = 45000): Promise<NpLog[]> {
    if (!this.dialect) await this.probeDialect()
    if (this.dialect === 'HTTP_JSON') {
      // /getLogInfo — the record schema is vendor-specific; enabled once we see a
      // real response (send your scripts/np-probe.js output). For now: a clear error.
      throw new SbxpcError(
        'این دستگاه از نسل جدید NP است و روی API HTTP (پورت 14460) کار می‌کند — تست اتصال موفق است. ' +
        'خواندن ترددها روی همین پروتکل در نسخهٔ بعدی فعال می‌شود؛ برای تسریع، خروجی scripts/np-probe.js را برای پشتیبانی بفرستید',
      )
    }

    if (this.dialect === 'CLASSIC_SECURE') {
      throw new SbxpcError(
        'گویش «کلاسیک رمزشده» تشخیص داده شد (رمز شبکه درست است) — خواندن ترددها روی این گویش در نسخهٔ بعدی فعال می‌شود؛ ' +
        'خروجی scripts/np-probe.js را برای پشتیبانی بفرستید تا اولویت‌بندی کنیم',
      )
    }

    if (this.dialect === 'EXT_SECURE') {
      return this.readAllLogsExtSecure(timeoutMs)
    }

    if (this.dialect === 'EXT') {
      return this.readAllLogsExt(timeoutMs)
    }

    // 1) count
    this.write(this.cmdHeader(0x107, 0, 0))
    if (this.dialect === 'CLASSIC') await this.expectClassicAck()
    const r1raw = await this.readN(14, DEFAULT_TIMEOUT)
    const r1 = parseExeResult(r1raw)
    if (!r1 || r1.status === 0) {
      throw new SbxpcError('دستگاه شمارش ترددها را برنگرداند', `result=${hex(r1raw)}`)
    }
    const count = r1.retCode
    if (count === 0) return []
    if (count > 300_000) throw new SbxpcError(`تعداد ترددهای دستگاه غیرمنطقی است (${count})`)

    // 2) data
    this.write(this.cmdHeader(0x107, 1, 0))
    if (this.dialect === 'CLASSIC') await this.expectClassicAck()
    this.write(this.dataFrame((() => { const b = Buffer.alloc(4); b.writeUInt32LE(1, 0); return b })()))
    const r2raw = await this.readN(14, DEFAULT_TIMEOUT)
    const r2 = parseExeResult(r2raw)
    if (!r2 || r2.status === 0) {
      throw new SbxpcError('دستگاه آماده ارسال ترددها نشد', `result=${hex(r2raw)}`)
    }
    const n = r2.retCode || count
    const raw = await this.recvBigData(n * 8, timeoutMs)
    this.write(this.dataFrame((() => { const b = Buffer.alloc(4); b.writeUInt32LE(n, 0); return b })()))
    const r3raw = await this.readN(14, DEFAULT_TIMEOUT)
    const r3 = parseExeResult(r3raw)
    const newFormat = r3?.retCode === 0x3e8

    const logs: NpLog[] = []
    for (let off = 0; off + 8 <= raw.length; off += 8) {
      const sec = raw.readUInt32LE(off)
      const userId = raw.readUInt16LE(off + 4)
      const modeB = raw[off + 6]
      const extra = raw[off + 7]
      const verifyMode = newFormat
        ? ((extra << 12) | ((modeB & 0xf0) << 4) | MODE_TABLE_NEW[modeB & 0xf]) & 0xffff
        : MODE_TABLE_OLD[modeB & 0xff] ?? 0
      logs.push({
        userId,
        ts: npTimestamp(sec).getTime(),
        verifyMode,
        raw: raw.subarray(off, off + 8).toString('hex'),
      })
    }
    return logs
  }

  /**
   * EXT-dialect log read — 12-byte frames. The vendor's SBPCCOMM _EXT path is
   * only partially documented; on failure the raw hex is included so the next
   * iteration can adapt precisely.
   */
  private async readAllLogsExt(timeoutMs: number): Promise<NpLog[]> {
    // phase 1 — count (exact 12-byte reads keep the stream in sync)
    this.write(this.cmdFrameExt(0x107))
    const d1 = await this.readN(12, DEFAULT_TIMEOUT)
    const ack1 = parseExtAck(d1)
    if (!ack1) {
      throw new SbxpcError(
        'دستگاه در حالت ۱۲بایتی (EXT) شمارش ترددها را برنگرداند — لطفاً متن خطا را برای پشتیبانی بفرستید',
        `recv=${hex(d1)}`,
      )
    }
    if (ack1.mid !== this.machineId && ack1.mid !== 0xffff) this.machineId = ack1.mid
    const count = ack1.dword
    if (count === 0) return []
    if (count > 300_000) throw new SbxpcError(`تعداد ترددهای دستگاه غیرمنطقی است (${count})`)

    // phase 2 — request data (classic flow shape, EXT framing)
    this.write(this.cmdFrameExt(0x107, 1))
    const d2 = await this.readN(12, DEFAULT_TIMEOUT)
    const ack2 = parseExtAck(d2)
    if (!ack2) {
      throw new SbxpcError('دستگاه ارسال ترددها را شروع نکرد', `recv=${hex(d2)}`)
    }
    const n = ack2.dword || count

    // bulk data: 5A A5 chunks of ≤1020 payload
    const raw = await this.recvBigData(n * 8, timeoutMs)
    try { this.write(this.ackFrameExt(0x107, n)) } catch { /* ignore */ }

    const logs: NpLog[] = []
    for (let off = 0; off + 8 <= raw.length; off += 8) {
      const sec = raw.readUInt32LE(off)
      const userId = raw.readUInt16LE(off + 4)
      const modeB = raw[off + 6]
      const extra = raw[off + 7]
      const verifyMode = ((extra << 12) | ((modeB & 0xf0) << 4) | MODE_TABLE_NEW[modeB & 0xf]) & 0xffff
      logs.push({
        userId,
        ts: npTimestamp(sec).getTime(),
        verifyMode,
        raw: raw.subarray(off, off + 8).toString('hex'),
      })
    }
    return logs
  }

  /**
   * EXT_SECURE log read — 12-byte encrypted frames; the bulk payload arrives
   * as encrypted blocks (payload + 12B trailer, per SBPCCOMM64 _EXT). If the
   * device streams some other shape, the raw hex accompanies the error so the
   * next iteration can adapt precisely.
   */
  private async readAllLogsExtSecure(timeoutMs: number): Promise<NpLog[]> {
    // phase 1 — count
    this.write(this.cmdFrameExtEnc(0x107))
    const d1 = await this.readN(12, DEFAULT_TIMEOUT)
    const ack1 = parseExtAck(xorCrypt(d1, this.key()))
    if (!ack1) {
      throw new SbxpcError(
        'دستگاه در حالت رمزشده شمارش ترددها را برنگرداند — لطفاً متن خطا را برای پشتیبانی بفرستید',
        `recv=${hex(d1)}`,
      )
    }
    if (ack1.mid !== this.machineId && ack1.mid !== 0xffff) this.machineId = ack1.mid
    const count = ack1.dword
    if (count === 0) return []
    if (count > 300_000) throw new SbxpcError(`تعداد ترددهای دستگاه غیرمنطقی است (${count})`)

    // phase 2 — request data
    this.write(this.cmdFrameExtEnc(0x107, 1))
    const d2 = await this.readN(12, DEFAULT_TIMEOUT)
    const ack2 = parseExtAck(xorCrypt(d2, this.key()))
    if (!ack2) throw new SbxpcError('دستگاه ارسال ترددها را شروع نکرد', `recv=${hex(d2)}`)
    const n = ack2.dword || count

    // bulk data: encrypted blocks — [payload][trailer 12] per ≤1020-byte chunk
    const raw = await this.recvBigDataExtSecure(n * 8, timeoutMs)
    try { this.write(this.ackFrameExtEnc(0x107, n)) } catch { /* ignore */ }

    const logs: NpLog[] = []
    for (let off = 0; off + 8 <= raw.length; off += 8) {
      const sec = raw.readUInt32LE(off)
      const userId = raw.readUInt16LE(off + 4)
      const modeB = raw[off + 6]
      const extra = raw[off + 7]
      const verifyMode = ((extra << 12) | ((modeB & 0xf0) << 4) | MODE_TABLE_NEW[modeB & 0xf]) & 0xffff
      logs.push({
        userId,
        ts: npTimestamp(sec).getTime(),
        verifyMode,
        raw: raw.subarray(off, off + 8).toString('hex'),
      })
    }
    return logs
  }

  /**
   * Receive bulk data in the EXT_SECURE dialect. Every frame is separately
   * XOR-encrypted (key phase restarts at each frame), so the whole stream is
   * drained first and then parsed offline. Accepted shapes, in order:
   *   a) one single [payload(total)][trailer 12B] block  (SBPCCOMM 0x2320 flow)
   *   b) chunked [payload(≤1020)][trailer 12B] blocks
   *   c) chunked classic big-data frames 5A A5 | mid | payload(≤1020) | cksum
   */
  private async recvBigDataExtSecure(totalLen: number, timeoutMs: number): Promise<Buffer> {
    const key = this.key()
    const overhead = Math.ceil(totalLen / BIG_CHUNK) * 12 + 24
    const raw = await this.readAvailable(Math.min(timeoutMs, 30_000), 900, totalLen + overhead)
    if (raw.length === 0) throw new SbxpcError('دستگاه داده‌ای ارسال نکرد (رمزشده)')

    // (a) single block: [payload(totalLen)][trailer]
    if (raw.length >= totalLen + 12) {
      const p = parseExtTrailer(xorCrypt(raw.subarray(0, totalLen + 12), key), totalLen)
      if (p) return p
    }

    // (b) chunked trailer blocks
    {
      const out: Buffer[] = []
      let off = 0
      let got = 0
      let ok = true
      while (got < totalLen) {
        const want = Math.min(BIG_CHUNK, totalLen - got)
        if (off + want + 12 > raw.length) { ok = false; break }
        const p = parseExtTrailer(xorCrypt(raw.subarray(off, off + want + 12), key), want)
        if (!p) { ok = false; break }
        out.push(p)
        got += want
        off += want + 12
      }
      if (ok && got === totalLen) return Buffer.concat(out)
    }

    // (c) chunked encrypted classic big-data frames
    {
      const out: Buffer[] = []
      let off = 0
      let got = 0
      let ok = true
      while (got < totalLen) {
        const want = Math.min(BIG_CHUNK, totalLen - got)
        if (off + 6 + want + 2 > raw.length) { ok = false; break }
        const dec = xorCrypt(raw.subarray(off, off + 6 + want + 2), key)
        if (dec[0] !== 0x5a || dec[1] !== 0xa5
          || dec.readUInt16LE(4 + want) !== sum16(dec, 0, 4 + want)) { ok = false; break }
        out.push(dec.subarray(4, 4 + want))
        got += want
        off += 6 + want + 2
      }
      if (ok && got === totalLen) return Buffer.concat(out)
    }

    throw new SbxpcError(
      'فرمت دادهٔ حجیم رمزشده قابل تشخیص نیست — لطفاً متن خطا را برای پشتیبانی بفرستید',
      `first=${hex(raw.subarray(0, Math.min(32, raw.length)))} len=${raw.length}`,
    )
  }

  /** Receive bulk data (chunked 5A A5 frames). */
  private async recvBigData(totalLen: number, timeoutMs = DEFAULT_TIMEOUT): Promise<Buffer> {
    const out = Buffer.alloc(totalLen)
    let got = 0
    while (got < totalLen) {
      const want = Math.min(BIG_CHUNK, totalLen - got)
      const f = await this.readN(want + 6, timeoutMs)
      if (f[0] !== 0x5a || f[1] !== 0xa5) {
        throw new SbxpcError('فرمت داده حجیم دریافتی نامعتبر است', `frame=${hex(f.subarray(0, Math.min(24, f.length)))}`)
      }
      if (f.readUInt16LE(4 + want) !== sum16(f, 0, 4 + want)) {
        throw new SbxpcError('داده حجیم دریافتی خراب است (چک‌سام)')
      }
      f.copy(out, got, 4, 4 + want)
      got += want
    }
    return out
  }
}

/* ---------------------------------------------------------------- */
/* connection test                                                   */
/* ---------------------------------------------------------------- */

export interface NpTestResult {
  ok: boolean
  serialNumber?: string
  deviceTime?: string | null
  dialect?: SbxDialect | null
  note?: string
  raw?: string
  error?: string
  detail?: string
  /** EXT_SECURE: the «رمز شبکه» the device accepted (also when auto-discovered) */
  password?: number
  /** EXT_SECURE: the salt variant that worked (0 = DLL default) */
  salt?: number
  /** HTTP_JSON: number of logs stored on the device (/getLogCount) */
  logCount?: number
}

/** Test the connection to an NP device: connect, probe dialect, read serial + clock. */
export async function npTest(ip: string, port: number, machineId = 1, password?: number | null): Promise<NpTestResult> {
  const client = new SbxpcClient(ip, port, machineId, password ?? null)
  try {
    await client.connect(6000)
    const probe = await client.probeDialect()
    // v1.2.4: the fallback reads must never kill an otherwise successful probe
    // (new dialects like HTTP_JSON/CLASSIC_SECURE cannot serve them)
    const serialNumber = probe.serialNumber
      ?? await client.getSerialNumber().catch(() => undefined)
    const deviceTime = probe.deviceTime
      ?? await client.getDeviceTime().catch(() => null)
    const raw = probe.exchanges.map((e) => `${e.dir === 'out' ? '»' : '«'} ${e.hex}`).join('\n')
    const ok = !!probe.dialect || !!serialNumber || !!deviceTime
    return {
      ok,
      serialNumber,
      deviceTime: deviceTime ? deviceTime.toISOString() : null,
      dialect: probe.dialect,
      note: probe.note,
      raw: raw.slice(0, 4000),
      password: probe.password,
      salt: probe.salt,
      logCount: probe.logCount,
    }
  } catch (err) {
    // v1.2.4: binary port unreachable? The "VisibleLight" generation speaks
    // HTTP on :14460 — try that before declaring the device dead
    if (err instanceof SbxpcError || /connect|ECONN|EHOST|timeout|مهلت/i.test(String((err as Error).message))) {
      const probe: ProbeInfo = { dialect: null, exchanges: [] }
      const client2 = new SbxpcClient(ip, port, machineId, password ?? null)
      const http = await client2.tryHttpApi(probe).catch(() => null)
      if (http) {
        return {
          ok: true,
          serialNumber: http.serialNumber,
          deviceTime: http.deviceTime ? http.deviceTime.toISOString() : null,
          dialect: 'HTTP_JSON',
          note: http.note,
          password: http.password,
          logCount: http.logCount,
          raw: `« (پورت ${port} باینری در دسترس نبود؛ دستگاه از طریق API HTTP پورت ${NP_HTTP_PORT} جواب داد)`,
        }
      }
    }
    return {
      ok: false,
      error: err instanceof Error ? err.message : String(err),
      detail: err instanceof SbxpcError ? err.detail : undefined,
    }
  } finally {
    client.disconnect()
  }
}
