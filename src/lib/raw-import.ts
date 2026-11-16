/**
 * RawPunch ingest pipeline — the single, source-agnostic entry point every
 * attendance import goes through (SBXPC pull today, FKATTEND x86 bridge in
 * Phase 2, reference CSV later). Phase 1 (Data Integrity & Security),
 * corrected in v1.3.1 after the Phase 1 code review.
 *
 * Guarantees (mirrored in prisma/schema.prisma comments):
 *   1. NO LOSS   — every record the source reports is accounted for: valid ones
 *                  are stored (matched or not), malformed ones are quarantined
 *                  (RejectedRecord) with their raw payload and a reason, and
 *                  records excluded by a requested date window are counted
 *                  separately by the orchestrator. Unmatched rows get
 *                  matchStatus=UNMATCHED and stay visible forever; they are
 *                  never dropped, and the sync cursor never runs past data
 *                  that is not safely persisted.
 *   2. NO DUPES  — identity = deviceSerial + sourceProtocol + enrollNumber +
 *                  rawFingerprint (sha-256 of the canonical raw tuple) +
 *                  dupIndex. `dupIndex` counts otherwise-identical records so
 *                  simultaneous punches (same person, same second, same raw
 *                  fields) are stored as distinct rows instead of collapsing.
 *   3. NO GUESSING — direction stays "UNKNOWN"; raw in/out & verify values are
 *                  preserved as-is. punchTime is the UTC instant, punchTimeRaw
 *                  keeps the device wall-clock string for auditing.
 *                  Personnel mapping NEVER guesses either (v1.3.1 fix 1):
 *                  enroll numbers are matched as EXACT strings against two
 *                  separate namespaces (deviceUserId, personnelCode); an
 *                  ambiguous or colliding number is left UNMATCHED with a
 *                  matchNote explaining why, for an administrator to resolve.
 *   4. ATOMIC    — raw rows + projection + bookmark advance in ONE transaction:
 *                  if anything fails, nothing is stored and the bookmark does
 *                  not move (the whole batch is retried next run). Rematching
 *                  (v1.3.1 fix 3) is atomic per record as well: mapping update,
 *                  legacy projection and the AuditLog entry commit together or
 *                  not at all, so a failed rematch stays UNMATCHED and is
 *                  recoverable by simply re-running it.
 *
 * The personnel projection (PunchRecord) is a best-effort enrichment of the
 * legacy attendance pipeline: only MATCHED rows are projected, direction "AUTO"
 * (= legacy vocabulary for undetermined — the authoritative value lives in
 * RawPunch.direction = UNKNOWN).
 */
import { createHash } from 'crypto'
import { Prisma } from '@prisma/client'
import { db } from '@/lib/db'
import { tehranDayOf } from '@/lib/jalali'

export type RawSourceProtocol =
  | 'FKATTEND' // NP756+ via the x86 FKAttend bridge (Phase 2)
  | 'SBXPC' // NP-series binary pull dialect (existing path)
  | 'RTLOG' // real-time push dialect (JSON, evidence pending)
  | 'EBIO' // Ebio XML push
  | 'CSV_REFERENCE' // the reference CSV import (comparison path)
  | 'MOCK' // test/mock device — clearly marked, never real data
  | (string & {}) // forward compatibility

/** A single raw attendance record as reported by a source. No interpretation. */
export interface RawPunchInput {
  /** enroll/user number exactly as the source reports it (kept as text) */
  enrollNumber: string | number
  /** UTC instant of the punch (sources convert the device wall clock upstream) */
  punchTime: Date
  /** device wall-clock string as received — preserved for auditing ("" if n/a) */
  punchTimeRaw?: string
  /** raw in/out value — meaning NOT verified; never mapped to a direction here */
  inOutModeRaw?: number
  /** raw verify value */
  verifyModeRaw?: number
  workCode?: string
  /** protocol-level record id when the source provides one (RTLOG log_id, …) */
  sourceRecordId?: string
  /** minimal raw payload (hex / JSON) for auditing */
  rawPayload?: string
}

export interface IngestOptions {
  deviceId: string | null
  deviceSerial: string
  sourceProtocol: RawSourceProtocol
  syncRunId?: string | null
  /** write the legacy PunchRecord projection for matched rows (default true) */
  project?: boolean
}

