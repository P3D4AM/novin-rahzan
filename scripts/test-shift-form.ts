/**
 * Stage 7 review fix test suite — the ShiftTimePicker ⇄ parent-form draft
 * validation gate (MOCK ONLY, pure functions).
 * =====================================================================
 * Everything here drives the EXACT pure functions both React components use
 * (src/lib/shift-time.ts: draftValidity / setDraftValidity / hasInvalidDrafts /
 * parseHHMM / validateShiftSegments) through the interaction sequences the
 * spec demands. NO DOM rendering is asserted (the repo has no React-render
 * test infrastructure by design — visual behaviour stays on the manual
 * checklist, like every other suite here); what IS proven is the decision &
 * aggregation contract the components are wired to:
 *
 *   * the picker's onTyping: only a VALID parse propagates minutes; an
 *     unparseable draft reports invalidity to the parent (the Stage 7 defect
 *     was: the invalid draft lived ONLY in the picker's internal state)
 *   * the parent's aggregate map: ANY invalid picker ⇒ save gate closed
 *   * the save HANDLER re-checks both gates on its own (button-independent)
 *   * removing a segment releases exactly that row's picker keys (stable
 *     per-segment ids — an invalid draft can never transfer to another row)
 *   * form (re)load / reset ⇒ fresh ids + cleared map (no stale draft)
 *
 * Run: npx tsx scripts/test-shift-form.ts
 */
import {
  parseHHMM,
  minutesToHHMM,
  draftValidity,
  setDraftValidity,
  hasInvalidDrafts,
  validateShiftSegments,
  type DraftValidityMap,
} from '../src/lib/shift-time'

let passed = 0
let failed = 0
const failures: string[] = []
function section(t: string) { console.log(`\n━━━ ${t}`) }
function check(name: string, cond: boolean, detail?: string) {
  if (cond) { passed++; console.log(`  ✔ ${name}`) }
  else { failed++; failures.push(name + (detail ? ` — ${detail}` : '')); console.log(`  ✘ ${name}${detail ? `  (${detail})` : ''}`) }
}

/** One ShiftTimePicker, driven exactly like the component's own handlers. */
interface PickerState {
  /** the picker's internal draft (null = not editing) */
  draft: string | null
  /** the parent form's stored minutes for this field */
  stored: number
  /** validity reported to the parent so far (null = not yet reported) */
  reported: boolean | null
  /** minutes propagated to the parent's onChange so far (null = none) */
  propagated: number | null
}

function newPicker(stored: number): PickerState {
  return { draft: null, stored, reported: null, propagated: null }
}

/** ShiftTimePicker.onTyping — the exact decision sequence of the component. */
function onTyping(p: PickerState, raw: string) {
  const cleaned = raw.replace(/[۰-۹]/g, (d) => String('۰۱۲۳۴۵۶۷۸۹'.indexOf(d)))
    .replace(/[٠-٩]/g, (d) => String('٠١٢٣٤٥٦٧٨٩'.indexOf(d)))
    .replace(/[^\d:]/g, '')
  p.draft = cleaned
  p.reported = draftValidity(cleaned) // ← what onValidityChange sends the parent
  if (cleaned === '') return p // empty draft keeps the stored value
  const r = parseHHMM(cleaned)
  if (r.ok) { p.stored = r.minutes; p.propagated = r.minutes } // onChange(minutes)
  return p
}

/** ShiftTimePicker.onBlur — abandon an invalid edit. */
function onBlur(p: PickerState) {
  p.draft = null
  p.reported = true
  return p
}

/** The parent's map update on receiving an onValidityChange report. */
function applyReport(map: DraftValidityMap, key: string, p: PickerState): DraftValidityMap {
  return setDraftValidity(map, key, p.reported === null ? true : p.reported)
}

