import { db } from '@/lib/db'
import { anomalySummary, pairPunches, type PairingPunchInput } from '@/lib/pairing'
import {
  addDaysMarker,
  dayMarkerOf,
  markerKey,
  tehranDayWindowOfMarker,
  type ShiftCtx,
} from '@/lib/time'
import {
  attributePunchWorkday,
  attributionSummary,
  coverageWindowsByDay,
  intersectIntervals,
  mergeIntervals,
  subtractIntervals,
  totalMinutes,
  type Interval,
  type WorkdayAttribution,
} from '@/lib/workday'
import { addDays, faWeekday, startOfUTCDay, tehranMinutes } from '@/lib/jalali'

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
  /** Phase 2 S3 (min_excess): payable OT = min(actual, approved request minutes). */
  payableOvertimeMinutes: number
  leaveTypeId: string | null
  leaveMinutes: number
  /** Phase 2 S3 (approved_credit): approved mission time credited as valid work time. */
  missionMinutes: number
  note: string | null
}

/** A raw approved leave/mission interval (true UTC instants). */
export interface LeaveSpanInput {
  start: Date
  end: Date
  leaveTypeId: string
}

export const DEFAULT_OT_THRESHOLD_MINUTES = 30
const OT_THRESHOLD_SETTING_ID = 'ot_threshold_minutes'

/**
 * Parse the `ot_threshold_minutes` Setting value. Integer & non-negative per
 * the approved decision; anything missing/malformed/negative falls back to the
 * documented default 30 — an invalid value never silently enters calculations
 * as a wrong threshold.
 */
export function parseOtThresholdMinutes(value: string | null | undefined): number {
  const n = Number(value)
  return Number.isInteger(n) && n >= 0 ? n : DEFAULT_OT_THRESHOLD_MINUTES
}

async function readOtThresholdMinutes(): Promise<number> {
  const row = await db.setting.findUnique({ where: { id: OT_THRESHOLD_SETTING_ID } })
  return parseOtThresholdMinutes(row?.value)
}

/** Interval helpers on Date spans (half-open, instants true UTC). */
function spanToInterval(s: { start: Date; end: Date }): Interval | null {
  const a = s.start.getTime()
  const b = s.end.getTime()
  return b > a ? { start: a, end: b } : null
}

/**
 * Compute daily attendance for one person on one workday.
 * PURE — no DB, no clock reads, deterministic on its inputs.
 *
 * Phase 2 S3 policies wired in here:
 *  - workday attribution itself happens in recalcRange (src/lib/workday.ts);
 *    this function receives the punches ALREADY attributed to this workday
 *    and the workday's effective coverage intervals.
 *  - overtime_policy = min_excess: `overtimeMinutes` keeps its meaning (actual
 *    excess over standard, gated by the configurable threshold) and the NEW
 *    `payableOvertimeMinutes` = min(actual, approved OvertimeRequest minutes).
 *  - mission_policy = approved_credit: approved mission spans count as valid
 *    work time AFTER subtracting actual attendance overlap (never double);
 *    mission time is excluded from leaveMinutes; workedMinutes = attendance
 *    pairs + credited mission time (the policy's final worked value).
 */
