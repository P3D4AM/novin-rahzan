<div dir="rtl">

# 🔧 حضور+ (HozorPlus) v1.2.1 — رفع خطای «پاسخ دستگاه قابل تشخیص نیست» برای NP756+

## ⭐ چه شد؟

شما پورت درست دستگاه (۴۲۳۸) را پیدا کردید و دستگاه **واقعاً پاسخ داد** — با فریم `aa 55 …`. یعنی اتصال برقرار است، ولی دستگاه شما (مثل خیلی از NP756+های واقعی) **گویش دیگری از پروتکل** صحبت می‌کند: فریم‌های **۱۲ بایتی (EXT)** به‌جای فریم‌های ۱۶ بایتی کلاسیک.

این موضوع را با مهندسی معکوس عمیق‌تر DLLهای اصلی نوین حضور (`SBPCCOMM64.dll` — توابع `_EXT`) کشف کردیم: پاسخ `aa 55 … 10 …` که دستگاه شما می‌فرستاد یک **ACK خطای ۱۲بایتی** است که یعنی «فرمان شما را نشناختم» (کد 16).

## ✅ چه چیزی درست شد

### ۱. تشخیص خودکار گویش پروتکل

حالا هنگام «تست اتصال»، نرم‌افزار خودش کشف می‌کند دستگاه با کدام گویش صحبت می‌کند:

| گویش | توضیح |
| --- | --- |
| `CLASSIC` | فریم‌های ۱۶ بایتی + ACK — مثل نرم‌افزار اصلی |
| `RESULT_DIRECT` | فریم‌های کلاسیک بدون ACK |
| `EXT` | فریم‌های ۱۲ بایتی — گویشی که NP756+ واقعی شما استفاده می‌کند |

روش تشخیص: اول فرمان کلاسیک «خواندن سریال» فرستاده می‌شود؛ اگر دستگاه با ACK خطای ۱۲بایتی (کد 0x10) جواب داد، اتصال نو می‌شود و همان فرمان با فریم ۱۲بایتی EXT دوباره فرستاده می‌شود — دستگاه سریال و ساعت خودش را برمی‌گرداند.

### ۲. عیب‌یابی شفاف

- در نتیجه تست: **گویش پروتکل تشخیص‌داده‌شده** + یادداشت توضیحی نمایش داده می‌شود
- در صورت خطا: **فریم‌های خام (hex)** کامل مکالمه نشان داده می‌شود — دیگر هیچ «جعبه سیاهی» باقی نمی‌ماند
- همهٔ این موارد در «تاریخچه عملیات» دستگاه هم ثبت می‌شود

### ۳. دانلود ترددها هم با گویش EXT کار می‌کند

«دانلود ترددها» (با انتخاب ۱/۳/۷/۱۴/۳۰/۹۰/۳۶۵ روز یا همه) برای هر سه گویش پیاده‌سازی شده و همگام‌سازی افزایشی (ادامه از آخرین نقطه) مثل قبل برقرار است.

> ⚠️ نکته: گویش EXT بر اساس تحلیل DLL استخراج شده؛ اگر دستگاه شما در دانلود ترددها گویش متفاوتی داشت، متن خطای فارسی + فریم‌های hex که الان نمایش داده می‌شود را برایمان بفرستید تا دقیقاً سازگارش کنیم — بدون دسترسی به دستگاه هم می‌توانیم از روی hex ادامه دهیم.

## 🧪 تست بدون دستگاه (سه گویش)

```bat
node scripts\mock-sbxpc-device.js                      :: گویش کلاسیک
set DIALECT=ext && node scripts\mock-sbxpc-device.js   :: گویش EXT (رفتار NP756+ واقعی)
set DIALECT=result && node scripts\mock-sbxpc-device.js
```

شبیه‌ساز حالا **دقیقاً مثل دستگاه واقعی شما** به فریم کلاسیک با کد خطای 0x10 جواب می‌دهد تا کل مسیر را بتوان بدون سخت‌افزار تست کرد.

## 📦 نصب و به‌روزرسانی

- **نصب تازه:** زیپ را استخراج کنید → `INSTALL.bat` → `START.bat` → `http://localhost:3000` با `admin/admin123`
- **به‌روزرسانی از v1.2.0:** زیپ جدید را روی محل قبلی استخراج کنید → `INSTALL.bat` (داده‌ها حفظ می‌شوند) → `START.bat`

## 🔄 خلاصه فنی تغییرات

- بازنویسی کامل `src/lib/sbxpc.ts` به کلاینت چندگویشی با فاز probe (تشخیص خودکار)
- پارسرهای خالص و قابل‌تست برای هر سه نوع فریم (ACK کلاسیک، RESULT، ACK بزرگ EXT) با اعتبارسنجی چک‌سام
- اتصال مجدد بین تلاش‌های تشخیص برای ریست پارسر دستگاه
- مستندسازی گویش EXT در `scripts/SBXPC-PROTOCOL.md` (فرمت فریم‌ها + فرمول‌های چک‌سام استخراج‌شده از دیس‌اسمبلی)

</div>

---

**English TL;DR** — v1.2.1 fixes the "پاسخ دستگاه قابل تشخیص نیست / unrecognized device reply" error on real NP756+ units. Deep disassembly of the vendor's `SBPCCOMM64.dll` revealed a second wire dialect (**EXT — 12-byte frames** with different checksums) used by real NP756+ hardware: your device answers classic 16-byte commands with a 12-byte error ack (`aa 55 … 10 …` = "unknown command"). The client now **auto-detects the dialect** on connect (CLASSIC / RESULT_DIRECT / EXT), reconnects between probes to reset the device parser, reads serial + clock in the correct dialect, and surfaces the detected dialect + full raw hex frames in test results and operation logs for diagnosis. Log download and incremental sync work in all dialects. The hardware simulator can now emulate all three (`DIALECT=classic|result|ext`), including the real device's error-0x10 behaviour.