export interface IngestResult {
  seen: number
  invalid: number
  newCount: number
  duplicate: number
  matched: number
  unmatched: number
  /** new rows left UNMATCHED because the enroll number is ambiguous (fix 1) */
  ambiguous: number
  punchRecordsCreated: number
  minPunchTime: Date | null
  maxPunchTime: Date | null
  /** (personId, day) pairs that received new legacy punches — for recalc by the caller */
  touchedDays: Array<{ personId: string; day: Date }>
}

const TX_OPTIONS = { timeout: 120_000, maxWait: 10_000 }

/** sha-256 fingerprint of the canonical raw tuple — the dedupe identity. */
export function rawFingerprint(input: RawPunchInput, deviceSerial: string, sourceProtocol: string): string {
  const canonical = [
    'v1',
    deviceSerial,
    sourceProtocol,
    String(input.enrollNumber),
    String(Math.floor(input.punchTime.getTime())),
    String(input.inOutModeRaw ?? 0),
    String(input.verifyModeRaw ?? 0),
    String(input.workCode ?? ''),
    String(input.sourceRecordId ?? ''),
  ].join('|')
  return createHash('sha256').update(canonical, 'utf8').digest('hex')
}

function isP2002(err: unknown): boolean {
  return err instanceof Prisma.PrismaClientKnownRequestError && err.code === 'P2002'
}

/* ============================================================
 * Personnel mapping (v1.3.1 — review fix 1: never guess)
 * ============================================================
 * Enroll numbers are EXACT strings — no numeric conversion anywhere, so
 * leading zeros can never be lost and "07" is not "7". Two SEPARATE indexes
 * are built: one keyed by deviceUserId, one by personnelCode. A punch is
 * auto-matched ONLY when exactly one unambiguous candidate exists:
 *   * deviceUserId claimed by more than one person  → AMBIGUOUS (never assigned)
 *   * deviceUserId of person A == personnelCode of person B → AMBIGUOUS
 *   * no candidate at all → plain UNMATCHED
 * Ambiguous rows keep personnelId=null and carry an AMBIGUOUS_* matchNote so
 * an administrator can see WHY they are unassigned and resolve them manually.
 */
export interface PersonnelIndex {
  byDeviceUserId: Map<string, string[]> // exact deviceUserId string → personnel ids
  byPersonnelCode: Map<string, string[]> // exact personnelCode string → personnel ids
}

export async function buildPersonnelIndex(): Promise<PersonnelIndex> {
  const persons = await db.personnel.findMany({ select: { id: true, deviceUserId: true, personnelCode: true } })
  const byDeviceUserId = new Map<string, string[]>()
  const byPersonnelCode = new Map<string, string[]>()
  for (const p of persons) {
    if (p.deviceUserId != null) {
      const key = String(p.deviceUserId) // exact — never Number(), never padded
      const arr = byDeviceUserId.get(key) ?? []
      arr.push(p.id)
      byDeviceUserId.set(key, arr)
    }
    const codeKey = String(p.personnelCode) // exact string, leading zeros preserved
    const arr = byPersonnelCode.get(codeKey) ?? []
    arr.push(p.id)
    byPersonnelCode.set(codeKey, arr)
  }
  return { byDeviceUserId, byPersonnelCode }
}

export type PersonnelResolution =
  | { status: 'MATCHED'; personnelId: string; matchedVia: 'DEVICE_USER_ID' | 'PERSONNEL_CODE' }
  | { status: 'AMBIGUOUS'; note: string }
  | { status: 'UNMATCHED' }

/** Resolve one enroll number against the personnel index — exact strings only. */
export function resolvePersonnelForEnroll(enrollNumber: string, index: PersonnelIndex): PersonnelResolution {
  const enroll = String(enrollNumber)
  const uidOwners = index.byDeviceUserId.get(enroll) ?? []
  const codeOwners = index.byPersonnelCode.get(enroll) ?? []

  // multiple owners of the same deviceUserId → can never auto-assign
  if (uidOwners.length > 1) {
    return {
      status: 'AMBIGUOUS',
      note: `AMBIGUOUS_DEVICE_USER_ID: enroll number "${enroll}" is the deviceUserId of ${uidOwners.length} different personnel`,
    }
  }
  // personnelCode is @unique so >1 is a data anomaly — treat as ambiguous, never guess
  if (codeOwners.length > 1) {
    return {
      status: 'AMBIGUOUS',
      note: `AMBIGUOUS_PERSONNEL_CODE: enroll number "${enroll}" is the personnelCode of ${codeOwners.length} different personnel`,
    }
  }
  const uidOwner = uidOwners[0] ?? null
  const codeOwner = codeOwners[0] ?? null

  // the number resolves in BOTH namespaces to different people → ambiguous
  if (uidOwner && codeOwner && uidOwner !== codeOwner) {
    return {
      status: 'AMBIGUOUS',
      note: `AMBIGUOUS_NAMESPACE_COLLISION: enroll number "${enroll}" is the deviceUserId of one person and the personnelCode of another`,
    }
  }
  const personnelId = uidOwner ?? codeOwner
  if (!personnelId) return { status: 'UNMATCHED' }
  return { status: 'MATCHED', personnelId, matchedVia: uidOwner ? 'DEVICE_USER_ID' : 'PERSONNEL_CODE' }
}

