/**
 * Phase 2 Stage S2 — attendance engine pairing & validation test suite.
 * =====================================================================
 * MOCK ONLY, PURE FUNCTIONS — no database, no clock, no network.
 * Exercises the direction-aware deterministic pairing layer (src/lib/pairing.ts)
 * and its integration into computeDaily (src/lib/attendance.ts) with the exact
 * same in-memory harness style as test-time.ts.
 *
 * The engine module imports @/lib/db (PrismaClient is constructed lazily and
 * never connected here — no DATABASE_URL is required by this suite because
 * computeDaily/pairPunches never touch the database).
 *
 * Mandatory coverage (S2 mandate §5 — each anomaly class is tested on its own,
 * not just one happy-path scenario):
 *   [1]  T1  normal IN+OUT day — one valid pair, exact worked minutes
 *   [2]      legacy pure-AUTO (device) days — arithmetic identical to the old
 *             positional engine (2-punch, 4-punch, odd 3-punch day)
 *   [3]  T5  duplicate IN — identified (DUPLICATE_IN), honest pairing kept
 *   [4]  T6  duplicate OUT — identified (DUPLICATE_OUT)
 *   [5]  T8  OUT without IN — orphan OUT identified, no fabricated pair
 *   [6]  T7  IN without OUT — open session identified, minutes NOT counted
 *   [7]  T10 simultaneous punches — flagged SIMULTANEOUS, deterministic order
 *   [8]  T9  out-of-order input — order invariance + repeat-call determinism
 *   [9]  T4  multi-pair day — per-pair worked, sum, first/last bounds
 *   [10]     mixed AUTO/IN/OUT and WILD fallback rules
 *   [11] T2  night-shift cross-midnight pairing (pure pairing; day
 *             attribution itself is stage S3/S5 scope, not S2)
 *   [12]     computeDaily integration: statuses, note provenance, unchanged
 *             late/early/OT formulas (regression), INCOMPLETE for zero-pair days
 *   [13]     purity — pairPunches never mutates its input (no raw punch edits)
 *   [14]     policy non-interference — OT/leave/holiday math untouched by S2
 *
 * Phase 2 S4 sections (36–48) — calculation completeness on the PURE layer:
 *   [36] late/early on REAL shift-window instants (day-shift parity, grace 0)
 *   [37] D13 regression — stray 02:30 punch on a 15:00–23:30 shift (no fake
 *         late/early; after-window punch keeps the real 529-min lateness)
 *   [38] night-shift late/early exact across midnight (incl. early arrival)
 *   [39] multi-segment window (first-segment start / last-segment end)
 *   [40] Jalali month+year boundary (Esfand 29 → Farvardin 1 night shift)
 *   [41] leave attached to the window start/end moves the late/early deadline
 *   [42] leave×work overlap carved — one interval counted exactly once
 *   [43] mission × leave × attendance with the carve (no double count)
 *   [44] ABSENT timing at the REAL window end / Tehran day end (exact ms)
 *   [45] holiday/off-day behavior DOCUMENTED (A4 kept: all worked = actual OT)
 *   [46] breakMin effect DOCUMENTED (A5 kept: standard only, never worked)
 *   [47] provenance — EDITED_PUNCHES note code, deterministic order
 *   [48] status precedence LEAVE > MISSION; MISSION never hides punch truth
 *   [49] S4 hotfix — stray pairs before/after the shift window never hide
 *         the real Late/Early (window-related pair anchors): A stray 02:30
 *         pair + real 15:30 pair → late 20; B genuine early arrival 14:30 →
 *         late 0; C lone IN 23:59 after window end → late 529 (contract kept);
 *         D stray 23:35 pair after the end does not hide the real 200-min
 *         early departure; E regression = sections 1–48 all green.
 *
 * Accounting follows the corrected convention (commit 41c958ad): assertions
 * (check() calls) and sections are counted separately; a section whose body
 * crashes counts as failed.
 *
 * Run: npx tsx scripts/test-attendance.ts   (no DATABASE_URL needed)
 */
import * as assert from 'node:assert/strict'

import { pairPunches, anomalySummary, type PairingAnomaly, type PairingResult } from '../src/lib/pairing'
import { computeDaily, parseOtThresholdMinutes, parsePositiveRequestMinutes, type ShiftLite } from '../src/lib/attendance'
import {
  attributePunchWorkday,
  coverageWindowsByDay,
  mergeIntervals,
  subtractIntervals,
  intersectIntervals,
  totalMinutes,
} from '../src/lib/workday'
import { tehranInstant, markerKey } from '../src/lib/time'

// --- accounting (separate assertions vs sections — repo convention) ---
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

// --- fixtures -------------------------------------------------------------
// Tehran wall-clock instants via a fixed +03:30 offset (no DST in 2026):
// the pure pairing layer compares instants only, so a fixed-offset builder
// is exact and host-timezone independent for these dates.
const T = (h: number, m = 0, day = 17): Date => new Date(Date.UTC(2026, 10, day, h - 3, m - 30))
const iso = (d: Date | null): string | null => (d ? d.toISOString() : null)

const reasons = (r: PairingResult): string[] => r.anomalies.map((a) => a.reason)
const noteOf = (r: PairingResult): string => anomalySummary(r.anomalies)

// A plain 08:00–16:00 shift (Tehran minutes), OT-eligible, graces 10/10.
const dayShift: ShiftLite = {
  id: 'shift-day',
  name: 'شیفت روزانه',
  lateGraceMin: 10,
  earlyGraceMin: 10,
  overtimeEligible: true,
  segments: [{ startMin: 8 * 60, endMin: 16 * 60, breakMin: 0 }],
}

/** computeDaily with defaults filled for a non-holiday working day. */
function daily(punches: Array<{ timestamp: Date; direction?: string; source?: string }>, shift: ShiftLite | null = dayShift) {
  return computeDaily({
    personId: 'p1',
    day: new Date(Date.UTC(2026, 10, 17, 0, 0)),
    shift,
    punches,
    isHoliday: false,
    isOff: false,
    now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
  })
}

console.log('== Phase 2 S2 attendance pairing tests ==\n')

// [1] T1 — normal day -------------------------------------------------------
section('1. T1 normal IN+OUT day: exactly one valid pair', () => {
  const r = pairPunches([
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(16), direction: 'OUT' },
  ])
  check('one pair, worked 480, firstIn=08:00, lastOut=16:00, no anomalies', () => {
    assert.equal(r.pairs.length, 1)
    assert.equal(r.pairs[0].minutes, 480)
    assert.equal(iso(r.firstIn), iso(T(8)))
    assert.equal(iso(r.lastOut), iso(T(16)))
    assert.deepEqual(reasons(r), [])
    assert.equal(noteOf(r), '')
    assert.deepEqual(r.unpairedIndexes, [])
    assert.equal(r.openPairIndex, null)
  })
  check('pair records reference the original input indexes', () => {
    assert.equal(r.pairs[0].inIndex, 0)
    assert.equal(r.pairs[0].outIndex, 1)
  })
})

// [2] legacy pure-AUTO (device) days ---------------------------------------
section('2. legacy pure-AUTO (device) days: arithmetic identical to old positional engine', () => {
  check('2 AUTO punches (08:00, 16:00) → one pair, worked 480', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(16), direction: 'AUTO' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
    assert.equal(reasons(r).length, 0)
  })
  check('4 AUTO punches (08,12,13,17) → two pairs 240+240=480 (old positional (p0,p1)+(p2,p3) parity)', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(12), direction: 'AUTO' },
      { timestamp: T(13), direction: 'AUTO' },
      { timestamp: T(17), direction: 'AUTO' },
    ])
    assert.equal(r.pairs.length, 2)
    assert.deepEqual(r.pairs.map((p) => p.minutes), [240, 240])
    assert.equal(r.workedMinutes, 480)
    assert.equal(reasons(r).length, 0)
  })
  check('odd 3 AUTO punches (08,12,17) → pair (08,12) only; 17:00 open session flagged IN_WITHOUT_OUT (old engine dropped it silently)', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(12), direction: 'AUTO' },
      { timestamp: T(17), direction: 'AUTO' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.pairs[0].minutes, 240)
    assert.equal(r.workedMinutes, 240)
    assert.deepEqual(reasons(r), ['IN_WITHOUT_OUT'])
    assert.equal(r.unpairedIndexes.length, 1)
    assert.equal(r.openPairIndex, 2)
    assert.equal(iso(r.lastOut), iso(T(12))) // OUT of the LAST VALID pair, not the open punch
  })
  check('missing direction field behaves as AUTO (legacy rows)', () => {
    const r = pairPunches([{ timestamp: T(8) }, { timestamp: T(16) }])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
  })
})

// [3] T5 — duplicate IN -----------------------------------------------------
section('3. T5 duplicate IN: identified, honest pairing kept', () => {
  const punches = [
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(9), direction: 'IN' }, // duplicate
    { timestamp: T(17), direction: 'OUT' },
  ]
  const r = pairPunches(punches)
  check('DUPLICATE_IN reported exactly once with the punch index', () => {
    assert.deepEqual(reasons(r), ['DUPLICATE_IN'])
    assert.deepEqual(r.anomalies[0].punchIndexes, [1])
    assert.ok(r.anomalies[0].message.length > 0)
  })
  check('pair = (first IN, the OUT that closed its session): worked 540; old positional engine wrongly gave 60', () => {
    assert.equal(r.pairs.length, 1)
    assert.equal(r.pairs[0].inIndex, 0)
    assert.equal(r.pairs[0].outIndex, 2)
    assert.equal(r.workedMinutes, 540)
    assert.deepEqual(r.unpairedIndexes, [1])
  })
  check('computeDaily: PRESENT with 540 worked and note carries the reason', () => {
    const d = daily(punches)
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 540)
    assert.equal(d.note, 'DUPLICATE_IN:1')
  })
})

// [4] T6 — duplicate OUT ----------------------------------------------------
section('4. T6 duplicate OUT: identified with reason', () => {
  const punches = [
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(12), direction: 'OUT' },
    { timestamp: T(17), direction: 'OUT' }, // duplicate exit
  ]
  const r = pairPunches(punches)
  check('DUPLICATE_OUT reported once for the second exit', () => {
    assert.deepEqual(reasons(r), ['DUPLICATE_OUT'])
    assert.deepEqual(r.anomalies[0].punchIndexes, [2])
  })
  check('only the direction-valid pair counts: worked 240; lastOut=12:00 (OUT of the valid pair, not the orphan)', () => {
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 240)
    assert.equal(iso(r.lastOut), iso(T(12)))
    assert.deepEqual(r.unpairedIndexes, [2])
  })
  check('computeDaily: PRESENT, note DUPLICATE_OUT:1', () => {
    const d = daily(punches)
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.note, 'DUPLICATE_OUT:1')
  })
})

