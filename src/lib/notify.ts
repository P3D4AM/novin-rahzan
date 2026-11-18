import { db } from '@/lib/db'
import { addDays, tehranDayOf } from '@/lib/jalali'
import { recalcDayForPerson } from '@/lib/attendance'

// internal emit endpoint (localhost only). HOZOR_RT_URL allows tests to
// point emitRealtime at their own listener without touching production code.
const RT_URL = process.env.HOZOR_RT_URL || 'http://localhost:3004/emit'

/** Push an event to the realtime socket.io service (best effort) */
export async function emitRealtime(event: string, payload: unknown) {
  try {
    await fetch(RT_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ event, payload }),
      signal: AbortSignal.timeout(2000),
    })
  } catch {
    // realtime service not running — ignore
  }
}

async function notifyRoles(roles: string[], title: string, body: string, type: string, refId?: string) {
  for (const role of roles) {
    await db.notification.create({ data: { role, title, body, type, refId } })
  }
  const users = await db.user.findMany({ where: { role: { in: roles }, active: true } })
  for (const u of users) {
    await db.notification.create({ data: { userId: u.id, title, body, type, refId } })
  }
}

export async function notifyUser(userId: string, title: string, body: string, type = 'INFO', refId?: string) {
  await db.notification.create({ data: { userId, title, body, type, refId } })
  await emitRealtime('notification', { userId, title, body, type })
}

/** Fire after a new punch is recorded: evaluate rules + emit live feed */
export async function onPunchRecorded(input: {
  personId: string
  personName: string
  timestamp: Date
  direction: string
  source: string
}) {
  const { personId, personName, timestamp } = input
  await emitRealtime('punch', {
    personId,
    personName,
    timestamp: timestamp.toISOString(),
    direction: input.direction,
    source: input.source,
  })

  const rules = await db.notificationRule.findMany({ where: { enabled: true } })

  const inRule = rules.find((r) => r.type === 'CHECKIN')
  if (inRule && inRule.scope === 'ALL') {
    // thresholdMin = minutes to consider "late notification"
    const today = tehranDayOf(timestamp)
    await recalcDayForPerson(personId, timestamp, input.direction)
    const daily = await db.dailyAttendance.findUnique({
      where: { personId_date: { personId, date: today } },
    })
    if (daily && daily.lateMinutes > (inRule.thresholdMin || 0) && (inRule.thresholdMin || 0) >= 0) {
      const mins = daily.lateMinutes
      await notifyRoles(
        [inRule.notifyRole],
        `تأخیر ${personName}`,
        `${personName} امروز با ${mins} دقیقه تأخیر وارد شد.`,
        'LATE'
      )
    }
  }
}

/** Daily absence sweep: called manually or by cron-like endpoint */
export async function runAbsenceSweep() {
  const today = tehranDayOf(new Date())
  const day = addDays(today, -1) // check yesterday (fully passed)
  const rules = await db.notificationRule.findMany({ where: { enabled: true, type: 'ABSENT' } })
  const absentRule = rules[0]
  if (!absentRule) return { checked: 0, absents: 0 }

  const persons = await db.personnel.findMany({ where: { status: 'ACTIVE' } })
  let absents = 0
  for (const p of persons) {
    const daily = await db.dailyAttendance.findUnique({
      where: { personId_date: { personId: p.id, date: day } },
    })
    if (!daily) continue
    if (daily.status === 'ABSENT') {
      absents++
      await notifyRoles(
        [absentRule.notifyRole],
        `غیبت ${p.firstName} ${p.lastName}`,
        `برای ${p.firstName} ${p.lastName} در تاریخ ${day.toISOString().slice(0, 10)} غیبت ثبت شد.`,
        'ABSENT'
      )
    }
  }
  await emitRealtime('absence-sweep', { absents, date: day.toISOString() })
  return { checked: persons.length, absents }
}

