# Phase 2 Stage 1 — Attendance Engine Audit (read-only)

Basis: `develop @ 41c958ad` (= tag `v1.6.0-r1`, release 2026-11-18).
Method: **read-only** — no code, no schema, no data was changed in this stage.
Every file referenced below was read in full at that commit.

Audited surfaces:

| Surface | Files |
| --- | --- |
| Engine | `src/lib/attendance.ts` (278 lines) |
| Time layers | `src/lib/time.ts` (586), `src/lib/jalali.ts` (140) |
| DB models | `prisma/schema.prisma` (461) — Personnel, Shift/ShiftSegment/ShiftRotation/ShiftAssignment, Holiday, PunchRecord, DailyAttendance, LeaveType/LeaveRequest, OvertimeRequest, Setting, AuditLog, RawPunch chain |
| Callers | `scripts/recalc.ts`, `src/app/api/{attendance,punches,kiosk,me,leave/requests,overtime,holidays,assignments,rotations,import,reports,ai,bootstrap}/route.ts`, `src/lib/{notify,raw-sync,ebio}.ts`, `src/components/app/{attendance,reports,dashboard,mycard,shell,types}.tsx` |
| Tests & data | `scripts/test-{time,raw-import,bridge-supervisor,scheduler,live-notif}.ts`, `scripts/seed.ts` |

---

## 1. Current architecture (who computes what)

**The entire calculation engine is one module**, `src/lib/attendance.ts`, with three entry points:

1. `recalcRange(personIds, start, end)` — the only writer of `DailyAttendance`; loops persons × days, upserts one row per (person, day).
2. `recalcDayForPerson(personId, ts)` — thin wrapper: `tehranDayOf(ts)` → `recalcRange` for that one day.
3. `computeDaily(...)` — pure per-day computation (pairing, late/early, OT, leave, status).

Everything else **reads** `DailyAttendance` (reports, attendance UI, dashboard, mycard, notify/scheduler notifications, ai). The calculation logic itself is *not* duplicated in APIs/UI — summaries are pure aggregations (`reduce`) of engine output. Good: the "one testable place" requirement is structurally satisfied; bad: the one place is small, heuristic and untested (§5 D14).

**Trigger map (who recalcs when):**

| Trigger | Path | Range semantics |
| --- | --- | --- |
| Kiosk / `/api/me` punch | `recalcDayForPerson(personId, ts)` | one day (via `tehranDayOf`) |
| Manual punch create/edit/delete | `punches` route → `recalcDayForPerson` (old **and** new timestamp on edit) | one day each |
| Device import (raw pipeline) | `raw-sync` → per touched day `recalcDayForPerson` (day from `tehranDayOf(punchTime)`) | per punch day |
| EBIO XML ingest | `ebio.ts` → `recalcDayForPerson` | per punch |
| Leave request create/approve/reject/cancel | `leave/requests` route → `recalcRange` over the leave span (UTC-day floored) | span |
| Overtime approval | `overtime` route PUT | **nothing — no recalc, engine never reads OvertimeRequest** |
| Holiday create/delete | `holidays` route → `recalcRange(all persons, date)` | one day |
| Shift assignment / rotation apply | `assignments`/`rotations` routes → `recalcRange` | span |
| CSV import | `import` route → `recalcRange` over import span | span |
| Manual recalc | `POST /api/attendance` / `scripts/recalc.ts` | span (note: attendance route adds +1 day beyond `to`; §5 D12) |

**Shift resolution for a day** (inside `recalcRange`): `ShiftAssignment(personId, date)` → its shift; else person's `defaultShift` (suppressed on holiday/Friday); else off. **`ShiftRotation` is not read by the engine** — the rotations API materializes a rotation into `ShiftAssignment` rows (upsert per day), which the engine then reads. That is a clean design (rotation = write-time expansion, engine sees plain assignments).

---

## 2. Answers to the eight audit questions

### 2.1 Current in/out pairing algorithm

`pairPunches` (`attendance.ts:80-90`):

- sorts the day's punches by timestamp;
- `firstIn` = first, `lastOut` = last (only when ≥ 2 punches);
- `worked` = **sum of consecutive pairs by position**: (p0,p1) + (p2,p3) + …

