# HozorPlus — API Date/Time Contract (v1.7.0, S5)

Single reference for what every date/time field on the API means and how it is
parsed/serialized. Enforced by `src/lib/time.ts` (the central layer) and tested
in `scripts/test-time.ts` (sections 14, 16–19, 20–24) plus
`scripts/test-attendance-db.ts` under the four-host-timezone matrix
(`scripts/tz-matrix.sh`: UTC / Asia/Tehran / Europe/Vienna / America/New_York).

## The three field classes

| Class | Meaning | Wire format | Server behavior |
| --- | --- | --- | --- |
| **instant** | a real point in time | full ISO 8601 **with an explicit timezone** (`2026-11-16T23:00:00.000Z`, `…+03:30`) | parsed by `parseInstantParam` → true UTC instant; stored as-is; never re-interpreted |
| **business day (marker)** | one Tehran calendar day | `YYYY-MM-DD` **date-only** or marker ISO (`…T00:00:00.000Z`) | parsed by `parseDayParam` → UTC-midnight marker; **never timezone-shifted** |
| **Jalali string** | display-only Jalali text | `1405/08/26` | validated by `parseJalaliInput` (Persian errors); stored as text |

## Reference timezone & conversion responsibility

- **Reference business timezone: `Asia/Tehran`** — via the Intl timezone
  database (DST-correct historically; Iran has had no DST since 2022). The
  fixed +03:30 offset is a last-resort fallback only.
- **Server responsibility**: parse everything strictly (`parseDayParam`,
  `parseInstantParam`), derive Tehran parts/marks/windows
  (`tehranParts`, `dayMarkerOf`, `tehranDayWindowOfMarker`), convert
  Gregorian⇄Jalali numerically (`jalaliOfMarker`, `tryJalaliToMarker`). The
  **host machine's timezone never enters any result** (proven by the four-TZ
  matrix; the engine core is byte-identical under UTC/Tehran/Vienna/New York).
- **Client responsibility**: send instants as full ISO with `Z`/`±HH:MM` and
  days as date-only `YYYY-MM-DD`; display through the central module
  (`src/lib/time.ts` is isomorphic) or `jalali-ui.tsx` — never through
  browser-local `new Date(y,m,d)` constructors or `.getDate()`/`.getHours()`.

## Parsers (the single, tested contracts)

**`parseDayParam` (day params — strict since v1.6.0):** date-only strings
parse as UTC midnight (ES spec — no accidental UTC conversion, no
browser-local shift); full ISO timestamps **with an explicit timezone** (`Z`
or `±HH:MM`, strict component ranges, leap-aware calendar) are floored to
their UTC day (legacy-compatible); **timezone-less datetimes
(`2026-11-17T00:30`) are REJECTED** — ECMA-262 treats them as host-LOCAL
time. Basic-format offsets (`+0330`), space-separated datetimes and every
non-ISO form are rejected; anything rejected → clean 400.

**`parseInstantParam` (timestamps — NEW in S5, strict):** the same grammar
as `parseDayParam`'s ISO branch but WITHOUT day-flooring — it returns the
true UTC instant. This is now the single write-path contract for
`POST/PUT /api/punches`, `POST /api/leave/requests` and the CSV `datetime`
T-form. **Timezone-less datetimes are REJECTED** (they used to fall into
`new Date(...)` and silently become host-local — the exact S5 defect class
S5-A3/A4/A5); malformed/impossible components are rejected, never normalized.

## Valid / invalid examples

```
DAY PARAMS (parseDayParam)
  2026-11-17                        ✔ day marker 2026-11-17
  2026-11-7                         ✔ (1–2 digit month/day, legacy)
  2026-11-17T22:30:00Z             ✔ instant → floored to 2026-11-17
  2026-11-16T22:30:00-05:00        ✔ → floored to 2026-11-17
  2026-11-17T00:30                 ✘ 400 — timezone-less datetime (host-local trap)
  2026-11-17T22:30:00+0330         ✘ 400 — basic-format offset
  2026-11-17 00:30 / March 5, 2026  ✘ 400 — non-ISO forms
  2026-02-31T10:00:00Z             ✘ 400 — impossible date

INSTANTS (parseInstantParam)
  2026-11-17T08:30:00Z             ✔ 2026-11-17T08:30:00.000Z
  2026-11-17T08:30:00.123-05:00    ✔ 2026-11-17T13:30:00.123Z
  2026-11-17T08:30:00+03:30        ✔ 2026-11-17T05:00:00.000Z
  2026-11-17T00:30 / …T00:30:00    ✘ (400 or skipped row) — timezone-less
  2026-11-17                        ✘ — a DAY is not an instant
  2026-11-17T25:00:00Z             ✘ — hour 25

NAIVE WALL CLOCK (CSV import, tehranWallClockStringToInstant)
  "2026-11-17 02:30"               ✔ Tehran wall-clock → 2026-11-16T23:00:00Z
  "2021-03-22 00:30"                ✘ historical DST gap hour — skipped
  "2021-09-21 23:30"               ✔ deterministic LATER occurrence
  "1405/08/26 02:30"               ✘ Jalali-year string — never reinterpreted
```

## Day boundaries & windows

- **Markers are storage** (`DailyAttendance.date`, `ShiftAssignment.date`,
  `Holiday.date`, `OvertimeRequest.date`): UTC midnight = one Tehran day.
- **Tehran day windows are instants**: `tehranDayWindowOfMarker(marker)` =
  [Tehran midnight, next Tehran midnight) (e.g. 2026-11-17 →
  `2026-11-16T20:30:00Z … 2026-11-17T20:30:00Z`).
