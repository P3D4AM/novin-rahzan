# HozorPlus — Phase 1 (Time Infrastructure & Jalali Calendar): Stage 1 Audit

> Phase: 1 — Time Infrastructure & Jalali Calendar
> Stage: 1 — read-only audit & design (no code / no DB changes in this stage)
> Baseline: `ffae67bc80204af00c66b2bae5576e31cf1ed9fa` (develop @ origin, verified via `git ls-remote`)
> Approved business policies: `workday_policy = shift`, `overtime_policy = min_excess`, `mission_policy = approved_credit`

## 1. Current date/time architecture (as found)

### Storage conventions (Prisma / SQLite) — to be preserved

| Class | Fields | Representation |
| --- | --- | --- |
| Day markers | `DailyAttendance.date`, `ShiftAssignment.date`, `Holiday.date`, `OvertimeRequest.date` | UTC-midnight `DateTime` marking a Tehran **calendar day** |
| Real instants | `PunchRecord.timestamp`, `RawPunch.punchTime` (+ `punchTimeRaw` wall-clock string), `LeaveRequest.startDateTime/endDateTime`, `Device.rawCursorAt/lastLogAt/lastSyncAt`, all audit `createdAt` | true UTC instants |
| Jalali display strings | `Personnel.birthDate/hireDate/endDate` | Jalali text, display-only |

No schema or data migration is required by this phase. The runtime dev DB is empty and stale (pre-v1.3.0 shape); production upgrades flow through the repo's own `scripts/migrate-v*.mjs` + `prisma db push` path, which later stages must respect.

### Two parallel date layers (the actual inconsistency)

- `src/lib/jalali.ts` — server helpers; **hardcoded `TEHRAN_OFFSET_MIN = 210`** and `toJalali(d)` goes through jalaali-js's **environment-local date getters** → server-timezone sensitive.
- `src/components/app/jalali-ui.tsx` — duplicates `faDigits`; a **different** `faWeekday` (`getUTCDay` vs `getDay`); `faToday`/`jFormatFa` are **browser-timezone sensitive**, while `faTime`/`useTehranClock` are already correct via `Intl … timeZone:'Asia/Tehran'`.

### Protected device subsystem (healthy — keep)

`sbxpc.ts:tehranWallClockToUTC`, `ebio.ts:wallClockToUTC`, `fkattend.ts:fkWallClockToUTC` convert Tehran device wall-clock → UTC instant through a **timezone-aware Intl offset** (fixed +03:30 only as last-resort fallback). The raw pipeline keeps full accounting (quarantine, dedupe fingerprints, transactional bookmark advance, `touchedDays` at `(personId, tehranDayOf(punchTime))`). This subsystem is explicitly out of refactoring scope.

## 2. Verified defects (file:line)

| # | Location | Defect |
| --- | --- | --- |
| D1 | engine windows in `attendance.ts` (`recalcRange`), `me/route.ts` todayPunches, `notify.ts` | Tehran 00:00–03:29 punches land in the previous day's UTC-midnight window (systematic 3.5h boundary skew) — **engine fix out of this phase's scope; read paths deliberately kept consistent with the engine until that phase** |
| D2 | `kiosk/route.ts:29` | direction "today" = UTC calendar day |
| D3 | `bootstrap/route.ts:13`, `me/route.ts:25` | app-wide "today" is server-timezone sensitive |
| D4 | `jalali-ui.tsx` `faToday`/`jFormatFa` | browser-timezone dependent display/defaults |
| D5 | `attendance.tsx:34/38/200`, `dashboard.tsx:41/66`, `leave.tsx:254` | "today"/default ranges derived from `toISOString().slice(0,10)` (UTC date) |
| D6 | `attendance.tsx:101–103`, `leave.tsx:212/217/223/227/369` | native Gregorian `<input type=date/time>` instead of Jalali controls |
| D7 | `attendance.tsx:233`, `leave.tsx:55–56` | manual punch/leave instants built with browser-local `new Date("…T…")` |
| D8 | `attendance.tsx:135/282`, `leave.tsx:149` | Gregorian ISO strings rendered with Persian digits instead of Jalali dates; hourly leave time shown in UTC |
| D9 | `import/route.ts:75–76` | CSV import date parsing is server-local |
| D10 | `overtime/route.ts`, `holidays`, `assignments`, `rotations` | date-only convention relies on implicit client-side UTC-midnight ISO |
| D11 | `devices/route.ts:263` | device-time status message formatted with `toLocaleString('fa-IR')` (server-local tz) |

No shift-aware workday resolution exists today (`computeDaily` has only overnight heuristics). `ShiftSegment` already models overnight shifts (`endMin <= startMin` → crosses midnight), which the new resolver builds on.

## 3. Healthy patterns to keep

- Reports UI Jalali month pick → UTC-midnight markers → `startOfUTCDay` round trip (the convention to extend).
- `JalaliDateInput` + `parseJalaliString` (validated, leap-aware) for Jalali string fields.
- `Intl`-based Tehran time formatting (`faTime`, `useTehranClock`).
- Device wall-clock → instant conversion in the protected pipeline.

## 4. Implementation plan (per stage)

1. **Stage 2 — central `src/lib/time.ts`**: tz-aware Tehran parts/offset/instant builders; true Tehran-midnight day windows; day-marker helpers; numeric-only Jalali conversions; Jalali input validation (Persian errors); `resolveWorkday(instant, shift)` implementing `workday_policy = shift` (overnight, month/year-crossing, >24h safety, documented fallbacks); `docs/TIME-POLICIES.md` recording all three approved policies verbatim; `scripts/test-time.ts` covering all 15 mandatory categories incl. DST-era `Asia/Tehran` (pre-2022) and browser/server TZ-independence (child processes under `TZ=UTC/America/New_York/Asia/Tehran`); device-pipeline regression assertions (equality with `tehranWallClockToUTC`/`fkWallClockToUTC`, legacy-compat markers). **The engine (`attendance.ts`) and the whole device pipeline stay untouched.**
2. **Stage 3 — API consistency**: explicit date-only parsing (no accidental UTC/tz shift), server-tz-independent "today" for display/default derivations (`bootstrap`, `me`, `ai`, `devices` message), contract documentation (`docs/API-DATE-CONTRACTS.md`) incl. the deliberate preservation of engine-consistent instant windows (D1 deferral rationale: fixing read paths alone would desync تردد/کارکردها/گزارش‌ها).
3. **Stage 4 — Jalali UI**: reusable `JalaliDatePicker` (RTL grid, month/year navigation, leap-aware, Tehran-based today, mobile+desktop, Persian validation); migrate D6/D7/D8 controls and displays; client "today" via the central module.
4. **Stage 5 — regression**: full new suite + `test:raw` + `test:bridge`; policies verified documented; raw pipeline invariants re-proven.
5. **Stage 6 — validation & delivery**: typecheck/lint/build, diff review, version bump to 1.6.0, tag + pre-release per repo release policy.

## 5. Dependency notes

- `jalaali-js@2.0.1` is reliable but only its **numeric** API is used (`toJalaali(gy,gm,gd)`); its `Date` overload uses local getters and is never used by the new layer.
- `date-fns` and `react-day-picker`/`ui/calendar.tsx` are unused in live paths; **no new dependencies** are introduced (tests run with the existing `tsx`).
- Node ≥ 20 ships full ICU (`Asia/Tehran` verified on Node 24 in the dev sandbox).