So pairing is purely positional alternation on the sorted list. **`PunchRecord.direction` (IN/OUT/AUTO/UNKNOWN — set to IN/OUT by kiosk/me/ebio, AUTO by device projection and manual punches) is never consulted.** Consequences:

- duplicate IN, duplicate OUT, out-of-order and simultaneous punches are all absorbed silently into whatever alternating interpretation the sorted order suggests;
- an **odd count** silently drops the last punch from `worked` (it is not counted as an open IN either);
- a single punch → `INCOMPLETE`, worked 0, `lastOut` null;
- out-without-in is indistinguishable from in-without-out (no direction awareness);
- identical timestamps from different sources can both survive (`@@unique([personId, timestamp, source])` dedupes only per source; the raw-import projection dedupes per person+instant in-memory per batch).

There is **no pairing state machine, no per-pair record, no reason codes, no quarantine for unpairable punches** — everything collapses into `{firstIn, lastOut, worked}`. The raw layer's excellent quarantine/deduze discipline (RawPunch, RejectedRecord) stops at `PunchRecord`: nothing equivalent exists at the pairing level.

### 2.2 Duplicate / incomplete / invalid / out-of-order punches

- **Device re-import**: deduped upstream (RawPunch unique key on rawFingerprint+dupIndex; P2002 fallback). Re-importing the same device log creates no new rows → recalcs see identical data → **idempotent for device data** (proven by test-raw-import).
- **Web/kiosk/manual**: unique (person, timestamp, source) prevents exact dupes only per source.
- **Incomplete** (1 punch): `INCOMPLETE` status, worked 0.
- **Out-of-order / duplicates / ambiguous**: no special handling — folded into the positional pairing (§2.1). No flag, no note, no visibility.
- **Invalid instants**: never reach `PunchRecord` (validated at raw-import/CSV/kiosk layers).
- `edited`/`editReason` on `PunchRecord` carry manual-edit provenance (audit trail in `AuditLog`), but the engine never surfaces them in `DailyAttendance.note` (always null) — the daily row does not say "based on edited punches".

### 2.3 Night / cross-midnight shifts

- Shift model: `ShiftSegment.startMin/endMin` are **minutes from Tehran midnight; `endMin <= startMin` means crosses midnight**. `shiftWindow`/`shiftStandardMinutes` handle the wrap (+1440).
- **BUT the engine never computes the shift window as instants.** Punch selection is the UTC-midnight window of the *calendar* day marker `[d, d+1)`; a night shift (Sat 22:00 → Sun 06:00) has its Sunday-morning OUT punch (Tehran 00:00–03:29 = UTC previous day 20:30–23:29) land in **Saturday's** UTC window — accidentally often right for night shifts, but for the wrong reason (see D1/D2: the 03:30–06:00 Tehran tail of the shift falls in *Sunday's* window and is attributed to Sunday).
- Late/early handle the wrap with ±1440 **heuristics** (`attendance.ts:114-123`): `effectiveIn = inMin < win.end && inMin < dayStartMin && win.start > 720 ? inMin + 1440 : inMin` and `effectiveOut = outMin < win.start && win.end > 1440 ? outMin + 1440 : outMin`. These have proven false positives (D13) and no shift-start-date semantics.
- Cross-Jalali-month/year works only accidentally (markers are plain dates) — the day the punch *belongs to* is still the calendar day, not the shift start day.

### 2.4 Workday determination

**`workday_policy = shift` is NOT implemented.** The engine's day attribution is:

- `recalcRange` day = UTC calendar day marker (loop `d` over markers);
- punch attribution = `timestamp ∈ [d, d+1)` **UTC-midnight window** — i.e. the punch is attributed to the Tehran calendar day *shifted*: Tehran 00:00–03:29 lands on the previous marker (documented as D1 in TIME-POLICIES.md);
- `recalcDayForPerson(ts)` uses `tehranDayOf(ts)` (fixed +03:30 Tehran calendar day) — **a different convention than the window it then queries** (D2: for Tehran 00:00–03:29 punches the trigger day does not contain the punch).

