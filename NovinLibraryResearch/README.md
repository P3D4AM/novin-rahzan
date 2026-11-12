# تحقیقات کتابخانه‌های نوین پرداز — NovinLibraryResearch

این پوشه حاوی خروجی کامل یک تحقیق مهندسی معکوس روی نرم‌افزار حضور و غیاب و حقوق و دستمزد **«نوین پرداز»** (پروژهٔ NPACC) است. بستهٔ اصلی (`NovinDll's.zip`، حدود ۳۵ مگابایت) شامل ۱۳ کتابخانهٔ اختصاصی، ۲۶ کتابخانهٔ `System.*` مایکروسافت و فایل‌های پیکربندی/PDB بود؛ دو بستهٔ بعدی («All novin-dlls» و «Novin-rahzan-dlls») بعدها برای تکمیل پوشش به همین پوشه اضافه شدند (جدول‌های پایین‌تر).

- **پشتهٔ فناوری شناسایی‌شده:** .NET Framework 4.8 / C# / WinForms / SQL Server
- **روش تحقیق:** دیکامپایل ۱۱ کتابخانهٔ مدیریت‌شده با ILSpy + استخراج نمادها از ۲ کتابخانهٔ بومی COM (zkemkeeper و zkemsdk) + تحلیل دستی کد

---

## 🔒 پاک‌سازی حریم خصوصی (Privacy Sanitization)

> **این نسخه از سورس‌ها پاک‌سازی‌شده است.** هر چیزی که حریم خصوصی کاربر نهایی را نقض می‌کرد
> (تله‌متری، توکن‌ها، بک‌دورها و کلیدهای هاردکد) از کد حذف یا خنثی شده و همهٔ تغییرات با
> نشانگر `[SANITIZED]` در خود کد مستند شده‌اند. اسکریپت بازتولیدپذیر این پاک‌سازی:
> `scripts/sanitize_privacy.py`

### آنچه حذف/خنثی شد (۱۰ فایل)

| # | فایل | مورد حذف‌شده |
|---|---|---|
| ۱ | `TimeAttendanceDAL/…/Utility.cs` | متد `SendTelegramMessage` — ارسال نسخهٔ برنامه، سیستم‌عامل، **سریال قفل**، **نام کاربر ویندوز** و **IPهای محلی** به ربات تلگرام سازنده + حذف دو **توکن ربات تلگرام** هاردکد → متد به no-op تبدیل شد |
| ۲ | `TimeAttendanceDAL/…/Utility.cs` | متد `SendTelegramMessageTaradods` — ارسال ترددهای دستگاه‌های خاص به کانال تلگرامی سازنده → no-op |
| ۳ | `TimeAttendanceDAL/…/WebServiceKey.cs` | نشانی پیش‌فرض هاب ابری سازنده (`novinrank.ir/hubCore`) خالی شد |
| ۴ | `TimeAttendanceDAL/…/EntityCacheConfig.cs` | ارسال سریال دستگاه/قفل، IP، نام کاربر اپ و ویندوز هنگام شناسایی دستگاه ناشناس → حذف |
| ۵ | `TimeAttendanceDAL/…/DeviceInfo.cs` | دو مورد ارسال سریال دستگاه و سریال قفل به ربات پایش → حذف |
| ۶ | `TimeAttendanceDAL/…/InternetUpdater.cs` | استخراج و اجرای مخفی `Internet_Update.exe` (رفتار Phone-Home) → غیرفعال |
| ۷ | `TimeAttendanceDAL/…/Person.cs` | متد `GenerateUrlQueryStringAsync` — ارسال نام کاربری و هش رمز شخص به پرتال سازنده → خنثی |
| ۸ | `TimeAttendanceDAL/…/UserPermission.cs` | کلید AES هاردکد برای رمزنگاری سطوح دسترسی → `[REDACTED]` |
| ۹ | `TimeAttendanceUI/…/FrmEnter.cs` | **بک‌دور «رمز استاد»** در فرم ورود (ورود بدون رمز به هر کاربری) → حذف کامل |
| ۱۰ | `TimeAttendanceUI/…/AttendanceRecord.cs` + `EbioRealTimeProcess.cs` | ارسال ترددهای زنده (کد شخص، زمان، دستگاه) به ربات‌های پایش سازنده → حذف |