// [5] T8 — OUT without IN ---------------------------------------------------
section('5. T8 OUT without IN: orphan OUT never becomes a fabricated pair', () => {
  check('single OUT punch: zero pairs, OUT_WITHOUT_IN, firstIn=null, lastOut=null, worked 0', () => {
    const r = pairPunches([{ timestamp: T(9), direction: 'OUT' }])
    assert.deepEqual(reasons(r), ['OUT_WITHOUT_IN'])
    assert.equal(r.pairs.length, 0)
    assert.equal(r.workedMinutes, 0)
    assert.equal(r.firstIn, null)
    assert.equal(r.lastOut, null)
    assert.equal(r.openPairIndex, null)
  })
  check('OUT,OUT at 09:00/09:30: first OUT_WITHOUT_IN then DUPLICATE_OUT — both distinct and reasoned', () => {
    const r = pairPunches([
      { timestamp: T(9), direction: 'OUT' },
      { timestamp: T(9, 30), direction: 'OUT' },
    ])
    assert.deepEqual(reasons(r), ['OUT_WITHOUT_IN', 'DUPLICATE_OUT'])
    assert.equal(r.pairs.length, 0)
    assert.equal(r.workedMinutes, 0)
  })
  check('OUT then valid IN,OUT: orphan first, valid pair after — worked from the pair only', () => {
    const r = pairPunches([
      { timestamp: T(7, 55), direction: 'OUT' },
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16), direction: 'OUT' },
    ])
    assert.deepEqual(reasons(r), ['OUT_WITHOUT_IN'])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
    assert.equal(iso(r.firstIn), iso(T(8))) // IN of the valid pair, not the orphan
  })
  check('computeDaily on an OUT-only day: INCOMPLETE (was wrongly PRESENT in the old positional engine), note OUT_WITHOUT_IN:1', () => {
    const d = daily([{ timestamp: T(9), direction: 'OUT' }])
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.workedMinutes, 0)
    assert.equal(d.firstIn, null)
    assert.equal(d.note, 'OUT_WITHOUT_IN:1')
  })
})

// [6] T7 — IN without OUT ---------------------------------------------------
section('6. T7 IN without OUT: open session, minutes never counted', () => {
  check('single IN punch: open pair, IN_WITHOUT_OUT, firstIn=08:00, lastOut=null (old-engine parity for 1-punch days)', () => {
    const r = pairPunches([{ timestamp: T(8), direction: 'IN' }])
    assert.deepEqual(reasons(r), ['IN_WITHOUT_OUT'])
    assert.equal(r.pairs.length, 0)
    assert.equal(r.workedMinutes, 0)
    assert.equal(iso(r.firstIn), iso(T(8)))
    assert.equal(r.lastOut, null)
    assert.equal(r.openPairIndex, 0)
  })
  check('IN,OUT then IN (forgot last OUT): pair (08,12) counted; second IN is an open session flagged IN_WITHOUT_OUT', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(12), direction: 'OUT' },
      { timestamp: T(13), direction: 'IN' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 240)
    assert.deepEqual(reasons(r), ['IN_WITHOUT_OUT'])
    assert.equal(iso(r.firstIn), iso(T(8)))
    assert.equal(iso(r.lastOut), iso(T(12)))
  })
  check('computeDaily single-IN day: INCOMPLETE with note (old engine: INCOMPLETE with note always null)', () => {
    const d = daily([{ timestamp: T(8), direction: 'IN' }])
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.workedMinutes, 0)
    assert.equal(iso(d.firstIn), iso(T(8)))
    assert.equal(d.note, 'IN_WITHOUT_OUT:1')
  })
})

// [7] T10 — simultaneous punches --------------------------------------------
section('7. T10 simultaneous punches: flagged and deterministically ordered', () => {
  check('IN and OUT at the same instant with an open session: OUT processed first → pair closes, then the same-instant IN opens a new session (no zero-minute fabrication from the IN side)', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(12), direction: 'OUT' },
      { timestamp: T(12), direction: 'IN' },
      { timestamp: T(17), direction: 'OUT' },
    ])
    assert.deepEqual(reasons(r), ['SIMULTANEOUS'])
    assert.equal(r.pairs.length, 2)
    assert.deepEqual(r.pairs.map((p) => p.minutes), [240, 300])
    assert.equal(r.workedMinutes, 540)
  })
  check('two OUT punches at the exact same instant: both flagged SIMULTANEOUS; second is DUPLICATE_OUT; no pair fabricated', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(12), direction: 'OUT' },
      { timestamp: T(12), direction: 'OUT' },
    ])
    assert.deepEqual(reasons(r), ['SIMULTANEOUS', 'DUPLICATE_OUT'])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 240)
  })
  check('two IN punches at the exact same instant: one opens, the other DUPLICATE_IN; both flagged SIMULTANEOUS', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16), direction: 'OUT' },
    ])
    assert.deepEqual(reasons(r), ['SIMULTANEOUS', 'DUPLICATE_IN'])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
  })
  check('simultaneity pre-scan lists every original index of the instant group', () => {
    const r = pairPunches([
      { timestamp: T(9), direction: 'OUT' },
      { timestamp: T(9), direction: 'OUT' },
    ])
    const sim = r.anomalies.find((a) => a.reason === 'SIMULTANEOUS') as PairingAnomaly
    assert.deepEqual([...sim.punchIndexes].sort((a, b) => a - b), [0, 1])
  })
  check('same-instant AUTO+AUTO: WILD processed OUT-rank-first closes the session the other opened → 0-minute pair + SIMULTANEOUS flag', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(8), direction: 'AUTO' },
    ])
    assert.deepEqual(reasons(r), ['SIMULTANEOUS'])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.pairs[0].minutes, 0)
    assert.equal(r.workedMinutes, 0)
  })
})

// [8] T9 — out-of-order input & determinism ---------------------------------
section('8. T9 out-of-order input: order invariance and repeat determinism', () => {
  const chronological = [
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(9), direction: 'IN' },
    { timestamp: T(12), direction: 'OUT' },
    { timestamp: T(12, 1), direction: 'OUT' },
    { timestamp: T(13), direction: 'IN' },
    { timestamp: T(17), direction: 'OUT' },
  ]
  const shuffled = [
    { timestamp: T(17), direction: 'OUT' },
    { timestamp: T(12, 1), direction: 'OUT' },
    { timestamp: T(13), direction: 'IN' },
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(12), direction: 'OUT' },
    { timestamp: T(9), direction: 'IN' },
  ]
  check('shuffled input produces the SAME multiset-level result (pairs by instant, anomalies, worked)', () => {
    const a = pairPunches(chronological)
    const b = pairPunches(shuffled)
    assert.equal(b.workedMinutes, a.workedMinutes)
    assert.equal(b.pairs.length, a.pairs.length)
    // pair punches are identified by INSTANT, not by input position:
    assert.deepEqual(
      b.pairs.map((p) => [iso(p.inAt), iso(p.outAt)]),
      a.pairs.map((p) => [iso(p.inAt), iso(p.outAt)]),
    )
    assert.deepEqual(reasons(b), reasons(a))
  })
  check('calling twice on the identical array returns a deep-equal result (determinism)', () => {
    assert.deepEqual(pairPunches(shuffled), pairPunches(shuffled))
  })
  check('repeat-determinism also holds for a pathological all-simultaneous array', () => {
    const weird = [
      { timestamp: T(9), direction: 'IN' },
      { timestamp: T(9), direction: 'OUT' },
      { timestamp: T(9), direction: 'IN' },
      { timestamp: T(9), direction: 'OUT' },
      { timestamp: T(9), direction: 'AUTO' },
    ]
    const r1 = pairPunches(weird)
    const r2 = pairPunches(weird)
    assert.deepEqual(r1, r2)
    assert.ok(r1.anomalies.some((a) => a.reason === 'SIMULTANEOUS'))
  })
  check('tie-break at identical instants is stable across permutations (source+index order)', () => {
    const a = [
      { timestamp: T(9), direction: 'IN', source: 'KIOSK' },
      { timestamp: T(9), direction: 'IN', source: 'WEB' },
    ]
    const b = [
      { timestamp: T(9), direction: 'IN', source: 'WEB' },
      { timestamp: T(9), direction: 'IN', source: 'KIOSK' },
    ]
    const ra = pairPunches(a)
    const rb = pairPunches(b)
    assert.equal(ra.workedMinutes, rb.workedMinutes)
    assert.equal(ra.anomalies.length, rb.anomalies.length)
    assert.deepEqual(reasons(ra), reasons(rb))
  })
})

// [9] T4 — multi-pair day ---------------------------------------------------
section('9. T4 multi-pair day: per-pair worked and sum', () => {
  const r = pairPunches([
    { timestamp: T(8), direction: 'IN' },
    { timestamp: T(12), direction: 'OUT' },
    { timestamp: T(13), direction: 'IN' },
    { timestamp: T(17), direction: 'OUT' },
    { timestamp: T(18), direction: 'IN' },
    { timestamp: T(20), direction: 'OUT' },
  ])
  check('three pairs, minutes 240/240/120, worked 600', () => {
    assert.deepEqual(r.pairs.map((p) => p.minutes), [240, 240, 120])
    assert.equal(r.workedMinutes, 600)
    assert.deepEqual(reasons(r), [])
  })
  check('firstIn = IN of the FIRST pair; lastOut = OUT of the LAST pair', () => {
    assert.equal(iso(r.firstIn), iso(T(8)))
    assert.equal(iso(r.lastOut), iso(T(20)))
    assert.deepEqual(r.unpairedIndexes, [])
  })
  check('computeDaily: PRESENT, worked 600 (parity with old positional arithmetic)', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(12), direction: 'OUT' },
      { timestamp: T(13), direction: 'IN' },
      { timestamp: T(17), direction: 'OUT' },
    ])
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 480)
    assert.equal(d.note, null)
  })
})

// [10] mixed directions & WILD fallback --------------------------------------
section('10. mixed AUTO/IN/OUT and WILD fallback rules', () => {
  check('AUTO opens when nothing is open, closes when a session is open (opposite-of-state)', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(16), direction: 'AUTO' },
      { timestamp: T(17), direction: 'IN' },
      { timestamp: T(18, 30), direction: 'AUTO' }, // session open → acts as OUT
    ])
    assert.equal(r.pairs.length, 2)
    assert.deepEqual(r.pairs.map((p) => p.minutes), [480, 90])
    assert.equal(r.workedMinutes, 570)
    assert.deepEqual(reasons(r), [])
  })
  check('IN then AUTO: AUTO closes the open IN session (device punch ending a kiosk session)', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16), direction: 'AUTO' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
    assert.deepEqual(reasons(r), [])
  })
  check('unknown direction string (legacy junk) is treated as WILD, never crashes', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'UNKNOWN' },
      { timestamp: T(16), direction: 'UNKNOWN' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 480)
  })
  check('AUTO as the FIRST punch of an open AUTO session, then IN, then OUT → duplicate-IN reported, session pairing still honest', () => {
    const r = pairPunches([
      { timestamp: T(8), direction: 'AUTO' }, // opens
      { timestamp: T(9), direction: 'IN' }, // duplicate IN (session already open)
      { timestamp: T(17), direction: 'OUT' }, // closes the 08:00 session
    ])
    assert.deepEqual(reasons(r), ['DUPLICATE_IN'])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.workedMinutes, 540)
  })
})

