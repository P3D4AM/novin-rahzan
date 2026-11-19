/**
 * Phase 1 (v1.6.0) central time-infrastructure test suite — MOCK ONLY, pure functions.
 * =====================================================================
 * Exercises src/lib/time.ts (the central date/time layer) and proves the
 * protected device pipeline conversions are UNCHANGED (regression).
 *
 * Mandatory coverage (phase 1 mission §6):
 *   [1]  Tehran midnight boundaries            [9]  shifts crossing Jalali month/year boundaries
 *   [2]  Jalali month boundaries               [10] shift-aware business-day assignment
 *   [3]  Jalali year boundaries                [11] missing/invalid shift fallback
 *   [4]  Esfand 29/30 & leap years            [12] browser-timezone independence (TZ child probes)
 *   [5]  Gregorian/Jalali round trips          [13] server-timezone independence (TZ child probes)
 *   [6]  UTC→Tehran conversion                [14] API date-range boundaries (date-only parsing)
 *   [7]  Tehran→UTC conversion (incl. DST)    [15] device import regression (sbxpc/legacy parity)
 *   [16] API day-param & CSV-timestamp contracts (stage 3)
 *   [17] strict wall-clock validation — impossible dates/times rejected, DST gap
 *        hour rejected, DST ambiguity → deterministic later occurrence (review fix 1)
 *   [18] strict API day-param parsing — ISO-with-timezone only (review fix 2)
 *   [19] day-param & wall-clock TZ-independence under 4 host timezones
 *   [20] Persian weekday of markers (S5) — faWeekdayOfMarker + legacy parity
 *   [21] strict INSTANT parsing (S5) — parseInstantParam: full ISO with explicit
 *        timezone only (timezone-less datetimes REJECTED — they parse host-locally)
 *   [22] Tehran day windows (S5) — tehranTodayWindow + tehranDayRangeInstantWindow
 *        (kiosk/me direction window + the D1-retired read-path punch filter)
 *   [23] engine core S5 scenarios on fixed instants (night/multi-segment/stray/
 *        Jalali boundary/leave deadline/mission) — exact expected values
 *   [24] S5 host-timezone independence — the whole S5 surface (parsers, weekday,
 *        windows, engine, attribution) byte-identical under
 *        UTC / Asia/Tehran / Europe/Vienna / America/New_York child probes
 *
 * DST behavior uses the REAL Asia/Tehran tz database (DST existed until
 * 2022-09; +04:30 summers before that, fixed +03:30 since) — never an
 * assumed permanent offset.
 *
 * Run: npx tsx scripts/test-time.ts   (no DATABASE_URL needed — no DB access)
 */
import { spawnSync } from 'child_process'
import * as assert from 'node:assert/strict'
import path from 'path'

import {
  tehranParts,
  tehranOffsetMin,
  tehranInstant,
  tehranWallClockToInstant,
  tehranWallClockStringToInstant,
  dayMarkerOf,
  markerOfTehranDate,
  addDaysMarker,
  markerKey,
  parseDateOnly,
  parseDayParam,
  parseInstantParam,
  markerRangeInstantWindow,
  tehranDayWindowOfMarker,
  tehranTodayWindow,
  tehranDayRangeInstantWindow,
  faWeekdayOfMarker,
  jalaliOfMarker,
  jalaliOfInstant,
  tryJalaliToMarker,
  jalaliToMarker,
  parseJalaliStringToMarker,
  jalaliMonthRange,
  parseJalaliInput,
  formatJalaliMarker,
  formatJalaliInstant,
  formatTehranTime,
  jalaaliMonthLength,
  resolveWorkday,
} from '../src/lib/time'
import { tehranWallClockToUTC } from '../src/lib/sbxpc'
// legacy fixed-offset layer (engine + raw pipeline still consume it — must stay compatible)
import * as legacy from '../src/lib/jalali'
// Phase 2 S5: the pure engine core (computeDaily never touches the DB — the
// PrismaClient in @/lib/db is constructed lazily, same as test-attendance.ts)
import { computeDaily, type ShiftLite } from '../src/lib/attendance'
import { attributePunchWorkday } from '../src/lib/workday'

const ROOT = path.resolve(__dirname, '..')

// --- probe mode (used by the TZ-independence sections): fixed instants only ---
// (never `new Date()` — deterministic under any host timezone)
if (process.argv[2] === '--probe') {
  const punch = tehranInstant(2026, 11, 15, 2, 30) // fixed: Sunday 02:30 Tehran
  const night = { segments: [{ startMin: 22 * 60, endMin: 6 * 60 }] }
  const r = resolveWorkday(punch, night)
  console.log(
    JSON.stringify({
      marker: markerKey(dayMarkerOf(punch)),
      jalali: formatJalaliInstant(punch),
      time: formatTehranTime(punch),
      workday: markerKey(r.workday),
      windowStart: r.window ? r.window.startInstant.toISOString() : null,
    }),
  )
  process.exit(0)
}

// --- day-param probe (fix 2 TZ-independence): parseDayParam + wall-clock parser ---
// under any host timezone (UTC / New York / Tehran / Kiritimati must agree)
if (process.argv[2] === '--probe-day') {
  const inputs: Array<[string, string]> = [
    ['date-only', '2026-11-17'],
    ['date-only-1d', '2026-11-7'],
    ['iso-Z', '2026-11-17T22:30:00Z'],
    ['iso-Z-ms', '2026-11-17T00:00:00.000Z'],
    ['iso-offset-neg', '2026-11-16T22:30:00-05:00'],
    ['iso-offset-pos14', '2026-11-17T00:30:00+14:00'],
    ['tz-less', '2026-11-17T00:30'],
    ['tz-less-sec', '2026-11-17T00:30:00'],
    ['junk', 'garbage'],
  ]
  const dayParams = inputs.map(([k, s]) => {
    const marker = parseDayParam(s)
    return { k, day: marker ? markerKey(marker) : null }
  })
  const wallInputs = [
    '2026-11-17 02:30',
    '2021-06-15 12:00',
    '2021-03-22 00:30',
    '2021-09-21 23:30',
    '2026-02-31 08:30',
    '2026-11-17 25:00',
  ]
  const wallClock = wallInputs.map((s) => {
    const d = tehranWallClockStringToInstant(s)
    return { s, iso: d ? d.toISOString() : null }
  })
  console.log(JSON.stringify({ dayParams, wallClock }))
  process.exit(0)
}

