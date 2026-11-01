import { NextResponse } from 'next/server'
import { ApiError } from '@/lib/auth'

/** Map low-level network failures to readable Persian messages */
const NET_MESSAGES: Record<string, string> = {
  ECONNREFUSED: 'اتصال رد شد — دستگاهی با این IP/پورت در شبکه پاسخ نمی‌دهد. IP و پورت دستگاه را از منوی تنظیمات ارتباط دستگاه بررسی کنید.',
  ETIMEDOUT: 'مهلت پاسخ دستگاه به پایان رسید — دستگاه خاموش است، IP اشتباه است یا در شبکه قابل دسترسی نیست.',
  EHOSTUNREACH: 'دستگاه در شبکه در دسترس نیست — آدرس IP اشتباه است یا سیستم و دستگاه در یک شبکه نیستند.',
  ENETUNREACH: 'شبکه در دسترس نیست — کابل شبکه/اتصال وای‌فای را بررسی کنید.',
  ECONNRESET: 'اتصال توسط دستگاه قطع شد — ممکن است پورت اشتباه باشد یا دستگاه ارتباط را نپذیرد.',
  EPIPE: 'اتصال در حین ارسال قطع شد.',
  EADDRINUSE: 'پورت موردنظر روی سرور اشغال است.',
}

/** Convert any thrown value (Error, object rejection, string, …) to a readable message */
export function errorMessage(err: unknown): string {
  if (err instanceof Error) {
    const code = (err as NodeJS.ErrnoException).code
    if (code && NET_MESSAGES[code]) return `${NET_MESSAGES[code]} (${code})`
    return err.message || String(err)
  }
  if (typeof err === 'string') return err
  if (err && typeof err === 'object') {
    const e = err as { message?: unknown; error?: { message?: unknown }; description?: unknown; errno?: unknown; code?: unknown }
    for (const v of [e.message, e.error?.message, e.description]) {
      if (typeof v === 'string' && v.trim()) return v
    }
    const code = e.code ?? e.errno
    if (typeof code === 'string' && NET_MESSAGES[code]) return `${NET_MESSAGES[code]} (${code})`
    if (typeof code === 'string' || typeof code === 'number') return `خطای شبکه (${code})`
    try { return JSON.stringify(err) } catch { /* fall through */ }
  }
  return 'خطای ناشناخته'
}

export function ok(data: unknown, init?: ResponseInit) {
  return NextResponse.json(data as Record<string, unknown>, init)
}

export function fail(err: unknown) {
  if (err instanceof ApiError) {
    return NextResponse.json({ error: err.message }, { status: err.status })
  }
  const message = errorMessage(err)
  console.error('[api]', err)
  return NextResponse.json({ error: message }, { status: 500 })
}

export function badRequest(message: string) {
  return NextResponse.json({ error: message }, { status: 400 })
}
