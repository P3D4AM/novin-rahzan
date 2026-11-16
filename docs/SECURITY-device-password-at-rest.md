# Device password at-rest security (v1.3.1 — Phase 1 review fix 6)

## Current state — an honest statement

`Device.password` stores the device «رمز شبکه» (network password) as a **plaintext
integer** in the SQLite database file (`db/custom.db`).

What IS protected (since v1.3.0, `src/lib/device-secrets.ts`):

* no API response ever returns the value (`hasPassword` / `passwordFound` booleans only),
* no `DeviceLog` / `DeviceSyncRun` / notification / realtime payload ever contains it,
* historical v1.2.x log rows that contained it are redacted on read.

What is NOT protected:

* **anyone who obtains the database FILE reads the password.**
  The redaction work protects the process boundary, not the file at rest.
  v1.3.0 must therefore not be described as "complete credential protection" —
  this document replaces that claim with the real boundary.

## The chosen approach: Windows DPAPI, LocalMachine scope, opt-in

The deployment environment is a single Windows machine (INSTALL.bat / START.bat,
Node + SQLite, optional localhost x86 bridge). The appropriate OS-provided
mechanism is **DPAPI** (`CryptProtectData`, `DataProtectionScope.LocalMachine`),
implemented in `src/lib/device-secret-store.ts`:

* **No key beside the database.** DPAPI derives the key material from the
  machine itself; no key file is ever written next to `db/custom.db`.
* **No hardcoded key in source.** There is no key in the repository at all.
* **No homemade crypto.** Protection/round-trip verification is done by
  `System.Security.Cryptography.ProtectedData` through PowerShell; the
  application never implements or composes a cipher itself.
* Secret bytes travel to PowerShell via the environment (never via
  command-line arguments — nothing secret appears in a process list), and only
  Base64 output is read back.
* Storage format: `Device.passwordEnc = "dpapi:machine:<base64 blob>"`, with
  `Device.password` set to `NULL`. While `passwordEnc` is NULL, the plaintext
  column keeps working unchanged (full backward compatibility).

### Protection boundary (what opting in actually buys)

| Threat | Protected? |
| --- | --- |
| Database file stolen / backed up / shared WITHOUT the machine | **Yes** — the blob is undecryptable off-machine |
| Code executing on the machine as a local account | **No** — LocalMachine scope decrypts for local processes |
| Full machine compromise, or a malicious HozorPlus process | **No** |
| Password being displayed in the UI / API / logs | Yes — unchanged since v1.3.0 |

### Recovery requirements (the trade-off you accept when opting in)

A DPAPI LocalMachine blob can only be decrypted **on the same Windows machine**
it was protected on. Consequences:

* a database restored onto different hardware has **unrecoverable** device
  passwords — they must be re-entered in the device profile (the device itself
  still knows them),
* `SYSTEM`-level backups of the machine (not just the db folder) are required
  to preserve decryptability,
* if this trade-off is unacceptable, **stay opted out** — the plaintext gap is
  documented and the system keeps working.

## How to enable it (explicit, never silent)

Protection is applied per write only when BOTH hold:

1. the process runs on Windows, and
2. the environment variable `HOZOR_DEVICE_SECRET_PROTECTION=1` is set.

Migrating the credentials that already exist is a one-time, explicit admin action
(`scripts/protect-device-secrets.ts`), which protects → verifies the round-trip
→ and only then nulls the plaintext column, so a credential can never be lost
to a broken protection write:

```bat
set HOZOR_DEVICE_SECRET_PROTECTION=1
npx tsx scripts\protect-device-secrets.ts            :: protect all stored credentials
npx tsx scripts\protect-device-secrets.ts --revert   :: decrypt back into the plaintext column
```

The script refuses to run on non-Windows systems. With protection enabled but
unavailable, device saves fall back to the plaintext column and write a WARN
device log — the credential is never lost, and the gap stays visible.

`GET /api/devices` keeps exposing no values; it now shows `hasPassword`
(either representation) and `hasProtectedPassword` (DPAPI blob present).

## Remaining security limitations (v1.3.1)

1. **Opt-in is OFF by default.** Until the owner enables it on the Windows
   machine, the database keeps the passwords in plaintext — by design, because
   flipping it on silently would change recovery semantics for every existing
   installation. This is the documented gap, not a solved problem.
2. **`Device.bridgeToken`** (the localhost bridge shared secret) remains
   plaintext at rest — same exposure class, but it only ever protects a
   localhost-only endpoint, so it is not on the critical path. It can move to
   the same DPAPI store later without schema changes (same pattern).
3. **DPAPI scope is LocalMachine**, not per-service-account: any local process
   can decrypt. A tighter scope (CurrentUser + a service account) would break
   the "any admin account on the box can run the app" deployment model; that
   is an operational decision for the owner, documented here rather than
   silently chosen.
4. The DPAPI code path itself requires validation on the real Windows
   deployment; it is implemented and unit-format-tested, but the sandbox
   test environment is Linux, so the Windows path is **not yet validated on
   hardware** — same status as the NP756+ pipeline (Phase 2 hardware testing
   remains open).