`resolveWorkday` (the approved-policy implementation, tested by 18 suite sections) is **called by nothing** in the engine. The `time.ts` header explicitly documented the engine as a protected frozen consumer of `jalali.ts` during Phase 1 — Phase 2 is precisely the migration point.

### 2.5 Where worked/late/early/OT live

All in `computeDaily` (`attendance.ts:93-168`) — single location, no duplication elsewhere:

- `workedMinutes` = pairPunches sum (§2.1); **break minutes are NOT deducted from worked** (they are only subtracted inside `shiftStandardMinutes`);
- `lateMinutes` = max(0, effectiveIn − win.start − lateGrace) — first punch only, vs the whole-shift window start (not per-segment);
- `earlyLeaveMinutes` = max(0, win.end − effectiveOut − earlyGrace) — last punch only, vs whole-shift window end;
- `overtimeMinutes`:
  - holiday/off days: **all worked = OT** (regardless of permission, shift, or duration);
  - otherwise: `shift.overtimeEligible && worked > standard + 30` → `worked − standard` — the **+30 min threshold is a hardcoded literal** (line 130), no rounding, no Setting, no OvertimeRequest consultation (see D7);
- status (priority order): `LEAVE` (≥480 approved leave minutes **and** zero punches) → `HOLIDAY` (zero punches) → `OFF` (zero punches, no shift) → `ABSENT` (zero punches and the UTC day fully passed **+ 6h hardcoded**) → `PENDING` (today) → `INCOMPLETE` (1 punch) → `PRESENT`.
  `MISSION` and `IGNORED` (documented in the schema comment) are **never produced**.

### 2.6 Leave & mission effect on calculations

- `leaveForDay` (`attendance.ts:56-78`): APPROVED requests overlapping the **UTC day window**; minutes clamped to 1440; `leaveTypeId` = **the last overlapping request's type (no ordering → nondeterministic when several overlap)**.
- Effects today: `leaveMinutes` stored, `LEAVE` status only when no punches at all; **leave minutes are never subtracted from worked/late** — an approved hourly leave (10:00–13:00) with punches 08:00–17:00 reports worked 540 **and** leave 180 (double exposure); late during leave is not excused.
- **Mission**: there is **no mission entity**. The seed models mission as a `LeaveType` named `مأموریت` (`paid: true, countsTowardsBalance: false`), so today a mission behaves exactly like leave: absence with leaveMinutes, **not** valid work time; no double-counting protection (because nothing is counted); `mission_policy = approved_credit` is unimplemented.

### 2.7 Paths depending on the current logic (blast radius)

Everything that reads `DailyAttendance` (or its shape): `attendance`/`reports` APIs + pages, `dashboard`, `mycard`, `shell` status maps, notification rules (`notify.ts` LATE on checkin; `scheduler.ts` daily ABSENT sweep at 12:30 Tehran), `ai` route prompts, xlsx export (monthly summary), `kiosk`/`me` direction windows (Tehran-marker convention — **already** different from the engine's attribution, see D2), `punches` GET instant filter (`markerRangeInstantWindow`, deliberately aligned to the engine's UTC window per API-DATE-CONTRACTS.md).

### 2.8 Confirmed defects, risks, ambiguities

**Confirmed defects (with evidence, read-only — to fix in later stages):**