export function computeDaily(input: {
  personId: string
  day: Date
  shift: ShiftLite | null
  punches: PairingPunchInput[]
  /** approved mission intervals (isMission LeaveType, APPROVED only) */
  missionSpans?: Array<{ start: Date; end: Date }>
  /** approved NORMAL leave intervals (mission excluded by the caller) */
  leaveSpans?: LeaveSpanInput[]
  /** effective coverage window of this workday (ms; default: its Tehran day) */
  coverageIntervals?: Interval[]
  /** summed minutes of APPROVED OvertimeRequests for this workday */
  approvedOvertimeMinutes?: number
  /** validated OT threshold (Setting ot_threshold_minutes; default 30) */
  otThresholdMinutes?: number
  isHoliday: boolean
  isOff: boolean // explicit off assignment or weekend without shift
  now: Date
}): DailyComputation {
  const {
    personId,
    day,
    shift,
    punches,
    missionSpans = [],
    leaveSpans = [],
    coverageIntervals,
    approvedOvertimeMinutes = 0,
    otThresholdMinutes = DEFAULT_OT_THRESHOLD_MINUTES,
    isHoliday,
    isOff,
    now,
  } = input
  const dayEnd = addDays(day, 1)
  // Phase 2 S2: direction-aware deterministic pairing (src/lib/pairing.ts).
  // workedMinutes counts ONLY direction-valid closed pairs; duplicate/orphan/
  // simultaneous/open punches are reported as anomalies in `note`, never
  // silently absorbed and never fabricated into pairs.
  const pairing = pairPunches(punches)
  const firstIn = pairing.firstIn
  const lastOut = pairing.lastOut
  const attendanceWorked = pairing.workedMinutes

  // coverage window of this workday (default: its Tehran calendar day)
  const dayWin = tehranDayWindowOfMarker(day)
  const coverage: Interval[] = coverageIntervals ?? [
    { start: dayWin.start.getTime(), end: dayWin.end.getTime() },
  ]

  // ---- mission credit (approved_credit): valid work time, no double count ----
  const missionUnion = mergeIntervals(
    missionSpans.map(spanToInterval).filter((x): x is Interval => x !== null),
  )
  const attendanceUnion = mergeIntervals(
    pairing.pairs.map((p) => ({ start: p.inAt.getTime(), end: p.outAt.getTime() })),
  )
  // mission minus actual attendance, clipped to this workday's coverage
  const missionCredit = intersectIntervals(subtractIntervals(missionUnion, attendanceUnion), coverage)
  const missionMinutes = totalMinutes(missionCredit)

  // ---- normal leave coverage: Tehran-day window, mission time excluded ----
  const leaveUnionRaw = mergeIntervals(
    leaveSpans.map(spanToInterval).filter((x): x is Interval => x !== null),
  )
  const leaveUnion = subtractIntervals(leaveUnionRaw, missionUnion) // a minute is never both leave and mission
  const leaveCovered = intersectIntervals(leaveUnion, coverage)
  const leaveMinutes = Math.min(totalMinutes(leaveCovered), 1440)
  // deterministic leaveTypeId: the latest-ending span that actually contributes
  let leaveTypeId: string | null = null
  const sortedSpans = [...leaveSpans].sort((a, b) => a.end.getTime() - b.end.getTime() || a.start.getTime() - b.start.getTime())
  for (const span of sortedSpans) {
    const iv = spanToInterval(span)
    if (iv && intersectIntervals([iv], coverage).length) leaveTypeId = span.leaveTypeId
  }

  // ---- final worked: valid attendance + credited mission time (policy) ----
  const worked = attendanceWorked + missionMinutes

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

  // ---- overtime (min_excess) ----
  // The threshold GATES activation (it is never subtracted from the excess);
  // equality stays zero, one minute above it makes the WHOLE excess actual OT.
  const standard = shiftStandardMinutes(shift)
  let overtimeMinutes = 0
  if (isHoliday || isOff) {
    overtimeMinutes = worked // all valid work time on off days counts as actual OT
  } else if (shift?.overtimeEligible && worked > standard + otThresholdMinutes) {
    overtimeMinutes = worked - standard
  }
  // payable = min(actual, approved); PENDING/REJECTED contribute nothing and
  // payable never exceeds actual. Excess above approval stays visible in
  // overtimeMinutes; paying it requires explicit approval, never automatic.
  const payableOvertimeMinutes = Math.min(overtimeMinutes, Math.max(0, approvedOvertimeMinutes))

  const fullDayLeave = leaveMinutes >= 8 * 60

  let status: string
  if (fullDayLeave && !punches.length) {
    status = 'LEAVE'
  } else if (missionMinutes > 0 && !punches.length) {
    // approved mission with valid credited time and no punches → MISSION.
    // (Punch-based statuses keep precedence whenever punches exist: incomplete
    // punches are never hidden by a mission.)
    status = 'MISSION'
  } else if (isHoliday && !punches.length) {
    status = 'HOLIDAY'
  } else if (isOff && !punches.length && !shift) {
    status = 'OFF'
  } else if (!punches.length) {
    // no punches: absent if the day window has fully passed
    status = dayEnd.getTime() - 6 * 3600000 <= now.getTime() ? 'ABSENT' : 'PENDING'
  } else if (punches.length === 1) {
    status = 'INCOMPLETE'
  } else if (pairing.pairs.length > 0) {
    // At least one direction-valid closed pair — presence is proven.
    // (Malformed days with punches but zero valid pairs fall through to
    // INCOMPLETE below instead of being fabricated as PRESENT.)
    status = 'PRESENT'
  } else {
    status = 'INCOMPLETE'
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
    payableOvertimeMinutes,
    leaveTypeId,
    leaveMinutes,
    missionMinutes,
    note: pairing.anomalies.length ? anomalySummary(pairing.anomalies) : null,
  }
}

/** Load active shifts as engine Lite objects (segments ordered). */
async function loadActiveShiftMap(): Promise<Map<string, ShiftLite>> {
  const shifts = await db.shift.findMany({ where: { isActive: true }, include: { segments: true } })
  return new Map<string, ShiftLite>(
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
    ]),
  )
}

/**
 * Assignment-history-aware resolver: the shift applicable ON a given day —
 * ShiftAssignment for that day (explicit OFF → null), else the person's
 * defaultShift. Today's shift is never applied to other days. Used for
 * ATTRIBUTION (no holiday/Friday suppression: attribution is data-driven;
 * off-ness is a per-row calculation flag, not an attribution input).
 */
