# Phase 3 — Shift & device form fixes (items 9–12)

Scope: `فاز ۳` of the six-phase fix plan (items 9, 10, 11, 12). Data-correctness first: the time picker is a **minutes contract** end-to-end, and the same validator runs in the form **and** on the server.

## Item 9 — Shift time control (`ShiftTimePicker`)

**Root cause of the reported defects (evidence):**
- The old form parsed the raw text of a plain input on EVERY keystroke:
  `const [h, m] = v.split(':').map(Number)` — typing `830` (before the colon) evaluated to `h=830` → `startMin = 49 800`. The stored value was corrupted **while typing**; reopening the form showed the garbage («مقدار واقعی پس از بازکردن مجدد فرم حفظ نمی‌شد»).
- Nothing validated ranges: `25:99` was silently stored.
- There was no list-based hour/minute selection at all.

**The fix — three layers, ONE pure module (`src/lib/shift-time.ts`):**

| Layer | File | Guarantee |
| --- | --- | --- |
| Pure core | `src/lib/shift-time.ts` | `parseHHMM` (accepts `8`, `8:3`, `830`, `0830`, Persian digits; rejects hour>23 / minute>59 with Persian reasons), `minutesToHHMM`, `crossesMidnight`, `shiftSegmentSpan` (engine-parity: end<start wraps +1440, end==start = 24h), `validateShiftSegments` |
| UI control | `ShiftTimePicker` in `src/components/app/jalali-ui.tsx` | manual HH:MM typing + Popover lists (ساعت ۲۴ / دقیقه ۶۰), live Persian error, **never propagates an unparseable draft** (invalid → error, revert on blur). Value contract: `minutes: number` |
| Server gate | `src/app/api/shifts/route.ts` | POST/PUT run `validateShiftSegments` — an invalid segment can no longer be stored even via direct API call. PUT validates **before** deleting old segments (no half-written state) |

Segment row in the dialog now shows: labeled شروع/پایان, «شب‌گذر — عبور از نیمه‌شب» / «۲۴ ساعته» badges, مدت بازه + کار مؤثر. Save button is disabled while any بازه is invalid; the shift list cards label night-crossing segments.

Engine parity: `shiftSegmentSpan` is byte-for-byte the arithmetic of `src/lib/attendance.ts` line 73 — night shifts and 24h shifts keep their exact historical meaning.

## Stage 7 review fix (draft-validation defect, strict parser, breakMin, atomic PUT)

The Stage 7 review found one hard defect and three contract gaps; all four are closed on top of the same three-layer design (no parallel state, no rewrite):

1. **Draft validation did not reach the parent form** (the review's reproduction: stored `08:30`, user types `25:99` → the picker showed its own error but `form.segments` kept the valid `08:30`, the parent validator kept passing, the ذخیره button stayed enabled — the stale previous value could be saved). Fix: `ShiftTimePicker` now reports the edit-in-progress validity via `onValidityChange`; the parent aggregates ALL pickers in one `DraftValidityMap` (pure helpers in `src/lib/shift-time.ts`: `draftValidity` / `setDraftValidity` / `hasInvalidDrafts`), disables ذخیره while ANY picker is invalid, AND `save()` re-checks both gates on its own (button-independent). Segment rows are keyed by a stable local `_id` (removal unmounts exactly that row's pickers and releases their flag — an invalid draft can never transfer to another row; every form (re)load gets fresh ids + a cleared map → no stale drafts).
2. **Strict grammar** — `8::30` used to be silently "repaired" to 08:30 (extra colons collapsed). The grammar is now explicit and strict: exactly one `:`, hour part ≤ 2 digits, minute part ≤ 2 digits (`8:030`, `000:30`, `8:30:`, `:30` all rejected) while keeping the typing-friendly forms (`8`, `8:3`, `830`, `0830`, Persian/Arabic-Indic digits) and the valid ranges (hour ≤ 23, minute ≤ 59).
3. **breakMin contract unified** — the validator treated it as required while the API stored `s.breakMin ?? 0`. Now it is OPTIONAL everywhere (client / server / persistence): absent/null → 0 (matches the `Int @default(0)` column); when present: integer ≥ 0 and ≤ span (string/float/NaN/negative all rejected before the DB).
4. **PUT `/api/shifts` is transactional** — the old route ran `deleteMany` UNCONDITIONALLY then recreated segments only if `segments` was in the body: (a) a segments-less PUT silently destroyed the shift's segments; (b) a failed update after the delete left the shift with NO segments. Fix: the CRUD core moved to `src/lib/shift-crud.ts` (the route is a thin ADMIN-guarded wrapper) — `segments` absent ⇒ segments untouched; present ⇒ validated first, then delete+recreate inside ONE `$transaction` (proven with a RAISE-trigger rollback test on REAL SQLite). DELETE is transactional the same way.

## Item 10 — Device form defaults (archaeology, NO rewrite)

`newDeviceFormDefaults()` (existing, `src/lib/device-form-defaults.ts`) already produces exactly the required trio: **نوین پرداز NP / FKATTEND / port 4238 / TCP** — verified by the existing test suite (`scripts/test-device-config.ts`, section `[form]`: 80/80 green, re-run after this phase).

