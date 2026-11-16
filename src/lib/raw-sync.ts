/**
 * runRawSync — device sync orchestrator over the RawPunch integrity pipeline.
 *
 * A `RawPunchProvider` encapsulates ONE transport (SBXPC pull today; the
 * FKAttend x86 bridge in Phase 2; the reference CSV import later). The
 * orchestrator is transport-agnostic and enforces the Phase 1 rules
 * (corrected in v1.3.1 after the Phase 1 code review):
 *
 *   * sync history  — every run gets a DeviceSyncRun row (status, counts,
 *                    retries, sanitized error) BEFORE anything is imported.
 *   * retry         — provider.readAll() is wrapped in withRetry (read-only,
 *                    idempotent pull → blind retry is always safe).
 *   * serial guard  — if the source reports a device serial that differs from
 *                    the one on file, the run STOPS before storing anything
 *                    (P1: no foreign data entering this device's stream).
 *   * no loss (fix 4) — EVERY record the source reports is accounted for:
 *                    seen = inWindow + rejected + outOfWindow. Malformed
 *                    records (invalid/non-finite timestamps) are quarantined
 *                    (RejectedRecord, with their raw payload and a reason)
 *                    BEFORE the ingest transaction, so the trace survives an
 *                    ingest failure; window-excluded records are counted
 *                    separately. A run with NEW quarantined records is never
 *                    reported as a clean SUCCESS — it closes as PARTIAL.
 *   * bookmark      — Device.rawCursorAt advances only inside the ingest
 *                    transaction (raw-import.ts), i.e. after safe storage.
 *   * recalc (fix 2) — the derived attendance recalculation runs for EVERY
 *                    distinct affected (personId, day) pair, using that pair's
 *                    OWN day (never a global maxPunchTime). Failures are NOT
 *                    swallowed: they mark the run PARTIAL with an explicit
 *                    recoverable state, and retrySyncRunRecalcs() can repair
 *                    them later WITHOUT re-importing anything (the punches are
 *                    already safely stored in RawPunch).
 *   * secrets      — device passwords never appear in logs, sync-run rows or
 *                    realtime payloads (redactDeviceSecrets as last resort).
 *   * modes        — HARDWARE | MOCK | DRY_RUN are recorded per run so mock
 *                    tests and real hardware results can never be confused.
 */
import { db } from '@/lib/db'
import { errorMessage } from '@/lib/api'
import { redactDeviceSecrets } from '@/lib/device-secrets'
import { withRetry } from '@/lib/retry'
import { ingestRawPunches, quarantineRawPunches, RawPunchInput, RawSourceProtocol, QuarantineSpec } from '@/lib/raw-import'
import { recalcDayForPerson } from '@/lib/attendance'
import { tehranDayOf } from '@/lib/jalali'
import { emitRealtime } from '@/lib/notify'

export interface RawPunchRead {
  records: RawPunchInput[]
  /** serial as reported by the source right now (undefined = not reported) */
  deviceSerial?: string
  deviceTime?: string | null
  /** sanitized transport metadata (dialect, counts…) — never secrets */
  meta?: Record<string, unknown>
}

export interface RawPunchProvider {
  readonly protocol: RawSourceProtocol
  /** connect + read all records; must NOT interpret directions; throws on failure */
  readAll(): Promise<RawPunchRead>
}

/** how far back a first-ever sync goes when nothing else is known */
export const DEFAULT_FIRST_SYNC_DAYS = 30

export interface RawSyncOptions {
  deviceId: string
  provider: RawPunchProvider
  mode?: 'HARDWARE' | 'MOCK' | 'DRY_RUN'
  /** manual "download N days back" (null = bookmark / default window) */
  daysBack?: number | null
  /** import the full history the source reports (FKAttend-style full read) */
  full?: boolean
  /** extra attempts for the transport read (default 2) */
  retries?: number
  /** v1.4.2 (addendum P3): quiet mode — a no-op run (0 new, nothing quarantined,
   *  no recalc failures, no error) writes no DeviceLog row and its DeviceSyncRun
   *  row is removed (history stays meaningful without a 30-second row flood). */
  quiet?: boolean
  /** provenance note recorded on the DeviceSyncRun row (e.g. 'rt-poll') */
  note?: string
  /**
   * TEST SEAM ONLY (never set in production): replaces the per-(personId, day)
   * recalculation step so tests can simulate recalculation failures and verify
   * the PARTIAL state + recovery semantics of fix 2.
   */
  recalc?: (personId: string, day: Date) => Promise<unknown>
}

