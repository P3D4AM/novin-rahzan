# NP756P.ReadOnlyProbe — ابزار تشخیصی Read-Only برای دستگاه NP756P

ابزار کنسولی مستقل و فقط-خواندنی برای بررسی برقراری ارتباط با دستگاه حضور و غیاب **Novin Pardaz NP756P** از طریق SDK رسمی سازنده (زنجیره `FK623Attend.dll`). این ابزار **هیچ تغییری در دستگاه ایجاد نمی‌کند**: نه ساعت، نه زبان، نه کاربران، نه سوابق تردد، نه تنظیمات شبکه.

---

## ۱) هدف ابزار

- تایید اینکه زنجیره DLLهای SDK صحیح و قابل بارگذاری است (تحلیل ایستا، بدون اجرای DLL):
  - معماری x86 هر ۷ فایل؛
  - Export هر ۶ تابع مجاز از `FK623Attend.dll`؛
  - حل‌شدن وابستگی‌ها؛
  - **تطبیق Importهای نام‌دار و Ordinal-based با جدول Export DLL مقصد** (FK623Attend.dll مثلاً ۱۶۰ تابع را با Ordinal از FKAttend.dll وارد می‌کند — Ordinal مفقود = شکست اعتبارسنجی با گزارش مبدأ/مقصد/Ordinal).
- برقراری یک اتصال آزمایشی با `FK_ConnectNet`.
- خواندن **فقط** این اطلاعات پایه:
  - شماره سریال (`FK_GetProductData` اندیس ۱) و کد محصول (اندیس ۳)
  - شماره دستگاه (`FK_GetDeviceInfo` اندیس ۲) و زبان (اندیس ۳)
  - ساعت دستگاه (`FK_GetDeviceTime`)
- قطع اتصال در `finally` در هر شرایطی.
- گزارش ساخت‌یافته و قابل ارسال برای پشتیبانی/توسعه‌دهنده **بدون افشای رمز ارتباط**.

## ۲) معماری و SDK مورد استفاده

| مورد | مقدار |
|---|---|
| مدل هدف | NP756P — شناسه مدل ۹۳۰۸، `DeviceManufacture = 3` (Hysoon کلاسیک) |
| مسیر پروتکل | `clsHysoon` + `FK623Attend.dll` (**نه** `ClsDynamicHysoon`) |
| فریم‌ورک | .NET Framework 4.8 |
| معماری | x86 (پلتفرم هدف x86؛ DLLهای SDK باینری ۳۲ بیتی‌اند) |
| نوع خروجی | `NP756P.ReadOnlyProbe.exe` (کنسول) |
| وابستگی‌های مدیریت‌شده | فقط BCL دات‌نت — هیچ اسمبلی از نرم‌افزار Novin ارجاع نمی‌شود |

> از اجرای فرایند ۶۴ بیتی با DLLهای ۳۲ بیتی خودداری کنید؛ اگر فرایند x64 باشد بارگذاری SDK با `BadImageFormatException` شکست می‌خورد (ابزار در مرحلهٔ A این را تشخیص می‌دهد و گزارش می‌کند).

## ۳) وابستگی DLLهای Native

حداقل مجموعهٔ لازم برای بارگذاری `FK623Attend.dll` (بر اساس تحلیل Import/Export Table واقعی فایل‌ها):

```
Application → FK623Attend.dll → FKAttend.dll → FKViaDev.dll
                                              → LFWViaDev.dll
                                              → FKPwdEncDec.dll
                                              → FpDataConv.dll
                                              → FaceDataConv.dll
                              (+ DLLهای سیستم: kernel32, ws2_32, gdi32, gdiplus, ...)
```

نکتهٔ مهم: `FK623Attend.dll` علاوه بر `FK_GetLastError` (نام‌دار)، **۱۶۰ تابع را با شمارهٔ Ordinal** از `FKAttend.dll` وارد می‌کند و `FKAttend.dll` نیز توابعی را با Ordinal از `FKPwdEncDec.dll`/`FpDataConv.dll`/`FaceDataConv.dll` و با نام از `FKViaDev.dll`/`LFWViaDev.dll` وارد می‌کند. Validator مرحلهٔ A همهٔ این Importها را با جدول Export مقصد تطبیق می‌دهد.