### در گزارش و اسکریپت‌ها

رمزهای استاد (`Novin********` / `11****60`)، کلید AES (`NovinP****************`) و توکن‌های
تلگرام در **گزارش PDF/HTML/CSV و اسکریپت‌های تولید آن به‌صورت ماسک‌شده** (با حفظ طول رشته تا
صفحه‌بندی تغییر نکند) آمده‌اند — ارزش مستندسازی حفظ شده ولی هیچ رشتهٔ حساس قابل‌استفاده‌ای وجود ندارد.

### محدودیت‌ها

- کلاس‌های `WebTelegramMessage`، `WebErrorLog` و `JaegerMonitor` در اسمبلی خارجی **PacketParser.dll** تعریف شده‌اند که در بستهٔ بررسی‌شده نبود؛ بنابراین **تمام محل‌های فراخوانی** آن‌ها (که در کتابخانه‌های ما بود) خنثی شده‌اند اما خود کلاس‌ها در دسترس نبودند.
- لینک‌های راهنما و بازاریابی عمومی (`novinp.ir`، `novinhozor.com`) صرفاً URL مستندات‌اند و نگه داشته شدند.
- نام دامنهٔ `novinrank.ir` در متن گزارش به‌عنوان «یافتهٔ پژوهشی» باقی مانده (نشانی عمومی است، نه اعتبارنامه)؛ در سورس حذف شده است.

> ⚠️ **توصیه:** توکن ربات تلگرام و هر سرویس دیگری که در بستهٔ اصلی لو رفته بود، پیش‌تر از طریق
> لینک file.kiwi عمومی شده بود؛ حتماً از BotFather هر دو توکن را Revoke کنید.

---

## ساختار پوشه

| مسیر | محتوا |
|---|---|
| `report/` | گزارش نهایی: **PDF ۴۱ صفحه‌ای**، نسخهٔ HTML قابل ویرایش، خلاصهٔ CSV کتابخانه‌ها و فونت‌های Vazirmatn |
| `scripts/` | اسکریپت‌های خط تولید گزارش + `sanitize_privacy.py` (پاک‌سازی بازتولیدپذیر) |
| `decompiled/` | سورس **پاک‌سازی‌شدهٔ** ۱۱ کتابخانهٔ مدیریت‌شدهٔ دیکامپایل‌شده با ILSpy (۶۲۵ فایل) |
| `decompiled-complete/` | دیکامپایل کامل هر ۳ بسته — مجموعاً **۲۲۰ کتابخانهٔ مدیریتی** (بستهٔ ۱: ۳۶ عدد، بستهٔ ۲: ۲۰ عدد، بستهٔ ۳: ۱۶۴ عدد) + آرشیو خام ۲۳۹ فایل DLL در `_dlls/` |
| `original dll/` *(در ریشهٔ ریپو)* | آرشیو خام ۲۳۹ فایل DLL بستهٔ سوم — عین فایل‌های آپلودشده در filebin، بدون هیچ تغییری (همان بایت‌های `_dlls/`) |

### فایل‌های گزارش

| فایل | توضیح |
|---|---|
| `report/گزارش_تحلیل_کتابخانه_ها.pdf` | گزارش اصلی — ۱۳ فصل، ۶۰+ قطعه کد واقعی، نمودار معماری، جداول کلاس‌ها، جعبه‌های آموزشی |
| `report/گزارش_تحلیل_کتابخانه_ها.html` | نسخهٔ HTML همان گزارش (فونت‌ها را از `report/fonts/` می‌خواند) |
| `report/خلاصه_کتابخانه_ها.csv` | جدول خلاصهٔ ۴۱ فایل: نام، حجم، دسته، نقش، کلاس‌های کلیدی، متدهای مهم و نکات |

### پوشهٔ `decompiled-complete/` — دیکامپایل کامل بسته

این پوشه خروجی دیکامپایل **تمام کتابخانه‌های مدیریتی** بستهٔ `NovinDll's.zip` است (همان بستهٔ دریافتی از لینک file.kiwi):

