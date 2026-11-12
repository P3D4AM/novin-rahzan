<div dir="rtl">

# 🔭 حضور+ (HozorPlus) v1.2.6 — رمز درست بود، مشکل جای دیگری بود: ترددها از «کانال پوش» می‌آیند، نه از پورت ۴۲۳۸

## 🧪 نتیجه پروب شما با رمز واقعی ۴۸۹۱۴۸۹

این‌بار که رمز را مستقیم از منوی دستگاه خواندید و گذاشتید ۴۸۹۱۴۸۹، نتیجه تکان‌دهنده اما روشن بود: فرمان ورود (۰x52) با رمز واقعی، با رمز صفر و با رمز غلط ۹۹۹۹۹۹ — هر سه دقیقاً همان ردِ ۱۰ بایتی (code 0x10) را گرفتند. یعنی **ردِ دستگاه به رمز اصلاً ربطی ندارد**؛ فرضیهٔ «اول وارد شو» نسخهٔ v1.2.5 رد شد و پورت ۴۲۳۸ عملاً یک استاب بسته است که فقط فریم کلاسیک را می‌خواند و رد می‌کند. مشکل رمز نبود؛ مشکل این بود که **آن پورت اصلاً جای ترددها نیست.**

## 💡 کشف بزرگ: کانال واقعی دستگاه شما

فایل لاگ نصبِ نوین‌حضور خود شما (`logRealTimeDevice1.txt` — حدود ۸۰۰ تردد واقعی از دستگاه‌تان) نشان می‌دهد دستگاه NP756+ چطور کار می‌کند: **خودش به سرور وصل می‌شود** و هر تردد را همان لحظه «پوش» می‌کند (در لاگ شما: `192.168.1.115:60420`). سپس DLLهای نرم‌افزار را باز کردیم و کل معماری را پیدا کردیم:

```
دستگاه → سرور:  RTLOG001 (magic) + طول + JSON تردد
   {"log_id":N,"user_id":2009,"fk_device_id":"1","io_mode":"1",
    "io_time":"20240118153723","serial_number":"…","verify_mode":"fp"}
سرور → دستگاه: {"log_id":N,"result":"OK"}
```
(خانوادهٔ دوم XML هم دارد — هردو در مستندات پروتکل این نسخه ثبت شده است.)

## 🛠 دو ابزار جدید برای شکستن قفل

- **`scripts/np-sniff.js`** — گوش‌دهندهٔ ترددها: مثل سرور سازنده عمل می‌کند، فریم‌های RTLOG را می‌خواند، خودش جواب OK می‌دهد تا دستگاه به کار ادامه دهد و همه‌چیز (hex + JSON) را در فایل `sniff-*.log` ثبت می‌کند. حالت پروکسی هم برای ترافیک نرم‌افزار↔دارد.
- **`scripts/np-portscan.js`** — اسکنر پورت دستگاه (پورت‌های ۵۰۰۵/۷۰۰۵/۱۴۴۶۰/۴۲۳۸ و…) تا ببینیم دقیقاً چه سرویس‌هایی باز است.

## 🧪 چه کنید (۲ دقیقه)

1. زیپ جدید را روی محل قبلی استخراج کنید.
2. **ثبت ترافیک واقعی دستگاه:**
   ```
   node scripts\np-sniff.js --listen 5005
   ```
   این پنجره باز بماند (فایروال ویندوز اجازه بدهد).
3. در منوی دستگاه، همان تنظیمی که قبلاً به نرم‌افزار نوین‌حضور «آدرس سرور» می‌دادید را به **IP همین کامپیوتر و پورت ۵۰۰۵** تغییر دهید.
4. یک تردد آزمایشی روی دستگاه ثبت کنید — باید در پنجره بالا JSON تردد ظاهر شود.
5. فایل `sniff-*.log` را برایمان بفرستید؛ با آن، **نسخهٔ v1.3.0 گیرندهٔ پوشِ بی‌درنگ** را می‌سازیم و ترددها بدون هیچ نرم‌افزار واسطه‌ای مستقیم داخل حضور+ ثبت می‌شوند.
6. اگر وصل نشد: `node scripts\np-portscan.js <IP دستگاه>` و خروجی را بفرستید (پورت فعلی پوش پیدا می‌شود).

## 🔄 خلاصهٔ فنی

- ردِ 0x10 مستقل از رمز (۳ رمز مختلف → همان پاسخ)؛ ۱۶ واریانت رمزنگاری هم بی‌پاسخ؛ 14460 بسته — همه در `scripts/SBXPC-PROTOCOL.md` مستند شد
- معماری بی‌درنگ نوین‌حضور: `TypeDeviceManufacture` 2=Ebio(XML/5005) · 3=Hysoon(JSON/RTLOG — دستگاه شما) · 5=DynamicHysoon(HTTP) · 6=VisibleLight(HTTP)
- فریم‌بندی RTLOG: `RTLOG001|2|3` + طول LE32 + payload — از دیسمبل RealSvrOcxTcp.ocx (پارسر 0x3730)
- شبیه‌ساز: `DIALECT=rtlog-push` — دستگاه ۷۵۶PLUS پوش‌کننده + ۲۲ تست سبز جدید، ۱۱ تست قبلی هم سبز

</div>

---

**English TL;DR** — The probe with the real device password (4891489) proved the 0x10 refusal is password-independent: port 4238 is a closed stub, not the data channel. The vendor's own shipped log (`logRealTimeDevice1.txt`) reveals the real architecture: the NP756+ **pushes** punches as JSON (`RTLOG001`-framed) over a device-initiated TCP connection (default server port 5005), expecting `{"log_id":N,"result":"OK"}` replies. v1.2.6 ships **np-sniff.js** (a listening capture server that speaks both push dialects — RTLOG JSON and Ebio XML — and logs everything) plus **np-portscan.js**. Run the listener, point the device's "server address" at your PC, punch once, send us `sniff-*.log` — and v1.3.0 gets a built-in real-time push receiver so punches land directly in HozorPlus.