/**
 * Store one batch of raw punches atomically.
 *
 * NOTE: `records` must already be window-filtered by the caller; this function
 * stores EVERYTHING it receives (it never drops unmatched rows) and advances
 * Device.rawCursorAt only inside the same transaction as the inserts.
 * (Malformed records with non-finite timestamps are still counted defensively
 * as `invalid` — but the orchestrator quarantines them BEFORE calling here.)
 */
export async function ingestRawPunches(records: RawPunchInput[], opts: IngestOptions): Promise<IngestResult> {
  const { deviceId, deviceSerial, sourceProtocol } = opts
  const syncRunId = opts.syncRunId ?? null
  const project = opts.project ?? true

  // ---- normalise + fingerprint (pure, outside tx) --------------------------
  type Normalised = {
    enrollNumber: string
    punchTime: Date
    punchTimeRaw: string
    inOutModeRaw: number
    verifyModeRaw: number
    workCode: string
    sourceRecordId: string
    rawPayload: string | null
    fingerprint: string
  }
  const valid: Normalised[] = []
  let invalid = 0
  for (const r of records) {
    const ms = r.punchTime?.getTime?.()
    if (typeof ms !== 'number' || !Number.isFinite(ms)) {
      invalid++
      continue
    }
    valid.push({
      enrollNumber: String(r.enrollNumber), // EXACT string — leading zeros preserved
      punchTime: r.punchTime,
      punchTimeRaw: String(r.punchTimeRaw ?? ''),
      inOutModeRaw: Number.isFinite(r.inOutModeRaw) ? Number(r.inOutModeRaw) : 0,
      verifyModeRaw: Number.isFinite(r.verifyModeRaw) ? Number(r.verifyModeRaw) : 0,
      workCode: String(r.workCode ?? ''),
      sourceRecordId: String(r.sourceRecordId ?? ''),
      rawPayload: r.rawPayload != null ? String(r.rawPayload) : null,
      fingerprint: rawFingerprint(r, deviceSerial, sourceProtocol),
    })
  }

  if (valid.length === 0) {
    return {
      seen: records.length, invalid, newCount: 0, duplicate: 0, matched: 0, unmatched: 0, ambiguous: 0,
      punchRecordsCreated: 0, minPunchTime: null, maxPunchTime: null, touchedDays: [],
    }
  }

  const personnelIndex = await buildPersonnelIndex()

  return db.$transaction(async (tx) => {
    // ---- existing identical records (dedupe base) --------------------------
    const existing = await tx.rawPunch.findMany({
      where: { deviceSerial, sourceProtocol },
      select: { enrollNumber: true, rawFingerprint: true },
    })
    const existingCount = new Map<string, number>() // enroll|fingerprint → rows already stored
    for (const row of existing) {
      const key = `${row.enrollNumber}|${row.rawFingerprint}`
      existingCount.set(key, (existingCount.get(key) ?? 0) + 1)
    }

    // ---- partition the batch: occurrence i < existing → duplicate, else new row at dupIndex = i
    const occurrence = new Map<string, number>()
    type NewRow = Normalised & { dupIndex: number; personnelId: string | null; matchNote: string | null }
    const newRows: NewRow[] = []
    let duplicate = 0
    let ambiguous = 0
    let maxTs: number | null = null
    let minTs: number | null = null

    for (const rec of valid) {
      const ms = rec.punchTime.getTime()
      if (maxTs === null || ms > maxTs) maxTs = ms
      if (minTs === null || ms < minTs) minTs = ms

      const key = `${rec.enrollNumber}|${rec.fingerprint}`
      const i = occurrence.get(key) ?? 0
      occurrence.set(key, i + 1)
      const already = existingCount.get(key) ?? 0
      if (i < already) {
        duplicate++
        continue // this identical record is already safely stored
      }
      const resolution = resolvePersonnelForEnroll(rec.enrollNumber, personnelIndex)
      const personnelId = resolution.status === 'MATCHED' ? resolution.personnelId : null
      const matchNote = resolution.status === 'AMBIGUOUS' ? resolution.note : null
      if (resolution.status === 'AMBIGUOUS') ambiguous++
      newRows.push({ ...rec, dupIndex: i, personnelId, matchNote })
    }

    // ---- store RawPunch rows (createMany; per-row fallback on conflict) ---
    let newCount = 0
    const rowPayload = newRows.map((r) => ({
      deviceSerial,
      sourceProtocol,
      enrollNumber: r.enrollNumber,
      punchTime: r.punchTime,
      punchTimeRaw: r.punchTimeRaw,
      inOutModeRaw: r.inOutModeRaw,
      verifyModeRaw: r.verifyModeRaw,
      workCode: r.workCode,
      sourceRecordId: r.sourceRecordId,
      rawFingerprint: r.fingerprint,
      dupIndex: r.dupIndex,
      deviceId,
      personnelId: r.personnelId,
      matchStatus: r.personnelId ? 'MATCHED' : 'UNMATCHED',
      direction: 'UNKNOWN',
      matchNote: r.matchNote,
      rawPayload: r.rawPayload,
      syncRunId,
    }))
    if (rowPayload.length > 0) {
      try {
        newCount = (await tx.rawPunch.createMany({ data: rowPayload })).count
      } catch {
        // rare conflict (e.g. concurrent run) — fall back to row-by-row
        newCount = 0
        for (const data of rowPayload) {
          try {
            await tx.rawPunch.create({ data })
            newCount++
          } catch (err) {
            if (isP2002(err)) duplicate++
            else throw err
          }
        }
      }
    }

    // ---- legacy projection: PunchRecord for MATCHED rows only -------------
    let punchRecordsCreated = 0
    const touchedDays: Array<{ personId: string; day: Date }> = []
    const projected = new Set<string>() // personId|ms — the legacy table allows one row per person+instant
    if (project) {
      for (const r of newRows) {
        if (!r.personnelId) continue
        const ms = r.punchTime.getTime()
        const seenKey = `${r.personnelId}|${ms}`
        if (projected.has(seenKey)) continue // simultaneous identical punch — legacy table keeps the first
        projected.add(seenKey)
        try {
          await tx.punchRecord.create({
            data: {
              personId: r.personnelId,
              timestamp: r.punchTime,
              direction: 'AUTO', // legacy vocabulary for "undetermined" — RawPunch.direction is the authority (UNKNOWN)
              source: 'DEVICE',
              deviceId,
              deviceUserId: Number(r.enrollNumber) || null,
              rawPayload: r.rawPayload ?? undefined,
            },
          })
          punchRecordsCreated++
          // legacy attendance-day key — the VERIFIED Tehran convention
          // (tehranDayOf, +03:30, same rule recalcRange/DailyAttendance use).
          // v1.3.1 fix 2: the day comes from THIS punch, and the orchestrator
          // recalcs exactly these (personId, day) pairs — never from a global
          // maxPunchTime that could point at the wrong day.
          touchedDays.push({ personId: r.personnelId, day: tehranDayOf(r.punchTime) })
        } catch (err) {
          if (isP2002(err)) continue // already projected by an earlier run/path
          throw err
        }
      }
    }

    // ---- bookmark: advances ONLY now, inside the same transaction ---------
    if (deviceId && maxTs !== null) {
      await tx.device.update({
        where: { id: deviceId },
        data: { rawCursorAt: new Date(maxTs) },
      }).catch((err: unknown) => {
        if (!(err instanceof Prisma.PrismaClientKnownRequestError && err.code === 'P2025')) throw err
      })
    }

    const matched = newRows.filter((r) => r.personnelId).length
    return {
      seen: records.length,
      invalid,
      newCount,
      duplicate,
      matched,
      unmatched: newCount - matched,
      ambiguous,
      punchRecordsCreated,
      minPunchTime: minTs !== null ? new Date(minTs) : null,
      maxPunchTime: maxTs !== null ? new Date(maxTs) : null,
      touchedDays,
    }
  }, TX_OPTIONS)
}