// --- S5 probe: the whole S5 surface on FIXED inputs (deterministic under any
// host timezone; compared byte-for-byte across UTC/Tehran/Vienna/New_York by
// section 24). NEVER uses new Date() for anything except the todayWindow
// internal-consistency boolean (its printed VALUE stays true under every TZ). --
if (process.argv[2] === '--probe-s5') {
  // [1] strict instant parsing — the S5 write-path contract
  const instantInputs: Array<[string, string]> = [
    ['iso-Z', '2026-11-17T08:30:00Z'],
    ['iso-Z-ms', '2026-11-17T08:30:00.123Z'],
    ['iso-pos330', '2026-11-17T08:30:00+03:30'],
    ['iso-neg5', '2026-11-17T08:30:00-05:00'],
    ['tz-less', '2026-11-17T00:30'],
    ['tz-less-sec', '2026-11-17T00:30:00'],
    ['date-only', '2026-11-17'],
    ['basic-offset', '2026-11-17T22:30:00+0330'],
    ['hour25', '2026-11-17T25:00:00Z'],
    ['feb30', '2026-02-30T10:00:00Z'],
    ['junk', 'garbage'],
  ]
  const instants = instantInputs.map(([k, s]) => ({
    k,
    iso: parseInstantParam(s) ? parseInstantParam(s)!.toISOString() : null,
  }))

  // [2] marker weekday — central + legacy (the engine's Friday detection)
  const weekdays = [
    '2026-11-13', // جمعه (Friday)
    '2026-11-14', // شنبه (Saturday)
    '2026-11-15', // یکشنبه
    '2026-11-16', // دوشنبه
    '2026-11-17', // سه‌شنبه
    '2026-11-18', // چهارشنبه
    '2026-11-19', // پنجشنبه
  ].map((k) => {
    const m = parseDateOnly(k)!
    return { k, central: faWeekdayOfMarker(m), legacy: legacy.faWeekday(m) }
  })

  // [3] Tehran day windows — kiosk/me direction window + the read-path filter
  const win = tehranDayWindowOfMarker(markerOfTehranDate(2026, 11, 17))
  const range = tehranDayRangeInstantWindow(markerOfTehranDate(2026, 11, 14), markerOfTehranDate(2026, 11, 17))
  const windows = {
    dayStart: win.start.toISOString(),
    dayEnd: win.end.toISOString(),
    rangeGte: range.gte.toISOString(),
    rangeLt: range.lt.toISOString(),
  }

  // [4] todayWindow internal consistency (the printed VALUE is TZ-independent):
  // start <= now < end and the window's day == the Tehran day of now
  const now = new Date()
  const tw = tehranTodayWindow()
  const todayOk =
    tw.start.getTime() <= now.getTime() &&
    now.getTime() < tw.end.getTime() &&
    markerKey(dayMarkerOf(tw.start)) === markerKey(dayMarkerOf(now))

  // [5] engine core on fixed instants — computeDaily scenarios (pure, no DB)
  const mkShift = (id: string, segs: Array<{ startMin: number; endMin: number; breakMin?: number }>, graces = 10): ShiftLite => ({
    id, name: id, lateGraceMin: graces, earlyGraceMin: graces, overtimeEligible: true,
    segments: segs.map((s) => ({ startMin: s.startMin, endMin: s.endMin, breakMin: s.breakMin ?? 0 })),
  })
  const dayShift = mkShift('day', [{ startMin: 8 * 60, endMin: 16 * 60 }])
  const eveningShift = mkShift('evening', [{ startMin: 15 * 60, endMin: 23 * 60 + 30 }])
  const nightShift = mkShift('night', [{ startMin: 22 * 60, endMin: 6 * 60 }])
  const splitShift = mkShift('split', [
    { startMin: 8 * 60, endMin: 12 * 60 },
    { startMin: 13 * 60, endMin: 17 * 60 },
  ], 0)
  const run = (
    k: string,
    day: Date,
    shift: ShiftLite | null,
    punches: Array<{ timestamp: Date; direction?: string }>,
    extra?: { missionSpans?: Array<{ start: Date; end: Date }>; leaveSpans?: Array<{ start: Date; end: Date; leaveTypeId: string }>; isHoliday?: boolean; isOff?: boolean },
  ) => {
    const d = computeDaily({
      personId: 'p1',
      day,
      shift,
      punches,
      missionSpans: extra?.missionSpans ?? [],
      leaveSpans: extra?.leaveSpans ?? [],
      isHoliday: extra?.isHoliday ?? false,
      isOff: extra?.isOff ?? false,
      now: new Date(Date.UTC(2026, 11, 1, 12, 0)),
    })
    return {
      k,
      status: d.status,
      worked: d.workedMinutes,
      late: d.lateMinutes,
      early: d.earlyLeaveMinutes,
      ot: d.overtimeMinutes,
      payable: d.payableOvertimeMinutes,
      leave: d.leaveMinutes,
      mission: d.missionMinutes,
      note: d.note,
      firstIn: d.firstIn ? d.firstIn.toISOString() : null,
      lastOut: d.lastOut ? d.lastOut.toISOString() : null,
    }
  }
  const T = (y: number, mo: number, d: number, h: number, mi = 0) => tehranInstant(y, mo, d, h, mi)
  const engine = [
    // E1: day shift Tuesday 2026-11-17 (a regular workday marker)
    run('E1-day-0820-1530', markerOfTehranDate(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 8, 20), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 15, 30), direction: 'OUT' },
    ]),
    // E2: night shift Saturday 2026-11-14 22:00→Sunday 06:00 (cross-midnight, the policy example)
    run('E2-night-sat', markerOfTehranDate(2026, 11, 14), nightShift, [
      { timestamp: T(2026, 11, 14, 22, 0), direction: 'IN' },
      { timestamp: T(2026, 11, 15, 6, 0), direction: 'OUT' },
    ]),
    // E3: S4 hotfix parity — stray 02:30–02:50 pair + real 15:30–20:00 on a 15:00–23:30 evening shift
    run('E3-stray-pair', markerOfTehranDate(2026, 11, 17), eveningShift, [
      { timestamp: T(2026, 11, 17, 2, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 2, 50), direction: 'OUT' },
      { timestamp: T(2026, 11, 17, 15, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 20, 0), direction: 'OUT' },
    ]),
    // E4: Jalali year boundary — night shift 1404-12-29 (2026-03-20) 22:00 → 1405-01-01 (2026-03-21) 06:00
    run('E4-esfand-farvardin', markerOfTehranDate(2026, 3, 20), nightShift, [
      { timestamp: T(2026, 3, 20, 22, 0), direction: 'IN' },
      { timestamp: T(2026, 3, 21, 6, 0), direction: 'OUT' },
    ]),
    // E5: approved leave attached to the window start excuses lateness (08:00–10:00, IN 09:30)
    run('E5-leave-start', markerOfTehranDate(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 9, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 0), direction: 'OUT' },
    ], { leaveSpans: [{ start: T(2026, 11, 17, 8, 0), end: T(2026, 11, 17, 10, 0), leaveTypeId: 'lt1' }] }),
    // E6: approved mission, no punches → MISSION 480
    run('E6-mission', markerOfTehranDate(2026, 11, 17), dayShift, [], {
      missionSpans: [{ start: T(2026, 11, 17, 8, 0), end: T(2026, 11, 17, 16, 0) }],
    }),
    // E7: multi-segment shift (08–12 + 13–17, graces 0): IN 08:01 → late 1, OUT 16:40 → early 20
    run('E7-split', markerOfTehranDate(2026, 11, 17), splitShift, [
      { timestamp: T(2026, 11, 17, 8, 1), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 40), direction: 'OUT' },
    ]),
    // E8: no punches, shift day, determination window passed → ABSENT
    run('E8-absent', markerOfTehranDate(2026, 11, 17), dayShift, []),
    // E9: holiday with all-day work → actual OT (A4: no minimum/permission gate)
    run('E9-holiday-ot', markerOfTehranDate(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 8, 0), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 0), direction: 'OUT' },
    ], { isHoliday: true }),
  ]

  // [6] workday attribution on fixed instants (the S3/S4 engine convention)
  const night = nightShift
  const shiftOf = (day: Date) => (markerKey(day) === '2026-11-14' ? night : null)
  const attr = (k: string, instant: Date, direction: string | null) => {
    const r = attributePunchWorkday(instant, shiftOf, direction)
    return { k, workday: markerKey(r.workday), reason: r.reason }
  }
  const attribution = [
    attr('sun-0230-in', T(2026, 11, 15, 2, 30), 'IN'),
    attr('sun-0600-out', T(2026, 11, 15, 6, 0), 'OUT'),
    attr('sun-0600-in', T(2026, 11, 15, 6, 0), 'IN'),
    attr('sun-1000-auto', T(2026, 11, 15, 10, 0), null),
    attr('sat-2300-in', T(2026, 11, 14, 23, 0), 'IN'),
  ]

  console.log(JSON.stringify({ todayOk, instants, weekdays, windows, engine, attribution }))
  process.exit(0)
}

// Accounting (final-review fix 3): assertions and sections are counted SEPARATELY.
// A section summary is NOT an independent assertion: the aggregate assertion count
// counts only check() calls. A section fails if any of its checks failed OR if its
// body threw an uncaught error (the latter previously — wrongly — counted as passed).
let assertionsPassed = 0
let assertionsFailed = 0
let sectionsPassed = 0
let sectionsFailed = 0
function section(name: string, fn: () => void) {
  const before = assertionsFailed
  let crashed = false
  try {
    fn()
    console.log(`  ✔ ${name}`)
  } catch (err) {
    crashed = true
    console.log(`  ✘ ${name}`)
    console.log(`    ${(err as Error)?.message ?? err}`)
  } finally {
    const ok = !crashed && assertionsFailed === before
    console.log(`— ${name}: ${ok ? 'OK' : 'FAILED'}`)
    if (ok) sectionsPassed++
    else sectionsFailed++
  }
}
function check(name: string, fn: () => void) {
  try {
    fn()
    assertionsPassed++
    console.log(`  ✔ ${name}`)
  } catch (err) {
    assertionsFailed++
    console.log(`  ✘ ${name} — ${(err as Error)?.message ?? err}`)
  }
}

const iso = (d: Date) => d.toISOString()
const g = (y: number, m: number, d: number) => markerOfTehranDate(y, m, d)

console.log('== Phase 1 time infrastructure tests ==\n')

// [1] Tehran midnight boundaries ------------------------------------------------
section('1. Tehran midnight boundaries', () => {
  check('Tehran midnight instant = 20:30Z of the previous UTC day', () => {
    assert.equal(iso(tehranInstant(2026, 11, 17, 0, 0)), '2026-11-16T20:30:00.000Z')
  })
  check('23:59:59 Tehran belongs to its own day; 00:00:00 to the next', () => {
    const late = tehranInstant(2026, 11, 16, 23, 59, 59)
    const midnight = tehranInstant(2026, 11, 17, 0, 0)
    assert.equal(markerKey(dayMarkerOf(late)), '2026-11-16')
    assert.equal(markerKey(dayMarkerOf(midnight)), '2026-11-17')
  })
  check('day window [start,end) of marker 2026-11-17', () => {
    const w = tehranDayWindowOfMarker(g(2026, 11, 17))
    assert.equal(iso(w.start), '2026-11-16T20:30:00.000Z')
    assert.equal(iso(w.end), '2026-11-17T20:30:00.000Z')
  })
  check('punch at Tehran 00:30 → its own day window, not the previous', () => {
    const punch = tehranInstant(2026, 11, 17, 0, 30)
    const w = tehranDayWindowOfMarker(g(2026, 11, 17))
    assert.ok(punch >= w.start && punch < w.end)
    assert.equal(markerKey(dayMarkerOf(punch)), '2026-11-17')
  })
})

