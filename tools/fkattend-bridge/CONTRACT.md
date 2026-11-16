# FK623 Bridge — JSON API Contract (v2.0.0)

The x86 bridge (`fkattend-bridge.ps1`, 32-bit PowerShell + **FK623Attend.dll**)
is the ONLY component allowed to talk to the NP756+ through the vendor SDK
(architecture decision D2: x86 DLLs never load inside the x64 Node process).
Since **v1.5.0** the bridge is **supervised** by HozorPlus itself
(`src/lib/bridge-supervisor.ts`, doc: `docs/BRIDGE-SUPERVISION.md`): started
hidden, restarted on crash, stopped cleanly — the user never runs, sees or
configures it.

```
HozorPlus (x64 Node/Next, :3000)  ── bridge-supervisor (managed child)
    │  localhost HTTP + auto token (env, never argv)          │
    ▼                                                          ▼
FK623 Bridge (32-bit PowerShell, 127.0.0.1:8765..8775) ── FK623Attend.dll
                                                          │ TCP 4238
                                                          ▼
                                                   NP756+ (192.168.1.75)
```

## Conventions

* Endpoint: `http://127.0.0.1:<port>` — **localhost only**, bound by the bridge
  itself; the port is chosen by the supervisor (8765 + fallback when busy).
* Token: generated per launch by the supervisor (`crypto.randomBytes(32)`) and
  delivered to the child ONLY through the `FKBRIDGE_TOKEN` environment variable
  — never on the command line, never in a file, never in the UI, never in Git.
  Every request except `/ping` carries `x-bridge-token: <token>`; wrong/missing
  token → `401`. The custom header forces a browser preflight, which blocks
  cross-site (CSRF) calls from web pages — combined with the loopback bind
  there is no network-reachable unauthenticated surface.
* Requests are **serialized** by the single-threaded HTTP loop → one device
  operation at a time; the bridge is a natural per-device mutex.
* Responses: JSON, UTF-8. `ok:false` carries a Persian `error` + machine-readable
  `code` (`CONNECT_FAILED` | `ENABLE_FAILED` | `LOAD_FAILED` | `AUTH` | `METHOD`
  | `NOT_FOUND` | `BRIDGE_ERROR`).
* **The network password is REQUEST-ONLY**: used for the single FK_ConnectNet
  call, never echoed, logged or written anywhere.
* **Read-only**: no endpoint empties/deletes logs or sets the device clock —
  such SDK calls are simply not reachable.
* Keypad re-enable (`FK_EnableDevice(handle, 1)`) + `FK_DisConnect` are guaranteed
  in `finally`; restore/disconnect failures are logged distinctly and reported.
* Exit codes: `0` clean stop · `1` wrong architecture · `2` script error ·
  `3` port already in use (supervisor retries on the next port) ·
  `4` FK623Attend.dll not found.

## Endpoints

### GET/POST /ping
**Unauthenticated liveness** (supervisor readiness probe). Returns the minimum:
`{ "ok": true, "pid": 4242 }` — no paths, no version, no dllDir.

### POST /health  (token required)
Status: `{ "ok": true, "version": "2.0.0", "dllDir": "C:\\...\\bin",
"uptimeSec": 120, "pid": 4242, "probeExtras": false }`

### POST /shutdown  (token required)
Graceful stop: `{ "ok": true }` then the listener stops and the process exits 0.

### POST /device/test  (token required)
**Read-only connection probe** — connect → (optional serial/time probe in a
CHILD process, off by default) → disconnect. Never locks the keypad, never
reads logs, never writes.

Request:
```json
{ "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000,
  "protocolType": 0, "password": 0, "license": 1261 }
```
Response (success):
```json
{ "ok": true, "serialNumber": null, "deviceTime": null, "latencyMs": 2104,
  "meta": { "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000, "license": 1261 } }
```
Response (failure → HTTP 200, `ok:false`):
```json
{ "ok": false, "error": "اتصال به دستگاه برقرار نشد (کد -1 ...) ...", "code": "CONNECT_FAILED" }
```

### POST /device/read-logs  (token required)
**Full log pull** — exactly the sequence recorded successful on the real device:
`FK_ConnectNet → FK_EnableDevice(0) [must return exactly 1, else reading never starts]
→ FK_LoadGeneralLogData(handle, 0) [must return exactly 1] → FK_GetGeneralLogData loop
(ret 1 = record, -7 = normal end) → FK_EnableDevice(1) + FK_DisConnect in finally`.

Same request shape as `/device/test`. Response (success):
```json
{ "ok": true,
  "serialNumber": null,
  "deviceTime": null,
  "latencyMs": 2871, "connectMs": 240,
  "endCode": -7,                 // -7 = normal end (observed on the real device)
  "endCodeAbnormal": false,      // true when the end code was UNEXPECTED — never silently "complete"
  "records": [
    { "enrollNumber": 1001, "verifyMode": 1, "inOutMode": 1,
      "timestamp": "2026-11-15 19:44:47" }
  ],
  "meta": { "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000, "license": 1261 } }
```

## The `workCode` rule (v2)

The confirmed `FK623Attend.dll` signature of `FK_GetGeneralLogData` returns
`enrollNumber / verifyMode / inOutMode / DateTime` — **no workCode**. The
bridge therefore emits records WITHOUT a `workCode` key, and HozorPlus treats
it as optional: `RawPunch.workCode` stays `''` and the raw payload omits the
key. **A fake value (e.g. a constant 0) is never fabricated.** (A bridge that
DOES return a workCode may include it; the field is optional on both sides.)

## Semantics on the HozorPlus side (src/lib/fkattend.ts)

* `timestamp` is Tehran wall clock → UTC via the shared DST-correct
  `tehranWallClockToUTC`, and kept verbatim in `RawPunch.punchTimeRaw`.
* `inOutMode`/`verifyMode` are stored RAW — the punch direction stays
  `UNKNOWN` until the vendor meaning is officially confirmed (never guessed).
* Idempotency: every record lands through the Phase-1 fingerprint dedupe —
  the recorded triple read of 2947 records imports nothing new on re-reads.

## Serial/time probes

`FK_GetSerialNumber` / `FK_GetDeviceTime` are NOT part of the recorded
successful call set → run (when explicitly enabled) in a crash-isolated child
process with the config piped via STDIN. Default in v2: **off** — out of the
critical connection path; a separate investigation phase owns them.
