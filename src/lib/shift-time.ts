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
 *
 * Value model (unchanged, matches the attendance engine in
 * src/lib/attendance.ts): a segment is { startMin, endMin, breakMin } in
 * minutes 0..1439; end < start = crosses midnight (e.g. 22:00→06:00);
 * end == start = a full 24-hour shift (engine: endMin + 1440 - startMin).
 */

export interface ShiftSegmentInput {
  startMin: number
  endMin: number
  breakMin: number
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
 * Parse a typed time into minutes-of-day. Accepted while-typing states:
 *   '8' → 08:00      (hour only — minute defaults to 0)
 *   '8:3' → 08:03    (partial minutes are still a real time)
 *   '830' → 08:30    (3 digits → H:MM)
 *   '0830' → 08:30   (4 digits → HH:MM)
 *   '۰۸:۳۰' → 08:30  (Persian digits normalized)
 * Rejected with a Persian reason: hour > 23, minute > 59, 5+ digits,
 * empty minute after a typed colon.
 */
export function parseHHMM(raw: string): ParseResult {
  const s = normalizeTimeDraft(raw)
  if (s === '') return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
  if (s.includes(':')) {
    const [hPart, ...rest] = s.split(':')
    const mPart = rest.join('') // extra colons collapse ('8::30' → '8:30')
    const h = Number(hPart)
    if (hPart === '' || Number.isNaN(h)) return { ok: false, error: 'ساعت را وارد کنید (۰۰ تا ۲۳)' }
    if (h > 23) return { ok: false, error: 'ساعت باید بین ۰۰ تا ۲۳ باشد' }
    if (mPart === '') return { ok: false, error: 'دقیقه را وارد کنید (۰۰ تا ۵۹)' }
    const m = Number(mPart)
    if (Number.isNaN(m)) return { ok: false, error: 'قالب ساعت: HH:MM (مثلاً ۰۸:۳۰)' }
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
 * Integer minutes 0..1439, breakMin an integer ≥ 0 and never longer than
 * the span, at least one segment. Messages are Persian, human-identifiable
 * (بازه N names the offending row).
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
    if (!Number.isInteger(s.breakMin) || s.breakMin! < 0) {
      return { ok: false, error: `${label}: وقفه باید عددی منفی‌نباشد (دقیقه) باشد` }
    }
    const span = shiftSegmentSpan(s.startMin!, s.endMin!)
    if (s.breakMin! > span) {
      return { ok: false, error: `${label}: وقفه (${s.breakMin} دقیقه) از طول خود بازه (${span} دقیقه) بیشتر است` }
    }
  }
  return { ok: true }
}