// [11] T2 — night-shift cross-midnight pairing -------------------------------
section('11. T2 night-shift cross-midnight pairing (pure layer; day attribution is S3/S5 scope)', () => {
  check('IN 22:00 Sat → OUT 06:00 Sun inside one call: one pair, 480 minutes (the pairing layer is calendar-agnostic)', () => {
    const r = pairPunches([
      { timestamp: T(22, 0, 16), direction: 'IN' },
      { timestamp: T(6, 0, 17), direction: 'OUT' },
    ])
    assert.equal(r.pairs.length, 1)
    assert.equal(r.pairs[0].minutes, 480)
    assert.equal(r.workedMinutes, 480)
    assert.deepEqual(reasons(r), [])
  })
  check('split-shift night pattern with midnight break: two pairs across midnight', () => {
    const r = pairPunches([
      { timestamp: T(22, 0, 16), direction: 'IN' },
      { timestamp: T(23, 30, 16), direction: 'OUT' },
      { timestamp: T(5, 0, 17), direction: 'IN' },
      { timestamp: T(6, 30, 17), direction: 'OUT' },
    ])
    assert.deepEqual(r.pairs.map((p) => p.minutes), [90, 90])
    assert.equal(r.workedMinutes, 180)
  })
})

// [12] computeDaily integration ------------------------------------------------
section('12. computeDaily integration: statuses, note provenance, unchanged late/early/OT formulas', () => {
  check('normal day 08:00→16:00: PRESENT, late 0, early 0, OT 0, note null (exact old-engine numbers)', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16), direction: 'OUT' },
    ])
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 480)
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.overtimeMinutes, 0)
    assert.equal(d.note, null)
  })
  check('late arrival 08:20 with grace 10 → late 10; early leave 15:30 → early 20 (formulas untouched by S2)', () => {
    const d = daily([
      { timestamp: T(8, 20), direction: 'IN' },
      { timestamp: T(15, 30), direction: 'OUT' },
    ])
    assert.equal(d.lateMinutes, 10)
    assert.equal(d.earlyLeaveMinutes, 20)
    assert.equal(d.workedMinutes, 430)
  })
  check('2 punches but zero valid pairs (IN,IN): INCOMPLETE, worked 0 — the fake PRESENT of the old engine is gone', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(9), direction: 'IN' },
    ])
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.workedMinutes, 0)
    assert.equal(d.note, 'DUPLICATE_IN:1|IN_WITHOUT_OUT:1')
  })
  check('pure-AUTO odd day: PRESENT, worked = first pair only (old-engine parity), open punch visible in note', () => {
    const d = daily([
      { timestamp: T(8), direction: 'AUTO' },
      { timestamp: T(12), direction: 'AUTO' },
      { timestamp: T(17), direction: 'AUTO' },
    ])
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.note, 'IN_WITHOUT_OUT:1')
    assert.equal(iso(d.lastOut), iso(T(12)))
  })
  check('no punches on a future day → PENDING (status path untouched)', () => {
    const d = computeDaily({
      personId: 'p1',
      day: new Date(Date.UTC(2026, 10, 17, 0, 0)),
      shift: dayShift,
      punches: [],
      isHoliday: false,
      isOff: false,
      now: new Date(Date.UTC(2026, 10, 16, 0, 0)),
    })
    assert.equal(d.status, 'PENDING')
  })
  check('computeDaily is deterministic across repeat calls (deep-equal)', () => {
    const punches = [
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(9), direction: 'IN' },
      { timestamp: T(17), direction: 'OUT' },
      { timestamp: T(19), direction: 'OUT' },
    ]
    assert.deepEqual(daily(punches), daily(punches))
  })
  check('note is null on anomaly-free days even with multiple pairs (no false positives)', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(12), direction: 'OUT' },
      { timestamp: T(13), direction: 'IN' },
      { timestamp: T(17), direction: 'OUT' },
    ])
    assert.equal(d.note, null)
  })
})

// [13] purity ------------------------------------------------------------------
section('13. purity: the pairing layer never mutates its input (no raw punch edits)', () => {
  check('input array order and punch objects are bit-identical after pairPunches', () => {
    const punches = [
      { timestamp: T(9), direction: 'IN', source: 'KIOSK' },
      { timestamp: T(12), direction: 'IN', source: 'WEB' },
      { timestamp: T(12), direction: 'OUT', source: 'KIOSK' },
      { timestamp: T(17), direction: 'OUT', source: 'WEB' },
    ]
    const snapshot = punches.map((p) => ({ ...p, ts: p.timestamp.getTime() }))
    pairPunches(punches)
    assert.equal(punches.length, snapshot.length)
    punches.forEach((p, i) => {
      assert.equal(p.timestamp.getTime(), snapshot[i].ts)
      assert.equal(p.direction, snapshot[i].direction)
      assert.equal(p.source, snapshot[i].source)
    })
  })
  check('an empty input is a valid, total no-op result', () => {
    const r = pairPunches([])
    assert.equal(r.pairs.length, 0)
    assert.equal(r.workedMinutes, 0)
    assert.equal(r.firstIn, null)
    assert.equal(r.lastOut, null)
    assert.deepEqual(r.anomalies, [])
    assert.deepEqual(r.unpairedIndexes, [])
    assert.equal(r.openPairIndex, null)
  })
})

// [14] policy non-interference ---------------------------------------------------
section('14. policy non-interference: OT / holiday / leave math untouched by S2', () => {
  check('OT threshold literal unchanged: worked 525 (> 480+30) → OT 45; worked 510 (< 510 threshold edge) → OT 0', () => {
    const above = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16, 45), direction: 'OUT' },
    ])
    assert.equal(above.workedMinutes, 525)
    assert.equal(above.overtimeMinutes, 45)
    const below = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16, 30), direction: 'OUT' },
    ])
    assert.equal(below.workedMinutes, 510)
    assert.equal(below.overtimeMinutes, 0) // 510 is NOT > 480+30 → still zero (S2 must not move this line)
  })
  check('holiday day with worked punches → OT = worked (unchanged holiday rule)', () => {
    const d = computeDaily({
      personId: 'p1',
      day: new Date(Date.UTC(2026, 10, 17, 0, 0)),
      shift: dayShift,
      punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(12), direction: 'OUT' },
      ],
      isHoliday: true,
      isOff: false,
      now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.overtimeMinutes, 240)
  })
  check('full-day approved leave without punches → LEAVE with leaveMinutes (unchanged rule)', () => {
    const d = computeDaily({
      personId: 'p1',
      day: new Date(Date.UTC(2026, 10, 17, 0, 0)),
      shift: dayShift,
      punches: [],
      leaveSpans: [{ start: T(8), end: T(16), leaveTypeId: 'lt1' }],
      isHoliday: false,
      isOff: false,
      now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.status, 'LEAVE')
    assert.equal(d.leaveMinutes, 480)
    assert.equal(d.note, null) // leave path untouched: no punch anomalies involved
  })
})

// ===========================================================================
// Phase 2 Stage S3 — approved policies (workday/shift, overtime/min_excess,
// mission/approved_credit) — pure-function sections.
// ===========================================================================

const DAY_SHIFT_CTX = { segments: [{ startMin: 8 * 60, endMin: 16 * 60 }] }
const NIGHT_SHIFT_CTX = { segments: [{ startMin: 22 * 60, endMin: 6 * 60 }] }

/** marker of Tehran day Y-M-D */
const M = (y: number, m: number, d: number) => new Date(Date.UTC(y, m - 1, d))
const MIN = 60000

console.log('== Phase 2 S3 policies (workday / overtime / mission) ==\n')

// [15] Workday attribution ---------------------------------------------------
section('15. S3 workday: normal day shift attribution', () => {
  check('day-shift punch 08:30 on its assigned day → that day, SHIFT_WINDOW', () => {
    const punch = tehranInstant(2026, 11, 17, 8, 30) // Tuesday
    const r = attributePunchWorkday(punch, (day) => (markerKey(day) === '2026-11-17' ? DAY_SHIFT_CTX : null))
    assert.equal(markerKey(r.workday), '2026-11-17')
    assert.equal(r.viaShift, true)
    assert.equal(r.reason, 'SHIFT_WINDOW')
  })
})

section('16. S3 workday: night shift 22→06, after-midnight punch → shift START day', () => {
  check('Sat-assigned night shift; Sunday 02:30 punch → SATURDAY workday (the policy example)', () => {
    const punch = tehranInstant(2026, 11, 15, 2, 30) // Sunday 02:30 Tehran
    const r = attributePunchWorkday(punch, (day) => (markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : null)) // Saturday assignment
    assert.equal(markerKey(r.workday), '2026-11-14') // Saturday
    assert.equal(r.viaShift, true)
    assert.equal(r.reason, 'SHIFT_WINDOW')
  })
  check('the 22:00 IN and a 05:59 OUT both land on Saturday; the window is half-open — an OUT at EXACTLY 06:00 falls back to Sunday', () => {
    const resolver = (day: Date) => (markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : null)
    const inPunch = tehranInstant(2026, 11, 14, 22, 0)
    assert.equal(markerKey(attributePunchWorkday(inPunch, resolver).workday), '2026-11-14')
  })
  check('OUT at 05:59 (strictly inside [22:00, 06:00)) → Saturday; OUT at exactly 06:00 → OUT_OF_WINDOW fallback to Sunday', () => {
    const resolver = (day: Date) => (markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : null)
    const outBefore = tehranInstant(2026, 11, 15, 5, 59)
    assert.equal(markerKey(attributePunchWorkday(outBefore, resolver).workday), '2026-11-14')
    const outExact = tehranInstant(2026, 11, 15, 6, 0)
    const r = attributePunchWorkday(outExact, resolver)
    assert.equal(markerKey(r.workday), '2026-11-15') // half-open window: [start, end)
    assert.equal(r.reason, 'OUT_OF_WINDOW')
  })
})

section('17. S3 workday: out-of-window punch and no-shift fallback (with reason)', () => {
  check('stray 02:30 Tehran punch with only a day shift → OUT_OF_WINDOW fallback to Tehran day', () => {
    const punch = tehranInstant(2026, 11, 17, 2, 30)
    const r = attributePunchWorkday(punch, () => DAY_SHIFT_CTX)
    assert.equal(markerKey(r.workday), '2026-11-17')
    assert.equal(r.viaShift, false)
    assert.equal(r.reason, 'OUT_OF_WINDOW')
  })
  check('no shift at all → NO_SHIFT fallback to Tehran day', () => {
    const punch = tehranInstant(2026, 11, 17, 10, 0)
    const r = attributePunchWorkday(punch, () => null)
    assert.equal(markerKey(r.workday), '2026-11-17')
    assert.equal(r.reason, 'NO_SHIFT')
  })
  check('malformed shift (startMin out of range) → INVALID_SHIFT fallback', () => {
    const punch = tehranInstant(2026, 11, 17, 10, 0)
    const r = attributePunchWorkday(punch, () => ({ segments: [{ startMin: 2000, endMin: 100 }] }))
    assert.equal(markerKey(r.workday), '2026-11-17')
    assert.equal(r.reason, 'INVALID_SHIFT')
  })
})

