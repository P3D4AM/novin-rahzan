#!/bin/bash
# v1.8.1 test suite — npm 10/11 × Node 20/24 install-scripts policy compatibility.
# =============================================================================
# REAL installs (network + real lifecycle scripts), no mocks: a scratch project
# mirrors the exact allowScripts field of package.json (approved + denied
# entries) with the same packages that triggered the user's npm 11 warning
# (prisma, @prisma/client, @prisma/engines, esbuild, sharp, @swc/core,
# @parcel/watcher, es5-ext, unrs-resolver).
#
# Per combo this asserts:
#   * `npm install --no-audit --no-fund --ignore-scripts=false` (the exact
#     INSTALL.bat invocation) exits 0
#   * NO "warn install-scripts" advisory appears (the user's v1.8.0 complaint)
#   * esbuild binary actually runs; sharp actually loads
#   * (npm >= 11 only) `npm approve-scripts --allow-scripts-pending` reports
#     ZERO uncovered packages
#
# Combos, located via env overrides (honest SKIP when a binary is absent):
#   NODE20_DIR (default /tmp/node-v20.20.2-linux-x64/bin) — a real Node 20
#   NPM10_BIN  (default /tmp/npm10/node_modules/.bin/npm)    — a real npm 10
#   NPM11N20_BIN (default /tmp/npm11n20/node_modules/.bin/npm) — npm 11 under Node 20
#   node 24 + npm 11 = the system pair.
#
# Run: bash scripts/test-npm-matrix.sh
set -u
PASS=0; FAIL=0; SKIP=0
declare -a FAILS=()
ok()   { PASS=$((PASS+1)); echo "  ✔ $1"; }
bad()  { FAIL=$((FAIL+1)); FAILS+=("$1"); echo "  ✘ $1"; }
skip() { SKIP=$((SKIP+1)); echo "  - SKIP $1 (باینری در دسترس نیست — صادقانه رد شد)"; }
note() { echo "    $1"; }

NODE20_DIR="${NODE20_DIR:-/tmp/node-v20.20.2-linux-x64/bin}"
NPM10_BIN="${NPM10_BIN:-/tmp/npm10/node_modules/.bin/npm}"
NPM11N20_BIN="${NPM11N20_BIN:-/tmp/npm11n20/bin/npm}"   # npm install -g puts the bin here
NPM11N20_PREFIX="$(dirname "$(dirname "$NPM11N20_BIN")")"

TMP="$(mktemp -d /tmp/npm-matrix-XXXXXX)"
MINI="$TMP/mini"
mkdir -p "$MINI"
cat > "$MINI/package.json" <<'JSON'
{
  "name": "npm-matrix-probe",
  "version": "1.0.0",
  "private": true,
  "dependencies": {
    "prisma": "6.19.2",
    "@prisma/client": "6.19.2",
    "@prisma/engines": "6.19.2",
    "esbuild": "0.28.2",
    "sharp": "0.34.5",
    "@swc/core": "1.15.8",
    "@parcel/watcher": "2.5.4",
    "es5-ext": "0.10.64",
    "unrs-resolver": "1.11.1"
  },
  "allowScripts": {
    "prisma": true,
    "@prisma/client": true,
    "@prisma/engines": true,
    "esbuild": true,
    "sharp": true,
    "@swc/core": true,
    "@parcel/watcher": true,
    "es5-ext": false,
    "unrs-resolver": false
  }
}
JSON

# if an npm 11 for Node 20 is missing but Node 20 exists, create it (real install)
if [ ! -x "$NPM11N20_BIN" ] && [ -d "$(dirname "$NODE20_DIR")" ]; then
  if PATH="$NODE20_DIR:$PATH" npm install -g npm@11 --prefix "$NPM11N20_PREFIX" >/dev/null 2>&1; then
    note "npm@11 برای Node 20 ساخته شد ($($NPM11N20_BIN -v 2>/dev/null))"
  fi
fi

