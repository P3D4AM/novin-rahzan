/**
 * HozorPlus central date/time layer — Phase 1 (v1.6.0).
 * =====================================================================
 * THE single authoritative module for:
 *   - Tehran-local civil date/time parts (timezone-aware via the Intl
 *     timezone database for `Asia/Tehran` — the fixed +03:30 offset is a
 *     LAST-RESORT fallback only, never the primary implementation)
 *   - Tehran calendar-day boundaries (true Tehran midnight instants)
 *   - day markers: the UTC-midnight representation of a Tehran calendar
 *     day used by DailyAttendance.date / ShiftAssignment.date /
 *     Holiday.date / OvertimeRequest.date (storage convention preserved)
 *   - Gregorian ⇆ Jalali conversion (numeric-only API of jalaali-js —
 *     its Date-based overload uses environment-local getters and is
 *     never used here)
 *   - Jalali input validation with Persian error messages
 *   - shift-aware workday resolution implementing the approved
 *     `workday_policy = shift`
 *
 * Isomorphic: runs on the server AND in the browser (Intl only).
 *
 * COMPATIBILITY CONTRACT (protected subsystems):
 *   - `src/lib/jalali.ts` (legacy, fixed-offset) keeps serving the
 *     attendance engine and the device raw-punch pipeline — those are
 *     out of refactor scope in this phase and their imports stay
 *     untouched. `scripts/test-time.ts` proves byte-equal behavior for
 *     the post-2022 no-DST era.
 *   - `sbxpc.ts:tehranWallClockToUTC` / `fkattend.ts:fkWallClockToUTC`
 *     remain the device-side authorities; this module reimplements the
 *     same Intl algorithm for the application side, and the test suite
 *     asserts equality of the two on a wide timestamp sample.
 */

import {
  toJalaali as g2jNum,
  toGregorian as j2gNum,
  isValidJalaaliDate,
  jalaaliMonthLength,
} from 'jalaali-js'

export { isValidJalaaliDate, jalaaliMonthLength }

export const TEHRAN_TZ = 'Asia/Tehran'
/** last-resort fallback offset (Iran has had no DST since 2022) — used only when the Intl tz database is unavailable */
export const TEHRAN_FALLBACK_OFFSET_MIN = 210

// ---------------------------------------------------------------------------
// Tehran civil parts (timezone-aware core)
// ---------------------------------------------------------------------------

export interface TehranParts {
  y: number
  m: number // 1..12
  d: number // 1..31
  hh: number // 0..23
  mi: number // 0..59
  ss: number // 0..59
}

let cachedFormatter: Intl.DateTimeFormat | null | undefined

function tehranFormatter(): Intl.DateTimeFormat | null {
  if (cachedFormatter !== undefined) return cachedFormatter
  try {
    cachedFormatter = new Intl.DateTimeFormat('en-US', {
      timeZone: TEHRAN_TZ,
      hour12: false,
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
    })
  } catch {
    cachedFormatter = null // no tz database — fixed-offset fallback below
  }
  return cachedFormatter
}

function partsFromFallbackShift(instant: Date): TehranParts {
  const t = new Date(instant.getTime() + TEHRAN_FALLBACK_OFFSET_MIN * 60000)
  return {
    y: t.getUTCFullYear(),
    m: t.getUTCMonth() + 1,
    d: t.getUTCDate(),
    hh: t.getUTCHours(),
    mi: t.getUTCMinutes(),
    ss: t.getUTCSeconds(),
  }
}

/** Tehran civil date/time of a UTC instant (tz database — never the machine clock). */
export function tehranParts(instant: Date): TehranParts {
  const f = tehranFormatter()
  if (!f) return partsFromFallbackShift(instant)
  const parts = f.formatToParts(instant)
  const get = (t: string) => Number(parts.find((p) => p.type === t)?.value ?? 0)
  return {
    y: get('year'),
    m: get('month'),
    d: get('day'),
    hh: get('hour') % 24, // hourCycle 'h24' artifact guard
    mi: get('minute'),
    ss: get('second'),
  }
}

/** Tehran timezone offset (minutes east of UTC) at the given instant — from the tz database. */
export function tehranOffsetMin(instant: Date): number {
  const p = tehranParts(instant)
  const wallAsUtc = Date.UTC(p.y, p.m - 1, p.d, p.hh, p.mi, p.ss)
  return Math.round((wallAsUtc - instant.getTime()) / 60000)
}

// ---------------------------------------------------------------------------
// Tehran wall clock → UTC instant (DST-correct; same algorithm as the
// device-side converters, kept in sync by tests)
// ---------------------------------------------------------------------------

