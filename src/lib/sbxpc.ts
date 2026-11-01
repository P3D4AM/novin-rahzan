/**
 * SBXPC binary protocol client — Novin Pardaz NP-series devices (NP756+, NP756,
 * NP746, NP757, NP770, NP620, NP290, …)
 *
 * Reverse-engineered from the vendor's own NovinHozor software
 * (SBXPCDLL64.dll + SBPCCOMM64.dll). See scripts/SBXPC-PROTOCOL.md.
 *
 * The device LISTENS on TCP (default port 5005) and speaks small binary
 * frames; the same device also pushes real-time XML ("Ebio") to our gateway —
 * both channels coexist and complement each other:
 *   • push  (ebio.ts)  → live punches as they happen
 *   • pull  (this file) → test connection, read device info + stored logs
 *
 * Frame formats (all little-endian):
 *   PC → DEV COMMAND (16B):  55 AA | mid(2) | 79 19 | cmd(2) | arg2(4) | arg1(2) | cksum(2)
 *   DEV → PC ACK (8B):       5A A5 | mid(2) | ?(1) | result(2)==1 | cksum(2)
 *   DEV → PC RESULT (14B):   AA 55 | mid(2) | ?(2) | status(2) | retCode(4) | cksum(2)
 *   PC → DEV DATA:           A5 5A | mid(2) | data | cksum(2)
 *   DEV → PC DATA:           A5 5A | mid(2) | data | cksum(2)
 *   DEV → PC BIG DATA:       5A A5 | mid(2) | data(≤1020) | cksum(2)  (chunked)
 */
import * as net from 'net'

const COMPANY_MARK = 0x1979
const BIG_CHUNK = 0x3fc // 1020 — max payload per data frame
const DEFAULT_TIMEOUT = 8000

/** verify-mode lookup tables extracted from SBXPCDLL64 */
const MODE_TABLE_OLD = [4, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 15, 23, 24, 25, 26]
const MODE_TABLE_NEW = [23, 1, 2, 3, 10, 11, 12, 13, 14, 20, 21, 4, 5, 6, 7, 22]

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

export interface NpDeviceIdentity {
  serialNumber: string
  deviceTime: Date | null
  firmware?: string
}

export class SbxpcClient {
  private socket: net.Socket | null = null
  private buf: Buffer = Buffer.alloc(0)
  private closed = false

  constructor(
    private readonly ip: string,
    private readonly port: number,
    private readonly machineId = 1,
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
    let sum = 0
    for (let i = 0; i < 14; i++) sum += h[i]
    h.writeUInt16LE(sum & 0xffff, 14)
    return h
  }

  private dataFrame(payload: Buffer): Buffer {
    const f = Buffer.alloc(payload.length + 6)
    f.writeUInt16LE(0x5aa5, 0) // bytes: A5 5A
    f.writeUInt16LE(this.machineId, 2)
    payload.copy(f, 4)
    let sum = 0
    for (let i = 0; i < 4 + payload.length; i++) sum += f[i]
    f.writeUInt16LE(sum & 0xffff, 4 + payload.length)
    return f
  }

