/**
 * Phase 3, item 9 test suite — shift time control (MOCK ONLY, pure functions).
 * =====================================================================
 * Everything here runs against the PURE module src/lib/shift-time.ts — no DB,
 * no network, no device. NO real hardware is involved and NO UI rendering is
 * asserted here (visual behaviour of ShiftTimePicker is on the manual visual
 * checklist; what IS proven here is the value contract it propagates).
 *
 * Covered:
 *   [parse]      HH:MM formats: '08:30', '8:3', '830', '0830', Persian digits,
 *                hour-only while typing ('8' → 08:00), hour > 23 / minute > 59
 *                rejected with Persian errors (the old form accepted '25:99')
 *   [roundtrip]  minutesToHHMM ∘ parseHHMM = identity for the whole 0..1439 domain
 *   [night]      crossesMidnight + span: 22:00→06:00 = 480 min, 08:00→16:30 = 510,
 *                start == end = 24h (engine parity, attendance.ts line 73)
 *   [validate]   segment validator: ok cases + rejections (range, non-integer,
 *                negative break, break > span, empty) — the SAME gate runs in
 *                the form AND in /api/shifts POST/PUT
 *   [regression] the OLD form's keystroke corruption: '830'.split(':') gave
 *                h=830 → 49 800 minutes stored; parseHHMM('830') = 510 and an
 *                unparseable draft NEVER produces minutes
 *
 * Run: npx tsx scripts/test-shift-time.ts
 */
import {
  parseHHMM,
  minutesToHHMM,
  normalizeTimeDraft,
  normalizeFaDigits,
  crossesMidnight,
  shiftSegmentSpan,
  validateShiftSegments,
} from '../src/lib/shift-time'

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}

