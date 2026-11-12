#!/bin/bash
# decompile_rahzan.sh — دیکامپایل ۱۶۴ کتابخانهٔ جدید بستهٔ سوم «Novin-rahzan-dlls» (۲۳۹ فایل DLL)
# کتابخانه‌های دیکامپایل‌شدهٔ قبلی (۵۶ عدد از بسته‌های ۱ و ۲) به‌طور خودکار skip می‌شوند.
# خروجی: NovinLibraryResearch/decompiled-complete/<نام-کتابخانه>/
# ابزار: ilspycmd 8.2.0.7535 (همان نسخهٔ تولید decompiled/ و decompiled-complete/ برای خروجی deterministic)
# استفاده: decompile_rahzan.sh <مسیر-پوشهٔ-DLLها> [all|<نام-کتابخانه>]
# هر کتابخانهٔ کامل‌شده با مارکر .done/.complete مشخص می‌شود (قابل ازسرگیری؛ مارکرها پیش از کامیت حذف می‌شوند).

set -u
export DOTNET_ROOT="${DOTNET_ROOT:-/tmp/dotnet}"
export PATH="$DOTNET_ROOT:$HOME/.dotnet/tools:$PATH"
ILSPY="$(command -v ilspycmd)"

SRC="${1:?usage: decompile_rahzan.sh <dll-dir> [all|<lib>]}"
MODE="${2:-all}"
HERE="$(cd "$(dirname "$0")" && pwd)"
OUT="$(cd "$HERE/.." && pwd)/decompiled-complete"
LOG="/tmp/decompile_rahzan.log"

# لیست کتابخانه‌های مدیریت‌شدهٔ بسته (تشخیص از روی هدر PE)
ALL_MANAGED="$(python3 "$HERE/classify_dlls.py" "$SRC")"

case "$MODE" in
  all) LIBS="$ALL_MANAGED" ;;
  *)   LIBS="$MODE" ;;
esac

mkdir -p "$OUT"
FAIL=0; OK=0; SKIP=0
for lib in $LIBS; do
  # skip: قبلاً در بسته‌های ۱ و ۲ دیکامپایل شده است
  if [ -d "$OUT/$lib" ] && [ -z "$(ls -A "$OUT/$lib" 2>/dev/null | grep -v '.done.complete')" ]; then
    :
  fi
  if [ -d "$OUT/$lib" ] && find "$OUT/$lib" -name '*.cs' -print -quit | grep -q .; then
    echo "### $lib already decompiled, skip"
    SKIP=$((SKIP+1))
    continue
  fi
  if [ -f "$OUT/$lib/.done.complete" ]; then
    echo "### $lib already decompiled (marker), skip"
    SKIP=$((SKIP+1))
    continue
  fi
  rm -rf "$OUT/$lib"
  echo "### Decompiling $lib ..."
  SZ=$(stat -c %s "$SRC/$lib.dll")
  # تایم‌اوت متناسب با حجم: پایه ۳۰۰s + ۶۰s به‌ازای هر MB
  TMO=$(( 300 + SZ / 1000 / 1000 * 60 ))
  [ "$SZ" -gt 5000000 ] && TMO=$(( TMO + 600 ))
  if timeout "$TMO" "$ILSPY" "$SRC/$lib.dll" -p -o "$OUT/$lib" --disable-updatecheck >> "$LOG" 2>&1; then
    nfiles=$(find "$OUT/$lib" -type f ! -name '.done.complete' | wc -l)
    touch "$OUT/$lib/.done.complete"
    echo "### OK $lib ($nfiles files)"
    OK=$((OK+1))
  else
    echo "### FAILED $lib (exit $?)"
    FAIL=$((FAIL+1))
  fi
done

echo "=== DONE ok=$OK skip=$SKIP fail=$FAIL (log: $LOG) ==="
echo "یادآوری: پیش از کامیت، مارکرهای .done.complete را حذف کنید:"
echo "  find \"$OUT\" -name '.done.complete' -delete"
