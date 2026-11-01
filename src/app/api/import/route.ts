import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { recalcRange } from '@/lib/attendance'
import { startOfUTCDay, addDays } from '@/lib/jalali'

/** POST multipart: file + mode=punches|personnel + mapping JSON */
export async function POST(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const form = await req.formData()
    const file = form.get('file') as File | null
    const mode = String(form.get('mode') || 'punches')
    if (!file) throw new ApiError(400, 'فایلی ارسال نشده است')

    const XLSX = await import('xlsx')
    const buf = Buffer.from(await file.arrayBuffer())
    let wb
    try {
      wb = XLSX.read(buf, { type: 'buffer', codepage: 65001 })
    } catch {
      throw new ApiError(400, 'فایل قابل خواندن نیست (Excel یا CSV مورد انتظار است)')
    }
    const sheet = wb.Sheets[wb.SheetNames[0]]
    const rows = XLSX.utils.sheet_to_json<Record<string, unknown>>(sheet, { defval: '' })
    if (!rows.length) throw new ApiError(400, 'فایل خالی است')

    const headers = Object.keys(rows[0])

    if (mode === 'personnel') {
      let created = 0, skipped = 0
      for (const r of rows) {
        const code = String(r['کد'] ?? r['کد پرسنلی'] ?? r['code'] ?? '').trim()
        const name = String(r['نام'] ?? r['نام و نام خانوادگی'] ?? r['name'] ?? '').trim()
        if (!code || !name) { skipped++; continue }
        const parts = name.split(/\s+/)
        const firstName = parts.slice(0, -1).join(' ') || name
        const lastName = parts.slice(-1).join(' ') || '-'
        const exists = await db.personnel.findFirst({ where: { OR: [{ personnelCode: code }] } })
        if (exists) { skipped++; continue }
        await db.personnel.create({
          data: {
            personnelCode: code, firstName, lastName,
            phone: String(r['موبایل'] ?? r['تلفن'] ?? r['phone'] ?? '') || null,
            nationalId: String(r['کد ملی'] ?? r['nationalId'] ?? '') || null,
            pin: String(1000 + Math.floor(Math.random() * 9000)),
          },
        })
        created++
      }
      return NextResponse.json({ ok: true, created, skipped, headers })
    }

    // punches mode
    const persons = await db.personnel.findMany({ select: { id: true, personnelCode: true, deviceUserId: true } })
    const byCode = new Map(persons.map((p) => [p.personnelCode, p.id]))
    const byUid = new Map<number, string>()
    for (const p of persons) {
      if (p.deviceUserId != null) byUid.set(p.deviceUserId, p.id)
      const n = Number(p.personnelCode)
      if (!Number.isNaN(n)) byUid.set(n, p.id)
    }

    let created = 0, skipped = 0, unmatched = 0
    let minDate: Date | null = null
    let maxDate: Date | null = null
    for (const r of rows) {
      const uidRaw = String(r['uid'] ?? r['کد'] ?? r['کد پرسنلی'] ?? r['user'] ?? r['id'] ?? '').trim()
      const dateRaw = String(r['تاریخ'] ?? r['date'] ?? '').trim()
      const timeRaw = String(r['ساعت'] ?? r['time'] ?? '').trim()
      const dtRaw = String(r['datetime'] ?? r['زمان'] ?? '').trim()
      let ts: Date | null = null
      try {
        if (dtRaw) ts = new Date(dtRaw)
        else if (dateRaw) ts = new Date(`${dateRaw}T${timeRaw || '00:00'}:00`)
        if (ts && isNaN(ts.getTime())) ts = null
      } catch { ts = null }
      if (!ts) { skipped++; continue }

      let personId = byCode.get(uidRaw) ?? undefined
      if (!personId) {
        const n = Number(uidRaw)
        personId = Number.isNaN(n) ? undefined : byUid.get(n)
      }
      if (!personId) { unmatched++; continue }

      const exists = await db.punchRecord.findFirst({
        where: { personId, timestamp: ts, source: 'IMPORT' },
        select: { id: true },
      })
      if (exists) { skipped++; continue }

      const dirRaw = String(r['ورود/خروج'] ?? r['direction'] ?? r['state'] ?? '').trim()
      const direction = /خروج|out|1/i.test(dirRaw) ? 'OUT' : /ورود|in|0/i.test(dirRaw) ? 'IN' : 'AUTO'

      await db.punchRecord.create({
        data: { personId, timestamp: ts, direction, source: 'IMPORT', rawPayload: JSON.stringify(r).slice(0, 500) },
      })
      created++
      if (!minDate || ts < minDate) minDate = ts
      if (!maxDate || ts > maxDate) maxDate = ts
    }

    if (created && minDate && maxDate) {
      await recalcRange(persons.map((p) => p.id), startOfUTCDay(minDate), addDays(startOfUTCDay(maxDate), 1))
    }
    return NextResponse.json({ ok: true, created, skipped, unmatched, headers, range: minDate ? { from: minDate, to: maxDate } : null })
  } catch (err) {
    return fail(err)
  }
}
