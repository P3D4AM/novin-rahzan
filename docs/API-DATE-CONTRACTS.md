# HozorPlus — API Date/Time Contract (Phase 1, v1.6.0)

Single reference for what every date/time field on the API means and how it is
parsed/serialized. Enforced by `src/lib/time.ts` (the central layer) and tested
in `scripts/test-time.ts` (sections 14 & 16).

## Field classes

| Class | Meaning | Wire format | Server behavior |
| --- | --- | --- | --- |
| **instant** | a real point in time | full ISO 8601 with timezone (`2026-11-16T23:00:00.000Z`) | stored as a true UTC instant; never re-interpreted |
| **business day (marker)** | one Tehran calendar day | `YYYY-MM-DD` **date-only** or marker ISO (`…T00:00:00.000Z`) | parsed by `parseDayParam` → UTC-midnight marker; **never timezone-shifted** |
| **Jalali string** | display-only Jalali text | `1405/08/26` | validated by `parseJalaliInput` (Persian errors); stored as text |

**`parseDayParam` contract (v1.6.0 review fix 2 — strict):** date-only strings
parse as UTC midnight (ES spec — no accidental UTC conversion, no
browser-local shift); full ISO timestamps **with an explicit timezone** (`Z`
or `±HH:MM`, strict component ranges, leap-aware calendar) are floored to
their UTC day (legacy-compatible); **timezone-less datetimes
(`2026-11-17T00:30`) are REJECTED** — ECMA-262 treats them as host-LOCAL
time, which silently shifted the parsed day with the server's timezone —
and so are basic-format offsets (`+0330`), space-separated datetimes and
every non-ISO form; anything rejected → clean 400. Verified identical under
TZ=UTC / America/New_York / Asia/Tehran / Pacific/Kiritimati
(`scripts/test-time.ts` section 19).

## Per-endpoint contract

| Endpoint | Fields | Class / rule |
| --- | --- | --- |
| `GET /api/punches` | `from`, `to` | date-only business days; instant filter `[from, to+1)` via `markerRangeInstantWindow` — **same UTC-midnight day-window convention the attendance engine uses**; junk → 400 (was 500) |
| `POST/PUT /api/punches` | `timestamp` | instant (full ISO); `recalcDayForPerson` attribution unchanged |
| `GET /api/attendance` | `from`, `to` | date-only business days → marker range; junk → 400 |
| `GET /api/reports` | `from`, `to` | date-only business days → marker range (Jalali month pickers map 1:1) |
| `POST /api/overtime` | `date` | date-only business day → marker (explicit `parseDayParam`, never tz-shifted) |
| `POST /api/holidays` | `dateJalali` / `date` | Jalali string → marker (validated) / date-only marker |
| `GET /api/holidays`, `/api/assignments`, `/api/rotations` | `from`, `to`, `date` | date-only business days → markers; junk → 400 |
| `POST /api/leave/requests` | `startDateTime`, `endDateTime` | instants (full ISO) |
| `GET /api/me`, `POST /api/me` | — | server computes "today" itself via `tehranTodayMarker()` (Asia/Tehran, server-tz independent) |
| `GET /api/bootstrap` | response `today` | Tehran Jalali today via the central module (server-tz independent) |
| `POST /api/kiosk` | — | direction window = Tehran-day marker window `[marker, marker+1)` — same convention as `/api/me` POST |
| `POST /api/import` (CSV) | `datetime` / `date`+`time` columns | full ISO (`…T…`) → instant passthrough; naive `YYYY-MM-DD HH:MM[:SS]` → **Tehran wall-clock** (device-consistent; identical on a Tehran-tz server, correct elsewhere); **strictly validated** (v1.6.0 review fix 1) — impossible dates/times and the historical DST gap hour are rejected, so unparseable rows are skipped, never normalized into a wrong-but-plausible day; ambiguous DST times resolve deterministically to the later occurrence |

## Intentional, documented behavior changes (stage 3)

1. **Invalid date params now return 400** with a Persian message instead of a
   Prisma 500 (`punches`, `holidays`) — same class of hardening the attendance
   route already had.
2. **Kiosk direction window** changed from the UTC calendar day to the Tehran
   day marker window — Tehran 00:00–03:29 punches now belong to the *current*
   Tehran day, exactly matching `/api/me` and the engine's day attribution.
3. **CSV import naive timestamps** are interpreted as Tehran wall-clock instead
   of server-local wall-clock. On the supported deployment (Tehran-tz Windows
   server) the result is bit-identical; on any other server it becomes correct.

## Deliberately preserved (NOT changed) — with reasons

- **Punch-instant filter windows remain UTC-midnight marker windows** (`[marker, marker+1)`)
  rather than true Tehran-midnight windows. The attendance engine
  (`src/lib/attendance.ts`, protected this phase) attributes punches with the
  same UTC-midnight convention; switching read paths alone would desync
  تردد / کارکردها / گزارش‌ها for punches between Tehran 00:00–03:30. The
  engine migration to `resolveWorkday` + `tehranDayWindowOfMarker` is a
  dedicated future phase; the primitives are ready and tested (`src/lib/time.ts`).
- The device raw-punch pipeline (`raw-import`, `raw-sync`, `fkattend`,
  `sbxpc`, `ebio`, `np-sync`) and the legacy `src/lib/jalali.ts` engine layer
  are untouched (proven by the stage diffs and by parity tests).