/** The parent's save gate — the exact sequence of ShiftsTab.save(). */
function saveGate(form: { name: string; segments: { startMin: number; endMin: number; breakMin: number }[] }, map: DraftValidityMap) {
  if (!form.name.trim()) return { allowed: false, reason: 'name' }
  const segv = validateShiftSegments(form.segments)
  if (!segv.ok) return { allowed: false, reason: 'segments' }
  if (hasInvalidDrafts(map)) return { allowed: false, reason: 'draft' }
  return { allowed: true, reason: '' }
}

/** The save BUTTON's disabled condition in the dialog. */
function buttonDisabled(form: { segments: unknown }, map: DraftValidityMap) {
  return !validateShiftSegments(form.segments).ok || hasInvalidDrafts(map)
}

function main() {
  /* ================================================================
   * 1) THE STAGE 7 REPRODUCTION — 25:99 into a stored 08:30
   * ================================================================ */
  section('REPRODUCTION — invalid draft must close the save gate')

  // stored start = 08:30; user types '25:99'
  const seg = { startMin: 510, endMin: 990, breakMin: 60 }
  const form = { name: 'اداری صبح', segments: [{ ...seg }] }
  let map: DraftValidityMap = {}
  const start = newPicker(510)

  // before typing: nothing invalid, save gate open, validation state correct
  check('initial load: empty map ⇔ valid — save gate OPEN on a clean form', saveGate(form, map).allowed === true)
  check('initial load: draftValidity(null) === true (no edit in progress)', draftValidity(null) === true)

  onTyping(start, '25:99')
  map = applyReport(map, '1:start', start)
  check("typing '25:99': the picker reports INVALIDITY to the parent (the Stage 7 defect: this never left the picker)", start.reported === false)
  check("typing '25:99': NO minutes propagate — the form still holds the previous VALID 08:30 (510)", start.propagated === null && form.segments[0].startMin === 510)
  check("typing '25:99': the parent's aggregate map now knows (key '1:start' present)", map['1:start'] === true)
  check("typing '25:99': save BUTTON disabled (old behavior: stayed enabled and could save the stale 08:30)", buttonDisabled(form, map) === true)
  check("typing '25:99': save HANDLER refuses independently of the button", saveGate(form, map).allowed === false && saveGate(form, map).reason === 'draft')

  // the user fixes the input → 08:45
  onTyping(start, '08:45')
  map = applyReport(map, '1:start', start)
  check("fixing the input to '08:45': the picker reports validity again", start.reported === true)
  check("fixing the input: minutes propagate (onChange(525)) — the form updates to 525", start.propagated === 525)
  check("fixing the input: the parent's map drains (key removed)", hasInvalidDrafts(map) === false)
  check('fixing the input: save button re-enabled, save handler allows', buttonDisabled(form, map) === false && saveGate(form, map).allowed === true)

  /* ================================================================
   * 2) MULTIPLE PICKERS — one invalid out of many blocks the save
   * ================================================================ */
  section('AGGREGATE — one invalid picker among many blocks save')

  const form2 = {
    name: 'چندبخشی',
    segments: [
      { startMin: 480, endMin: 720, breakMin: 0 },
      { startMin: 780, endMin: 1020, breakMin: 15 },
    ],
  }
  let map2: DraftValidityMap = {}
  const s1start = newPicker(480), s1end = newPicker(720), s2start = newPicker(780), s2end = newPicker(1020)

  check('clean split form: save allowed', saveGate(form2, map2).allowed === true)

  // only the SECOND segment's END picker goes invalid
  onTyping(s2end, '18:75')
  map2 = applyReport(map2, '2:end', s2end)
  check("only بازه ۲'s end picker invalid ('18:75'): reported invalid", s2end.reported === false)
  check("only one invalid among four pickers: the aggregate map has exactly ONE key", Object.keys(map2).length === 1 && map2['2:end'] === true)
  check('only one invalid among four pickers: save disabled for the WHOLE form', buttonDisabled(form2, map2) === true)
  check('only one invalid among four pickers: save handler refuses (reason: draft)', saveGate(form2, map2).reason === 'draft')

  // fixing just that one picker re-opens the gate — and ONLY that one
  onTyping(s2end, '17:00')
  map2 = applyReport(map2, '2:end', s2end)
  check('fixing that single picker: map drains, save re-enabled', hasInvalidDrafts(map2) === false && buttonDisabled(form2, map2) === false)

  /* ================================================================
   * 3) REMOVING THE INVALID SEGMENT — the draft must not transfer
   * ================================================================ */
  section('REMOVAL — deleting the invalid segment cleans the form state')

  const form3 = {
    name: 'امنیت',
    segments: [
      { startMin: 480, endMin: 720, breakMin: 0 },
      { startMin: 1320, endMin: 360, breakMin: 60 },
    ],
  }
  let map3: DraftValidityMap = {}
  const aStart = newPicker(480), aEnd = newPicker(720), bStart = newPicker(1320), bEnd = newPicker(360)

  // the user types garbage into بازه ۱'s start picker
  onTyping(aStart, '25:99')
  map3 = applyReport(map3, '1:start', aStart)
  check("بازه ۱ start invalid ('25:99'): save blocked", buttonDisabled(form3, map3) === true)

  // remove بازه ۱ — React unmounts EXACTLY that row's pickers (stable ids);
  // their unmount cleanup reports true → the parent drops their keys
  map3 = setDraftValidity(map3, '1:start', true) // ← the picker's unmount release
  form3.segments = form3.segments.filter((_, i) => i !== 0)
  check('removing the invalid segment: its keys are released → map drains', hasInvalidDrafts(map3) === false)
  check('removing the invalid segment: the remaining segment is untouched (22:00→06:00 intact)', form3.segments.length === 1 && form3.segments[0].startMin === 1320 && form3.segments[0].endMin === 360)
  check('removing the invalid segment: save re-enabled with the remaining valid form', buttonDisabled(form3, map3) === false && saveGate(form3, map3).allowed === true)
  check('no invalid draft TRANSFERRED onto the remaining row (its pickers were never reported invalid)', bStart.reported === null && bEnd.reported === null)

  /* ================================================================
   * 4) SHIFT CHANGE / RESET — no stale drafts survive into the next form
   * ================================================================ */
  section('RESET — shift change & form reset leave no stale draft')

  // simulate an ABANDONED invalid edit: the dialog closed with '25:99' typed
  const stale = newPicker(510)
  onTyping(stale, '25:99')
  let mapStale: DraftValidityMap = { '1:start': true } // parent got the invalid report

  // dialog closes → pickers unmount → each releases its key
  mapStale = setDraftValidity(mapStale, '1:start', true)
  check('dialog closed: unmount releases the invalid key — map drains', hasInvalidDrafts(mapStale) === false)

  // reopening ANOTHER shift (openEdit): fresh ids + cleared map — the new
  // form's validation state starts correctly clean
  const mapReopened: DraftValidityMap = {} // setInvalidDrafts({}) in openEdit
  const nightShift = { name: 'شب', segments: [{ startMin: 1320, endMin: 360, breakMin: 60 }] }
  check('openEdit of a night shift: fresh map + valid segments → save gate correctly OPEN', saveGate(nightShift, mapReopened).allowed === true)
  check('openEdit: the new pickers start with no draft (null → valid)', draftValidity(null) === true)

  // openNew (blank form): fresh ids, cleared map
  const blankForm = { name: '', segments: [{ startMin: 480, endMin: 720, breakMin: 0 }] }
  check('openNew: name gate still enforced (blank name refuses save)', saveGate(blankForm, mapReopened).allowed === false && saveGate(blankForm, mapReopened).reason === 'name')

  /* ================================================================
   * 5) BLUR — abandoning an invalid edit restores validity
   * ================================================================ */
  section('BLUR — abandoning an invalid edit')

  const p = newPicker(510)
  let mapB: DraftValidityMap = {}
  onTyping(p, '25:99')
  mapB = applyReport(mapB, '1:start', p)
  check('before blur: invalid (save blocked)', hasInvalidDrafts(mapB) === true)
  onBlur(p)
  mapB = applyReport(mapB, '1:start', p)
  check('after blur: the edit is abandoned — the picker reports valid again', p.reported === true && p.draft === null)
  check('after blur: the stored value stands (08:30) and the map drains', p.stored === 510 && hasInvalidDrafts(mapB) === false)
  check('after blur: display shows the stored value (draft null → minutesToHHMM(stored))', minutesToHHMM(510) === '08:30')

  /* ================================================================
   * 6) SAVE HANDLER — refuses invalid input even when the button lies
   * ================================================================ */
  section('HANDLER — the save handler refuses on its own (button-independent)')

  // (a) segments valid, map poisoned (e.g. devtools re-enabled the button)
  const validForm = { name: 'X', segments: [{ startMin: 480, endMin: 720, breakMin: 0 }] }
  const poisonedMap: DraftValidityMap = { '1:start': true }
  check('(a) valid segments + live invalid draft ⇒ handler REFUSES', saveGate(validForm, poisonedMap).allowed === false && saveGate(validForm, poisonedMap).reason === 'draft')

  // (b) map clean but segments invalid (breakMin '-5' typed → Number('-5'))
  const badSegForm = { name: 'X', segments: [{ startMin: 480, endMin: 720, breakMin: -5 }] }
  check('(b) clean map + invalid segments ⇒ handler REFUSES', saveGate(badSegForm, {}).allowed === false && saveGate(badSegForm, {}).reason === 'segments')

  // (c) both invalid
  check('(c) both invalid ⇒ handler refuses (segments checked first, same as the real save())', saveGate(badSegForm, poisonedMap).reason === 'segments')

  // (d) everything valid
  check('(d) valid segments + clean map ⇒ handler allows', saveGate(validForm, {}).allowed === true)

  /* ================================================================
   * 7) THE PICKER'S VALUE CONTRACT — valid parses propagate, garbage never
   * ================================================================ */
  section('VALUE CONTRACT — what a typing session actually propagates')

  const q = newPicker(1320) // 22:00
  onTyping(q, '2')
  check("typing '2' (partial hour): propagates 02:00 (120) — live-parse on valid input", q.propagated === 120)
  onTyping(q, '22')
  check("typing '22': propagates 22:00 (1320)", q.propagated === 1320)
  onTyping(q, '22:0')
  check("typing '22:0' (partial minute): propagates 22:00 (1320)", q.propagated === 1320)
  onTyping(q, '22:05')
  check("typing '22:05': propagates 1325", q.propagated === 1325)
  onTyping(q, '8::30')
  check("typing '8::30' (malformed): NOTHING propagates, invalidity reported", q.propagated === 1325 && q.reported === false)
  onTyping(q, '۲۲:۳۰') // Persian digits
  check("typing '۲۲:۳۰' (Persian): propagates 1350, valid", q.propagated === 1350 && q.reported === true)
  onTyping(q, '')
  check("clearing the input (''): nothing propagates, valid (stored value stands)", q.propagated === 1350 && q.reported === true && q.stored === 1350)

  /* ================================================================
   * summary — mock vs manual/hardware explicitly separated
   * ================================================================ */
  console.log('\n━━━ SUMMARY (Stage 7 review — TimePicker ⇄ parent-form draft gate)')
  console.log(`  MOCK (pure) tests : ${passed} passed, ${failed} failed`)
  console.log(`  MANUAL visual     : dialog rendering (disabled button styling, error paragraphs) — manual visual checklist, NOT automated here`)
  console.log(`  HARDWARE tests    : 0 (not applicable — pure UI logic; no device involved)`)
  if (failures.length) {
    console.log('\n  failures:')
    for (const f of failures) console.log(`   - ${f}`)
  }
  process.exit(failed > 0 ? 1 : 0)
}

main()
