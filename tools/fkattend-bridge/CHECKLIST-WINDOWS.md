# FK623 Bridge — Hardware Validation Checklist (Windows machine)

This checklist runs the **real-hardware validation** of the v1.5.1 release
(NP756+ through the supervised FK623 bridge). Until every step below is
executed on the Windows machine that hosts the NP756+, the feature is
honestly reported as **"implemented + Mock-tested"** — NOT hardware-tested.

Machine context (recorded from the successful tests):
* Device: Novin Pardaz NP756+, serial `7561202505176`
* Device IP: `192.168.1.75`, TCP port **4238**
* SDK DLLs (all x86, all 7 of them) SHIP INSIDE the v1.5.1 zip at
  `tools\fkattend-bridge\bin` (`FK623Attend.dll` 41,984 bytes +
  FKAttend.dll, FKViaDev.dll, LFWViaDev.dll, FKPwdEncDec.dll,
  FpDataConv.dll, FaceDataConv.dll — the closure verified by PE import
  analysis; SHA-256 list in `bin/README-DLL.txt`)
* 32-bit PowerShell is the ONLY process type that may load them:
  `%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe`
* v1.5.0 reads through **FK623Attend.dll** with the recorded-successful
  signature set (FK_GetGeneralLogData WITHOUT workCode, FK_DisConnect,
  LoadGeneralLogData(handle, readMark)) — see CONTRACT.md.

> **v1.5.0 note:** there is NO manual bridge step anymore. The app starts,
> hides, supervises and stops the bridge by itself. This checklist exercises
> exactly the user-visible behaviour; a debug appendix at the end covers
> manual inspection when something needs investigating.
>
> **v1.5.1 note:** there is NO manual DLL copy anymore either. The 7 SDK DLLs
> ship inside the setup zip (`tools\fkattend-bridge\bin`) and the bridge
> script is saved UTF-8 **with BOM** so Windows PowerShell 5.1 decodes the
> Persian messages correctly on every system codepage.

---

## 1) Install (one time)

- [ ] Install HozorPlus (INSTALL.bat) on the Windows machine that can reach
      `192.168.1.75`. Start it once (START.bat), stop it again.
- [ ] Sanity (should already be true — nothing is copied manually anymore):
      `dir tools\fkattend-bridge\bin\FK623Attend.dll` → the ~41 KB file is
      there, together with its 6 companion DLLs (they ship in the zip).
- [ ] The device form now opens as a sectioned wider dialog (v1.5.1
      redesign: شناسهٔ دستگاه / اتصال / دریافت خودکار) — everything else
      behaves as before.

## 2) Device profile (NO bridge fields — they no longer exist)

- [ ] Login as admin, Devices → «دستگاه جدید»:
      - نوع: نوین پرداز NP · سریال: `7561202505176`
      - پروتکل ارتباطی: **FKAttend** (port default becomes 4238)
      - IP دستگاه: `192.168.1.75` · پورت: `4238`
      - رمز شبکه: the NUMERIC «رمز شبکه» (empty = 0) — never guessed; take
        the value from the device menu / vendor.
      - Advanced SDK (collapsed): defaults `5000 / 1 / 1261` are correct.
- [ ] Save. The card shows «FKAttend · SDK رسمی». The page-top chip shows
      «سرویس ارتباط دستگاه (داخلی): آماده — هنگام اولین استفاده خودکار اجرا می‌شود».

## 3) First «تست اتصال» — the app must do ALL of it

- [ ] Press «تست اتصال». Expected: within a few seconds a success box with
      latency (recorded: ~2104 ms) — and NO console window ever appears.
- [ ] The device keypad stays usable (the test never locks it).
- [ ] The service chip flips to «در حال اجرا · نسخه 2.0.0 …».
- [ ] Device log gains a SUCCESS row **without any secret value**.
- [ ] With the probe off (default): `serialNumber` is `—` in the result and
      that is FINE (the probe is deliberately off the critical path; a
      separate phase investigates it).

## 4) First real import (P0 acceptance)

- [ ] «دانلود ترددها» → «همه ترددهای حافظه».
- [ ] Expected: ~**2947 records** (the recorded triple read: 2947 × 3 with
      `endCode=-7`, ~2.9 s per read), matched to the enrolled personnel.
