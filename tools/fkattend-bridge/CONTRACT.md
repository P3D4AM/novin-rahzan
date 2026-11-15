# FKAttend Bridge — JSON API Contract (v1.4.0)

The x86 bridge (`fkattend-bridge.ps1`, 32-bit PowerShell + FKAttend.dll) is the
ONLY component allowed to talk to the NP756+ through the official FKAttend SDK
(architecture decision D2: x86 DLLs never load inside the x64 Node process).

```
HozorPlus (x64 Node/Next, :3000)
    │  HTTP + token, localhost only
    ▼
FKAttend Bridge (32-bit PowerShell, 127.0.0.1:8765)
    │  FKAttend.dll — recorded-successful call set only
    ▼
NP756+  (e.g. 192.168.1.75:4238)
```

## Conventions

* Base URL: `http://127.0.0.1:<port>` — **localhost only**, enforced by the
  bridge AND re-checked by HozorPlus (device settings validation + provider).
* Auth: every request (except `/health`) carries header
  `x-bridge-token: <shared secret>`. Wrong/missing token → `401`.
* Requests are **serialized** by the single-threaded HTTP loop → one device
  operation at a time; the bridge is a natural per-device mutex.
* Responses: JSON, UTF-8. `ok:false` carries a Persian `error` + machine-readable `code`
  (`CONNECT_FAILED` | `LOAD_FAILED` | `AUTH` | `METHOD` | `NOT_FOUND` | `BRIDGE_ERROR`).
* **The network password is REQUEST-ONLY**: it is used for the single
  FK_ConnectNet call, never echoed, logged or written anywhere.
* **Read-only**: the bridge exposes NO endpoint that empties/deletes logs or
  sets the device clock — such SDK calls are simply not reachable.
* Keypad re-enable (`FK_EnableDevice(1)`) + `FK_Disconnect` are guaranteed in
  `finally` — no code path leaves the device locked or connected.

## Endpoints

### POST /health
No token required. Liveness + DLL status.

```json
{ "ok": true, "version": "1.4.0", "dllDir": "C:\\...\\NP756-SDK-Analysis",
  "uptimeSec": 120, "pid": 4242, "probeExtras": true }
```

### POST /device/test
**Read-only connection probe** — connect → optional serial/time probe (child
process) → disconnect. Never locks the keypad, never reads logs, never writes.

Request:
```json
{ "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000,
  "protocolType": 0, "password": 0, "license": 1261 }
```
Response (success):
```json
{ "ok": true,
  "serialNumber": "7561202505176",        // null when the probe is off/failed
  "deviceTime": "2026-11-16 10:12:30",    // null when unavailable
  "latencyMs": 812,
  "meta": { "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000, "license": 1261 } }
```
Response (failure → HTTP 200, `ok:false`):
```json
{ "ok": false, "error": "اتصال به دستگاه برقرار نشد (کد -1 ...) ...", "code": "CONNECT_FAILED" }
```

### POST /device/read-logs
**Full log pull** — the recorded-successful sequence:
`FK_ConnectNet → FK_EnableDevice(0) → FK_LoadGeneralLogData → FK_GetGeneralLogData loop → FK_EnableDevice(1) → FK_Disconnect`
(re-enable/disconnect in `finally`).

Same request shape as `/device/test`. Response (success):
```json
{ "ok": true,
  "serialNumber": "7561202505176",
  "deviceTime": "2026-11-16 10:12:30",
  "latencyMs": 1503, "connectMs": 240,
  "endCode": -7,                          // -7 = normal end (as observed on the real device)
  "records": [
    { "enrollNumber": 1001, "verifyMode": 1, "inOutMode": 1, "workCode": 0,
      "timestamp": "2026-11-15 19:44:47" }   // device WALL CLOCK (Tehran) — converted to UTC by HozorPlus
  ],
  "meta": { "machine": 1, "ip": "192.168.1.75", "port": 4238, "timeoutMs": 5000, "license": 1261 } }
```

Semantics on the HozorPlus side (implemented in `src/lib/fkattend.ts`):
* `timestamp` is treated as Tehran wall clock → UTC via the shared
  `tehranWallClockToUTC` (DST-correct through the tz database), and ALSO kept
  verbatim in `RawPunch.punchTimeRaw`.
* `inOutMode`/`verifyMode` are stored RAW — the punch direction stays
  `UNKNOWN` until the vendor meaning is officially confirmed (never guessed).
* Idempotency: every record lands through the Phase-1 fingerprint dedupe —
  re-reading the full history imports nothing new.

## Security notes

* Bridge binds `127.0.0.1` only. Device settings in HozorPlus additionally
  reject any non-localhost bridge URL ("آدرس Bridge فقط روی همین سیستم مجاز است").
* The bridge token is stored like the network password (never returned by any
  API, never written to logs; the UI shows only "توکن: ●●●●").
* Serial/time probes (`FK_GetSerialNumber`, `FK_GetDeviceTime`) are
  crash-isolated child processes because their signatures are NOT part of the
  recorded successful test. If they misbehave, run the bridge with
  `-ProbeExtras 0` — only the serial column becomes unavailable.
