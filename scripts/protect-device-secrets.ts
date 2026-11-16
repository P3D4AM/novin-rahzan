/**
 * v1.3.1 (Phase 1 review fix 6) — ONE-TIME, EXPLICIT admin action to move the
 * device network passwords («رمز شبکه») from the plaintext Device.password
 * column into DPAPI-protected Device.passwordEnc blobs.
 *
 * This script is the ONLY sanctioned way to migrate existing credentials:
 *   * it PRESERVES every credential (protect → verify round-trip → only then
 *     null the plaintext column),
 *   * it refuses to run on non-Windows systems (DPAPI is unavailable — the
 *     plaintext gap stays documented instead of being faked),
 *   * it never prints secret values (only counts and device names),
 *   * `--revert` decrypts everything back into the plaintext column (rollback
 *     path / pre-migration of a database to another machine).
 *
 * Usage (on the Windows deployment machine):
 *   set HOZOR_DEVICE_SECRET_PROTECTION=1
 *   npx tsx scripts/protect-device-secrets.ts            # protect
 *   npx tsx scripts/protect-device-secrets.ts --revert   # back to plaintext
 *
 * See docs/SECURITY-device-password-at-rest.md for the protection boundary
 * and the recovery trade-offs BEFORE opting in.
 */
import { db } from '../src/lib/db'
import {
  deviceSecretProtectionPlatform,
  protectDeviceSecret,
  unprotectDeviceSecret,
  isProtectedSecret,
} from '../src/lib/device-secret-store'

async function main() {
  const revert = process.argv.includes('--revert')

  if (!deviceSecretProtectionPlatform()) {
    console.error(
      '[protect-device-secrets] این اسکریپت فقط روی Windows اجرا می‌شود (DPAPI) — ' +
      'روی سیستم‌های دیگر، رمزهای دستگاه به‌صورت متن ساده باقی می‌مانند (محدودیت مستندشده).',
    )
    process.exit(2)
  }

  if (revert) {
    const devices = await db.device.findMany({ where: { passwordEnc: { not: null } } })
    console.log(`[protect-device-secrets] reverting ${devices.length} protected credential(s) to the plaintext column…`)
    let ok = 0
    let failed = 0
    for (const d of devices) {
      if (!d.passwordEnc || !isProtectedSecret(d.passwordEnc)) continue
      try {
        const plain = await unprotectDeviceSecret(d.passwordEnc)
        const n = Number(plain)
        if (!Number.isFinite(n)) throw new Error('decrypted value is not numeric')
        // write the plaintext column FIRST; only then clear the blob — a crash
        // between the two steps leaves BOTH set, which reads still handle
        await db.device.update({ where: { id: d.id }, data: { password: n } })
        await db.device.update({ where: { id: d.id }, data: { passwordEnc: null } })
        ok++
      } catch (err) {
        failed++
        console.error(`[protect-device-secrets]  ✘ ${d.name}: ${(err as Error).message}`)
      }
    }
    console.log(`[protect-device-secrets] revert done — ${ok} reverted, ${failed} failed (they stay protected).`)
    process.exit(failed > 0 ? 1 : 0)
  }

  const devices = await db.device.findMany({ where: { password: { not: null }, passwordEnc: null } })
  console.log(`[protect-device-secrets] protecting ${devices.length} plaintext credential(s) with DPAPI (LocalMachine)…`)
  let ok = 0
  let skipped = 0
  let failed = 0
  for (const d of devices) {
    if (d.password == null) continue
    try {
      const enc = await protectDeviceSecret(String(d.password))
      // verify the round-trip BEFORE dropping the plaintext copy — a credential
      // must never be lost to a broken protection write
      const roundTrip = await unprotectDeviceSecret(enc)
      if (roundTrip !== String(d.password)) throw new Error('round-trip verification failed — keeping plaintext')
      await db.device.update({ where: { id: d.id }, data: { passwordEnc: enc } })
      await db.device.update({ where: { id: d.id }, data: { password: null } })
      ok++
    } catch (err) {
      failed++
      console.error(`[protect-device-secrets]  ✘ ${d.name}: ${(err as Error).message} — plaintext kept, nothing lost`)
    }
  }
  const alreadyProtected = await db.device.count({ where: { passwordEnc: { not: null } } })
  skipped = devices.length - ok - failed
  console.log(
    `[protect-device-secrets] done — ${ok} protected, ${failed} failed (kept plaintext), ` +
    `${Math.max(skipped, 0)} skipped; ${alreadyProtected} credential(s) are protected at rest now.`,
  )
  await db.$disconnect()
  process.exit(failed > 0 ? 1 : 0)
}

main().catch(async (err) => {
  console.error('[protect-device-secrets] CRASHED:', (err as Error).message)
  await db.$disconnect().catch(() => {})
  process.exit(1)
})
