/**
 * Phase 3 stage 6 (item 6 — bulk operations): transactional, audit-logged,
 * recalc-aware bulk edit/delete for the operational layers.
 * =====================================================================
 * Data-integrity rules implemented here (per the Phase 2 item 6 spec):
 *
 *  * RawPunch    — the received device record is NEVER destroyed: an
 *                  operational delete is a TOMBSTONE (deletedAt/deletedBy/
 *                  deleteReason). The projected PunchRecord rows ARE deleted
 *                  (with full old values in the audit trail) and the
 *                  fingerprint dedupe still sees the tombstoned raw row, so
 *                  a re-read of the same physical record can never resurrect
 *                  a deleted punch.
 *  * PunchRecord — bulk edit writes `edited`/`editReason` provenance and ONE
 *                  audit row carrying every OLD value (recoverable); bulk
 *                  delete stores the removed rows in the audit trail.
 *  * DailyAttendance — DERIVED data: the administrative correction is a
 *                  RECORDED OVERRIDE (manualOverride JSON) that recalcRange
 *                  re-applies after recomputing the day (an override never
 *                  silently evaporates in a recalc); clearing it returns the
 *                  row to computed values. Bulk delete of derived rows is
 *                  allowed (they are recomputable) and audited.
 *  * Every operation runs in ONE transaction (atomic — a mid-batch failure
 *    rolls back EVERYTHING including the audit row) and holds an in-process
 *    per-entity lock so duplicate/double-clicked requests cannot interleave.
 *  * Punch-affecting operations recalculate every affected (person, day)
 *    AFTER the commit (failures there are reported, never swallowed).
 */
import { db } from '@/lib/db'
import { recalcDayForPerson } from '@/lib/attendance'
import { tehranDayOf } from '@/lib/jalali'

export class BulkBusyError extends Error {
  constructor(entity: string) {
    super(`عملیات گروهی دیگری روی «${entity}» در حال اجراست؛ چند لحظه بعد دوباره تلاش کنید`)
    this.name = 'BulkBusyError'
  }
}
export class BulkValidationError extends Error {
  status = 400
}

/* ------------------------------------------------------------------ */
/* in-flight lock (one bulk op per entity at a time)                   */
/* ------------------------------------------------------------------ */
const inflight = new Set<string>()
async function withLock<T>(entity: string, fn: () => Promise<T>): Promise<T> {
  if (inflight.has(entity)) throw new BulkBusyError(entity)
  inflight.add(entity)
  try {
    return await fn()
  } finally {
    inflight.delete(entity)
  }
}

const DIRECTIONS = new Set(['IN', 'OUT', 'AUTO'])
const STATUSES = new Set(['PRESENT', 'ABSENT', 'LEAVE', 'MISSION', 'HOLIDAY', 'OFF', 'INCOMPLETE', 'PENDING'])

function assertIds(ids: unknown): string[] {
  if (!Array.isArray(ids) || !ids.length || ids.length > 5000) {
    throw new BulkValidationError('فهرست شناسه‌ها نامعتبر است (۱ تا ۵۰۰۰ رکورد)')
  }
  for (const id of ids) {
    if (typeof id !== 'string' || !id.trim()) throw new BulkValidationError('شناسهٔ نامعتبر در فهرست')
  }
  return ids as string[]
}

export interface DailyOverridePatch {
  status?: string
  workedMinutes?: number
  lateMinutes?: number
  earlyLeaveMinutes?: number
  note?: string
}

