import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole } from '@/lib/auth'
import { fail } from '@/lib/api'

/**
 * GET /api/raw-punches — v1.3.1 (Phase 1 review fixes 1 & 3).
 * Read-only admin surface for the RawPunch landing zone so UNMATCHED and
 * AMBIGUOUS rows are visible for administrator review (they are never
 * auto-assigned and never deleted). Also exposes the RejectedRecord quarantine
 * so no source record disappears without a trace.
 *
 * Query params:
 *   ?unmatched=1          — only rows without a personnel mapping
 *   ?ambiguous=1          — only rows whose mapping is ambiguous (matchNote set)
 *   ?serial=<serial>      — filter by device serial
 *   ?rejected=1           — list quarantined (malformed) source records instead
 *   ?take=<n>             — page size (default 100, max 500)
 *   ?skip=<n>             — offset
 *
 * No device credentials are involved anywhere in this surface.
 */
export async function GET(req: NextRequest) {
  try {
    await requireRole('ADMIN')
    const sp = req.nextUrl.searchParams
    const take = Math.min(Math.max(Number(sp.get('take') ?? 100) || 100, 1), 500)
    const skip = Math.max(Number(sp.get('skip') ?? 0) || 0, 0)
    const serial = sp.get('serial')?.trim() || null

    if (sp.get('rejected') === '1') {
      const where = {
        ...(serial ? { deviceSerial: serial } : {}),
      }
      const [rows, total] = await Promise.all([
        db.rejectedRecord.findMany({ where, orderBy: { receivedAt: 'desc' }, take, skip }),
        db.rejectedRecord.count({ where }),
      ])
      return NextResponse.json({ rejected: rows, total })
    }

    const unmatchedOnly = sp.get('unmatched') === '1'
    const ambiguousOnly = sp.get('ambiguous') === '1'
    const serialFilter = serial ? { deviceSerial: serial } : {}
    const where = {
      ...serialFilter,
      ...(unmatchedOnly ? { matchStatus: 'UNMATCHED' } : {}),
      ...(ambiguousOnly ? { matchStatus: 'UNMATCHED', matchNote: { not: null } } : {}),
    }
    const unmatchedWhere = { ...serialFilter, matchStatus: 'UNMATCHED' }
    const ambiguousWhere = { ...serialFilter, matchStatus: 'UNMATCHED', matchNote: { not: null } }
    const [rows, total, unmatchedCount, ambiguousCount] = await Promise.all([
      db.rawPunch.findMany({
        where,
        orderBy: { punchTime: 'desc' },
        take,
        skip,
        select: {
          id: true,
          deviceSerial: true,
          sourceProtocol: true,
          enrollNumber: true,
          punchTime: true,
          punchTimeRaw: true,
          inOutModeRaw: true,
          verifyModeRaw: true,
          workCode: true,
          sourceRecordId: true,
          matchStatus: true,
          matchNote: true,
          direction: true,
          personnelId: true,
          deviceId: true,
          syncRunId: true,
          importedAt: true,
        },
      }),
      db.rawPunch.count({ where }),
      db.rawPunch.count({ where: unmatchedWhere }),
      db.rawPunch.count({ where: ambiguousWhere }),
    ])
    return NextResponse.json({ rawPunches: rows, total, unmatchedCount, ambiguousCount })
  } catch (err) {
    return fail(err)
  }
}