export async function onLeaveRequest(requestId: string) {
  const rules = await db.notificationRule.findMany({ where: { enabled: true, type: 'LEAVE_REQUEST' } })
  const rule = rules[0]
  const req = await db.leaveRequest.findUnique({ where: { id: requestId }, include: { person: true } })
  if (!req || !rule) return
  const fullName = `${req.person.firstName} ${req.person.lastName}`
  await notifyRoles(
    [rule.notifyRole],
    'درخواست مرخصی جدید',
    `${fullName} درخواست مرخصی ثبت کرد (${req.kind === 'DAILY' ? 'روزانه' : 'ساعتی'}).`,
    'LEAVE_REQUEST',
    requestId
  )
  await emitRealtime('leave-request', { requestId, fullName, kind: req.kind })
}

/**
 * v1.4.3 (addendum P4): notifications for newly imported device punches.
 *
 * Rules implemented:
 *   * only NEW rows are considered — selected by syncRunId (a re-sync that
 *     imports nothing new can never re-notify: dedupe by construction);
 *   * matched punches notify ONLY the punched person's own user account
 *     (EMPLOYEE never sees other people's punches in their notifications);
 *   * punches of personnel WITHOUT a linked user account notify nobody
 *     (the reports carry them);
 *   * UNMATCHED punches notify admins (ADMIN + SUPER_ADMIN) with ONE
 *     aggregated row per run — they are kept, visible and auto-rematch later;
 *   * the direction is deliberately NOT mentioned anywhere: it stays UNKNOWN
 *     until the vendor meaning of InOutModeRaw is confirmed;
 *   * per user per run: ONE aggregated notification (no 30-row spam when a
 *     poll delivers several punches).
 */
export async function createPunchNotifications(input: {
  runId: string
  deviceName: string
}): Promise<{ userNotifications: number; adminNotifications: number; usersNotified: number }> {
  const rows = await db.rawPunch.findMany({
    where: { syncRunId: input.runId, matchStatus: 'MATCHED' },
    select: { punchTime: true, personnelId: true, personnel: { select: { user: { select: { id: true } } } } },
  })
  const perUser = new Map<string, { count: number; lastAt: Date }>()
  for (const r of rows) {
    const uid = r.personnel?.user?.id
    if (!uid) continue // no linked user account — reports carry the data
    const cur = perUser.get(uid) ?? { count: 0, lastAt: r.punchTime }
    cur.count++
    if (r.punchTime > cur.lastAt) cur.lastAt = r.punchTime
    perUser.set(uid, cur)
  }
  const fmt = new Intl.DateTimeFormat('fa-IR', { hour: '2-digit', minute: '2-digit', timeZone: 'Asia/Tehran' })
  for (const [uid, agg] of perUser) {
    await notifyUser(
      uid,
      'تردد جدید ثبت شد',
      `${agg.count === 1 ? 'یک تردد جدید' : `${agg.count.toLocaleString('fa-IR')} تردد جدید`} شما از دستگاه «${input.deviceName}» دریافت شد — آخرین ساعت ${fmt.format(agg.lastAt)}.`,
      'PUNCH',
    )
  }
  const unmatched = await db.rawPunch.count({ where: { syncRunId: input.runId, matchStatus: 'UNMATCHED' } })
  let adminNotifications = 0
  if (unmatched > 0) {
    await notifyRoles(
      ['ADMIN', 'SUPER_ADMIN'],
      'تردد بدون تطبیق دریافت شد',
      `${unmatched.toLocaleString('fa-IR')} تردد بدون تطبیق کارمند از دستگاه «${input.deviceName}» دریافت و نگه‌داشته شد (حذف نمی‌شود) — با تعریف پرسنل مربوط، خودکار تطبیق می‌خورد.`,
      'UNMATCHED_PUNCH',
    )
    adminNotifications = 1
  }
  return { userNotifications: perUser.size, adminNotifications, usersNotified: perUser.size }
}