**Why the installed copy behaved differently — git evidence:**

| ref | `src/lib/device-form-defaults.ts` | form defaults |
| --- | --- | --- |
| v1.5.0 / v1.5.1 / v1.6.0 | **absent** | no NP/FKATTEND/4238 defaults |
| v1.7.0 … HEAD | present (introduced by `92d4dbc7`, shipped in v1.7.0) | correct trio |

The install log banner said **v1.6.0** while HEAD's installer said v1.7.0 (the item-3 discrepancy). The item-12 quoted SDK text also exists verbatim **only in v1.6.0** (`devices.tsx` line 514 at that tag). Conclusion: the artifact the user installed was **built from v1.6.0-era code** — its form predates the fix. The code at HEAD is correct; per the spec («این منطق را بی‌دلیل بازنویسی نکن») nothing was rewritten.

Edit form for existing devices keeps stored values verbatim (including the legacy SBXPC fallback) — covered by the same suite.

## Item 11 — Device dialog width

- v1.6.0 dialog: `max-w-2xl` (672 px, **uncapped**) → on windows narrower than ~672 px the dialog overflowed the viewport → the reported «بیش از حد عریض / اسکرول افقی».
- v1.7.0 (commit `92d4dbc7`) redesigned the dialog responsive: fixed header/footer, scrollable body, `sm:max-w-[min(48rem,calc(100vw-2rem))] lg:max-w-[min(56rem,calc(100vw-2rem))]`, `grid-cols-1 sm:grid-cols-2/3` sections. **Byte-verified at HEAD** (an earlier suspicion of a corrupted class string turned out to be a display artifact; `od -c` confirms the real bytes).
- This phase adds only an explicit base cap `max-w-[calc(100%-2rem)]` (so the file does not rely on the `ui/dialog.tsx` default carrying it) plus the archaeology note in the comment.

## Item 12 — SDK note removed from the form

- The quoted sentence («SDK لازم (FK623Attend.dll و ۶ DLL همراه) …») existed **in v1.6.0** and was already softened in v1.7.0; the FKATTEND info box still carried the DLL path logistics («DLLها همراه بستهٔ نصب در tools\fkattend-bridge\bin است …»).
- Removed from the **form** now. Kept (per spec): the broken-install diagnostic in the devices list view (only renders when DLLs are missing), technical docs, and internal diagnostic reports.

## Tests

| suite | scope | result |
| --- | --- | --- |
| `scripts/test-shift-time.ts` (extended this stage) | pure parse/roundtrip/night/validate + the 49 800-min regression + STRICT grammar rejections + Arabic-Indic digits + the unified breakMin contract | **75 passed / 0 failed** (47 before the extension, all still green) |
| `scripts/test-shift-form.ts` (NEW) | the TimePicker ⇄ parent draft gate: invalid draft closes the save gate, fixing re-opens it, one-invalid-of-many blocks, removal/reset release keys, blur abandons, handler refuses independently, value contract of a whole typing session | **41 passed / 0 failed** |
| `scripts/test-shift-crud.ts` (NEW) | REAL SQLite + REAL Prisma + REAL `src/lib/shift-crud.ts` on a throwaway test-owned DB: create/PUT-without-segments (risk 1) / PUT-with-segments atomic replace / invalid PUT untouched / mid-transaction RAISE rollback (risk 2) / nonexistent id / empty list / breakMin end-to-end / DELETE atomicity | **43 passed / 0 failed** |
| `scripts/test-device-config.ts` (re-run) | form defaults, harmonization, edit-form preservation | **80 passed / 0 failed** |
| `scripts/test-time.ts` (re-run) | phase-1/S5 time infrastructure regression | **101 assertions / 0 failed, 23 sections / 0 failed** |
| `npx tsc --noEmit` | whole-project type gate | see phase report |

Mock vs reality: all of the above is pure/UI logic except `test-shift-crud.ts`, which runs on a **real throwaway SQLite database** — no device, no Windows installer involved, no real data touched. Visual/UX verification of the picker and the responsive dialog is on the manual checklist (narrow window widths, RTL layout). Hardware tests: none apply to this phase.

## Manual visual checklist (for the installed build)

1. Shift dialog: type `830` → shows `08:30` after colon/blur, no corruption; `25:99` → red error, ذخیره disabled; reopen edit → values intact.
2. Stage 7 fix: type `25:99` in ANY time field → ذخیره goes disabled immediately and stays disabled until the field is fixed (or the edit abandoned by clicking elsewhere); with two بازه rows, an invalid draft in only one of them blocks save; deleting that بازه re-enables save; switching to another shift / بازه جدید starts clean.
3. `8::30` now shows a red format error (it is no longer silently read as 08:30).
4. Night shift 22:00→06:00 → «شب‌گذر» badge, مدت ۸س; 24h (start==end) → «۲۴ ساعته».
5. Device dialog: NP selected → no horizontal scroll at 360/480/600/800/1280 px widths; body scrolls vertically only.
6. New device form opens with نوین پرداز NP / FKATTEND / 4238 / TCP.
7. No SDK/DLL logistics text in the device form (diagnostics still appear when DLLs are missing).