/** Validate + serialize an override patch (pure — reused by the API route). */
export function normalizeDailyOverride(b: { status?: unknown; workedMinutes?: unknown; lateMinutes?: unknown; earlyLeaveMinutes?: unknown; note?: unknown }): DailyOverridePatch {
  const out: DailyOverridePatch = {}
  if (b.status != null) {
    const s = String(b.status)
    if (!STATUSES.has(s)) throw new BulkValidationError('وضعیت نامعتبر است')
    out.status = s
  }
  for (const k of ['workedMinutes', 'lateMinutes', 'earlyLeaveMinutes'] as const) {
    const v = (b as Record<string, unknown>)[k]
    if (v == null) continue
    const n = Number(v)
    if (!Number.isInteger(n) || n < 0 || n > 24 * 60) throw new BulkValidationError(`«${k}» باید عدد صحیح و بین ۰ تا ۱۴۴۰ دقیقه باشد`)
    out[k] = n
  }
  if (typeof b.note === 'string' && b.note.trim()) out.note = b.note.trim().slice(0, 500)
  if (!Object.keys(out).length) throw new BulkValidationError('هیچ فیلد قابل ویرایشی ارسال نشده است')
  return out
}

/* ------------------------------------------------------------------ */
/* PunchRecord bulk edit                                               */
/* ------------------------------------------------------------------ */
export interface BulkEditPunchesArgs {
  ids: string[]
  /** allowed bulk fields ONLY: direction + a shared provenance note */
  direction?: string
  editReason?: string
  actor: { id: string; username: string }
}

export async function bulkEditPunches(args: BulkEditPunchesArgs): Promise<{ requested: number; updated: number; notFound: string[]; recalc: number; auditId: string }> {
  const ids = assertIds(args.ids)
  if (args.direction != null && !DIRECTIONS.has(args.direction)) throw new BulkValidationError('جهت نامعتبر است (IN/OUT/AUTO)')
  if (!args.direction && !args.editReason) throw new BulkValidationError('یکی از فیلدهای جهت یا یادداشت الزامی است')
  const editReason = args.editReason?.trim().slice(0, 500) || null
  if (editReason === null && args.editReason != null) throw new BulkValidationError('یادداشت ویرایش نمی‌تواند فقط فاصله باشد')

  return withLock('punch', async () => {
    const before = await db.punchRecord.findMany({ where: { id: { in: ids } }, select: { id: true, personId: true, timestamp: true, direction: true, edited: true, editReason: true } })
    const foundIds = before.map((p) => p.id)
    const notFound = ids.filter((id) => !foundIds.includes(id))
    if (!before.length) return { requested: ids.length, updated: 0, notFound, recalc: 0, auditId: '' }

    const auditId = await db.$transaction(async (tx) => {
      const r = await tx.punchRecord.updateMany({
        where: { id: { in: foundIds } },
        data: {
          ...(args.direction ? { direction: args.direction } : {}),
          edited: true,
          ...(editReason !== null ? { editReason } : {}),
        },
      })
      if (r.count !== foundIds.length) throw new Error('تراکنش ناسازگار — تغییرات اعمال نشد')
      const audit = await tx.auditLog.create({
        data: {
          userId: args.actor.id,
          action: 'PUNCH_BULK_EDIT',
          entity: 'PunchRecord',
          details: JSON.stringify({
            count: foundIds.length,
            direction: args.direction ?? null,
            editReason,
            old: before.map((p) => ({ id: p.id, personId: p.personId, timestamp: p.timestamp.toISOString(), direction: p.direction, edited: p.edited, editReason: p.editReason })),
          }),
        },
      })
      return audit.id
    }, { timeout: 120_000, maxWait: 30_000 })

    const recalcCount = await recalcAffectedDays(before.map((p) => ({ personId: p.personId, timestamp: p.timestamp })))
    return { requested: ids.length, updated: foundIds.length, notFound, recalc: recalcCount, auditId }
  })
}