- **D1 (documented, deliberate until Phase 2):** engine day attribution = UTC-midnight windows; Tehran 00:00–03:29 punches are shown on the previous day. All read paths were aligned to it on purpose.
- **D2:** `recalcDayForPerson` uses `tehranDayOf` (Tehran calendar day) but `recalcRange` selects punches with the UTC window → for Tehran 00:00–03:29 punches the recalculated day **does not contain the punch that triggered it**; kiosk (`me`, kiosk) uses the Tehran-marker window for direction toggling → direction bookkeeping and day attribution disagree in 00:00–03:29.
- **D3:** pairing ignores `direction` entirely; odd punch counts silently drop the last punch; repeated IN/OUT out-of-order all collapse into positional alternation; no unpairable/ambiguous report.
- **D4:** `faWeekday` uses `Date.getDay()` — **host-machine timezone weekday**. `isFriday` (weekend logic, `attendance.ts:213`) is host-tz dependent: on an America/New_York server the weekend can shift by a day. Direct violation of the host-tz independence goal.
- **D5:** the engine runs on `jalali.ts` fixed +03:30 (no tz database) — wrong by 1h for pre-2022 DST-era instants; `time.ts` (tz-database, tested) exists and is unused by the engine.
- **D6:** `resolveWorkday` unused ⇒ `workday_policy = shift` not applied (night-shift Sunday 02:30 punch → Sunday's row, not Saturday's).
- **D7:** OT policy gaps: hardcoded +30 threshold; no rounding rule; holiday/off-day worked → 100% OT without any minimum; **OvertimeRequest never consulted** (payable-vs-approved separation of `min_excess` unimplemented; approving an OT request triggers no recalc).
- **D8:** leave: overlapping-hourly-leave minutes are double-exposed with worked; `leaveTypeId` nondeterministic across overlapping requests; full-day threshold `8*60` hardcoded; hourly leave does not excuse late or reduce worked.
- **D9:** mission: no entity, no credit-as-work-time, `MISSION` status never produced (§2.6).
- **D10:** `recalcRange` loads only `isActive: true` shifts — deactivating a shift retroactively changes historical days recalculated later (assignment references shiftId but resolution silently yields "no shift").
- **D11:** ABSENT timing: hardcoded `- 6h` after UTC day end; PENDING semantics per host clock `now` only.
- **D12:** off-by-one inconsistencies across callers: `attendance` POST and `import`/`holidays` pass `to + 1` into an already-inclusive loop (one extra day recalculated); others pass `to` directly.
- **D13:** the ±1440 late/early heuristics produce false results in legal-but-unhandled shapes (e.g. evening shift 15:00–23:30 plus a stray 02:30 Tehran punch → `effectiveIn += 1440` → ~11h false late).
- **D14:** **zero unit coverage of engine semantics** — the only attendance-related checks are smoke-level (test-live-notif: "recalculated rows exist"; scheduler: device-loop effects). No test asserts pairing, late/early/OT numbers, leave interplay, overnight attribution, or idempotency of recalcRange.
- **D15:** `DailyAttendance` has no provenance (no `note`, no link to which punch rows produced it, no "based on edited punches" flag) — traceability of manual corrections ends at PunchRecord/AuditLog.

**Business ambiguities (needs a decision; not guessed):**

- **A1 mission source of truth:** dedicated entity vs the existing "mission as a LeaveType" seed convention. `approved_credit` + "counts as valid work time, never double-counted" can be built on either, but the data source and the status (`MISSION` vs `LEAVE`) must be decided. Recommendation prepared in §6/S3 — decision required before schema work.
- **A2 `min_excess` parameters:** threshold (current literal 30 min), rounding (none), and the relationship of payable OT to `OvertimeRequest` approvals (engine currently reports actual excess only; TIME-POLICIES.md says payable = min(actual, approved), excess visible via explicit process). Existing `Setting` rows `ot_rate_normal/friday/holiday` are payroll rates — unused by the engine today; scope must be confirmed.
- **A3 weekend definition:** Friday-only is hardcoded; multi-day/parametric weekends not modeled.
- **A4 punches on off/holiday days:** today all worked counts as OT (A2-adjacent: is there a minimum? permission?).
- **A5 break accounting:** worked never subtracts breakMin while standard does — intentional? (If not decided, keep and document.)

---

## 3. Behavior matrix — current vs expected (per approved policies) vs required test

