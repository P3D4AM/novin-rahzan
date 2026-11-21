#!/bin/bash
# v1.8.1 test suite — REAL end-to-end install pipeline (network, no mocks).
# =============================================================================
# This is the "at least one REAL install + Prisma Generate" suite the spec
# demands. On Linux it executes the exact INSTALL.bat stage sequence against a
# pristine copy of the repository HEAD:
#
#   1. git archive HEAD → /tmp (the same packaging basis as the release ZIP)
#   2. npm install --no-audit --no-fund --ignore-scripts=false   (REAL network install)
#   3. npx prisma generate                                        (REAL generation)
#   4. node scripts/verify-native.js                              (REAL verification — must exit 0)
#   5. DATABASE_URL=… npx prisma db push --skip-generate           (REAL schema push)
#   6. env-ensure twice with a pre-existing SECRET in .env          (preservation proof)
#   7. insert a REAL row with @prisma/client
#   8. RE-RUN steps 2..6 over the same install                     (no data destruction)
#      → user row still there, SECRET still there
#
# Run: bash scripts/test-installer-real.sh [head|work]
#   head — archive the COMMITTED HEAD (exactly the release-ZIP basis; run this
#         AFTER committing the fix for the official record)
#   work — copy the WORKING TREE including uncommitted changes (pre-commit
#         verification while developing; requires no history rewrite)
set -u
cd "$(dirname "$0")/.." || exit 2
REPO_ROOT="$(pwd)"
MODE="${1:-head}"
HEAD_SHA="$(git rev-parse HEAD)"

PASS=0; FAIL=0; SKIP=0
declare -a FAILS=()
ok()   { PASS=$((PASS+1)); echo "  ✔ $1"; }
bad()  { FAIL=$((FAIL+1)); FAILS+=("$1"); echo "  ✘ $1"; }
note() { echo "    $1"; }
phase(){ echo ""; echo "━━━ $1"; }

WORK="$(mktemp -d /tmp/real-install-XXXXXX)"
D="$WORK/HozorPlus"
mkdir -p "$D"

phase "پایه — $([ "$MODE" = head ] && echo "git archive از HEAD" || echo "کپی درخت کاری (تغییرات کامیت‌نشده)") ($HEAD_SHA)"
if [ "$MODE" = head ]; then
  git archive --format=tar HEAD | tar -x -C "$D" && ok "درخت HEAD استخراج شد ($(git ls-tree -r --name-only HEAD | wc -l) فایل)" || { bad "استخراج git archive ناموفق"; exit 1; }
else
  # tracked + untracked-not-ignored files of the working tree (uncommitted
  # changes included), preserving relative paths
  git ls-files --cached --others --exclude-standard | while IFS= read -r f; do
    mkdir -p "$D/$(dirname "$f")"
    cp -a "$REPO_ROOT/$f" "$D/$f"
  done
  ok "درخت کاری کپی شد ($(git ls-files --cached --others --exclude-standard | wc -l) فایل)"
fi
[ -f "$D/prisma/schema.prisma" ] && ok "prisma/schema.prisma در بسته هست" || bad "schema.prisma نیست"
[ -f "$D/INSTALL.bat" ] && ok "INSTALL.bat در بسته هست" || bad "INSTALL.bat نیست"
[ -f "$D/scripts/verify-native.js" ] && ok "scripts/verify-native.js در بسته هست" || bad "verify-native.js نیست"

phase "مرحلهٔ ۲/۸ — npm install واقعی (شبکه)"
OUT="$(cd "$D" && npm install --no-audit --no-fund --ignore-scripts=false 2>&1)"
CODE=$?
if [ $CODE -eq 0 ]; then ok "npm install → exit 0 ($(ls "$D/node_modules" | wc -l) پکیج)"; else bad "npm install شکست خورد: $(echo "$OUT" | tail -3 | tr '\n' ' ')"; fi
if echo "$OUT" | grep -q "warn install-scripts"; then
  bad "هشدار install-scripts ظاهر شد (نباید): $(echo "$OUT" | grep -A3 'warn install-scripts' | head -4 | tr '\n' ' ')"
else ok "بدون هشدار install-scripts (مشکل npm 11 کاربر حل شد)"
fi

phase "مرحلهٔ ۳/۸ — prisma generate واقعی"
OUT="$(cd "$D" && npx prisma generate 2>&1)"
CODE=$?
if [ $CODE -eq 0 ]; then ok "prisma generate → exit 0"; else bad "prisma generate شکست خورد: $(echo "$OUT" | tail -3 | tr '\n' ' ')"; fi
[ -f "$D/node_modules/.prisma/client/index.js" ] && ok "کلاینت تولیدشده: node_modules/.prisma/client/index.js" || bad "کلاینت تولیدشده ساخته نشد"
ENG="$(ls "$D/node_modules/.prisma/client" | grep -E '^(lib)?query_engine' | head -1 || true)"
[ -n "$ENG" ] && ok "موتور در پوشهٔ کلاینت کپی شد: $ENG" || bad "موتور در .prisma/client نیست"
ENG2="$(ls "$D/node_modules/@prisma/engines" | grep -E '^(lib)?query_engine' | head -1 || true)"
[ -n "$ENG2" ] && ok "موتور در @prisma/engines موجود است: $ENG2" || bad "موتور در @prisma/engines نیست"