/* ------------------------------------------------------------------ */
/* PunchRecord bulk delete                                             */
/* ------------------------------------------------------------------ */
export async function bulkDeletePunches(args: { ids: string[]; reason: string; actor: { id: string; username: string } }): Promise<{ requested: number; deleted: number; notFound: string[]; recalc: number; auditId: string }> {
  const ids = assertIds(args.ids)
  const reason = args.reason?.trim()
  if (!reason) throw new BulkValidationError('برای حذف گروهی، ذکر دلیل الزامی است')

  return withLock('punch', async () => {
    const before = await db.punchRecord.findMany({ where: { id: { in: ids } }, select: { id: true, personId: true, timestamp: true, direction: true, source: true, edited: true, editReason: true, rawPayload: true } })
    const foundIds = before.map((p) => p.id)
    const notFound = ids.filter((id) => !foundIds.includes(id))
    if (!before.length) return { requested: ids.length, deleted: 0, notFound, recalc: 0, auditId: '' }

    const auditId = await db.$transaction(async (tx) => {
      const r = await tx.punchRecord.deleteMany({ where: { id: { in: foundIds } } })
      if (r.count !== foundIds.length) throw new Error('تراکنش ناسازگار — تغییرات اعمال نشد')
      const audit = await tx.auditLog.create({
        data: {
          userId: args.actor.id,
          action: 'PUNCH_BULK_DELETE',
          entity: 'PunchRecord',
          details: JSON.stringify({
            count: foundIds.length,
            reason,
            removed: before.map((p) => ({ id: p.id, personId: p.personId, timestamp: p.timestamp.toISOString(), direction: p.direction, source: p.source, edited: p.edited, editReason: p.editReason, rawPayload: p.rawPayload })),
          }),
        },
      })
      return audit.id
    }, { timeout: 120_000, maxWait: 30_000 })

    const recalcCount = await recalcAffectedDays(before.map((p) => ({ personId: p.personId, timestamp: p.timestamp })))
    return { requested: ids.length, deleted: foundIds.length, notFound, recalc: recalcCount, auditId }
  })
}

/* ------------------------------------------------------------------ */
/* RawPunch bulk delete = TOMBSTONE + projected punch removal          */
/* ------------------------------------------------------------------ */
export async function bulkDeleteRawPunches(args: { ids: string[]; reason: string; actor: { id: string; username: string } }): Promise<{ requested: number; tombstoned: number; projectedDeleted: number; notFound: string[]; recalc: number; auditId: string }> {
  const ids = assertIds(args.ids)
  const reason = args.reason?.trim()
  if (!reason) throw new BulkValidationError('برای حذف گروهی، ذکر دلیل الزامی است')

  return withLock('rawpunch', async () => {
    const before = await db.rawPunch.findMany({ where: { id: { in: ids }, deletedAt: null } })
    const foundIds = before.map((r) => r.id)
    const notFound = ids.filter((id) => !foundIds.includes(id))
    if (!before.length) return { requested: ids.length, tombstoned: 0, projectedDeleted: 0, notFound, recalc: 0, auditId: '' }

    // the projected operational rows of these raw records (the honest join:
    // matched raw → PunchRecord of the same person at the same instant)
    const matched = before.filter((r) => r.personnelId)
    const projected = matched.length
      ? await db.punchRecord.findMany({
          where: { OR: matched.map((r) => ({ personId: r.personnelId as string, timestamp: r.punchTime })) },
          select: { id: true, personId: true, timestamp: true, direction: true, source: true, edited: true, editReason: true, rawPayload: true },
        })
      : []

    const auditId = await db.$transaction(async (tx) => {
      const now = new Date()
      const t = await tx.rawPunch.updateMany({
        where: { id: { in: foundIds } },
        data: { deletedAt: now, deletedBy: args.actor.username, deleteReason: reason.slice(0, 500) },
      })
      if (t.count !== foundIds.length) throw new Error('تراکنش ناسازگار — تغییرات اعمال نشد')
      let pd = 0
      if (projected.length) {
        const d = await tx.punchRecord.deleteMany({ where: { id: { in: projected.map((p) => p.id) } } })
        pd = d.count
      }
      const audit = await tx.auditLog.create({
        data: {
          userId: args.actor.id,
          action: 'RAWPUNCH_BULK_DELETE',
          entity: 'RawPunch',
          details: JSON.stringify({
            count: foundIds.length,
            reason,
            projectedDeleted: pd,
            tombstoned: before.map((r) => ({ id: r.id, deviceSerial: r.deviceSerial, enrollNumber: r.enrollNumber, punchTime: r.punchTime.toISOString(), punchTimeRaw: r.punchTimeRaw, personnelId: r.personnelId, rawPayload: r.rawPayload })),
            removedOperational: projected,
          }),
        },
      })
      return audit.id
    }, { timeout: 120_000, maxWait: 30_000 })

    const recalcCount = await recalcAffectedDays(projected.map((p) => ({ personId: p.personId, timestamp: p.timestamp })))
    return { requested: ids.length, tombstoned: foundIds.length, projectedDeleted: projected.length, notFound, recalc: recalcCount, auditId }
  })
}