| # | Scenario | Current behavior | Expected (Phase 2 target) | Required test |
| --- | --- | --- | --- | --- |
| 1 | valid IN+OUT day shift | worked = span; PRESENT | same, but via explicit pair records with instants; host-tz independent | T1 normal shift |
| 2 | night shift, out next morning | OUT at Tehran 00:00–03:29 → previous day's row; 03:30–06:00 tail → Sunday's row (split!) | whole shift attributes to **shift start day** (`resolveWorkday`); worked/late/early across midnight | T2 night cross-midnight |
| 3 | shift crossing Jalali month/year boundary | calendar-day attribution (accidental) | shift-start-day attribution; marker math correct across Esfand 29/30, leap | T3 cross month/year |
| 4 | multiple IN/OUT pairs per day | positional pairs (p0,p1)+(p2,p3); no pair records | deterministic pair records (direction-aware); per-pair worked; sum | T4 multi-pair |
| 5 | duplicate IN | absorbed silently | identified, reported (pairing anomaly), no silent data change | T5 duplicate IN |
| 6 | duplicate OUT | absorbed silently | identified + reported | T6 duplicate OUT |
| 7 | IN without OUT | odd count → last punch ignored by worked; INCOMPLETE | open pair (open-IN) semantics documented; INCOMPLETE | T7 missing OUT |
| 8 | OUT without IN | same as 7 (no distinction — direction unused) | open-OUT / orphan reported distinctly | T8 orphan OUT |
| 9 | out-of-order punches | sorted, positional | sorted + direction-aware state machine; anomalies flagged | T9 out-of-order |
| 10 | simultaneous punches | both kept (source differs) → adjacent pair 0 min | identified as simultaneous; deterministic handling | T10 simultaneous |
| 11 | stray punch 02:30 Tehran (evening shift) | false late up to +1440 wrap (D13) | outside-shift-window punch: no late/early distortion, reported as out-of-window | T13 regression |
| 12 | late / early-leave | vs whole-shift window ± grace, wrap heuristics | per shift window on shift-start-day; grace from shift; no heuristics | T9/T2 |
| 13 | OT below threshold | 0 (threshold literal 30) | documented threshold from settings/decision (A2) | T10 OT below/above |
| 14 | OT above threshold | worked − standard (actual only) | actual reported **and** payable = min(actual, approved) once A2 decided | T10 |
| 15 | OT on holiday/off | 100% of worked = OT | per approved policy decision (A2/A4) | T13 holidays |
| 16 | approved mission | leave-like row (LEAVE) | mission = valid work-time credit, no double count, `MISSION` status | T11 approved mission |
| 17 | pending (unapproved) mission | PENDING leave contributes nothing (leaveForDay filters APPROVED) | same — `approved_credit` already satisfied for the leave-shaped path | T11 unapproved |
| 18 | hourly leave | double exposure with worked (D8) | leave minutes carved out of worked/late per policy | T12 hourly + daily leave |
| 19 | daily leave + no punches | LEAVE (threshold 480 hardcoded) | same semantics, threshold from shift standard (decision) | T12 |
| 20 | holiday (no punches, no shift) | HOLIDAY / OFF | same | T13 |
| 21 | Friday weekend | host-tz dependent weekday (D4) | Tehran-based weekday via tz database | T16 host-TZ matrix |
| 22 | manual punch edit | old+new day recalc; provenance only in AuditLog | recalc affected workdays (shift-aware); DailyAttendance records provenance | T14 manual edit |
| 23 | device re-import same log | deduped (raw layer) → idempotent | unchanged (protected pipeline) | raw-import suite (existing) |
| 24 | recalc rerun (idempotency) | upsert per (person, day) — deterministic in content but N+1 queries | identical output on rerun (bit-equal), including overnight shift days | T15 idempotency |
| 25 | engine under host TZ ≠ Tehran | weekend wrong (D4); day math fixed-offset (D5) | identical results under UTC/NY/Tehran/Kiritimati hosts | T16 TZ matrix |
| 26 | OvertimeRequest approval | no effect, no recalc | participates per A2 decision; triggers recalc | T10/T11 |

---

## 4. What stays protected (unchanged in Phase 2)

- The device raw pipeline end-to-end (`raw-import`, `raw-sync`, `fkattend`, `sbxpc`, `ebio`, `np-sync`, bridge supervisor) — its dedupe/quarantine/audit discipline is the model the pairing layer should imitate, not be rewritten with.
- `src/lib/time.ts` (the Phase 1 core) — consumed, not modified (unless a pure additive export is needed and tested).
- The device-day convention inside `raw-import`'s touchedDays (frozen with its own regression suite; the engine's changed attribution is consumed via the recalc trigger, which Phase 2 will route through the same workday resolution so no contract is broken).
- Storage convention: markers stay UTC-midnight representations of Tehran days (TIME-POLICIES.md) — Phase 2 changes *attribution*, not the storage format.

