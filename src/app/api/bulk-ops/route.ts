/**
 * Phase 3 stage 6 (item 6 — bulk operations) API.
 * =====================================================================
 * POST /api/bulk-ops  { entity, action, ids, ... } — ADMIN only.
 *
 *   entity=punch   action=edit     { direction?, editReason? }
 *   entity=punch   action=delete   { reason }           (audited, recalc)
 *   entity=raw     action=delete   { reason }           (TOMBSTONE + projected punch removal, audited, recalc)
 *   entity=daily   action=edit     { status?, workedMinutes?, lateMinutes?, earlyLeaveMinutes?, note? }  (recorded OVERRIDE)
 *   entity=daily   action=edit     { clear: true }      (clear the override → computed values)
 *   entity=daily   action=delete   { reason }           (derived rows, audited)
 *
 * The heavy lifting is in src/lib/bulk-ops.ts: one transaction per operation
 * (audit inside), an in-flight per-entity lock against duplicate requests,
 * and recalculation of every affected (person, day) AFTER the commit.
 * Previews live in the UI (it shows exactly what is selected); the server
 * validates ids/fields and reports per-item not-found lists.
 */
import { NextRequest, NextResponse } from 'next/server'
import { requireRole, ApiError } from '@/lib/auth'
import { fail } from '@/lib/api'
import {
  bulkEditPunches, bulkDeletePunches, bulkDeleteRawPunches, bulkOverrideDaily, bulkDeleteDaily,
  normalizeDailyOverride, BulkValidationError,
} from '@/lib/bulk-ops'

export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('ADMIN')
    const b = await req.json()
    const entity = String(b?.entity ?? '')
    const action = String(b?.action ?? '')

    if (entity === 'punch' && action === 'edit') {
      if (!b.direction && !b.editReason) throw new ApiError(400, 'یکی از فیلدهای جهت یا یادداشت الزامی است')
      const r = await bulkEditPunches({ ids: b.ids, direction: b.direction, editReason: b.editReason, actor: { id: user.id, username: user.username } })
      return NextResponse.json({ ok: true, ...r })
    }
    if (entity === 'punch' && action === 'delete') {
      const r = await bulkDeletePunches({ ids: b.ids, reason: b.reason, actor: { id: user.id, username: user.username } })
      return NextResponse.json({ ok: true, ...r })
    }
    if (entity === 'raw' && action === 'delete') {
      const r = await bulkDeleteRawPunches({ ids: b.ids, reason: b.reason, actor: { id: user.id, username: user.username } })
      return NextResponse.json({ ok: true, ...r })
    }
    if (entity === 'daily' && action === 'edit') {
      if (b.clear === true) {
        const r = await bulkOverrideDaily({ ids: b.ids, override: null, actor: { id: user.id, username: user.username } })
        return NextResponse.json({ ok: true, cleared: r.overridden, ...r })
      }
      const override = normalizeDailyOverride(b)
      const r = await bulkOverrideDaily({ ids: b.ids, override, actor: { id: user.id, username: user.username } })
      return NextResponse.json({ ok: true, ...r })
    }
    if (entity === 'daily' && action === 'delete') {
      const r = await bulkDeleteDaily({ ids: b.ids, reason: b.reason, actor: { id: user.id, username: user.username } })
      return NextResponse.json({ ok: true, ...r })
    }
    throw new ApiError(400, 'عملیات گروهی نامعتبر')
  } catch (err) {
    if (err instanceof BulkValidationError) {
      return NextResponse.json({ error: err.message }, { status: 400 })
    }
    return fail(err)
  }
}