phase "مرحلهٔ ۴/۸ — verify-native واقعی (همان ترتیب جدید INSTALL.bat)"
OUT="$(cd "$D" && node scripts/verify-native.js --json 2>&1)"
CODE=$?
if [ $CODE -eq 0 ]; then ok "verify-native → exit 0 (نصب واقعی سالم اعلام می‌شود)"; else bad "verify-native شکست خورد (exit $CODE): $OUT"; fi
if echo "$OUT" | node -e "let d='';process.stdin.on('data',c=>d+=c).on('end',()=>{try{const j=JSON.parse(d);console.log(j.ok===true&&j.critical===false?'JSONOK':'JSONBAD:'+JSON.stringify(j.lines&&j.lines.filter(l=>l.level==='bad')))}catch(e){console.log('PARSEFAIL')}})" 2>/dev/null | grep -q '^JSONOK'; then ok "حالت JSON: ok=true / critical=false"; else bad "حالت JSON نتیجهٔ سالم نداد: $(echo "$OUT" | head -c 300)"; fi

phase "مرحلهٔ ۵/۸ — prisma db push واقعی (SQLite)"
export DATABASE_URL="file:$D/db/custom.db"
mkdir -p "$D/db"
OUT="$(cd "$D" && npx prisma db push --skip-generate 2>&1)"
CODE=$?
if [ $CODE -eq 0 ]; then ok "prisma db push → exit 0"; else bad "db push شکست خورد: $(echo "$OUT" | tail -3 | tr '\n' ' ')"; fi
[ -f "$D/db/custom.db" ] && ok "فایل دیتابیس ساخته شد: db/custom.db" || bad "فایل دیتابیس ساخته نشد"

phase "مرحلهٔ ۶/۸ — حفظ .env با env-ensure"
printf '# existing install\nSECRET_KEY=my-secret-123\nOTHER_TOKEN=keep-me\n' > "$D/.env"
(cd "$D" && node scripts/env-ensure.js "DATABASE_URL=file:$D/db/custom.db" >/dev/null 2>&1) && ok "env-ensure اجرا شد" || bad "env-ensure شکست خورد"
grep -q '^SECRET_KEY=my-secret-123$' "$D/.env" && ok "SECRET_KEY موجود حفظ شد" || bad "SECRET_KEY از بین رفت"
grep -q '^OTHER_TOKEN=keep-me$' "$D/.env" && ok "OTHER_TOKEN موجود حفظ شد" || bad "OTHER_TOKEN از بین رفت"
grep -q "^DATABASE_URL=file:$D/db/custom.db$" "$D/.env" && ok "DATABASE_URL به‌روز شد" || bad "DATABASE_URL نوشته نشد"

phase "مرحلهٔ ۷ — درج ردیف واقعی با @prisma/client"
OUT="$(cd "$D" && DATABASE_URL="file:$D/db/custom.db" node -e "
const { PrismaClient } = require('@prisma/client');
const p = new PrismaClient();
p.user.create({ data: { username: 'installtest', passwordHash: 'x', fullName: 'Install Test' } })
  .then(() => p.user.count())
  .then((c) => { console.log('COUNT=' + c); return p.\$disconnect(); })
  .catch((e) => { console.error(e.message); process.exit(1); });
" 2>&1)"
CODE=$?
echo "$OUT" | grep -q 'COUNT=1' && ok "یک ردیف واقعی درج شد و count=1" || bad "درج/شمارش ردیف شکست خورد: $OUT"

phase "اجرای دوبارهٔ نصب روی نصب موجود (بدون تخریب)"
OUT="$(cd "$D" && npm install --no-audit --no-fund --ignore-scripts=false 2>&1)"
CODE=$?
[ $CODE -eq 0 ] && ok "npm install دوباره → exit 0" || bad "npm install دوباره شکست خورد"
OUT="$(cd "$D" && npx prisma generate 2>&1)"; CODE=$?
[ $CODE -eq 0 ] && ok "prisma generate دوباره → exit 0" || bad "generate دوباره شکست خورد"
OUT="$(cd "$D" && node scripts/verify-native.js 2>&1)"; CODE=$?
[ $CODE -eq 0 ] && ok "verify-native دوباره → exit 0" || bad "verify دوباره شکست خورد"
OUT="$(cd "$D" && DATABASE_URL="file:$D/db/custom.db" npx prisma db push --skip-generate 2>&1)"; CODE=$?
[ $CODE -eq 0 ] && ok "db push دوباره → exit 0" || bad "db push دوباره شکست خورد"
OUT="$(cd "$D" && DATABASE_URL="file:$D/db/custom.db" node -e "
const { PrismaClient } = require('@prisma/client');
const p = new PrismaClient();
p.user.count()
  .then((c) => { console.log('COUNT=' + c); return p.\$disconnect(); })
  .catch((e) => { console.error(e.message); process.exit(1); });
" 2>&1)"
echo "$OUT" | grep -q 'COUNT=1' && ok "ردیف کاربر پس از اجرای دوبارهٔ کامل هنوز موجود است (داده حفظ شد)" || bad "ردیف از بین رفت: $OUT"
grep -q '^SECRET_KEY=my-secret-123$' "$D/.env" && ok "SECRET_KEY پس از اجرای دوباره حفظ شد" || bad "SECRET_KEY پس از اجرای دوباره از بین رفت"

echo ""
echo "══════════════════════════════════════════"
echo "مجموعهٔ installer-real ($MODE @ ${HEAD_SHA:0:8}): $PASS تایید شد، $FAIL شکست خورد، $SKIP رد شد"
if [ ${#FAILS[@]} -gt 0 ]; then
  echo "شکست‌ها:"
  for f in "${FAILS[@]}"; do echo "  ✘ $f"; done
fi
echo "══════════════════════════════════════════"
rm -rf "$WORK"
exit $([ $FAIL -gt 0 ] && echo 1 || echo 0)
