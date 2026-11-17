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
  tehranDayWindowOfMarker,
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

const ROOT = path.resolve(__dirname, '..')

// --- probe mode (used by the TZ-independence section): fixed instants only ---
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

let passed = 0
let failed = 0
function section(name: string, fn: () => void) {
  const before = failed
  try {
    fn()
    console.log(`  ✔ ${name}`)
  } catch (err) {
    console.log(`  ✘ ${name}`)
    console.log(`    ${(err as Error)?.message ?? err}`)
  } finally {
    console.log(`— ${name}: ${failed === before ? 'OK' : 'FAILED'}`)
    if (failed === before) passed++
    else failed++
  }
}
function check(name: string, fn: () => void) {
  try {
    fn()
    passed++
    console.log(`  ✔ ${name}`)
  } catch (err) {
    failed++
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
  check('wall-clock-string parse ("YYYY-MM-DD HH:MM[:SS]")', () => {
    const d = tehranWallClockStringToInstant('2026-11-17 02:30:00')
    assert.equal(iso(d!), '2026-11-16T23:00:00.000Z')
    // seconds optional — HH:MM equals HH:MM:00
    assert.equal(tehranWallClockStringToInstant('2026-11-17 02:30')!.getTime(), d!.getTime())
    assert.equal(tehranWallClockStringToInstant('garbage'), null)
    assert.equal(tehranWallClockStringToInstant('2026/11/17 02:30:00'), null) // strict grammar
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

console.log(`\n== ${passed} passed, ${failed} failed ==`)
if (failed > 0) process.exitCode = 1