/* ============================================================
 * Quarantine (v1.3.1 — review fix 4: no source record disappears)
 * ============================================================
 * A record the source reported but that cannot be stored as a RawPunch (e.g.
 * a non-finite / invalid timestamp) is quarantined HERE — with everything the
 * source gave us — instead of being silently dropped. Writes are idempotent
 * (fingerprint dedupe), so a 30-second rt-poll re-reporting the same bad
 * record cannot flood the table; only the FIRST quarantine creates a row.
 */
export interface QuarantineSpec {
  record: RawPunchInput
  reason: string // machine-readable, e.g. INVALID_TIMESTAMP
  detail?: string
}

export interface QuarantineResult {
  quarantined: number // rows newly written this call
  duplicate: number // already quarantined on a previous call/run
}

export function rejectedFingerprint(spec: QuarantineSpec, deviceSerial: string, sourceProtocol: string): string {
  const r = spec.record
  const canonical = [
    'rej-v1',
    deviceSerial,
    sourceProtocol,
    String(r.enrollNumber ?? ''),
    String(r.punchTimeRaw ?? ''),
    String(r.rawPayload ?? ''),
    String(spec.reason),
  ].join('|')
  return createHash('sha256').update(canonical, 'utf8').digest('hex')
}

export async function quarantineRawPunches(
  specs: QuarantineSpec[],
  opts: { deviceId: string | null; deviceSerial: string; sourceProtocol: RawSourceProtocol; syncRunId?: string | null },
): Promise<QuarantineResult> {
  if (specs.length === 0) return { quarantined: 0, duplicate: 0 }

  const rows = specs.map((spec) => {
    const r = spec.record
    const meta: Record<string, unknown> = {}
    if (r.inOutModeRaw !== undefined) meta.inOutModeRaw = r.inOutModeRaw
    if (r.verifyModeRaw !== undefined) meta.verifyModeRaw = r.verifyModeRaw
    if (r.workCode !== undefined) meta.workCode = r.workCode
    if (r.sourceRecordId !== undefined) meta.sourceRecordId = r.sourceRecordId
    if (spec.detail) meta.detail = spec.detail
    return {
      deviceId: opts.deviceId,
      deviceSerial: opts.deviceSerial,
      sourceProtocol: opts.sourceProtocol,
      syncRunId: opts.syncRunId ?? null,
      enrollNumber: r.enrollNumber != null ? String(r.enrollNumber) : null,
      punchTimeRaw: r.punchTimeRaw != null ? String(r.punchTimeRaw) : null,
      rawPayload: r.rawPayload != null ? String(r.rawPayload) : null,
      rejectReason: spec.reason,
      sourceMeta: Object.keys(meta).length ? JSON.stringify(meta) : null,
      fingerprint: rejectedFingerprint(spec, opts.deviceSerial, String(opts.sourceProtocol)),
    }
  })

  return db.$transaction(async (tx) => {
    let quarantined = 0
    let duplicate = 0
    try {
      quarantined = (await tx.rejectedRecord.createMany({ data: rows })).count
    } catch {
      quarantined = 0
      for (const data of rows) {
        try {
          await tx.rejectedRecord.create({ data })
          quarantined++
        } catch (err) {
          if (isP2002(err)) duplicate++ // already quarantined by an earlier run
          else throw err
        }
      }
    }
    return { quarantined, duplicate }
  }, TX_OPTIONS)
}

