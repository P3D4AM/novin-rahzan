'use client'

import { useEffect, useRef, useState } from 'react'
import { toGregorian, jalaaliMonthLength } from 'jalaali-js'
import { CalendarDays, ChevronLeft, ChevronRight, Clock } from 'lucide-react'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import {
  tehranTodayMarker,
  jalaliOfMarker,
  jalaliOfInstant,
  markerKey,
  parseJalaliInput,
  formatJalaliMarker,
  faDigits as faDigitsCentral,
} from '@/lib/time'
import { parseHHMM, minutesToHHMM, normalizeTimeDraft, draftValidity } from '@/lib/shift-time'

export const FA_MONTHS = [
  'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
  'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
]

export const FA_WEEKDAYS_SHORT = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج']

/** 0=شنبه ... 6=جمعه */
export function faWeekday(d: Date): number {
  return (d.getUTCDay() + 1) % 7
}

export function faDigits(s: string | number): string {
  return faDigitsCentral(s)
}

/** v1.6.0 (phase 1 stage 4): Tehran-based "today" via the central module —
 *  browser-timezone independent (was local getters). */
export function faToday(): { jy: number; jm: number; jd: number } {
  return jalaliOfMarker(tehranTodayMarker())
}

/** Jalali '1405/08/26' (Persian digits) of an instant's Tehran calendar day — browser-tz independent.
 *  Works for API marker ISOs too (a marker's Tehran calendar day is the marker's own date). */
export function jFormatFa(d: Date | string): string {
  const date = typeof d === 'string' ? new Date(d) : d
  if (isNaN(date.getTime())) return '-'
  const { jy, jm, jd } = jalaliOfInstant(date)
  return faDigitsCentral(`${jy}/${String(jm).padStart(2, '0')}/${String(jd).padStart(2, '0')}`)
}

export function faTime(d: Date | string | null | undefined): string {
  if (!d) return '-'
  const date = typeof d === 'string' ? new Date(d) : d
  if (isNaN(date.getTime())) return '-'
  return new Intl.DateTimeFormat('fa-IR', { hour: '2-digit', minute: '2-digit', timeZone: 'Asia/Tehran' }).format(date)
}

export function toG(jy: number, jm: number, jd: number): Date {
  const g = toGregorian(jy, jm, jd)
  return new Date(Date.UTC(g.gy, g.gm - 1, g.gd))
}

/** Jalali month calendar grid (starts Saturday) */
export function monthGrid(jy: number, jm: number): Array<{ jd: number; weekday: number; gDate: Date } | null> {
  const days = jalaaliMonthLength(jy, jm)
  const first = toG(jy, jm, 1)
  const firstWeekday = faWeekday(first)
  const cells: Array<{ jd: number; weekday: number; gDate: Date } | null> = Array(firstWeekday).fill(null)
  for (let d = 1; d <= days; d++) {
    const g = toG(jy, jm, d)
    cells.push({ jd: d, weekday: faWeekday(g), gDate: g })
  }
  return cells
}

export const monthLabel = (jy: number, jm: number) => `${FA_MONTHS[jm - 1]} ${faDigits(jy)}`
export const WEEKDAY_HEADERS = FA_WEEKDAYS_SHORT

/* ---------- JalaliDateInput ---------- */
export function JalaliDateInput({
  value,
  onChange,
  placeholder,
}: {
  value: string
  onChange: (v: string) => void
  placeholder?: string
}) {
  return (
    <input
      dir="ltr"
      inputMode="numeric"
      className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm text-center tracking-widest"
      placeholder={placeholder || '1403/05/12'}
      value={value}
      onChange={(e) => onChange(e.target.value.replace(/[^\d/]/g, ''))}
    />
  )
}

/* ---------- TimeInput (HH:MM) ---------- */
export function TimeInput({ value, onChange }: { value: string; onChange: (v: string) => void }) {
  return (
    <input
      dir="ltr"
      className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm text-center"
      placeholder="08:30"
      value={value}
      onChange={(e) => onChange(e.target.value.replace(/[^\d:]/g, ''))}
    />
  )
}

/* ---------- ShiftTimePicker (Phase 3, item 9 + Stage 7 review fix) ----------
 * The shift-dialog time control: MANUAL HH:MM typing AND list-based
 * hour/minute selection, value contract = minutes-of-day (number).
 *
 * The old form parsed the raw text of every keystroke
 * (`v.split(':').map(Number)`) — typing "830" before the colon silently
 * stored 49 800 minutes and corrupted the saved shift. This component
 * never propagates an unparseable draft: only a VALID parsed time reaches
 * onChange; an invalid draft shows a Persian error and reverts on blur.
 *
 * Stage 7 review fix — the defect this closes: an INVALID draft ('25:99')
 * used to live ONLY in this component's internal state; the parent form
 * kept the last VALID minutes, so its validator kept passing and the save
 * button stayed enabled (the stale previous value could be saved).
 * The picker now REPORTS the draft's validity to the parent via
 * onValidityChange (true = valid / blank / abandoned, false = unparseable
 * draft sitting in the input); the parent aggregates ALL pickers and must
 * block save while ANY one reports false.
 */
