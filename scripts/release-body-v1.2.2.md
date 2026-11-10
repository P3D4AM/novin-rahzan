<div dir="rtl">

# 🔐 حضور+ (HozorPlus) v1.2.2 — رمز شبکهٔ NP756+ (پروتکل رمزشدهٔ EXT)

## ⭐ حدس شما درست بود!

شما پرسیدید: «دستگاه یک قسمت به نام رمز شبکه دارد — این خطاها به خاطر آن نیست؟» — **بله، دقیقاً!**

از روی فریم‌های خامی که v1.2.1 نمایش داد، دو واقعیت قطعی دیده می‌شد:

1. دستگاه فریم‌های ما را **کامل و سالم** دریافت می‌کند — چون در پاسخ، چک‌سام خودِ فریم ما را عیناً برمی‌گرداند (`a6 01`)؛
2. ولی با کد خطای `0x10` **ردش** می‌کند و به فریم‌های رمزنشده هم جوابی نمی‌دهد.

این امضای یک گویش چهارم از پروتکل است که در لایه ارتباطی نرم‌افزار اصلی (`SBPCCOMM64.dll` نسخه 3.0.6) پیدا کردیم — **فرمان‌های رمزشده**:

- کلید رمز: `key = رمز شبکه + 0x25B84` (۴ بایت، little-endian)
- کل فریم ۱۲ بایتی دستور/ACK و بلوک‌های داده با `frame[j] ^= key[j % 4]` XOR می‌شوند
- «رمز شبکه» همان آرگومان چهارم `ConnectTcpip(شماره دستگاه، IP، پورت، رمز)` در DLL است — یعنی همان گزینه‌ای که در منوی دستگاه (تنظیمات ← ارتباط ← رمز شبکه) می‌بینید

## ✅ چه چیزی اضافه شد

### ۱. گویش چهارم: `EXT_SECURE` با کشف خودکار رمز

هنگام «تست اتصال»، اگر دستگاه فریم کلاسیک را با ACK ردکردن ۱۰ بایتی جواب داد، نرم‌افزار خودش رمزهای رایج (**۰، ۱۲۳۴۵۶، ۱، ۱۲۳۴، ۹۹۹۹، ۶۵۵۳۵**) را یکی‌یکی روی اتصال‌های تازه امتحان می‌کند تا رمز درست را پیدا کند — بعد سریال، ساعت و دانلود ترددها همه با همان رمزنگاری انجام می‌شود.

### ۲. فیلد «رمز شبکه» در پروندهٔ دستگاه

اگر رمز دستگاهتان هیچ‌کدام از رمزهای رایج نیست، در پروندهٔ دستگاه (ویرایش دستگاه) فیلد **«رمز شبکه»** را پر کنید — دقیقاً همان عددی که در منوی دستگاه می‌بینید. رمزی که خودکار کشف شود هم در پرونده ذخیره می‌شود تا دانلودهای بعدی و همگام‌سازی خودکار مستقیم از همان استفاده کنند.

### ۳. شفافیت بیشتر در نتیجه تست

نتیجهٔ «تست اتصال» حالا **رمز کشف‌شده** را هم نشان می‌دهد و در «تاریخچه عملیات» ثبت می‌شود. اگر هیچ رمزی پذیرفته نشد، پیام فارسی واضح می‌گوید رمز را از منوی دستگاه پیدا کرده و در پرونده وارد کنید.

### ۴. اصلاح چک‌سام EXT (فنی)

چک‌سام فریم‌های ۱۲ بایتی حالا دقیقاً مطابق DLL است: `Σ[2..9] + 0xFF` (نسخه قبل `Σ[0..9]` را می‌داد — عملاً هم‌ارز بود چون `55+AA = 0xFF`، ولی حالا لایه به لایه با دیس‌اسمبلی سازگار است).

## 🧪 تست بدون دستگاه (چهار گویش)

```bat
node scripts\mock-sbxpc-device.js                                   :: کلاسیک
set DIALECT=result && node scripts\mock-sbxpc-device.js             :: بدون ACK
set DIALECT=ext && node scripts\mock-sbxpc-device.js                :: EXT ساده
set DIALECT=extenc && node scripts\mock-sbxpc-device.js             :: رمزشده (رمز ۰)
set DIALECT=extenc&& set PASSWORD=123456&& node scripts\mock-sbxpc-device.js
```

