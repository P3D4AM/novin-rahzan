#!/bin/bash
export DOTNET_ROOT=/home/z/dotnet
export PATH=/home/z/dotnet:/home/z/.dotnet/tools:$PATH
SRC="/home/z/my-project/upload/novindlls_extracted/NovinDll's"
OUT="/home/z/my-project/decompiled"
mkdir -p "$OUT"

# Custom/proprietary libraries (main focus)
for dll in Tele TimeAttendance.ValidatorAttribute TimeAttendanceDAL TimeAttendanceDevices TimeAttendancePrint TimeAttendanceUI WebSmsStandard ZDC2911 ZK Tulpep.NotificationWindow WinFormAnimation; do
    if [ ! -d "$OUT/$dll" ]; then
        echo "### Decompiling $dll ..."
        timeout 600 ilspycmd "$SRC/$dll.dll" -p -o "$OUT/$dll" 2>&1 | grep -iE "error" | head -3
    else
        echo "### $dll already decompiled"
    fi
done

# Check native COM DLLs
for dll in zkemkeeper zkemsdk; do
    echo "### $dll: $(file "$SRC/$dll.dll" | cut -d: -f2)"
done

echo "=== ALL DONE ==="
ls "$OUT"