/* ------------------------------------------------------------------ */
/* DailyAttendance bulk override / clear                               */
/* ------------------------------------------------------------------ */
export async function bulkOverrideDaily(args: { ids: string[]; override: DailyOverridePatch | null; actor: { id: string; username: string } }): Promise<{ requested: number; overridden: number; notFound: string[]; auditId: string }> {
  const ids = assertIds(args.ids)
  if (args.override === null) {
    // clear: restore computed values by RECALCULATING without the override
    return withLock('daily', async () => {
      const before = await db.dailyAttendance.findMany({ where: { id: { in: ids }, manualOverride: { not: null } }, select: { id: true, personId: true, date: true } })
      const foundIds = before.map((b) => b.id)
      const notFound = ids.filter((id) => !foundIds.includes(id))
      if (!before.length) return { requested: ids.length, overridden: 0, notFound, auditId: '' }
      const auditId = await db.$transaction(async (tx) => {
        const r = await tx.dailyAttendance.updateMany({
          where: { id: { in: foundIds } },
          data: { manualOverride: null, overrideBy: null, overrideAt: null },
        })
        if (r.count !== foundIds.length) throw new Error('تراکنش ناسازگار — تغییرات اعمال نشد')
        const audit = await tx.auditLog.create({
          data: { userId: args.actor.id, action: 'DAILY_BULK_OVERRIDE_CLEAR', entity: 'DailyAttendance', details: JSON.stringify({ count: foundIds.length, rows: before.map((b) => ({ id: b.id, personId: b.personId, date: b.date.toISOString() })) }) },
        })
        return audit.id
      }, { timeout: 120_000, maxWait: 30_000 })
      // recompute the cleared days (computed values return immediately)
      for (const b of before) await recalcDayForPerson(b.personId, b.date).catch(() => null)
      return { requested: ids.length, overridden: foundIds.length, notFound, auditId }
    })
  }

  const ov = normalizeDailyOverride(args.override as unknown as Record<string, unknown>) // validation also inside the lib (defense in depth — the route validates too)
  const ovJson = JSON.stringify(ov)

  return withLock('daily', async () => {
    const before = await db.dailyAttendance.findMany({ where: { id: { in: ids } }, select: { id: true, personId: true, date: true, status: true, workedMinutes: true, lateMinutes: true, earlyLeaveMinutes: true, note: true, manualOverride: true } })
    const foundIds = before.map((b) => b.id)
    const notFound = ids.filter((id) => !foundIds.includes(id))
    if (!before.length) return { requested: ids.length, overridden: 0, notFound, auditId: '' }

    const now = new Date()
    const auditId = await db.$transaction(async (tx) => {
      for (const b of before) {
        // apply the override on top of the CURRENT stored row (works whether or
        // not an older override existed — the new one replaces it whole)
        await tx.dailyAttendance.update({
          where: { id: b.id },
          data: {
            ...(ov.status ? { status: ov.status } : {}),
            ...(ov.workedMinutes != null ? { workedMinutes: ov.workedMinutes } : {}),
            ...(ov.lateMinutes != null ? { lateMinutes: ov.lateMinutes } : {}),
            ...(ov.earlyLeaveMinutes != null ? { earlyLeaveMinutes: ov.earlyLeaveMinutes } : {}),
            ...(ov.note != null ? { note: ov.note } : {}),
            manualOverride: ovJson,
            overrideBy: args.actor.username,
            overrideAt: now,
          },
        })
      }
      const audit = await tx.auditLog.create({
        data: {
          userId: args.actor.id,
          action: 'DAILY_BULK_OVERRIDE',
          entity: 'DailyAttendance',
          details: JSON.stringify({
            count: foundIds.length,
            override: ov,
            old: before.map((b) => ({ id: b.id, personId: b.personId, date: b.date.toISOString(), status: b.status, workedMinutes: b.workedMinutes, lateMinutes: b.lateMinutes, earlyLeaveMinutes: b.earlyLeaveMinutes, note: b.note, previousOverride: b.manualOverride })),
          }),
        },
      })
      return audit.id
    }, { timeout: 120_000, maxWait: 30_000 })

    return { requested: ids.length, overridden: foundIds.length, notFound, auditId }
  })
}