شبیه‌ساز در حالت `extenc` **دقیقاً مثل NP756+ واقعی** رفتار می‌کند: فریم کلاسیک را با ACK ردکردن ۱۰ بایتی (کد 0x10 + echo چک‌سام) جواب می‌دهد، فریم رمزنشده را بی‌پاسخ می‌اندازد و فقط فریم‌های رمزشده با رمز درست را می‌پذیرد.

## 📦 نصب و به‌روزرسانی

- **نصب تازه:** زیپ را استخراج کنید → `INSTALL.bat` → `START.bat` → `http://localhost:3000` با `admin/admin123`
- **به‌روزرسانی از v1.2.1:** زیپ جدید را روی محل قبلی استخراج کنید → `INSTALL.bat` (ستون «رمز شبکه» به دیتابیس اضافه می‌شود؛ همهٔ داده‌ها حفظ می‌شوند) → `START.bat`
- بعد از نصب: **دستگاه‌ها ← تست اتصال** — باید سریال واقعی دستگاه + گویش `EXT_SECURE` + رمز کشف‌شده را ببینید. سپس «دانلود ترددها».

> ⚠️ اگر دستگاه شما رمز شبکهٔ خاصی دارد و در لیست رمزهای رایج نیست، فقط کافی است آن را در پروندهٔ دستگاه (فیلد «رمز شبکه») وارد کنید و دوباره «تست اتصال» بزنید. اگر باز هم خطا داد، متن فارسی خطا + فریم‌های hex که نمایش داده می‌شود را برایمان بفرستید — از روی hex ادامه می‌دهیم.

## 🔄 خلاصه فنی تغییرات

- گویش `EXT_SECURE` در `src/lib/sbxpc.ts`: فریم‌ساز/پارسر رمزشده (XOR با کلید رمز+0x25B84)، probe رمز روی اتصال‌های تازه، خواندن سریال/ساعت/ترددها و ارسال ACK رمزشده
- پارسر جدید «ACK ردکردن ۱۰ بایتی NP» (`aa 55 | mid | 0x10 | echo`) برای تشخیص گویش رمزشده
- ستون `password` (رمز شبکه) در مدل Device + فیلد در فرم دستگاه‌ها + پاس دادن به تست/همگام‌سازی + ذخیرهٔ رمز کشف‌شده
- دانلود حجیم رمزشده با سه شکل پذیرفته‌شدهٔ بلوک داده (تک‌بلوک / تکه‌های ۱۰۲۰ بایتی / فریم کلاسیک رمزشده) و اعتبارسنجی trailer
- مستندسازی کامل گویش EXT_SECURE در `scripts/SBXPC-PROTOCOL.md` با شواهد دیس‌اسمبلی (آدرس‌های دقیق توابع و ثابت 0x25B84)
- شبیه‌ساز: حالت `DIALECT=extenc` با `PASSWORD` قابل تنظیم

</div>

---

**English TL;DR** — v1.2.2 adds the **password-encrypted EXT dialect (EXT_SECURE)** that real NP756+ units actually speak. Your hunch about the device's "network password" (رمز شبکه) was correct: the frame hex proved the device receives our frames intact (it echoes our checksum) but refuses them — because newer NP firmware only accepts frames XOR-encrypted with `key = LE32(password + 0x25B84)`, discovered by disassembling the vendor's own `SBPCCOMM64.dll` (`SendCommandX_EXT` family; the password is the 4th argument of `ConnectTcpip`). The client now detects the 10-byte refusal ack (`aa 55 | mid | 0x10 | cksum-echo`), probes common device passwords (0, 123456, 1, 1234, 9999, 65535) on fresh connections, and speaks the encrypted dialect end-to-end (serial, clock, log download). A new optional **"network password" field** on each device record lets you set the exact رمز شبکه from the device menu; auto-discovered passwords are persisted and reused by syncs. The EXT checksum rule was corrected to match the DLL byte-for-byte (`Σ[2..9]+0xFF`), and the hardware simulator gained `DIALECT=extenc PASSWORD=…` which reproduces the real device's refusal/silence/accept behaviour exactly.