export function ShiftTimePicker({
  minutes,
  onChange,
  onValidityChange,
  ariaLabel,
}: {
  /** stored value: minutes-of-day 0..1439; null = nothing chosen yet */
  minutes: number | null
  onChange: (minutes: number) => void
  /** Draft-validity report to the parent form (see the header above). */
  onValidityChange?: (valid: boolean) => void
  ariaLabel?: string
}) {
  const [draft, setDraft] = useState<string | null>(null)
  const [error, setError] = useState('')
  const [open, setOpen] = useState(false)
  // latest-ref so the unmount release never fires a stale closure
  const onValidityRef = useRef(onValidityChange)
  useEffect(() => { onValidityRef.current = onValidityChange })
  // on unmount, release this picker's invalid-draft flag in the parent map —
  // a removed/abandoned picker must not keep the parent's save gate closed
  useEffect(() => () => { onValidityRef.current?.(true) }, [])
  const report = (valid: boolean) => { onValidityRef.current?.(valid) }

  const text = draft ?? (minutes == null ? '' : minutesToHHMM(minutes))
  const curH = minutes == null ? 0 : Math.floor(minutes / 60)
  const curM = minutes == null ? 0 : minutes % 60

  const onTyping = (raw: string) => {
    const cleaned = normalizeTimeDraft(raw)
    setDraft(cleaned)
    // the draft gate: '' counts as valid (the stored value stands)
    report(draftValidity(cleaned))
    if (cleaned === '') { setError(''); return } // empty draft keeps the stored value
    const r = parseHHMM(cleaned)
    if (r.ok) { setError(''); onChange(r.minutes) } else setError(r.error)
  }

  const onBlur = () => {
    // abandon an invalid edit — display goes back to the stored minutes
    setDraft(null)
    setError('')
    report(true)
  }

  const pickHour = (h: number) => { setError(''); setDraft(null); report(true); onChange(h * 60 + curM) }
  const pickMinute = (m: number) => { setError(''); setDraft(null); setOpen(false); report(true); onChange(curH * 60 + m) }

  return (
    <div className="w-full">
      <div className="flex items-center gap-1.5">
        <input
          dir="ltr"
          inputMode="numeric"
          aria-label={ariaLabel || 'ساعت (HH:MM)'}
          className={`w-full rounded-md border bg-background px-3 py-2 text-sm text-center font-mono ${error ? 'border-destructive' : 'border-input'}`}
          placeholder="08:30"
          value={text}
          onChange={(e) => onTyping(e.target.value)}
          onBlur={onBlur}
        />
        <Popover open={open} onOpenChange={setOpen}>
          <PopoverTrigger asChild>
            <button
              type="button"
              aria-label="انتخاب ساعت از فهرست"
              className="inline-flex h-9 w-9 shrink-0 items-center justify-center rounded-md border border-input bg-background hover:bg-accent"
            >
              <Clock className="h-4 w-4" />
            </button>
          </PopoverTrigger>
          <PopoverContent dir="rtl" align="start" className="w-auto p-0">
            <div className="flex gap-1 p-2">
              <div className="w-[4.5rem]">
                <div className="mb-1 text-center text-[11px] font-semibold text-muted-foreground">ساعت</div>
                <div className="max-h-48 overflow-y-auto">
                  {Array.from({ length: 24 }, (_, h) => (
                    <button
                      key={h}
                      type="button"
                      onClick={() => pickHour(h)}
                      className={`mb-0.5 flex h-7 w-full items-center justify-center rounded-md font-mono text-xs hover:bg-accent ${minutes != null && h === curH ? 'bg-primary text-primary-foreground hover:bg-primary' : ''}`}
                    >
                      {faDigits(String(h).padStart(2, '0'))}
                    </button>
                  ))}
                </div>
              </div>
              <div className="w-[4.5rem]">
                <div className="mb-1 text-center text-[11px] font-semibold text-muted-foreground">دقیقه</div>
                <div className="max-h-48 overflow-y-auto">
                  {Array.from({ length: 60 }, (_, m) => (
                    <button
                      key={m}
                      type="button"
                      onClick={() => pickMinute(m)}
                      className={`mb-0.5 flex h-7 w-full items-center justify-center rounded-md font-mono text-xs hover:bg-accent ${minutes != null && m === curM ? 'bg-primary text-primary-foreground hover:bg-primary' : ''}`}
                    >
                      {faDigits(String(m).padStart(2, '0'))}
                    </button>
                  ))}
                </div>
              </div>
            </div>
            <div className="border-t px-2 py-1.5 text-center text-[10px] text-muted-foreground">
              انتخاب ساعت و دقیقه — انتخاب دقیقه پنجره را می‌بندد
            </div>
          </PopoverContent>
        </Popover>
      </div>
      {error && <p className="mt-1 text-xs text-destructive">{error}</p>}
    </div>
  )
}

