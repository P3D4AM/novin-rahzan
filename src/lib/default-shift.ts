/**
 * Phase 2 S3 review fix 1 — dated default-shift history.
 * =====================================================================
 * `Personnel.defaultShiftId` is a single CURRENT value; before this module
 * the engine used it for EVERY day without an explicit ShiftAssignment, so
 * changing today's default silently rewrote historical recalculation.
 *
 * This module gives the default shift a DATED, retrievable history
 * (DefaultShiftHistory rows) and one deterministic resolution rule:
 *
 *   defaultShiftFor(D) =
 *     1. the LATEST entry with effectiveFrom <= D      (dated history)
 *     2. else, when entries exist, the EARLIEST entry  (backward extension)
 *     3. else null                                      (no default known)
 *
 * Backward extension — the documented legacy limitation (docs/TIME-POLICIES.md):
 *   installs that predate this table have one migration-snapshot row per
 *   person ("as of migration day the default was X" — a fact recorded at
 *   migration time, NOT fabricated back-dated history). Days before the
 *   earliest entry resolve to that entry's shift, which reproduces the
 *   pre-upgrade behavior exactly (no rewrite of legacy rows on upgrade);
 *   the true sequence of pre-history changes is unknowable and is never
 *   invented. After the first dated change, days before it keep resolving to
 *   the older entry/snapshot — changing today's shift can never move the
 *   pre-effective-date past.
 *
 * Rows are written ONLY by `recordDefaultShiftChange` (personnel create /
 * edit / migration snapshot) — the engine NEVER writes history; recalc is
 * idempotent over it.
 *
 * Explicit per-day ShiftAssignment ALWAYS has priority over the dated default
 * (see buildShiftResolver in src/lib/attendance.ts).
 */
import { db } from '@/lib/db'
import { startOfUTCDay } from '@/lib/jalali'
import type { DayMarker } from '@/lib/time'

/** One dated default-shift row (as stored, or synthesized legacy fallback). */
export interface DatedDefaultShiftEntry {
  /** Tehran-day marker (UTC midnight) this default becomes effective. */
  effectiveFrom: Date
  /** shift id, or null = no default shift from this date on. */
  shiftId: string | null
}

/**
 * PURE. Resolve the default shift id applicable on `day`.
 * `entries` need not be pre-sorted (this sorts a copy, ties on the same
 * effectiveFrom resolve to the later-listed row deterministically).
 */
export function resolveDatedDefaultShiftId(
  entries: DatedDefaultShiftEntry[],
  day: Date,
): string | null {
  if (!entries.length) return null
  const sorted = [...entries].sort((a, b) => a.effectiveFrom.getTime() - b.effectiveFrom.getTime())
  let result: string | null = sorted[0].shiftId // rule 2: backward extension
  for (const e of sorted) {
    if (e.effectiveFrom.getTime() <= day.getTime()) result = e.shiftId // rule 1: latest covering
  }
  return result
}

/** Load the dated default-shift entries of the given persons (single query). */
export async function loadDatedDefaultShiftEntries(
  personIds: string[],
  /** Fallback for persons with a defaultShiftId but no history rows yet
   * (e.g. rows created before this table existed and never migrated):
   * the CURRENT default extends over the whole timeline — legacy behavior,
   * recorded in memory only, never written to the database. */
  currentDefaults: Map<string, string | null>,
): Promise<Map<string, DatedDefaultShiftEntry[]>> {
  const out = new Map<string, DatedDefaultShiftEntry[]>()
  for (const id of personIds) out.set(id, [])
  if (!personIds.length) return out
  const rows = await db.defaultShiftHistory.findMany({
    where: { personId: { in: personIds } },
    orderBy: [{ effectiveFrom: 'asc' }, { createdAt: 'asc' }],
    select: { personId: true, effectiveFrom: true, shiftId: true },
  })
  for (const r of rows) out.get(r.personId)?.push({ effectiveFrom: r.effectiveFrom, shiftId: r.shiftId })
  for (const id of personIds) {
    const list = out.get(id)
    if (list && !list.length) {
      const current = currentDefaults.get(id)
      if (current) list.push({ effectiveFrom: startOfUTCDay(new Date(0)), shiftId: current })
    }
  }
  return out
}

/**
 * Record a default-shift change: writes Personnel.defaultShiftId (the current
 * value for new assignments / UI) AND one dated DefaultShiftHistory row.
 * No-op when the value is unchanged (a re-submitted edit writes nothing —
 * recalc stability). Used by the personnel API routes (create / edit) and the
 * v1.7.0-s3r2 migration snapshot path (tests exercise it directly through
 * the real database).
 *
 * `previousShiftId`: the value the CALLER read before its own row update
 * (personnel PUT updates Personnel.defaultShiftId first, then calls this) —
 * omit it to compare against the current stored row (personnel POST /
 * migration snapshot, where no prior write happened).
 */
export async function recordDefaultShiftChange(input: {
  personId: string
  /** new default shift id (null = default removed); undefined = no change */
  shiftId?: string | null
  /** Tehran-day marker (UTC midnight) the new default becomes effective. */
  effectiveFrom: DayMarker
  note?: string
  /** caller-known OLD value (read before the caller's own update); omit to use the stored row */
  previousShiftId?: string | null
}): Promise<{ changed: boolean; effectiveFrom: Date; shiftId: string | null }> {
  const { personId, effectiveFrom, note } = input
  const shiftId = input.shiftId === undefined ? undefined : input.shiftId
  const day = startOfUTCDay(effectiveFrom)
  const person = await db.personnel.findUnique({ where: { id: personId }, select: { defaultShiftId: true } })
  if (!person) throw new Error(`personnel not found: ${personId}`)
  const previous = input.previousShiftId !== undefined ? input.previousShiftId : person.defaultShiftId
  if (shiftId === undefined || shiftId === previous) {
    return { changed: false, effectiveFrom: day, shiftId: person.defaultShiftId }
  }
  if (shiftId) {
    const exists = await db.shift.findUnique({ where: { id: shiftId }, select: { id: true } })
    if (!exists) throw new Error(`shift not found: ${shiftId}`)
  }
  const [row] = await db.$transaction([
    db.defaultShiftHistory.create({
      data: { personId, effectiveFrom: day, shiftId, note: note ?? null },
    }),
    db.personnel.update({ where: { id: personId }, data: { defaultShiftId: shiftId } }),
  ])
  return { changed: true, effectiveFrom: row.effectiveFrom, shiftId: row.shiftId }
}