| مورد | توضیح |
|---|---|
| منبع | بستهٔ `NovinDll's.zip` — ۳۸ فایل DLL: ۳۶ کتابخانهٔ مدیریتی (.NET) + ۲ کتابخانهٔ COM بومی |
| ابزار | `ilspycmd 8.2.0.7535` — دقیقاً همان نسخهٔ تولید `decompiled/` (خروجی deterministic) |
| محتوا | ۱۱ کتابخانهٔ اختصاصی + ۲۵ کتابخانهٔ `System.*` مایکروسافت — مجموع **۱٬۶۱۶ فایل** (۱٬۵۰۸ فایل `.cs`) |
| پاک‌سازی | هر دو مرحلهٔ `sanitize_privacy.py` (pass 1 + pass 2) به‌طور کامل اعمال شد؛ سورس ۱۱ کتابخانهٔ اختصاصی **بایت‌به‌بایت همان پوشهٔ `decompiled/`** است |
| اسکن امنیتی | کل ۱٬۶۱۶ فایل برای توکن تلگرام، رمزهای استاد، کلید AES، دامنه‌های سازنده و کانال ربات اسکن شد — **صفر مورد** |
| خارج از پوشش | `zkemkeeper.dll` و `zkemsdk.dll` (COM بومی ۳۲ بیتی — با ILSpy قابل دیکامپایل نیستند) و فایل‌های `.pdb` / `.config` |
| بازتولید | `bash scripts/decompile_complete.sh` سپس `python3 scripts/sanitize_privacy.py sources <مسیر decompiled-complete>` |

### بستهٔ دوم «All novin-dlls» (۲۰ کتابخانه)

| مورد | توضیح |
|---|---|
| منبع | بستهٔ `All novin-dlls` — ۲۰ کتابخانهٔ اختصاصی/جانبی (Stimulsoft.Report و Win، LK، AccSqlServerPersistence، EntityCacheStandard، PacketParserStandard، BehComponents، DevComponents.DotNetBar2 و…) |
| ابزار | `ilspycmd 8.2.0.7535` — همان نسخهٔ بستهٔ ۱ |
| پاک‌سازی | اعمال نشد — سورس‌ها مطابق خروجی خام ILSpy کامیت شدند |
| بازتولید | `bash scripts/decompile_alldlls.sh` |

### بستهٔ سوم «Novin-rahzan-dlls» — پوشش کامل پوشهٔ نصب (۲۳۹ DLL)

| مورد | توضیح |
|---|---|
| منبع | بستهٔ `Novin-rahzan-dlls.zip` (~۷۳ مگابایت، ۲۳۹ فایل DLL) — پوشش کامل DLLهای پوشهٔ نصب نرم‌افزار |
| ترکیب | ۲۲۰ اسمبلی مدیریت‌شدهٔ .NET + ۱۹ DLL بومی ۳۲/۶۴ بیتی (FKAttend، SBXPCDLL، zkemkeeper، zkemsdk، GEN_FONT، brolib، FaceDataConv، SNI و…) |
| عملیات | **۵۶ کتابخانهٔ تکراری** با بسته‌های ۱ و ۲ skip شدند؛ **۱۶۴ کتابخانهٔ جدید** دیکامپایل شدند (~۲۳٬۷۰۰ فایل: ۱۵٬۵۰۰+ فایل `.cs` + منابع) |
| ابزار | `ilspycmd 8.2.0.7535` — همان نسخهٔ بسته‌های ۱ و ۲ (خروجی deterministic) |
| کتابخانه‌های شاخص جدید | Stimulsoft.Report.Design/Helper/Web/Check/Dashboard/Editor/Blockly، Microsoft.SqlServer.Smo/SqlParser/SqlEnum (مجموعهٔ کامل SMO)، Entity Framework Core + SQL Server، ASP.NET Core SignalR (کلاینت/سرور)، Serilog + همهٔ Sinkها، Microsoft.Identity.Client، Newtonsoft.Json، Ninject، Azure.Core/Identity، ExcelDataReader، Smartgroup، mCoreLib و کتابخانه‌های اختصاصی سازنده: Server.Core، ActivationPacketParserStandard، PayamakSettings، RegisterManagement، SbLogServer، SignalRCoreConnectionCore، Riss.Devices_ZDC2911 |
| آرشیو باینری | هر ۲۳۹ فایل DLL خام (بدون تغییر) در `decompiled-complete/_dlls/` و همچنین در پوشهٔ `original dll/` در ریشهٔ ریپو نگهداری می‌شوند — تنها نسخهٔ موجود از ۱۹ DLL بومیِ غیرقابل‌دیکامپایل نیز همینجاست |
| اسکن امنیتی | کل ۲۳٬۸۰۰ فایل سورس جدید برای توکن تلگرام (الگوی `\d{8,10}:AA…`)، رمزهای استاد، کلید AES و دامنه‌های سازنده اسکن شد — **صفر توکن/رمز هاردکد در کتابخانه‌های جدید** |
| یافته‌های غیرحساس | `ActivationPacketParserStandard/SharedActivation/ApiLoginParamter.cs` نقطهٔ پایانی فعال‌سازی `novinrank.ir` را دارد (نشانی عمومی، بدون اعتبارنامه)؛ `Server.Core` در ۸ فایل از `WebTelegramMessage.…bot().Sent2web(...)` برای پایش عملیاتی استفاده می‌کند — خودِ کلاس و توکن‌های آن در `PacketParserStandard` بستهٔ ۲ تعریف شده‌اند |
| بازتولید | `bash scripts/decompile_rahzan.sh <مسیر-DLLها>` (تشخیص خودکار managed/native با `scripts/classify_dlls.py` و skip خودکار دیکامپایل‌شده‌ها) |

