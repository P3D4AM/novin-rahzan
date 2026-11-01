import { db } from '@/lib/db'
import { addDays, faWeekday, startOfUTCDay, tehranDayOf, tehranMinutes } from '@/lib/jalali'

export interface ShiftSegmentLite {
  startMin: number
  endMin: number
  breakMin: number
}

export interface ShiftLite {
  id: string
  name: string
  lateGraceMin: number
  earlyGraceMin: number
  overtimeEligible: boolean
  segments: ShiftSegmentLite[]
}

export function shiftStandardMinutes(shift: ShiftLite | null): number {
  if (!shift || !shift.segments.length) return 8 * 60
  return shift.segments.reduce((acc, s) => {
    const len = s.endMin > s.startMin ? s.endMin - s.startMin : s.endMin + 1440 - s.startMin
    return acc + len - s.breakMin
  }, 0)
}

export function shiftWindow(shift: ShiftLite | null): { start: number; end: number } | null {
  if (!shift || !shift.segments.length) return null
  const starts = shift.segments.map((s) => s.startMin)
  const ends = shift.segments.map((s) => (s.endMin > s.startMin ? s.endMin : s.endMin + 1440))
  return { start: Math.min(...starts), end: Math.max(...ends) }
}

export interface DailyComputation {
  personId: string
  date: Date
  status: string
  shiftId: string | null
  firstIn: Date | null
  lastOut: Date | null
  workedMinutes: number
  lateMinutes: number
  earlyLeaveMinutes: number
  overtimeMinutes: number
  leaveTypeId: string | null
  leaveMinutes: number
  note: string | null
}

interface LeaveSpan {
  leaveTypeId: string
  minutesCovered: number
}

/** Gather approved leave minutes covering the given day */
async function leaveForDay(personId: string, day: Date): Promise<LeaveSpan | null> {
  const dayStart = day
  const dayEnd = addDays(day, 1)
  const reqs = await db.leaveRequest.findMany({
    where: {
      personId,
      status: 'APPROVED',
      startDateTime: { lt: dayEnd },
      endDateTime: { gt: dayStart },
    },
    include: { leaveType: true },
  })
  if (!reqs.length) return null
  let minutes = 0
  let typeId: string | null = null
  for (const r of reqs) {
    const from = r.startDateTime > dayStart ? r.startDateTime : dayStart
    const to = r.endDateTime < dayEnd ? r.endDateTime : dayEnd
    minutes += Math.round((to.getTime() - from.getTime()) / 60000)
    typeId = r.leaveTypeId
  }
  return { leaveTypeId: typeId!, minutesCovered: Math.min(minutes, 1440) }
}

function pairPunches(punches: { timestamp: Date }[]): { firstIn: Date | null; lastOut: Date | null; worked: number } {
  if (!punches.length) return { firstIn: null, lastOut: null, worked: 0 }
  const sorted = [...punches].sort((a, b) => a.timestamp.getTime() - b.timestamp.getTime())
  const firstIn = sorted[0].timestamp
  const lastOut = sorted.length > 1 ? sorted[sorted.length - 1].timestamp : null
  let worked = 0
  for (let i = 0; i + 1 < sorted.length; i += 2) {
    worked += Math.round((sorted[i + 1].timestamp.getTime() - sorted[i].timestamp.getTime()) / 60000)
  }
  return { firstIn, lastOut, worked }
}

/** Compute daily attendance for one person on one day */
export function computeDaily(input: {
  personId: string
  day: Date
  shift: ShiftLite | null
  punches: { timestamp: Date }[]
  leave: LeaveSpan | null
  isHoliday: boolean
  isOff: boolean // explicit off assignment or weekend without shift
  now: Date
}): DailyComputation {
  const { personId, day, shift, punches, leave, isHoliday, isOff, now } = input
  const dayEnd = addDays(day, 1)
  const { firstIn, lastOut, worked } = pairPunches(punches)

  const win = shiftWindow(shift)
  const grace = shift?.lateGraceMin ?? 10
  const earlyGrace = shift?.earlyGraceMin ?? 10

  let lateMinutes = 0
  let earlyLeaveMinutes = 0
  if (shift && win && firstIn) {
    const inMin = tehranMinutes(firstIn)
    // shift may start before midnight of this day
    const dayStartMin = win.start
    const effectiveIn = inMin < win.end && inMin < dayStartMin && win.start > 720 ? inMin + 1440 : inMin
    lateMinutes = Math.max(0, effectiveIn - dayStartMin - grace)
    if (lastOut) {
      const outMin = tehranMinutes(lastOut)
      const effectiveOut = outMin < win.start && win.end > 1440 ? outMin + 1440 : outMin
      earlyLeaveMinutes = Math.max(0, win.end - effectiveOut - earlyGrace)
    }
  }

  const standard = shiftStandardMinutes(shift)
  let overtimeMinutes = 0
  if (isHoliday || isOff) {
    overtimeMinutes = worked // all work on off days counts as OT
  } else if (shift?.overtimeEligible && worked > standard + 30) {
    overtimeMinutes = worked - standard
  }

  const leaveMinutes = leave?.minutesCovered ?? 0
  const fullDayLeave = leaveMinutes >= 8 * 60

  let status: string
  if (fullDayLeave && !punches.length) {
    status = 'LEAVE'
  } else if (isHoliday && !punches.length) {
    status = 'HOLIDAY'
  } else if (isOff && !punches.length && !shift) {
    status = 'OFF'
  } else if (!punches.length) {
    // no punches: absent if the day window has fully passed
    status = dayEnd.getTime() - 6 * 3600000 <= now.getTime() ? 'ABSENT' : 'PENDING'
  } else if (punches.length === 1) {
    status = 'INCOMPLETE'
  } else {
    status = 'PRESENT'
  }

  return {
    personId,
    date: day,
    status,
    shiftId: shift?.id ?? null,
    firstIn,
    lastOut,
    workedMinutes: worked,
    lateMinutes,
    earlyLeaveMinutes,
    overtimeMinutes,
    leaveTypeId: leave?.leaveTypeId ?? null,
    leaveMinutes,
    note: null,
  }
}

