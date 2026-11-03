#!/bin/bash
# decompile_alldlls.sh — دیکامپایل کامل ۲۰ کتابخانهٔ بستهٔ «All novin-dlls» (بستهٔ دوم)
# خروجی: NovinLibraryResearch/decompiled-complete/<نام-کتابخانه>/
# ابزار: ilspycmd 8.2.0.7535 (همان نسخهٔ تولید decompiled/ و decompiled-complete/ برای خروجی deterministic)
# استفاده: decompile_alldlls.sh [all|<نام-کتابخانه>]
# هر کتابخانهٔ کامل‌شده با فایل مارکر .done/.complete مشخص می‌شود (قابل ازسرگیری؛ مارکرها پیش از کامیت حذف می‌شوند).

export DOTNET_ROOT=/tmp/dotnet
export PATH=/tmp/dotnet:$PATH
ILSPY=/home/z/.dotnet/tools/ilspycmd

SRC="/home/z/my-project/upload/libs/extracted/All novin-dlls"
OUT="/tmp/novin-rahzan/NovinLibraryResearch/decompiled-complete"
LOG="/tmp/decompile_new.log"

MODE="${1:-all}"

LIBS=(
  AccSqlServerPersistence
  AxInterop.RealSvrOcxTcpLib
  AxInterop.SBXPCLib
  AxInterop.zkemkeeper
  BackUpDLL
  BehComponents
  ClientHubStandard
  DevComponents.DotNetBar2
  EntityCacheStandard
  FK623AttendDllCSSample
  FormCustom
  Interop.zkemkeeper
  LK
  NPResourceManagement
  PacketParserStandard
  PayamakUi
  Riss.Devices
  SBXPCSampleCSharp
  Stimulsoft.Report
  Stimulsoft.Report.Win
)

case "$MODE" in
  all) LIBS=("${LIBS[@]}");;
  *)   LIBS=("$MODE");;
esac

mkdir -p "$OUT"

FAIL=0
for lib in "${LIBS[@]}"; do
  if [ -f "$OUT/$lib/.done.complete" ]; then
    echo "### $lib already decompiled, skip"
    continue
  fi
  rm -rf "$OUT/$lib"
  echo "### Decompiling $lib ..."
  # توجه: AccSqlServerPersistence.dll حدود ۴۴MB است؛ تایم‌اوت بلندتر لازم است.
  TMO=600
  [ "$lib" = "AccSqlServerPersistence" ] && TMO=3600
  if timeout $TMO "$ILSPY" "$SRC/$lib.dll" -p -o "$OUT/$lib" --disable-updatecheck >> "$LOG" 2>&1; then
    nfiles=$(find "$OUT/$lib" -type f ! -name '.done.complete' | wc -l)
    touch "$OUT/$lib/.done.complete"
    echo "### OK $lib ($nfiles files)"
  else
    echo "### FAILED $lib (exit $?)"
    FAIL=$((FAIL+1))
  fi
done

echo "=== DONE MODE=$MODE (failures: $FAIL) ==="
