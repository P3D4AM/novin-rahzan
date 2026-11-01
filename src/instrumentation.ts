/**
 * Next.js server instrumentation — runs once per server process (dev & prod).
 * Starts the Novin Pardaz (NP-series) device push gateway on port 5005 so
 * devices like NP756+ can connect and stream attendance logs in real time.
 *
 * Set EBIO_DISABLED=1 to turn the gateway off, or EBIO_PORT=<n> to change
 * the listening port.
 */
export async function register() {
  if (process.env.NEXT_RUNTIME !== 'nodejs') return
  if (process.env.EBIO_DISABLED === '1') return
  const g = globalThis as { __hozorplus_ebio_started?: boolean }
  if (g.__hozorplus_ebio_started) return
  g.__hozorplus_ebio_started = true
  try {
    const { startEbioGateway } = await import('./lib/ebio')
    startEbioGateway()
  } catch (err) {
    console.error('[ebio] failed to start device gateway:', err)
  }
}
