# HozorPlus — Phase 2 Stage S5 Audit: Host-Timezone Independence

S5 mission: the computation engine and the API routes must be independent of the
**host machine's timezone**. This audit lists every place where the host
timezone could leak into a result, the real effect, and the fix. Every fix in
S5-B maps to an item (S5-A1 … S5-A12) in this table.

## The real project contract (extracted, not invented)

- **Instants** (`PunchRecord.timestamp`, `RawPunch.punchTime`,
  `LeaveRequest.startDateTime/endDateTime`, bookmarks): true UTC points in
  time. On the wire they are full ISO 8601 strings **with an explicit timezone**
  (`Z` or `±HH:MM`).
- **Day markers** (`DailyAttendance.date`, `ShiftAssignment.date`,
  `Holiday.date`, `OvertimeRequest.date`): a UTC-midnight `Date` that names one
  **Tehran calendar day**. Date-only strings parse as UTC midnight — never
  timezone-shifted (ES spec).
- **Business timezone**: `Asia/Tehran` via the Intl tz database (fixed +03:30 is
  a last-resort fallback only). The host machine's timezone and the browser's
  timezone are never business inputs.
- **Engine convention (since S3/S4)**: punches are fetched over **true
  Tehran-midnight windows** (`tehranDayWindowOfMarker`) and attributed to
  workdays via `attributePunchWorkday`; shift windows resolve through
  `tehranInstant` (DST-correct wall clock). The engine is already host-tz
  independent **except for one call: `faWeekday`** (item S5-A1).
- **UTC, Tehran, browser-local are three different things.** UTC = wire/storage
  format of instants; Tehran = the business calendar; browser-local = never a
  business input (display only, and even display goes through the central
  module).

## Findings

| # | Location | Host-tz dependency | Effect on a non-Tehran host | Fix (S5-B) |
| --- | --- | --- | --- | --- |
| S5-A1 | `src/lib/jalali.ts:14` `faWeekday(d)` → `d.getDay()` (LOCAL getter); used by the engine `recalcRange` (`isFriday`) and `scripts/seed.ts` | Local weekday of a UTC-midnight marker | On hosts west of UTC (e.g. `America/New_York`) every marker's weekday is shifted one day back: the Iranian weekend (Friday) is detected on **Thursday markers' day+1** — the OFF/weekend suppression runs on the wrong days, `OFF`/`HOLIDAY` rows and default-shift suppression shift by a day | Marker-based weekday from UTC getters: new central `faWeekdayOfMarker(marker)` (`(marker.getUTCDay()+1)%7`); legacy `faWeekday` switches to the UTC getter too (same value on Tehran/UTC hosts, correct everywhere) |
| S5-A2 | `src/lib/jalali.ts:18` `toJalali(d)` (jalaali-js local getters) via `jFormat`/`jFormatFull`/`faDateTime`; live caller: `scripts/seed.ts:108` (`toJalali(new Date())`) | Local Gregorian date of "now" | Seed picks the Jalali month of the HOST-LOCAL date — around Tehran midnight the seeded demo month/holidays differ by a day | Seed uses `jalaliOfMarker(tehranTodayMarker())` (central module). The legacy display functions keep their frozen signatures; after the seed fix they have **zero live callers** (documented below) |
| S5-A3 | `src/app/api/punches/route.ts` POST/PUT — `new Date(b.timestamp)` with no validation | ECMA-262: timezone-less datetimes (`2026-11-17T08:30`) parse as HOST-LOCAL | A manual punch/edited punch instant silently depends on the server timezone; `recalcDayForPerson` then attributes it to the wrong workday | Strict instant parsing via new central `parseInstantParam` (full ISO with explicit timezone only) → clean Persian 400 otherwise |
| S5-A4 | `src/app/api/leave/requests/route.ts` POST — `new Date(b.startDateTime)` / `new Date(b.endDateTime)` | same as S5-A3 | Leave spans shift with the server timezone → wrong `leaveMinutes`/`MISSION` credit | Same strict `parseInstantParam` contract → 400 |
| S5-A5 | `src/app/api/import/route.ts:83` — `new Date(dtRaw)` for every `…T…` string, without requiring a timezone suffix | same as S5-A3 | A CSV row with a timezone-less ISO datetime becomes a host-local instant (the documented contract says full ISO **with timezone** passes through) | T-form strings go through `parseInstantParam` — explicit timezone mandatory; timezone-less T-forms are skipped exactly like other unparseable rows |
| S5-A6 | `src/app/api/kiosk/route.ts` and `src/app/api/me/route.ts` (GET + POST): "today's punches" instant window `[tehranTodayMarker(), marker+1)` | The marker is UTC-midnight, so the INSTANT window is the UTC calendar day of the Tehran date — **not** the Tehran day | The direction-flip window is displaced by +3:30 from Tehran midnight: Tehran 00:00–03:29 punches are NOT counted for direction (an OUT right after them becomes a second IN), while the NEXT day's 00:00–03:29 punches ARE counted. The v1.6.0 comment claimed Tehran-day alignment; the instants say otherwise | New central `tehranTodayWindow()` = `tehranDayWindowOfMarker(tehranTodayMarker())`; both routes use its true Tehran-midnight `[start, end)` |
| S5-A7 | `src/app/api/punches/route.ts` GET — `markerRangeInstantWindow(from, to)` = UTC-midnight windows for punch instants (documented defect **D1**) | UTC-midnight instant window ≠ Tehran-day window | Read path desynced from the engine (which since S3 attributes by Tehran-day windows): punches at Tehran 00:00–03:29 of day D appear under day D−1's filter and day D+1's early punches appear under D. D1's original reason ("engine still uses UTC-midnight windows") no longer holds — the engine moved in S3/S4 | New central `tehranDayRangeInstantWindow(from, to)` = `[Tehran midnight of from, Tehran midnight of to+1)` — the SAME window the engine fetches punches over. **D1 is retired** in docs/API-DATE-CONTRACTS.md |
| S5-A8 | `src/lib/ebio.ts:207` — fallback year `new Date().getFullYear()` when a device TimeLog omits `Year` | HOST-LOCAL year | Between UTC Dec 31 20:30 and Tehran Jan 1 03:29 the Tehran year is already the next one; a negative-offset host reports the previous year → punch year off by one | Fallback year = `tehranParts(new Date()).y` (the device wall clock is Tehran time) |
| S5-A9 | `src/app/api/ai/route.ts` — `dayMarkerOf(new Date(String(args.date)))` (4 tool branches) | `new Date(garbage)` → Invalid Date; naive datetimes → host-local | Junk/naive date from the assistant silently becomes a NaN marker (Prisma 500) or a host-local instant | All branches parse through the strict central `parseDayParam` (date-only or full ISO with timezone → marker); invalid input returns a Persian error summary, never a wrong day |
| S5-A10 | `src/app/api/rotations/route.ts:5,89` — dead `faWeekday` import kept alive by `void faWeekday` | none (dead code) | Imports the host-tz symbol for no reason | Remove the dead import + `void` statement |
| S5-A11 | `src/lib/raw-import.ts`, `src/lib/raw-sync.ts`, `src/lib/notify.ts`, `src/lib/jalali.ts` fixed-offset `tehranDayOf`/`tehranMinutes`/`tehranDate`/`faTime` | **None** — these use only UTC getters plus the fixed +03:30 offset (host-tz independent; DST-naive for pre-2022 instants by the frozen device-pipeline contract, parity-proven by the test suite) | — | Untouched (protected device pipeline; documented) |
| S5-A12 | `src/lib/time.ts` core (`tehranParts`, `tehranInstant`, `dayMarkerOf`, `resolveWorkday`, …), API day-param parsing (`parseDayParam`), `attendance`/`holidays`/`overtime`/`assignments`/`reports`/`bootstrap` routes, UI `jalali-ui.tsx` | **None** — Intl-based / UTC getters / marker columns | — | Untouched (verified by the S5 TZ matrix) |