function buildShiftResolver(
  personId: string,
  defaultShiftId: string | null,
  assignMap: Map<string, { shiftId: string | null }>,
  shiftMap: Map<string, ShiftLite>,
): (day: Date) => ShiftCtx | null {
  return (day: Date) => {
    const assign = assignMap.get(`${personId}|${markerKey(day)}`)
    const shiftId = assign ? assign.shiftId : defaultShiftId
    if (!shiftId) return null
    const shift = shiftMap.get(shiftId)
    if (!shift) return null
    return { segments: shift.segments }
  }
}

/**
 * Recalculate & persist daily attendance for a set of persons over a date
 * range of WORKDAYS (day markers).
 *
 * Phase 2 S3 (workday_policy = shift): every punch is attributed to the START
 * DAY of the shift whose window contains it (assignment-history aware, via
 * src/lib/workday.ts) — NOT to the UTC-midnight window of the calendar day.
 * Night-shift punches after midnight land on the shift-start workday; a punch
 * is computed in exactly one workday (never lost, never duplicated). Leave and
 * mission coverage uses the workday's coverage window partition (also never
 * credited to two days). Fallback punches (no/malformed shift, out of window)
 * keep the Tehran calendar day and are surfaced in `note` with reason codes.
 */
export async function recalcRange(personIds: string[], start: Date, end: Date): Promise<number> {
  if (!personIds.length) return 0
  const dayStart = startOfUTCDay(start)
  const dayEnd = startOfUTCDay(end)
  const now = new Date()

  const persons = await db.personnel.findMany({
    where: { id: { in: personIds } },
    include: { defaultShift: { include: { segments: true } } },
  })

  const shiftMap = await loadActiveShiftMap()

  const holidays = await db.holiday.findMany({ where: { date: { gte: dayStart, lte: dayEnd } } })
  const holidayKeys = new Set(holidays.map((h) => h.date.toISOString().slice(0, 10)))

  // attribution candidate span: punches of Tehran days [start .. end+1] can
  // only start from assignments of [start-2 .. end+1]
  const assignments = await db.shiftAssignment.findMany({
    where: { personId: { in: personIds }, date: { gte: addDaysMarker(dayStart, -2), lte: addDaysMarker(dayEnd, 1) } },
  })
  const assignMap = new Map<string, { shiftId: string | null }>()
  for (const a of assignments) assignMap.set(`${a.personId}|${a.date.toISOString().slice(0, 10)}`, { shiftId: a.shiftId })

  // overtime threshold (Setting ot_threshold_minutes; default 30)
  const otThresholdMinutes = await readOtThresholdMinutes()

  // approved overtime minutes per (person, workday) — PENDING/REJECTED excluded
  const otReqs = await db.overtimeRequest.findMany({
    where: { personId: { in: personIds }, status: 'APPROVED', date: { gte: dayStart, lte: dayEnd } },
    select: { personId: true, date: true, minutes: true },
  })
  const otApproved = new Map<string, number>()
  for (const o of otReqs) {
    const k = `${o.personId}|${markerKey(o.date)}`
    otApproved.set(k, (otApproved.get(k) ?? 0) + o.minutes)
  }

  // approved leave/mission spans overlapping the coverage span (one query)
  const coverageSpanStart = tehranDayWindowOfMarker(dayStart).start
  const coverageSpanEnd = tehranDayWindowOfMarker(addDaysMarker(dayEnd, 2)).start
  const leaveReqs = await db.leaveRequest.findMany({
    where: {
      personId: { in: personIds },
      status: 'APPROVED',
      startDateTime: { lt: coverageSpanEnd },
      endDateTime: { gt: coverageSpanStart },
    },
    include: { leaveType: { select: { isMission: true } } },
  })
  const leavesByPerson = new Map<string, typeof leaveReqs>()
  for (const r of leaveReqs) {
    const list = leavesByPerson.get(r.personId) ?? []
    list.push(r)
    leavesByPerson.set(r.personId, list)
  }

  // punch fetch window: any punch whose WORKDAY is in [start..end] lies inside
  // [Tehran midnight of start, Tehran midnight of end+2)
  const punchFrom = tehranDayWindowOfMarker(dayStart).start
  const punchTo = tehranDayWindowOfMarker(addDaysMarker(dayEnd, 2)).start

  let count = 0
  for (const p of persons) {
    const shiftOf = buildShiftResolver(p.id, p.defaultShiftId, assignMap, shiftMap)

    // effective coverage partition over the range (claim-subtracted)
    const coverage = coverageWindowsByDay(dayStart, dayEnd, shiftOf)

    // fetch the person's punches once, attribute each to its workday
    const punches = await db.punchRecord.findMany({
      where: { personId: p.id, timestamp: { gte: punchFrom, lt: punchTo } },
      select: { timestamp: true, direction: true, source: true },
    })
    const byWorkday = new Map<string, { punches: PairingPunchInput[]; attributions: WorkdayAttribution[] }>()
    for (const pu of punches) {
      const attr = attributePunchWorkday(pu.timestamp, shiftOf)
      const k = markerKey(attr.workday)
      const bucket = byWorkday.get(k) ?? { punches: [], attributions: [] }
      bucket.punches.push(pu)
      bucket.attributions.push(attr)
      byWorkday.set(k, bucket)
    }

    const personLeaves = leavesByPerson.get(p.id) ?? []
    const missionSpans = personLeaves
      .filter((r) => r.leaveType.isMission)
      .map((r) => ({ start: r.startDateTime, end: r.endDateTime }))
    const leaveSpans = personLeaves
      .filter((r) => !r.leaveType.isMission)
      .map((r) => ({ start: r.startDateTime, end: r.endDateTime, leaveTypeId: r.leaveTypeId }))

    for (let d = new Date(dayStart); d <= dayEnd; d = addDays(d, 1)) {
      const key = markerKey(d)
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

      const bucket = byWorkday.get(key)
      const dayPunches = bucket?.punches ?? []
      const attributions = bucket?.attributions ?? []

      const result = computeDaily({
        personId: p.id,
        day: d,
        shift,
        punches: dayPunches,
        missionSpans,
        leaveSpans,
        coverageIntervals: coverage.get(key) ?? [],
        approvedOvertimeMinutes: otApproved.get(`${p.id}|${key}`) ?? 0,
        otThresholdMinutes,
        isHoliday,
        isOff: isOff && !shift,
        now,
      })

      // note = pairing anomalies | attribution fallback/ambiguity reasons
      const attrNote = attributionSummary(attributions)
      const note = [result.note, attrNote].filter(Boolean).join('|') || null

      await db.dailyAttendance.upsert({
        where: { personId_date: { personId: p.id, date: d } },
        create: { ...result, note },
        update: {
          status: result.status,
          shiftId: result.shiftId,
          firstIn: result.firstIn,
          lastOut: result.lastOut,
          workedMinutes: result.workedMinutes,
          lateMinutes: result.lateMinutes,
          earlyLeaveMinutes: result.earlyLeaveMinutes,
          overtimeMinutes: result.overtimeMinutes,
          payableOvertimeMinutes: result.payableOvertimeMinutes,
          leaveTypeId: result.leaveTypeId,
          leaveMinutes: result.leaveMinutes,
          missionMinutes: result.missionMinutes,
          note,
        },
      })
      count++
    }
  }
  return count
}

