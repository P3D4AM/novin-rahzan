import { toJalaali, toGregorian as j2g, isValidJalaaliDate, jalaaliMonthLength } from 'jalaali-js'

export { jalaaliMonthLength, j2g as toGregorianObject }

export const FA_MONTHS = [
  'فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
  'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند',
]

export const FA_WEEKDAYS = ['شنبه', 'یکشنبه', 'دوشنبه', 'سه‌شنبه', 'چهارشنبه', 'پنجشنبه', 'جمعه']
export const FA_WEEKDAYS_SHORT = ['ش', 'ی', 'د', 'س', 'چ', 'پ', 'ج']

/** 0=شنبه ... 6=جمعه  (JS getDay: 0=Sunday) */
export function faWeekday(d: Date): number {
  return (d.getDay() + 1) % 7
}

export function toJalali(d: Date): { jy: number; jm: number; jd: number } {
  return toJalaali(d)
}

export function toGregorian(jy: number, jm: number, jd: number): Date {
  const g = j2g(jy, jm, jd)
  return new Date(Date.UTC(g.gy, g.gm - 1, g.gd, 0, 0, 0))
}

export function faDigits(s: string | number): string {
  return String(s).replace(/\d/g, (d) => '۰۱۲۳۴۵۶۷۸۹'[Number(d)])
}

/** 1403/05/12 */
export function jFormat(d: Date, opts?: { withMonthName?: boolean; fa?: boolean }): string {
  const { jy, jm, jd } = toJalali(d)
  const base = opts?.withMonthName
    ? `${jd} ${FA_MONTHS[jm - 1]} ${jy}`
    : `${jy}/${String(jm).padStart(2, '0')}/${String(jd).padStart(2, '0')}`
  return opts?.fa ? faDigits(base) : base
}

/** پنجشنبه ۱۲ مرداد ۱۴۰۳ */
export function jFormatFull(d: Date): string {
  const { jy, jm, jd } = toJalali(d)
  return `${FA_WEEKDAYS[faWeekday(d)]} ${faDigits(jd)} ${FA_MONTHS[jm - 1]} ${faDigits(jy)}`
}

export function hhmm(minutes: number): string {
  const m = ((minutes % 1440) + 1440) % 1440
  return `${String(Math.floor(m / 60)).padStart(2, '0')}:${String(m % 60).padStart(2, '0')}`
}

export function parseHHMM(s: string): number {
  const [h, m] = s.split(':').map(Number)
  return (h || 0) * 60 + (m || 0)
}

export function durationText(minutes: number): string {
  if (!minutes) return '۰'
  const h = Math.floor(minutes / 60)
  const m = minutes % 60
  if (h && m) return `${faDigits(h)} ساعت و ${faDigits(m)} دقیقه`
  if (h) return `${faDigits(h)} ساعت`
  return `${faDigits(m)} دقیقه`
}

/** Parse "1403/05/12" or "1403-05-12" to UTC-midnight Date; returns null if invalid */
export function parseJalaliString(s: string): Date | null {
  if (!s) return null
  const m = s.trim().match(/^(\d{4})[/\-](\d{1,2})[/\-](\d{1,2})$/)
  if (!m) return null
  try {
    const jy = Number(m[1]), jm = Number(m[2]), jd = Number(m[3])
    if (!isValidJalaaliDate(jy, jm, jd)) return null
    return toGregorian(jy, jm, jd)
  } catch {
    return null
  }
}

/** day key in local timezone: we store days as UTC midnight */
export function dayKey(d: Date): string {
  return d.toISOString().slice(0, 10)
}

/** Floor a Date to UTC midnight */
export function startOfUTCDay(d: Date): Date {
  return new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate()))
}

/** Convert a local (Tehran) wall-clock "YYYY-MM-DD HH:MM" to Date.
 *  We treat the whole app in Tehran time (+03:30 fixed, no DST). */
export const TEHRAN_OFFSET_MIN = 210

export function tehranNow(): Date {
  return new Date(Date.now() + TEHRAN_OFFSET_MIN * 60000)
}

/** Tehran wall clock minutes since midnight */
export function tehranMinutes(d: Date): number {
  const t = new Date(d.getTime() + TEHRAN_OFFSET_MIN * 60000)
  return t.getUTCHours() * 60 + t.getUTCMinutes() + (t.getUTCSeconds() >= 30 ? 1 : 0)
}

export function tehranDayOf(d: Date): Date {
  const t = new Date(d.getTime() + TEHRAN_OFFSET_MIN * 60000)
  return new Date(Date.UTC(t.getUTCFullYear(), t.getUTCMonth(), t.getUTCDate()))
}

/** Build UTC Date from Tehran wall clock y,m,d,hh,mm */
export function tehranDate(y: number, mo: number, day: number, h = 0, mi = 0): Date {
  return new Date(Date.UTC(y, mo - 1, day, h, mi) - TEHRAN_OFFSET_MIN * 60000)
}

export function faTime(d: Date): string {
  const t = new Date(d.getTime() + TEHRAN_OFFSET_MIN * 60000)
  return faDigits(
    `${String(t.getUTCHours()).padStart(2, '0')}:${String(t.getUTCMinutes()).padStart(2, '0')}`
  )
}

export function faDateTime(d: Date): string {
  return `${jFormat(d, { fa: true })} - ${faTime(d)}`
}

export function monthRange(jy: number, jm: number): { start: Date; end: Date; days: number } {
  const days = jalaaliMonthLength(jy, jm)
  const start = toGregorian(jy, jm, 1)
  const end = toGregorian(jy, jm, days)
  return { start, end, days }
}

export function addDays(d: Date, n: number): Date {
  return new Date(d.getTime() + n * 86400000)
}

export function datesBetween(start: Date, end: Date): Date[] {
  const out: Date[] = []
  for (let d = startOfUTCDay(start); d <= end; d = addDays(d, 1)) out.push(d)
  return out
}
