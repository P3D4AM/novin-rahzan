/**
 * RawPunch ingest pipeline — the single, source-agnostic entry point every
 * attendance import goes through (SBXPC pull today, FKATTEND x86 bridge in
 * Phase 2, reference CSV later). Phase 1 (Data Integrity & Security).
 *
 * Guarantees (mirrored in prisma/schema.prisma comments):
 *   1. NO LOSS   — every valid record is stored, matched or not. Unmatched
 *                  rows get matchStatus=UNMATCHED and stay visible forever;
 *                  they are never dropped, and the sync cursor never runs
 *                  past data that is not safely persisted.
 *   2. NO DUPES  — identity = deviceSerial + sourceProtocol + enrollNumber +
 *                  rawFingerprint (sha-256 of the canonical raw tuple) +
 *                  dupIndex. `dupIndex` counts otherwise-identical records so
 *                  simultaneous punches (same person, same second, same raw
 *                  fields) are stored as distinct rows instead of collapsing.
 *   3. NO GUESSING — direction stays "UNKNOWN"; raw in/out & verify values are
 *                  preserved as-is. punchTime is the UTC instant, punchTimeRaw
 *                  keeps the device wall-clock string for auditing.
 *   4. ATOMIC    — raw rows + projection + bookmark advance in ONE transaction:
 *                  if anything fails, nothing is stored and the bookmark does
 *                  not move (the whole batch is retried next run).
 *
 * The personnel projection (PunchRecord) is a best-effort enrichment of the
 * legacy attendance pipeline: only MATCHED rows are projected, direction "AUTO"
 * (= legacy vocabulary for undetermined — the authoritative value lives in
 * RawPunch.direction = UNKNOWN).
 */
import { createHash } from 'crypto'
import { Prisma } from '@prisma/client'
import { db } from '@/lib/db'

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

/** personnel lookup: deviceUserId first, numeric personnelCode as fallback */
type PersonnelIndex = Map<number, { id: string }>

async function buildPersonnelIndex(): Promise<PersonnelIndex> {
  const persons = await db.personnel.findMany({ select: { id: true, deviceUserId: true, personnelCode: true } })
  const byUid = new Map<number, { id: string }>()
  for (const p of persons) {
    if (p.deviceUserId != null && !byUid.has(p.deviceUserId)) byUid.set(p.deviceUserId, { id: p.id })
  }
  for (const p of persons) {
    const code = Number(p.personnelCode)
    if (!Number.isNaN(code) && !byUid.has(code)) byUid.set(code, { id: p.id })
  }
  return byUid
}

/**
 * Store one batch of raw punches atomically.
 *
 * NOTE: `records` must already be window-filtered by the caller; this function
 * stores EVERYTHING it receives (it never drops unmatched rows) and advances
 * Device.rawCursorAt only inside the same transaction as the inserts.
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
      enrollNumber: String(r.enrollNumber),
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
      seen: records.length, invalid, newCount: 0, duplicate: 0, matched: 0, unmatched: 0,
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
    type NewRow = Normalised & { dupIndex: number; personnelId: string | null }
    const newRows: NewRow[] = []
    let duplicate = 0
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
      const person = personnelIndex.get(Number(rec.enrollNumber))
      newRows.push({ ...rec, dupIndex: i, personnelId: person?.id ?? null })
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
          // legacy attendance day key (Tehran day, 03:30 split) — same rule the old importer used
          const dayKey = new Date(ms - 3.5 * 3600 * 1000)
          dayKey.setUTCHours(0, 0, 0, 0)
          touchedDays.push({ personId: r.personnelId, day: dayKey })
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
      punchRecordsCreated,
      minPunchTime: minTs !== null ? new Date(minTs) : null,
      maxPunchTime: maxTs !== null ? new Date(maxTs) : null,
      touchedDays,
    }
  }, TX_OPTIONS)
}

/**
 * Rematch UNMATCHED RawPunch rows against the personnel table (deviceUserId
 * or numeric personnelCode). Called after personnel are created/edited so
 * previously unmatched punches become usable — records are never deleted,
 * only enriched with the mapping + MATCHED status + legacy projection.
 */
export async function rematchRawPunches(_opts: { personnelId?: string } = {}): Promise<{ matched: number; punchRecordsCreated: number }> {
  // The mapping is recomputed from the full personnel index every time (cheap on
  // SQLite) — which unmatched rows exist matters more than which person changed.
  const candidates = await db.rawPunch.findMany({
    where: { matchStatus: 'UNMATCHED' },
    select: { id: true, enrollNumber: true },
  })

  const index = await buildPersonnelIndex()
  let matched = 0
  let punchRecordsCreated = 0
  for (const row of candidates) {
    const person = index.get(Number(row.enrollNumber))
    if (!person) continue
    await db.rawPunch.update({ where: { id: row.id }, data: { personnelId: person.id, matchStatus: 'MATCHED' } })
    matched++
    // best-effort legacy projection (P2002 = already projected before)
    try {
      const raw = await db.rawPunch.findUnique({ where: { id: row.id } })
      if (raw) {
        await db.punchRecord.create({
          data: {
            personId: person.id,
            timestamp: raw.punchTime,
            direction: 'AUTO',
            source: 'DEVICE',
            deviceId: raw.deviceId,
            deviceUserId: Number(raw.enrollNumber) || null,
            rawPayload: raw.rawPayload ?? undefined,
          },
        })
        punchRecordsCreated++
      }
    } catch (err) {
      if (!isP2002(err)) throw err
    }
  }
  return { matched, punchRecordsCreated }
}