/* ============================================================
 * Rematch (v1.3.1 — review fix 3: atomic + auditable)
 * ============================================================
 * Rematch UNMATCHED RawPunch rows against the personnel table. For every row:
 * mapping update + legacy projection + AuditLog entry happen in ONE database
 * transaction — if the projection fails, the mapping update rolls back too,
 * so the row stays UNMATCHED and is retried by the next rematch (idempotent).
 * Ambiguous enroll numbers are never assigned; their matchNote is refreshed so
 * the reason stays visible for administrator review.
 */
export interface RematchResult {
  matched: number
  punchRecordsCreated: number
  /** rows whose enroll number is ambiguous — left UNMATCHED on purpose */
  ambiguous: number
  /** per-row failures (transaction rolled back — rows stay UNMATCHED, recoverable) */
  failed: number
  errors: string[]
}

export interface RematchOptions {
  /** acting administrator (for the AuditLog rows); null = system/background */
  userId?: string | null
  /**
   * TEST SEAM ONLY (never set in production): replaces the legacy-projection
   * step inside the per-row transaction so tests can simulate a failure
   * between the mapping update and the projection creation.
   */
  _projectionCreate?: (tx: Prisma.TransactionClient, raw: { id: string; personnelId: string; punchTime: Date; deviceId: string | null; enrollNumber: string; rawPayload: string | null }) => Promise<unknown>
}