- [ ] Run it AGAIN → `0 جدید / همه تکراری` (fingerprint dedupe — the three
      recorded identical reads must produce ONE set of rows, never three).
- [ ] Check DB: `RawPunch` rows with `sourceProtocol='FKATTEND'`,
      `direction='UNKNOWN'`, `punchTimeRaw` = device wall clock strings,
      `workCode=''` (the FK623 signature returns none — never fabricated).
      Unmatched enroll numbers are stored with `matchStatus='UNMATCHED'`.
- [ ] `DeviceSyncRun` has a `SUCCESS` row with the counts and the mode.

## 5) Automatic pull (P2 acceptance)

- [ ] In the device form enable «دریافت خودکار» with interval `5` minutes.
- [ ] Wait ~6 minutes with the browser CLOSED → a new DeviceSyncRun row
      appears (scheduler lives in the server, not the browser; the bridge is
      already warm because an FKATTEND device exists).
- [ ] Restart HozorPlus (STOP.bat / START.bat) → the bridge child does NOT
      survive the stop (no orphan), and the next due tick starts it again by
      itself — no manual window, ever.
- [ ] While a sync is running, click «دانلود ترددها» → a clear busy message,
      no interleaving.

## 6) Near real-time polling (P3 acceptance)

- [ ] Enable «دریافت شبه‌بی‌درنگ (Polling)» with interval `30` seconds.
- [ ] Punch once on the device → within ~30 s + read time it appears in
      HozorPlus (RawPunch +, on a matched employee: PunchRecord + attendance).
- [ ] Task manager: exactly ONE hidden 32-bit powershell child of the Node
      server — not one per poll.

## 7) Live reports & notifications (P4 acceptance)

- [ ] With the attendance page open, punch on the device → the page refreshes
      without a manual reload.
- [ ] The punched employee (linked account) gets an in-app notification;
      admins get notified only for UNMATCHED punches (aggregated per run).
- [ ] Manual sync again → NO duplicate notifications.

## 8) Failure drills (integration acceptance)

- [ ] Kill the hidden bridge process in Task Manager → press «تست اتصال» →
      the app restarts it by itself and the test succeeds (crash recovery).
- [ ] Unplug the device / disable its network → «تست اتصال» shows the
      Persian connect error; plug back in → recovery works with no restart.
- [ ] Wrong port in the profile → the right error; restore `4238` → works.
      The DEVICE's own settings are never modified by any of this.
- [ ] Remove `tools\fkattend-bridge\bin\FK623Attend.dll` temporarily → the
      app shows the actionable «DLLهای SDK همراه خود بسته هستند — بسته را
      دوباره باز کنید / قرنطینهٔ آنتی‌ویروس» message; restore the DLL → works
      again.
- [ ] Close HozorPlus (STOP.bat) → no powershell bridge child remains.

## Rollback

* DB chain: `node scripts\migrate-v1.5.0-bridge-internal.mjs down --confirm`
  (re-adds the two obsolete bridge columns EMPTY — documented, raw data
  untouched), then the earlier migrations as needed.
* Device profile: switch «پروتکل ارتباطی» back to SBXPC — the legacy path
  keeps working unchanged.
* No device-side change is ever needed — HozorPlus is read-only against it.

## Debug appendix (manual bridge — ONLY for investigation)

`START-BRIDGE.bat` remains as a **debug tool**: it opens a visible bridge
console with its OWN printed token (separate from the app-managed one —
the app never reads a manually started instance; it will simply pick the
next free port for its own child). Useful commands while it runs:

```powershell
Invoke-RestMethod http://127.0.0.1:8765/ping
Invoke-RestMethod -Method Post http://127.0.0.1:8765/health -Headers @{ 'x-bridge-token' = '<PRINTED TOKEN>' }
Invoke-RestMethod -Method Post http://127.0.0.1:8765/device/test -Headers @{ 'x-bridge-token' = '<PRINTED TOKEN>' } `
  -ContentType 'application/json' -Body '{"machine":1,"ip":"192.168.1.75","port":4238,"timeoutMs":5000,"password":0,"license":1261}'
```