export interface RawSyncSummary {
  runId: string
  deviceSerial: string
  sourceProtocol: string
  mode: string
  status: string
  seen: number
  inWindow: number
  newCount: number
  duplicate: number
  matched: number
  unmatched: number
  punchRecordsCreated: number
  invalid: number
  /** malformed records newly quarantined this run (fix 4) */
  rejected: number
  /** malformed records already quarantined by an earlier run (dedupe hits) */
  rejectedDuplicate: number
  /** valid records intentionally excluded by the requested date window (fix 4) */
  outOfWindow: number
  /** (personId, day) pairs whose derived-day recalculation failed (fix 2) */
  recalcFailed: number
  cutoff: string
  newBookmark: string | null
  deviceTime?: string | null
  retries: number
}

export class SerialMismatchError extends Error {
  constructor(reported: string, expected: string) {
    super(
      `سریال گزارش‌شدهٔ دستگاه (${reported}) با سریال ثبت‌شده در پرونده دستگاه (${expected}) یکی نیست — ` +
      'برای جلوگیری از ورود داده‌های ناهمگان، دریافت متوقف شد و هیچ رکوردی ذخیره نشد.',
    )
    this.name = 'SerialMismatchError'
  }
}

/** Run one full sync: history → retry-read → serial guard → classify → quarantine → ingest → per-day recalc → log. */
export async function runRawSync(opts: RawSyncOptions): Promise<RawSyncSummary> {
  const device = await db.device.findUnique({ where: { id: opts.deviceId } })
  if (!device) throw new Error('دستگاه یافت نشد')

  const mode = opts.mode ?? 'HARDWARE'
  const serialOnFile = device.serialNumber?.trim() ?? ''

  // ---- 1) sync history row: RUNNING -------------------------------------
  const run = await db.deviceSyncRun.create({
    data: {
      deviceId: device.id,
      deviceSerial: serialOnFile,
      sourceProtocol: String(opts.provider.protocol),
      mode,
      status: 'RUNNING',
      ...(opts.note ? { note: opts.note } : {}),
    },
  })

  const finishRun = async (status: string, data: Record<string, unknown>) => {
    await db.deviceSyncRun
      .update({ where: { id: run.id }, data: { status, finishedAt: new Date(), ...data } })
      .catch(() => {})
  }

  // accounting known so far (kept outside the try so the FAILED path can
  // record whatever classification managed to complete — fix 4: no record
  // may disappear without a trace, even on failure)
  let seenCount = 0
  let inWindowCount = 0
  let rejectedCount = 0
  let outOfWindowCount = 0

  try {
    // ---- 2) transport read with retry -----------------------------------
    const { value: read, retries } = await withRetry(() => opts.provider.readAll(), {
      retries: opts.retries ?? 2,
      onRetry: (_err, attempt) => {
        db.deviceSyncRun
          .update({ where: { id: run.id }, data: { retries: attempt } })
          .catch(() => {})
      },
    })
    if (retries > 0) {
      await db.deviceSyncRun.update({ where: { id: run.id }, data: { retries } }).catch(() => {})
    }

    // ---- 3) serial guard (P1: stop before storing foreign data) ----------
    const reportedSerial = read.deviceSerial?.trim() ?? ''
    if (
      reportedSerial &&
      serialOnFile &&
      reportedSerial.toLowerCase() !== serialOnFile.toLowerCase()
    ) {
      await finishRun('FAILED', { error: redactDeviceSecrets(new SerialMismatchError(reportedSerial, serialOnFile).message) })
      await db.deviceLog
        .create({
          data: {
            deviceId: device.id,
            level: 'ERROR',
            message: redactDeviceSecrets(
              `توقف دریافت: سریال دستگاه گزارش‌شده (${reportedSerial}) با سریال پرونده (${serialOnFile}) متفاوت است — هیچ رکوردی ذخیره نشد (شناسه اجرا: ${run.id.slice(0, 8)})`,
            ),
          },
        })
        .catch(() => {})
      throw new SerialMismatchError(reportedSerial, serialOnFile)
    }
    const effectiveSerial = (reportedSerial || serialOnFile || 'UNKNOWN').trim()

    // ---- 4) classify EVERY record the source reported (fix 4) -----------
    // Nothing is dropped silently: malformed records become quarantine specs,
    // window-excluded records are counted, the rest go to the ingest window.
    const now = Date.now()
    let cutoffMs: number
    if (opts.full) {
      cutoffMs = 0 // full history — dedupe (fingerprints) makes re-reads harmless
    } else if (typeof opts.daysBack === 'number' && opts.daysBack > 0) {
      cutoffMs = now - opts.daysBack * 24 * 3600 * 1000
    } else if (device.rawCursorAt) {
      cutoffMs = device.rawCursorAt.getTime() - 60 * 1000 // 1-min overlap guard
    } else if (device.lastLogAt) {
      cutoffMs = device.lastLogAt.getTime() - 60 * 1000 // legacy bookmark (pre-v1.3.0 syncs)
    } else {
      cutoffMs = now - DEFAULT_FIRST_SYNC_DAYS * 24 * 3600 * 1000
    }
    const futureGuardMs = now + 24 * 3600 * 1000

    const inWindow: RawPunchInput[] = []
    const rejectedSpecs: QuarantineSpec[] = []
    let outOfWindow = 0
    for (const r of read.records) {
      const ms = r.punchTime?.getTime?.()
      if (typeof ms !== 'number' || !Number.isFinite(ms)) {
        rejectedSpecs.push({
          record: r,
          reason: 'INVALID_TIMESTAMP',
          detail: `punchTime is not a finite instant (got: ${describeTimestamp(r.punchTime)})`,
        })
        continue
      }
      if (ms < cutoffMs || ms > futureGuardMs) {
        outOfWindow++ // intentional window exclusion — counted, NOT quarantined
        continue
      }
      inWindow.push(r)
    }
    seenCount = read.records.length
    inWindowCount = inWindow.length
    outOfWindowCount = outOfWindow

    // ---- 5) quarantine malformed records BEFORE the ingest transaction --
    // (their trace then survives an ingest failure; idempotent by fingerprint
    // so 30-second rt-polls re-reporting the same bad record create no flood)
    const quarantine = await quarantineRawPunches(rejectedSpecs, {
      deviceId: device.id,
      deviceSerial: effectiveSerial,
      sourceProtocol: opts.provider.protocol,
      syncRunId: run.id,
    })
    rejectedCount = quarantine.quarantined

    // ---- 6) atomic ingest (bookmark advances only after safe storage) ----
    const ingest = await ingestRawPunches(inWindow, {
      deviceId: device.id,
      deviceSerial: effectiveSerial,
      sourceProtocol: opts.provider.protocol,
      syncRunId: run.id,
    })

    // ---- 7) derived attendance recalc — per distinct (personId, day) ----
    // (fix 2: every affected pair is recalculated with its OWN day; errors
    // are collected and surfaced, never swallowed)
    const recalc = opts.recalc ?? ((personId: string, day: Date) => recalcDayForPerson(personId, day))
    const pairs = new Map<string, { personId: string; day: Date }>()
    for (const t of ingest.touchedDays) {
      pairs.set(`${t.personId}|${t.day.toISOString()}`, t)
    }
    const recalcFailures: Array<{ personId: string; day: string; error: string }> = []
    for (const pair of pairs.values()) {
      try {
        await recalc(pair.personId, pair.day)
      } catch (err) {
        recalcFailures.push({ personId: pair.personId, day: pair.day.toISOString(), error: errorMessage(err) })
      }
    }

    // ---- 8) close the run + device + log (sanitised) --------------------
    const newBookmark = ingest.maxPunchTime
    const problems: string[] = []
    if (rejectedCount > 0) {
      problems.push(`${rejectedCount.toLocaleString('fa-IR')} رکورد نامعتبر (مهر زمانی خراب) قرنطینه شد — رکوردها نگه‌داشته شده‌اند و قابل بررسی‌اند`)
    }
    if (recalcFailures.length > 0) {
      problems.push(`بازمحاسبهٔ حضور برای ${recalcFailures.length.toLocaleString('fa-IR')} روز/شخصیت ناموفق بود — وضعیت اجرا «ناقص» ثبت شد؛ ترددها ذخیره شده‌اند و بازمحاسبه بعداً بدون دریافت مجدد قابل تکرار است`)
    }
    const status = problems.length > 0 ? 'PARTIAL' : 'SUCCESS'
    const summary: RawSyncSummary = {
      runId: run.id,
      deviceSerial: effectiveSerial,
      sourceProtocol: String(opts.provider.protocol),
      mode,
      status,
      seen: read.records.length,
      inWindow: inWindow.length,
      newCount: ingest.newCount,
      duplicate: ingest.duplicate,
      matched: ingest.matched,
      unmatched: ingest.unmatched,
      punchRecordsCreated: ingest.punchRecordsCreated,
      invalid: ingest.invalid,
      rejected: rejectedCount,
      rejectedDuplicate: quarantine.duplicate,
      outOfWindow,
      recalcFailed: recalcFailures.length,
      cutoff: new Date(cutoffMs).toISOString(),
      newBookmark: newBookmark ? newBookmark.toISOString() : null,
      deviceTime: read.deviceTime ?? null,
      retries,
    }

    const noopQuiet =
      Boolean(opts.quiet) &&
      summary.newCount === 0 &&
      ingest.invalid === 0 &&
      rejectedCount === 0 &&
      recalcFailures.length === 0 &&
      retries === 0
    // NOTE: outOfWindow does NOT block quiet mode — window-excluded records
    // are intentional, expected exclusions (bookmark cutoff / future guard),
    // not events worth recording.
    if (noopQuiet) {
      // a quiet no-op run records nothing (0 new, nothing quarantined, no
      // recalc failures, no error) — the run row created upfront is removed;
      // punches are what matter and a no-op run has none attached (nothing
      // references its id)
      await db.deviceSyncRun.delete({ where: { id: run.id } }).catch(() => {})
    } else {
      await finishRun(status, {
        recordsSeen: summary.seen,
        recordsInWindow: summary.inWindow,
        recordsNew: summary.newCount,
        recordsDuplicate: summary.duplicate,
        recordsMatched: summary.matched,
        recordsUnmatched: summary.unmatched,
        recordsRejected: rejectedCount,
        recordsOutOfWindow: outOfWindow,
        bookmarkAt: newBookmark,
        error: problems.length > 0 ? redactDeviceSecrets(problems.join('؛ ')) : null,
      })
    }

    await db.device
      .update({
        where: { id: device.id },
        data: {
          status: 'ONLINE',
          lastSyncAt: new Date(),
          lastSyncCount: summary.newCount,
          // v1.4.0 (addendum P2): the sanitized last error clears on success
          lastSyncError: null,
          ...(effectiveSerial !== 'UNKNOWN' && !serialOnFile ? { serialNumber: effectiveSerial } : {}),
          ...(newBookmark ? { rawCursorAt: newBookmark, lastLogAt: newBookmark } : {}),
        },
      })
      .catch(() => {})

    const modeFa = mode === 'MOCK' ? ' [آزمایشی — Mock]' : mode === 'DRY_RUN' ? ' [بدون ذخیره‌سازی]' : ''
    const quietNoNews = noopQuiet
    if (!quietNoNews) {
      // quiet no-op polls write no SUCCESS/PARTIAL log — a 30s polling loop
      // would otherwise flood the device-log list shown in the UI
      await db.deviceLog
        .create({
          data: {
            deviceId: device.id,
            level: problems.length > 0 ? 'WARN' : 'SUCCESS',
            message: redactDeviceSecrets(
              `دریافت از دستگاه انجام شد${modeFa}${opts.note === 'rt-poll' ? ' [شبه‌بی‌درنگ]' : ''}: ${summary.seen} تردد در حافظه دستگاه، ${summary.inWindow} در بازه، ` +
              `${summary.newCount} رکورد جدید، ${summary.duplicate} تکراری (دینوپه)، ${summary.unmatched} بدون تطبیق (نگه‌داشته شد)` +
              `${rejectedCount > 0 ? `، ${rejectedCount} نامعتبر (قرنطینه شد)` : ''}` +
              `${outOfWindow > 0 ? `، ${outOfWindow} خارج از بازهٔ درخواستی` : ''}` +
              `${recalcFailures.length > 0 ? `، بازمحاسبهٔ ${recalcFailures.length} روز ناموفق` : ''}` +
              `${effectiveSerial ? ` — سریال: ${effectiveSerial}` : ''}` +
              ` (اجرای ${run.id.slice(0, 8)})`,
            ),
          },
        })
        .catch(() => {})
    }

    if (summary.newCount > 0) {
      // v1.4.3 (addendum P4): per-user + admin notifications for the NEW rows
      // (selected by syncRunId — a no-op re-sync can never re-notify).
      // Best effort: a notification failure must never fail the sync itself.
      const { createPunchNotifications } = await import('@/lib/notify')
      await createPunchNotifications({ runId: run.id, deviceName: device.name }).catch(() => {})

      await emitRealtime('device-sync', {
        deviceId: device.id,
        created: summary.newCount,
        skipped: summary.duplicate,
        unmatched: summary.unmatched,
        matched: summary.matched,
        rejected: rejectedCount,
        outOfWindow,
        mode,
      }).catch(() => {})
    }
    return summary
  } catch (err) {
    // ---- failure: mark the run FAILED, offline status, sanitized log ------
    const sanitizedError = redactDeviceSecrets(errorMessage(err))
    await finishRun('FAILED', {
      error: sanitizedError,
      ...(seenCount ? { recordsSeen: seenCount, recordsInWindow: inWindowCount, recordsRejected: rejectedCount, recordsOutOfWindow: outOfWindowCount } : {}),
    })
    await db.device
      .update({
        where: { id: device.id },
        data: {
          status: 'OFFLINE',
          // v1.4.0 (addendum P2): sanitized last error for the settings UI
          lastSyncError: sanitizedError.slice(0, 500),
        },
      })
      .catch(() => {})
    await db.deviceLog
      .create({
        data: { deviceId: device.id, level: 'ERROR', message: redactDeviceSecrets(`خطای دریافت از دستگاه: ${errorMessage(err)} (اجرای ${run.id.slice(0, 8)})`) },
      })
      .catch(() => {})
    throw err
  }
}