---

## فصل‌های گزارش

1. مقدمه؛ بسته‌ای که بررسی کردیم
2. معماری کلان سیستم؛ نقشهٔ راه فنی
3. کتابخانهٔ TimeAttendanceDAL؛ قلب تپندهٔ سیستم
4. کتابخانهٔ TimeAttendanceDevices؛ صحبت با دستگاه‌ها
5. کتابخانهٔ TimeAttendanceUI؛ چهرهٔ نرم‌افزار
6. کتابخانهٔ TimeAttendancePrint؛ چاپ و گزارش‌سازی
7. کتابخانهٔ ValidatorAttribute؛ نگهبان داده‌ها
8. کتابخانهٔ Tele؛ مودم صوتی و تردد تلفنی
9. کتابخانهٔ WebSmsStandard؛ ارسال پیامک
10. کتابخانه‌های ZK، ZDC2911 و zkemkeeper؛ دنیای بومی
11. کتابخانه‌های جانبی؛ اعلان و انیمیشن (Tulpep / WinFormAnimation)
12. کتابخانه‌های System.* مایکروسافت؛ زیرساخت
13. جمع‌بندی؛ درس‌ها و یافته‌های امنیتی

---

## یافته‌های کلیدی

### معماری
- **معماری سه‌لایه:** `TimeAttendanceUI` (فرم‌های WinForms) ← `TimeAttendanceDAL` (دسترسی داده) ← SQL Server
- **پشتیبانی از ۶ برند دستگاه حضور و غیاب:** ZKTeco، Hysoon (دو نسل: static و Dynamic/RealTime)، Ebio، Eizon، ZDC2911 و NP761
- **پایگاه داده:** نام‌گذاری `Attendance0` تا `Attendance99` برای سال‌های شمسی
- **همگام‌سازی ابری:** SignalR Hub روی دامنهٔ `novinrank.ir` (فایل `WebServiceKey.cs` — پیش‌فرض پاک‌سازی شد)
- **تقویم شمسی:** تاریخ مرجع ۱۳۰۰/۰۱/۰۱ برای محاسبات سال/ماه

### نکات امنیتی (همگی در این نسخه پاک‌سازی شده‌اند)
- 🔴 ~~رمزهای Master هاردکد~~ در `FrmEnter.cs` → بک‌دور حذف شد (`Novin********` / `11****60`)
- 🔴 ~~کلید AES هاردکد~~ در `UserPermission.cs` → `[REDACTED]` شد
- 🔴 **SQL Injection** در کلاس `Shifts.cs` (الحاق رشته مستقیم به کوئری) — *نکتهٔ آموزشی، در سورس باقی است*
- 🔴 ~~توکن‌های ربات تلگرام~~ → متدهای ارسال no-op شدند
- 🔴 ~~تله‌متری نام کاربر/IP/سریال قفل~~ → همهٔ نقاط ارسال حذف شدند
- 🔴 ~~کالبک لاگ دستگاه به دامنهٔ سازنده~~ در `FrmDevice.cs` (حالت غیر RealTime لاگ ترددها به سرور سازنده می‌رفت) → حذف شد *(پاک‌سازی مرحلهٔ ۲)*
- 🔴 ~~نشانی‌های هاب ابری سازنده در فهرست تنظیمات~~ در `FrmSetting.cs` → تنها `localhost` باقی ماند *(پاک‌سازی مرحلهٔ ۲)*
- 🟡 رمزها و رشته‌های اتصال در `app.config` بدون رمزنگاری معنادار