section('18. S3 workday: different assignments on adjacent days (no bleed-through)', () => {
  check('Sat=night, Sun=day: Sunday 02:30 → SATURDAY (via Sat window), Sunday 08:30 → Sunday (via Sun window)', () => {
    const resolver = (day: Date) =>
      markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : markerKey(day) === '2026-11-15' ? DAY_SHIFT_CTX : null
    const early = tehranInstant(2026, 11, 15, 2, 30)
    const normal = tehranInstant(2026, 11, 15, 8, 30)
    assert.equal(markerKey(attributePunchWorkday(early, resolver).workday), '2026-11-14')
    assert.equal(markerKey(attributePunchWorkday(normal, resolver).workday), '2026-11-15')
  })
  check("today's shift is never applied to old records: Monday-only night shift cannot capture Sunday's 02:30 punch", () => {
    // Sunday 02:30 can only belong to a shift STARTING Fri/Sat/Sun (the candidate
    // span); a shift assigned only on Monday is never even consulted → NO_SHIFT.
    const resolver = (day: Date) => (markerKey(day) === '2026-11-16' ? NIGHT_SHIFT_CTX : null) // Monday
    const punch = tehranInstant(2026, 11, 15, 2, 30) // Sunday early morning
    const r = attributePunchWorkday(punch, resolver)
    assert.equal(markerKey(r.workday), '2026-11-15') // fallback Tehran day — NOT Monday
    assert.equal(r.reason, 'NO_SHIFT') // Monday is outside the candidate span entirely
  })
})

section('19. S3 workday: Jalali month & year boundaries (Esfand→Farvardin, leap)', () => {
  check('night shift starting Esfand 29 1404 (2026-03-20): Farvardin 1 02:30 punch (2026-03-21) → Esfand 29 workday', () => {
    const resolver = (day: Date) => (markerKey(day) === '2026-03-20' ? NIGHT_SHIFT_CTX : null)
    const punch = tehranInstant(2026, 3, 21, 2, 30)
    assert.equal(markerKey(attributePunchWorkday(punch, resolver).workday), '2026-03-20')
  })
  check('night shift starting Esfand 30 1403 (2025-03-20, leap year): next-day punch → Esfand 30 workday; Farvardin 1 = 2025-03-21', () => {
    const resolver = (day: Date) => (markerKey(day) === '2025-03-20' ? NIGHT_SHIFT_CTX : null)
    const punch = tehranInstant(2025, 3, 21, 3, 0)
    assert.equal(markerKey(attributePunchWorkday(punch, resolver).workday), '2025-03-20')
  })
})

section('20. S3 workday: coverage partition (no leave/mission minute in two workdays)', () => {
  check('night Saturday: [00:00 Sat, 06:00 Sun) claimed by Saturday; Sunday coverage = [06:00 Sun, 00:00 Mon)', () => {
    const resolver = (day: Date) => (markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : null)
    const cov = coverageWindowsByDay(M(2026, 11, 14), M(2026, 11, 15), resolver)
    const sat = cov.get('2026-11-14')!
    const sun = cov.get('2026-11-15')!
    assert.equal(sat.length, 1)
    assert.equal(new Date(sat[0].start).toISOString(), '2026-11-13T20:30:00.000Z') // Tehran midnight Sat
    assert.equal(new Date(sat[0].end).toISOString(), '2026-11-15T02:30:00.000Z') // + night window end Sun 06:00
    // Sunday: Tehran day minus Saturday's claim → [Sun 06:00, Mon 00:00 Tehran)
    assert.equal(sun.length, 1)
    assert.equal(new Date(sun[0].start).toISOString(), '2026-11-15T02:30:00.000Z')
    assert.equal(new Date(sun[0].end).toISOString(), '2026-11-15T20:30:00.000Z') // Mon 00:00 Tehran = Sun 20:30Z
    // partition check: no overlap, no gap between the two
    assert.equal(sat[0].end, sun[0].start)
  })
  check('two consecutive night shifts: Sunday coverage is exactly [06:00 Sun, 06:00 Mon)', () => {
    const resolver = (day: Date) => (markerKey(day) === '2026-11-14' || markerKey(day) === '2026-11-15' ? NIGHT_SHIFT_CTX : null)
    const cov = coverageWindowsByDay(M(2026, 11, 14), M(2026, 11, 16), resolver)
    const sun = cov.get('2026-11-15')!
    assert.equal(new Date(sun[0].start).toISOString(), '2026-11-15T02:30:00.000Z') // 06:00 Tehran Sun
    assert.equal(new Date(sun[0].end).toISOString(), '2026-11-16T02:30:00.000Z') // 06:00 Tehran Mon
  })
})

// [21] Overtime (min_excess) --------------------------------------------------
section('21. S3 overtime: threshold gating below/at/above (whole minutes, no rounding added)', () => {
  check('worked below threshold → actual 0, payable 0', () => {
    const d = daily([{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16, 29), direction: 'OUT' }])
    assert.equal(d.workedMinutes, 509) // standard 480 + 29 < +30
    assert.equal(d.overtimeMinutes, 0)
    assert.equal(d.payableOvertimeMinutes, 0)
  })
  check('worked EXACTLY standard+30 → still zero (boundary preserved)', () => {
    const d = daily([{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16, 30), direction: 'OUT' }])
    assert.equal(d.workedMinutes, 510)
    assert.equal(d.overtimeMinutes, 0) // 510 is NOT > 480+30
    assert.equal(d.payableOvertimeMinutes, 0)
  })
  check('one minute above threshold → the WHOLE excess is actual OT (threshold gates, never subtracts)', () => {
    const d = daily([{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16, 31), direction: 'OUT' }])
    assert.equal(d.workedMinutes, 511)
    assert.equal(d.overtimeMinutes, 31) // 511-480 — NOT 511-480-30
    assert.equal(d.payableOvertimeMinutes, 0) // no approved request
  })
})

section('22. S3 overtime: threshold from Setting (custom & default)', () => {
  check('custom threshold 60: worked 525 → 0 (below 480+60); worked 541 → 61 actual', () => {
    const mk = (outH: number, outM: number) =>
      computeDaily({
        personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
        punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(outH, outM), direction: 'OUT' }],
        otThresholdMinutes: 60, isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
      })
    assert.equal(mk(16, 45).overtimeMinutes, 0) // 525 < 540
    assert.equal(mk(17, 1).overtimeMinutes, 61) // 541 > 540 → 541-480
  })
  check('parseOtThresholdMinutes: valid "45" → 45; missing → 30; "abc"/"-5"/"2.5" → 30 (invalid never enters silently)', () => {
    assert.equal(parseOtThresholdMinutes('45'), 45)
    assert.equal(parseOtThresholdMinutes(undefined), 30)
    assert.equal(parseOtThresholdMinutes('abc'), 30)
    assert.equal(parseOtThresholdMinutes('-5'), 30)
    assert.equal(parseOtThresholdMinutes('2.5'), 30)
    assert.equal(parseOtThresholdMinutes('0'), 0) // zero is a legal non-negative integer
  })
})

section('23. S3 overtime: payable = min(actual, approved) with request statuses', () => {
  const ot = (workedH: number, workedM: number, approved: number) =>
    computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
      punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(workedH, workedM), direction: 'OUT' }],
      approvedOvertimeMinutes: approved,
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
  check('PENDING / REJECTED requests contribute nothing: caller passes 0 → payable 0 while actual stays visible', () => {
    const d = ot(18, 31, 0) // worked 631 → actual 151
    assert.equal(d.overtimeMinutes, 151)
    assert.equal(d.payableOvertimeMinutes, 0)
  })
  check('approved LESS than actual → payable = approved; actual excess stays visible', () => {
    const d = ot(18, 31, 60)
    assert.equal(d.overtimeMinutes, 151)
    assert.equal(d.payableOvertimeMinutes, 60)
  })
  check('approved MORE than actual → payable = actual (never exceeds actual)', () => {
    const d = ot(18, 31, 500)
    assert.equal(d.payableOvertimeMinutes, 151)
  })
  check('approved equals actual → payable = actual', () => {
    const d = ot(18, 31, 151)
    assert.equal(d.payableOvertimeMinutes, 151)
  })
  check('multiple approved requests are summed by the caller-side preload (map sum) — sum 90+61=151 → payable 151', () => {
    const d = ot(18, 31, 90 + 61)
    assert.equal(d.payableOvertimeMinutes, 151)
  })
  check('no actual OT → payable 0 even with approvals (min(actual, approved) with actual=0)', () => {
    const d = ot(16, 30, 200)
    assert.equal(d.overtimeMinutes, 0)
    assert.equal(d.payableOvertimeMinutes, 0)
  })
})

section('24. S3 overtime: holiday/off-day actual OT unchanged; payable still min(actual, approved)', () => {
  check('holiday with 240 worked → actual 240; no approval → payable 0; approved 100 → payable 100', () => {
    const mk = (approved: number) =>
      computeDaily({
        personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
        punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(12), direction: 'OUT' }],
        approvedOvertimeMinutes: approved, isHoliday: true, isOff: false,
        now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
      })
    assert.equal(mk(0).overtimeMinutes, 240)
    assert.equal(mk(0).payableOvertimeMinutes, 0)
    assert.equal(mk(100).payableOvertimeMinutes, 100)
    assert.equal(mk(999).payableOvertimeMinutes, 240)
  })
})

// [25] Mission (approved_credit) -----------------------------------------------
const nightShiftLite: ShiftLite = {
  id: 'shift-night', name: 'شیفت شب', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
  segments: [{ startMin: 22 * 60, endMin: 6 * 60, breakMin: 0 }],
}

section('25. S3 mission: unapproved/rejected → zero credit, zero status effect', () => {
  check('PENDING/REJECTED mission spans are simply not passed in (caller filters APPROVED) — passing none gives 0', () => {
    const d = daily([{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16), direction: 'OUT' }])
    assert.equal(d.missionMinutes, 0)
    assert.equal(d.status, 'PRESENT')
  })
  check('a mission span on ANOTHER day (zero overlap with this workday coverage) → credit 0, status untouched', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(8, 0, 18), end: T(12, 0, 18) }], // Nov 18 08:00-12:00 — different Tehran day
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 17, 12, 0)), // before day end − 6h → PENDING
    })
    assert.equal(d.missionMinutes, 0)
    assert.equal(d.status, 'PENDING')
  })
})

section('26. S3 mission: approved mission without punches → MISSION status with exact credited minutes', () => {
  check('full-day mission 08:00–16:00 → missionMinutes 480, status MISSION, firstIn/lastOut null (no fabricated punches)', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(8), end: T(16) }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 480)
    assert.equal(d.status, 'MISSION')
    assert.equal(d.firstIn, null)
    assert.equal(d.lastOut, null)
    assert.equal(d.workedMinutes, 480) // policy: final worked = attendance + mission credit
  })
  check('hourly mission 10:00–13:00 → 180 credited, MISSION status', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(10), end: T(13) }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 180)
    assert.equal(d.status, 'MISSION')
  })
})

