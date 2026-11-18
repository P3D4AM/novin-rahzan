# HozorPlus — Time & Business Policies (Phase 1, v1.6.0)

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

## Where the layers live

| Layer | File | Role |
| --- | --- | --- |
| Central authoritative module | `src/lib/time.ts` | Tehran parts/instants/windows, markers, Jalali conversion & validation, `resolveWorkday`, formatting |
| Legacy compat (frozen) | `src/lib/jalali.ts` | fixed-offset helpers still consumed by the attendance engine & raw device pipeline (protected subsystems — unchanged in Phase 1) |
| UI wrappers | `src/components/app/jalali-ui.tsx` | client-side Jalali controls; delegates to the central module |

The attendance engine's own day windows (UTC-midnight instant windows) are a
known, deliberately preserved limitation (audit defect D1) until a dedicated
engine phase — read paths must stay consistent with the engine in the meantime.
