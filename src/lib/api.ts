import { NextResponse } from 'next/server'
import { ApiError } from '@/lib/auth'

export function ok(data: unknown, init?: ResponseInit) {
  return NextResponse.json(data as Record<string, unknown>, init)
}

export function fail(err: unknown) {
  if (err instanceof ApiError) {
    return NextResponse.json({ error: err.message }, { status: err.status })
  }
  const message = err instanceof Error ? err.message : 'خطای داخلی سرور'
  console.error('[api]', err)
  return NextResponse.json({ error: message }, { status: 500 })
}

export function badRequest(message: string) {
  return NextResponse.json({ error: message }, { status: 400 })
}
