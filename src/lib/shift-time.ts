/**
 * Shift time (HH:MM) parsing & segment validation — the pure core behind the
 * shift-form time picker (Phase 3, item 9).
 * =====================================================================
 * Pure & synchronous — NO React, NO DB, NO clock → unit-testable from
 * scripts/test-shift-time.ts. The ShiftTimePicker component and the
 * /api/shifts route both delegate to THIS module so the UI gate and the
 * server gate can never disagree.
 *
 * WHY this module exists (root cause of the reported defects):
 *   * The old shift form fed the raw text of a plain input straight into
 *     `const [h, m] = v.split(':').map(Number)` on EVERY keystroke — typing
 *     "830" (no colon yet) produced h=830 → startMin=49 800: the stored
 *     value was corrupted while typing and the reopened form showed the
 *     garbage ("مقدار واقعی پس از بازکردن مجدد فرم حفظ نمی‌شد").
 *   * Nothing validated hour/minute ranges (25:99 was silently accepted),
 *     and nothing marked a night shift explicitly.
 *   * (Stage 7 review) `8::30` was silently "repaired" into 08:30 by
 *     collapsing extra colons — malformed structure must be REJECTED, not
 *     guessed. The grammar below is now explicit and strict.
 *
 * Value model (unchanged, matches the attendance engine in
 * src/lib/attendance.ts): a segment is { startMin, endMin, breakMin } in
 * minutes 0..1439; end < start = crosses midnight (e.g. 22:00→06:00);
 * end == start = a full 24-hour shift (engine: endMin + 1440 - startMin).
 *
 * GRAMMAR (after normalizeTimeDraft strips noise — only digits and ':' are
 * left; Persian/Arabic digits are normalized first):
 *   COLONED := HOUR ':' MINUTE   exactly ONE colon; HOUR = 1..2 digits (0..23);
 *                               MINUTE = 1..2 digits (0..59)
 *   BARE    := 1..2 digits → hour, minute 0 | 3 digits → H:MM | 4 digits → HH:MM
 * ACCEPTED: '08:30', '8:3' (typing-friendly), '830', '0830', '8', '۰۸:۳۰'.
 * REJECTED: '25:00' (hour>23), '08:75' (minute>59), '8::30' (two colons),
 *           '8:30:' (trailing colon), ':30' (empty hour), '8:' (empty minute),
 *           '8:030' (3-digit minute part), '000:30' (3-digit hour part),
 *           '123456' (5+ bare digits), '' (nothing typed).
 */

export interface ShiftSegmentInput {
  startMin: number
  endMin: number
  /** Stage 7 review — the UNIFIED optional contract: absent/null → 0
   *  (same as the API's `?? 0` and the DB column default). */
  breakMin?: number | null
}

export interface ParseResultOk { ok: true; minutes: number }
export interface ParseResultErr { ok: false; error: string }
export type ParseResult = ParseResultOk | ParseResultErr

/** Persian (۰-۹) and Arabic-Indic (٠-٩) digits → ASCII digits. */
export function normalizeFaDigits(s: string): string {
  return s.replace(/[۰-۹]/g, (d) => String('۰۱۲۳۴۵۶۷۸۹'.indexOf(d)))
    .replace(/[٠-٩]/g, (d) => String('٠١٢٣٤٥٦٧٨٩'.indexOf(d)))
}

/** Keep only digits and ':' — typed noise (spaces, dots, dashes) is dropped. */
export function normalizeTimeDraft(s: string): string {
  return normalizeFaDigits(s).replace(/[^\d:]/g, '')
}

/**
 * Parse a typed time into minutes-of-day (STRICT grammar — see the module
 * header). Typing-friendly partials ('8' → 08:00, '8:3' → 08:03, '830' →
 * 08:30, '۰۸:۳۰' → 08:30) stay accepted; STRUCTURALLY malformed input
 * ('8::30', '8:030', ':30', …) is rejected instead of being guessed into
 * a valid time.
 */
export function parseHHMM(raw: string): ParseResult {
  const s = normalizeTimeDraft(raw)
  if (s === '') return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
  const colonCount = (s.match(/:/g) ?? []).length
  if (colonCount > 1) return { ok: false, error: 'قالب ساعت نامعتبر — فقط یک «:» مجاز است (مثلاً ۰۸:۳۰)' }
  if (colonCount === 1) {
    const colonAt = s.indexOf(':')
    const hPart = s.slice(0, colonAt)
    const mPart = s.slice(colonAt + 1)
    if (hPart === '') return { ok: false, error: 'ساعت را وارد کنید (۰۰ تا ۲۳)' }
    if (mPart === '') return { ok: false, error: 'دقیقه را وارد کنید (۰۰ تا ۵۹)' }
    if (hPart.length > 2 || mPart.length > 2) {
      return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
    }
    const h = Number(hPart)
    const m = Number(mPart)
    if (Number.isNaN(h) || Number.isNaN(m)) return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
    if (h > 23) return { ok: false, error: 'ساعت باید بین ۰۰ تا ۲۳ باشد' }
    if (m > 59) return { ok: false, error: 'دقیقه باید بین ۰۰ تا ۵۹ باشد' }
    return { ok: true, minutes: h * 60 + m }
  }
  // no colon — digit-count heuristics that stay honest while typing
  if (s.length <= 2) {
    const h = Number(s)
    if (Number.isNaN(h)) return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
    if (h > 23) return { ok: false, error: 'ساعت باید بین ۰۰ تا ۲۳ باشد' }
    return { ok: true, minutes: h * 60 }
  }
  if (s.length === 3) {
    const h = Number(s.slice(0, 1))
    const m = Number(s.slice(1))
    if (h > 23) return { ok: false, error: 'ساعت باید بین ۰۰ تا ۲۳ باشد' }
    if (m > 59) return { ok: false, error: 'دقیقه باید بین ۰۰ تا ۵۹ باشد' }
    return { ok: true, minutes: h * 60 + m }
  }
  if (s.length === 4) {
    const h = Number(s.slice(0, 2))
    const m = Number(s.slice(2))
    if (h > 23) return { ok: false, error: 'ساعت باید بین ۰۰ تا ۲۳ باشد' }
    if (m > 59) return { ok: false, error: 'دقیقه باید بین ۰۰ تا ۵۹ باشد' }
    return { ok: true, minutes: h * 60 + m }
  }
  return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
}

