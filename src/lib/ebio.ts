/**
 * Novin Pardaz "Ebio" push gateway (NP-series devices: NP756+, NP756, NP746,
 * NP757, NP770, NP620, NP290, …)
 *
 * Wire protocol (reverse-engineered from the vendor's own NovinHozor
 * software — PacketParserStandard.dll / EbioTcpClientConnection):
 *
 *  • The DEVICE opens a TCP connection TO this server (default port 5005)
 *    and pushes short XML messages as it goes — real-time attendance logs.
 *  • Bytes are decoded as Windows-1256; each message is a small XML document
 *    whose root has direct children:
 *      TerminalType (e.g. "756PLUS"), TerminalID, DeviceSerialNo, TransID,
 *      Event = TimeLog | AdminLog | Alarm | KeepAlive
 *  • TimeLog adds: Year, Month, Day, Hour, Minute, Second, UserID, DoorID,
 *      AttendStat, VerifMode, JobCode, APStat, LogImage (base64, optional)
 *  • The server MUST ACK each message, ASCII + trailing NUL byte:
 *      <?xml version="1.0"?><Message><Request>UploadedLog</Request><TransID>N</TransID></Message>\0
 *      (KeptAlive for KeepAlive events) — without an ACK the device retries.
 *  • Messages whose DeviceSerialNo does not match a registered device are
 *    rejected with no ACK (same behaviour as the vendor software — this is
 *    why the serial number is required when defining the device).
 */
import * as net from 'net'
import { db } from '@/lib/db'
import { recalcDayForPerson } from '@/lib/attendance'
import { emitRealtime } from '@/lib/notify'

const DEFAULT_PORT = Number(process.env.EBIO_PORT || 5005)
const MAX_BUFFER = 256 * 1024

export interface EbioStatus {
  running: boolean
  port: number
  connections: number
  startedAt: string | null
  lastEventAt: string | null
  lastError: string | null
  events: number
  punches: number
  lastUnknownSerial: string | null
}

interface Gateway {
  server: net.Server
  status: EbioStatus
  unknownLoggedAt: Map<string, number>
}

const GKEY = '__hozorplus_ebio_gateway'

function gateway(): Gateway | undefined {
  return (globalThis as Record<string, unknown>)[GKEY] as Gateway | undefined
}

/** Current gateway status (safe to call even when not started) */
export function getEbioStatus(): EbioStatus {
  const g = gateway()
  if (!g) {
    return {
      running: false, port: DEFAULT_PORT, connections: 0, startedAt: null,
      lastEventAt: null, lastError: null, events: 0, punches: 0, lastUnknownSerial: null,
    }
  }
  return { ...g.status, connections: g.server.connections }
}

/* ------------------------------------------------------------------ */
/* helpers                                                             */
/* ------------------------------------------------------------------ */

let cp1256: TextDecoder | null = null
function decodeBytes(buf: Buffer): string {
  try {
    cp1256 ??= new TextDecoder('windows-1256')
    return cp1256.decode(buf)
  } catch {
    return buf.toString('latin1')
  }
}

/** Persian (۰-۹) and Arabic-Indic (٠-٩) digits → ASCII, like the vendor's FixString */
function fixDigits(s: string): string {
  const fa = '۰۱۲۳۴۵۶۷۸۹'
  const ar = '٠١٢٣٤٥٦٧٨٩'
  return s.replace(/[۰-۹٠-٩]/g, (ch) => {
    const i = fa.indexOf(ch)
    return String(i >= 0 ? i : ar.indexOf(ch))
  })
}

/** Extract a direct child element's text value (tolerant, no XML parser needed) */
function el(xml: string, tag: string): string | null {
  const m = xml.match(new RegExp(`<${tag}(?:\\s[^>]*)?>([\\s\\S]*?)</${tag}>`, 'i'))
  return m ? m[1].trim() : null
}

/**
 * Split a raw stream into complete XML messages.
 * Keeps any trailing partial message so it can be completed by the next chunk.
 */
