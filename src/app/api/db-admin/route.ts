/**
 * Phase 1 (v1.8.0-dev) — "Database management" API (settings section).
 * =====================================================================
 * GET  /api/db-admin  → status (path, counts, last backup, backups list)
 * POST /api/db-admin  → { action: 'backup' | 'reset' | 'seed', confirm }
 *
 * Access: SUPER_ADMIN only ("only the senior admin or an explicitly
 * authorized role"). Destructive actions additionally require the exact
 * confirmation phrase — the multi-stage UI shows what will be deleted and
 * what is preserved BEFORE the phrase is typed.
 *
 * All heavy lifting is in src/lib/db-admin.ts (backup-first, transactional,
 * audit-logged, concurrency-locked). No secrets ever appear in responses,
 * logs or audit details.
 */
import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import { dbStatus, dbBackup, dbWipeToEmpty, dbDemoSeed, DbAdminBusyError, DbAdminError } from '@/lib/db-admin'

export async function GET() {
  try {
    await requireRole('SUPER_ADMIN')
    const status = await dbStatus(db)
    return NextResponse.json({ status })
  } catch (err) {
    return fail(err)
  }
}

/** Exact confirmation phrases — must match what the UI displays. */
const CONFIRM_PHRASES = {
  reset: 'پاک‌سازی',
  seed: 'نصب نمونه',
} as const

export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('SUPER_ADMIN')
    const b = await req.json()
    const action = String(b?.action ?? '')

    if (action === 'backup') {
      const backup = await dbBackup(db, { reason: 'manual' })
      return NextResponse.json({ ok: true, action, backup })
    }

    if (action === 'reset') {
      if (b?.confirm !== CONFIRM_PHRASES.reset) {
        throw new ApiError(400, `برای پاک‌سازی، عبارت «${CONFIRM_PHRASES.reset}» را دقیقاً وارد کنید`)
      }
      const result = await dbWipeToEmpty(db, { reason: `reset by ${user.username}` })
      return NextResponse.json({ ok: true, action, backup: result.backup, wiped: result.wiped, auditId: result.auditId })
    }

    if (action === 'seed') {
      if (b?.confirm !== CONFIRM_PHRASES.seed) {
        throw new ApiError(400, `برای نصب اطلاعات نمونه، عبارت «${CONFIRM_PHRASES.seed}» دقیقاً وارد کنید`)
      }
      const result = await dbDemoSeed(db, { reason: `seed by ${user.username}` })
      return NextResponse.json({ ok: true, action, backup: result.backup, wiped: result.wiped, seeded: result.seeded, auditId: result.auditId })
    }

    throw new ApiError(400, 'عملیات نامعتبر')
  } catch (err) {
    if (err instanceof DbAdminBusyError || err instanceof DbAdminError) {
      return NextResponse.json({ error: err.message }, { status: err.status })
    }
    return fail(err)
  }
}