// [2] Jalali month boundaries ---------------------------------------------------
section('2. Jalali month boundaries', () => {
  check('Aban 1405 has 30 days; month range markers are adjacent', () => {
    const mr = jalaliMonthRange(1405, 8)
    assert.equal(mr.days, 30)
    assert.equal(markerKey(mr.startMarker), '2026-10-23')
    assert.equal(markerKey(mr.endMarker), '2026-11-21')
    assert.equal(mr.endMarker.getTime() - mr.startMarker.getTime(), 29 * 86400000)
  })
  check('Mehr 1405 (30 days) and Farvardin 1405 (31 days) marker ranges', () => {
    const mehr = jalaliMonthRange(1405, 7)
    assert.equal(mehr.days, 30)
    assert.equal(markerKey(mehr.startMarker), '2026-09-23')
    assert.equal(markerKey(mehr.endMarker), '2026-10-22')
    const fav = jalaliMonthRange(1405, 1)
    assert.equal(fav.days, 31)
    assert.equal(markerKey(fav.startMarker), '2026-03-21')
    assert.equal(markerKey(fav.endMarker), '2026-04-20')
  })
  check('invalid day beyond month length rejected by parseJalaliInput (Persian error)', () => {
    const r = parseJalaliInput('1405/08/31')
    assert.equal(r.ok, false)
    if (!r.ok) assert.match(r.error, /۳۰ روز/)
  })
})

// [3] Jalali year boundaries ----------------------------------------------------
section('3. Jalali year boundaries', () => {
  check('Esfand 30 1403 = 2025-03-20; Farvardin 1 1404 = 2025-03-21 (consecutive days)', () => {
    const e30 = jalaliToMarker(1403, 12, 30)
    const n1 = jalaliToMarker(1404, 1, 1)
    assert.equal(markerKey(e30), '2025-03-20')
    assert.equal(markerKey(n1), '2025-03-21')
    assert.equal(n1.getTime() - e30.getTime(), 86400000)
  })
  check('Esfand 29 1404 = 2026-03-20 → Farvardin 1 1405 = 2026-03-21', () => {
    assert.equal(markerKey(jalaliToMarker(1404, 12, 29)), '2026-03-20')
    assert.equal(markerKey(jalaliToMarker(1405, 1, 1)), '2026-03-21')
  })
  check('addDaysMarker over the new-year edge', () => {
    assert.equal(markerKey(addDaysMarker(jalaliToMarker(1404, 12, 29), 1)), markerKey(jalaliToMarker(1405, 1, 1)))
  })
})

// [4] Esfand 29/30 and leap years ----------------------------------------------
section('4. Esfand 29/30 & leap years', () => {
  check('jalaaliMonthLength: 1403→30, 1402→29, 1404→29, 1408→30', () => {
    assert.equal(jalaaliMonthLength(1403, 12), 30)
    assert.equal(jalaaliMonthLength(1402, 12), 29)
    assert.equal(jalaaliMonthLength(1404, 12), 29)
    assert.equal(jalaaliMonthLength(1408, 12), 30)
  })
  check('parseJalaliInput accepts 1403/12/30 (leap) and rejects 1402/12/30', () => {
    assert.equal(parseJalaliInput('1403/12/30').ok, true)
    assert.equal(parseJalaliInput('1402/12/30').ok, false)
  })
  check('tryJalaliToMarker(null) on 1404/12/30', () => {
    assert.equal(tryJalaliToMarker(1404, 12, 30), null)
  })
  check('parseJalaliStringToMarker matches legacy parseJalaliString on valid & invalid', () => {
    assert.equal(markerKey(parseJalaliStringToMarker('1403/05/12')!), markerKey(legacy.parseJalaliString('1403/05/12')!))
    assert.equal(parseJalaliStringToMarker('1404/12/30'), null)
    assert.equal(legacy.parseJalaliString('1404/12/30'), null)
  })
})

// [5] Gregorian/Jalali round trips ---------------------------------------------
section('5. Gregorian/Jalali round trips', () => {
  check('10 years of markers round-trip j→g→j (stride 13d)', () => {
    let n = 0
    for (let t = Date.UTC(2020, 0, 1); t <= Date.UTC(2030, 11, 31); t += 13 * 86400000) {
      const marker = new Date(t)
      const j = jalaliOfMarker(marker)
      assert.equal(markerKey(jalaliToMarker(j.jy, j.jm, j.jd)), markerKey(marker), markerKey(marker))
      n++
    }
    assert.ok(n > 250)
  })
  check('round trip instants: jalaliOfInstant ↔ marker', () => {
    for (const t of ['2026-11-17T00:00:00Z', '2025-03-20T12:00:00Z', '2024-02-29T23:59:59Z', '2021-06-15T12:00:00Z']) {
      const inst = new Date(t)
      assert.equal(markerKey(dayMarkerOf(inst)), markerKey(dayMarkerOf(new Date(dayMarkerOf(inst).getTime() + 12 * 3600000))))
    }
  })
  check('today (2026-11-17) = Aban 26, 1405', () => {
    assert.deepEqual(jalaliOfMarker(g(2026, 11, 17)), { jy: 1405, jm: 8, jd: 26 })
  })
})

// [6] UTC → Tehran conversion --------------------------------------------------
section('6. UTC→Tehran conversion (tz database, incl. DST era)', () => {
  check('2026-11-17T00:00Z → Tehran 03:30 same day', () => {
    const p = tehranParts(new Date('2026-11-17T00:00:00Z'))
    assert.deepEqual([p.y, p.m, p.d, p.hh, p.mi], [2026, 11, 17, 3, 30])
  })
  check('2026-11-16T21:00Z → Tehran 2026-11-17 00:30 (next day)', () => {
    const p = tehranParts(new Date('2026-11-16T21:00:00Z'))
    assert.deepEqual([p.y, p.m, p.d, p.hh, p.mi], [2026, 11, 17, 0, 30])
  })
  check('DST summer 2021: 12:00Z → Tehran 16:30, offset +04:30', () => {
    const inst = new Date('2021-06-15T12:00:00Z')
    const p = tehranParts(inst)
    assert.equal(p.hh, 16)
    assert.equal(p.mi, 30)
    assert.equal(tehranOffsetMin(inst), 270)
  })
  check('winter 2021: offset +03:30', () => {
    assert.equal(tehranOffsetMin(new Date('2021-12-15T12:00:00Z')), 210)
  })
  check('post-2022 fixed +03:30 (DST abolished)', () => {
    for (const s of ['2023-06-15T12:00:00Z', '2024-01-01T00:00:00Z', '2026-11-17T00:00:00Z']) {
      assert.equal(tehranOffsetMin(new Date(s)), 210, s)
    }
    assert.equal(tehranOffsetMin(new Date('2022-06-15T12:00:00Z')), 270) // last DST summer
  })
})

// [7] Tehran → UTC conversion --------------------------------------------------
section('7. Tehran→UTC conversion', () => {
  check('tehranInstant(2026,11,17,0,0) = 2026-11-16T20:30Z', () => {
    assert.equal(iso(tehranInstant(2026, 11, 17, 0, 0)), '2026-11-16T20:30:00.000Z')
  })
  check('DST summer: tehranInstant(2021,6,15,12,0) = 12:00Z - 270min', () => {
    assert.equal(iso(tehranInstant(2021, 6, 15, 12, 0)), '2021-06-15T07:30:00.000Z')
  })
  check('wall-clock-string parse ("YYYY-MM-DD HH:MM[:SS]", lenient digits/separators)', () => {
    const d = tehranWallClockStringToInstant('2026-11-17 02:30:00')
    assert.equal(iso(d!), '2026-11-16T23:00:00.000Z')
    // seconds optional — HH:MM equals HH:MM:00
    assert.equal(tehranWallClockStringToInstant('2026-11-17 02:30')!.getTime(), d!.getTime())
    // lenient: '/' separators and 1–2 digit fields (CSV import contract)
    assert.equal(tehranWallClockStringToInstant('2026/11/17 2:30')!.getTime(), d!.getTime())
    assert.equal(tehranWallClockStringToInstant('2026-11-17 2:5')!.getTime(), tehranWallClockStringToInstant('2026-11-17 02:05')!.getTime())
    assert.equal(tehranWallClockStringToInstant('garbage'), null)
    assert.equal(tehranWallClockStringToInstant('1405/08/26 02:30'), null) // Jalali strings are NOT this grammar
  })
})