  private write(buf: Buffer) {
    if (!this.socket || this.closed) throw new SbxpcError('اتصال به دستگاه برقرار نیست')
    this.socket.write(buf)
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

  /* ---------------------------------------------------------------- */
  /* protocol steps                                                    */
  /* ---------------------------------------------------------------- */

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
        resolve()
      })
    })
    // socket error handling from now on: mark closed
    this.socket!.on('error', () => { this.closed = true })
    this.socket!.on('close', () => { this.closed = true })
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

  /** Send a command and wait for the 8-byte ACK. Throws with raw bytes on failure. */
  private async sendCommand(cmd: number, arg1: number, arg2: number, timeoutMs = DEFAULT_TIMEOUT): Promise<void> {
    this.write(this.cmdHeader(cmd, arg1, arg2))
    const ack = await this.readN(8, timeoutMs)
    if (ack[0] !== 0x5a || ack[1] !== 0xa5) {
      throw new SbxpcError('پاسخ دستگاه قابل تشخیص نیست — احتمالاً این پورت به دستگاه تعلق ندارد', `ack=${hex(ack)}`)
    }
    let sum = (ack[2] + ack[3] + ack[4] + ack[5] + 0xff) & 0xffff
    if (ack.readUInt16LE(6) !== sum) {
      throw new SbxpcError('پاسخ دستگاه خراب دریافت شد (چک‌سام)', `ack=${hex(ack)}`)
    }
    const result = ack.readUInt16LE(4)
    if (result !== 1) {
      throw new SbxpcError(`دستگاه فرمان را نپذیرفت (کد ${result})`, `ack=${hex(ack)}`)
    }
  }

  /** Receive the 14-byte execution-result frame. */
  private async recvResult(timeoutMs = DEFAULT_TIMEOUT): Promise<{ status: number; retCode: number }> {
    const r = await this.readN(14, timeoutMs)
    if (r[0] !== 0xaa || r[1] !== 0x55) {
      throw new SbxpcError('فرمت پاسخ دستگاه نامعتبر است', `result=${hex(r)}`)
    }
    let sum = 0
    for (let i = 0; i < 12; i++) sum += r[i]
    if (r.readUInt16LE(12) !== (sum & 0xffff)) {
      throw new SbxpcError('پاسخ دستگاه خراب دریافت شد (چک‌سام)', `result=${hex(r)}`)
    }
    return { status: r.readUInt16LE(6), retCode: r.readUInt32LE(8) }
  }

  /** Send raw payload data to the device (A5 5A frame). */
  private async sendData(payload: Buffer): Promise<void> {
    if (payload.length > BIG_CHUNK) throw new SbxpcError('حجم داده ارسالی بیش از حد مجاز پروتکل است')
    this.write(this.dataFrame(payload))
  }

  /** Receive a data frame of the given payload length (A5 5A). */
  private async recvData(len: number, timeoutMs = DEFAULT_TIMEOUT): Promise<Buffer> {
    const f = await this.readN(len + 6, timeoutMs)
    if (f[0] !== 0xa5 || f[1] !== 0x5a) {
      throw new SbxpcError('فرمت داده دریافتی نامعتبر است', `frame=${hex(f.subarray(0, Math.min(24, f.length)))}`)
    }
    let sum = 0
    for (let i = 0; i < 4 + len; i++) sum += f[i]
    if (f.readUInt16LE(4 + len) !== (sum & 0xffff)) {
      throw new SbxpcError('داده دریافتی خراب است (چک‌سام)')
    }
    return f.subarray(4, 4 + len)
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
      let sum = 0
      for (let i = 0; i < 4 + want; i++) sum += f[i]
      if (f.readUInt16LE(4 + want) !== (sum & 0xffff)) {
        throw new SbxpcError('داده حجیم دریافتی خراب است (چک‌سام)')
      }
      f.copy(out, got, 4, 4 + want)
      got += want
    }
    return out
  }

  /* ---------------------------------------------------------------- */
  /* high-level operations                                             */
  /* ---------------------------------------------------------------- */

  /** cmd 0x113 — device serial number (the one the push gateway matches by) */
  async getSerialNumber(): Promise<string> {
    await this.sendCommand(0x113, 0, 0)
    const r = await this.recvResult()
    if (r.status === 0) throw new SbxpcError('دستگاه شماره سریال را برنگرداند', `status=${r.status} ret=${r.retCode}`)
    const data = await this.recvData(32)
    const s = data.toString('ascii').replace(/[\0\r\n\t ]+$/g, '').trim()
    return s
  }

  /** cmd 0x10e — device clock */
  async getDeviceTime(): Promise<Date | null> {
    try {
      await this.sendCommand(0x10e, 0, 4)
      const r = await this.recvResult()
      if (r.status === 0) return null
      const data = await this.recvData(4)
      const v = data.readUInt32LE(0)
      return npTimestamp(v)
    } catch {
      return null // non-fatal
    }
  }

  /** cmd 0x10b/0x10c — lock/unlock the device keypad during operations */
  async enableDevice(enabled: boolean): Promise<void> {
    await this.sendCommand(enabled ? 0x10c : 0x10b, 0, 0)
    await this.recvResult()
  }

  /**
   * cmd 0x107 — read ALL attendance logs stored in the device
   * (exactly the vendor's ReadAllGLogData flow).
   */
  async readAllLogs(timeoutMs = 30000): Promise<NpLog[]> {
    // 1) count
    await this.sendCommand(0x107, 0, 0)
    const r1 = await this.recvResult()
    if (r1.status === 0) throw new SbxpcError('دستگاه شمارش ترددها را برنگرداند', `status=${r1.status} ret=${r1.retCode}`)
    const count = r1.retCode
    if (count === 0) return []
    if (count > 300_000) throw new SbxpcError(`تعداد ترددهای دستگاه غیرمنطقی است (${count})`)

    // 2) data
    await this.sendCommand(0x107, 1, 0)
    await this.sendData(Buffer.from([1, 0, 0, 0]))
    const r2 = await this.recvResult()
    if (r2.status === 0) throw new SbxpcError('دستگاه آماده ارسال ترددها نشد', `status=${r2.status} ret=${r2.retCode}`)
    const n = r2.retCode || count
    const raw = await this.recvBigData(n * 8, timeoutMs)
    await this.sendData((() => { const b = Buffer.alloc(4); b.writeUInt32LE(n, 0); return b })())
    const r3 = await this.recvResult()
    const newFormat = r3.retCode === 0x3e8

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
}

/** Test the connection to an NP device: connect, read serial + clock. */
export async function npTest(ip: string, port: number, machineId = 1): Promise<{
  ok: boolean
  serialNumber?: string
  deviceTime?: string | null
  error?: string
  detail?: string
}> {
  const client = new SbxpcClient(ip, port, machineId)
  try {
    await client.connect(6000)
    const serialNumber = await client.getSerialNumber()
    const deviceTime = await client.getDeviceTime()
    return {
      ok: true,
      serialNumber,
      deviceTime: deviceTime ? deviceTime.toISOString() : null,
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
