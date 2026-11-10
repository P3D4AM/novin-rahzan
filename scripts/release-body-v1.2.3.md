<div dir="rtl">

# 🔬 حضور+ (HozorPlus) v1.2.3 — رفع فرمول رمز «رمز شبکه» + ابزار تشخیص np-probe

## ⭐ چه شد؟

 شما رمز شبکه را درست وارد کردید و فریم هم دقیقاً درست ساخته شد — اما دستگاه بی‌پاسخ ماند. عمیق‌تر DLLهای نرم‌افزار اصلی را دوباره بررسی کردیم و **یک اشتباه در v1.2.2 پیدا شد**:

- در v1.2.2 کلید رمز را `رمز + 154500 (0x25B84)` می‌ساختیم، چون این عدد را در فایل DLL «خوانده بودیم».
- بررسی دقیق‌تر نشان داد آن عدد **آشغال فایل** بود (بخشی از جدول exception ها)، نه مقدار واقعی! جای واقعی این ثابت در DLL **خالی و صفر** است (BSS) — یعنی فرمول درست DLL:

```
کلید رمز = LE32(رمز شبکه)        ← بدون هیچ عدد اضافه!
```

## ✅ چه چیزی درست شد

### ۱. کلاینت حالا هر دو فرمول را امتحان می‌کند

اول `کلید = رمز` (پیش‌فرض واقعی DLL)، بعد `کلید = رمز + 0x25B84` (واریانت قدیمی). اگر دستگاهتان با فرمول درست کار کند، بدون هیچ کاری «تست اتصال» حالا موفق می‌شود و سریال + ساعت دستگاه را می‌خواند.

### ۲. رمز 12345 به لیست رمزهای خودکار اضافه شد

اگر رمز دستگاهتان از لیست رایج باشد حتی نیاز به وارد کردنش نیست.

### ۳. ابزار جدید تشخیص: `scripts\np-probe.js` 🛠️

اگر باز هم جواب نگرفتید، این ابزار **همه گویش‌ها و فرمول‌های رمز ممکن** را روی دستگاه واقعی امتحان می‌کند و پاسخ hex هر تلاش را چاپ می‌کند:

```bat
node scripts\np-probe.js 192.168.1.75 4238 12345
```

- ۱۳ واریانت باینری (کلاسیک، EXT ساده، رمزشده با ۷ فرمول کلید مختلف، فریم کلاسیک رمزشده، …)
- **+ بررسی API HTTP روی پورت 14460** — کشف مهم امروز: دستگاه‌های جدید NP (NP756P/NP761P «VisibleLight») یک API کامل HTTP/JSON دارند (`/getDeviceKey`, `/getLogCount`, `/getLogInfo`...) که اگر دستگاهتان داشته باشد، یکپارچه‌سازی بسیار تمیزتری ممکن می‌شود!

خروجی کامل را برایمان بفرستید تا از روی فریم‌های hex دقیقاً بگوییم دستگاهتان چه صحبت می‌کند.

## 📦 نصب و به‌روزرسانی

- زیپ جدید را روی محل قبلی استخراج کنید → `INSTALL.bat` → `START.bat`
- سپس: **دستگاه‌ها ← تست اتصال** — اگر موفق شد، «دانلود ترددها» هم کار می‌کند
- اگر خطا داد: `node scripts\np-probe.js <IP> <پورت> <رمز>` را اجرا و خروجی را بفرستید

## 🔄 خلاصه فنی

- `extKeyFor(password, salt)` با salt ∈ {0, 0x25B84} — تلاش خودکار هر دو روی اتصال‌های تازه
- تصحیح مستندات: اسلات نمک BSS است؛ `ChangeCompanyMark_EXT` تنها نویسندهٔ آن است و هیچ‌کس در استک فروشنده صدایش نمی‌زند؛ TimeAttendance.exe سه‌۲بیتی است و OCX فقط توابع کلاسیک را import می‌کند
- مستندسازی API HTTP دستگاه‌های VisibleLight (پورت 14460) با endpoint های استخراج‌شده از PacketParserStandard.dll
- `scripts/np-probe.js` — پروبر ۱۳-واریانته + HTTP با خروجی hex کامل فارسی
- رمز 12345 به PASSWORD_CANDIDATES اضافه شد

</div>

---

**English TL;DR** — v1.2.3 fixes the EXT_SECURE crypto formula. Your password was transmitted correctly, but v1.2.2 derived the XOR key as `LE32(password + 0x25B84)` — that constant turned out to be a misread of file padding (the DLL's salt slot is zero-initialized BSS; only `ChangeCompanyMark_EXT` ever writes it and nothing in the vendor stack calls it). The correct default is **key = LE32(رمز شبکه)** with no salt. The client now tries both derivations automatically (salt 0 first, then 0x25B84), and 12345 joined the auto-probed password list. A new standalone diagnostic `scripts/np-probe.js` exercises 13 binary variants **plus the newly-discovered HTTP JSON API on port 14460** that the newest NP face devices (NP756P/NP761P "VisibleLight", model codes 9308/9309 in the vendor UI) expose (`/getDeviceKey`, `/getLogCount`, `/getLogInfo`, …) — run it against the device and send us the hex output to pinpoint the exact dialect.
