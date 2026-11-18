/**
 * Phase 2 Stage S2 — direction-aware punch pairing & validation layer.
 * =====================================================================
 * Pure module: NO database access, NO clock reads, NO randomness.
 * Deterministic: the output depends only on the multiset of input punches
 * (timestamp, direction, source) — identical input always produces an
 * identical result, regardless of input array order (the input is sorted
 * into a canonical total order first).
 *
 * Contract (Phase 2 stage S2 mandate):
 *   - `direction` is REALLY consulted (IN/OUT); legacy AUTO/UNKNOWN punch
 *     directions (device projection, legacy manual rows) are treated as
 *     direction-unknown ("WILD") and fall back to positional pairing
 *     (opposite of the current session state), which reproduces the old
 *     engine's arithmetic for pure-AUTO days bit-for-bit.
 *   - Duplicate IN, duplicate OUT, OUT without IN, IN without OUT and
 *     simultaneous punches are IDENTIFIED and REPORTED with an explicit
 *     machine reason code (+ Persian message) — never silently absorbed.
 *   - No raw punch is ever deleted or mutated: this function only READS its
 *     input and returns a report. Persisted PunchRecord rows are never
 *     touched by this layer (the engine writes only DailyAttendance).
 *   - A valid pair is only ever (an IN that opened a session, the OUT that
 *     closed that same session). Incomplete/ambiguous punches (orphans,
 *     duplicates, open sessions) are NEVER counted into worked minutes and
 *     never fabricated into pairs.
 *
 * Reason codes (stable API — persisted in DailyAttendance.note):
 *   DUPLICATE_IN     — IN arrives while a session is already open
 *   OUT_WITHOUT_IN   — first unmatched OUT of the day (no IN ever opened)
 *   DUPLICATE_OUT    — a later unmatched OUT (an OUT already seen before it)
 *   IN_WITHOUT_OUT   — session still open at end of day (missing OUT)
 *   SIMULTANEOUS     — two or more punches at the exact same instant
 *
 * Tie-break at identical timestamps (deterministic total order):
 *   timestamp asc → OUT before WILD before IN → source string asc →
 *   original array index asc. Rationale: at the same instant an OUT closes
 *   the already-open session before anything else can open a new one
 *   (models "left and immediately returned"); IN is processed last so a
 *   same-instant IN+WILD does not fabricate a zero-minute pair.
 */

/** Punch direction as understood by the pairing state machine. */
export type PairingDirection = 'IN' | 'OUT' | 'WILD' // WILD = unknown (AUTO/UNKNOWN/missing)

/** Input shape — a superset-compatible projection of PunchRecord. */
export interface PairingPunchInput {
  timestamp: Date
  direction?: string | null
  source?: string | null
}

export type PairingReason =
  | 'DUPLICATE_IN'
  | 'OUT_WITHOUT_IN'
  | 'DUPLICATE_OUT'
  | 'IN_WITHOUT_OUT'
  | 'SIMULTANEOUS'

export interface PairingAnomaly {
  reason: PairingReason
  /** Persian human-readable explanation (for future UI surfaces). */
  message: string
  /** Indexes into the ORIGINAL input array (stable identification). */
  punchIndexes: number[]
  /** The instant the anomaly is anchored to, when applicable. */
  at: Date | null
}

/** A closed, direction-valid work session. Nothing else counts as worked. */
export interface ValidPair {
  inIndex: number
  outIndex: number
  inAt: Date
  outAt: Date
  minutes: number
}

export interface PairingResult {
  pairs: ValidPair[]
  /** IN of the first valid pair; else the punch of a still-open session; else null. */
  firstIn: Date | null
  /** OUT of the last valid pair (an open session or orphan never counts); null if no pair. */
  lastOut: Date | null
  /** Sum of ValidPair.minutes ONLY — anomalies never contribute. */
  workedMinutes: number
  anomalies: PairingAnomaly[]
  /** Original-input indexes of every punch that is not part of a valid pair. */
  unpairedIndexes: number[]
  /** Index of the punch whose session never closed (IN_WITHOUT_OUT), else null. */
  openPairIndex: number | null
}

const DIRECTION_RANK: Record<PairingDirection, number> = { OUT: 0, WILD: 1, IN: 2 }

function normalizeDirection(d: string | null | undefined): PairingDirection {
  if (d === 'IN') return 'IN'
  if (d === 'OUT') return 'OUT'
  return 'WILD' // AUTO, UNKNOWN, missing, legacy value — direction unknown
}

const REASON_FA: Record<PairingReason, string> = {
  DUPLICATE_IN: 'ورود تکراری: ورودی ثبت شده در حالی که نشست ورود قبلی هنوز خروج ندارد',
  OUT_WITHOUT_IN: 'خروج بدون ورود: خروج ثبت شده اما هیچ ورودی پیش از آن نشست باز نکرده است',
  DUPLICATE_OUT: 'خروج تکراری: خروج جدید بدون ورود مجدد در میان دو خروج',
  IN_WITHOUT_OUT: 'ورود بدون خروج: نشست ورود در پایان بازه همچنان باز مانده است',
  SIMULTANEOUS: 'ترددهای هم‌زمان: بیش از یک تردد در همان لحظه ثبت شده است',
}