- **Punch read filters** (`GET /api/punches` from/to) use
  `tehranDayRangeInstantWindow(from, to)` =
  [Tehran midnight of `from`, Tehran midnight of `to`+1) — **the same window
  the attendance engine fetches punches over** (`recalcRange`). D1 is retired:
  since S3/S4 the engine attributes by Tehran-day/shift windows, so the read
  paths now align with it (the old UTC-midnight instant windows were
  displaced +3:30 from Tehran midnight — a Tehran 00:00–03:29 punch appeared
  under the previous day's filter).
- **Kiosk/me "today" windows** use `tehranTodayWindow()` — the true Tehran
  day of `now` (direction flip + today's punch list; same convention as the
  engine's fallback day attribution). Was the UTC calendar day of the Tehran
  date — same +3:30 displacement.

## Per-endpoint contract

| Endpoint | Fields | Class / rule |
| --- | --- | --- |
| `GET /api/punches` | `from`, `to` | date-only business days → **Tehran-day instant window** `tehranDayRangeInstantWindow` (S5; was UTC-midnight marker window — D1); junk → 400 |
| `POST/PUT /api/punches` | `timestamp` | **strict instant** (`parseInstantParam`, S5) — full ISO with explicit timezone; timezone-less → 400 Persian message |
| `GET /api/attendance` | `from`, `to` | date-only business days → marker range on `DailyAttendance.date`; junk → 400 |
| `GET /api/reports` | `from`, `to` | date-only business days → marker range (Jalali month pickers map 1:1) |
| `POST /api/overtime` | `date` | date-only business day → marker (`parseDayParam`) |
| `POST /api/holidays` | `dateJalali` / `date` | Jalali string → marker (validated) / date-only marker |
| `GET /api/holidays`, `/api/assignments`, `/api/rotations` | `from`, `to`, `date` | date-only business days → markers; junk → 400 |
| `POST /api/leave/requests` | `startDateTime`, `endDateTime` | **strict instants** (`parseInstantParam`, S5); timezone-less → 400 |
| `GET /api/me`, `POST /api/me` | — | server computes "today" itself via `tehranTodayMarker()` / `tehranTodayWindow()` (Asia/Tehran, server-tz independent) |
| `GET /api/bootstrap` | response `today` | Tehran Jalali today via the central module (server-tz independent) |
| `POST /api/kiosk` | — | direction window = `tehranTodayWindow()` — true Tehran-midnight boundaries (S5) |
| `POST /api/import` (CSV) | `datetime` / `date`+`time` columns | full ISO **with timezone** → instant passthrough (`parseInstantParam`, S5 — timezone-less T-forms are now skipped like other unparseable rows, never host-local); naive `YYYY-MM-DD HH:MM[:SS]` → **Tehran wall-clock** (device-consistent); strictly validated — impossible dates/times and the historical DST gap hour are rejected; ambiguous DST times resolve deterministically to the later occurrence |
| `/api/ai` tool args | `date` | strict `parseDayParam` (S5) — invalid tool dates return a Persian error summary, never a wrong day |

## Weekday computation (S5)

The Persian weekday of a business day comes from the marker's own UTC date
(`faWeekdayOfMarker` / `jalali.ts:faWeekday` — UTC getters). The Iranian
weekend is **Friday** (`faWeekdayOfMarker === 6`); Saturday is a workday.
The legacy local-getter implementation was host-tz dependent (one day off on
hosts west of UTC — the engine's weekend suppression ran on the wrong days);
S5-A1 fixed it and the four-TZ matrix proves identical values everywhere.

## Intentional, documented behavior changes (stage 3 + S5)

1. **Invalid date params → 400** with a Persian message instead of a Prisma
   500 (`punches`, `holidays`, …) — v1.6.0.
2. **Kiosk direction window** → the Tehran day window (v1.6.0 comment's
   intent); **S5 makes the instants actually match the claim**
   (`tehranTodayWindow`, true Tehran midnight).
3. **CSV import naive timestamps** are Tehran wall-clock (v1.6.0); **S5**
   additionally requires an explicit timezone on ISO T-form `datetime`
   values (timezone-less T-forms used to parse host-locally and are now
   skipped, like every other unparseable row).
4. **Punch read filters** (`GET /api/punches`) moved from UTC-midnight
   marker windows to true Tehran-day windows (S5, D1 retired — see above).
5. **Write-path timestamps** (`punches` POST/PUT, `leave/requests` POST)
   require full ISO with an explicit timezone (S5) — timezone-less datetimes
   400 instead of silently becoming host-local.
6. **AI tool dates** go through strict `parseDayParam` (S5).

## Deliberately preserved (NOT changed) — with reasons

- The device raw-punch pipeline (`raw-import`, `raw-sync`, `fkattend`,
  `sbxpc`, `ebio`, `np-sync`) and the legacy `src/lib/jalali.ts` fixed-offset
  helpers (`tehranDayOf`, `tehranMinutes`, `faTime`, `tehranDate`) stay as
  the frozen device-side authorities — host-tz independent by construction
  (UTC getters + fixed +03:30), parity-proven by the test suite. The only
  legacy change in S5 is `faWeekday` (UTC getter — the engine's weekend
  logic demanded it; identical values on Tehran/UTC hosts).
- `legacy jalali.ts` Date-based display helpers (`toJalali`, `jFormat`,
  `jFormatFull`, `faDateTime`) are host-tz dependent **by legacy design and
  have ZERO live callers since S5** (the UI uses the central module; the
  seed migrated). They are documented in `docs/PHASE2-S5-AUDIT.md` (S5-A2)
  so the dependency can never silently re-enter the engine.
- `ebio.ts` missing-`Year` fallback now uses the Tehran year of now (S5-A8)
  — the device wall clock is Tehran time.
