/**
 * Next.js server instrumentation — runs once per server process (dev & prod).
 *
 * 1. Starts the Novin Pardaz (NP-series) device push gateway on port 5005 so
 *    devices like NP756+ can connect and stream attendance logs in real time.
 * 2. Starts the automatic device sync loop: every minute it pulls new logs
 *    from every enabled device with a sync interval configured — continuing
 *    incrementally from the last downloaded punch (Device.lastLogAt), never
 *    re-importing old records.
 *
 * Set EBIO_DISABLED=1 to turn the gateway off, or EBIO_PORT=<n> to change
 * the listening port. Set AUTO_SYNC_DISABLED=1 to turn the sync loop off.
 */
export async function register() {
  if (process.env.NEXT_RUNTIME !== 'nodejs') return
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
    const { autoSyncDueDevices } = await import('./lib/np-sync')
    const tick = async () => {
      try {
        const res = await autoSyncDueDevices()
        if (res.synced.length) console.log(`[auto-sync] ${res.synced.join('، ')} همگام شد`)
        for (const e of res.errors) console.error(`[auto-sync] ${e.name}: ${e.error}`)
      } catch (err) {
        console.error('[auto-sync] loop error:', err)
      }
    }
    // first check after 90s (let the app settle), then every 60s
    setTimeout(() => {
      tick()
      setInterval(tick, 60 * 1000)
    }, 90 * 1000)
    console.log('[auto-sync] automatic device sync loop started (checks every minute)')
  }
}
