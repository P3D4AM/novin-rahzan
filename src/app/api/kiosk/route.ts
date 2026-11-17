import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { fail } from '@/lib/api'
import { onPunchRecorded } from '@/lib/notify'
import { recalcDayForPerson } from '@/lib/attendance'
import { tehranTodayMarker, addDaysMarker } from '@/lib/time'

/** Kiosk punch: POST { mode: "pin" | "qr", code?, pin?, token? } — no auth needed (shared device) */
export async function POST(req: NextRequest) {
  try {
    const b = await req.json()
    let person = null
    if (b.mode === 'pin') {
      if (!b.code || !b.pin) return NextResponse.json({ error: 'کد پرسنلی و پین الزامی است' }, { status: 400 })
      person = await db.personnel.findFirst({ where: { personnelCode: String(b.code).trim(), pin: String(b.pin).trim() } })
      if (!person) return NextResponse.json({ error: 'کد پرسنلی یا پین اشتباه است' }, { status: 404 })
    } else if (b.mode === 'qr') {
      if (!b.token) return NextResponse.json({ error: 'توکن QR نامعتبر است' }, { status: 400 })
      person = await db.personnel.findFirst({ where: { qrToken: String(b.token).trim() } })
      if (!person) return NextResponse.json({ error: 'کد QR نامعتبر است' }, { status: 404 })
    } else {
      return NextResponse.json({ error: 'حالت نامعتبر' }, { status: 400 })
    }

    if (person.status !== 'ACTIVE') {
      return NextResponse.json({ error: 'وضعیت این پرسنل فعال نیست' }, { status: 403 })
    }

    // find today's punches to determine direction
    // v1.6.0 (phase 1 stage 3): Tehran-day marker window — same convention as
    // /api/me POST and the engine's day attribution (was UTC calendar day).
    // Tehran 00:00–03:29 punches now correctly belong to the current Tehran day.
    const dayStart = tehranTodayMarker()
    const dayEnd = addDaysMarker(dayStart, 1)
    const todayPunches = await db.punchRecord.findMany({
      where: { personId: person.id, timestamp: { gte: dayStart, lt: dayEnd } },
      orderBy: { timestamp: 'desc' },
      take: 1,
    })
    const direction = todayPunches.length === 0 ? 'IN' : todayPunches[0].direction === 'IN' ? 'OUT' : 'IN'

    const now = new Date()
    // dedupe: same source within 60s
    const dup = await db.punchRecord.findFirst({
      where: { personId: person.id, timestamp: { gte: new Date(now.getTime() - 60000) }, source: b.mode === 'qr' ? 'QR' : 'KIOSK' },
    })
    if (dup) {
      return NextResponse.json({ error: 'ثبت مکرر! لطفاً یک دقیقه صبر کنید', person: { name: `${person.firstName} ${person.lastName}` } }, { status: 429 })
    }

    const punch = await db.punchRecord.create({
      data: { personId: person.id, timestamp: now, direction, source: b.mode === 'qr' ? 'QR' : 'KIOSK' },
    })
    await recalcDayForPerson(person.id, now)
    await onPunchRecorded({
      personId: person.id,
      personName: `${person.firstName} ${person.lastName}`,
      timestamp: now,
      direction,
      source: b.mode === 'qr' ? 'QR' : 'KIOSK',
    })
    return NextResponse.json({
      ok: true,
      direction,
      person: { id: person.id, name: `${person.firstName} ${person.lastName}`, code: person.personnelCode },
      timestamp: punch.timestamp,
    })
  } catch (err) {
    return fail(err)
  }
}