/* ---------- MonthPicker ---------- */
export function MonthPicker({ jy, jm, onChange }: { jy: number; jm: number; onChange: (jy: number, jm: number) => void }) {
  const ty = faToday().jy
  return (
    <div className="flex items-center gap-2">
      <select className="rounded-md border border-input bg-background px-2 py-1.5 text-sm" value={jm} onChange={(e) => onChange(jy, Number(e.target.value))}>
        {FA_MONTHS.map((m, i) => (
          <option key={m} value={i + 1}>{m}</option>
        ))}
      </select>
      <select className="rounded-md border border-input bg-background px-2 py-1.5 text-sm" value={jy} onChange={(e) => onChange(Number(e.target.value), jm)}>
        {Array.from({ length: 7 }, (_, i) => ty - 3 + i).map((y) => (
          <option key={y} value={y}>{faDigits(y)}</option>
        ))}
      </select>
    </div>
  )
}

/* ---------- JalaliDatePicker (v1.6.0 — phase 1 stage 4) ----------
 * Value convention: '' or 'YYYY-MM-DD' — the GREGORIAN date-only key of the
 * business day (what the API day-params expect; see docs/API-DATE-CONTRACTS.md).
 * Display & user interaction are fully Jalali (RTL grid, month/year navigation,
 * leap-correct month lengths, Tehran-based "today", typed input with Persian
 * validation, mobile-sized cells). Never uses the browser timezone.
 */