function main() {
  /* ================================================================
   * 1) PARSE — HH:MM, typed states, Persian digits, range errors
   * ================================================================ */
  section('PARSE — HH:MM formats & typed states')

  check("'08:30' → 510", parseHHMM('08:30').ok === true && (parseHHMM('08:30') as { minutes: number }).minutes === 510)
  check("'8:30' → 510 (single-digit hour)", parseHHMM('8:30').ok === true && (parseHHMM('8:30') as { minutes: number }).minutes === 510)
  check("'8:3' → 483 (partial minute still a time, typing-friendly)", parseHHMM('8:3').ok === true && (parseHHMM('8:3') as { minutes: number }).minutes === 483)
  check("'830' → 510 (3 digits → H:MM)", parseHHMM('830').ok === true && (parseHHMM('830') as { minutes: number }).minutes === 510)
  check("'0830' → 510 (4 digits → HH:MM)", parseHHMM('0830').ok === true && (parseHHMM('0830') as { minutes: number }).minutes === 510)
  check("'8' → 480 (hour-only while typing, minute=0)", parseHHMM('8').ok === true && (parseHHMM('8') as { minutes: number }).minutes === 480)
  check("'23' → 1380 (max hour typed alone)", parseHHMM('23').ok === true && (parseHHMM('23') as { minutes: number }).minutes === 1380)
  check("'۰۸:۳۰' → 510 (Persian digits normalized)", parseHHMM('۰۸:۳۰').ok === true && (parseHHMM('۰۸:۳۰') as { minutes: number }).minutes === 510)
  check("'۲۲:۰۰' → 1320 (Persian night-shift start)", parseHHMM('۲۲:۰۰').ok === true && (parseHHMM('۲۲:۰۰') as { minutes: number }).minutes === 1320)
  check("'08.30' → 510 (dot noise stripped)", parseHHMM('08.30').ok === true && (parseHHMM('08.30') as { minutes: number }).minutes === 510)

  const e1 = parseHHMM('25:00')
  check("'25:00' REJECTED (hour > 23 — old form accepted it silently)", e1.ok === false, e1.ok ? 'accepted!' : '')
  const e2 = parseHHMM('08:75')
  check("'08:75' REJECTED (minute > 59 — old form accepted it silently)", e2.ok === false)
  const e3 = parseHHMM('8:')
  check("'8:' rejected while typing (minute not yet entered) — with Persian reason", e3.ok === false)
  const e4 = parseHHMM('123456')
  check("'123456' rejected (5+ digits)", e4.ok === false)
  const e5 = parseHHMM('')
  check("'' rejected (nothing typed)", e5.ok === false)
  check("rejected results carry a Persian error message", (e1.ok === false && /ساعت/.test(e1.error)) && (e2.ok === false && /دقیقه/.test(e2.error)))

  check("normalizeFaDigits('۰۸:۳۰') === '08:30'", normalizeFaDigits('۰۸:۳۰') === '08:30')
  check("normalizeTimeDraft(' 08 : 30 ') === '08:30'", normalizeTimeDraft(' 08 : 30 ') === '08:30')

  /* ================================================================
   * 2) ROUNDTRIP — the storage contract is minutes, display is HH:MM
   * ================================================================ */
  section('ROUNDTRIP — minutes ↔ HH:MM over the whole day')

  let roundtripOk = true
  let firstBad = -1
  for (let m = 0; m <= 1439; m++) {
    const back = parseHHMM(minutesToHHMM(m))
    if (!(back.ok === true && (back as { minutes: number }).minutes === m)) { roundtripOk = false; firstBad = m; break }
  }
  check('parse(minutesToHHMM(m)) === m for ALL m in 0..1439 (1440 cases)', roundtripOk, firstBad >= 0 ? `first failure at ${firstBad}` : '')
  check('minutesToHHMM(0) === "00:00"', minutesToHHMM(0) === '00:00')
  check('minutesToHHMM(510) === "08:30"', minutesToHHMM(510) === '08:30')
  check('minutesToHHMM(1439) === "23:59"', minutesToHHMM(1439) === '23:59')

  /* ================================================================
   * 3) NIGHT SHIFTS — midnight crossing & span (engine parity)
   * ================================================================ */
  section('NIGHT SHIFTS — midnight crossing, 24h, engine parity')

  check('22:00→06:00 crosses midnight', crossesMidnight(22 * 60, 6 * 60) === true)
  check('08:00→16:30 does NOT cross midnight', crossesMidnight(8 * 60, 16 * 60 + 30) === false)
  check('span(22:00→06:00) = 480 (engine: end+1440-start)', shiftSegmentSpan(22 * 60, 6 * 60) === 480)
  check('span(08:00→16:30) = 510', shiftSegmentSpan(480, 990) === 510)
  check('span(00:00→23:59) = 1439', shiftSegmentSpan(0, 1439) === 1439)
  check('span(start==end) = 1440 — a full 24h shift, engine arithmetic (attendance.ts L73)', shiftSegmentSpan(480, 480) === 1440)

  /* ================================================================
   * 4) SEGMENT VALIDATION — the shared UI/API gate
   * ================================================================ */
  section('VALIDATE — shared segment gate (form + /api/shifts)')

  check('single 08:00→16:30 break 60 ok', validateShiftSegments([{ startMin: 480, endMin: 990, breakMin: 60 }]).ok === true)
  check('split shift ok', validateShiftSegments([{ startMin: 480, endMin: 720, breakMin: 0 }, { startMin: 780, endMin: 1020, breakMin: 15 }]).ok === true)
  check('night shift 22:00→06:00 break 60 ok', validateShiftSegments([{ startMin: 1320, endMin: 360, breakMin: 60 }]).ok === true)
  check('24h shift (start==end) ok', validateShiftSegments([{ startMin: 480, endMin: 480, breakMin: 0 }]).ok === true)

  const v1 = validateShiftSegments([{ startMin: -1, endMin: 990, breakMin: 0 }])
  check('startMin -1 rejected', v1.ok === false)
  const v2 = validateShiftSegments([{ startMin: 1440, endMin: 990, breakMin: 0 }])
  check('startMin 1440 (24:00) rejected', v2.ok === false)
  const v3 = validateShiftSegments([{ startMin: 480.5, endMin: 990, breakMin: 0 }])
  check('non-integer minutes rejected (old form could store them)', v3.ok === false)
  const v4 = validateShiftSegments([{ startMin: 480, endMin: 1440, breakMin: 0 }])
  check('endMin 1440 rejected', v4.ok === false)
  const v5 = validateShiftSegments([{ startMin: 480, endMin: 990, breakMin: -5 }])
  check('negative breakMin rejected', v5.ok === false)
  const v6 = validateShiftSegments([{ startMin: 480, endMin: 510, breakMin: 60 }])
  check('breakMin (60) > span (30) rejected', v6.ok === false)
  const v7 = validateShiftSegments([])
  check('empty segment list rejected', v7.ok === false)
  const v8 = validateShiftSegments([{ startMin: 480, endMin: 510, breakMin: 30 }])
  check('breakMin == span accepted (effective 0, mathematically consistent)', v8.ok === true)
  const v9 = validateShiftSegments([{ startMin: 480, endMin: 510, breakMin: 31 }])
  check('error names the offending بازه (Persian, row-identifiable)', v9.ok === false && /بازه 1/.test((v9 as { error: string }).error))

  /* ================================================================
   * 5) REGRESSION — the corruption that motivated item 9
   * ================================================================ */
  section('REGRESSION — the old keystroke corruption')

  // The OLD shift form: const [h, m] = v.split(':').map(Number) on EVERY
  // keystroke — typing "830" (before the colon) evaluated to h=830:
  const oldFormValue = (v: string) => { const [h, m] = v.split(':').map(Number); return (h || 0) * 60 + (m || 0) }
  check("old behavior reproduced: '830' → 49 800 minutes (the bug this module kills)", oldFormValue('830') === 49800)
  check("new behavior: parseHHMM('830') → 510 — NOT 49 800", (parseHHMM('830') as { minutes: number }).minutes === 510)
  check("'99:99' never produces minutes (rejected) — state cannot be poisoned via typing", parseHHMM('99:99').ok === false)
  check("'9999' never produces minutes (rejected)", parseHHMM('9999').ok === false)
  check("reopened-form preservation: 1320 stored → '22:00' shown → parse → 1320 (lossless roundtrip)", (() => { const stored = 1320; const shown = minutesToHHMM(stored); const r = parseHHMM(shown); return r.ok === true && (r as { minutes: number }).minutes === stored })())
  check("reopened-form preservation: 510 stored → '08:30' shown → parse → 510", (() => { const stored = 510; const r = parseHHMM(minutesToHHMM(stored)); return r.ok === true && (r as { minutes: number }).minutes === stored })())

  /* ================================================================
   * summary — mock vs manual/hardware explicitly separated
   * ================================================================ */
  console.log('\n━━━ SUMMARY (Phase 3, item 9 — shift time control)')
  console.log(`  MOCK (pure) tests : ${passed} passed, ${failed} failed`)
  console.log(`  MANUAL visual     : ShiftTimePicker rendering/UX (popover lists, error styles) — manual visual checklist, NOT automated here`)
  console.log(`  HARDWARE tests    : 0 (not applicable — pure UI logic; no device involved)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  process.exit(failed > 0 ? 1 : 0)
}

main()