/** minutes-of-day → Latin 'HH:MM' (the storage contract is always minutes). */
export function minutesToHHMM(m: number): string {
  const h = Math.floor(m / 60)
  const mm = m % 60
  return `${String(h).padStart(2, '0')}:${String(mm).padStart(2, '0')}`
}

/** Does the segment cross midnight (night shift)? end < start. */
export function crossesMidnight(startMin: number, endMin: number): boolean {
  return endMin < startMin
}

/** Segment length in minutes — EXACTLY the engine's arithmetic
 *  (src/lib/attendance.ts line 73): end>start → diff; end==start → 24h;
 *  end<start → wraps +1440. */
export function shiftSegmentSpan(startMin: number, endMin: number): number {
  return endMin > startMin ? endMin - startMin : endMin + 1440 - startMin
}

export interface SegmentsValidationOk { ok: true }
export interface SegmentsValidationErr { ok: false; error: string }
export type SegmentsValidation = SegmentsValidationOk | SegmentsValidationErr

/**
 * Full segment validation — the ONE gate shared by the form's save button
 * and the /api/shifts POST/PUT routes (server side stays authoritative).
 * Integer minutes 0..1439, at least one segment. Messages are Persian,
 * human-identifiable (بازه N names the offending row).
 *
 * breakMin contract (Stage 7 review — UNIFIED across client / server /
 * persistence): breakMin is OPTIONAL. Absent (undefined or null) means 0 —
 * exactly like the API's `s.breakMin ?? 0` and the DB default
 * (`breakMin Int @default(0)`). When PRESENT it must be an integer ≥ 0 and
 * never longer than the segment span (so the "required" reading that
 * previously contradicted the API is gone).
 */
export function validateShiftSegments(segments: unknown): SegmentsValidation {
  if (!Array.isArray(segments) || segments.length === 0) {
    return { ok: false, error: 'حداقل یک بازه زمانی لازم است' }
  }
  for (let i = 0; i < segments.length; i++) {
    const s = segments[i] as Partial<ShiftSegmentInput> | null | undefined
    const label = `بازه ${i + 1}`
    if (!s || !Number.isInteger(s.startMin) || s.startMin! < 0 || s.startMin! > 1439) {
      return { ok: false, error: `${label}: ساعت شروع نامعتبر است (۰۰:۰۰ تا ۲۳:۵۹)` }
    }
    if (!Number.isInteger(s.endMin) || s.endMin! < 0 || s.endMin! > 1439) {
      return { ok: false, error: `${label}: ساعت پایان نامعتبر است (۰۰:۰۰ تا ۲۳:۵۹)` }
    }
    const rawBreak = s.breakMin
    const breakMin = rawBreak === undefined || rawBreak === null ? 0 : rawBreak
    if (typeof breakMin !== 'number' || !Number.isInteger(breakMin) || breakMin < 0) {
      return { ok: false, error: `${label}: وقفه باید عددی منفی‌نباشد (دقیقه) باشد` }
    }
    const span = shiftSegmentSpan(s.startMin!, s.endMin!)
    if (breakMin > span) {
      return { ok: false, error: `${label}: وقفه (${breakMin} دقیقه) از طول خود بازه (${span} دقیقه) بیشتر است` }
    }
  }
  return { ok: true }
}

/* ---------- Draft-validity gate (Stage 7 review fix) ----------
 * The ShiftTimePicker keeps the edit-in-progress ("draft") in its own state
 * and only propagates a VALID parse to the parent form (value contract =
 * minutes). The Stage 7 defect: while the draft was INVALID ('25:99') the
 * parent had no idea — its stored value stayed the last VALID minutes, so
 * validateShiftSegments kept passing and the save gate stayed OPEN (the
 * button could save the stale previous value).
 *
 * Fix architecture — no duplicated state: the picker REPORTS its draft
 * validity via onValidityChange; the parent aggregates ALL pickers in ONE
 * map below and must refuse to save while ANY picker is invalid. These pure
 * functions are that aggregation — used by shifts.tsx AND the tests; the
 * React wiring stays thin. */

/** Is a draft (edit-in-progress) acceptable? null/'…' empty = nothing being
 *  typed → valid (the stored minutes stand); otherwise the draft must parse. */
export function draftValidity(draft: string | null): boolean {
  if (draft === null || draft === '') return true
  return parseHHMM(draft).ok
}

/** The parent's aggregation map — only INVALID picker keys are present, so
 *  "empty map" ⇔ every picker in the form is valid. */
export type DraftValidityMap = Record<string, true>

/** Immutable update of one picker's validity in the parent map. */
export function setDraftValidity(map: DraftValidityMap, key: string, valid: boolean): DraftValidityMap {
  if (valid) {
    if (!map[key]) return map
    const next: DraftValidityMap = { ...map }
    delete next[key]
    return next
  }
  return map[key] ? map : { ...map, [key]: true }
}

/** The aggregate save gate from the pickers' side: true ⇔ no picker in the
 *  form is holding an invalid draft. */
export function hasInvalidDrafts(map: DraftValidityMap): boolean {
  return Object.keys(map).length > 0
}