مسیر این فایل‌ها در ریپو: `NovinLibraryResearch/decompiled-complete/_dlls`

**کپی کورکورانه انجام نشده**؛ ابزار فقط این هفت فایل را کنار خود جست‌وجو می‌کند (یا مسیر را با `--dll-dir` می‌گیرد) و درستی معماری x86، حل‌شدن وابستگی‌ها و Import/Export Chain را به‌صورت ایستا بررسی می‌کند. فایل غیر-PE، فایل قطع‌شده/خراب، معماری نامناسب، IOException هنگام خواندن و Ordinal/تابع مفقود، همگی به‌عنوان شکست اعتبارسنجی گزارش می‌شوند (رفتار پیش‌فرضِ Validator همیشه شکست است).

## ۴) پیش‌نیازهای ویندوز

- ویندوز ۳۲/۶۴ بیتی با دسترسی به دستگاه در شبکه.
- .NET Framework 4.8 runtime (در ویندوز ۱۰/۱۱ معمولاً موجود است).
- هفت DLL فروشنده در کنار exe (یا مسیرشان را با `--dll-dir` بدهید).
- مجوز خروجی TCP از فایروال به سمت IP دستگاه.

## ۵) روش Build

روش پیشنهادی (Visual Studio / ویندوز):

```
msbuild tools\NP756P.ReadOnlyProbe\NP756P.ReadOnlyProbe.csproj /p:Configuration=Release /p:Platform=AnyCPU
```
(PlatformTarget در csproj روی x86 قفل شده است؛ AnyCPU در سطح Platform پروژه مشکلی ندارد چون خروجی مونو است و PlatformTarget=x86 اجرا را در فرایند ۳۲ بیتی اجبار می‌کند.)

روش Build بدون ویندوز (لینوکس/CI — با reference assemblies):

```
dotnet build -c Release tools/NP756P.ReadOnlyProbe/NP756P.ReadOnlyProbe.csproj
```

پکیج `Microsoft.NETFramework.ReferenceAssemblies` (PrivateAssets) اجازه می‌دهد IL برای net48 بدون نصب Targeting Pack تولید شود. **Build موفق به معنی اجرای موفق روی دستگاه نیست.**

## ۶) روش اجرای برنامه

۱. exe را در پوشه‌ای کنار هفت DLL بالا قرار دهید (یا `--dll-dir` بدهید).
۲. یک پنجره cmd باز کنید:

```
NP756P.ReadOnlyProbe.exe --ip 192.168.1.75 --port 4238
```

۳. رمز عددی ارتباط دستگاه را در پیام راه‌اندازی وارد کنید (ورودی مخفی است).
۴. خروجی مرحله‌بندی‌شده [1]..[7] را چاپ و ضبط کنید.

> **کنسول Redirect شده:** اگر ورودی برنامه piped/redirect شده باشد (مثلاً `echo 1234 | ...`)، اعلان رمز مخفی قابل اجرا نیست؛ برنامه به‌جای crash پیام خطای قابل فهم می‌دهد و با کد خروج `1` پایان می‌یابد. ابزار را در پنجرهٔ تعاملی cmd/PowerShell اجرا کنید.

## ۷) نمونهٔ اجرا با پارامترها

```
NP756P.ReadOnlyProbe.exe --ip 192.168.1.75 --port 4238 --machine 1 --timeout 5000 --protocol tcp
NP756P.ReadOnlyProbe.exe --ip 192.168.1.75 --port 4238 --dll-dir C:\Vendor\_dlls
NP756P.ReadOnlyProbe.exe --help
```

مقادیر پیش‌فرض: `ip=192.168.1.75` · `port=4238` · `machine=1` · `timeout=5000` · `protocol=tcp` · `license=1261`.

## ۸) وارد کردن رمز به‌صورت مخفی

