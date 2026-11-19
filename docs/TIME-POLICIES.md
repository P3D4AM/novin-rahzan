# HozorPlus — Time & Business Policies (v1.7.0, incl. Phase 2 S5)

This document is the single source of truth for the approved business policies
and the time conventions of HozorPlus. The policies below are **approved
business decisions** — they are preserved here verbatim and are binding for all
future calculation work.

## Approved business policies

### Workday policy — `workday_policy = shift`

The business date of an attendance record is determined by the **start date
of the employee's applicable shift**.

Example:

- Shift starts Saturday at 22:00.
- Shift ends Sunday at 06:00.
- Punch at Sunday 02:30 belongs to **Saturday's** workday.

UTC calendar-day boundaries are NOT the business-day definition.

**Fallback (explicit, documented — never silently guessed):** for records
without an applicable shift (no ShiftAssignment, no defaultShift), for
malformed shift definitions, or for punches outside the shift window, the
business day is the **Tehran calendar day** of the punch instant.

**Shift assignment history:** the applicable shift is the one assigned to the
employee on the punch's date (ShiftAssignment for that date, else the person's
defaultShift AS OF that date). Today's shift is never silently applied to old
records. Historical recalculation must use the assignment state of the day
being recalculated.

**Dated default-shift history (S3 review fix 1):** the default shift is dated
through the `DefaultShiftHistory` table. For a day D the engine applies the
LATEST entry with `effectiveFrom <= D`; days before the earliest entry take
the earliest entry's shift (backward extension). A default change records its
effective date (`defaultShiftEffectiveFrom` API param, default today) — so
changing today's default never moves days before the effective date. Rows are
written ONLY for dates when a change was actually observed (personnel
creation, an API change, or the migration snapshot "as of migration day the
default was X"); no back-dated history is ever fabricated. The true sequence
of pre-history default changes is unknowable for legacy installs — the
earliest recorded entry (usually the migration snapshot) extends backward,
which reproduces the pre-upgrade behavior exactly, so the upgrade itself never
rewrites legacy rows. Explicit per-day `ShiftAssignment` ALWAYS has priority
over the dated default.

**Inactive shifts and history (S3 review fix 2):** `Shift.isActive = false`
gates NEW assignment selection only. The calculation path resolves shifts by
id regardless of `isActive` — a shift once assigned to a past date stays
resolvable for that date, so deactivating a shift never degrades its
historical rows to `NO_SHIFT`/fallback. Known limitation: EDITING a shift's
segment definition retroactively changes every historical day that used it
(the engine resolves by reference and does not version definitions);
recommended practice is to deactivate the shift and create a new one.

**Direction-aware window end (S3 review fix 3):** an OUT punch exactly at the
shift-window end closes that shift's session and belongs to that shift's
START DAY (window closed at the end for OUT; half-open `[start, end)` for IN
and direction-unknown punches). When the next day's shift starts exactly at
the same boundary instant, IN and OUT resolve independently and
deterministically: IN at the boundary opens the new shift's day, OUT at the
boundary closes the old shift's day.

Reference implementation: `src/lib/time.ts` → `resolveWorkday(instant, shift)`
(direction-less reference), `src/lib/workday.ts` →
`attributePunchWorkday(instant, shiftForDay, direction)`.

### Overtime policy — `overtime_policy = min_excess`

Actual overtime and approved overtime must remain **separate values**.

Normally payable overtime is the **lesser of actual and approved overtime**.
Actual overtime exceeding the approved amount must remain **visible** and may
be payable only through an **explicitly authorized excess-approval process**.

> NOT implemented in Phase 1 — preserved in architecture documentation only.
> The calculation engine itself is out of this phase's scope.

### Mission policy — `mission_policy = approved_credit`

An approved mission counts as **valid work time**, but **overlapping actual
attendance must never be counted twice** (no double-counting of mission time
and attendance time in the same interval).

> NOT implemented in Phase 1 — preserved in architecture documentation only.
> The calculation engine itself is out of this phase's scope.

## Time & storage conventions

- **Business timezone:** `Asia/Tehran` — via the timezone database (Intl).
  The fixed +03:30 offset is a last-resort fallback only (Iran has had no DST
  since 2022). Server-machine and browser timezones are never used for
  business-date calculations.
- **Instants** (`PunchRecord.timestamp`, `RawPunch.punchTime`,
  `LeaveRequest.startDateTime/endDateTime`, device bookmarks) are true UTC
  instants. `RawPunch.punchTimeRaw` preserves the device wall clock verbatim.
- **Day markers** (`DailyAttendance.date`, `ShiftAssignment.date`,
  `Holiday.date`, `OvertimeRequest.date`) are UTC-midnight values representing
  one Tehran **calendar day**. Date-only values are never timezone-shifted.
- **Jalali strings** (`Personnel.birthDate/hireDate/endDate`) are
  display-only.
- Real timestamps are never replaced by Jalali date strings.

## Phase 2 Stage S5 — host-timezone independence (binding conventions)

S5 mandate: the computation engine and the API routes never depend on the host
machine's timezone. Audited in `docs/PHASE2-S5-AUDIT.md`; enforced by
`scripts/test-time.ts` sections 20–24 and the four-TZ regression matrix
(`scripts/tz-matrix.sh` — UTC / Asia/Tehran / Europe/Vienna /
America/New_York, all suites green and byte-identical where deterministic).

1. **Weekday of a business day** always comes from the marker's own UTC date
   (`faWeekdayOfMarker` / the UTC-getter `jalali.ts:faWeekday`):
   شنبه=0 … جمعه=6, the Iranian weekend is FRIDAY (6) and Saturday is a
   workday. The old local-getter value was one day off on hosts west of UTC
   and moved the engine's weekend suppression to the wrong days (S5-A1).
2. **Read paths and the engine share ONE day-window convention**:
   `GET /api/punches` from/to filters use the same Tehran-midnight instant
   window the engine fetches punches over (`tehranDayRangeInstantWindow`,
   S5-A7). Audit defect **D1 is retired** — its premise ("the engine still
   uses UTC-midnight windows") expired when S3/S4 moved the engine to
   workday attribution + Tehran-midnight fetch windows.
3. **Kiosk/me "today" windows** are the TRUE Tehran day of now
   (`tehranTodayWindow` — [Tehran midnight, next Tehran midnight), S5-A6):
   Tehran 00:00–03:29 punches count for the CURRENT Tehran day's direction
   flip and today's list; the next day's early punches do not.
4. **Write-path timestamps are strict instants** (`parseInstantParam`,
   S5-A3/A4/A5): full ISO with an explicit timezone; timezone-less datetimes
   are rejected (400) or skipped (CSV row) — never parsed host-locally.
5. **AI tool dates** go through the strict `parseDayParam` (S5-A9).
6. The legacy `jalali.ts` Date-based display helpers (`toJalali`, `jFormat`,
   `jFormatFull`, `faDateTime`) remain frozen with **zero live callers** —
   their host-local behavior is recorded in the S5 audit so it never
   re-enters a live path (S5-A2).

## Where the layers live

| Layer | File | Role |
| --- | --- | --- |
| Central authoritative module | `src/lib/time.ts` | Tehran parts/instants/windows, markers, Jalali conversion & validation, `resolveWorkday`, strict day/instant parsing, day-window derivation (S5), formatting |
| Legacy compat (frozen) | `src/lib/jalali.ts` | fixed-offset helpers still consumed by the attendance engine & raw device pipeline (`faWeekday` now UTC-getter based — S5-A1) |
| UI wrappers | `src/components/app/jalali-ui.tsx` | client-side Jalali controls; delegates to the central module |

The attendance engine's punch fetch and day attribution have used TRUE
Tehran-midnight windows since S3/S4 (`tehranDayWindowOfMarker` +
`attributePunchWorkday`); the D1 read-path exception is retired (S5) — read
paths, kiosk/me windows and the engine now share the same convention.

## Phase 2 Stage S4 — calculation completeness (binding conventions)

Implemented in `src/lib/attendance.ts` (`computeDaily`); every rule below is
pinned by `scripts/test-attendance.ts` sections 36–48 and
`scripts/test-attendance-db.ts` sections M–P.

### Late / Early — real shift-window instants

Late/early compare **true UTC instants** against this workday's shift window
(`shiftWindowInstants`): night shifts, multi-segment shifts (first-segment
start / last-segment end) and Jalali month/year boundaries are exact; the old
±1440 minutes-of-day wrap heuristics are gone. A punch before the window
start is simply early (late 0); a punch entirely before the window can never
fabricate early-leave. `lateGraceMin`/`earlyGraceMin` keep their meaning.

### Approved leave attached to the shift window

A **normal approved leave covering the window start** moves the arrival
deadline to the leave's end (lateness is excused for the leave's duration);
one **covering the window end** moves the departure deadline to the leave's
start. A leave ending exactly at the start (or starting exactly at the end)
does not excuse anything. This is the audited expectation (PHASE2-STAGE1-AUDIT
§3 row 18, D8) — not a newly invented rule.

### Leave × work overlap (no double counting)

An approved **normal-leave minute that overlaps actual attendance is counted
exactly once — as leave**: `workedMinutes` reports attendance minutes NOT
covered by approved leave (`worked + leaveMinutes` are never double-exposed
for one interval). Real punch data is never faked or removed (firstIn/lastOut/
pairs stay verbatim); overlapping leaves are unioned; mission and leave stay
disjoint; OT feeds the carved worked value.

### Absence determination timing

Absence becomes determinable when the day's **real determination window** has
fully passed: the shift window END on a shift day, the Tehran calendar-day
end otherwise. The previously hardcoded undocumented −6h buffer is removed.
HOLIDAY/OFF/LEAVE/MISSION keep their precedence over ABSENT.

### Provenance

A day computed from **edited punches** carries the stable note code
`EDITED_PUNCHES:n` (n = edited punches among that workday's attributed
inputs) after the pairing anomaly codes and before the attribution codes; no
free text from `editReason` ever enters the note (audit detail stays in
AuditLog/PunchRecord).

### Statuses

The never-implemented `IGNORED` placeholder was removed from the schema
comment (no producer or consumer existed anywhere). `MISSION` never hides
punch truth (punch-based statuses win whenever punches exist); true full-day
LEAVE keeps precedence over MISSION.

### Deliberately NOT changed (open business ambiguities — decisions still required)

- **A4 (holiday/off-day OT):** ALL valid work time on holiday/off days stays
  actual OT (no minimum/permission gate added); `payable` still = min(actual,
  approved).
- **A5 (break accounting):** `breakMin` keeps reducing the shift STANDARD
  only — real worked time from punch pairs is never reduced by it.
- **Mid-shift leave late/early effect:** none (no documented rule exists;
  the current behavior is pinned by test and left untouched).
- **Mission attached to the shift start and lateness:** a mission does NOT
  excuse lateness (mission time is credited as work time by its own policy;
  no documented late-exemption exists — recorded as an open question).