function wallClockInstantFallback(wall: number): Date {
  return new Date(wall - TEHRAN_FALLBACK_OFFSET_MIN * 60000)
}

/** Build the UTC instant of a Tehran civil date/time (y, m, d, hh, mi, ss). */
export function tehranInstant(y: number, m: number, d: number, hh = 0, mi = 0, ss = 0): Date {
  if (!tehranFormatter()) return wallClockInstantFallback(Date.UTC(y, m - 1, d, hh, mi, ss))
  const wall = Date.UTC(y, m - 1, d, hh, mi, ss)
  let ts = wall - TEHRAN_FALLBACK_OFFSET_MIN * 60000 // first guess
  for (let i = 0; i < 3; i++) {
    const offset = tehranOffsetMin(new Date(ts))
    const next = wall - offset * 60000
    if (next === ts) break
    ts = next
  }
  return new Date(ts)
}

/** Convert a Tehran wall-clock instant (expressed as "as-if-UTC" milliseconds) to the true UTC instant. Mirrors sbxpc.ts:tehranWallClockToUTC. */
export function tehranWallClockToInstant(msSinceEpochWallAsUtc: number): Date {
  if (!tehranFormatter()) return wallClockInstantFallback(msSinceEpochWallAsUtc)
  return tehranInstant(
    new Date(msSinceEpochWallAsUtc).getUTCFullYear(),
    new Date(msSinceEpochWallAsUtc).getUTCMonth() + 1,
    new Date(msSinceEpochWallAsUtc).getUTCDate(),
    new Date(msSinceEpochWallAsUtc).getUTCHours(),
    new Date(msSinceEpochWallAsUtc).getUTCMinutes(),
    new Date(msSinceEpochWallAsUtc).getUTCSeconds(),
  )
}

/** Parse a "YYYY-MM-DD HH:MM[:SS]" Tehran wall-clock string → UTC instant (null when invalid). Mirrors fkattend.ts:fkWallClockToUTC semantics; lenient on 1–2 digit fields and '/' separators for CSV imports. Gregorian year must be 1900–2199 — Jalali-year strings like "1405/08/26 02:30" are rejected (never silently reinterpreted). */
export function tehranWallClockStringToInstant(s: string): Date | null {
  const m = /^((?:1[9]|2[01])\d{2})[-/](\d{1,2})[-/](\d{1,2}) (\d{1,2}):(\d{1,2})(?::(\d{1,2}))?$/.exec(String(s ?? '').trim())
  if (!m) return null
  const d = tehranInstant(
    Number(m[1]),
    Number(m[2]),
    Number(m[3]),
    Number(m[4]),
    Number(m[5]),
    m[6] ? Number(m[6]) : 0,
  )
  return Number.isFinite(d.getTime()) ? d : null
}

// ---------------------------------------------------------------------------
// Day markers — the storage convention (UTC midnight = one Tehran day)
// ---------------------------------------------------------------------------

/** A UTC-midnight Date representing one Tehran calendar day (storage convention of DailyAttendance/ShiftAssignment/Holiday/OvertimeRequest). */
export type DayMarker = Date

/** Marker of the Tehran calendar day containing `instant`. */
export function dayMarkerOf(instant: Date): DayMarker {
  const p = tehranParts(instant)
  return new Date(Date.UTC(p.y, p.m - 1, p.d))
}

/** Marker for an explicit Tehran calendar date (no timezone shift ever — date-only semantics). */
export function markerOfTehranDate(y: number, m: number, d: number): DayMarker {
  return new Date(Date.UTC(y, m - 1, d))
}

/** Marker of the current Tehran day — independent of the server machine timezone. */
export function tehranTodayMarker(): DayMarker {
  return dayMarkerOf(new Date())
}

export function addDaysMarker(marker: DayMarker, n: number): DayMarker {
  return new Date(marker.getTime() + n * 86400000)
}

/** "YYYY-MM-DD" key of a marker. */
export function markerKey(marker: DayMarker): string {
  return marker.toISOString().slice(0, 10)
}

/**
 * Parse a date-only "YYYY-MM-DD" string into a marker — NO timezone conversion,
 * NO accidental UTC shift of date-only values (the API date-only contract).
 * Accepts zero-padded and one/two-digit month/day (e.g. "2026-11-7").
 */
