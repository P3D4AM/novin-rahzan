<div dir="rtl">

# 🧱 حضور+ (HozorPlus) v1.3.0 — مرحلهٔ اول «صحت داده»: RawPunch، بدون تکرار، بدون حذف، بدون افشای رمز

این نسخه مرحلهٔ اول (Data Integrity & Security) از برنامهٔ اتصال دستگاه NP756+ است: **زیرساخت ذخیره‌سازیِ درست، آمادهٔ ورودِ اولین تردد واقعی** — پیش از آنکه Bridge دستگاه (مرحلهٔ دوم) روشن شود.

## چرا این مرحله اول است؟

تجربهٔ v1.2.x نشان داد مسیر قدیمی `np-sync` دو مشکل بنیادی داشت: رکوردهای بدون تطبیق پرسنلی **دور ریخته می‌شدند** و نشانگر (bookmark) از روی همان‌ها رد می‌شد — یعنی داده‌ی گم‌شده‌ی همیشگی. این نسخه آن مسیر را از پایه بازسازی می‌کند:

- **جدول مستقل `RawPunch`** — محل فرود همهٔ ترددها، بدون وابستگی اجباری به تعریف پرسنل. رکورد بدون تطبیق؟ ذخیره می‌شود با وضعیت `UNMATCHED`، قابل مشاهده، و **هیچ‌وقت حذف نمی‌شود**. با افزودن پرسنل بعدی، خودکار تطبیق می‌خورد (`rematchRawPunches` — از همین نسخه در API پرسنل هوک شده).
- **هویت یکتا در برابر تکرار** — سریال دستگاه + پروتکل + شمارهٔ ثبت + اثر انگشت SHA-256 از مقادیر خام + «اندیس هم‌زمانی» (dupIndex). دو رویداد کاملاً یکسان در یک ثانیه، دو رکورد جدا ذخیره می‌شوند؛ خواندن مجدد همان داده‌ها هیچ رکورد جدیدی نمی‌سازد.
- **نشانگر امن** — `Device.rawCursorAt` فقط داخل همان تراکنشِ ذخیره، و فقط پس از نوشتن امن بسته، جلو می‌رود. خطا = هیچ چیز ذخیره نمی‌شود + نشانگر سر جای خودش می‌ماند (تست شده).
- **تاریخچهٔ همگام‌سازی** — هر اجرا یک ردیف `DeviceSyncRun` دارد: وضعیت، شمارنده‌ها، تعداد Retry، خطای پاک‌سازی‌شده. حالت `MOCK` و `HARDWARE` از هم جدا ثبت می‌شوند تا نتیجهٔ آزمایشی هرگز با دادهٔ واقعی اشتباه نشود.
- **ضد خطای سریال** — اگر دستگاه سریالی غیر از سریال پرونده گزارش دهد، دریافت **قبل از ذخیره** متوقف می‌شود؛ هیچ دادهٔ ناهمگانی وارد این دستگاه نمی‌شود.
- **جهت تردد هرگز حدس زده نمی‌شود** — مقادیر خام in/out و verify عیناً ذخیره می‌شوند و جهت تا تأیید معنای رسمی آن‌ها `UNKNOWN` می‌ماند. ساعت خام دستگاه هم کنار زمان UTC نگه داشته می‌شود (`punchTimeRaw`).
- **پایان رمز افشا شده** — رمز «شبکه» از این پس در هیچ پاسخ API (فهرست دستگاه‌ها، bootstrap، نتیجهٔ تست)، هیچ DeviceLog و هیچ پیام خطایی ظاهر نمی‌شود؛ ردیف‌های قدیمی هم هنگام نمایش پاک‌سازی می‌شوند. فرم ویرایش دستگاه بدون خالی‌کردن رمز ذخیره می‌شود (خالی = بدون تغییر).

## 🔧 مسیر قدیمی SBXPC چه شد؟