function describeTimestamp(t: unknown): string {
  if (t instanceof Date) return `Date(${String(t)})`
  return typeof t === 'object' ? JSON.stringify(String(t)) : String(t)
}

/**
 * Retry the derived-attendance recalculation for one sync run (fix 2).
 *
 * The punches of the run are already safely stored in RawPunch — a recalc
 * retry re-derives the affected (personId, day) pairs from the STORED rows and
 * recalculates them; it never talks to the device and never re-imports.
 * If every pair recalculates and the run was PARTIAL, the run row is closed
 * out (SUCCESS when nothing else was wrong with it).
 */
export async function retrySyncRunRecalcs(runId: string): Promise<{
  runId: string
  pairs: number
  failed: number
  errors: string[]
}> {
  const run = await db.deviceSyncRun.findUnique({ where: { id: runId } })
  if (!run) throw new Error('اجرای همگام‌سازی یافت نشد')

  const rows = await db.rawPunch.findMany({
    where: { syncRunId: runId, personnelId: { not: null } },
    select: { personnelId: true, punchTime: true },
  })
  const pairs = new Map<string, { personId: string; day: Date }>()
  for (const r of rows) {
    const day = tehranDayOf(r.punchTime)
    pairs.set(`${r.personnelId}|${day.toISOString()}`, { personId: r.personnelId as string, day })
  }
  let failed = 0
  const errors: string[] = []
  for (const p of pairs.values()) {
    try {
      await recalcDayForPerson(p.personId, p.day)
    } catch (err) {
      failed++
      errors.push(`${p.personId}@${p.day.toISOString().slice(0, 10)}: ${errorMessage(err)}`)
    }
  }
  if (failed === 0 && run.status === 'PARTIAL' && run.recordsRejected === 0) {
    await db.deviceSyncRun
      .update({ where: { id: runId }, data: { status: 'SUCCESS', error: null } })
      .catch(() => {})
  }
  return { runId, pairs: pairs.size, failed, errors }
}