/* ------------------------------------------------------------------ */
/* DailyAttendance bulk delete (derived rows — recomputable)            */
/* ------------------------------------------------------------------ */
export async function bulkDeleteDaily(args: { ids: string[]; reason: string; actor: { id: string; username: string } }): Promise<{ requested: number; deleted: number; notFound: string[]; auditId: string }> {
  const ids = assertIds(args.ids)
  const reason = args.reason?.trim()
  if (!reason) throw new BulkValidationError('برای حذف گروهی، ذکر دلیل الزامی است')

  return withLock('daily', async () => {
    const before = await db.dailyAttendance.findMany({ where: { id: { in: ids } }, select: { id: true, personId: true, date: true, status: true, workedMinutes: true, note: true, manualOverride: true } })
    const foundIds = before.map((b) => b.id)
    const notFound = ids.filter((id) => !foundIds.includes(id))
    if (!before.length) return { requested: ids.length, deleted: 0, notFound, auditId: '' }

    const auditId = await db.$transaction(async (tx) => {
      const r = await tx.dailyAttendance.deleteMany({ where: { id: { in: foundIds } } })
      if (r.count !== foundIds.length) throw new Error('تراکنش ناسازگار — تغییرات اعمال نشد')
      const audit = await tx.auditLog.create({
        data: {
          userId: args.actor.id,
          action: 'DAILY_BULK_DELETE',
          entity: 'DailyAttendance',
          details: JSON.stringify({
            count: foundIds.length,
            reason,
            removed: before.map((b) => ({ id: b.id, personId: b.personId, date: b.date.toISOString(), status: b.status, workedMinutes: b.workedMinutes, note: b.note, override: b.manualOverride })),
          }),
        },
      })
      return audit.id
    }, { timeout: 120_000, maxWait: 30_000 })

    return { requested: ids.length, deleted: foundIds.length, notFound, auditId }
  })
}

/* ------------------------------------------------------------------ */
/* affected-day recalculation (after commit; failures reported)         */
/* ------------------------------------------------------------------ */
async function recalcAffectedDays(pairs: Array<{ personId: string; timestamp: Date }>): Promise<number> {
  const unique = new Map<string, { personId: string; day: Date }>()
  for (const p of pairs) {
    const day = tehranDayOf(p.timestamp)
    unique.set(`${p.personId}|${day.toISOString()}`, { personId: p.personId, day })
  }
  let failed = 0
  for (const { personId, day } of unique.values()) {
    try {
      await recalcDayForPerson(personId, day)
    } catch {
      failed++
    }
  }
  if (failed) {
    await db.deviceLog.create({
      data: { level: 'WARN', message: redact(`بازمحاسبهٔ ${failed} روز پس از عملیات گروهی ناموفق بود — رکوردها ذخیره شده‌اند؛ از «بازمحاسبهٔ بازه» استفاده کنید`) },
    }).catch(() => {})
  }
  return unique.size - failed
}

function redact(s: string): string {
  return s.replace(/\b\d{6,}\b/g, '…') // long digit runs (secrets/serials) never logged
}