- رمز فقط از طریق ورودی کنسولِ **بدون اکو** (نمایش `*`) دریافت می‌شود.
- رمز باید **عدد صحیح** باشد؛ ورودی خالی یا غیرعددی با پیام خطا رد می‌شود (بدون چاپ مقدار).
- رمز به‌هیچ‌وجه در لاگ، گزارش تشخیصی، متن استثنا، `ToString()` گزینه‌ها یا Debug Output ظاهر نمی‌شود (تست واحد مخصوص دارد).
- رمز یا هر اطلاعات محرمانه در Git Commit قرار نمی‌گیرد.

## ۹) تفسیر نتیجهٔ اتصال

- `Connect Result: SUCCESS (handle=N)` — N بزرگ‌تر از صفر یعنی اتصال SDK برقرار است (همان شرط نمونهٔ رسمی فروشنده).
- `Connect Result: FAILED (SDK returned -2)` — مقدار برگشتی واقعی SDK؛ معنای متنی هر کد در گزارش چاپ می‌شود (بخش ۱۰).
- `Connect Result: FAILED (exception: <نوع> - no SDK return value)` — اتصال با استثنا شکست خورده؛ در این حالت **هیچ Raw Code یا Last Error ثبت نمی‌شود** تا عدد صفر به‌اشتباه «No support» تلقی نشود.
- `Elapsed` مدت واقعی فراخوانی اتصال به میلی‌ثانیه.
- شکست اتصال **هرگز** به تلاش مجدد خودکار، تعویض رمز یا اسکن پورت منجر نمی‌شود.

## ۱۰) کدهای خطای مهم (از سورس رسمی `ReturnResultPrint`)

| کد | معنی |
|---|---|
| 1 | Successful |
| 0 | No support |
| -1 | Unknown error |
| -2 | No Open Comm (اتصال شبکه برقرار نشد) |
| -3 | Write Error |
| -4 | Read Error |
| -5 | Parameter Error |
| -6 | Execution of command failed |
| -7 | End of data |
| -8 | Nonexistence data |
| -9 | Memory Allocating Error |
| -10 | License Error |
| -11 | Full enrolldata |
| -12 | This id is already existed |
| -14 | Full manager |
| -15 | Mistake fp data version |
| -13 | در سورس دیکامپایل‌شده متنی ندارد؛ «Unmapped» گزارش می‌شود |

**کدهای خروج برنامه (با رفتار واقعی `Program` تطبیق داده شده):**

| کد | شرط |
|---|---|
| 0 | همهٔ مراحل موفق **و** اتصال واقعاً با `FK_DisConnect` بسته شده (یا از ابتدا هندل معتبری وجود نداشته) |
| 1 | خطای ورودی/رمز — یا کنسول Redirect شده (بدون crash) |
| 2 | شکست مرحلهٔ DLLها (فایل مفقود/غیر-PE/خراب/معماری نامناسب/Import-Export حل‌نشده) |
| 3 | شکست اتصال (مقدار برگشتی ≤ 0 یا استثنا در `FK_ConnectNet`) |
| 4 | شکست مرحلهٔ خواندن — یا شکست خودِ `FK_DisConnect` (خطای قطع اتصال هرگز با خروجی 0 همراه نیست) |
| 5 | استثنای پیش‌بینی‌نشدهٔ خارج از جریان عادی؛ با catch سراسری کنترل می‌شود (فقط نوع استثنا + پیام کوتاه، بدون Stack Trace/Inner) |

## ۱۱) محدودیت‌های نسخه

- فقط خواندن؛ هیچ عملیات نوشتنی انجام نمی‌شود.
- رمز جایگزین و اسکن پورت خودکار ممنوع و پیاده‌سازی نشده.
- در شکست خواندن، **هیچ فرمان اضافه‌ای برای «رفع خطا»** ارسال نمی‌شود؛ فقط علت گزارش می‌شود.
- کاربران/بیومتریک/سوابق تردد دریافت نمی‌شوند.
- `FK_GetLastError` فقط با هندل معتبر و پس از شکست یک خواندن فراخوانی می‌شود.
- مراحلِ اجرا نشده در گزارش `SKIPPED` هستند، نه `FAILED`؛ تابعِ دقیقاً شکست‌خورده در FAILURE DETAILS با نام کامل (مثلاً `FK_GetProductData(1)`) مشخص می‌شود.
- پارسر PE ابزار PE32/PE32+ استاندارد را پوشش می‌دهد؛ Delay-Load/Forwarder/Bound و DLLهای سیستم را نه (مستندات پژوهش، بخش ۵ را ببینید).
- تست‌های خودکار هیچ DLL Native را **اجرا** نمی‌کنند (دو DLL واقعی FK623Attend/FKAttend فقط برای Parse ایستا به‌عنوان دادهٔ مرجع استفاده می‌شوند) و به هیچ دستگاهی وصل نمی‌شوند.

