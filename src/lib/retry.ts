/**
 * withRetry — small, dependency-free retry helper for read-only/idempotent
 * device operations (pull syncs, bridge calls). Phase 1 (Data Integrity).
 *
 * Semantics:
 *   * `retries` = EXTRA attempts after the first failure (2 → up to 3 runs total).
 *   * Exponential backoff with jitter: delay = min(maxDelay, base * 2^(attempt-1)) + jitter
 *   * By default every error is retried — reading attendance logs from a device is
 *     read-only and idempotent, so a blind retry can never corrupt data. Non-retryable
 *     cases (e.g. validation errors) can be excluded via `retryOn`.
 *   * An optional `onRetry` hook records the retry count on the sync-run row
 *     (DeviceSyncRun.retries) so the sync history shows how many retries were used.
 */

export interface RetryOptions {
  /** extra attempts after the first failure (default 2) */
  retries?: number
  /** first backoff delay in ms (default 500) */
  baseDelayMs?: number
  /** backoff cap in ms (default 5000) */
  maxDelayMs?: number
  /** decide whether a thrown error is retryable (default: everything) */
  retryOn?: (err: unknown) => boolean
  /** called before each retry sleep: (err, attemptNumber, delayMs) */
  onRetry?: (err: unknown, attempt: number, delayMs: number) => void
  /** optional abort signal — checked between attempts */
  signal?: AbortSignal
}

export interface RetryOutcome<T> {
  value: T
  /** how many extra attempts were used (0 = first try succeeded) */
  retries: number
}

function sleep(ms: number, signal?: AbortSignal): Promise<void> {
  return new Promise((resolve, reject) => {
    const t = setTimeout(() => {
      signal?.removeEventListener('abort', onAbort)
      resolve()
    }, ms)
    function onAbort() {
      clearTimeout(t)
      reject(new Error('retry aborted'))
    }
    signal?.addEventListener('abort', onAbort, { once: true })
  })
}

/** Run `fn` with retries and exponential backoff. Never retries a success. */
export async function withRetry<T>(fn: () => Promise<T>, opts: RetryOptions = {}): Promise<RetryOutcome<T>> {
  const retries = Math.max(0, opts.retries ?? 2)
  const base = Math.max(0, opts.baseDelayMs ?? 500)
  const max = Math.max(base, opts.maxDelayMs ?? 5000)
  const retryOn = opts.retryOn ?? (() => true)

  let attempt = 0 // failed attempts so far
  for (;;) {
    if (opts.signal?.aborted) throw new Error('retry aborted')
    try {
      const value = await fn()
      return { value, retries: attempt }
    } catch (err) {
      attempt++
      if (attempt > retries || !retryOn(err)) throw err
      const delay = Math.min(max, base * 2 ** (attempt - 1)) + Math.floor(Math.random() * 250)
      opts.onRetry?.(err, attempt, delay)
      await sleep(delay, opts.signal)
    }
  }
}