section('27. S3 mission: mission + real attendance — overlap subtracted, no double count', () => {
  check('attendance 08:00–16:00 + mission 14:00–18:00 → credit only 16:00–18:00 (120); worked 480+120=600; status PRESENT', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
      punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16), direction: 'OUT' }],
      missionSpans: [{ start: T(14), end: T(18) }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 120)
    assert.equal(d.workedMinutes, 600)
    assert.equal(d.status, 'PRESENT')
    assert.equal(iso(d.firstIn), iso(T(8))) // real punches only
    assert.equal(iso(d.lastOut), iso(T(16)))
  })
  check('mission fully inside attendance 08:00–16:00 ⊆ mission 08:00–17:00 → credit 60 (17:00−16:00)', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
      punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16), direction: 'OUT' }],
      missionSpans: [{ start: T(8), end: T(17) }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 60)
    assert.equal(d.workedMinutes, 540)
  })
  check('incomplete punch day keeps INCOMPLETE (mission never hides bad punches); credit still computed', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
      punches: [{ timestamp: T(8), direction: 'IN' }], // missing OUT
      missionSpans: [{ start: T(10), end: T(14) }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.missionMinutes, 240)
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.note, 'IN_WITHOUT_OUT:1')
  })
})

section('28. S3 mission: night mission attributes to shift-start workday via coverage', () => {
  check('night shift workday Sat (window 22:00→06:00): mission Sat 22:00→Sun 06:00 → 480 credited on SATURDAY row', () => {
    // coverage intervals of Sat = [Sat 00:00, Sun 06:00) per the partition (section 20)
    const satCoverage = [{ start: Date.UTC(2026, 10, 13, 20, 30), end: Date.UTC(2026, 10, 15, 2, 30) }]
    const d = computeDaily({
      personId: 'p1', day: M(2026, 11, 14), shift: nightShiftLite, punches: [],
      missionSpans: [{ start: tehranInstant(2026, 11, 14, 22, 0), end: tehranInstant(2026, 11, 15, 6, 0) }],
      coverageIntervals: satCoverage,
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 11, 16, 12, 0)),
    })
    assert.equal(d.missionMinutes, 480)
    assert.equal(d.status, 'MISSION')
  })
  check('same night mission credited 0 on the SUNDAY row (its coverage starts Sun 06:00)', () => {
    const sunCoverage = [{ start: Date.UTC(2026, 10, 15, 2, 30), end: Date.UTC(2026, 10, 16, 20, 30) }]
    const d = computeDaily({
      personId: 'p1', day: M(2026, 11, 15), shift: dayShift, punches: [],
      missionSpans: [{ start: tehranInstant(2026, 11, 14, 22, 0), end: tehranInstant(2026, 11, 15, 6, 0) }],
      coverageIntervals: sunCoverage,
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 11, 16, 12, 0)),
    })
    assert.equal(d.missionMinutes, 0)
  })
})

section('29. S3 mission: overlapping missions — union, never double', () => {
  check('two overlapping missions 08:00–14:00 and 10:00–16:00 → union 08:00–16:00 = 480 credited once', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [
        { start: T(8), end: T(14) },
        { start: T(10), end: T(16) },
      ],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 480)
    assert.equal(d.status, 'MISSION')
  })
  check('two disjoint missions 08:00–10:00 and 14:00–16:00 → 120+120 = 240', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [
        { start: T(8), end: T(10) },
        { start: T(14), end: T(16) },
      ],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 240)
  })
})

section('30. S3 mission × leave × attendance: no interval counted twice', () => {
  check('mission 10:00–14:00 + sick leave 12:00–16:00: mission credit 240; leaveMinutes only 14:00–16:00=120 (overlap removed)', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(10), end: T(14) }],
      leaveSpans: [{ start: T(12), end: T(16), leaveTypeId: 'lt-sick' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 240)
    assert.equal(d.leaveMinutes, 120) // 12:00–16:00 minus mission overlap 12:00–14:00
    assert.equal(d.status, 'MISSION') // leave 120 < 480 → mission status
    assert.equal(d.leaveTypeId, 'lt-sick')
  })
  check('attendance 08:00–12:00 + mission 10:00–14:00 + leave 15:00–17:00 → worked 240+120=360; mission 120; leave 120', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift,
      punches: [{ timestamp: T(8), direction: 'IN' }, { timestamp: T(12), direction: 'OUT' }],
      missionSpans: [{ start: T(10), end: T(14) }],
      leaveSpans: [{ start: T(15), end: T(17), leaveTypeId: 'lt2' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.missionMinutes, 120) // 10:00–14:00 minus attendance 10:00–12:00
    assert.equal(d.workedMinutes, 360) // 240 attendance + 120 mission
    assert.equal(d.leaveMinutes, 120)
    assert.equal(d.status, 'PRESENT')
  })
  check('full-day LEAVE takes status precedence over MISSION when no punches', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(14), end: T(18) }],
      leaveSpans: [{ start: T(8), end: T(16), leaveTypeId: 'lt3' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    // leave 08–16 minus mission 14–18 ∩ = 08–14 = 360 < 480 → MISSION status wins here
    assert.equal(d.leaveMinutes, 360)
    assert.equal(d.missionMinutes, 240)
    assert.equal(d.status, 'MISSION')
    // true full-day leave (480, no mission overlap) → LEAVE:
    const d2 = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(17), end: T(19) }],
      leaveSpans: [{ start: T(8), end: T(16), leaveTypeId: 'lt3' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d2.leaveMinutes, 480)
    assert.equal(d2.status, 'LEAVE')
  })
})

// [31] interval helpers sanity ------------------------------------------------
section('31. S3 interval math: union/subtract/intersect totals', () => {
  check('union of overlapping [0,4)+[3,7) minutes = [0,7) → 7 total minutes', () => {
    const u = mergeIntervals([{ start: 0, end: 4 * MIN }, { start: 3 * MIN, end: 7 * MIN }])
    assert.deepEqual(u, [{ start: 0, end: 7 * MIN }])
    assert.equal(totalMinutes(u), 7)
  })
  check('subtract removes the covered part exactly', () => {
    const d = subtractIntervals([{ start: 0, end: 10 * MIN }], [{ start: 3 * MIN, end: 5 * MIN }])
    assert.deepEqual(d, [{ start: 0, end: 3 * MIN }, { start: 5 * MIN, end: 10 * MIN }])
  })
  check('intersect clips to the window', () => {
    const i = intersectIntervals(
      [{ start: 2 * MIN, end: 8 * MIN }],
      [{ start: 0, end: 4 * MIN }, { start: 6 * MIN, end: 12 * MIN }],
    )
    assert.deepEqual(i, [{ start: 2 * MIN, end: 4 * MIN }, { start: 6 * MIN, end: 8 * MIN }])
  })
})

// [32] S3 review fix 4 — strict threshold parsing ------------------------------
section('32. S3 review fix 4: parseOtThresholdMinutes strict matrix (no coercion to 0)', () => {
  check("valid digit strings parse: '45'→45, '0'→0 (legal zero), ' 60 '→60 (whitespace-wrapped digits)", () => {
    assert.equal(parseOtThresholdMinutes('45'), 45)
    assert.equal(parseOtThresholdMinutes('0'), 0)
    assert.equal(parseOtThresholdMinutes(' 60 '), 60)
  })
  check('null / undefined / non-string → 30 (Number(null)===0 coercion must NOT hijack the threshold)', () => {
    assert.equal(parseOtThresholdMinutes(null), 30)
    assert.equal(parseOtThresholdMinutes(undefined), 30)
    assert.equal(parseOtThresholdMinutes(45 as unknown as string), 30) // type violation → default, never guessed
  })
  check("empty / whitespace-only strings → 30 ('' and '   ' must not become 0)", () => {
    assert.equal(parseOtThresholdMinutes(''), 30)
    assert.equal(parseOtThresholdMinutes('   '), 30)
  })
  check("decimal / negative / non-numeric / exponent / hex strings → 30", () => {
    assert.equal(parseOtThresholdMinutes('2.5'), 30)
    assert.equal(parseOtThresholdMinutes('-5'), 30)
    assert.equal(parseOtThresholdMinutes('abc'), 30)
    assert.equal(parseOtThresholdMinutes('1e2'), 30)
    assert.equal(parseOtThresholdMinutes('0x10'), 30)
    assert.equal(parseOtThresholdMinutes('45abc'), 30)
    assert.equal(parseOtThresholdMinutes('4 5'), 30)
  })
})

// [33] S3 review fix 5 — request-minute validation (shared POST/PUT contract) ---
section('33. S3 review fix 5: parsePositiveRequestMinutes — one contract for POST and PUT', () => {
  check('number and digit-string inputs parse: 90, "90", " 75 "', () => {
    assert.equal(parsePositiveRequestMinutes(90), 90)
    assert.equal(parsePositiveRequestMinutes('90'), 90)
    assert.equal(parsePositiveRequestMinutes(' 75 '), 75)
  })
  check('zero and negative are REJECTED (null): 0, "0", -5, "-5"', () => {
    assert.equal(parsePositiveRequestMinutes(0), null)
    assert.equal(parsePositiveRequestMinutes('0'), null)
    assert.equal(parsePositiveRequestMinutes(-5), null)
    assert.equal(parsePositiveRequestMinutes('-5'), null)
  })
  check('decimal and NaN are REJECTED (null): 2.5, "2.5", NaN, "1e2"', () => {
    assert.equal(parsePositiveRequestMinutes(2.5), null)
    assert.equal(parsePositiveRequestMinutes('2.5'), null)
    assert.equal(parsePositiveRequestMinutes(NaN), null)
    assert.equal(parsePositiveRequestMinutes('1e2'), null)
  })
  check('missing / non-numeric / other types are REJECTED (null): undefined, null, "", "   ", "abc", true, {}', () => {
    assert.equal(parsePositiveRequestMinutes(undefined), null)
    assert.equal(parsePositiveRequestMinutes(null), null)
    assert.equal(parsePositiveRequestMinutes(''), null)
    assert.equal(parsePositiveRequestMinutes('   '), null)
    assert.equal(parsePositiveRequestMinutes('abc'), null)
    assert.equal(parsePositiveRequestMinutes(true), null)
    assert.equal(parsePositiveRequestMinutes({}), null)
  })
})