export function JalaliDatePicker({
  value,
  onChange,
  placeholder = 'انتخاب تاریخ',
  allowClear = true,
}: {
  value: string
  onChange: (v: string) => void
  placeholder?: string
  allowClear?: boolean
}) {
  const selected = /^\d{4}-\d{2}-\d{2}$/.test(value) ? new Date(`${value}T00:00:00.000Z`) : null
  const [open, setOpen] = useState(false)
  const [view, setView] = useState<{ jy: number; jm: number }>(() => jalaliOfMarker(selected ?? tehranTodayMarker()))
  // draft = raw text while the user is typing; null → display the formatted value
  const [draft, setDraft] = useState<string | null>(null)
  const [error, setError] = useState('')
  const text = draft ?? (selected ? formatJalaliMarker(selected) : '')

  const commit = (marker: Date | null) => {
    setError('')
    setDraft(null)
    if (marker) {
      onChange(markerKey(marker))
      setView(jalaliOfMarker(marker))
    } else {
      onChange('')
    }
    setOpen(false)
  }

  const onTyping = (raw: string) => {
    setDraft(raw)
    if (!raw) {
      setError('')
      if (allowClear) onChange('')
      return
    }
    const r = parseJalaliInput(raw)
    if (r.ok) {
      setError('')
      onChange(markerKey(r.marker))
    } else {
      setError(r.error)
    }
  }

  const ty = jalaliOfMarker(tehranTodayMarker()).jy
  const grid = monthGrid(view.jy, view.jm)
  const selectedKey = selected ? markerKey(selected) : ''
  const todayKey = markerKey(tehranTodayMarker())

  const moveMonth = (delta: number) => {
    let { jy, jm } = view
    jm += delta
    if (jm < 1) { jm = 12; jy -= 1 }
    if (jm > 12) { jm = 1; jy += 1 }
    setView({ jy, jm })
  }

  return (
    <div className="w-full">
      <div className="flex items-center gap-1.5">
        <input
          dir="ltr"
          inputMode="numeric"
          className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm text-center tracking-widest"
          placeholder={placeholder || '1405/08/26'}
          value={text}
          onChange={(e) => onTyping(e.target.value.replace(/[^\d/]/g, ''))}
        />
        <Popover open={open} onOpenChange={setOpen}>
          <PopoverTrigger asChild>
            <button
              type="button"
              aria-label="تقویم شمسی"
              className="inline-flex h-9 w-9 shrink-0 items-center justify-center rounded-md border border-input bg-background hover:bg-accent"
            >
              <CalendarDays className="h-4 w-4" />
            </button>
          </PopoverTrigger>
          <PopoverContent dir="rtl" align="start" className="w-auto p-0">
            <div className="w-[17.5rem] p-3">
              {/* header: month navigation */}
              <div className="mb-2 flex items-center gap-1">
                <button type="button" className="inline-flex h-7 w-7 items-center justify-center rounded-md hover:bg-accent" onClick={() => moveMonth(-1)} aria-label="ماه قبل">
                  <ChevronRight className="h-4 w-4" />
                </button>
                <div className="flex flex-1 items-center justify-center gap-1.5">
                  <select
                    className="rounded-md border border-input bg-background px-1.5 py-1 text-sm"
                    value={view.jm}
                    onChange={(e) => setView({ ...view, jm: Number(e.target.value) })}
                  >
                    {FA_MONTHS.map((m, i) => (
                      <option key={m} value={i + 1}>{m}</option>
                    ))}
                  </select>
                  <select
                    className="rounded-md border border-input bg-background px-1.5 py-1 text-sm"
                    value={view.jy}
                    onChange={(e) => setView({ ...view, jy: Number(e.target.value) })}
                  >
                    {Array.from({ length: 13 }, (_, i) => ty - 10 + i).map((y) => (
                      <option key={y} value={y}>{faDigits(y)}</option>
                    ))}
                  </select>
                </div>
                <button type="button" className="inline-flex h-7 w-7 items-center justify-center rounded-md hover:bg-accent" onClick={() => moveMonth(1)} aria-label="ماه بعد">
                  <ChevronLeft className="h-4 w-4" />
                </button>
              </div>
              {/* weekday headers */}
              <div className="grid grid-cols-7 gap-0.5 pb-1 text-center text-[11px] text-muted-foreground">
                {FA_WEEKDAYS_SHORT.map((w, i) => (
                  <span key={i}>{w}</span>
                ))}
              </div>
              {/* day cells */}
              <div className="grid grid-cols-7 gap-0.5">
                {grid.map((cell, i) =>
                  cell ? (
                    <button
                      type="button"
                      key={i}
                      onClick={() => commit(cell.gDate)}
                      className={`flex h-9 w-9 items-center justify-center rounded-md text-sm hover:bg-accent ${
                        selectedKey === markerKey(cell.gDate)
                          ? 'bg-primary text-primary-foreground hover:bg-primary'
                          : markerKey(cell.gDate) === todayKey
                            ? 'border border-primary/60 text-primary'
                            : ''
                      }`}
                    >
                      {faDigits(cell.jd)}
                    </button>
                  ) : (
                    <span key={i} />
                  ),
                )}
              </div>
              {/* footer */}
              <div className="mt-2 flex items-center justify-between gap-2 border-t pt-2">
                <button
                  type="button"
                  className="rounded-md px-2 py-1 text-xs text-primary hover:bg-accent"
                  onClick={() => commit(tehranTodayMarker())}
                >
                  امروز
                </button>
                {allowClear && (
                  <button
                    type="button"
                    className="rounded-md px-2 py-1 text-xs text-muted-foreground hover:bg-accent"
                    onClick={() => commit(null)}
                  >
                    پاک کردن
                  </button>
                )}
              </div>
            </div>
          </PopoverContent>
        </Popover>
      </div>
      {error && <p className="mt-1 text-xs text-destructive">{error}</p>}
    </div>
  )
}

/* ---------- Live Tehran clock ---------- */
export function useTehranClock(): { time: string; dateFa: string } {
  const [now, setNow] = useState(() => new Date())
  useEffect(() => {
    const t = setInterval(() => setNow(new Date()), 1000)
    return () => clearInterval(t)
  }, [])
  const time = new Intl.DateTimeFormat('fa-IR', { hour: '2-digit', minute: '2-digit', second: '2-digit', timeZone: 'Asia/Tehran' }).format(now)
  const dateFa = new Intl.DateTimeFormat('fa-IR', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric', timeZone: 'Asia/Tehran' }).format(now)
  return { time, dateFa }
}

export function minutesToHM(m: number): string {
  if (!m) return '۰:۰۰'
  const h = Math.floor(m / 60)
  const mm = m % 60
  return faDigits(`${h}:${String(mm).padStart(2, '0')}`)
}

export function durationFa(m: number): string {
  if (!m) return '—'
  const h = Math.floor(m / 60)
  const mm = m % 60
  if (h && mm) return `${faDigits(h)}س ${faDigits(mm)}د`
  if (h) return `${faDigits(h)} ساعت`
  return `${faDigits(mm)} دقیقه`
}

/** Jalali 'YYYY/MM/DD' (Persian digits) of an API day field — accepts 'YYYY-MM-DD' or full marker ISO ('…T00:00:00.000Z'). */
export function gregKeyToJalali(s: string | null | undefined): string {
  const key = String(s ?? '').slice(0, 10)
  if (!/^\d{4}-\d{2}-\d{2}$/.test(key)) return '-'
  const marker = new Date(`${key}T00:00:00.000Z`)
  if (isNaN(marker.getTime())) return '-'
  return faDigits(formatJalaliMarker(marker))
}
