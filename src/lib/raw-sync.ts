/**
 * runRawSync — device sync orchestrator over the RawPunch integrity pipeline.
 *
 * A `RawPunchProvider` encapsulates ONE transport (SBXPC pull today; the
 * FKAttend x86 bridge in Phase 2; the reference CSV import later). The
 * orchestrator is transport-agnostic and enforces the Phase 1 rules:
 *
 *   * sync history  — every run gets a DeviceSyncRun row (status, counts,
 *                    retries, sanitized error) BEFORE anything is imported.
 *   * retry         — provider.readAll() is wrapped in withRetry (read-only,
 *                    idempotent pull → blind retry is always safe).
 *   * serial guard  — if the source reports a device serial that differs from
 *                    the one on file, the run STOPS before storing anything
 *                    (P1: no foreign data entering this device's stream).
 *   * bookmark      — Device.rawCursorAt advances only inside the ingest
 *                    transaction (raw-import.ts), i.e. after safe storage.
 *   * secrets      — device passwords never appear in logs, sync-run rows or
 *                    realtime payloads (redactDeviceSecrets as last resort).
 *   * modes         — HARDWARE | MOCK | DRY_RUN are recorded per run so mock
 *                    tests and real hardware results can never be confused.
 */
import { db } from '@/lib/db'
import { errorMessage } from '@/lib/api'
import { redactDeviceSecrets } from '@/lib/device-secrets'
import { withRetry } from '@/lib/retry'
import { ingestRawPunches, RawPunchInput, RawSourceProtocol } from '@/lib/raw-import'
import { recalcDayForPerson } from '@/lib/attendance'
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
  /** v1.4.2 (addendum P3): quiet mode — a no-op run (0 new, no error) writes
   *  no DeviceLog row and its DeviceSyncRun row is removed (history stays
   *  meaningful without a 30-second row flood from rt-polls). */
  quiet?: boolean
  /** provenance note recorded on the DeviceSyncRun row (e.g. 'rt-poll') */
  note?: string
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

/** Run one full sync: history → retry-read → serial guard → ingest → recalc → log. */
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

    // ---- 4) window (daysBack | full | bookmark/default) ------------------
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
    const inWindow = read.records.filter(
      (r) => {
        const ms = r.punchTime?.getTime?.()
        return typeof ms === 'number' && Number.isFinite(ms) && ms >= cutoffMs && ms <= futureGuardMs
      },
    )

    // ---- 5) atomic ingest (bookmark advances only after safe storage) ----
    const ingest = await ingestRawPunches(inWindow, {
      deviceId: device.id,
      deviceSerial: effectiveSerial,
      sourceProtocol: opts.provider.protocol,
      syncRunId: run.id,
    })

    // ---- 6) derived attendance recalc (best effort, outside the tx) ------
    const byDay = new Map<string, Set<string>>()
    for (const t of ingest.touchedDays) {
      const key = t.day.toISOString().slice(0, 10)
      if (!byDay.has(key)) byDay.set(key, new Set())
      byDay.get(key)!.add(t.personId)
    }
    for (const [, personIds] of byDay) {
      for (const personId of personIds) {
        await recalcDayForPerson(personId, ingest.maxPunchTime ?? new Date()).catch(() => {})
      }
    }

    // ---- 7) close the run + device + log (sanitised) --------------------
    const newBookmark = ingest.maxPunchTime
    const summary: RawSyncSummary = {
      runId: run.id,
      deviceSerial: effectiveSerial,
      sourceProtocol: String(opts.provider.protocol),
      mode,
      status: 'SUCCESS',
      seen: read.records.length,
      inWindow: inWindow.length,
      newCount: ingest.newCount,
      duplicate: ingest.duplicate,
      matched: ingest.matched,
      unmatched: ingest.unmatched,
      punchRecordsCreated: ingest.punchRecordsCreated,
      invalid: ingest.invalid,
      cutoff: new Date(cutoffMs).toISOString(),
      newBookmark: newBookmark ? newBookmark.toISOString() : null,
      deviceTime: read.deviceTime ?? null,
      retries,
    }

    const noopQuiet = Boolean(opts.quiet) && summary.newCount === 0 && ingest.invalid === 0 && retries === 0
    if (noopQuiet) {
      // a quiet no-op run records nothing (0 new, no error, no retries) —
      // the run row created upfront is removed; punches are what matter and
      // a no-op run has none attached (nothing references its id)
      await db.deviceSyncRun.delete({ where: { id: run.id } }).catch(() => {})
    } else {
      await finishRun('SUCCESS', {
        recordsSeen: summary.seen,
        recordsInWindow: summary.inWindow,
        recordsNew: summary.newCount,
        recordsDuplicate: summary.duplicate,
        recordsMatched: summary.matched,
        recordsUnmatched: summary.unmatched,
        bookmarkAt: newBookmark,
        error: ingest.invalid > 0 ? `${ingest.invalid} رکورد نامعتبر نادیده گرفته شد` : null,
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
    const quietNoNews = Boolean(opts.quiet) && summary.newCount === 0 && ingest.invalid === 0 && retries === 0
    if (!quietNoNews) {
      // quiet no-op polls write no SUCCESS log — a 30s polling loop would
      // otherwise flood the device-log list shown in the UI
      await db.deviceLog
        .create({
          data: {
            deviceId: device.id,
            level: 'SUCCESS',
            message: redactDeviceSecrets(
              `دریافت از دستگاه انجام شد${modeFa}${opts.note === 'rt-poll' ? ' [شبه‌بی‌درنگ]' : ''}: ${summary.seen} تردد در حافظه دستگاه، ${summary.inWindow} در بازه، ` +
              `${summary.newCount} رکورد جدید، ${summary.duplicate} تکراری (دиноپه)، ${summary.unmatched} بدون تطبیق (نگه‌داشته شد)` +
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
        mode,
      }).catch(() => {})
    }
    return summary
  } catch (err) {
    // ---- failure: mark the run FAILED, offline status, sanitized log ------
    const sanitizedError = redactDeviceSecrets(errorMessage(err))
    await finishRun('FAILED', { error: sanitizedError })
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
