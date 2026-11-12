#!/usr/bin/env python3
"""classify_dlls.py — تشخیص اسمبلی مدیریت‌شدهٔ .NET از روی هدر PE (دایرکتوری COM descriptor).

خروجی: نام (بدون .dll) همهٔ DLLهای مدیریت‌شدهٔ موجود در پوشهٔ داده‌شده، خط‌به‌خط.
"""
import os
import struct
import sys


def is_managed(path):
    """RVA دایرکتوری ۱۴ (CLR Runtime Header) غیرصفر ⇒ اسمبلی .NET است."""
    try:
        with open(path, "rb") as f:
            hdr = f.read(0x400)
            if hdr[:2] != b"MZ":
                return False
            e_lfanew = struct.unpack_from("<I", hdr, 0x3C)[0]
            if hdr[e_lfanew:e_lfanew + 4] != b"PE\x00\x00":
                return False
            opt_off = e_lfanew + 24  # بعد از امضا + COFF (۲۰ بایت)
            magic = struct.unpack_from("<H", hdr, opt_off)[0]
            if magic == 0x10B:      # PE32
                ddir_off = opt_off + 96
            elif magic == 0x20B:    # PE32+
                ddir_off = opt_off + 112
            else:
                return False
            need = ddir_off + 15 * 8
            if need > len(hdr):
                f.seek(0)
                hdr = f.read(need)
            rva = struct.unpack_from("<I", hdr, ddir_off + 14 * 8)[0]
            return rva != 0
    except OSError:
        return False


if __name__ == "__main__":
    src = sys.argv[1]
    for fn in sorted(os.listdir(src)):
        if fn.lower().endswith(".dll") and is_managed(os.path.join(src, fn)):
            print(fn[:-4])