function extractMessages(text: string): { msgs: string[]; rest: string } {
  const msgs: string[] = []
  let s = text.replace(/\x00/g, '')
  for (;;) {
    const start = s.search(/<\??\s*[A-Za-z_]/)
    if (start < 0) { s = ''; break }
    const rest = s.slice(start)
    const rootM = rest.match(/^<\?xml[^>]*\?>\s*<([A-Za-z_][\w:.-]*)/) || rest.match(/^<([A-Za-z_][\w:.-]*)/)
    if (!rootM) { s = rest; break }
    const close = `</${rootM[1]}>`
    const end = rest.toUpperCase().indexOf(close.toUpperCase())
    if (end < 0) break // incomplete — wait for more bytes
    msgs.push(rest.slice(0, end + close.length))
    s = rest.slice(end + close.length)
  }
  return { msgs, rest: s }
}

/** Tehran wall-clock (as sent by the device) → correct UTC instant */
function wallClockToUTC(y: number, mo: number, d: number, h: number, mi: number, se: number): Date {
  const asUtc = Date.UTC(y, mo - 1, d, h, mi, se)
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
    // Asia/Tehran unavailable — fixed +03:30 (no DST since 2022)
    return new Date(asUtc - 3.5 * 3600 * 1000)
  }
}

function ack(socket: net.Socket, request: string, transId: number) {
  const xml = `<?xml version="1.0"?><Message><Request>${request}</Request><TransID>${transId}</TransID></Message>`
  const buf = Buffer.alloc(xml.length + 1)
  buf.write(xml, 0, 'ascii')
  buf[xml.length] = 0 // vendor protocol: trailing NUL
  socket.write(buf)
}

/** OUT-ish attendance states → direction (NP devices mostly use 0=In 1=Out) */
function directionFrom(stat: string | null): 'IN' | 'OUT' | 'AUTO' {
  if (!stat) return 'AUTO'
  const s = stat.trim()
  if (/out|خروج/i.test(s)) return 'OUT'
  const n = Number(s)
  if (!Number.isNaN(n)) return n === 1 || n === 2 || n === 5 ? 'OUT' : 'IN'
  return 'AUTO'
}

async function logDevice(deviceId: string | null, level: string, message: string) {
  try {
    await db.deviceLog.create({ data: { deviceId, level, message } })
  } catch { /* ignore */ }
}

/** Unknown serials are logged at most once per 10 minutes per serial */
async function logUnknownSerial(g: Gateway, serial: string, termType: string, ip: string) {
  g.status.lastUnknownSerial = serial
  const now = Date.now()
  const last = g.unknownLoggedAt.get(serial) ?? 0
  if (now - last < 10 * 60 * 1000) return
  g.unknownLoggedAt.set(serial, now)
  if (g.unknownLoggedAt.size > 200) g.unknownLoggedAt.clear()
  await logDevice(null, 'WARN',
    `پیام از دستگاه ثبت‌نشده دریافت شد — سریال «${serial}»${termType ? ` (نوع ${termType})` : ''} از ${ip}. برای فعال‌سازی، دستگاهی با همین شماره سریال تعریف کنید.`)
}

/* ------------------------------------------------------------------ */
/* message handling                                                    */
/* ------------------------------------------------------------------ */

interface ParsedMsg {
  termType: string
  termId: number
  serial: string
  transId: number
  event: string
  raw: string
}

function parseMessage(xml: string): ParsedMsg | null {
  const event = el(xml, 'Event')
  const serial = el(xml, 'DeviceSerialNo') ?? ''
  const termType = el(xml, 'TerminalType') ?? ''
  if (!event && !serial && !termType) return null
  const transId = Number(fixDigits(el(xml, 'TransID') ?? '-1'))
  return {
    termType,
    termId: Number(fixDigits(el(xml, 'TerminalID') ?? '-1')),
    serial: serial.trim(),
    transId: Number.isNaN(transId) ? -1 : transId,
    event: (event ?? '').trim(),
    raw: xml,
  }
}