// [8] Overnight shifts ---------------------------------------------------------
section('8. Overnight shifts', () => {
  const night = { segments: [{ startMin: 22 * 60, endMin: 6 * 60 }] } // 22:00 → 06:00 (crosses midnight)
  check('punch Sunday 02:30 → SATURDAY workday (policy example)', () => {
    // 2026-11-14 is a Saturday, 2026-11-15 is a Sunday
    const punch = tehranInstant(2026, 11, 15, 2, 30)
    const r = resolveWorkday(punch, night)
    assert.equal(r.reason, 'SHIFT_WINDOW')
    assert.ok(r.viaShift)
    assert.equal(markerKey(r.workday), '2026-11-14')
  })
  check('punch Saturday 23:59 → Saturday workday', () => {
    const punch = tehranInstant(2026, 11, 14, 23, 59)
    assert.equal(markerKey(resolveWorkday(punch, night).workday), '2026-11-14')
  })
  check('punch Sunday 07:00 (after window) → Tehran-day fallback, documented', () => {
    const punch = tehranInstant(2026, 11, 15, 7, 0)
    const r = resolveWorkday(punch, night)
    assert.equal(r.reason, 'OUT_OF_WINDOW')
    assert.equal(markerKey(r.workday), '2026-11-15')
  })
  check('boundary: exactly 06:00 Tehran is OUT (end exclusive); 22:00 exact is IN', () => {
    const endPunch = tehranInstant(2026, 11, 15, 6, 0)
    assert.equal(resolveWorkday(endPunch, night).reason, 'OUT_OF_WINDOW')
    const startPunch = tehranInstant(2026, 11, 14, 22, 0)
    assert.equal(markerKey(resolveWorkday(startPunch, night).workday), '2026-11-14')
  })
})

// [9] Shifts crossing Jalali month/year boundaries ------------------------------
section('9. Shifts crossing Jalali month/year boundaries', () => {
  check('shift Esfand 29 → Farvardin 1 (month boundary): early-morning punch → Esfand 29 workday', () => {
    const shift = { segments: [{ startMin: 22 * 60, endMin: 4 * 60 }] }
    const punch = tehranInstant(2025, 3, 20, 2, 30) // Farvardin 1 1404, 02:30
    const r = resolveWorkday(punch, shift)
    assert.equal(markerKey(r.workday), '2025-03-19') // Esfand 29 1403
    assert.equal(jalaliOfMarker(r.workday).jm, 12)
    assert.equal(jalaliOfMarker(r.workday).jd, 29)
  })
  check('shift Esfand 30 1403 → Farvardin 1 1404 (year boundary, leap Esfand 30)', () => {
    const shift = { segments: [{ startMin: 22 * 60, endMin: 6 * 60 }] }
    const punch = tehranInstant(2025, 3, 21, 3, 30) // Farvardin 1 1404, 03:30
    const r = resolveWorkday(punch, shift)
    assert.equal(markerKey(r.workday), '2025-03-20') // Esfand 30 1403
    assert.deepEqual(jalaliOfMarker(r.workday), { jy: 1403, jm: 12, jd: 30 })
  })
})

// [10] Shift-aware business-day assignment --------------------------------------
section('10. Shift-aware business-day assignment', () => {
  const day = { segments: [{ startMin: 8 * 60, endMin: 17 * 60 }] }
  check('plain day-shift punch 09:00 → same day; window starts at 08:00 Tehran', () => {
    const punch = tehranInstant(2026, 11, 17, 9, 0)
    const r = resolveWorkday(punch, day)
    assert.equal(r.reason, 'SHIFT_WINDOW')
    assert.equal(markerKey(r.workday), '2026-11-17')
    assert.equal(iso(r.window!.startInstant), '2026-11-17T04:30:00.000Z') // 08:00 Tehran
  })
  check('split shift (08–12 + 13–17): both segments map to the same day', () => {
    const split = { segments: [{ startMin: 480, endMin: 720 }, { startMin: 780, endMin: 1020 }] }
    for (const h of [9, 11, 14, 16]) {
      const punch = tehranInstant(2026, 11, 17, h, 0)
      assert.equal(markerKey(resolveWorkday(punch, split).workday), '2026-11-17')
    }
  })
  check('punch before window (06:30) → OUT_OF_WINDOW fallback to Tehran day', () => {
    const punch = tehranInstant(2026, 11, 17, 6, 30)
    const r = resolveWorkday(punch, day)
    assert.equal(r.reason, 'OUT_OF_WINDOW')
    assert.equal(markerKey(r.workday), '2026-11-17')
  })
  check('Tehran 00:30 punch with 08–17 shift → OUT_OF_WINDOW, still the same Tehran day', () => {
    const punch = tehranInstant(2026, 11, 17, 0, 30)
    assert.equal(markerKey(resolveWorkday(punch, day).workday), '2026-11-17')
  })
})

// [11] Missing/invalid shift fallback -------------------------------------------
section('11. Missing shift fallback', () => {
  const punch = tehranInstant(2026, 11, 17, 2, 30)
  check('null shift → NO_SHIFT, Tehran calendar day of the punch', () => {
    const r = resolveWorkday(punch, null)
    assert.equal(r.reason, 'NO_SHIFT')
    assert.ok(!r.viaShift)
    assert.equal(markerKey(r.workday), '2026-11-17')
  })
  check('empty segments → NO_SHIFT', () => {
    assert.equal(resolveWorkday(punch, { segments: [] }).reason, 'NO_SHIFT')
  })
  check('malformed segment (startMin 1500) → INVALID_SHIFT fallback', () => {
    const r = resolveWorkday(punch, { segments: [{ startMin: 1500, endMin: 200 }] })
    assert.equal(r.reason, 'INVALID_SHIFT')
    assert.equal(markerKey(r.workday), '2026-11-17')
  })
})

// [14] API date-range boundaries (unit level — routes tested in stage 3/5) ------
section('14. API date-range boundaries (date-only contract)', () => {
  check('parseDateOnly: strict YYYY-MM-DD → UTC-midnight marker, NO tz shift', () => {
    assert.equal(markerKey(parseDateOnly('2026-11-17')!), '2026-11-17')
    assert.equal(parseDateOnly('2026-11-17')!.toISOString(), '2026-11-17T00:00:00.000Z')
  })
  check('parseDateOnly rejects impossible dates and junk', () => {
    assert.equal(parseDateOnly('2026-11-31'), null)
    assert.equal(parseDateOnly('2026-02-30'), null)
    assert.equal(parseDateOnly('not-a-date'), null)
    assert.equal(parseDateOnly(''), null)
  })
  check('parseDateOnly accepts one/two-digit month/day (API leniency, still date-only)', () => {
    assert.equal(markerKey(parseDateOnly('2026-11-7')!), '2026-11-07')
  })
  check('Jalali month range maps to intended Tehran business days (markers)', () => {
    const mr = jalaliMonthRange(1405, 8)
    assert.equal(markerKey(mr.startMarker), '2026-10-23')
    assert.equal(markerKey(mr.endMarker), '2026-11-21')
    const days = Math.round((mr.endMarker.getTime() - mr.startMarker.getTime()) / 86400000) + 1
    assert.equal(days, 30)
  })
  check('marker day-window boundaries are true Tehran midnights (DST-correct)', () => {
    // DST-summer day: Tehran midnight is 19:30Z, NOT 20:30Z
    const w = tehranDayWindowOfMarker(g(2021, 6, 15))
    assert.equal(iso(w.start), '2021-06-14T19:30:00.000Z')
    assert.equal(iso(w.end), '2021-06-15T19:30:00.000Z')
  })
})

// [15] Device import regression -------------------------------------------------
section('15. Device pipeline regression (protected subsystem unchanged)', () => {
  check('time.tehranWallClockToInstant === sbxpc.tehranWallClockToUTC across 2020–2027 (incl. DST)', () => {
    let n = 0
    for (let t = Date.UTC(2020, 0, 1); t <= Date.UTC(2027, 11, 31); t += 7 * 86400000 + 3600000) {
      const wall = t
      const a = tehranWallClockToInstant(wall)
      const b = tehranWallClockToUTC(wall)
      assert.equal(a.getTime(), b.getTime(), new Date(t).toISOString())
      n++
    }
    assert.ok(n > 300)
  })
  check('wall-clock-string conversion equals the fkattend construction (ms + sbxpc)', () => {
    const s = '2021-06-15 12:00:00'
    const m = /^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$/.exec(s)!
    const fkWay = tehranWallClockToUTC(Date.UTC(+m[1], +m[2] - 1, +m[3], +m[4], +m[5], +m[6]))
    assert.equal(tehranWallClockStringToInstant(s)!.getTime(), fkWay.getTime())
    assert.equal(iso(tehranWallClockStringToInstant(s)!), '2021-06-15T07:30:00.000Z') // DST +04:30
  })
  check('legacy tehranDayOf === dayMarkerOf for the no-DST era (post-2022) — engine compat', () => {
    for (let t = Date.UTC(2023, 0, 1); t <= Date.UTC(2026, 11, 31); t += 17 * 86400000) {
      const inst = new Date(t)
      assert.equal(legacy.tehranDayOf(inst).getTime(), dayMarkerOf(inst).getTime(), inst.toISOString())
    }
  })
  check('legacy tehranMinutes/faTime unchanged semantics on a fixed instant', () => {
    const inst = tehranInstant(2026, 11, 17, 8, 45)
    assert.equal(legacy.tehranMinutes(inst), 8 * 60 + 45)
    assert.equal(formatTehranTime(inst), '08:45')
    assert.equal(legacy.faTime(inst), '۰۸:۴۵')
    assert.equal(formatTehranTime(inst, { fa: true }), '۰۸:۴۵')
  })
  check('legacy monthRange === jalaliMonthRange markers (bootstrap/me consumers)', () => {
    const legacyMr = legacy.monthRange(1405, 8)
    const mr = jalaliMonthRange(1405, 8)
    assert.equal(legacyMr.start.getTime(), mr.startMarker.getTime())
    assert.equal(legacyMr.end.getTime(), mr.endMarker.getTime())
    assert.equal(legacyMr.days, mr.days)
  })
  check('formatting: Jalali marker/instant formats', () => {
    const inst = tehranInstant(2026, 11, 17, 8, 45) // Tehran 2026-11-17 = Aban 26, 1405
    assert.equal(formatJalaliInstant(inst), '1405/08/26')
    assert.equal(formatJalaliInstant(inst, { fa: true }), '۱۴۰۵/۰۸/۲۶')
    assert.equal(formatJalaliMarker(g(2026, 11, 17), { withMonthName: true }), '26 آبان 1405')
  })
})