/**
 * Recalculate the WORKDAYS actually affected by an attendance event at `ts`
 * (fire-and-forget safe).
 *
 * Phase 2 S3: the affected workdays are the attribution of the triggering
 * instant ITSELF plus the workdays of every punch in that Tehran calendar
 * day — so a night-shift punch after midnight recomputes the SHIFT-START day,
 * and a moved/deleted punch recomputes the row that previously contained it.
 */
export async function recalcDayForPerson(personId: string, ts: Date) {
  const person = await db.personnel.findUnique({
    where: { id: personId },
    include: { defaultShift: { include: { segments: true } } },
  })
  if (!person) return 0

  const day = dayMarkerOf(ts)
  const assigns = await db.shiftAssignment.findMany({
    where: { personId, date: { gte: addDaysMarker(day, -2), lte: day } },
  })
  const assignMap = new Map<string, { shiftId: string | null }>()
  for (const a of assigns) assignMap.set(`${a.personId}|${a.date.toISOString().slice(0, 10)}`, { shiftId: a.shiftId })

  const shiftMap = await loadActiveShiftMap()
  const shiftOf = buildShiftResolver(personId, person.defaultShiftId, assignMap, shiftMap)

  const workdays = new Set<string>() // markerKey strings — UTC-midnight markers
  workdays.add(markerKey(attributePunchWorkday(ts, shiftOf).workday))
  const dayWin = tehranDayWindowOfMarker(day)
  const dayPunches = await db.punchRecord.findMany({
    where: { personId, timestamp: { gte: dayWin.start, lt: dayWin.end } },
    select: { timestamp: true },
  })
  for (const pu of dayPunches) {
    workdays.add(markerKey(attributePunchWorkday(pu.timestamp, shiftOf).workday))
  }

  const markers = [...workdays].sort()
  const first = new Date(`${markers[0]}T00:00:00.000Z`)
  const last = new Date(`${markers[markers.length - 1]}T00:00:00.000Z`)
  return recalcRange([personId], first, last)
}