export function parseDateOnly(s: string): DayMarker | null {
  if (!s) return null
  const m = /^(\d{4})[-/](\d{1,2})[-/](\d{1,2})$/.exec(s.trim())
  if (!m) return null
  const y = Number(m[1])
  const mo = Number(m[2])
  const d = Number(m[3])
  if (mo < 1 || mo > 12 || d < 1 || d > 31) return null
  const marker = markerOfTehranDate(y, mo, d)
  if (
    marker.getUTCFullYear() !== y ||
    marker.getUTCMonth() + 1 !== mo ||
    marker.getUTCDate() !== d
  ) {
    return null // e.g. 2026-02-31
  }
  return marker
}

// ---------------------------------------------------------------------------
// API day-parameter parsing (single, tested contract for all routes)
// ---------------------------------------------------------------------------

/**
 * Parse an API "day" parameter into a marker. Accepts:
 *   - "YYYY-MM-DD" (date-only — parsed as UTC midnight per the ES spec, NO tz shift)
 *   - a full ISO instant (floored to its UTC day — the legacy startOfUTCDay behavior)
 * Returns null for absent/invalid input (routes decide their own 400 handling).
 */
export function parseDayParam(s: string | null | undefined): DayMarker | null {
  if (!s) return null
  const direct = parseDateOnly(String(s))
  if (direct) return direct
  const d = new Date(String(s))
  if (!Number.isFinite(d.getTime())) return null
  return new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate()))
}

/**
 * Instant window for a [from, to] marker range as consumed by punch-instant
 * filters: [from-marker, (to+1)-marker) — UTC-midnight semantics, consistent
 * with the attendance engine's day-attribution convention (see
 * docs/API-DATE-CONTRACTS.md for why Tehran-midnight windows are deferred to
 * the engine phase).
 */
export function markerRangeInstantWindow(fromMarker: DayMarker, toMarker: DayMarker): {
  gte: Date
  lt: Date
} {
  return { gte: fromMarker, lt: addDaysMarker(toMarker, 1) }
}

// ---------------------------------------------------------------------------
// Tehran day windows (true Tehran midnight boundaries)
// ---------------------------------------------------------------------------

/** UTC instants [start, end) of the Tehran calendar day named by `marker`. */
export function tehranDayWindowOfMarker(marker: DayMarker): { start: Date; end: Date } {
  const start = tehranInstant(
    marker.getUTCFullYear(),
    marker.getUTCMonth() + 1,
    marker.getUTCDate(),
  )
  const next = addDaysMarker(marker, 1)
  const end = tehranInstant(
    next.getUTCFullYear(),
    next.getUTCMonth() + 1,
    next.getUTCDate(),
  )
  return { start, end }
}

// ---------------------------------------------------------------------------
// Jalali calendar (numeric-only)
// ---------------------------------------------------------------------------

export interface JalaliDate {
  jy: number
  jm: number // 1..12
  jd: number // 1..31
}

/** Jalali date of the Tehran calendar day named by a marker (or of a Gregorian y/m/d). */
export function jalaliOfMarker(marker: DayMarker): JalaliDate {
  return g2jNum(marker.getUTCFullYear(), marker.getUTCMonth() + 1, marker.getUTCDate())
}

/** Jalali date of the Tehran calendar day containing `instant`. */
export function jalaliOfInstant(instant: Date): JalaliDate {
  return jalaliOfMarker(dayMarkerOf(instant))
}

/** Marker of a Jalali date (null when invalid, e.g. 1402/12/30 in a non-leap year). */
export function tryJalaliToMarker(jy: number, jm: number, jd: number): DayMarker | null {
  if (!isValidJalaaliDate(jy, jm, jd)) return null
  const g = j2gNum(jy, jm, jd)
  return markerOfTehranDate(g.gy, g.gm, g.gd)
}

/** Marker of a Jalali date — throws on invalid input (use tryJalaliToMarker for user input). */
export function jalaliToMarker(jy: number, jm: number, jd: number): DayMarker {
  const marker = tryJalaliToMarker(jy, jm, jd)
  if (!marker) throw new Error(`تاریخ شمسی نامعتبر: ${jy}/${jm}/${jd}`)
  return marker
}

/** Parse "1403/05/12" or "1403-05-12" → marker (null when invalid). Same grammar as legacy jalali.ts:parseJalaliString. */
export function parseJalaliStringToMarker(s: string): DayMarker | null {
  const m = /^(\d{4})[/\-](\d{1,2})[/\-](\d{1,2})$/.exec(String(s ?? '').trim())
  if (!m) return null
  return tryJalaliToMarker(Number(m[1]), Number(m[2]), Number(m[3]))
}