// [12][13] Browser/server timezone independence ---------------------------------
// The SAME module runs in the browser (client components) and on the server:
// prove outputs are independent of the host timezone by re-running a fixed
// probe under TZ=UTC, TZ=America/New_York, TZ=Asia/Tehran, TZ=Pacific/Kiritimati.
section('12+13. Host-timezone independence (child probes under 4 TZs)', () => {
  const PROBE = '--probe'
  const self = path.join('scripts', 'test-time.ts')
  const tsxCli = path.join(ROOT, 'node_modules', 'tsx', 'dist', 'cli.mjs')
  const outputs: Record<string, string> = {}
  for (const tz of ['UTC', 'America/New_York', 'Asia/Tehran', 'Pacific/Kiritimati']) {
    const res = spawnSync(process.execPath, [tsxCli, self, PROBE], {
      cwd: ROOT,
      env: { ...process.env, TZ: tz },
      encoding: 'utf8',
    })
    assert.equal(res.status, 0, `probe failed under TZ=${tz}: ${res.stderr?.slice(0, 400)}`)
    outputs[tz] = res.stdout.trim()
    assert.ok(res.stdout.includes('"marker"'), `probe produced no JSON under TZ=${tz}`)
  }
  check('all four TZs produce IDENTICAL outputs for fixed instants', () => {
    const base = outputs['UTC']
    for (const [tz, out] of Object.entries(outputs)) {
      assert.equal(out, base, `TZ=${tz} diverged`)
    }
  })
  check('probe output is stable and self-consistent', () => {
    const data = JSON.parse(outputs['UTC'])
    assert.equal(data.marker, '2026-11-15')
    assert.equal(data.jalali, '1405/08/24')
    assert.equal(data.time, '02:30')
    assert.equal(data.workday, '2026-11-14')
    assert.equal(data.windowStart, '2026-11-14T18:30:00.000Z')
  })
})

