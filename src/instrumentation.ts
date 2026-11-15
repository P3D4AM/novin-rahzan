/**
 * Next.js server instrumentation — runs once per server process (dev & prod).
 *
 * 1. Starts the Novin Pardaz (NP-series) device push gateway on port 5005 so
 *    devices like NP756+ can connect and stream attendance logs in real time.
 * 2. Starts the device sync scheduler (v1.4.0 addendum P2): a server-side
 *    loop that re-reads device settings from the DB on every tick and pulls
 *    new punches from every enabled device with an interval configured —
 *    serialized per device, restart-safe, with failure hold-off. See
 *    src/lib/scheduler.ts.
 *
 * Set EBIO_DISABLED=1 to turn the gateway off, or EBIO_PORT=<n> to change the
 * listening port. Set AUTO_SYNC_DISABLED=1 to turn the scheduler off.
 */
export async function register() {
  if (process.env.NEXT_RUNTIME !== 'nodejs') return
  // never inside `next build` prerendering
  if (process.env.NEXT_PHASE === 'phase-production-build') return
  const g = globalThis as { __hozorplus_ebio_started?: boolean }
  if (g.__hozorplus_ebio_started) return
  g.__hozorplus_ebio_started = true

  try {
    const { startEbioGateway } = await import('./lib/ebio')
    startEbioGateway()
  } catch (err) {
    console.error('[ebio] failed to start device gateway:', err)
  }

  if (process.env.AUTO_SYNC_DISABLED !== '1') {
    try {
      const { startDeviceScheduler } = await import('./lib/scheduler')
      // singleton — a second call (hot reload) returns the existing handle
      startDeviceScheduler()
    } catch (err) {
      console.error('[scheduler] failed to start device sync scheduler:', err)
    }
  }
}
