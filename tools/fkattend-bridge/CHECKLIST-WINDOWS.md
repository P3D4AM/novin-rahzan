# FKAttend Bridge — Hardware Validation Checklist (Windows machine)

This checklist runs the **real-hardware validation** of the v1.4.0 addendum
features. Until every step below is executed on the Windows machine that
hosts the NP756+, the new features are honestly reported as
**"implemented + Mock-tested"** — NOT hardware-tested.

Machine context (recorded from the successful tests):
* Device: Novin Pardaz NP756+, serial `7561202505176`
* Device IP: `192.168.1.75`, FKAttend TCP port **4238**
* SDK DLLs (all x86): in `C:\Users\Administrator\Desktop\NP756-SDK-Analysis`
  (FKAttend.dll, FK623Attend.dll, FKViaDev.dll, LFWViaDev.dll, FKPwdEncDec.dll, FpDataConv.dll, FaceDataConv.dll)
* 32-bit PowerShell is the ONLY process type that may load them:
  `%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe`

---

## 0) Preflight

- [ ] Windows machine and the NP756+ on the same LAN; ping `192.168.1.75`.
- [ ] FKAttend.dll present in the SDK analysis folder.
- [ ] (Optional, strengthens the push/poll investigation) run the existing
      export enumeration on the DLLs and archive the output:
      `python scripts\map_exports.py` / `re_recon.py` against FKAttend.dll —
      this is the definitive answer to "does FKAttend expose event callbacks".

## 1) Bridge up

- [ ] `tools\fkattend-bridge\START-BRIDGE.bat` (first arg = DLL folder if the
      DLLs are NOT next to the script):
      `START-BRIDGE.bat "C:\Users\Administrator\Desktop\NP756-SDK-Analysis"`
- [ ] The bridge prints its **BRIDGE TOKEN** — copy it (used in step 3).
- [ ] Health check from another PowerShell:
      `Invoke-RestMethod http://127.0.0.1:8765/health` → `ok: true`.
- [ ] If the window closes immediately: FKAttend.dll missing → pass the DLL folder.
      If it prints "32-bit only" → you launched 64-bit PowerShell; use the .bat.

## 2) Read-only connection test (keypad must NOT lock)

- [ ] From PowerShell:
      ```powershell
      Invoke-RestMethod -Method Post http://127.0.0.1:8765/device/test `
        -Headers @{ 'x-bridge-token' = '<TOKEN>' } -ContentType 'application/json' `
        -Body '{"machine":1,"ip":"192.168.1.75","port":4238,"timeoutMs":5000,"password":0,"license":1261}'
      ```
- [ ] Expected: `ok:true`, `latencyMs` in the hundreds, and (with the probe on)
      `serialNumber:"7561202505176"`, `deviceTime` in Tehran wall clock.
- [ ] The device screen must stay usable during the test (no keypad lock — the
      test never calls EnableDevice(0)).
- [ ] If `serialNumber`/`deviceTime` stay `null` and the bridge console logs
      "extras probe failed" → acceptable; set `-ProbeExtras 0` if noisy.
- [ ] If the probe CRASHES the child repeatedly → same remedy (`-ProbeExtras 0`);
      the bridge itself keeps serving.
- [ ] **Password note:** `password` above is the NUMERIC «رمز شبکه». The
      text→numeric conversion rule is undocumented and is NOT guessed here.
      If the device refuses connect with a wrong-password style code, take the
      numeric value from the device menu (or the vendor) — never a guess.

## 3) HozorPlus device profile

- [ ] Start HozorPlus, login as admin, Devices → «دستگاه جدید»:
      - نوع: نوین پرداز NP · سریال: `7561202505176`
      - پروتکل ارتباطی: **FKAttend** (explicit — port default becomes 4238)
      - IP دستگاه: `192.168.1.75` · پورت: `4238`
      - آدرس Bridge: `http://127.0.0.1:8765` · توکن Bridge: the copied token
      - رمز شبکه: the numeric network password (empty = 0)
      - مهلت اتصال: `5000` · شمارهٔ دستگاه SDK: `1` · License: `1261`
- [ ] Save. The device card shows `FKAttend · Bridge`.
- [ ] «تست اتصال» from the UI → success box with serial, response time,
      protocol FKAttend. Device log gains a SUCCESS row **without any secret**.

## 4) First real import (P0 acceptance)

- [ ] «دانلود ترددها» → «همه ترددهای حافظه».
- [ ] Expected: ~**2944 records, 14 enroll numbers, 2015-01-08 … 2026-11-15**
      (the same numbers as the recorded successful PowerShell test).
- [ ] Run it AGAIN → `0 جدید / همه تکراری` (fingerprint dedupe — no duplicates).
- [ ] Check DB: `RawPunch` rows with `sourceProtocol='FKATTEND'`,
      `direction='UNKNOWN'`, `punchTimeRaw` = device wall clock strings.
      Unmatched enroll numbers are stored with `matchStatus='UNMATCHED'`.
- [ ] `DeviceSyncRun` has a `SUCCESS` row with the counts and the mode.

## 5) Automatic pull (P2 acceptance)

- [ ] In the device form enable «دریافت خودکار» with interval `5` minutes.
- [ ] Wait ~6 minutes with the browser CLOSED → a new DeviceSyncRun row appears
      (scheduler lives in the server, not the browser).
- [ ] Restart HozorPlus (`STOP.bat` / `START.bat`) → wait for the next due tick;
      the schedule resumes WITHOUT re-configuring (settings live in the DB).
- [ ] While a sync is running, click «دانلود ترددها» → you get a clear
      «همگام‌سازی قبلی هنوز در حال اجراست» message, no interleaving.

## 6) Near real-time polling (P3 acceptance)

- [ ] Enable «دریافت شبه‌بی‌درنگ (Polling)» with interval `30` seconds.
- [ ] Punch once on the device (a finger/PIN that exists there).
- [ ] Expected: within ~30 s + read time, the punch appears in HozorPlus
      (RawPunch +, on a matched employee: PunchRecord + daily attendance row).
- [ ] Watch the bridge console: a `read-logs records=N endCode=-7` line per poll.
      If the device keypad feels busy, raise the interval (e.g. 60 s) — the UI
      label honestly says Polling, not real push.

## 7) Live reports & notifications (P4 acceptance)

- [ ] With the attendance page open, punch on the device → the page refreshes
      its data without a manual reload (socket event → refetch).
- [ ] The punched employee (with a linked user account) gets an in-app
      notification with time + device name; admins get notified only for
      UNMATCHED punches (aggregated per sync run).
- [ ] Run a manual sync again → NO duplicate notifications appear.
- [ ] EMPLOYEE account sees only its own notification, never other people's.

## 8) Failure drills (acceptance items 2/11)

- [ ] Stop the bridge → «تست اتصال» shows the clear "Bridge در حال اجرا نیست"
      Persian error; restart the bridge → everything recovers (retry/backoff).
- [ ] Change the device port in HozorPlus to a wrong value → connect fails
      with the right error; restore `4238` → works again. The DEVICE's own
      settings are never modified by any of this.

## Rollback

* DB: `node scripts\migrate-v1.4.0-phase2.mjs down --confirm` (drops only the
  v1.4.0 columns; v1.3.0 RawPunch layer is untouched).
* Device profile: switch «پروتکل ارتباطی» back to SBXPC — the legacy path
  keeps working unchanged. Stop the bridge (close its window).
* No device-side change is ever needed — HozorPlus is read-only against it.
