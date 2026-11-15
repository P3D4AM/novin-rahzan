# FKAttend Real-Time Capability Investigation — v1.4.2 (addendum §3.1)

**Mandatory technical investigation before any real-time implementation.**
Question: can HozorPlus detect a new punch on the NP756+ *as it happens*
(Push/Event), or is periodic Pull the only reliable path?

Verdict per the addendum's categories — **A/B/C per channel**:

| Channel | Category | Verdict |
| --- | --- | --- |
| **FKAttend.dll** (the port-4238 SDK path this integration uses) | **C** | Pull-only. No event/notification capability is available through this SDK. Periodic pull is the only reliable mechanism on this channel. |
| **Device-initiated RTLOG push** (the vendor's own real-time architecture) | **B** | The capability exists and is documented by the vendor's own artefacts, but it is a SEPARATE channel from FKAttend, has specific dependencies, and has NOT yet been validated against this particular NP756+ by us. |

Consequence: HozorPlus implements **Near Real-Time Polling** (addendum §3.3)
on the FKAttend pull path — honestly labelled "شبه‌بی‌درنگ (Polling)" in the UI —
and keeps the RTLOG push receiver as a SEPARATE investigation track that must
not be claimed as working until real-device evidence exists.

---

## 1. What was actually examined

1. **The call set recorded as successful against the real device** (from the
   user's own 32-bit PowerShell test on the Windows machine, port 4238):
   `FK_ConnectNet`, `FK_EnableDevice`, `FK_LoadGeneralLogData`,
   `FK_GetGeneralLogData`, `FK_Disconnect` — a full pull read of **2944
   records** ending with the SDK's normal end code **-7**.
2. **The FK Attend SDK family shape** (classic FK-series access-control SDK):
   the public API surface of this family is a request/response client —
   connect, enable/disable keypad, load buffers, read buffers by ref-params,
   disconnect. There is no documented event-callback, subscription, or
   notification API in this family, and none appeared in any call we could
   exercise.
3. **The repo's own protocol knowledge base** (`scripts/SBXPC-PROTOCOL.md`,
   v1.2.5–v1.2.6 reverse engineering of the vendor software: RealSvrOcxTcp.ocx
   parser, PacketParserStandard.dll, the vendor's shipped runtime log
   `logRealTimeDevice1.txt` with ~800 real punches): it describes a
   **device-initiated TCP push** of JSON punch frames (`RTLOG001|…` framing,
   `{"log_id":…,"user_id":…,"io_mode":…}`), which the VENDOR's server software
   receives and ACKs — i.e. the vendor achieves real-time via their own
   listening server, **not** via FKAttend.dll.
4. **What we could NOT do from this sandbox:** enumerate the actual DLL exports
   (the DLLs live on the Windows machine; the analysis outputs were never
   committed). The definitive enumeration is left to the machine that owns
   the DLLs — see §4.

## 2. The addendum's five questions, answered

**Q1 — Does FKAttend have an Event Notification capability?**
No. No evidence of any event/callback API exists in the call set, the SDK
family's public shape, or the vendor's own software (which needed a separate
push channel precisely because FKAttend does not provide one).

**Q2 — Can the device push new records to a server?**
Yes — but NOT through FKAttend/port 4238. The NP756+ family can be configured
(the vendor's software does exactly this) to open its own TCP connection to a
server address and push `RTLOG001`-framed JSON punches in real time. Evidence:
the vendor's own runtime log + the disassembled RTLOG parser. This is channel
"B": real architecture, real dependencies (device menu server-address config,
a listening server + firewall port, ACK replies), and — critically — **not yet
validated on this specific device by this project** (v1.2.6 shipped
`scripts/np-sniff.js` to capture exactly that evidence; no capture log has been
returned so far).

**Q3 — Does the SDK allow subscribing to new-record events?**
No. The FK classic SDK family has no subscribe API. (Definitive check: §4.)

**Q4 — Can a connection be kept open to receive notifications?**
No. FKAttend connections are per-operation: `FK_ConnectNet` returns a handle,
operations run, `FK_Disconnect` closes. Holding the handle open does not
produce any notification stream (and would hold the vendor's device session
without benefit). HozorPlus therefore connects per read and always disconnects
in `finally`.

**Q5 — Is periodic Pull the only option here?**
On the FKAttend path: yes. Alternatives that exist: the RTLOG push channel
(Category B, separate investigation) and short-interval polling (implemented).

## 3. Consequence implemented in this version (addendum §3.3)

* **Near Real-Time Polling** on the FKAttend pull path — per-device toggle,
  default 30 s (backend-validated 15..600 s), separate from the normal
  automatic sync interval, full-read + fingerprint dedupe (a poll can never
  miss a clock-skewed punch and can never duplicate anything), per-device
  mutex shared with the scheduler and manual sync, exponential backoff on
  failures, honest UI label «شبه‌بی‌درنگ (Polling)» with the latency explained
  (a punch appears within ~one polling interval + read time).
* **The RTLOG push receiver remains a separate track**: building it does not
  depend on FKAttend; it needs one real-device sniff capture to fix the exact
  framing parameters. Until that capture exists, no push capability is claimed
  anywhere in the product.

## 4. Definitive verification (on the Windows machine — 5 minutes)

Run the repo's own DLL analysis tools against `FKAttend.dll` (from the SDK
analysis folder) and inspect the export list for anything event/callback-like
(RegisterEvent, SetCallback, Notify, Subscribe, RealTime, Event…):

```
python scripts\map_exports.py     (export enumeration)
python scripts\re_recon.py        (strings + recon)
python scripts\scan_cmds.py       (command IDs)
```

Expected outcome (per the family shape above): connect/enable/load/get/set
buffers + device-info calls only. If an export with event/callback semantics
surfaces, the investigation moves to Category B for FKAttend and a follow-up
task should be opened — this document and the polling fallback stay the
default until then.

---
*This document records the state of evidence at v1.4.2. It must be updated,
not deleted, when the §4 verification runs on real hardware.*
