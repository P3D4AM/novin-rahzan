import { NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { getSessionUser } from '@/lib/auth'
import { fail } from '@/lib/api'
import { jalaliOfInstant, jalaliMonthRange } from '@/lib/time'
import { sanitizeDeviceForApi } from '@/lib/device-secrets'

export async function GET() {
  try {
    const user = await getSessionUser()
    if (!user) return NextResponse.json({ error: 'unauthorized' }, { status: 401 })
    // v1.6.0 (phase 1 stage 3): Tehran "today" via the central tz-aware module —
    // independent of the server machine timezone (was legacy toJalali → server-local getters)
    const now = new Date()
    const { jy, jm, jd } = jalaliOfInstant(now)
    const mr = jalaliMonthRange(jy, jm)
    const [departments, jobTitles, employmentTypes, shifts, rotations, leaveTypes, devices, settings, users, holidays, rules] = await Promise.all([
      db.department.findMany({ orderBy: { name: 'asc' } }),
      db.jobTitle.findMany({ orderBy: { name: 'asc' } }),
      db.employmentType.findMany({ orderBy: { name: 'asc' } }),
      db.shift.findMany({ where: { isActive: true }, include: { segments: { orderBy: { order: 'asc' } } }, orderBy: { name: 'asc' } }),
      db.shiftRotation.findMany({ where: { isActive: true } }),
      db.leaveType.findMany({ orderBy: { name: 'asc' } }),
      db.device.findMany({ orderBy: { name: 'asc' } }),
      db.setting.findMany(),
      db.user.findMany({ select: { id: true, username: true, role: true, fullName: true, active: true, personnelId: true }, orderBy: { username: 'asc' } }),
      db.holiday.findMany({ where: { date: { gte: mr.startMarker, lte: mr.endMarker } }, orderBy: { date: 'asc' } }),
      db.notificationRule.findMany(),
    ])
    const settingsMap: Record<string, string> = {}
    for (const s of settings) settingsMap[s.id] = s.value
    return NextResponse.json({
      departments, jobTitles, employmentTypes, shifts, rotations, leaveTypes,
      // v1.3.0: device rows are sanitized — the network password never leaves the process
      devices: devices.map(sanitizeDeviceForApi), users, rules,
      holidays, settings: settingsMap,
      today: { jy, jm, jd },
    })
  } catch (err) {
    return fail(err)
  }
}