async function handleTimeLog(g: Gateway, msg: ParsedMsg, deviceId: string): Promise<string | null> {
  const num = (t: string) => { const n = Number(fixDigits(t)); return Number.isNaN(n) ? null : n }
  const y = num(el(msg.raw, 'Year') ?? '') ?? new Date().getFullYear()
  const mo = num(el(msg.raw, 'Month') ?? '') ?? 1
  const d = num(el(msg.raw, 'Day') ?? '') ?? 1
  const h = num(el(msg.raw, 'Hour') ?? '') ?? 0
  const mi = num(el(msg.raw, 'Minute') ?? '') ?? 0
  const se = num(el(msg.raw, 'Second') ?? '') ?? 0
  const userRaw = fixDigits(el(msg.raw, 'UserID') ?? '').trim()
  const userId = Number(userRaw)
  if (!userRaw || Number.isNaN(userId)) {
    await logDevice(deviceId, 'WARN', `تردد بدون شناسه کاربر (TransID ${msg.transId}) — نادیده گرفته شد`)
    return null
  }
  const timestamp = wallClockToUTC(y, mo, d, h, mi, se)

  // map device user id → personnel (deviceUserId or numeric personnelCode)
  const persons = await db.personnel.findMany({ select: { id: true, deviceUserId: true, personnelCode: true, firstName: true, lastName: true } })
  const byUid = new Map<number, { id: string; name: string }>()
  for (const p of persons) {
    if (p.deviceUserId != null) byUid.set(p.deviceUserId, { id: p.id, name: `${p.firstName} ${p.lastName}` })
    const code = Number(p.personnelCode)
    if (!Number.isNaN(code) && !byUid.has(code)) byUid.set(code, { id: p.id, name: `${p.firstName} ${p.lastName}` })
  }
  const person = byUid.get(userId)
  if (!person) {
    await logDevice(deviceId, 'WARN',
      `شناسه ${userRaw} روی دستگاه «${msg.serial}» به هیچ پرسنلی نگاشت نشده است — پرونده پرسنل را باز کنید و «شناسه روی دستگاه» را ${userRaw} قرار دهید.`)
    return null
  }

  const direction = directionFrom(el(msg.raw, 'AttendStat'))
  if (!Number.isFinite(timestamp.getTime())) {
    await logDevice(deviceId, 'ERROR', `زمان نامعتبر در پیام دستگاه (TransID ${msg.transId}) — سال/ماه/روز/ساعت را بررسی کنید`)
    return null
  }
  const raw = {
    termType: msg.termType, termId: msg.termId, serial: msg.serial, transId: msg.transId,
    attendStat: el(msg.raw, 'AttendStat'), verifMode: el(msg.raw, 'VerifMode'),
    doorId: el(msg.raw, 'DoorID'), jobCode: el(msg.raw, 'JobCode'), apStat: el(msg.raw, 'APStat'),
  }
  try {
    await db.punchRecord.create({
      data: {
        personId: person.id,
        timestamp,
        direction,
        source: 'DEVICE',
        deviceId,
        deviceUserId: userId,
        rawPayload: JSON.stringify(raw),
      },
    })
  } catch (err) {
    console.error('[ebio] punch create failed:', err)
    await logDevice(deviceId, 'ERROR', `ثبت تردد از دستگاه شکست خورد: ${err instanceof Error ? err.message : String(err)}`)
    return null // duplicate (same person+time+source) — already recorded
  }

  g.status.punches++
  try {
    await db.device.update({ where: { id: deviceId }, data: { lastSyncAt: new Date(), lastSyncCount: { increment: 1 }, status: 'ONLINE' } })
    await recalcDayForPerson(person.id, timestamp)
    await emitRealtime('punch', {
      personId: person.id, personName: person.name,
      timestamp: timestamp.toISOString(), direction, source: 'DEVICE',
    })
    await emitRealtime('device-sync', { deviceId, created: 1 })
    await logDevice(deviceId, 'INFO', `تردد ${person.name} — ${direction === 'OUT' ? 'خروج' : direction === 'IN' ? 'ورود' : 'ثبت'} در ${`${y}/${String(mo).padStart(2, '0')}/${String(d).padStart(2, '0')} ${String(h).padStart(2, '0')}:${String(mi).padStart(2, '0')}:${String(se).padStart(2, '0')}`} (شناسه ${userRaw})`)
  } catch { /* non-fatal */ }
  return person.name
}

