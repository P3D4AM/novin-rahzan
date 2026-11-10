/**
 * SBXPC binary protocol client — Novin Pardaz NP-series devices (NP756+, NP756,
 * NP746, NP757, NP770, NP620, NP290, …)
 *
 * Reverse-engineered from the vendor's own NovinHozor software
 * (SBXPCDLL64.dll + SBPCCOMM64.dll). See scripts/SBXPC-PROTOCOL.md.
 *
 * Real-world devices come in (at least) three wire "dialects", so this client
 * PROBES the device on connect and adapts automatically:
 *
 *  CLASSIC        cmd(16B, 55AA) → ACK(8B, 5AA5) → RESULT(14B, AA55) → DATA(A55A)
 *  RESULT_DIRECT  cmd(16B, 55AA) → RESULT(14B, AA55) → DATA(A55A)      [no ACK]
 *  EXT            cmd(12B, 55AA) → ACK(12B, AA55) → DATA …            [SBPCCOMM *_EXT path]
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
 *     cksum = (Σ bytes[0..9]) & 0xFFFF
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

/** verify-mode lookup tables extracted from SBXPCDLL64 */
const MODE_TABLE_OLD = [4, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 15, 23, 24, 25, 26]
const MODE_TABLE_NEW = [23, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 4, 5, 6, 7, 22]

export type SbxDialect = 'CLASSIC' | 'RESULT_DIRECT' | 'EXT'

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
  if (f.readUInt16LE(10) !== sum16(f, 0, 10)) return null
  return { dword: f.readUInt32LE(4) >>> 0, mid }
}

/** A5 5A data frame of a KNOWN payload length */
export function parseDataFrame(f: Buffer, payloadLen: number): Buffer | null {
  const total = payloadLen + 6
  if (f.length < total || f[0] !== 0xa5 || f[1] !== 0x5a) return null
  if (f.readUInt16LE(4 + payloadLen) !== sum16(f, 0, 4 + payloadLen)) return null
  return f.subarray(4, 4 + payloadLen)
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
}

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
  ) {}

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
    await new Promise((r) => setTimeout(r, 250))
    await this.connect(timeoutMs)
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

      /* ---- attempt 2b: EXT dialect (12-byte frames) — reconnect for a clean
         device-side parser state (our classic frame may have half-consumed) ---- */
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

    /* ---- attempt 3: try GetDeviceTime (0x10e) classic — harmless ---- */
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

    /* ---- nothing recognized — but the device IS alive ---- */
    info.note = 'دستگاه پاسخ داد ولی فریم‌های آن با هیچ‌یک از گویش‌های شناخته‌شدهٔ پروتکل مطابقت نداشت'
    return info
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

  private async safeGetDeviceTime(): Promise<Date | null> {
    try { return await this.getDeviceTime() } catch { return null }
  }

  /* ---------------------------------------------------------------- */
  /* protocol operations                                               */
  /* ---------------------------------------------------------------- */

  /** send a command in the active dialect and drain the immediate response */
  private async sendCmd(cmd: number, arg1: number, arg2: number, timeoutMs = DEFAULT_TIMEOUT): Promise<Buffer> {
    if (this.dialect === 'EXT') {
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
}

/** Test the connection to an NP device: connect, probe dialect, read serial + clock. */
export async function npTest(ip: string, port: number, machineId = 1): Promise<NpTestResult> {
  const client = new SbxpcClient(ip, port, machineId)
  try {
    await client.connect(6000)
    const probe = await client.probeDialect()
    const serialNumber = probe.serialNumber
      ?? await client.getSerialNumber().catch(() => undefined)
    const deviceTime = probe.deviceTime
      ?? await client.getDeviceTime()
    const raw = probe.exchanges.map((e) => `${e.dir === 'out' ? '»' : '«'} ${e.hex}`).join('\n')
    const ok = !!probe.dialect || !!serialNumber || !!deviceTime
    return {
      ok,
      serialNumber,
      deviceTime: deviceTime ? deviceTime.toISOString() : null,
      dialect: probe.dialect,
      note: probe.note,
      raw: raw.slice(0, 4000),
    }
  } catch (err) {
    return {
      ok: false,
      error: err instanceof Error ? err.message : String(err),
      detail: err instanceof SbxpcError ? err.detail : undefined,
    }
  } finally {
    client.disconnect()
  }
}
