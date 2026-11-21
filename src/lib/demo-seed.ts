/**
 * Phase 1 (v1.8.0-dev) — demo data seeding, refactored out of scripts/seed.ts.
 * =====================================================================
 * One implementation shared by the installer CLI (scripts/db-admin.ts seed),
 * the settings UI (POST /api/db-admin) and the legacy `npm run seed`.
 *
 * PRESERVE-ON-CONFLICT semantics (Phase 1 spec: "Seed must never unintention-
 * ally overwrite passwords, device connection settings, secrets or
 * installation-specific settings"):
 *   * Setting      — only MISSING keys are created; existing values (org name,
 *                    SMS API key, thresholds, …) keep the install's values.
 *   * Device       — the demo device is created ONLY when the device table is
 *                    empty; existing devices (connection settings, network
 *                    passwords, DPAPI blobs) are never touched.
 *   * User         — demo users are created ONLY when their username is free;
 *                    existing accounts keep their CURRENT passwords (a re-seed
 *                    never resets credentials).
 *   * everything else is freshly created (the caller wipes business data in
 *     the same transaction — see dbDemoSeed).
 *
 * The seed's "today" is the TEHRAN day via the central time module (S5-A2).
 */
import { addDays, faWeekday, tehranDate, jalaaliMonthLength, toGregorianObject } from './jalali'
import { tehranTodayMarker, jalaliOfMarker } from './time'
import { hashPassword } from './auth-hash'
import type { DbAdminClient } from './db-admin'

function rand(min: number, max: number) {
  return Math.floor(Math.random() * (max - min + 1)) + min
}
function pick<T>(arr: T[]): T {
  return arr[rand(0, arr.length - 1)]
}
function round5(m: number) {
  return Math.round(m / 5) * 5
}

export interface SeedResult {
  persons: number
  punches: number
  usersCreated: string[]
  deviceCreated: boolean
  settingsCreated: string[]
  settingsPreserved: string[]
}

