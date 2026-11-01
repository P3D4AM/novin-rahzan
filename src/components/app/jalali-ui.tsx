'use client'

import { useEffect, useState } from 'react'
import { toJalaali, toGregorian, jalaaliMonthLength } from 'jalaali-js'

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
  return String(s).replace(/\d/g, (dg) => '۰۱۲۳۴۵۶۷۸۹'[Number(dg)])
}

export function faToday(): { jy: number; jm: number; jd: number } {
  return toJalaali(new Date())
}

export function jFormatFa(d: Date | string): string {
  const date = typeof d === 'string' ? new Date(d) : d
  if (isNaN(date.getTime())) return '-'
  const { jy, jm, jd } = toJalaali(date)
  return faDigits(`${jy}/${String(jm).padStart(2, '0')}/${String(jd).padStart(2, '0')}`)
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
