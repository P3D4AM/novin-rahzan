#!/bin/bash
# decompile_complete.sh — دیکامپایل کامل همهٔ کتابخانه‌های مدیریتی بستهٔ NovinDll's
# خروجی: NovinLibraryResearch/decompiled-complete/<نام-کتابخانه>/
# ابزار: ilspycmd 8.2.0.7535 (همان نسخهٔ تولید decompiled/ برای خروجی deterministic)
# استفاده: decompile_complete.sh [all|custom|system|<نام-کتابخانه>]
# هر کتابخانهٔ کامل‌شده با فایل مارکر .done/.complete مشخص می‌شود (قابل ازسرگیری).

export DOTNET_ROOT=/home/z/dotnet
export PATH=/home/z/dotnet:/home/z/.dotnet/tools:$PATH

SRC="/home/z/my-project/upload/novindlls_extracted/NovinDll's"
OUT="/tmp/novin-rahzan/NovinLibraryResearch/decompiled-complete"
LOG="/tmp/decompile_complete.log"

MODE="${1:-all}"

CUSTOM_LIBS=(
  Tele
  TimeAttendance.ValidatorAttribute
  TimeAttendanceDAL
  TimeAttendanceDevices
  TimeAttendancePrint
  TimeAttendanceUI
  Tulpep.NotificationWindow
  WebSmsStandard
  WinFormAnimation
  ZDC2911
  ZK
)

SYSTEM_LIBS=(
  System.CodeDom
  System.Collections.Immutable
  System.ComponentModel.Annotations
  System.Configuration.ConfigurationManager
  System.Data.SqlClient
  System.Diagnostics.DiagnosticSource
  System.Drawing.Common
  System.IO.Pipelines
  System.IO.Ports
  System.IdentityModel.Tokens.Jwt
  System.Memory
  System.Net.WebSockets.WebSocketProtocol
  System.Numerics.Vectors
  System.Runtime.CompilerServices.Unsafe
  System.Security.AccessControl
  System.Security.Cryptography.ProtectedData
  System.Security.Permissions
  System.Security.Principal.Windows
  System.Text.Encoding.CodePages
  System.Text.Encodings.Web
  System.Text.Json
  System.Threading.Channels
  System.Threading.Tasks.Dataflow
  System.Threading.Tasks.Extensions
  System.ValueTuple
)

case "$MODE" in
  all)     LIBS=("${CUSTOM_LIBS[@]}" "${SYSTEM_LIBS[@]}");;
  custom)  LIBS=("${CUSTOM_LIBS[@]}");;
  system)  LIBS=("${SYSTEM_LIBS[@]}");;
  *)       LIBS=("$MODE");;
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
  if timeout 540 ilspycmd "$SRC/$lib.dll" -p -o "$OUT/$lib" >> "$LOG" 2>&1; then
    nfiles=$(find "$OUT/$lib" -type f ! -name '.done.complete' | wc -l)
    touch "$OUT/$lib/.done.complete"
    echo "### OK $lib ($nfiles files)"
  else
    echo "### FAILED $lib (exit $?)"
    FAIL=$((FAIL+1))
  fi
done

echo "=== DONE MODE=$MODE (failures: $FAIL) ==="