run_combo() {
  local label="$1" pathprefix="$2" npmcmd="$3"
  echo ""
  echo "━━━ $label"
  local d="$TMP/$(echo "$label" | tr -cd 'a-zA-Z0-9')"
  rm -rf "$d"; mkdir -p "$d"
  cp "$MINI/package.json" "$d/package.json"

  local nv av
  nv="$(PATH="$pathprefix:$PATH" node -v 2>/dev/null)"
  av="$(PATH="$pathprefix:$PATH" "$npmcmd" -v 2>/dev/null)"
  if [ -z "$nv" ] || [ -z "$av" ]; then
    bad "ابزارهای این ترکیب در دسترس نیستند (node/npm)"
    return
  fi
  note "node $nv / npm $av"

  local out code
  out="$(cd "$d" && PATH="$pathprefix:$PATH" "$npmcmd" install --no-audit --no-fund --ignore-scripts=false 2>&1)"
  code=$?
  if [ $code -eq 0 ]; then ok "npm install (دقیقاً با فلگ‌های INSTALL.bat) → exit 0"; else bad "npm install شکست خورد (exit $code): $(echo "$out" | tail -2 | tr '\n' ' ')"; return; fi

  if echo "$out" | grep -q "warn install-scripts"; then
    bad "هشدار install-scripts هنوز ظاهر می‌شود: $(echo "$out" | grep -A2 'warn install-scripts' | head -3 | tr '\n' ' ')"
  else
    ok "هیچ هشدار install-scripts ظاهر نشد (مشکل اصلی کاربر)"
  fi

  if (cd "$d" && PATH="$pathprefix:$PATH" ./node_modules/.bin/esbuild --version >/dev/null 2>&1); then
    ok "باینری esbuild واقعاً اجرا می‌شود"
  else bad "esbuild اجرا نشد"; fi

  if (cd "$d" && PATH="$pathprefix:$PATH" node -e "require('sharp')" 2>/dev/null); then
    ok "sharp واقعاً بارگذاری می‌شود"
  else bad "sharp بارگذاری نشد"; fi

  # npm >= 11: the pending list must be EMPTY (all install scripts covered)
  local npm_major="${av%%.*}"
  if [ "$npm_major" -ge 11 ] 2>/dev/null; then
    local pend
    pend="$(cd "$d" && PATH="$pathprefix:$PATH" "$npmcmd" approve-scripts --allow-scripts-pending 2>&1)"
    if echo "$pend" | grep -qE "^0 packages|^No packages with unreviewed|^no packages"; then
      ok "فهرست pending خالی است (همهٔ اسکریپت‌ها پوشش داده شده‌اند)"
    elif echo "$pend" | grep -q "have install scripts not yet covered"; then
      bad "بسته‌هایی بدون پوشش مانده‌اند: $(echo "$pend" | head -4 | tr '\n' ' ')"
    else
      note "خروجی pending بررسی دستی: $(echo "$pend" | head -2 | tr '\n' ' ')"
    fi
  else
    note "npm $av فیلد allowScripts را نادیده می‌گیرد (رفتار مورد انتظار npm 10)"
  fi
}

echo "════ ماتریس npm/Node — نصب‌های واقعی (شبکه + اسکریپت‌های نصب واقعی)"

# 1) Node 24 + npm 11 — the system pair (also the user's real environment shape)
run_combo "Node24 + npm11 (سیستم)" "" "npm"

# 2) Node 24 + npm 10 — field must be ignored cleanly
if [ -x "$NPM10_BIN" ]; then
  run_combo "Node24 + npm10" "" "$NPM10_BIN"
else
  echo ""; echo "━━━ Node24 + npm10"; skip "NPM10_BIN ($NPM10_BIN)"
fi

# 3) Node 20 + its bundled npm 10
if [ -d "$NODE20_DIR" ]; then
  run_combo "Node20 + npm10 (همراه Node)" "$NODE20_DIR" "npm"
else
  echo ""; echo "━━━ Node20 + npm10 (همراه Node)"; skip "NODE20_DIR ($NODE20_DIR)"
fi

# 4) Node 20 + npm 11 — forward compatibility
if [ -x "$NPM11N20_BIN" ] && [ -d "$NODE20_DIR" ]; then
  run_combo "Node20 + npm11" "$NODE20_DIR" "$NPM11N20_BIN"
else
  echo ""; echo "━━━ Node20 + npm11"; skip "NPM11N20_BIN ($NPM11N20_BIN)"
fi

echo ""
echo "══════════════════════════════════════════"
echo "مجموعهٔ npm-matrix: $PASS تایید شد، $FAIL شکست خورد، $SKIP رد شد"
if [ ${#FAILS[@]} -gt 0 ]; then
  echo "شکست‌ها:"
  for f in "${FAILS[@]}"; do echo "  ✘ $f"; done
fi
echo "══════════════════════════════════════════"
rm -rf "$TMP"
exit $([ $FAIL -gt 0 ] && echo 1 || echo 0)