export async function rematchRawPunches(opts: RematchOptions = {}): Promise<RematchResult> {
  const candidates = await db.rawPunch.findMany({
    where: { matchStatus: 'UNMATCHED' },
    select: { id: true, enrollNumber: true, personnelId: true, matchNote: true },
  })

  const index = await buildPersonnelIndex()
  let matched = 0
  let punchRecordsCreated = 0
  let ambiguous = 0
  let failed = 0
  const errors: string[] = []

  for (const row of candidates) {
    const resolution = resolvePersonnelForEnroll(row.enrollNumber, index)

    if (resolution.status === 'AMBIGUOUS') {
      ambiguous++
      // keep the row UNMATCHED but refresh the reason so it stays explainable
      if (row.matchNote !== resolution.note) {
        await db.rawPunch
          .update({ where: { id: row.id }, data: { matchNote: resolution.note } })
          .catch(() => {})
      }
      continue
    }
    if (resolution.status !== 'MATCHED') continue

    const projectionCreate =
      opts._projectionCreate ??
      (async (tx: Prisma.TransactionClient, raw: { personnelId: string; punchTime: Date; deviceId: string | null; enrollNumber: string; rawPayload: string | null }) => {
        try {
          await tx.punchRecord.create({
            data: {
              personId: raw.personnelId,
              timestamp: raw.punchTime,
              direction: 'AUTO', // legacy "undetermined" — RawPunch.direction stays the authority
              source: 'DEVICE',
              deviceId: raw.deviceId,
              deviceUserId: Number(raw.enrollNumber) || null,
              rawPayload: raw.rawPayload ?? undefined,
            },
          })
        } catch (err) {
          if (isP2002(err)) return // already projected by an earlier run/path — not a failure
          throw err
        }
      })

    try {
      // ONE transaction: mapping update + projection + audit — all or nothing
      await db.$transaction(async (tx) => {
        const raw = await tx.rawPunch.findUnique({ where: { id: row.id } })
        if (!raw) return // row vanished (device cleanup) — nothing to do
        if (raw.matchStatus !== 'UNMATCHED') return // idempotency guard (already matched elsewhere)
        await tx.rawPunch.update({
          where: { id: row.id },
          data: { personnelId: resolution.personnelId, matchStatus: 'MATCHED', matchNote: null },
        })
        await projectionCreate(tx, {
          id: raw.id,
          personnelId: resolution.personnelId,
          punchTime: raw.punchTime,
          deviceId: raw.deviceId,
          enrollNumber: raw.enrollNumber,
          rawPayload: raw.rawPayload,
        })
        // Audit record: identifies the RawPunch, the previous mapping, the new
        // mapping and the operation time (createdAt). Contains no device
        // credentials — only ids and the enroll number.
        await tx.auditLog.create({
          data: {
            userId: opts.userId ?? null,
            action: 'RAW_PUNCH_REMATCH',
            entity: 'RawPunch',
            entityId: row.id,
            details: JSON.stringify({
              rawPunchId: row.id,
              enrollNumber: raw.enrollNumber,
              deviceSerial: raw.deviceSerial,
              sourceProtocol: raw.sourceProtocol,
              punchTime: raw.punchTime.toISOString(),
              previousMapping: { personnelId: raw.personnelId, matchStatus: raw.matchStatus },
              newMapping: { personnelId: resolution.personnelId, matchStatus: 'MATCHED' },
              matchedVia: resolution.matchedVia,
              operationTime: new Date().toISOString(),
            }),
          },
        })
      }, TX_OPTIONS)
      matched++
      punchRecordsCreated++
    } catch (err) {
      // the whole transaction rolled back: still UNMATCHED, still recoverable
      failed++
      errors.push(`${row.id}: ${(err as Error)?.message ?? String(err)}`)
    }
  }
  return { matched, punchRecordsCreated, ambiguous, failed, errors }
}