export async function seedDemoData(tx: DbAdminClient): Promise<SeedResult> {
  const usersCreated: string[] = []
  const settingsCreated: string[] = []
  const settingsPreserved: string[] = []

  /* org structure ---------------------------------------------------- */
  const departments = await Promise.all(
    ['مدیریت', 'مالی و حسابداری', 'فناوری اطلاعات', 'فروش و بازاریابی', 'منابع انسانی', 'تولید و عملیات'].map(
      (name, i) => tx.department.create({ data: { name, description: ['دفتر مرکزی', 'امور مالی', 'زیرساخت و نرم‌افزار', 'توسعه بازار', 'امور پرسنلی', 'کارگاه تولید'][i] } })
    )
  )
  const jobTitles = await Promise.all(
    ['مدیرعامل', 'مدیر مالی', 'کارشناس حسابداری', 'برنامه‌نویس ارشد', 'کارشناس شبکه', 'کارشناس فروش', 'مدیر منابع انسانی', 'اپراتور تولید', 'کارمند اداری', 'کارشناس پشتیبانی'].map(
      (name) => tx.jobTitle.create({ data: { name } })
    )
  )
  const employmentTypes = await Promise.all(
    ['رسمی', 'قراردادی', 'پیمانکاری', 'کارآموز'].map((name) => tx.employmentType.create({ data: { name } }))
  )

  /* shifts ----------------------------------------------------------- */
  const shiftMorning = await tx.shift.create({
    data: {
      name: 'اداری صبح', type: 'FIXED', color: '#10b981',
      lateGraceMin: 10, earlyGraceMin: 10,
      segments: { create: [{ order: 1, startMin: 8 * 60, endMin: 16 * 60 + 30, breakMin: 60 }] },
    },
  })
  const shiftEvening = await tx.shift.create({
    data: {
      name: 'اداری عصر', type: 'FIXED', color: '#f59e0b',
      lateGraceMin: 10, earlyGraceMin: 10,
      segments: { create: [{ order: 1, startMin: 15 * 60, endMin: 23 * 60 + 30, breakMin: 60 }] },
    },
  })
  const shiftNight = await tx.shift.create({
    data: {
      name: 'امنیت شب', type: 'FIXED', color: '#8b5cf6',
      lateGraceMin: 15, earlyGraceMin: 15,
      segments: { create: [{ order: 1, startMin: 22 * 60, endMin: 6 * 60, breakMin: 45 }] },
    },
  })
  const shiftSplit = await tx.shift.create({
    data: {
      name: 'اسپلیت دو بخشی', type: 'SPLIT', color: '#06b6d4',
      lateGraceMin: 10, earlyGraceMin: 10,
      segments: {
        create: [
          { order: 1, startMin: 8 * 60, endMin: 12 * 60, breakMin: 0 },
          { order: 2, startMin: 16 * 60, endMin: 20 * 60, breakMin: 0 },
        ],
      },
    },
  })

  /* leave types & holidays ------------------------------------------- */
  const leaveTypes = await Promise.all([
    tx.leaveType.create({ data: { name: 'استحقاقی', paid: true, color: '#10b981', countsTowardsBalance: true } }),
    tx.leaveType.create({ data: { name: 'استعلاجی', paid: true, color: '#ef4444', countsTowardsBalance: false } }),
    tx.leaveType.create({ data: { name: 'بدون حقوق', paid: false, color: '#6b7280', countsTowardsBalance: false } }),
    tx.leaveType.create({ data: { name: 'مأموریت', paid: true, color: '#3b82f6', countsTowardsBalance: false, isMission: true } }),
  ])

  const today = tehranTodayMarker()
  const { jy, jm } = jalaliOfMarker(today)
  const monthLen = jalaaliMonthLength(jy, jm)
  for (const d of [3, 12, 22]) {
    if (d <= monthLen && d >= 1) {
      const g = toGregorianObject(jy, jm, d)
      await tx.holiday.create({ data: { date: new Date(Date.UTC(g.gy, g.gm - 1, g.gd)), title: 'تعطیل رسمی نمونه', type: 'OFFICIAL' } })
    }
  }

  /* personnel -------------------------------------------------------- */
  const peopleSpec: Array<[string, string, number, number, number, string]> = [
    ['محمدرضا', 'احمدی', 0, 0, 1001, 'MORNING'],
    ['فاطمه', 'کریمی', 4, 6, 1002, 'MORNING'],
    ['علی', 'رضایی', 2, 3, 1003, 'MORNING'],
    ['زهرا', 'موسوی', 2, 4, 1004, 'EVENING'],
    ['حسین', 'محمدی', 5, 7, 1005, 'NIGHT'],
    ['مریم', 'حسینی', 1, 2, 1006, 'MORNING'],
    ['امیر', 'تهرانی', 3, 5, 1007, 'MORNING'],
    ['سارا', 'قاسمی', 3, 9, 1008, 'SPLIT'],
    ['مهدی', 'نوری', 5, 7, 1009, 'NIGHT'],
    ['نگار', 'شریفی', 1, 1, 1010, 'MORNING'],
    ['رضا', 'عباسی', 2, 3, 1011, 'MORNING'],
    ['الهام', 'رستمی', 4, 9, 1012, 'EVENING'],
  ]
  const shiftByCode: Record<string, string> = {
    MORNING: shiftMorning.id, EVENING: shiftEvening.id, NIGHT: shiftNight.id, SPLIT: shiftSplit.id,
  }

  const persons: Array<{ id: string }> = []
  for (let i = 0; i < peopleSpec.length; i++) {
    const [fn, ln, deptIdx, jobIdx, devUid, shiftCode] = peopleSpec[i]
    const p = await tx.personnel.create({
      data: {
        personnelCode: String(2011 + i),
        firstName: fn, lastName: ln,
        nationalId: String(1234567890 + i * 137),
        fatherName: pick(['حسن', 'اکبر', 'محمد', 'حسین', 'اسدالله']),
        gender: ['زهرا', 'فاطمه', 'مریم', 'سارا', 'نگار', 'الهام'].includes(fn) ? 'FEMALE' : 'MALE',
        birthDate: `13${rand(65, 75)}/${String(rand(1, 12)).padStart(2, '0')}/${String(rand(1, 28)).padStart(2, '0')}`,
        phone: `0912${String(1000000 + i * 35713).slice(0, 7)}`,
        email: `user${i + 1}@hozorplus.ir`,
        departmentId: departments[deptIdx].id,
        jobTitleId: jobTitles[jobIdx].id,
        employmentTypeId: employmentTypes[rand(0, 1)].id,
        hireDate: `13${rand(90, 99)}/${String(rand(1, 12)).padStart(2, '0')}/${String(rand(1, 28)).padStart(2, '0')}`,
        status: 'ACTIVE',
        pin: String(1000 + i * 111).slice(0, 4),
        deviceUserId: devUid,
        defaultShiftId: shiftByCode[shiftCode],
      },
    })
    persons.push({ id: p.id })
  }

  /* users — created only for FREE usernames -------------------------- */
  const usersSpec: Array<[string, string, string, number]> = [
    ['admin', 'admin123', 'SUPER_ADMIN', 0],
    ['hr', 'hr123', 'ADMIN', 1],
    ['manager', 'manager123', 'APPROVER', 9],
    ['ali', 'ali123', 'EMPLOYEE', 2],
  ]
  for (const [username, pass, role, personIdx] of usersSpec) {
    const taken = await tx.user.findUnique({ where: { username }, select: { id: true } })
    if (taken) continue // existing account: keep its CURRENT password
    const p = persons[personIdx]
    await tx.user.create({
      data: { username, passwordHash: hashPassword(pass), role, fullName: `${peopleSpec[personIdx][0]} ${peopleSpec[personIdx][1]}`, personnelId: p.id },
    })
    usersCreated.push(username)
  }

  /* device — only when the install has none -------------------------- */
  const deviceCount = await tx.device.count()
  let deviceCreated = false
  let deviceId: string
  if (deviceCount === 0) {
    const device = await tx.device.create({
      data: {
        name: 'دستگاه ورودی اصلی',
        type: 'ZKTECO',
        ipAddress: '192.168.1.201',
        port: 4370,
        protocol: 'TCP',
        location: 'ورودی ساختمان - طبقه همکف',
        serialNumber: '7561202505176',
        enabled: true,
        status: 'UNKNOWN',
      },
    })
    deviceId = device.id
    deviceCreated = true
  } else {
    const existing = await tx.device.findFirst({ orderBy: { createdAt: 'asc' }, select: { id: true } })
    deviceId = existing!.id
  }

  /* notification rules — only when none exist ------------------------ */
  const rulesCount = await tx.notificationRule.count()
  if (rulesCount === 0) {
    await tx.notificationRule.createMany({
      data: [
        { type: 'LATE', enabled: true, thresholdMin: 15, notifyRole: 'ADMIN' },
        { type: 'ABSENT', enabled: true, notifyRole: 'ADMIN' },
        { type: 'CHECKIN', enabled: false, notifyRole: 'ADMIN' },
        { type: 'LEAVE_REQUEST', enabled: true, notifyRole: 'APPROVER' },
      ],
    })
  }

  /* settings — only MISSING keys are created -------------------------- */
  const settingsSpec: Array<[string, string]> = [
    ['org_name', 'شرکت نمونه پارس'],
    ['ot_rate_normal', '1.4'],
    ['ot_rate_friday', '1.6'],
    ['ot_rate_holiday', '2.0'],
    // Phase 2 S3 (min_excess): overtime activation threshold in minutes —
    // gates activation, never subtracted; validated integer ≥ 0, default 30.
    ['ot_threshold_minutes', '30'],
    ['sms_provider', 'kavenegar'],
    ['sms_api_key', ''],
    ['sms_sender', ''],
  ]
  for (const [id, value] of settingsSpec) {
    const existing = await tx.setting.findUnique({ where: { id }, select: { id: true } })
    if (existing) { settingsPreserved.push(id); continue } // the install's value stays
    await tx.setting.create({ data: { id, value } })
    settingsCreated.push(id)
  }

  /* demo punches for the last 30 days --------------------------------- */
  const shiftsDef: Record<string, { start: number; end: number; cross: boolean }> = {
    MORNING: { start: 8 * 60, end: 16 * 60 + 30, cross: false },
    EVENING: { start: 15 * 60, end: 23 * 60 + 30, cross: false },
    NIGHT: { start: 22 * 60, end: 6 * 60, cross: true },
    SPLIT: { start: 8 * 60, end: 20 * 60, cross: false },
  }

  let punchCount = 0
  for (let back = 30; back >= 0; back--) {
    // today is already the UTC-midnight marker of the Tehran day — S5
    const day = addDays(today, -back)
    const dow = faWeekday(day)
    for (const [idx] of persons.entries()) {
      const shiftCode = peopleSpec[idx][5]
      const def = shiftsDef[shiftCode]
      const absentChance = dow === 6 ? 1 : 0.07
      if (Math.random() < absentChance) continue
      const y = day.getUTCFullYear(), mo = day.getUTCMonth() + 1, d = day.getUTCDate()

      const lateOffset = Math.random() < 0.25 ? rand(1, 35) : -rand(1, 20)
      const inMin = def.start + lateOffset
      const otExtra = Math.random() < 0.3 ? rand(15, 90) : 0
      const outMin = def.end + otExtra - (Math.random() < 0.15 ? rand(5, 40) : 0)

      const mk = (minutes: number, dayOffset = 0) =>
        tehranDate(y, mo, d + dayOffset, Math.floor(minutes / 60), minutes % 60)

      await tx.punchRecord.create({
        data: { personId: persons[idx].id, timestamp: mk(round5(inMin)), direction: 'IN', source: 'DEVICE', deviceId, deviceUserId: peopleSpec[idx][4] },
      })
      const outTs = def.cross ? mk(round5(outMin), 1) : mk(round5(outMin))
      await tx.punchRecord.create({
        data: { personId: persons[idx].id, timestamp: outTs, direction: 'OUT', source: 'DEVICE', deviceId, deviceUserId: peopleSpec[idx][4] },
      })
      punchCount += 2
    }
  }

  /* leave & overtime requests ---------------------------------------- */
  const mkDT = (n: number, h = 8, m = 0) => {
    const d = addDays(today, n)
    return tehranDate(d.getUTCFullYear(), d.getUTCMonth() + 1, d.getUTCDate(), h, m)
  }
  await tx.leaveRequest.createMany({
    data: [
      { personId: persons[3].id, leaveTypeId: leaveTypes[0].id, kind: 'DAILY', startDateTime: mkDT(2), endDateTime: mkDT(3, 16), durationMinutes: 16 * 60, reason: 'امور شخصی', status: 'PENDING' },
      { personId: persons[7].id, leaveTypeId: leaveTypes[1].id, kind: 'DAILY', startDateTime: mkDT(-5), endDateTime: mkDT(-4, 16), durationMinutes: 16 * 60, reason: 'بیماری - گواهی پزشک', status: 'APPROVED', decidedAt: mkDT(-6) },
      { personId: persons[10].id, leaveTypeId: leaveTypes[0].id, kind: 'HOURLY', startDateTime: mkDT(1, 10), endDateTime: mkDT(1, 13), durationMinutes: 180, reason: 'مراجعه به بانک', status: 'PENDING' },
      { personId: persons[5].id, leaveTypeId: leaveTypes[3].id, kind: 'DAILY', startDateTime: mkDT(-10), endDateTime: mkDT(-9, 16), durationMinutes: 16 * 60, reason: 'مأموریت اداری مشهد', status: 'APPROVED', decidedAt: mkDT(-12) },
      { personId: persons[9].id, leaveTypeId: leaveTypes[0].id, kind: 'DAILY', startDateTime: mkDT(5), endDateTime: mkDT(7, 16), durationMinutes: 48 * 60, reason: 'سفر خانوادگی', status: 'REJECTED', decisionNote: 'به دلیل جلسه مالی ماه', decidedAt: mkDT(-1) },
    ],
  })
  await tx.overtimeRequest.createMany({
    data: [
      { personId: persons[2].id, date: addDays(today, -2), minutes: 120, reason: 'استقرار نسخه جدید', status: 'APPROVED' },
      { personId: persons[4].id, date: addDays(today, -1), minutes: 90, reason: 'نگهبانی رویداد', status: 'PENDING' },
      { personId: persons[6].id, date: addDays(today, -6), minutes: 60, reason: 'پشتیبانی مشتری', status: 'APPROVED' },
    ],
  })

  return { persons: persons.length, punches: punchCount, usersCreated, deviceCreated, settingsCreated, settingsPreserved }
}

/** Demo users reference (for docs/tests): the fixed demo credentials. */
export const DEMO_USERS = [
  { username: 'admin', password: 'admin123', role: 'SUPER_ADMIN' },
  { username: 'hr', password: 'hr123', role: 'ADMIN' },
  { username: 'manager', password: 'manager123', role: 'APPROVER' },
  { username: 'ali', password: 'ali123', role: 'EMPLOYEE' },
] as const
