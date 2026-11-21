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
| `scripts/test-shift-time.ts` (NEW) | pure parse/roundtrip/night/validate + the 49 800-min regression | **47 passed / 0 failed** |
| `scripts/test-device-config.ts` (re-run) | form defaults, harmonization, edit-form preservation | **80 passed / 0 failed** |
| `npx tsc --noEmit` | whole-project type gate | see phase report |

Mock vs reality: all of the above is **pure/UI logic — no device, no Windows installer involved**. Visual/UX verification of the picker and the responsive dialog is on the manual checklist (narrow window widths, RTL layout). Hardware tests: none apply to this phase.

## Manual visual checklist (for the installed build)

1. Shift dialog: type `830` → shows `08:30` after colon/blur, no corruption; `25:99` → red error, ذخیره disabled; reopen edit → values intact.
2. Night shift 22:00→06:00 → «شب‌گذر» badge, مدت ۸س; 24h (start==end) → «۲۴ ساعته».
3. Device dialog: NP selected → no horizontal scroll at 360/480/600/800/1280 px widths; body scrolls vertically only.
4. New device form opens with نوین پرداز NP / FKATTEND / 4238 / TCP.
5. No SDK/DLL logistics text in the device form (diagnostics still appear when DLLs are missing).
