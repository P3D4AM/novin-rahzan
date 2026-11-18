/**
 * Phase 2 Stage S3 — central workday attribution & coverage interval math.
 * =====================================================================
 * PURE module: no database, no clock, no randomness. Everything here is a
 * deterministic function of its inputs.
 *
 * workday_policy = shift (docs/TIME-POLICIES.md — approved, binding):
 *   The business workday of a punch is the START DATE of the shift that was
 *   actually in effect and whose window contains the punch — NOT necessarily
 *   the Tehran calendar day of the punch instant.
 *     Shift Sat 22:00 → Sun 06:00 ⇒ a Sun 02:30 punch belongs to SATURDAY.
 *
 * The applicable shift for a candidate start day C is supplied by the CALLER
 * through `shiftForDay(C)` — the engine passes an assignment-history-aware
 * resolver (ShiftAssignment(C) → its shift, else the person's defaultShift).
 * Today's shift is therefore NEVER silently applied to old records: each
 * candidate day contributes only the shift that day itself carries.
 *
 * Candidate start days: the punch's Tehran calendar day and the two days
 * before it (same defensive span as time.ts resolveWorkday — a legal
 * ShiftSegment window is < 48h). Among matching windows the LATEST start wins
 * (the documented AMBIGUOUS_LATEST tie-break of resolveWorkday).
 *
 * Fallback (explicit, never guessed): no applicable shift on any candidate
 * day, malformed shift, or punch outside every shift window ⇒ the Tehran
 * CALENDAR day of the punch instant (dayMarkerOf), with a machine reason.
 *
 * Coverage windows (mission/leave interval attribution — same policy):
 *   For a workday W the coverage window is [Tehran midnight of W, max(next
 *   Tehran midnight, shift window end)). A punch/mission minute inside the
 *   shift window belongs to W even after midnight; a minute outside every
 *   shift window belongs to its Tehran calendar day. Walking days
 *   chronologically and subtracting already-claimed time yields a PARTITION:
 *   every instant belongs to exactly one workday, so no leave/mission minute
 *   is ever credited to two rows (no double counting).
 */

import { addDaysMarker, dayMarkerOf, markerKey, shiftWindowInstants, tehranDayWindowOfMarker, type DayMarker, type ShiftCtx, type WorkdayReason } from './time'

/** marker → applicable shift (assignment-aware; supplied by the engine). */
export type ShiftForDay = (day: Date) => ShiftCtx | null

export interface WorkdayAttribution {
  /** business workday (UTC-midnight marker) the instant belongs to */
  workday: DayMarker
  /** true when attributed via a shift window (policy) rather than the fallback */
  viaShift: boolean
  reason: WorkdayReason
  /** the matching shift window when viaShift (auditable provenance) */
  window?: { startInstant: Date; endInstant: Date }
}

/**
 * Attribute one attendance instant to its business workday.
 * Deterministic; depends only on (instant, shiftForDay).
 */
export function attributePunchWorkday(instant: Date, shiftForDay: ShiftForDay): WorkdayAttribution {
  const punchDay = dayMarkerOf(instant)
  let sawShift = false
  let sawInvalid = false
  let sawValidWindow = false
  let best: { marker: DayMarker; startInstant: Date; endInstant: Date } | null = null
  let matches = 0

  // candidate start days, oldest first; LATEST matching start wins below
  for (let back = 2; back >= 0; back--) {
    const day = addDaysMarker(punchDay, -back)
    const shift = shiftForDay(day)
    if (!shift) continue
    sawShift = true
    const win = shiftWindowInstants(day, shift)
    if (!win) {
      sawInvalid = true
      continue
    }
    sawValidWindow = true
    if (instant.getTime() >= win.startInstant.getTime() && instant.getTime() < win.endInstant.getTime()) {
      matches++
      if (!best || win.startInstant.getTime() > best.startInstant.getTime()) {
        best = { marker: day, startInstant: win.startInstant, endInstant: win.endInstant }
      }
    }
  }

  if (!best) {
    // no shift at all → NO_SHIFT; only malformed shifts seen → INVALID_SHIFT;
    // at least one valid window that simply does not contain the punch → OUT_OF_WINDOW
    const reason: WorkdayReason = !sawShift ? 'NO_SHIFT' : !sawValidWindow && sawInvalid ? 'INVALID_SHIFT' : 'OUT_OF_WINDOW'
    return { workday: punchDay, viaShift: false, reason }
  }
  return {
    workday: best.marker,
    viaShift: true,
    reason: matches > 1 ? 'AMBIGUOUS_LATEST' : 'SHIFT_WINDOW',
    window: { startInstant: best.startInstant, endInstant: best.endInstant },
  }
}

// ---------------------------------------------------------------------------
// Interval math (milliseconds, half-open [start, end)) — pure helpers for
// mission/leave/attendance overlap credit. Sums are rounded to whole minutes.
// ---------------------------------------------------------------------------

export interface Interval {
  start: number
  end: number
}

const MIN = 60000