## Verified clean (no host-tz dependency)

- All API day-parameter routes (`attendance`, `reports`, `holidays`,
  `overtime`, `assignments`, `rotations`, `punches GET`) go through the strict
  `parseDayParam`.
- `scheduler.ts` works on millisecond intervals (`Date.now()`), no
  calendar-day logic.
- `mini-services/realtime` has no date logic.
- Client components (`dashboard`, `attendance`, `leave`, `reports`,
  `mycard`, `kiosk`) already consume the central module (v1.6.0 phase 1
  stage 4) — browser-tz independent.
- `src/lib/ebio.ts` `wallClockToUTC`, `src/lib/sbxpc.ts`
  `tehranWallClockToUTC`, `src/lib/fkattend.ts` `fkWallClockToUTC` are
  Intl/Tehran-based (device-side authorities).

## Historical decisions preserved (NOT reverted without reason)

- The frozen legacy layer `src/lib/jalali.ts` keeps serving the raw device
  pipeline (`tehranDayOf` etc. — fixed offset). Only `faWeekday` changes (its
  engine call site demanded it); `toJalali`/`jFormat`/`jFormatFull`/
  `faDateTime` keep their signatures — after S5 they have **zero live
  callers** (seed migrated; the UI uses the central module), and their
  host-local behavior is recorded here so it can never silently re-enter the
  engine.
- D1 ("read paths keep UTC-midnight windows until the engine migrates") is
  **retired** in S5 because its premise expired: S3/S4 moved the engine to
  Tehran-midnight windows + workday attribution, so the read paths now ALIGN
  with the engine (S5-A7) instead of trailing it.
- The marker storage convention (UTC-midnight = one Tehran day) is unchanged;
  S5 changes only **instant-window derivation** for read paths, never stored
  values.