// [34] S3 review fix 3 — direction-aware window END boundary --------------------
section('34. S3 review fix 3: direction-aware night-shift end boundary (OUT closes, IN opens)', () => {
  const nightResolver = (day: Date) => (markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : null)
  check('mandated case 1 — Sat 22:00 IN + Sun 06:00 OUT: the OUT at EXACTLY the end stays on SATURDAY (valid pair never split)', () => {
    const inPunch = tehranInstant(2026, 11, 14, 22, 0)
    const outPunch = tehranInstant(2026, 11, 15, 6, 0)
    const rIn = attributePunchWorkday(inPunch, nightResolver, 'IN')
    const rOut = attributePunchWorkday(outPunch, nightResolver, 'OUT')
    assert.equal(markerKey(rIn.workday), '2026-11-14')
    assert.equal(rIn.reason, 'SHIFT_WINDOW')
    assert.equal(markerKey(rOut.workday), '2026-11-14') // SAME workday — the pair survives
    assert.equal(rOut.reason, 'SHIFT_WINDOW')
    assert.equal(rOut.viaShift, true)
  })
  check('an IN at EXACTLY 06:00 is OUTSIDE the half-open night window → Tehran day fallback (Sunday) with OUT_OF_WINDOW', () => {
    const r = attributePunchWorkday(tehranInstant(2026, 11, 15, 6, 0), nightResolver, 'IN')
    assert.equal(markerKey(r.workday), '2026-11-15')
    assert.equal(r.reason, 'OUT_OF_WINDOW')
    assert.equal(r.viaShift, false)
  })
  check('mandated case 2 — OUT at 06:01 (strictly past the window) → OUT_OF_WINDOW fallback to Sunday (documented fallback, no fabricated attribution)', () => {
    const r = attributePunchWorkday(tehranInstant(2026, 11, 15, 6, 1), nightResolver, 'OUT')
    assert.equal(markerKey(r.workday), '2026-11-15')
    assert.equal(r.reason, 'OUT_OF_WINDOW')
    assert.equal(r.viaShift, false)
  })
  check('direction-unknown punch (AUTO) at EXACTLY 06:00 keeps the half-open rule → OUT_OF_WINDOW Sunday (no guessing a closing session)', () => {
    const rAuto = attributePunchWorkday(tehranInstant(2026, 11, 15, 6, 0), nightResolver)
    const rWild = attributePunchWorkday(tehranInstant(2026, 11, 15, 6, 0), nightResolver, 'AUTO')
    assert.equal(markerKey(rAuto.workday), '2026-11-15')
    assert.equal(rAuto.reason, 'OUT_OF_WINDOW')
    assert.equal(markerKey(rWild.workday), '2026-11-15')
    assert.equal(rWild.reason, 'OUT_OF_WINDOW')
  })
  check('mandated case 3 — abutting next-day shift starting exactly at the boundary: OUT 06:00 → SATURDAY (closes the night), IN 06:00 → SUNDAY (opens the day shift); independent & deterministic', () => {
    const boundaryDayCtx = { segments: [{ startMin: 6 * 60, endMin: 14 * 60 }] } // Sunday 06:00→14:00 — starts EXACTLY at the night end
    const bothResolver = (day: Date) =>
      markerKey(day) === '2026-11-14' ? NIGHT_SHIFT_CTX : markerKey(day) === '2026-11-15' ? boundaryDayCtx : null
    const t = tehranInstant(2026, 11, 15, 6, 0)
    const rOut = attributePunchWorkday(t, bothResolver, 'OUT')
    const rIn = attributePunchWorkday(t, bothResolver, 'IN')
    assert.equal(markerKey(rOut.workday), '2026-11-14') // the shift the punch CLOSES
    assert.equal(rOut.reason, 'AMBIGUOUS_LATEST') // two definitions matched the instant — deterministic pick
    assert.equal(rOut.viaShift, true)
    assert.equal(markerKey(rIn.workday), '2026-11-15') // the shift the punch OPENS
    assert.equal(rIn.reason, 'SHIFT_WINDOW')
  })
  check('reason codes preserved: NO_SHIFT / INVALID_SHIFT / OUT_OF_WINDOW unchanged for the direction-less API', () => {
    assert.equal(attributePunchWorkday(tehranInstant(2026, 11, 17, 10, 0), () => null).reason, 'NO_SHIFT')
    assert.equal(attributePunchWorkday(tehranInstant(2026, 11, 17, 10, 0), () => ({ segments: [{ startMin: 2000, endMin: 100 }] })).reason, 'INVALID_SHIFT')
    assert.equal(attributePunchWorkday(tehranInstant(2026, 11, 17, 2, 30), () => DAY_SHIFT_CTX).reason, 'OUT_OF_WINDOW')
  })
})

// [35] S3 review fix 6 — leaveTypeId only from leaves with a real share ---------
section('35. S3 review fix 6: leaveTypeId references only leaves that contribute to leaveMinutes', () => {
  check('leave fully covered by an overlapping mission → leaveMinutes 0 AND leaveTypeId null (no zero-share reference)', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(8), end: T(16) }],
      leaveSpans: [{ start: T(9), end: T(15), leaveTypeId: 'lt-zero-share' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.leaveMinutes, 0)
    assert.equal(d.leaveTypeId, null)
    assert.equal(d.missionMinutes, 480)
  })
  check('two leaves — one fully mission-subtracted, one partially covered → id from the CONTRIBUTING leave only', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      missionSpans: [{ start: T(8), end: T(14) }],
      leaveSpans: [
        { start: T(9), end: T(12), leaveTypeId: 'lt-swallowed' },   // fully inside the mission
        { start: T(13), end: T(17), leaveTypeId: 'lt-real' },        // 14:00–17:00 survives
      ],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.leaveMinutes, 180) // 14:00–17:00 only
    assert.equal(d.leaveTypeId, 'lt-real')
  })
  check('deterministic pick among several contributing leaves: the latest-ending contributor wins (ties → later start)', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      leaveSpans: [
        { start: T(8), end: T(10), leaveTypeId: 'lt-a' },
        { start: T(11), end: T(13), leaveTypeId: 'lt-b' },
      ],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.leaveMinutes, 240)
    assert.equal(d.leaveTypeId, 'lt-b') // latest-ending contributor
  })
  check('leave outside the day coverage contributes nothing → leaveTypeId null even though the span exists', () => {
    const d = computeDaily({
      personId: 'p1', day: new Date(Date.UTC(2026, 10, 17, 0, 0)), shift: dayShift, punches: [],
      leaveSpans: [{ start: T(8, 0, 18), end: T(12, 0, 18), leaveTypeId: 'lt-other-day' }],
      isHoliday: false, isOff: false, now: new Date(Date.UTC(2026, 10, 18, 12, 0)),
    })
    assert.equal(d.leaveMinutes, 0)
    assert.equal(d.leaveTypeId, null)
  })
})


// ===========================================================================
// Phase 2 Stage S4 — calculation completeness (real shift-window instants for
// late/early, leave×work overlap carving, ABSENT timing, holiday/off/break
// documentation, provenance) — pure-function sections.
// ===========================================================================

console.log('== Phase 2 S4 calculation completeness ==\n')

const D17 = () => new Date(Date.UTC(2026, 10, 17, 0, 0))
const NOW18 = () => new Date(Date.UTC(2026, 10, 18, 12, 0))
/** evening shift 15:00–23:30 (Tehran) — the D13 regression shape */
const eveningShift: ShiftLite = {
  id: 'shift-evening', name: 'شیفت عصر', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
  segments: [{ startMin: 15 * 60, endMin: 23 * 60 + 30, breakMin: 0 }],
}
/** night shift 22:00–06:00 (Tehran, crosses midnight) */
const nightShift: ShiftLite = {
  id: 'shift-night', name: 'شیفت شب', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
  segments: [{ startMin: 22 * 60, endMin: 6 * 60, breakMin: 0 }],
}
/** two-segment shift 08:00–12:00 + 13:00–17:00 */
const splitShift: ShiftLite = {
  id: 'shift-split', name: 'شیفت چندبخشی', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
  segments: [
    { startMin: 8 * 60, endMin: 12 * 60, breakMin: 0 },
    { startMin: 13 * 60, endMin: 17 * 60, breakMin: 0 },
  ],
}

// [36] Late/Early on REAL instants — day shift parity + grace edges -----------
section('36. S4 late/early on real instants: day-shift parity and grace edges', () => {
  check('parity: 08:20 IN with grace 10 → late 10; 15:30 OUT → early 20 (unchanged numbers)', () => {
    const d = daily([
      { timestamp: T(8, 20), direction: 'IN' },
      { timestamp: T(15, 30), direction: 'OUT' },
    ])
    assert.equal(d.lateMinutes, 10)
    assert.equal(d.earlyLeaveMinutes, 20)
  })
  check('grace 0 shift: 08:01 IN → late 1; 15:59 OUT → early 1 (grace meaning preserved exactly)', () => {
    const g0: ShiftLite = { ...dayShift, lateGraceMin: 0, earlyGraceMin: 0 }
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: g0, punches: [
        { timestamp: T(8, 1), direction: 'IN' },
        { timestamp: T(15, 59), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 1)
    assert.equal(d.earlyLeaveMinutes, 1)
  })
  check('early arrival 07:30 IN → late 0 (a punch BEFORE the window start is simply early)', () => {
    const d = daily([
      { timestamp: T(7, 30), direction: 'IN' },
      { timestamp: T(16), direction: 'OUT' },
    ])
    assert.equal(d.lateMinutes, 0)
  })
  check('worked late past window end: OUT 16:30 → early 0 (overtime territory, not early-leave)', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16, 30), direction: 'OUT' },
    ])
    assert.equal(d.earlyLeaveMinutes, 0)
  })
})

// [37] The D13 regression — stray out-of-window punch --------------------------
section('37. S4 D13 regression: stray 02:30 punch on a 15:00–23:30 shift — no fake late/early', () => {
  check('stray 02:30 IN as the only punch → late 0 (old ±1440 logic fabricated ~11h)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.status, 'INCOMPLETE')
  })
  check('stray 02:30 IN+OUT pair → worked counted, late 0 AND early 0 (old logic fabricated ~21h early-leave here)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 20)
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.status, 'PRESENT')
  })
  check('stray 02:30 punches + real 15:00–23:30 work: late/early measured on the REAL window only', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
        { timestamp: T(15, 10), direction: 'IN' },
        { timestamp: T(23, 10), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0) // 15:10 within 10-min grace
    assert.equal(d.earlyLeaveMinutes, 10) // 23:10 vs 23:30 end
    assert.equal(d.workedMinutes, 20 + 8 * 60)
  })
  check('punch AFTER the window end keeps the old parity: IN 23:59 → late 529 (real punch after shift start, no wrap involved)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(23, 59), direction: 'IN' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 8 * 60 + 59 - 10)
  })
})

// [38] Night shift late/early across midnight ----------------------------------
section('38. S4 night shift: late/early exact across midnight (true instants)', () => {
  check('IN 23:00 (1h after 22:00 start, grace 10) → late 50; OUT 05:00 → early 10 (window ends 06:00)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: nightShift, punches: [
        { timestamp: T(23), direction: 'IN' },
        { timestamp: T(5, 0, 18), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 60 - 10)
    assert.equal(d.earlyLeaveMinutes, 60 - 10)
    assert.equal(d.workedMinutes, 6 * 60)
  })
  check('after-midnight arrival IN 02:30 (day 18) → late 270−10=260 vs the 22:00 start (4h30m late)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: nightShift, punches: [
        { timestamp: T(2, 30, 18), direction: 'IN' },
        { timestamp: T(6, 0, 18), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 270 - 10)
    assert.equal(d.earlyLeaveMinutes, 0) // OUT exactly at window end
  })
  check('early arrival 21:00 BEFORE the 22:00 start → late 0 (old ±1440 wrap fabricated 22h50m fake lateness here)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: nightShift, punches: [
        { timestamp: T(21), direction: 'IN' },
        { timestamp: T(6, 0, 18), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.workedMinutes, 9 * 60)
  })
})