// [16] API day-param contract (stage 3) --------------------------------------
section('16. API day-param & CSV-timestamp contracts (stage 3)', () => {
  check('parseDayParam: date-only, full marker ISO, instant floored, junk rejected', () => {
    assert.equal(markerKey(parseDayParam('2026-11-17')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-7')!), '2026-11-07')
    assert.equal(markerKey(parseDayParam('2026-11-17T00:00:00.000Z')!), '2026-11-17')
    // a full ISO instant is floored to its UTC day (legacy startOfUTCDay semantics)
    assert.equal(markerKey(parseDayParam('2026-11-17T22:30:00Z')!), '2026-11-17')
    assert.equal(parseDayParam('garbage'), null)
    assert.equal(parseDayParam(null), null)
    assert.equal(parseDayParam(''), null)
  })
  check('markerRangeInstantWindow: [from, to+1) — engine-consistent punch filter', () => {
    const w = markerRangeInstantWindow(g(2026, 11, 14), g(2026, 11, 15))
    assert.equal(iso(w.gte), '2026-11-14T00:00:00.000Z')
    assert.equal(iso(w.lt), '2026-11-16T00:00:00.000Z')
  })
  check('CSV import: ISO passthrough vs naive Tehran wall-clock', () => {
    // ISO (has 'T') → instant, unchanged
    assert.equal(tehranWallClockStringToInstant('2026-11-17T22:30:00Z'), null) // not this grammar — handled by new Date() in the route
    // naive date+time columns → Tehran wall-clock instant
    const ts = tehranWallClockStringToInstant('2026-11-17 08:30')
    assert.equal(iso(ts!), '2026-11-17T05:00:00.000Z') // 08:30 Tehran = 05:00Z
    // DST-era naive string → +04:30
    assert.equal(iso(tehranWallClockStringToInstant('2021-06-15 12:00')!), '2021-06-15T07:30:00.000Z')
  })
})

// [17] Strict wall-clock validation (v1.6.0 review fix 1) ----------------------
// DST facts used below come from the REAL Asia/Tehran tz database:
//   2021 spring-forward at 2021-03-21T20:30Z: wall jumped 2021-03-21 23:59 →
//   2021-03-22 01:00, so 2021-03-22 00:00–00:59 NEVER existed.
//   2021 fall-back at 2021-09-21T19:30Z: wall went 23:59 back to 23:00, so
//   2021-09-21 23:00–23:59 happened twice (+04:30 then +03:30).
//   (Same pattern in 2020: gap 2020-03-21 00:xx; 2022: gap 2022-03-22 00:xx,
//   ambiguous 2022-09-21 23:xx — the last DST year.)
section('17. Strict wall-clock validation (reject, never normalize)', () => {
  check('impossible Gregorian dates rejected (not silently normalized)', () => {
    assert.equal(tehranWallClockStringToInstant('2026-02-31 08:30'), null) // Feb 2026 has 28 days
    assert.equal(tehranWallClockStringToInstant('2026-04-31 08:30'), null) // Apr has 30
    assert.equal(tehranWallClockStringToInstant('2026-13-01 08:30'), null) // month 13
    assert.equal(tehranWallClockStringToInstant('2026-00-15 08:30'), null) // month 0
    assert.equal(tehranWallClockStringToInstant('2027-02-29 08:30'), null) // 2027 is not a leap year
    assert.equal(tehranWallClockStringToInstant('2026-11-00 08:30'), null) // day 0
    assert.equal(tehranWallClockStringToInstant('2026-11-32 08:30'), null) // day 32
  })
  check('impossible time components rejected (25:00 / 08:60 / 08:30:60)', () => {
    assert.equal(tehranWallClockStringToInstant('2026-11-17 25:00'), null)
    assert.equal(tehranWallClockStringToInstant('2026-11-17 08:60'), null)
    assert.equal(tehranWallClockStringToInstant('2026-11-17 08:30:60'), null)
    assert.equal(tehranWallClockStringToInstant('2026-11-17 24:00'), null)
  })
  check('valid day-for-month accepted incl. leap 2028-02-29 and month ends', () => {
    assert.equal(iso(tehranWallClockStringToInstant('2028-02-29 12:00')!), '2028-02-29T08:30:00.000Z')
    assert.equal(iso(tehranWallClockStringToInstant('2026-04-30 12:00')!), '2026-04-30T08:30:00.000Z')
    assert.equal(iso(tehranWallClockStringToInstant('2026-02-28 12:00')!), '2026-02-28T08:30:00.000Z')
  })
  check('Esfand 29/30 wall-clock equivalents convert and round-trip exactly', () => {
    // Esfand 30 1403 (leap year) = Gregorian 2025-03-20 — the rare existing 30th
    const e30 = jalaliToMarker(1403, 12, 30)
    assert.equal(markerKey(e30), '2025-03-20')
    const inst = tehranWallClockStringToInstant('2025-03-20 09:15')!
    assert.equal(iso(inst), '2025-03-20T05:45:00.000Z')
    const p = tehranParts(inst)
    assert.deepEqual([p.y, p.m, p.d, p.hh, p.mi, p.ss], [2025, 3, 20, 9, 15, 0])
    // Esfand 29 1404 (non-leap, last day of the year) = Gregorian 2026-03-20
    const e29 = jalaliToMarker(1404, 12, 29)
    assert.equal(markerKey(e29), '2026-03-20')
    const inst2 = tehranWallClockStringToInstant('2026-03-20 23:59')!
    assert.equal(iso(inst2), '2026-03-20T20:29:00.000Z')
    assert.equal(markerKey(addDaysMarker(e29, 1)), markerKey(jalaliToMarker(1405, 1, 1)))
    // the Gregorian day that WOULD be Esfand 30 1404 does not exist — 2026-03-21 is Farvardin 1
    assert.equal(tryJalaliToMarker(1404, 12, 30), null)
    assert.equal(markerKey(jalaliToMarker(1405, 1, 1)), '2026-03-21')
  })
  check('valid historical DST conversions preserved (device/CSV compatible)', () => {
    assert.equal(iso(tehranWallClockStringToInstant('2021-06-15 12:00')!), '2021-06-15T07:30:00.000Z') // DST +04:30
    assert.equal(iso(tehranWallClockStringToInstant('2021-01-15 08:00')!), '2021-01-15T04:30:00.000Z') // winter +03:30
    assert.equal(iso(tehranWallClockStringToInstant('2021-03-21 23:30')!), '2021-03-21T20:00:00.000Z') // last +03:30 minute
    assert.equal(iso(tehranWallClockStringToInstant('2021-03-22 01:30')!), '2021-03-21T21:00:00.000Z') // first +04:30 hour
    assert.equal(iso(tehranWallClockStringToInstant('2026/11/17 2:30')!), '2026-11-16T23:00:00.000Z') // lenient CSV grammar
  })
  check('NONEXISTENT DST wall-clock times rejected (spring-forward gap)', () => {
    assert.equal(tehranWallClockStringToInstant('2021-03-22 00:00'), null)
    assert.equal(tehranWallClockStringToInstant('2021-03-22 00:30'), null)
    assert.equal(tehranWallClockStringToInstant('2021-03-22 00:59'), null)
    assert.equal(tehranWallClockStringToInstant('2020-03-21 00:30'), null) // 2020 gap
    assert.equal(tehranWallClockStringToInstant('2022-03-22 00:30'), null) // 2022 gap (last DST year)
    // the first existing instant of the new offset stays valid
    assert.equal(iso(tehranWallClockStringToInstant('2021-03-22 01:00')!), '2021-03-21T20:30:00.000Z')
  })
  check('AMBIGUOUS DST wall-clock times → deterministic LATER occurrence (documented)', () => {
    const inst = tehranWallClockStringToInstant('2021-09-21 23:30')!
    assert.equal(iso(inst), '2021-09-21T20:00:00.000Z') // post-fallback +03:30 occurrence
    const p = tehranParts(inst)
    assert.deepEqual([p.y, p.m, p.d, p.hh, p.mi, p.ss], [2021, 9, 21, 23, 30, 0]) // round-trip exact
    assert.equal(iso(tehranWallClockStringToInstant('2021-09-21 23:00')!), '2021-09-21T19:30:00.000Z') // the transition instant
    assert.equal(iso(tehranWallClockStringToInstant('2022-09-21 23:30')!), '2022-09-21T20:00:00.000Z') // 2022 fallback
    assert.equal(iso(tehranWallClockStringToInstant('2020-09-20 23:30')!), '2020-09-20T20:00:00.000Z') // 2020 fallback
  })
  check('valid post-2022 (no-DST era) conversions unchanged (regression)', () => {
    assert.equal(iso(tehranWallClockStringToInstant('2026-11-17 02:30:00')!), '2026-11-16T23:00:00.000Z')
    assert.equal(tehranWallClockStringToInstant('2026-11-17 2:5')!.getTime(), tehranWallClockStringToInstant('2026-11-17 02:05')!.getTime())
  })
})

// [18] Strict API day-param parsing (v1.6.0 review fix 2) ----------------------
section('18. Strict API day-param parsing (ISO-with-timezone only)', () => {
  check('date-only strings still accepted (existing API contract, no tz shift)', () => {
    assert.equal(markerKey(parseDayParam('2026-11-17')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-7')!), '2026-11-07')
    assert.equal(markerKey(parseDayParam('2026/11/17')!), '2026-11-17')
    assert.equal(parseDayParam('2026-11-17')!.toISOString(), '2026-11-17T00:00:00.000Z')
  })
  check('full ISO WITH explicit timezone accepted; UTC-day flooring preserved', () => {
    assert.equal(markerKey(parseDayParam('2026-11-17T00:00:00.000Z')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-17T22:30:00Z')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-17T23:59:59.999Z')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-17T22:30:00+00:00')!), '2026-11-17')
    assert.equal(markerKey(parseDayParam('2026-11-17T22:30Z')!), '2026-11-17') // seconds optional (ISO short form)
    // negative offset: 2026-11-16T22:30-05:00 = 2026-11-17T03:30Z
    assert.equal(markerKey(parseDayParam('2026-11-16T22:30:00-05:00')!), '2026-11-17')
    // +14:00 (Kiritimati): 2026-11-17T00:30+14:00 = 2026-11-16T10:30Z
    assert.equal(markerKey(parseDayParam('2026-11-17T00:30:00+14:00')!), '2026-11-16')
    // leap day in a full ISO form
    assert.equal(markerKey(parseDayParam('2028-02-29T12:00:00Z')!), '2028-02-29')
  })
  check('timezone-less datetimes REJECTED (were silently host-TZ-dependent)', () => {
    assert.equal(parseDayParam('2026-11-17T00:30'), null)
    assert.equal(parseDayParam('2026-11-17T00:30:00'), null)
    assert.equal(parseDayParam('2026-11-17T00:30:00.000'), null)
    assert.equal(parseDayParam('2026-11-17 00:30'), null) // space-separated, no offset
  })
  check('malformed/arbitrary strings REJECTED (no Date-engine leniency)', () => {
    assert.equal(parseDayParam('garbage'), null)
    assert.equal(parseDayParam(null), null)
    assert.equal(parseDayParam(''), null)
    assert.equal(parseDayParam('March 5, 2026'), null)
    assert.equal(parseDayParam('17/11/2026'), null)
    assert.equal(parseDayParam('2026'), null) // year-only junk
    assert.equal(parseDayParam('2026-11-17T22:30:00+0330'), null) // basic-format offset — not ISO-extended
    assert.equal(parseDayParam('2026-11-17T25:00:00Z'), null) // hour 25
    assert.equal(parseDayParam('2026-11-17T10:61:00+03:30'), null) // minute 61
    assert.equal(parseDayParam('2026-11-17T10:00:99Z'), null) // second 99
    assert.equal(parseDayParam('2026-11-17T10:00:00+24:00'), null) // offset hour 24
    assert.equal(parseDayParam('2026-02-30T10:00:00Z'), null) // Feb 30
    assert.equal(parseDayParam('2026-13-01T10:00:00Z'), null) // month 13
    assert.equal(parseDayParam('2027-02-29T12:00:00Z'), null) // non-leap Feb 29
  })
})

// [19] Day-param & wall-clock TZ-independence — same input behavior under 4 TZs --
section('19. Day-param & wall-clock TZ-independence (child probes under 4 TZs)', () => {
  const self = path.join('scripts', 'test-time.ts')
  const tsxCli = path.join(ROOT, 'node_modules', 'tsx', 'dist', 'cli.mjs')
  const outputs: Record<string, string> = {}
  for (const tz of ['UTC', 'America/New_York', 'Asia/Tehran', 'Pacific/Kiritimati']) {
    const res = spawnSync(process.execPath, [tsxCli, self, '--probe-day'], {
      cwd: ROOT,
      env: { ...process.env, TZ: tz },
      encoding: 'utf8',
    })
    assert.equal(res.status, 0, `probe-day failed under TZ=${tz}: ${res.stderr?.slice(0, 400)}`)
    outputs[tz] = res.stdout.trim()
    assert.ok(res.stdout.includes('"dayParams"'), `probe-day produced no JSON under TZ=${tz}`)
  }
  check('all four TZs produce IDENTICAL parseDayParam & wall-clock results', () => {
    const base = outputs['UTC']
    for (const [tz, out] of Object.entries(outputs)) {
      assert.equal(out, base, `TZ=${tz} diverged`)
    }
  })
  check('expected values: strict rejects everywhere, flooring identical everywhere', () => {
    const data = JSON.parse(outputs['UTC'])
    const days = Object.fromEntries(data.dayParams.map((x: { k: string; day: string | null }) => [x.k, x.day]))
    assert.equal(days['date-only'], '2026-11-17')
    assert.equal(days['date-only-1d'], '2026-11-07')
    assert.equal(days['iso-Z'], '2026-11-17')
    assert.equal(days['iso-Z-ms'], '2026-11-17')
    assert.equal(days['iso-offset-neg'], '2026-11-17')
    assert.equal(days['iso-offset-pos14'], '2026-11-16')
    assert.equal(days['tz-less'], null)
    assert.equal(days['tz-less-sec'], null)
    assert.equal(days['junk'], null)
    const wall = Object.fromEntries(data.wallClock.map((x: { s: string; iso: string | null }) => [x.s, x.iso]))
    assert.equal(wall['2026-11-17 02:30'], '2026-11-16T23:00:00.000Z')
    assert.equal(wall['2021-06-15 12:00'], '2021-06-15T07:30:00.000Z')
    assert.equal(wall['2021-03-22 00:30'], null) // DST gap — rejected under every TZ
    assert.equal(wall['2021-09-21 23:30'], '2021-09-21T20:00:00.000Z') // ambiguous — deterministic
    assert.equal(wall['2026-02-31 08:30'], null) // impossible date — rejected under every TZ
    assert.equal(wall['2026-11-17 25:00'], null) // impossible time — rejected under every TZ
  })
})

// [20] Persian weekday of markers (S5-A1) -----------------------------------
section('20. Persian weekday of markers (S5): faWeekdayOfMarker + legacy parity', () => {
  // 2026-11-13 is a Friday (جمعه), 11-14 Saturday (شنبه), … 11-19 پنجشنبه
  const cases: Array<[string, number]> = [
    ['2026-11-13', 6],
    ['2026-11-14', 0],
    ['2026-11-15', 1],
    ['2026-11-16', 2],
    ['2026-11-17', 3],
    ['2026-11-18', 4],
    ['2026-11-19', 5],
  ]
  for (const [k, want] of cases) {
    check(`weekday of ${k} = ${want} (faWeekdayOfMarker, UTC getter)`, () => {
      assert.equal(faWeekdayOfMarker(parseDateOnly(k)!), want)
    })
  }
  check('legacy faWeekday === faWeekdayOfMarker on markers (engine values byte-identical on Tehran/UTC hosts)', () => {
    for (const [k] of cases) {
      assert.equal(legacy.faWeekday(parseDateOnly(k)!), faWeekdayOfMarker(parseDateOnly(k)!))
    }
  })
  check('engine weekend detection: 2026-11-13 is Friday (6) — the Iranian weekend marker', () => {
    assert.equal(faWeekdayOfMarker(g(2026, 11, 13)), 6)
    assert.equal(faWeekdayOfMarker(g(2026, 11, 14)), 0) // Saturday is a WORKDAY marker
  })
})

// [21] Strict instant parsing (S5-A3/A4/A5) ---------------------------------
section('21. Strict instant parsing (S5): full ISO with explicit timezone only', () => {
  check('valid: Z / ±HH:MM / fractional seconds → the exact UTC instant', () => {
    assert.equal(iso(parseInstantParam('2026-11-17T08:30:00Z')!), '2026-11-17T08:30:00.000Z')
    assert.equal(iso(parseInstantParam('2026-11-17T08:30:00+03:30')!), '2026-11-17T05:00:00.000Z')
    assert.equal(iso(parseInstantParam('2026-11-17T08:30:00.123-05:00')!), '2026-11-17T13:30:00.123Z')
  })
  check('REJECTED: timezone-less datetimes (ECMA-262 reads them HOST-LOCAL — the S5 trap)', () => {
    assert.equal(parseInstantParam('2026-11-17T00:30'), null)
    assert.equal(parseInstantParam('2026-11-17T00:30:00'), null)
    assert.equal(parseInstantParam('2026-11-17 00:30'), null) // space form — never guessed
  })
  check('REJECTED: date-only strings (a DAY, not an instant), junk, basic offsets, impossible components', () => {
    assert.equal(parseInstantParam('2026-11-17'), null)
    assert.equal(parseInstantParam('garbage'), null)
    assert.equal(parseInstantParam('2026-11-17T22:30:00+0330'), null)
    assert.equal(parseInstantParam('2026-11-17T25:00:00Z'), null)
    assert.equal(parseInstantParam('2026-02-30T10:00:00Z'), null)
    assert.equal(parseInstantParam('2026-11-17T10:00:00+24:00'), null)
    assert.equal(parseInstantParam(''), null)
    assert.equal(parseInstantParam(null), null)
    assert.equal(parseInstantParam(undefined), null)
  })
})

// [22] Tehran day windows (S5-A6/A7) ----------------------------------------
section('22. Tehran day windows (S5): today window + read-path range window', () => {
  check('tehranTodayWindow brackets now and carries the Tehran day of now (kiosk/me window)', () => {
    const now = new Date()
    const w = tehranTodayWindow()
    assert.ok(w.start.getTime() <= now.getTime() && now.getTime() < w.end.getTime())
    assert.equal(markerKey(dayMarkerOf(w.start)), markerKey(dayMarkerOf(now)))
    assert.equal(markerKey(dayMarkerOf(w.end)), markerKey(addDaysMarker(dayMarkerOf(now), 1)))
  })
  check('day window boundaries are true Tehran midnights (20:30Z, no-DST era)', () => {
    const w = tehranDayWindowOfMarker(g(2026, 11, 17))
    assert.equal(w.start.toISOString(), '2026-11-16T20:30:00.000Z')
    assert.equal(w.end.toISOString(), '2026-11-17T20:30:00.000Z')
  })
  check('tehranDayRangeInstantWindow(from,to) = [Tehran midnight of from, Tehran midnight of to+1) — the engine fetch window (D1 retired)', () => {
    const w = tehranDayRangeInstantWindow(g(2026, 11, 14), g(2026, 11, 17))
    assert.equal(w.gte.toISOString(), '2026-11-13T20:30:00.000Z')
    assert.equal(w.lt.toISOString(), '2026-11-17T20:30:00.000Z')
    // identical to the engine's own recalcRange fetch span construction
    const engineFrom = tehranDayWindowOfMarker(g(2026, 11, 14)).start
    const engineTo = tehranDayWindowOfMarker(addDaysMarker(g(2026, 11, 17), 1)).start
    assert.equal(w.gte.getTime(), engineFrom.getTime())
    assert.equal(w.lt.getTime(), engineTo.getTime())
  })
  check('DST-era day windows stay exact (+04:30 summers, real tz database)', () => {
    const w = tehranDayWindowOfMarker(g(2021, 6, 21))
    assert.equal(w.start.toISOString(), '2021-06-20T19:30:00.000Z')
    assert.equal(w.end.toISOString(), '2021-06-21T19:30:00.000Z')
  })
  check('kiosk/me direction semantics: Tehran 00:30 of day D inside, 00:30 of day D+1 outside', () => {
    const w = tehranDayWindowOfMarker(g(2026, 11, 17))
    const earlyToday = tehranInstant(2026, 11, 17, 0, 30)
    const earlyNext = tehranInstant(2026, 11, 18, 0, 30)
    assert.ok(earlyToday.getTime() >= w.start.getTime() && earlyToday.getTime() < w.end.getTime())
    assert.ok(earlyNext.getTime() >= w.end.getTime())
  })
})

// [23] Engine core S5 scenarios on fixed instants ----------------------------
section('23. Engine core S5 scenarios (fixed instants): night / multi-segment / stray / Jalali boundary / Late & Early / mission', () => {
  const T = (y: number, mo: number, d: number, h: number, mi = 0) => tehranInstant(y, mo, d, h, mi)
  const mkShift = (
    id: string,
    segs: Array<{ startMin: number; endMin: number }>,
    graces = 10,
  ): ShiftLite => ({
    id,
    name: id,
    lateGraceMin: graces,
    earlyGraceMin: graces,
    overtimeEligible: true,
    segments: segs.map((s) => ({ startMin: s.startMin, endMin: s.endMin, breakMin: 0 })),
  })
  const dayShift = mkShift('day', [{ startMin: 8 * 60, endMin: 16 * 60 }])
  const eveningShift = mkShift('evening', [{ startMin: 15 * 60, endMin: 23 * 60 + 30 }])
  const nightShift = mkShift('night', [{ startMin: 22 * 60, endMin: 6 * 60 }])
  const splitShift = mkShift('split', [{ startMin: 8 * 60, endMin: 12 * 60 }, { startMin: 13 * 60, endMin: 17 * 60 }], 0)
  const run = (
    day: Date,
    shift: ShiftLite | null,
    punches: Array<{ timestamp: Date; direction?: string }>,
    extra?: { missionSpans?: Array<{ start: Date; end: Date }>; leaveSpans?: Array<{ start: Date; end: Date; leaveTypeId: string }>; isHoliday?: boolean },
  ) =>
    computeDaily({
      personId: 'p1',
      day,
      shift,
      punches,
      missionSpans: extra?.missionSpans ?? [],
      leaveSpans: extra?.leaveSpans ?? [],
      isHoliday: extra?.isHoliday ?? false,
      isOff: false,
      now: new Date(Date.UTC(2026, 11, 1, 12, 0)),
    })

  check('E1 day shift: 08:20 IN / 15:30 OUT → PRESENT, late 10, early 20 (graces 10)', () => {
    const d = run(g(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 8, 20), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 15, 30), direction: 'OUT' },
    ])
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.lateMinutes, 10)
    assert.equal(d.earlyLeaveMinutes, 20)
    assert.equal(d.workedMinutes, 430)
  })
  check('E2 night shift Sat 22:00→Sun 06:00 on SATURDAY\'s row: worked 480, window exact across midnight', () => {
    const d = run(g(2026, 11, 14), nightShift, [
      { timestamp: T(2026, 11, 14, 22, 0), direction: 'IN' },
      { timestamp: T(2026, 11, 15, 6, 0), direction: 'OUT' },
    ])
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 480)
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
  })
  check('E3 S4 hotfix parity: stray 02:30–02:50 pair + real 15:30–20:00 on a 15:00–23:30 shift → late 20, early 200', () => {
    const d = run(g(2026, 11, 17), eveningShift, [
      { timestamp: T(2026, 11, 17, 2, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 2, 50), direction: 'OUT' },
      { timestamp: T(2026, 11, 17, 15, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 20, 0), direction: 'OUT' },
    ])
    assert.equal(d.lateMinutes, 20)
    assert.equal(d.earlyLeaveMinutes, 200)
  })
  check('E4 Jalali year boundary: night 1404/12/29 22:00 → 1405/01/01 06:00 on Esfand-29\'s row, worked 480', () => {
    const d = run(g(2026, 3, 20), nightShift, [
      { timestamp: T(2026, 3, 20, 22, 0), direction: 'IN' },
      { timestamp: T(2026, 3, 21, 6, 0), direction: 'OUT' },
    ])
    assert.equal(d.workedMinutes, 480)
    assert.equal(d.status, 'PRESENT')
    // the boundary markers themselves: 2026-03-20 = 1404/12/29, 2026-03-21 = 1405/01/01
    const a = jalaliOfMarker(g(2026, 3, 20))
    const b = jalaliOfMarker(g(2026, 3, 21))
    assert.deepEqual([a.jy, a.jm, a.jd], [1404, 12, 29])
    assert.deepEqual([b.jy, b.jm, b.jd], [1405, 1, 1])
  })
  check('E5 leave attached to the window start (08:00–10:00) excuses lateness: IN 09:30 → late 0', () => {
    const d = run(g(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 9, 30), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 0), direction: 'OUT' },
    ], { leaveSpans: [{ start: T(2026, 11, 17, 8, 0), end: T(2026, 11, 17, 10, 0), leaveTypeId: 'lt1' }] })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.leaveMinutes, 120)
  })
  check('E6 approved mission without punches → MISSION status, 480 credited', () => {
    const d = run(g(2026, 11, 17), dayShift, [], {
      missionSpans: [{ start: T(2026, 11, 17, 8, 0), end: T(2026, 11, 17, 16, 0) }],
    })
    assert.equal(d.status, 'MISSION')
    assert.equal(d.missionMinutes, 480)
  })
  check('E7 multi-segment shift (08–12 + 13–17, graces 0): anchors at first-segment start / last-segment end', () => {
    const d = run(g(2026, 11, 17), splitShift, [
      { timestamp: T(2026, 11, 17, 8, 1), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 40), direction: 'OUT' },
    ])
    assert.equal(d.lateMinutes, 1)
    assert.equal(d.earlyLeaveMinutes, 20)
  })
  check('E8 no punches after the determination window → ABSENT', () => {
    const d = run(g(2026, 11, 17), dayShift, [])
    assert.equal(d.status, 'ABSENT')
  })
  check('E9 holiday work: all valid time counts as actual OT (A4 — no gate), payable 0 without approval', () => {
    const d = run(g(2026, 11, 17), dayShift, [
      { timestamp: T(2026, 11, 17, 8, 0), direction: 'IN' },
      { timestamp: T(2026, 11, 17, 16, 0), direction: 'OUT' },
    ], { isHoliday: true })
    assert.equal(d.overtimeMinutes, 480)
    assert.equal(d.payableOvertimeMinutes, 0)
  })
})

