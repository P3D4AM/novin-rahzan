/* Seed: Persian demo data for حضور+ attendance system */
import { PrismaClient } from '@prisma/client'
import { randomBytes, scryptSync } from 'crypto'
import { addDays, faWeekday, tehranDate, toJalali, jalaaliMonthLength, toGregorianObject } from '../src/lib/jalali'

const db = new PrismaClient()

function hashPassword(password: string): string {
  const salt = randomBytes(16).toString('hex')
  const hash = scryptSync(password, salt, 64).toString('hex')
  return `${salt}:${hash}`
}

function rand(min: number, max: number) {
  return Math.floor(Math.random() * (max - min + 1)) + min
}
function pick<T>(arr: T[]): T {
  return arr[rand(0, arr.length - 1)]
}
function round5(m: number) {
  return Math.round(m / 5) * 5
}

async function main() {
  console.log('🧹 clearing …')
  await db.punchRecord.deleteMany()
  await db.dailyAttendance.deleteMany()
  await db.leaveRequest.deleteMany()
  await db.overtimeRequest.deleteMany()
  await db.shiftAssignment.deleteMany()
  await db.notification.deleteMany()
  await db.notificationRule.deleteMany()
  await db.deviceLog.deleteMany()
  await db.device.deleteMany()
  await db.session.deleteMany()
  await db.user.deleteMany()
  await db.personnel.deleteMany()
  await db.shiftSegment.deleteMany()
  await db.shift.deleteMany()
  await db.shiftRotation.deleteMany()
  await db.leaveType.deleteMany()
  await db.holiday.deleteMany()
  await db.department.deleteMany()
  await db.jobTitle.deleteMany()
  await db.employmentType.deleteMany()
  await db.setting.deleteMany()
  await db.auditLog.deleteMany()

  console.log('🏢 org …')
  const departments = await Promise.all(
    ['مدیریت', 'مالی و حسابداری', 'فناوری اطلاعات', 'فروش و بازاریابی', 'منابع انسانی', 'تولید و عملیات'].map(
      (name, i) => db.department.create({ data: { name, description: ['دفتر مرکزی', 'امور مالی', 'زیرساخت و نرم‌افزار', 'توسعه بازار', 'امور پرسنلی', 'کارگاه تولید'][i] } })
    )
  )
  const jobTitles = await Promise.all(
    ['مدیرعامل', 'مدیر مالی', 'کارشناس حسابداری', 'برنامه‌نویس ارشد', 'کارشناس شبکه', 'کارشناس فروش', 'مدیر منابع انسانی', 'اپراتور تولید', 'کارمند اداری', 'کارشناس پشتیبانی'].map(
      (name) => db.jobTitle.create({ data: { name } })
    )
  )
  const employmentTypes = await Promise.all(
    ['رسمی', 'قراردادی', 'پیمانکاری', 'کارآموز'].map((name) => db.employmentType.create({ data: { name } }))
  )

  console.log('🕒 shifts …')
  const shiftMorning = await db.shift.create({
    data: {
      name: 'اداری صبح', type: 'FIXED', color: '#10b981',
      lateGraceMin: 10, earlyGraceMin: 10,
      segments: { create: [{ order: 1, startMin: 8 * 60, endMin: 16 * 60 + 30, breakMin: 60 }] },
    },
  })
  const shiftEvening = await db.shift.create({
    data: {
      name: 'اداری عصر', type: 'FIXED', color: '#f59e0b',
      lateGraceMin: 10, earlyGraceMin: 10,
      segments: { create: [{ order: 1, startMin: 15 * 60, endMin: 23 * 60 + 30, breakMin: 60 }] },
    },
  })
  const shiftNight = await db.shift.create({
    data: {
      name: 'امنیت شب', type: 'FIXED', color: '#8b5cf6',
      lateGraceMin: 15, earlyGraceMin: 15,
      segments: { create: [{ order: 1, startMin: 22 * 60, endMin: 6 * 60, breakMin: 45 }] },
    },
  })
  const shiftSplit = await db.shift.create({
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

  console.log('🌴 leave types & holidays …')
  const leaveTypes = await Promise.all([
    db.leaveType.create({ data: { name: 'استحقاقی', paid: true, color: '#10b981', countsTowardsBalance: true } }),
    db.leaveType.create({ data: { name: 'استعلاجی', paid: true, color: '#ef4444', countsTowardsBalance: false } }),
    db.leaveType.create({ data: { name: 'بدون حقوق', paid: false, color: '#6b7280', countsTowardsBalance: false } }),
    db.leaveType.create({ data: { name: 'مأموریت', paid: true, color: '#3b82f6', countsTowardsBalance: false } }),
  ])

  const today = new Date()
  const { jy, jm } = toJalali(today)
  const monthLen = jalaaliMonthLength(jy, jm)
  for (const d of [3, 12, 22]) {
    if (d <= monthLen && d >= 1) {
      const g = toGregorianObject(jy, jm, d)
      await db.holiday.create({ data: { date: new Date(Date.UTC(g.gy, g.gm - 1, g.gd)), title: 'تعطیل رسمی نمونه', type: 'OFFICIAL' } })
    }
  }

  console.log('👥 personnel …')
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

  const persons = []
  for (let i = 0; i < peopleSpec.length; i++) {
    const [fn, ln, deptIdx, jobIdx, devUid, shiftCode] = peopleSpec[i]
    const p = await db.personnel.create({
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
    persons.push(p)
  }

  console.log('👤 users …')
  const usersSpec: Array<[string, string, string, number]> = [
    ['admin', 'admin123', 'SUPER_ADMIN', 0],
    ['hr', 'hr123', 'ADMIN', 1],
    ['manager', 'manager123', 'APPROVER', 9],
    ['ali', 'ali123', 'EMPLOYEE', 2],
  ]
  for (const [username, pass, role, personIdx] of usersSpec) {
    const p = persons[personIdx]
    await db.user.create({
      data: {
        username, passwordHash: hashPassword(pass), role,
        fullName: `${p.firstName} ${p.lastName}`,
        personnelId: p.id,
      },
    })
  }

  console.log('📱 device …')
  const device = await db.device.create({
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

  console.log('🔔 notification rules …')
  await db.notificationRule.createMany({
    data: [
      { type: 'LATE', enabled: true, thresholdMin: 15, notifyRole: 'ADMIN' },
      { type: 'ABSENT', enabled: true, notifyRole: 'ADMIN' },
      { type: 'CHECKIN', enabled: false, notifyRole: 'ADMIN' },
      { type: 'LEAVE_REQUEST', enabled: true, notifyRole: 'APPROVER' },
    ],
  })

  console.log('⚙️ settings …')
  await db.setting.createMany({
    data: [
      { id: 'org_name', value: 'شرکت نمونه پارس' },
      { id: 'ot_rate_normal', value: '1.4' },
      { id: 'ot_rate_friday', value: '1.6' },
      { id: 'ot_rate_holiday', value: '2.0' },
      { id: 'sms_provider', value: 'kavenegar' },
      { id: 'sms_api_key', value: '' },
      { id: 'sms_sender', value: '' },
    ],
  })

  console.log('📆 generating punches for last 30 days …')
  const shiftsDef: Record<string, { start: number; end: number; cross: boolean }> = {
    MORNING: { start: 8 * 60, end: 16 * 60 + 30, cross: false },
    EVENING: { start: 15 * 60, end: 23 * 60 + 30, cross: false },
    NIGHT: { start: 22 * 60, end: 6 * 60, cross: true },
    SPLIT: { start: 8 * 60, end: 20 * 60, cross: false },
  }

  let punchCount = 0
  for (let back = 30; back >= 0; back--) {
    const day = addDays(new Date(Date.UTC(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate())), -back)
    const dow = faWeekday(day)
    for (const [idx, p] of persons.entries()) {
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

      await db.punchRecord.create({
        data: { personId: p.id, timestamp: mk(round5(inMin)), direction: 'IN', source: 'DEVICE', deviceId: device.id, deviceUserId: peopleSpec[idx][4] },
      })
      const outTs = def.cross ? mk(round5(outMin), 1) : mk(round5(outMin))
      await db.punchRecord.create({
        data: { personId: p.id, timestamp: outTs, direction: 'OUT', source: 'DEVICE', deviceId: device.id, deviceUserId: peopleSpec[idx][4] },
      })
      punchCount += 2
    }
  }

  console.log('🌴 leave requests …')
  const mkDT = (n: number, h = 8, m = 0) => {
    const d = addDays(today, n)
    return tehranDate(d.getUTCFullYear(), d.getUTCMonth() + 1, d.getUTCDate(), h, m)
  }
  await db.leaveRequest.createMany({
    data: [
      { personId: persons[3].id, leaveTypeId: leaveTypes[0].id, kind: 'DAILY', startDateTime: mkDT(2), endDateTime: mkDT(3, 16), durationMinutes: 16 * 60, reason: 'امور شخصی', status: 'PENDING' },
      { personId: persons[7].id, leaveTypeId: leaveTypes[1].id, kind: 'DAILY', startDateTime: mkDT(-5), endDateTime: mkDT(-4, 16), durationMinutes: 16 * 60, reason: 'بیماری - گواهی پزشک', status: 'APPROVED', decidedAt: mkDT(-6) },
      { personId: persons[10].id, leaveTypeId: leaveTypes[0].id, kind: 'HOURLY', startDateTime: mkDT(1, 10), endDateTime: mkDT(1, 13), durationMinutes: 180, reason: 'مراجعه به بانک', status: 'PENDING' },
      { personId: persons[5].id, leaveTypeId: leaveTypes[3].id, kind: 'DAILY', startDateTime: mkDT(-10), endDateTime: mkDT(-9, 16), durationMinutes: 16 * 60, reason: 'مأموریت اداری مشهد', status: 'APPROVED', decidedAt: mkDT(-12) },
      { personId: persons[9].id, leaveTypeId: leaveTypes[0].id, kind: 'DAILY', startDateTime: mkDT(5), endDateTime: mkDT(7, 16), durationMinutes: 48 * 60, reason: 'سفر خانوادگی', status: 'REJECTED', decisionNote: 'به دلیل جلسه مالی ماه', decidedAt: mkDT(-1) },
    ],
  })

  console.log('⏱️ overtime requests …')
  await db.overtimeRequest.createMany({
    data: [
      { personId: persons[2].id, date: addDays(today, -2), minutes: 120, reason: 'استقرار نسخه جدید', status: 'APPROVED' },
      { personId: persons[4].id, date: addDays(today, -1), minutes: 90, reason: 'نگهبانی رویداد', status: 'PENDING' },
      { personId: persons[6].id, date: addDays(today, -6), minutes: 60, reason: 'پشتیبانی مشتری', status: 'APPROVED' },
    ],
  })

  console.log(`✅ seed done: ${persons.length} persons, ${punchCount} punches`)
}

main()
  .catch((e) => {
    console.error(e)
    process.exit(1)
  })
  .finally(() => db.$disconnect())
