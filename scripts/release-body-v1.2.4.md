<div dir="rtl">

# 🔧 حضور+ (HozorPlus) v1.2.4 — ابزار np-probe این‌بار واقعاً داخل بسته + کشف پروتکل HTTP دستگاه‌های NP جدید

## 🙏 اول: عذرخواهی

در v1.2.3 نوشته بودیم ابزار `scripts\np-probe.js` داخل بسته است — **نبود**. فایل در پوشهٔ کاری ساخت ساخته شده بود ولی موقع بسته‌بندی جا افتاده بود. در این نسخه حقیقتاً داخل زیپ است (بررسی شد) و جداگانه هم برایتان گذاشته‌ایم.

## 🔬 کشف بزرگ این نسخه: API HTTP دستگاه‌های NP

وقتی رمز شما (۱۲۳۴۵) هم درست ارسال شد و دستگاه باز سکوت کرد، دیگهٔ DLLهای نرم‌افزار اصلی را تا ته باز کردیم و به نتیجهٔ قطعی رسیدیم:

- **مسیر کلاسیکِ DLL هیچ‌وقت رمزنگاری نمی‌کند** و نرم‌افزار خود فروشنده هم فقط توابع کلاسیک را فرامی‌خواند — یعنی نرم‌افزار فروشنده هم هرگز نمی‌تواند با دستگاهِ «رمزدار» از مسیر باینری صحبت کند!
- نسل جدید دستگاه‌های NP (NP756P/NP761P و خانوادهٔ «VisibleLight») یک **API کامل HTTP/JSON روی پورت 14460** دارند — همین است که نرم‌افزار فروشنده واقعاً با آن‌ها کار می‌کند:
  `POST /getDeviceKey`، `/getLogCount`، `/getLogInfo`، `/getDeviceTime` و… با بدنهٔ `{"pass": رمز شبکه}`
- پورت باینری این دستگاه‌ها فقط یک «جاگیر» قدیمی است: فریم کلاسیک را با همان پاسخ ردِ ۱۰بایتی (که شما دیدید: `aa 55 … 10 00 …`) برمی‌گرداند و بقیه را ساکت رها می‌کند — **دقیقاً اثر انگشتی که دستگاه شما نشان داد.**

## ✅ چه چیزی در v1.2.4 تغییر کرد

1. **«تست اتصال» حالا API HTTP را هم خودکار امتحان می‌کند** — اگر دستگاه شما HTTP داشته باشد، تست موفق می‌شود و سریال + ساعت + تعداد ترددهای ذخیره‌شده روی دستگاه را نشان می‌دهد (برچسب گویش: «API HTTP — نسل جدید NP»). حتی اگر پورت باینری اصلاً باز نباشد، تست قبل از شکست، HTTP را امتحان می‌کند.
2. **گویش فرضی «کلاسیکِ رمزشده»** هم خودکار آزموده می‌شود — اگر دستگاه شما آن باشد، تست موفق می‌شود و سریال/ساعت خوانده می‌شود.
3. **`scripts\np-probe.js` — داخل بسته!** ابزار تشخیص مستقل: همهٔ گویش‌ها را روی دستگاه واقعی امتحان می‌کند و پاسخ hex/JSON هر تلاش را چاپ می‌کند (۲۰+ واریانت باینری + HTTP + UDP).
4. خواندن ترددها روی پروتکل HTTP → **نسخهٔ بعدی**: قالب رکوردهای `/getLogInfo` را باید از پاسخ دستگاهِ واقعی شما ببینیم تا دقیق و بدون دادهٔ خراب پیاده‌اش کنیم — به همین دلیل خروجی np-probe برای ما طلاست.

## 🧪 دقیقاً چه کنید (۵ دقیقه)

1. زیپ جدید را روی محل قبلی استخراج کنید → `INSTALL.bat` → `START.bat`
2. **دستگاه‌ها ← تست اتصال** — خیلی محتمل است همین‌جا موفق شود (برچسب «API HTTP» را ببینید)
3. اگر خطا داد، در پوشهٔ برنامه اجرا کنید و خروجی را کامل برایمان بفرستید:
   ```bat
   node scripts\np-probe.js <IP دستگاه> <پورت> 12345
   ```
   (IP و پورت همان‌هایی که در پرونده دستگاه وارد کرده‌اید؛ ابزار خودش پورت 14460 را هم جدا امتحان می‌کند)

## 🔄 خلاصهٔ فنی

- `tryHttpApi()`: POST :14460 `/getDeviceKey` با `{"pass":N}` (عدد و رشته) → `/getDeviceTime` + `/getLogCount` → گویش `HTTP_JSON`
- `tryClassicSecure()`: کل تبادل کلاسیک رمزشده با کلید LE32(رمز) — فریم‌به‌فریم رمزگشایی (ACK 8B / RESULT 14B / DATA)
- ثابت ماندن رمز تنظیم‌شده بعد از جست‌وجوی ناموفق گویش‌ها (باگ v1.2.3)
- `scripts/np-probe.js` — ۱۶ واریانت باینری + HTTP(:14460) + UDP، خروجی فارسی با جدول جمع‌بندی و «نمای رمزگشایی‌شده»
- مستندات پروتکل: جدول کامل ارسال رمزِ 0x4630 بر اساس نوع لینک (TCP=رمز+نمک، COM/USB=نمک تنها) و فهرست کامل endpoint های HTTP از PacketParserStandard.dll

</div>

---

**English TL;DR** — v1.2.3 promised `scripts/np-probe.js` but the file never made it into the zip — fixed (it is in this one, verified). Bigger: deeper RE of the vendor DLLs proves the vendor's own software NEVER encrypts the classic path and only imports classic functions — so password-protected NP face devices are reached by the vendor through a **JSON/HTTP API on port 14460** (`/getDeviceKey`, `/getLogCount`, `/getLogInfo`, … with `{"pass":N}` bodies — the "VisibleLight" NP756P/NP761P generation). That matches your NP756+ fingerprint exactly (classic → 10-byte refusal; encrypted EXT with the correct password → silence). The app now auto-probes that API in «تست اتصال» (serial + clock + stored log count), also tries the encrypted-classic hypothesis, and falls back to HTTP when the binary port is unreachable. Log download over HTTP lands in the next release once we see your device's real `/getLogInfo` response — run `node scripts\np-probe.js <ip> <port> 12345` and send the full output.