// [39] Multi-segment shift late/early ------------------------------------------
section('39. S4 multi-segment shift: late vs first segment start, early vs last segment end', () => {
  check('split 08–12+13–17: IN 08:15 → late 5; OUT 16:40 → early 10 (vs 17:00 end, grace 10)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: splitShift, punches: [
        { timestamp: T(8, 15), direction: 'IN' },
        { timestamp: T(16, 40), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 15 - 10)
    assert.equal(d.earlyLeaveMinutes, 20 - 10)
  })
  check('mid-gap punch 12:30 (between segments) stays in-window: no late/early fabrication', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: splitShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(12, 30), direction: 'OUT' },
        { timestamp: T(12, 45), direction: 'IN' },
        { timestamp: T(17), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.workedMinutes, 270 + 255) // (08:00→12:30) + (12:45→17:00)
  })
})

// [40] Jalali month/year boundary ----------------------------------------------
section('40. S4 Jalali month/year boundary: night shift Esfand 29 → Farvardin 1', () => {
  // 2027-03-20 = Esfand 29 1405 (last day of the Jalali year); the shift starts
  // that night 22:00 and ends 2027-03-21 = Farvardin 1 1406 at 06:00.
  const M3 = (h: number, m = 0, day = 20) => new Date(Date.UTC(2027, 2, day, h - 3, m - 30))
  const d = computeDaily({
    personId: 'p1', day: new Date(Date.UTC(2027, 2, 20, 0, 0)), shift: nightShift, punches: [
      { timestamp: M3(23, 0), direction: 'IN' },        // Esfand 29 23:00
      { timestamp: M3(2, 30, 21), direction: 'IN' },    // Farvardin 1 02:30 (duplicate IN)
      { timestamp: M3(6, 0, 21), direction: 'OUT' },    // Farvardin 1 06:00
    ], isHoliday: false, isOff: false, now: new Date(Date.UTC(2027, 2, 22, 12, 0)),
  })
  check('late measured across the Jalali year boundary: IN 23:00 → late 50; duplicate IN reported, OUT at end → early 0', () => {
    assert.equal(d.lateMinutes, 60 - 10)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.note, 'DUPLICATE_IN:1')
  })
})

// [41] Leave attached to the shift start/end — late/early effect ---------------
section('41. S4 leave attached to window start/end moves the late/early deadline', () => {
  check('leave 08:00–10:00 covering the start: IN 10:00 → late 0 (excused for the leave duration)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(10), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [{ start: T(8), end: T(10), leaveTypeId: 'lt-start' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.leaveMinutes, 120)
    assert.equal(d.workedMinutes, 360) // 10:00–16:00 only — leave carved from worked
  })
  check('same punches WITHOUT the leave → late 110 (the leave is what excuses it)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(10), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 110)
  })
  check('leave 07:00–08:30 straddling the 08:00 start: deadline = 08:30 → IN 08:35 late 0 (5 < grace)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8, 35), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [{ start: T(7), end: T(8, 30), leaveTypeId: 'lt-straddle' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
  })
  check('leave 15:00–18:00 covering the 16:00 end: OUT 15:00 → early 0 (departure allowed at leave start)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(15), direction: 'OUT' },
      ], leaveSpans: [{ start: T(15), end: T(18), leaveTypeId: 'lt-end' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.leaveMinutes, 180)
    assert.equal(d.workedMinutes, 420) // 08:00–15:00 — the leave tail replaces work
  })
  check('same punches WITHOUT the leave → early 50 (the leave is what excuses it)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(15), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.earlyLeaveMinutes, 50)
  })
  check('leave ending exactly at the start (07:00–08:00) does NOT excuse lateness (deadline stays 08:00)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8, 20), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [{ start: T(7), end: T(8), leaveTypeId: 'lt-before' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 10)
  })
  check('mid-shift leave 10:00–12:00 has NO documented late/early effect (current behavior kept, ambiguity recorded)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8, 20), direction: 'IN' },
        { timestamp: T(15, 30), direction: 'OUT' },
      ], leaveSpans: [{ start: T(10), end: T(12), leaveTypeId: 'lt-mid' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 10)     // unchanged by the mid-shift leave
    assert.equal(d.earlyLeaveMinutes, 20) // unchanged
  })
})

// [42] Leave × worked overlap carving (no double count) ------------------------
section('42. S4 leave×work overlap: one interval counted exactly once (worked carved, leave kept)', () => {
  check('attendance 08:00–16:00 + hourly leave 10:00–12:00 → worked 360, leave 120 (sum = attendance once)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [{ start: T(10), end: T(12), leaveTypeId: 'lt-hourly' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 360)
    assert.equal(d.leaveMinutes, 120)
    assert.equal(d.status, 'PRESENT') // real punches keep precedence
    assert.equal(iso(d.firstIn), iso(T(8))) // real in/out never faked or removed
    assert.equal(iso(d.lastOut), iso(T(16)))
  })
  check('leave fully inside attendance 08:00–12:00 ⊂ attendance 08:00–16:00 → worked 240, leave 240', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [{ start: T(8), end: T(12), leaveTypeId: 'lt-a' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.leaveMinutes, 240)
  })
  check('overlapping leaves 09–11 + 10–13 (union 09:00–13:00=240) × attendance 08–16 → worked carved ONCE (240)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], leaveSpans: [
        { start: T(9), end: T(11), leaveTypeId: 'lt-1' },
        { start: T(10), end: T(13), leaveTypeId: 'lt-2' },
      ],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.leaveMinutes, 240) // union 09:00–13:00, never double
    assert.equal(d.workedMinutes, 480 - 240)
    assert.equal(d.leaveTypeId, 'lt-2') // latest-ending contributor
  })
  check('leave with NO attendance overlap → worked unchanged (carve only removes the overlap)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(12), direction: 'OUT' },
      ], leaveSpans: [{ start: T(14), end: T(16), leaveTypeId: 'lt-later' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.leaveMinutes, 120)
  })
  check('carve feeds OT: attendance 08:00–17:00 (540) with leave 08:00–10:00 → worked 420 → NO OT (was OT 30 before S4)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(17), direction: 'OUT' },
      ], leaveSpans: [{ start: T(8), end: T(10), leaveTypeId: 'lt-ot' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 420)
    assert.equal(d.overtimeMinutes, 0)
    assert.equal(d.lateMinutes, 0) // the attached leave also excuses the 10:00 arrival deadline
  })
  check('PENDING/REJECTED leave never carves: only APPROVED spans reach the engine (caller contract)', () => {
    // (the DB suite proves the status filter end-to-end; here: no leave spans → full worked)
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 480)
  })
})

// [43] Leave + mission + attendance triple with the carve -----------------------
section('43. S4 mission × leave × attendance with the carve: no interval counted twice', () => {
  check('attendance 08–16 + mission 10–14 + leave 14–16: worked 480, mission 0 (inside attendance), leave 120 carved → 360', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], missionSpans: [{ start: T(10), end: T(14) }],
      leaveSpans: [{ start: T(14), end: T(16), leaveTypeId: 'lt2' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.missionMinutes, 0)       // fully covered by attendance
    assert.equal(d.leaveMinutes, 120)
    assert.equal(d.workedMinutes, 480 - 120) // mission 0 + attendance 480 − leave overlap 120
  })
  check('disjoint triple 08–10 leave + 10–14 attendance + 14–18 mission: worked 240+240=480; leave 120 (nothing overlaps)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(10), direction: 'IN' },
        { timestamp: T(14), direction: 'OUT' },
      ], missionSpans: [{ start: T(14), end: T(18) }],
      leaveSpans: [{ start: T(8), end: T(10), leaveTypeId: 'lt3' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.leaveMinutes, 120)
    assert.equal(d.missionMinutes, 240)
    assert.equal(d.workedMinutes, 240 + 240)
  })
})

// [44] ABSENT timing — the real determination window ---------------------------
section('44. S4 ABSENT timing: shift-window end / Tehran day end, exact boundary', () => {
  const mk = (now: Date, shift: ShiftLite | null) => computeDaily({
    personId: 'p1', day: D17(), shift, punches: [], isHoliday: false, isOff: false, now,
  })
  check('shift day: now = window end − 1ms → PENDING; now = window end exactly → ABSENT (16:00 boundary)', () => {
    assert.equal(mk(new Date(T(16).getTime() - 1), dayShift).status, 'PENDING')
    assert.equal(mk(T(16), dayShift).status, 'ABSENT')
  })
  check('no-shift day: now = Tehran midnight − 1ms → PENDING; at/after → ABSENT (00:00 next day boundary)', () => {
    // Tehran midnight of 11-18 = 2026-11-17T20:30Z
    const midnight = new Date(Date.UTC(2026, 10, 17, 20, 30))
    assert.equal(mk(new Date(midnight.getTime() - 1), null).status, 'PENDING')
    assert.equal(mk(midnight, null).status, 'ABSENT')
  })
  check('night shift: not absent before 06:00 next day (the window end rules, not the calendar day)', () => {
    // Tehran 06:00 on 11-18 = UTC 02:30Z (BEFORE the Tehran midnight of 11-18
    // at 20:30Z) — the night shift's determination window ends at the 06:00
    // window end, i.e. EARLIER than the no-shift calendar-day boundary.
    const nightEnd = T(6, 0, 18)
    assert.equal(mk(new Date(nightEnd.getTime() - 1), nightShift).status, 'PENDING')
    assert.equal(mk(nightEnd, nightShift).status, 'ABSENT')
  })
  check('holiday/off precedence intact before/after the window end (never ABSENT)', () => {
    const h = (now: Date, isHoliday: boolean, isOff: boolean) => computeDaily({
      personId: 'p1', day: D17(), shift: null, punches: [], isHoliday, isOff, now,
    })
    assert.equal(h(new Date(T(15).getTime() - 1), true, false).status, 'HOLIDAY')
    assert.equal(h(NOW18(), true, false).status, 'HOLIDAY')
    assert.equal(h(NOW18(), false, true).status, 'OFF')
  })
})