/** Jalali month range as markers (start = 1st, end = last day; `days` = month length). */
export function jalaliMonthRange(jy: number, jm: number): {
  startMarker: DayMarker
  endMarker: DayMarker
  days: number
} {
  const days = jalaaliMonthLength(jy, jm)
  const startMarker = jalaliToMarker(jy, jm, 1)
  const endMarker = jalaliToMarker(jy, jm, days)
  return { startMarker, endMarker, days }
}

export const FA_MONTHS = [
  'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
  'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
]

// ---------------------------------------------------------------------------
// Jalali input validation (user-facing, Persian errors)
// ---------------------------------------------------------------------------

export type JalaliParseResult =
  | { ok: true; marker: DayMarker; jy: number; jm: number; jd: number }
  | { ok: false; error: string }

/** Validate & parse a user-entered Jalali date ("1403/05/12" style) with clear Persian errors. */
export function parseJalaliInput(s: string): JalaliParseResult {
  const raw = String(s ?? '').trim()
  if (!raw) return { ok: false, error: 'تاریخ را وارد کنید (مثال: ۱۴۰۳/۰۵/۱۲)' }
  const m = /^(\d{4})[/\-](\d{1,2})[/\-](\d{1,2})$/.exec(raw)
  if (!m) {
    return { ok: false, error: 'قالب تاریخ درست نیست — نمونهٔ درست: ۱۴۰۳/۰۵/۱۲' }
  }
  const jy = Number(m[1])
  const jm = Number(m[2])
  const jd = Number(m[3])
  if (jm < 1 || jm > 12) return { ok: false, error: 'ماه باید بین ۱ تا ۱۲ باشد' }
  const days = jalaaliMonthLength(jy, jm)
  if (jd < 1 || jd > days) {
    return {
      ok: false,
      error: days === 30 ? 'این ماه ۳۰ روز دارد' : days === 29 ? 'این ماه ۲۹ روز دارد' : 'این ماه ۳۱ روز دارد',
    }
  }
  const marker = tryJalaliToMarker(jy, jm, jd)
  if (!marker) return { ok: false, error: 'تاریخ شمسی نامعتبر است' }
  return { ok: true, marker, jy, jm, jd }
}

// ---------------------------------------------------------------------------
// Formatting (marker vs instant are EXPLICIT — never guess)
// ---------------------------------------------------------------------------

export function faDigits(s: string | number): string {
  return String(s).replace(/\d/g, (d) => '۰۱۲۳۴۵۶۷۸۹'[Number(d)])
}

/** "1403/05/12" (optionally Persian digits / month name) — for a DAY MARKER. */
export function formatJalaliMarker(
  marker: DayMarker,
  opts?: { withMonthName?: boolean; fa?: boolean },
): string {
  const { jy, jm, jd } = jalaliOfMarker(marker)
  const base = opts?.withMonthName
    ? `${jd} ${FA_MONTHS[jm - 1]} ${jy}`
    : `${jy}/${String(jm).padStart(2, '0')}/${String(jd).padStart(2, '0')}`
  return opts?.fa ? faDigits(base) : base
}

/** "1403/05/12" for an instant (its Tehran calendar day). */
export function formatJalaliInstant(instant: Date, opts?: { withMonthName?: boolean; fa?: boolean }): string {
  return formatJalaliMarker(dayMarkerOf(instant), opts)
}

/** Tehran wall-clock "HH:MM" (optionally seconds / Persian digits) for an instant. */
export function formatTehranTime(instant: Date, opts?: { seconds?: boolean; fa?: boolean }): string {
  const p = tehranParts(instant)
  const base = `${String(p.hh).padStart(2, '0')}:${String(p.mi).padStart(2, '0')}${
    opts?.seconds ? `:${String(p.ss).padStart(2, '0')}` : ''
  }`
  return opts?.fa ? faDigits(base) : base
}

/** "1403/05/12 - 08:30" for an instant. */
export function formatTehranDateTime(instant: Date, opts?: { fa?: boolean }): string {
  return `${formatJalaliInstant(instant, { fa: opts?.fa })} - ${formatTehranTime(instant, { fa: opts?.fa })}`
}

// ---------------------------------------------------------------------------
// Shift-aware workday resolution — approved policy: workday_policy = shift
// ---------------------------------------------------------------------------

export interface ShiftSegmentCtx {
  startMin: number // minutes from Tehran midnight
  endMin: number // minutes from midnight; endMin <= startMin → crosses midnight
}

export interface ShiftCtx {
  segments: ShiftSegmentCtx[]
}