// [24] The S5 TZ matrix — REAL child runs under the four mandated host TZs ----
section('24. S5 host-timezone independence: UTC / Asia/Tehran / Europe/Vienna / America/New_York', () => {
  const self = path.join('scripts', 'test-time.ts')
  const tsxCli = path.join(ROOT, 'node_modules', 'tsx', 'dist', 'cli.mjs')
  const outputs: Record<string, string> = {}
  for (const tz of ['UTC', 'Asia/Tehran', 'Europe/Vienna', 'America/New_York']) {
    const res = spawnSync(process.execPath, [tsxCli, self, '--probe-s5'], {
      cwd: ROOT,
      env: { ...process.env, TZ: tz },
      encoding: 'utf8',
    })
    assert.equal(res.status, 0, `probe-s5 failed under TZ=${tz}: ${res.stderr?.slice(0, 400)}`)
    outputs[tz] = res.stdout.trim()
    assert.ok(res.stdout.includes('"engine"'), `probe-s5 produced no engine JSON under TZ=${tz}`)
  }
  check('ALL FOUR host TZs produce BYTE-IDENTICAL S5 output (parsers, weekday, windows, engine, attribution)', () => {
    const base = outputs['UTC']
    for (const [tz, out] of Object.entries(outputs)) {
      assert.equal(out, base, `TZ=${tz} diverged from UTC`)
    }
  })
  check('todayWindow internal consistency holds under every host TZ', () => {
    for (const [tz, out] of Object.entries(outputs)) {
      assert.equal(JSON.parse(out).todayOk, true, `todayOk failed under TZ=${tz}`)
    }
  })
  check('strict instant parser: identical accepts/rejects under every host TZ', () => {
    for (const out of Object.values(outputs)) {
      const data = JSON.parse(out)
      const inst = Object.fromEntries(data.instants.map((x: { k: string; iso: string | null }) => [x.k, x.iso]))
      assert.equal(inst['iso-Z'], '2026-11-17T08:30:00.000Z')
      assert.equal(inst['iso-Z-ms'], '2026-11-17T08:30:00.123Z')
      assert.equal(inst['iso-pos330'], '2026-11-17T05:00:00.000Z')
      assert.equal(inst['iso-neg5'], '2026-11-17T13:30:00.000Z')
      assert.equal(inst['tz-less'], null)
      assert.equal(inst['tz-less-sec'], null)
      assert.equal(inst['date-only'], null)
      assert.equal(inst['basic-offset'], null)
      assert.equal(inst['hour25'], null)
      assert.equal(inst['feb30'], null)
      assert.equal(inst['junk'], null)
    }
  })
  check('marker weekday: identical values under every host TZ (legacy parity incl.)', () => {
    for (const out of Object.values(outputs)) {
      const data = JSON.parse(out)
      const wd = Object.fromEntries(data.weekdays.map((x: { k: string; central: number; legacy: number }) => [x.k, x.legacy === x.central ? `${x.central}` : 'DIVERGED']))
      assert.equal(wd['2026-11-13'], '6')
      assert.equal(wd['2026-11-14'], '0')
      assert.equal(wd['2026-11-15'], '1')
      assert.equal(wd['2026-11-16'], '2')
      assert.equal(wd['2026-11-17'], '3')
      assert.equal(wd['2026-11-18'], '4')
      assert.equal(wd['2026-11-19'], '5')
    }
  })
  check('day windows & read-path range: identical Tehran-midnight instants under every host TZ', () => {
    for (const out of Object.values(outputs)) {
      const w = JSON.parse(out).windows
      assert.equal(w.dayStart, '2026-11-16T20:30:00.000Z')
      assert.equal(w.dayEnd, '2026-11-17T20:30:00.000Z')
      assert.equal(w.rangeGte, '2026-11-13T20:30:00.000Z')
      assert.equal(w.rangeLt, '2026-11-17T20:30:00.000Z')
    }
  })
  check('engine scenarios: identical statuses/minutes under every host TZ', () => {
    for (const out of Object.values(outputs)) {
      const e = Object.fromEntries(JSON.parse(out).engine.map((x: { k: string }) => [x.k, x]))
      assert.equal(e['E1-day-0820-1530'].status, 'PRESENT')
      assert.equal(e['E1-day-0820-1530'].late, 10)
      assert.equal(e['E1-day-0820-1530'].early, 20)
      assert.equal(e['E2-night-sat'].worked, 480)
      assert.equal(e['E3-stray-pair'].late, 20)
      assert.equal(e['E3-stray-pair'].early, 200)
      assert.equal(e['E4-esfand-farvardin'].worked, 480)
      assert.equal(e['E5-leave-start'].late, 0)
      assert.equal(e['E5-leave-start'].leave, 120)
      assert.equal(e['E6-mission'].status, 'MISSION')
      assert.equal(e['E6-mission'].mission, 480)
      assert.equal(e['E7-split'].late, 1)
      assert.equal(e['E7-split'].early, 20)
      assert.equal(e['E8-absent'].status, 'ABSENT')
      assert.equal(e['E9-holiday-ot'].ot, 480)
    }
  })
  check('workday attribution: identical workdays/reasons under every host TZ (night-shift policy)', () => {
    for (const out of Object.values(outputs)) {
      const a = Object.fromEntries(JSON.parse(out).attribution.map((x: { k: string }) => [x.k, x]))
      assert.equal(a['sun-0230-in'].workday, '2026-11-14') // the S3 policy example — Sunday 02:30 → SATURDAY
      assert.equal(a['sun-0230-in'].reason, 'SHIFT_WINDOW')
      assert.equal(a['sun-0600-out'].workday, '2026-11-14') // direction-aware window end
      assert.equal(a['sun-0600-out'].reason, 'SHIFT_WINDOW')
      assert.equal(a['sun-0600-in'].workday, '2026-11-15') // IN at the boundary opens the next day — the Sat night window is half-open for IN; no Sunday shift, a valid Sat window simply does not contain it → documented OUT_OF_WINDOW fallback to the Tehran day
      assert.equal(a['sun-0600-in'].reason, 'OUT_OF_WINDOW')
      assert.equal(a['sun-1000-auto'].workday, '2026-11-15')
      assert.equal(a['sun-1000-auto'].reason, 'OUT_OF_WINDOW')
      assert.equal(a['sat-2300-in'].workday, '2026-11-14')
      assert.equal(a['sat-2300-in'].reason, 'SHIFT_WINDOW')
    }
  })
})

console.log(`\n== Time assertions: ${assertionsPassed} passed / ${assertionsFailed} failed ==`)
console.log(`== Time sections: ${sectionsPassed} passed / ${sectionsFailed} failed ==`)
if (assertionsFailed > 0 || sectionsFailed > 0) process.exitCode = 1