/** Sorted, disjoint union of the given intervals (empty/invalid ignored). */
export function mergeIntervals(list: Interval[]): Interval[] {
  const valid = list.filter((i) => Number.isFinite(i.start) && Number.isFinite(i.end) && i.end > i.start)
  if (!valid.length) return []
  valid.sort((a, b) => a.start - b.start || a.end - b.end)
  const out: Interval[] = [{ ...valid[0] }]
  for (const cur of valid.slice(1)) {
    const last = out[out.length - 1]
    if (cur.start <= last.end) last.end = Math.max(last.end, cur.end)
    else out.push({ ...cur })
  }
  return out
}

/** a − b (set difference) for disjoint sorted interval lists. */
export function subtractIntervals(a: Interval[], b: Interval[]): Interval[] {
  const A = mergeIntervals(a)
  const B = mergeIntervals(b)
  const out: Interval[] = []
  let bi = 0
  for (const iv of A) {
    let start = iv.start
    const end = iv.end
    // advance past B-intervals that end before this A-interval starts
    while (bi < B.length && B[bi].end <= start) bi++
    let j = bi
    let cur = start
    while (j < B.length && B[j].start < end) {
      if (B[j].start > cur) out.push({ start: cur, end: Math.min(B[j].start, end) })
      cur = Math.max(cur, B[j].end)
      if (cur >= end) break
      j++
    }
    if (cur < end) out.push({ start: cur, end })
  }
  return out
}

/** a ∩ b (set intersection) for disjoint sorted interval lists. */
export function intersectIntervals(a: Interval[], b: Interval[]): Interval[] {
  const A = mergeIntervals(a)
  const B = mergeIntervals(b)
  const out: Interval[] = []
  let i = 0
  let j = 0
  while (i < A.length && j < B.length) {
    const start = Math.max(A[i].start, B[j].start)
    const end = Math.min(A[i].end, B[j].end)
    if (start < end) out.push({ start, end })
    if (A[i].end <= B[j].end) i++
    else j++
  }
  return out
}

/** Total covered minutes (rounded per-interval, matching punch-minute math). */
export function totalMinutes(list: Interval[]): number {
  return list.reduce((acc, iv) => acc + Math.round((iv.end - iv.start) / MIN), 0)
}

const ATTRIBUTION_REASON_ORDER: WorkdayReason[] = [
  'AMBIGUOUS_LATEST',
  'OUT_OF_WINDOW',
  'INVALID_SHIFT',
  'NO_SHIFT',
]

/**
 * Deterministic ASCII summary of fallback/ambiguous attribution reasons for
 * one day's punches, e.g. `OUT_OF_WINDOW:1|NO_SHIFT:2` ('' when every punch
 * came from a plain shift window). Composed into DailyAttendance.note AFTER
 * the pairing codes so the stored note reads `PAIRING…|ATTRIBUTION…`.
 */
export function attributionSummary(attributions: WorkdayAttribution[]): string {
  if (!attributions.length) return ''
  const counts = new Map<WorkdayReason, number>()
  for (const a of attributions) {
    if (a.reason === 'SHIFT_WINDOW') continue // the normal case is not an anomaly
    counts.set(a.reason, (counts.get(a.reason) ?? 0) + 1)
  }
  const parts: string[] = []
  for (const reason of ATTRIBUTION_REASON_ORDER) {
    const c = counts.get(reason)
    if (c) parts.push(`${reason}:${c}`)
  }
  return parts.join('|')
}

/**
 * The RAW coverage window of a workday marker W (before neighbor-claim
 * subtraction): [Tehran midnight of W, max(next Tehran midnight, shift window
 * end)). Day shifts collapse to exactly the Tehran day; night shifts extend
 * into the next morning. Always non-empty and contiguous.
 */
export function rawCoverageWindow(day: DayMarker, shiftForDay: ShiftForDay): Interval {
  const dayWin = tehranDayWindowOfMarker(day)
  let endMs = dayWin.end.getTime()
  const shift = shiftForDay(day)
  const win = shift ? shiftWindowInstants(day, shift) : null
  if (win) endMs = Math.max(endMs, win.endInstant.getTime())
  return { start: dayWin.start.getTime(), end: endMs }
}

/**
 * Effective coverage windows for a range of workday markers: the raw windows
 * walked chronologically, each minus the time already claimed by EARLIER
 * days. The result PARTITIONS [Tehran midnight of the first day, …]: every
 * instant belongs to exactly one marker's coverage — one leave/mission minute
 * is credited to exactly one row, regardless of which subrange is later
 * recalculated (the claim timeline is a pure function of the day sequence).
 * Markers before `start` (down to −2 days, the attribution span) participate
 * in claiming so night-shift tails are never double-claimed by the following
 * day, but only markers within [start..end] get an entry in the result.
 */
export function coverageWindowsByDay(
  start: DayMarker,
  end: DayMarker,
  shiftForDay: ShiftForDay,
): Map<string, Interval[]> {
  const out = new Map<string, Interval[]>()
  let claimed: Interval[] = []
  for (let d = addDaysMarker(start, -2); d <= end; d = addDaysMarker(d, 1)) {
    const raw = rawCoverageWindow(d, shiftForDay)
    if (d >= start) {
      const eff = subtractIntervals([raw], claimed)
      out.set(markerKey(d), eff)
    }
    claimed = mergeIntervals([...claimed, raw])
  }
  return out
}