/** Canonical order for the note summary (deterministic output). */
const REASON_ORDER: PairingReason[] = [
  'DUPLICATE_IN',
  'OUT_WITHOUT_IN',
  'DUPLICATE_OUT',
  'IN_WITHOUT_OUT',
  'SIMULTANEOUS',
]

/**
 * Deterministic, human-greppable ASCII summary for DailyAttendance.note,
 * e.g. `DUPLICATE_IN:1|IN_WITHOUT_OUT:1` (empty string when no anomalies).
 */
export function anomalySummary(anomalies: PairingAnomaly[]): string {
  if (!anomalies.length) return ''
  const counts = new Map<PairingReason, number>()
  for (const a of anomalies) counts.set(a.reason, (counts.get(a.reason) ?? 0) + 1)
  const parts: string[] = []
  for (const reason of REASON_ORDER) {
    const c = counts.get(reason)
    if (c) parts.push(`${reason}:${c}`)
  }
  return parts.join('|')
}

/**
 * Direction-aware deterministic punch pairing.
 * Pure: never mutates its input (verified by test-attendance section 13).
 */
export function pairPunches(input: PairingPunchInput[]): PairingResult {
  const norm = input.map((p, index) => ({
    index,
    ts: p.timestamp.getTime(),
    at: p.timestamp,
    dir: normalizeDirection(p.direction),
    source: (p.source ?? '').toString(),
  }))

  // Canonical total order — see module header for the tie-break rationale.
  const sorted = [...norm].sort((a, b) => {
    if (a.ts !== b.ts) return a.ts - b.ts
    const ra = DIRECTION_RANK[a.dir]
    const rb = DIRECTION_RANK[b.dir]
    if (ra !== rb) return ra - rb
    if (a.source !== b.source) return a.source < b.source ? -1 : 1
    return a.index - b.index
  })

  const anomalies: PairingAnomaly[] = []
  const unpaired = new Set<number>()

  // Simultaneity pre-scan: two or more punches at the exact same instant.
  for (let i = 0; i < sorted.length; ) {
    let j = i
    while (j < sorted.length && sorted[j].ts === sorted[i].ts) j++
    if (j - i > 1) {
      anomalies.push({
        reason: 'SIMULTANEOUS',
        message: REASON_FA.SIMULTANEOUS,
        punchIndexes: sorted.slice(i, j).map((x) => x.index),
        at: sorted[i].at,
      })
    }
    i = j
  }

  // Direction-aware state machine over the sorted punches.
  const pairs: ValidPair[] = []
  let openIndex: number | null = null
  let openTs = 0
  let openAt: Date | null = null
  let sawOut = false // any OUT already processed today (closed a pair or orphan)

  for (const p of sorted) {
    if (p.dir === 'OUT') {
      if (openIndex !== null) {
        pairs.push({
          inIndex: openIndex,
          outIndex: p.index,
          inAt: openAt as Date,
          outAt: p.at,
          minutes: Math.round((p.ts - openTs) / 60000),
        })
        openIndex = null
        openAt = null
      } else if (!sawOut) {
        // First unmatched OUT of the day — nothing was ever opened before it.
        anomalies.push({
          reason: 'OUT_WITHOUT_IN',
          message: REASON_FA.OUT_WITHOUT_IN,
          punchIndexes: [p.index],
          at: p.at,
        })
        unpaired.add(p.index)
      } else {
        // A previous OUT (valid or orphan) already exists without a re-entry.
        anomalies.push({
          reason: 'DUPLICATE_OUT',
          message: REASON_FA.DUPLICATE_OUT,
          punchIndexes: [p.index],
          at: p.at,
        })
        unpaired.add(p.index)
      }
      sawOut = true
    } else if (p.dir === 'IN') {
      if (openIndex === null) {
        openIndex = p.index
        openTs = p.ts
        openAt = p.at
      } else {
        anomalies.push({
          reason: 'DUPLICATE_IN',
          message: REASON_FA.DUPLICATE_IN,
          punchIndexes: [p.index],
          at: p.at,
        })
        unpaired.add(p.index)
      }
    } else {
      // WILD — direction unknown (AUTO/UNKNOWN/legacy): positional fallback,
      // i.e. act as the opposite of the current state. Reproduces the old
      // engine's arithmetic for pure-AUTO (device) days bit-for-bit.
      if (openIndex === null) {
        openIndex = p.index
        openTs = p.ts
        openAt = p.at
      } else {
        pairs.push({
          inIndex: openIndex,
          outIndex: p.index,
          inAt: openAt as Date,
          outAt: p.at,
          minutes: Math.round((p.ts - openTs) / 60000),
        })
        openIndex = null
        openAt = null
      }
    }
  }

  // A session that never closed: reported, its minutes NOT counted as worked.
  if (openIndex !== null) {
    anomalies.push({
      reason: 'IN_WITHOUT_OUT',
      message: REASON_FA.IN_WITHOUT_OUT,
      punchIndexes: [openIndex],
      at: openAt,
    })
    unpaired.add(openIndex)
  }

  const workedMinutes = pairs.reduce((acc, pair) => acc + pair.minutes, 0)
  const firstIn = pairs.length > 0 ? pairs[0].inAt : openAt
  const lastOut = pairs.length > 0 ? pairs[pairs.length - 1].outAt : null

  return {
    pairs,
    firstIn,
    lastOut,
    workedMinutes,
    anomalies,
    unpairedIndexes: [...unpaired].sort((a, b) => a - b),
    openPairIndex: openIndex,
  }
}