---

## 5. Proposed stage plan (small, reviewable commits — awaiting approval of this audit before starting)

- **S2 — pairing & validation layer (engine-internal, no schema change):** direction-aware deterministic state machine over the day's punches → explicit pair records (in-memory result objects + anomaly reasons); covers scenarios 4–11; new suite `scripts/test-attendance.ts` sections A (pure computeDaily-level tests, mock DB like test-raw-import's harness).
- **S3 — approved policies wired into computeDaily:** `workday_policy = shift` (resolveWorkday-based attribution for punch→day AND shift windows as instants), `overtime_policy = min_excess` (threshold from Setting with the current 30-min value as default; actual vs payable split once A2 answered), `mission_policy = approved_credit` (per A1 decision; likely: mission = LeaveType-flagged credit counting toward worked with overlap deduction — no double counting).
- **S4 — calculation completeness:** per-pair worked, leave overlap carving, holiday/off behavior, late/early per real shift instants, breaks decision (A5), ABSENT timing, statuses (MISSION/IGNORED or removal from comment), provenance on DailyAttendance (note/versioning for auditability).
- **S5 — host-TZ independence & read-path alignment:** engine day windows = `tehranDayWindowOfMarker`/shift windows via `time.ts` everywhere; `faWeekday` replaced by marker-based weekday (D4); kiosk/me direction windows and punches/attendance/reports filters re-aligned to the new attribution contract; API-DATE-CONTRACTS.md updated (the D1 deferral note is retired).
- **S6 — comprehensive suite:** T1–T16 as sections of `scripts/test-attendance.ts` (16 scenarios × host-TZ matrix where relevant); keep time/raw/bridge/scheduler/live-notif green; add engine parity checks under 4 host TZs.
- **S7 — data compatibility & release:** no destructive migration (additive columns only if S3/S4 require provenance/mission storage); recalcRange backfill = the existing manual recalc over the affected range; before/after comparison report (export old rows, recalc, diff summary); version bump → `v1.7.0` (minor: new policy semantics, additive); tag + GitHub Release + Windows ZIP from the final commit (per repo release policy; NP756+ hardware validation explicitly not performed here — no fake claims).

---

## 6. Test inventory (as of this audit — real, executed)

| Suite | Command | Last recorded result (v1.6.0-r1 session, 2026-11-18) |
| --- | --- | --- |
| time infrastructure | `npm run test:time` | 68 assertions / 0 failed + 18 sections / 0 failed (exit 0) |
| raw import | `npm run test:raw` | 164 passed / 0 failed (exit 0) |
| bridge supervisor | `npm run test:bridge` | 25 passed / 0 failed (exit 0) |
| scheduler | `npx tsx scripts/test-scheduler.ts` | not rerun in this audit stage (device-loop scope; to be rerun in S6) |
| live notifications | `npx tsx scripts/test-live-notif.ts` | not rerun in this audit stage (to be rerun in S6) |
| **attendance engine** | — | **does not exist (D14)** |

Engine-affecting checks found in existing suites: 1 smoke check (live-notif: rows exist after import). **No assertion anywhere pins pairing/late/early/OT/leave numbers.**

---

## 7. Stage-1 result summary

- No code, schema, or data changed (read-only stage, as mandated).
- The engine is structurally centralized but semantically minimal: positional pairing, heuristic ±1440 late/early, hardcoded OT threshold, no direction use, no shift-day attribution, fixed-offset time, host-tz weekend, no engine tests.
- The three approved policies are all currently unimplemented in executable logic (D6/D7/D9) — Phase 2 stages S2–S5 implement them; the primitives (`resolveWorkday`, tz-database core, marker system) are ready and tested.
- Open business decisions required before S3: A1 (mission data source), A2 (`min_excess` threshold/rounding/payable-vs-approved), A3 (weekend config), A5 (break accounting).
- **Next step: await audit approval, then S2 (pairing) begins.**