async function handleMessage(g: Gateway, socket: net.Socket, xml: string, ip: string) {
  const msg = parseMessage(xml)
  if (!msg) return
  g.status.events++
  g.status.lastEventAt = new Date().toISOString()

  // ignore HTTP-ish probes (vendor software does the same)
  if (/post\s+\/\s+http/i.test(msg.raw)) return

  if (msg.event !== 'KeepAlive' && !msg.serial) {
    await logDevice(null, 'WARN', `پیام بدون شماره سریال از ${ip} نادیده گرفته شد`)
    return
  }

  // match the device by serial number (case-insensitive) — exactly like the vendor software
  const devices = await db.device.findMany({ where: { serialNumber: { not: null } } })
  const device = devices.find((dv) => (dv.serialNumber ?? '').trim().toLowerCase() === msg.serial.toLowerCase())

  if (!device) {
    await logUnknownSerial(g, msg.serial, msg.termType, ip)
    return // no ACK → the device retries (vendor behaviour)
  }
  if (!device.enabled) {
    await logDevice(device.id, 'WARN', 'دستگاه غیرفعال است — پیام نادیده گرفته شد')
    return
  }

  try {
    if (msg.event === 'KeepAlive') {
      await db.device.update({ where: { id: device.id }, data: { status: 'ONLINE' } })
      ack(socket, 'KeptAlive', msg.transId)
      return
    }

    if (msg.event === 'TimeLog') {
      await handleTimeLog(g, msg, device.id)
      ack(socket, 'UploadedLog', msg.transId)
      return
    }

    if (msg.event === 'AdminLog' || msg.event === 'Alarm') {
      const who = el(xml, 'AdminID') ?? el(xml, 'UserID') ?? '—'
      const kind = msg.event === 'AdminLog' ? 'مدیریتی' : 'هشدار/آلارم'
      await db.device.update({ where: { id: device.id }, data: { status: 'ONLINE' } })
      await logDevice(device.id, 'INFO', `رویداد ${kind} از دستگاه ${msg.termType} (شناسه ${who}${el(xml, 'Type') ? `، نوع ${el(xml, 'Type')}` : ''})`)
      ack(socket, 'UploadedLog', msg.transId)
      return
    }

    // unknown event type — still ACK so the device does not retry forever
    ack(socket, 'UploadedLog', msg.transId)
  } catch (err) {
    g.status.lastError = err instanceof Error ? err.message : String(err)
  }
}

/* ------------------------------------------------------------------ */
/* server                                                              */
/* ------------------------------------------------------------------ */

/** Start the Ebio push gateway (idempotent; called from instrumentation.ts) */
export function startEbioGateway(): EbioStatus {
  const existing = gateway()
  if (existing) return getEbioStatus()

  const g: Gateway = {
    unknownLoggedAt: new Map(),
    status: {
      running: false, port: DEFAULT_PORT, connections: 0, startedAt: null,
      lastEventAt: null, lastError: null, events: 0, punches: 0, lastUnknownSerial: null,
    },
    server: net.createServer(),
  }
  ;(globalThis as Record<string, unknown>)[GKEY] = g

  g.server.on('connection', (socket) => {
    const ip = socket.remoteAddress ?? '?'
    let buffer = ''
    socket.on('data', async (chunk: Buffer) => {
      try {
        buffer += decodeBytes(chunk)
        if (buffer.length > MAX_BUFFER) buffer = buffer.slice(-MAX_BUFFER)
        const { msgs, rest } = extractMessages(fixDigits(buffer))
        buffer = rest
        for (const xml of msgs) {
          await handleMessage(g, socket, xml, ip)
        }
      } catch (err) {
        g.status.lastError = err instanceof Error ? err.message : String(err)
      }
    })
    socket.on('error', () => { /* device dropped the line — normal */ })
    socket.on('close', () => { buffer = '' })
    // vendor server closes idle connections after 60s; the device reconnects
    socket.setTimeout(90_000, () => socket.end())
  })

  g.server.on('error', (err) => {
    g.status.running = false
    g.status.lastError = err.message
    console.error(`[ebio] gateway error on port ${g.status.port}: ${err.message}`)
  })

  g.server.listen(DEFAULT_PORT, '0.0.0.0', () => {
    g.status.running = true
    g.status.startedAt = new Date().toISOString()
    console.log(`[ebio] Novin Pardaz push gateway listening on 0.0.0.0:${DEFAULT_PORT} (NP-series devices connect here)`)
  })
  return getEbioStatus()
}