export type WorkdayReason =
  | 'SHIFT_WINDOW' // punch inside the shift window of a concrete start day → that day
  | 'NO_SHIFT' // no applicable shift → Tehran calendar day (documented fallback)
  | 'INVALID_SHIFT' // malformed shift definition → Tehran calendar day (documented fallback)
  | 'OUT_OF_WINDOW' // punch outside the shift window → Tehran calendar day (documented fallback)
  | 'AMBIGUOUS_LATEST' // overlapping shift-day definitions → the LATEST shift start (deterministic)

export interface WorkdayResult {
  /** business workday (day marker) the attendance instant belongs to */
  workday: DayMarker
  /** true when the workday came from the shift start date (policy) rather than the fallback */
  viaShift: boolean
  reason: WorkdayReason
  /** the resolved shift window when viaShift (auditable provenance) */
  window?: { startInstant: Date; endInstant: Date }
}

/**
 * Resolve the business workday of an attendance instant per `workday_policy = shift`:
 * the business date of a record is the START DATE of the employee's applicable shift.
 *
 *  - overnight shifts: start Saturday 22:00 → end Sunday 06:00 ⇒ a Sunday 02:30 punch
 *    belongs to SATURDAY's workday
 *  - shifts crossing Jalali month/year boundaries work naturally (markers are plain dates)
 *  - candidate start days are searched up to 2 days back (defensive; a legal
 *    ShiftSegment window is < 48h) — no assumption of exactly-24h shifts
 *  - no applicable shift / malformed shift / punch outside the window →
 *    the Tehran CALENDAR day of the punch (explicit, documented fallback — never guessed)
 *  - overlapping definitions → latest shift start wins (deterministic, documented)
 *
 * NOTE (shift assignment history): the CALLER supplies the shift applicable ON
 * the punch's day (from ShiftAssignment/defaultShift as of that date). This
 * function never applies "today's" shift to an old record — see docs/TIME-POLICIES.md.
 */
export function resolveWorkday(instant: Date, shift: ShiftCtx | null | undefined): WorkdayResult {
  const fallback = (reason: WorkdayReason): WorkdayResult => ({
    workday: dayMarkerOf(instant),
    viaShift: false,
    reason,
  })

  if (!shift || !Array.isArray(shift.segments) || shift.segments.length === 0) {
    return fallback('NO_SHIFT')
  }
  const valid = shift.segments.every(
    (s) =>
      Number.isFinite(s.startMin) &&
      Number.isFinite(s.endMin) &&
      s.startMin >= 0 &&
      s.startMin < 1440 &&
      s.endMin >= 0 &&
      s.endMin < 1440,
  )
  if (!valid) return fallback('INVALID_SHIFT')

  const punchDay = dayMarkerOf(instant)
  let best: { marker: DayMarker; startInstant: Date; endInstant: Date } | null = null
  let matches = 0

  for (let back = 2; back >= 0; back--) {
    const day = addDaysMarker(punchDay, -back)
    // shift window over all segments (same semantics as the engine's shiftWindow)
    let startMin = Number.POSITIVE_INFINITY
    let endEff = Number.NEGATIVE_INFINITY
    for (const s of shift.segments) {
      startMin = Math.min(startMin, s.startMin)
      endEff = Math.max(endEff, s.endMin > s.startMin ? s.endMin : s.endMin + 1440)
    }
    // wall-clock instants of the window (DST-correct via the tz database)
    const dayStart = tehranInstant(
      day.getUTCFullYear(),
      day.getUTCMonth() + 1,
      day.getUTCDate(),
      Math.floor(startMin / 60),
      startMin % 60,
    )
    const endDay = addDaysMarker(day, Math.floor(endEff / 1440))
    const rem = endEff % 1440
    const endInstant = tehranInstant(
      endDay.getUTCFullYear(),
      endDay.getUTCMonth() + 1,
      endDay.getUTCDate(),
      Math.floor(rem / 60),
      rem % 60,
    )
    if (instant.getTime() >= dayStart.getTime() && instant.getTime() < endInstant.getTime()) {
      matches++
      if (!best || dayStart.getTime() > best.startInstant.getTime()) {
        best = { marker: day, startInstant: dayStart, endInstant }
      }
    }
  }

  if (!best) return fallback('OUT_OF_WINDOW')
  return {
    workday: best.marker,
    viaShift: true,
    reason: matches > 1 ? 'AMBIGUOUS_LATEST' : 'SHIFT_WINDOW',
    window: { startInstant: best.startInstant, endInstant: best.endInstant },
  }
}
