/**
 * v1.3.1 (Phase 1 review fix 7) — cross-process concurrent-import test worker.
 *
 * The test suite spawns TWO of these processes against the SAME temporary
 * database at the SAME time, each importing the SAME batch, to prove that the
 * database uniqueness constraint (RawPunch deviceSerial+protocol+enroll+
 * fingerprint+dupIndex) remains the FINAL duplicate safeguard when two server
 * processes overlap on one device — exactly the scenario the DB-backed
 * DeviceSyncLock serializes and this test backstops.
 *
 * SQLite serializes writers; a transient "database is locked" is retried with
 * jitter (that is normal contention handling, NOT a correctness workaround —
 * correctness comes from the unique key). Prints one WORKER_RESULT JSON line.
 *
 * Environment:
 *   DATABASE_URL       — file: URL of the shared temp database (validated against TMP root)
 *   WORKER_TMP_ROOT    — the test-owned root directory for path validation
 *   WORKER_DEVICE_ID   — optional Device id to attach
 *   WORKER_SERIAL      — device serial (default CONC-SERIAL)
 */
import { db } from '../src/lib/db'
import { ingestRawPunches } from '../src/lib/raw-import'
import { assertTestOwnedDatabasePath } from './test-db-guard'

const T0 = Date.UTC(2026, 10, 10, 6, 30, 0) // 2026-11-10 06:30 UTC — fixed, deterministic batch

function buildBatch() {
  const rec = (enroll: number | string, msAgo: number) => ({
    enrollNumber: enroll,
    punchTime: new Date(T0 - msAgo),
    inOutModeRaw: 1,
    verifyModeRaw: 1,
    workCode: '',
    rawPayload: JSON.stringify({ mock: true, worker: process.pid, enroll }),
  })
  // 6 identical-across-workers records (same enroll + same times + same raw
  // values — the fingerprint identity is what must dedupe them)
  return [
    rec(1001, 3600_000),
    rec(1001, 7200_000),
    rec(2002, 3600_000),
    rec(2002, 7200_000),
    rec(3003, 1800_000),
    rec(3003, 5400_000),
  ]
}

async function main() {
  const url = process.env.DATABASE_URL ?? ''
  const tmpRoot = process.env.WORKER_TMP_ROOT
  if (!tmpRoot) {
    console.error('WORKER_ERROR WORKER_TMP_ROOT is not set')
    process.exit(2)
  }
  // HARD rule: this worker only ever touches a test-owned temp database
  try {
    assertTestOwnedDatabasePath(url, tmpRoot)
  } catch (err) {
    console.error('WORKER_ERROR ' + (err as Error).message)
    process.exit(2)
  }

  const deviceId = process.env.WORKER_DEVICE_ID ?? null
  const serial = process.env.WORKER_SERIAL ?? 'CONC-SERIAL'
  const batch = buildBatch()

  let attempt = 0
  for (;;) {
    attempt++
    try {
      const res = await ingestRawPunches(batch, { deviceId, deviceSerial: serial, sourceProtocol: 'MOCK' })
      console.log(
        'WORKER_RESULT ' +
          JSON.stringify({
            pid: process.pid,
            attempt,
            seen: res.seen,
            newCount: res.newCount,
            duplicate: res.duplicate,
            matched: res.matched,
            unmatched: res.unmatched,
          }),
      )
      await db.$disconnect()
      process.exit(0)
    } catch (err) {
      const msg = (err as Error)?.message ?? String(err)
      const transient = /database is locked|database is busy|P2034/i.test(msg) && attempt < 4
      if (!transient) {
        console.error(`WORKER_ERROR attempt=${attempt} ${msg}`)
        await db.$disconnect().catch(() => {})
        process.exit(1)
      }
      await new Promise((r) => setTimeout(r, 150 + Math.floor(Math.random() * 350)))
    }
  }
}

main().catch(async (err) => {
  console.error('WORKER_CRASHED ' + ((err as Error)?.message ?? String(err)))
  await db.$disconnect().catch(() => {})
  process.exit(1)
})