## ۱۲) توابعی که عمداً اجرا نمی‌شوند (ایمنی)

```text
FK_SetDeviceTime      FK_SetDeviceInfo      FK_EnableDevice
FK_EmptyEnrollData    FK_EmptyGeneralLogData  FK_EmptySuperLogData
FK_DeleteEnrollData   FK_PutEnrollData       FK_SetUserName
FK_SetEnrollPhoto    FK_SetServerNetInfo   FK_SetRealTimeInfo
FK_HS_SetTimeZone     FK_HS_SetUserWeekPassTime
```

این توابع حتی به‌صورت DllImport در کد تعریف نشده‌اند تا از فراخوانی تصادفی در هر شرایطی جلوگیری شود (تست واحدِ بازتابی، مجموعهٔ P/Invoke را با فهرست مجاز تطبیق می‌دهد).

## ۱۳) ارسال گزارش بدون افشای رمز

1. خروجی کامل کنسول را (همهٔ بخش‌های `[1]`..`[7]` و FAILURE DETAILS) کپی/ذخیره کنید.
2. فایل لاگ یا گزارشی که رمز در آن هست ارسال نکنید؛ خروجی ابزار به‌ساختِ خودش رمز را جایی چاپ نمی‌کند.
3. اگر خواستید وضعیت شبکه را هم بفرستید، فقط IP/پورت دستگاه و نتیجهٔ `ping` را ذکر کنید.
4. رمز ارتباط فقط در حافظهٔ فرایند در طول اجرا نگه داشته می‌شود و پس از پایان برنامه باقی نمی‌ماند.

---

### ساختار پروژه

```
tools/NP756P.ReadOnlyProbe/
  NP756P.ReadOnlyProbe.csproj   net48 · x86 · Release-ready
  Program.cs                    نقطهٔ ورود، catch سراسری (کد 5)، ورودی مخفی رمز، کنسول Redirect
  ProbeOptions.cs               پارامترها + اعتبارسنجی کامل (IP/پورت/ماشین/...)
  NativeMethods.cs              شش DllImport مجاز (دقیقاً مطابق FKAttendDLL2.cs)
  IDeviceChannel.cs            انتزاع برای Mock کردن در تست‌ها
  FkDeviceChannel.cs            پیاده‌سازی واقعی روی SDK (تنها نقطهٔ اجرای Native)
  ProbeRunner.cs                ترتیب مراحل A..F + مدیریت استثنا + finally-disconnect
  ProbeResult.cs                مدل گزارش و رندر (بدون رمز)
  ExitCodes.cs                  ثابت‌های کد خروج + قالب‌بندی امن پیام استثنا
  NativeLibraryValidator.cs     تحلیل ایستای PE: x86، Export، Import نام‌دار/Ordinal، Bounds کامل
  README.md
  .gitignore                    (bin/obj از Git حذف‌اند)

tools/NP756P.ReadOnlyProbe.Tests/
  NP756P.ReadOnlyProbe.Tests.csproj   net8.0 هارنس تست بدون دستگاه
  MockDeviceChannel.cs                شبیه‌ساز SDK (اندیس‌های 1/3 و 2/3، ثبت گزینه‌ها، استثناها)
  TestRunner.cs                       ۱۳۵ تست: اعتبارسنجی، PE (مصنوعی + دو DLL واقعی)، سناریوها، محرمانگی، P/Invoke
  .gitignore
```

اجرای تست‌ها (بدون دستگاه، بدون اجرای DLL، بدون شبکه):

```
dotnet run -c Release tools/NP756P.ReadOnlyProbe.Tests
```