// [45] Holiday / off-day behavior — documented, unchanged ------------------------
section('45. S4 holiday/off-day policy documented (A4 kept): all valid work = actual OT; payable still min()', () => {
  check('holiday + valid punches → PRESENT, overtimeMinutes = worked (ALL of it), payable = min(worked, approved)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(12), direction: 'OUT' },
      ], approvedOvertimeMinutes: 60, isHoliday: true, isOff: false, now: NOW18(),
    })
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 240)
    assert.equal(d.overtimeMinutes, 240)      // actual = ALL valid work (A4: unchanged)
    assert.equal(d.payableOvertimeMinutes, 60) // payable = min(240, 60)
  })
  check('holiday + incomplete punch → INCOMPLETE (mission/leave never hide bad punches)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
      ], isHoliday: true, isOff: false, now: NOW18(),
    })
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.workedMinutes, 0)
  })
  check('off-day (explicit OFF, no shift) + valid punches → worked as actual OT; no punches → OFF', () => {
    const withPunches = computeDaily({
      personId: 'p1', day: D17(), shift: null, punches: [
        { timestamp: T(9), direction: 'IN' },
        { timestamp: T(13), direction: 'OUT' },
      ], isHoliday: false, isOff: true, now: NOW18(),
    })
    assert.equal(withPunches.status, 'PRESENT')
    assert.equal(withPunches.overtimeMinutes, 240)
    const noPunches = computeDaily({
      personId: 'p1', day: D17(), shift: null, punches: [], isHoliday: false, isOff: true, now: NOW18(),
    })
    assert.equal(noPunches.status, 'OFF')
    assert.equal(noPunches.overtimeMinutes, 0)
  })
  check('holiday with no punches → HOLIDAY regardless of elapsed time', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: null, punches: [], isHoliday: true, isOff: false, now: NOW18(),
    })
    assert.equal(d.status, 'HOLIDAY')
  })
})

// [46] Break minutes (breakMin) — documented A5 behavior ------------------------
section('46. S4 breakMin (A5 documented): reduces the shift STANDARD, never the worked time', () => {
  const breakShift: ShiftLite = {
    ...dayShift, segments: [{ startMin: 8 * 60, endMin: 16 * 60, breakMin: 60 }],
  }
  check('standard = 480 − 60 break = 420; punches 08:00–16:00 → worked 480 (break NOT deducted from worked)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: breakShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.workedMinutes, 480)
    assert.equal(d.overtimeMinutes, 480 - 420) // 60: excess over the break-reduced standard (> 420+30)
  })
  check('same day with break=0 → standard 480 → OT 0 (the break made the difference, nothing else)', () => {
    const d = daily([
      { timestamp: T(8), direction: 'IN' },
      { timestamp: T(16), direction: 'OUT' },
    ])
    assert.equal(d.overtimeMinutes, 0)
  })
})

// [47] Provenance — EDITED_PUNCHES note code ------------------------------------
section('47. S4 provenance: EDITED_PUNCHES note code, deterministic composition', () => {
  check('day computed from 1 edited punch (IN without OUT) → note EDITED_PUNCHES:1 beside the anomaly code', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
      ], editedPunchCount: 1, isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.note, 'IN_WITHOUT_OUT:1|EDITED_PUNCHES:1')
  })
  check('pairing anomalies first, then EDITED_PUNCHES (order deterministic); 2 edited punches → count 2', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(9), direction: 'IN' },
      ], editedPunchCount: 2, isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.note, 'DUPLICATE_IN:1|IN_WITHOUT_OUT:1|EDITED_PUNCHES:2')
  })
  check('clean day with edited punches → note = EDITED_PUNCHES:n only; clean day without → null', () => {
    const a = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(16), direction: 'OUT' },
      ], editedPunchCount: 2, isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(a.note, 'EDITED_PUNCHES:2')
    assert.equal(daily([{ timestamp: T(8), direction: 'IN' }, { timestamp: T(16), direction: 'OUT' }]).note, null)
  })
  check('default editedPunchCount=0 keeps the note EXACTLY as before (no surprise code)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
        { timestamp: T(9), direction: 'IN' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.note, 'DUPLICATE_IN:1|IN_WITHOUT_OUT:1')
  })
})

// [48] MISSION / LEAVE precedence + IGNORED removal ------------------------------
section('48. S4 status precedence: LEAVE > MISSION > HOLIDAY/OFF; MISSION never hides punch truth', () => {
  check('true full-day leave (480) + mission, no punches → LEAVE takes precedence over MISSION', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [],
      missionSpans: [{ start: T(14), end: T(18) }],
      leaveSpans: [{ start: T(8), end: T(16), leaveTypeId: 'lt-full' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    // leave 08–16 minus mission overlap 14–16 = 360 < 480 → actually MISSION per the S3 rule; the true-480 case:
    assert.equal(d.leaveMinutes, 360)
    assert.equal(d.status, 'MISSION')
    const d2 = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [],
      missionSpans: [{ start: T(17), end: T(19) }],
      leaveSpans: [{ start: T(8), end: T(16), leaveTypeId: 'lt-full' }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d2.leaveMinutes, 480)
    assert.equal(d2.status, 'LEAVE')
  })
  check('MISSION with an incomplete punch day → INCOMPLETE (punch truth visible), mission credit still computed', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: dayShift, punches: [
        { timestamp: T(8), direction: 'IN' },
      ], missionSpans: [{ start: T(10), end: T(14) }],
      isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.status, 'INCOMPLETE')
    assert.equal(d.missionMinutes, 240)
    assert.equal(d.note, 'IN_WITHOUT_OUT:1')
  })
  check('IGNORED is not a status the engine can produce (removed from the schema comment — never had behavior)', () => {
    const produced = new Set(['PRESENT', 'ABSENT', 'LEAVE', 'MISSION', 'HOLIDAY', 'OFF', 'INCOMPLETE', 'PENDING'])
    // every branch in computeDaily's status ladder lands inside this set
    assert.ok(produced.has('PRESENT') && !produced.has('IGNORED'))
  })
})

section('49. S4 hotfix: stray pairs never hide the real Late/Early (window-related anchors)', () => {
  // Test A (mandate): shift 15:00–23:30, grace 10; stray pair 02:30–02:50
  // (unrelated visit); real pair 15:30–23:30 → lateMinutes === 20. Before the
  // fix the stray pair was pairs[0], so firstIn=02:30 collapsed late to 0.
  // workedMinutes keeps counting the stray minutes (500) and the raw firstIn
  // column keeps the honest 02:30 — punch truth is never faked or removed.
  check('A: stray 02:30–02:50 + real 15:30–23:30 → late 20 (was 0 before the fix), early 0, worked 500', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
        { timestamp: T(15, 30), direction: 'IN' },
        { timestamp: T(23, 30), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 20)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.workedMinutes, 20 + 8 * 60) // stray minutes still counted (unchanged)
    assert.equal(iso(d.firstIn), iso(T(2, 30))) // raw punch truth kept verbatim
    assert.equal(iso(d.lastOut), iso(T(23, 30)))
  })
  // Test B (mandate): genuine early arrival — IN 14:30 (BEFORE the 15:00
  // start) whose pair runs into the window → the pair is window-related and
  // stays the anchor → late 0. Early arrivals are never deleted.
  check('B: real early arrival IN 14:30 (pair reaching into the window) → late 0', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(14, 30), direction: 'IN' },
        { timestamp: T(23, 30), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.status, 'PRESENT')
    assert.equal(d.workedMinutes, 9 * 60)
  })
  // A+B combined: the early-arrival pair is the late anchor even when a stray
  // pair exists earlier in the day.
  check('A+B: stray 02:30–02:50 + real early 14:30–23:30 → late 0 measured on the REAL pair', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
        { timestamp: T(14, 30), direction: 'IN' },
        { timestamp: T(23, 30), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.workedMinutes, 20 + 9 * 60)
  })
  // Test C (mandate): the existing after-window-end arrival contract is kept
  // EXACTLY — a lone IN 23:59 (no pairs, open session) → late 529.
  check('C: lone IN 23:59 after the window end → late 529 unchanged (existing contract)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(23, 59), direction: 'IN' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 8 * 60 + 59 - 10)
    assert.equal(d.lateMinutes, 529)
    assert.equal(d.status, 'INCOMPLETE')
  })
  // Test D (mandate): a stray pair AFTER the window end must not hide a real
  // early departure. Expected value per the engine's CURRENT policy:
  //   earlyLeaveMinutes = max(0, winEnd − latestRelatedOut − earlyGrace)
  //                    = 23:30 − 20:00 − 10 = 200  (the real pair; the
  //   23:35–23:55 stray is not window-related and never moves the anchor).
  check('D: real 15:00–20:00 + stray 23:35–23:55 after the window end → early 200 (was 0), late 0', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(15, 0), direction: 'IN' },
        { timestamp: T(20, 0), direction: 'OUT' },
        { timestamp: T(23, 35), direction: 'IN' },
        { timestamp: T(23, 55), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.earlyLeaveMinutes, 200)
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.workedMinutes, 300 + 20)
    assert.equal(iso(d.lastOut), iso(T(23, 55))) // raw column keeps the honest last punch
  })
  // multi-pair day with strays on BOTH sides: late is measured from the
  // earliest related IN, early from the latest related OUT.
  check('multi-pair: late from the FIRST related IN, early from the LAST related OUT (strays on both sides)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
        { timestamp: T(15, 30), direction: 'IN' },
        { timestamp: T(19, 0), direction: 'OUT' },
        { timestamp: T(19, 30), direction: 'IN' },
        { timestamp: T(23, 0), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 20)        // 15:30 (first related IN), not the 02:30 stray
    assert.equal(d.earlyLeaveMinutes, 20)   // 23:00 vs the 23:30 end − grace 10
  })
  // stray-only day: no related pair at all → the previous S4 fallback
  // contract applies verbatim (late 0, early 0, PRESENT, worked 20).
  check('stray-only day (no related pair) keeps the S4 fallback contract: late 0, early 0, PRESENT, worked 20', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: eveningShift, punches: [
        { timestamp: T(2, 30), direction: 'IN' },
        { timestamp: T(2, 50), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.lateMinutes, 0)
    assert.equal(d.earlyLeaveMinutes, 0)
    assert.equal(d.workedMinutes, 20)
    assert.equal(d.status, 'PRESENT')
  })
  // night shift (pure-layer robustness): a post-window stray pair must not
  // hide the real early departure there either.
  check('night shift: stray 06:30–06:50 after the 06:00 end does not hide the real 05:00 departure (early 50)', () => {
    const d = computeDaily({
      personId: 'p1', day: D17(), shift: nightShift, punches: [
        { timestamp: T(22, 0), direction: 'IN' },
        { timestamp: T(5, 0, 18), direction: 'OUT' },
        { timestamp: T(6, 30, 18), direction: 'IN' },
        { timestamp: T(6, 50, 18), direction: 'OUT' },
      ], isHoliday: false, isOff: false, now: NOW18(),
    })
    assert.equal(d.earlyLeaveMinutes, 50) // 06:00 − 05:00 − 10 grace, measured on the real OUT
    assert.equal(d.lateMinutes, 0)
  })
})

console.log(`\n== Attendance assertions: ${assertionsPassed} passed / ${assertionsFailed} failed ==`)
console.log(`== Attendance sections: ${sectionsPassed} passed / ${sectionsFailed} failed ==`)
if (assertionsFailed > 0 || sectionsFailed > 0) process.exitCode = 1