/** Recalculate & persist daily attendance for a set of persons over a date range */
export async function recalcRange(personIds: string[], start: Date, end: Date): Promise<number> {
  if (!personIds.length) return 0
  const dayStart = startOfUTCDay(start)
  const dayEnd = startOfUTCDay(end)
  const now = new Date()

  const persons = await db.personnel.findMany({
    where: { id: { in: personIds } },
    include: { defaultShift: { include: { segments: true } } },
  })

  const shifts = await db.shift.findMany({ where: { isActive: true }, include: { segments: true } })
  const shiftMap = new Map<string, ShiftLite>(
    shifts.map((s) => [
      s.id,
      {
        id: s.id,
        name: s.name,
        lateGraceMin: s.lateGraceMin,
        earlyGraceMin: s.earlyGraceMin,
        overtimeEligible: s.overtimeEligible,
        segments: s.segments
          .sort((a, b) => a.order - b.order)
          .map((seg) => ({ startMin: seg.startMin, endMin: seg.endMin, breakMin: seg.breakMin })),
      },
    ])
  )

  const holidays = await db.holiday.findMany({ where: { date: { gte: dayStart, lte: dayEnd } } })
  const holidayKeys = new Set(holidays.map((h) => h.date.toISOString().slice(0, 10)))

  const assignments = await db.shiftAssignment.findMany({
    where: { personId: { in: personIds }, date: { gte: dayStart, lte: dayEnd } },
  })
  const assignMap = new Map<string, { shiftId: string | null }>()
  for (const a of assignments) assignMap.set(`${a.personId}|${a.date.toISOString().slice(0, 10)}`, { shiftId: a.shiftId })

  let count = 0
  for (const p of persons) {
    for (let d = new Date(dayStart); d <= dayEnd; d = addDays(d, 1)) {
      const key = d.toISOString().slice(0, 10)
      const assign = assignMap.get(`${p.id}|${key}`)
      const isFriday = faWeekday(d) === 6
      const isHoliday = holidayKeys.has(key)

      let shift: ShiftLite | null = null
      let isOff = false
      if (assign) {
        if (assign.shiftId) shift = shiftMap.get(assign.shiftId) ?? null
        else isOff = true // explicit off day
      } else if (p.defaultShiftId) {
        if (isHoliday || isFriday) {
          isOff = true
        } else {
          shift = p.defaultShiftId ? shiftMap.get(p.defaultShiftId) ?? null : null
        }
      } else {
        // no default shift: weekend off; otherwise compute from punches only
        isOff = isFriday || isHoliday
      }

      const punches = await db.punchRecord.findMany({
        where: {
          personId: p.id,
          timestamp: { gte: d, lt: addDays(d, 1) },
        },
        select: { timestamp: true },
      })
      const leave = await leaveForDay(p.id, d)

      const result = computeDaily({
        personId: p.id,
        day: d,
        shift,
        punches,
        leave,
        isHoliday,
        isOff: isOff && !shift,
        now,
      })

      await db.dailyAttendance.upsert({
        where: { personId_date: { personId: p.id, date: d } },
        create: result,
        update: {
          status: result.status,
          shiftId: result.shiftId,
          firstIn: result.firstIn,
          lastOut: result.lastOut,
          workedMinutes: result.workedMinutes,
          lateMinutes: result.lateMinutes,
          earlyLeaveMinutes: result.earlyLeaveMinutes,
          overtimeMinutes: result.overtimeMinutes,
          leaveTypeId: result.leaveTypeId,
          leaveMinutes: result.leaveMinutes,
        },
      })
      count++
    }
  }
  return count
}

/** recalc a single day after a punch event (fire-and-forget safe) */
export async function recalcDayForPerson(personId: string, ts: Date) {
  const day = tehranDayOf(ts)
  return recalcRange([personId], day, day)
}