حذف نشد — بازسازی شد تا از همان پایپ‌لاین جدید عبور کند (`importNpDeviceLogs` حالا از `runRawSync` استفاده می‌کند). ضمناً ۶ ایراد تاریخی آن رفع شد: قفل‌ماندن صفحه‌کلید دستگاه هنگام خطا (فعال‌سازی مجدد تضمین‌شده در `finally`)، حذف رکوردهای بدون تطبیق، جلو رفتن نشانگر از روی دادهٔ ذخیره‌نشده، عدم توقف روی ناهمخوانی سریال، حدس جهت تردد از verify، و نبود تاریخچهٔ اجرا. ZK و Ebio دست‌نخورده باقی مانده‌اند.

## 🗄️ مهاجرت دیتابیس — افزایشی، تکرارپذیر، قابل بازگشت

دو جدول جدید (`RawPunch`، `DeviceSyncRun`) + یک ستون اختیاری (`Device.rawCursorAt`). **هیچ داده/جدول/ستون موجودی تغییر یا حذف نمی‌شود.** نصب جدید همان `prisma db push` نصب را اجرا می‌کند؛ نصب موجود:

```
node scripts\migrate-v1.3.0-phase1.mjs          # اعمال (ایمن — قابل تکرار)
node scripts\migrate-v1.3.0-phase1.mjs status  # وضعیت
node scripts\migrate-v1.3.0-phase1.mjs down --confirm   # بازگشت کامل مرحله
```

## 🧪 نتایج تست — Mock و سخت‌افزار جدا گزارش می‌شوند

- **Mock: ۶۳/۶۳ سبز** — مهاجرت (روی دیتابیس v1.2.6 واقعی + تکرارپذیری + Parity با db push + بازگشت)، ورود داده، داینوپهٔ اثر انگشت، رویدادهای هم‌ثانیه، شکست تراکنشی (ذخیرهٔ صفر + انجماد نشانگر)، Retry با ۲ افت و موفقیت، گارد سریال، پاک‌سازی رمز، rematch، و یک اجرای سرتاسری `importNpDeviceLogs` روی شبیه‌ساز SBXPC (`mock-sbxpc-device.js`) با ۶۰ تردد واقعی روی سوکت.
- **سخت‌افزار: ۰ تست** — NP756+ واقعی در این مرحله آزمایش نشده (این محیط آزمایش، دستگاه ندارد). اولین تست سخت‌افزاری، روشن‌کردن FKAttend Bridge در مرحلهٔ دوم روی همان ویندوزی است که اتصال موفق ۴۲۳۸/۲۹۴۴ تردد را ثبت کرده بود.
- بیلد پروداکشن `next build` سبز؛ lint بدون خطا روی فایل‌های تغییر یافته (۵۶ خطای TypeScript قدیمی و بی‌ربطِ پروژه دست‌نخورده باقی مانده‌اند).

## 🔜 مرحلهٔ بعد (مرحلهٔ دوم)

Bridge x86 با FKAttend.dll روی همان روش موفق آزمایش‌شده (پورت ۴۲۳۸، همان DLLهای ۳۲ بیتی) — اتصال واقعی، خواندن کامل ۲۹۴۴ تردد، ورود به همین پایپ‌لاین، سپس تطبیق رکوردبه‌رکورد با CSV مرجع. APIهای مخرب دستگاه مطلقاً استفاده نمی‌شوند.

</div>

---

**English TL;DR** — v1.3.0 is Phase 1 (Data Integrity & Security) of the NP756+ integration: a new independent `RawPunch` table lands every device punch without requiring a personnel mapping (unmatched rows are kept forever and auto-rematch when staff are added), with SHA-256 fingerprint dedupe that even keeps simultaneous identical punches distinct, a transaction-safe bookmark that only advances after the batch is safely stored, a `DeviceSyncRun` sync history with retry counts and MOCK/HARDWARE separation, a serial-mismatch guard that stops imports before storing foreign data, never-guessed punch directions (raw values preserved), and a full fix of the device-password disclosure (never again in any API response, log or error; historical log rows are redacted on read). The old SBXPC path was rebuilt on top of this pipeline (six historical defects fixed) and ZK/Ebio are untouched. Migration is incremental/idempotent/reversible (`scripts/migrate-v1.3.0-phase1.mjs`). Tests: **63/63 green, all MOCK** (including a full end-to-end run against `mock-sbxpc-device.js`); hardware: **none yet** — that is Phase 2 (the FKAttend x86 bridge) on the Windows box that recorded the successful port-4238 connection.