### نکات فنی جالب
- پروتکل Hysoon با **magic number** `0x18181818`
- سرور HTTP داخلی برای ارتباط RealTime دستگاه‌های Dynamic Hysoon
- کتابخانهٔ `Tele` برای مودم‌های صوتی با دستگاه تشخیص شماره (CLS Number Detector)

---

## کتابخانه‌های اختصاصی (۱۳ عدد)

| کتابخانه | حجم | نقش |
|---|---|---|
| `TimeAttendanceUI.dll` | 15.2 MB | لایهٔ نمایش؛ صدها فرم WinForms |
| `TimeAttendanceDAL.dll` | 6.3 MB | لایهٔ داده؛ DAL + Mapper + View + JsonConverter |
| `TimeAttendancePrint.dll` | 2.2 MB | چاپ و گزارش‌سازی (Stimulsoft) |
| `TimeAttendanceDevices.dll` | 294 KB | ارتباط با ۶ برند دستگاه |
| `TimeAttendance.ValidatorAttribute.dll` | 77 KB | اعتبارسنجی مدل‌ها با Attribute |
| `ZDC2911.dll` | 116 KB | درایور دستگاه ZDC2911 |
| `zkemkeeper.dll` | 652 KB | COM بومی ZKTeco |
| `zkemsdk.dll` | 206 KB | COM بومی ZKTeco |
| `WebSmsStandard.dll` | 26 KB | ارسال پیامک REST |
| `Tele.dll` | 23 KB | مودم صوتی / AT Command |
| `Tulpep.NotificationWindow.dll` | 28 KB | اعلان Popup ویندوز |
| `WinFormAnimation.dll` | 38 KB | انیمیشن فرم‌ها |
| `ZK.dll` | 10 KB | Wrapper دات‌نت zkemkeeper |

---

## نحوهٔ تولید مجدد گزارش

```bash
# 1) دیکامپایل کتابخانه‌ها (نیازمند dotnet + ilspycmd)
bash scripts/decompile_all.sh

# 2) پاک‌سازی حریم خصوصی از سورس‌ها (بازتولیدپذیر؛ شامل هر دو مرحله)
python3 scripts/sanitize_privacy.py sources <مسیر decompiled>

# 3) تولید HTML و CSV (نیازمند Python 3؛ برای PDF نیازمند Chromium/Playwright + reportlab + pypdf)
python3 scripts/build_report.py html2 && python3 scripts/build_report.py csv

# 4) رندر PDF و شماره‌گذاری صفحات
node html2pdf-next.js report/گزارش_تحلیل_کتابخانه_ها.html --nopaged
python3 scripts/stamp_pages.py
```

> اسکریپت `sanitize_privacy.py` همچنین `scripts` و `verify` را می‌پذیرد: حالت اول رشته‌های حساس
> اسکریپت‌های گزارش را ماسک می‌کند و حالت دوم کل درخت را برای اطمینان از نبود رشتهٔ حساس اسکن می‌کند.
> مسیرهای داخل اسکریپت‌ها مطابق محیط اجرای اولیه (سندباکس) تنظیم شده‌اند؛ برای اجرای محلی تطبیق دهید.

## ابزارهای استفاده‌شده

- **ILSpy** (`ilspycmd 8.2`) — دیکامپایل اسمبلی‌های .NET
- **Python 3** + `reportlab` + `pypdf` — ساختاردهی، شماره‌گذاری و متادیتای PDF
- **Chromium (headless)** — تبدیل HTML به PDF با صفحه‌بندی راست‌به‌چپ
- **فونت Vazirmatn** — تایپوگرافی فارسی گزارش
