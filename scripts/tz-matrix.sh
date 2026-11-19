#!/usr/bin/env bash
# Phase 2 S5 — the REAL four-timezone regression matrix.
#
# Runs every self-contained (mock/pure/sqlite-throwaway) suite FOUR times, each
# in a child process whose TZ environment is set to one of:
#   UTC / Asia/Tehran / Europe/Vienna / America/New_York
# For identical inputs the outputs must be identical (host-tz independence) —
# except where a documented contract explicitly depends on the Tehran day
# (day markers / windows — which are TZ-derived, never host-derived).
#
# Suites that need real hardware (NP756+ / FK623Attend.dll on Windows) are NOT
# run here — see tools/fkattend-bridge/CHECKLIST-WINDOWS.md. test-sbxpc.ts and
# test-np-v12x.mjs are interactive probes against a started mock device, not
# assertion suites (their coverage is embedded in test-raw-import.ts).
#
# Usage: bash scripts/tz-matrix.sh [logsDir]
set -u
cd "$(dirname "$0")/.." || exit 1
LOGS="${1:-/tmp/s5-tz-matrix}"
mkdir -p "$LOGS"
TZS=(UTC Asia/Tehran Europe/Vienna America/New_York)
SUITES=(
  scripts/test-time.ts
  scripts/test-attendance.ts
  scripts/test-attendance-db.ts
  scripts/test-raw-import.ts
  scripts/test-bridge-supervisor.ts
  scripts/test-device-config.ts
  scripts/test-scheduler.ts
  scripts/test-live-notif.ts
  scripts/test-rt-poll.ts
)
declare -A PASS FAIL VERDICT
overall=0
for suite in "${SUITES[@]}"; do
  name=$(basename "$suite")
  for tz in "${TZS[@]}"; do
    log="$LOGS/${name%.ts}.TZ-${tz//\//_}.log"
    TZ="$tz" npx tsx "$suite" >"$log" 2>&1
    code=$?
    # suite summaries print "N passed, M failed" (or assertions/sections lines)
    p=$(grep -oE '[0-9]+ passed' "$log" | awk '{s+=$1} END {print s+0}')
    f=$(grep -oE '[0-9]+ failed' "$log" | awk '{s+=$1} END {print s+0}')
    PASS["$name|$tz"]=${p:-0}
    FAIL["$name|$tz"]=${f:-0}
    if [ "$code" -ne 0 ] || [ "${FAIL[$name|$tz]}" -ne 0 ]; then
      VERDICT["$name|$tz"]="FAILED"
      overall=1
    else
      VERDICT["$name|$tz"]="ok"
    fi
  done
  printf '%-28s' "$name"
  for tz in "${TZS[@]}"; do printf ' | %s %s/%s' "${tz}" "${VERDICT[$name|$tz]}" "p${PASS[$name|$tz]}" ; done
  printf '\n'
done
printf '\n'
for tz in "${TZS[@]}"; do
  tp=0; tf=0; bad=0
  for suite in "${SUITES[@]}"; do
    name=$(basename "$suite")
    tp=$((tp + PASS[$name|$tz])); tf=$((tf + FAIL[$name|$tz]))
    [ "${VERDICT[$name|$tz]}" = "FAILED" ] && bad=1
  done
  printf 'TZ=%-18s total: %d passed, %d failed — %s\n' "$tz" "$tp" "$tf" "$([ $bad -eq 0 ] && echo ALL GREEN || echo HAS FAILURES)"
done
exit $overall
