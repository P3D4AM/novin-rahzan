# پژوهش NP756P — مدل ۹۳۰۸، نگاشت Hysoon، پروتکل کلاسیک در برابر Dynamic، امضای SDK و وابستگی‌های Native

این سند خلاصهٔ یافته‌های تحقیقی است که مبنای طراحی ابزار `tools/NP756P.ReadOnlyProbe` قرار گرفت. همهٔ ادعاها با خواندن مستقیم سورس‌های `NovinLibraryResearch/decompiled-complete` و تحلیل ایستای PE فایل‌های `_dlls` (با اسکریپت `scripts/pe_inspect.py` در محیط پژوهش) راستی‌آزمایی شده‌اند.

---

## ۱. شناسایی مدل NP756P

**فایل:** `NovinLibraryResearch/decompiled-complete/TimeAttendanceDAL/TimeAttendanceDAL.Dal.Device/DeviceInfo.cs`

در ستِرِ `Model`، نگاشت شناسه‌های مدل به سازنده (`DeviceManufacture`) چنین است:

| بازه/مقدار | DeviceManufacture |
|---|---|
| 9001..9003 | 0 (Eizon) |
| 9101..9102 | 1 |
| 9201..9204 | 2 (Ebio) |
| 9301..9306 | **3 (Hysoon کلاسیک)** |
| 9307 | **5 (Dynamic Hysoon)** |
| 9308 | **3 (Hysoon کلاسیک)** ← NP756P |
| 9309 | 6 |
| 9400 | 4 (ZK) |

> اصلاح مهم نسبت به نسخهٔ اول این سند: در سوییچِ دیکامپایل‌شده `switch (value - 9301)` کیس‌های ۰ تا ۵ و ۷ به Manufacture=3 می‌نشینند (یعنی **9301..9306 و 9308**)، کیس ۶ (9307) به Manufacture=5 و کیس ۸ (9309) به Manufacture=6. جدول بالا مطابق سورس است؛ نگاشت قبلی «9301..9305 و 9307 = 3 و 9306 = 5» نادرست بود.

`9308` در case `value - 9301 == 7` می‌نشیند و به `DeviceManufacture = 3` نگاشت می‌شود.

**فایل:** `NovinLibraryResearch/decompiled-complete/TimeAttendanceUI/TimeAttendanceUI.Forms.Device/FrmDevice.cs` (خطوط ~573 و ~1856): انتخاب رادیو-دکمهٔ `ucNP756P` مقدار `_clsDeviceInfo.Model = (DeviceModelEnumType)9308` را ست می‌کند.

**نتیجه:** NP756P = 9308 = مسیر `clsHysoon` کلاسیک.

## ۲. دو پروتکل مستقل Hysoon

در `DeviceInfo.cs` (متدهای `ReceiveDataFromDeviceAsync`/`TestConnectionAsync` و…):

- `DeviceManufacture == 3` → `clsHysoon` (SDK کلاسیک `FK623Attend.dll` — P/Invoke).
- `DeviceManufacture == 5` → `ClsDynamicHysoon` (پروتکل مستقل TCP با هدر ۳۲ بایتی و بدنهٔ JSON).

نمونهٔ فراخوانی مسیر ۳:

```csharp
new clsHysoon(ContextGuid, DeviceNo, ParseToInt(CommunicationPass, 0), Ip, TcpPort, GetNewTaradodOnly)
```

**تصمیم ابزار:** برای NP756P فقط مسیر کلاسیک (`clsHysoon`/`FK623Attend.dll`)؛ مسیر Dynamic صرفاً برای شناخت معماری بررسی شد و در ابزار وارد نشده است. (پروتکل خام SBXPC هم — که قبلاً آزمایش شده و ناموفق بوده — در این ابزار ارسال نمی‌شود.)

## ۳. SDK صحیح و امضای توابع

**فایل‌های منبع امضا:**

- `TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/FKAttendDLL2.cs`
- `TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/clsHysoon.cs`
- `FK623AttendDllCSSample/FKAttendDllCSSample/FrmConnectDeviceHysoon.cs`
- `FK623AttendDllCSSample/FKAttendDllCSSample/FrmCheckDeviceHysoon.cs`

همهٔ DllImportها روی `FK623Attend.dll` با `CharSet.Ansi` هستند. امضاهای مجاز ابزار (دقیقاً مطابق سورس):

```csharp
int FK_ConnectNet(int anMachineNo, string astrIpAddress, int anNetPort,
                  int anTimeOut, int anProtocolType, int anNetPassword, int anLicense)
void FK_DisConnect(int anHandleIndex)
int  FK_GetLastError(int anHandleIndex)
int  FK_GetProductData(int anHandleIndex, int anDataIndex,
                      [MarshalAs(UnmanagedType.LPStr)] ref string apstrValue)
int  FK_GetDeviceInfo(int anHandleIndex, int anInfoIndex, ref int apnValue)
int  FK_GetDeviceTime(int anHandleIndex, ref DateTime apnDateTime)
```

مقادیر استفاده‌شده در `clsHysoon` (تاییدشده): `Timeout=5000` · `Protocol=0 (TCP)` · `License=1261` · رمز عددی. مقادیر پروتکل در نمونهٔ فروشنده: `ChkUdpFlag.Checked ? 1 : 0` (TCP=0, UDP=1).

نکتهٔ مهم: مقدار `5005` در `FrmConnectDeviceHysoon` فقط مقدار پیش‌فرض کادر متن پورت است و اثباتی برای پورت واقعی دستگاه نیست (مشاهدات شبکهٔ قبلی: `4238` باز، `5005` بسته — فقط به‌عنوان مقدار پیشنهادی پیش‌فرض ابزار استفاده شده و قابل تغییر است).

الگوهای استفادهٔ تاییدشده در نمونهٔ فروشنده:

| عمل | الگو |
|---|---|
| موفقیت اتصال | `FK_ConnectNet(...) > 0` |
| بافر `FK_GetProductData` | `new string(' ', 256)` و سپس `Trim()` |
| اندیس‌های محصول | 1 = شماره سریال، 3 = کد محصول |
| اندیس‌های `FK_GetDeviceInfo` | 2 = شماره دستگاه (DI_MACHINENUM)، 3 = زبان (DI_LANGAUGE؛ 0 = فارسی) |
| `FK_GetDeviceTime` | `DateTime.Now` اولیه + `ref` + استفاده فقط وقتی کد=1 |
| کد موفقیت عمومی | `1` (طبق `ReturnResultPrint`) |

`FK_DisConnect` در سورس **void** است (کد بازگشتی ندارد) — ابزار همین‌طور صدا می‌زند و نتیجه را «اجرا شد» گزارش می‌کند.

## ۴. نگاشت کدهای خطا

از `FKAttendDLL2.ReturnResultPrint` (دی‌کامپایل): کد 1 = موفق؛ 0 = No support؛ -1 = Unknown؛ -2 = No Open Comm؛ -3 = Write؛ -4 = Read؛ -5 = Parameter؛ -6 = execution failed؛ -7 = End of data؛ -8 = Nonexistence؛ -9 = Memory؛ -10 = License؛ -11 = full enrolldata؛ -12 = id existed؛ -14 = full manager؛ -15 = fp data version. کد **-13** در سورس متنی ندارد (کاستِ دیکامپایل خالی) و در ابزار «Unmapped» گزارش می‌شود (بدون جعل متن).

## ۵. وابستگی‌های Native (تحلیل PE واقعی فایل‌های `_dlls`)

| DLL | معماری | تعداد Export | Importهای بومی از فروشنده |
|---|---|---|---|
| FK623Attend.dll | x86 (i386) | 161 | از **fkattend.dll**: 1 تابع نام‌دار (`FK_GetLastError`) + **160 تابع به‌صورت Ordinal** (۱..۱۶ و ۱۸..۱۶۱) |
| FKAttend.dll | x86 | 162 | از **fkviadev.dll**: 14 تابع نام‌دار (نام‌های Managled مانند `?FV_ConnectNet@CFKViaDev@@...`) · از **lfwviadev.dll**: 12 تابع نام‌دار · از **fkpwdencdec.dll**: 9 تابع Ordinal (۱..۹) · از **fpdataconv.dll**: 5 تابع Ordinal (1،2،3،4،6) · از **facedataconv.dll**: 1 تابع Ordinal (۱) |
| FKViaDev.dll | x86 | 45 | — (فقط DLLهای سیستم) |
| LFWViaDev.dll | x86 | 23 | — (فقط DLLهای سیستم) |
| FKPwdEncDec.dll | x86 | 9 | — (فقط kernel32) |
| FpDataConv.dll | x86 | 8 | — (فقط DLLهای سیستم) |
| FaceDataConv.dll | x86 | 1 | — (فقط DLLهای سیستم) |

- وابستگی‌های سیستمی: kernel32, user32, gdi32, gdiplus, ws2_32 (سوکت), advapi32, ole32/oleaut32/oleacc, comctl32, comdlg32, msimg32, imm32, shell32, shlwapi, setupapi (USB), winmm, winspool.drv — همه با ویندوز عرضه می‌شوند و در Validator جزو DLLهای سیستم در نظر گرفته می‌شوند (بدون بررسی Ordinal، چون جدول Export آن‌ها در این محیط موجود نیست).
- هر دو DLL `FK623Attend.dll` و `FKAttend.dll` شش تابع مجاز ابزار را Export می‌کنند (بررسی‌شده با استخراج جدول Export).
- **اصلاح مهم نسبت به نسخهٔ اول این سند:** ادعای قبلی که «FK623Attend.dll تنها FK_GetLastError را از fkattend.dll Import مستقیم دارد و بقیه در زمان اجرا حل می‌شوند» نادرست بود. بررسی واقعی Import Table نشان می‌دهد FK623Attend.dll علاوه بر `FK_GetLastError` (نام‌دار)، **۱۶۰ تابع را با شمارهٔ Ordinal** از FKAttend.dll وارد می‌کند؛ به همین دلیل Validator ابزار اکنون Importهای نام‌دار و Ordinal-based را استخراج و در برابر جدول Export DLL مقصد تطبیق می‌دهد (Ordinal مفقود = شکست Validation با گزارش مبدأ/مقصد/Ordinal). همهٔ ۱۶۰ Ordinal موردنیاز در جدول Export FKAttend.dll (162 اسلات، Base=1) موجود و تأییدشده است.
- نتیجه: **حداقل مجموعهٔ لازم برای بارگذاری = همان ۷ DLL**؛ هر ۷ فایل x86 هستند پس فرایند اجرا باید x86 باشد.
- DLLهای دیگر `_dlls` (مانند SBXPCDLL/ZK/…) برای این ابزار لازم نیستند و کپی نمی‌شوند.

### محدودیت‌های Parser ابزار (صادقانه، بدون ادعای سازگاری کامل)

پارسر ایستای Validator کامل نیست و همهٔ گونه‌های PE دنیا را پوشش نمی‌دهد؛ دقیقاً همین‌ها پشتیبانی می‌شوند:

- تصاویر **PE32 (0x10B)** و **PE32+ (0x20B)** با جدول Import/Export استاندارد Microsoft COFF.
- Import از طریق **OriginalFirstThunk** (و در صورت صفر بودن، fallback به FirstThunk) خوانده می‌شود؛ Importهای نام‌دار و **Ordinal-based** هر دو پشتیبانی می‌شوند.
- جدول Export با شمارش occupancy اسلات‌های `AddressOfFunctions` بررسی می‌شود (Ordinal `N` فقط وقتی صادق است که `Base ≤ N < Base+NumberOfFunctions` و اسلات غیرصفر باشد).
- موارد بررسی‌نشده: **Delay-Load Imports**، Importهای Bound (پیش‌حل‌شده)، **Forwarder Exportها** (اسلات اشغال شمرده می‌شوند اما زنجیرهٔ forward دنبال نمی‌شود)، عنوان‌های TLS/Relocation/Debug، فرم‌های mixed-mode دات‌نت، و Importهای DLLهای سیستم ویندوز (که در این محیط موجود نیستند).
- هرگونه جدول قطع‌شده، RVA خارج از فایل، شمارش نامعقول یا نام خالی، به‌صورت خطای Parse (غیرقابل‌چشم‌پوشی) گزارش می‌شود — رفتار پیش‌فرض همیشه شکست است، نه موفقیت ضمنی.

## ۶. معماری ابزار و محدودیت ایمنی

- net48 · x86 · کنسول · مستقل از اسمبلی‌های Novin.
- فقط ۶ تابع مجاز تعریف شده‌اند؛ توابع نوشتنی حتی DllInvoke هم ندارند (تست واحدِ بازتابی دارد که مجموعهٔ P/Invoke را با فهرست مجاز تطبیق می‌دهد).
- `FK_EnableDevice` — هرچند در نمونهٔ فروشنده قبل از برخی عملیات استفاده می‌شود — **عمداً در ابزار اجرا نمی‌شود**؛ اگر خواندن‌ها بدون آن شکست خوردند فقط علت گزارش می‌شود (تصمیم جداگانهٔ بعدی).
- `FK_GetLastError` فقط با هندل معتبر و پس از شکست خواندن صدا زده می‌شود.
- رمز عددی فقط مخفی از کاربر گرفته می‌شود؛ در هیچ خروجی/لاگ/ToString نمی‌آید (تست واحد مخصوص دارد).
- شکست هر مرحله، مراحل وابستهٔ بعدی را SKIPPED می‌کند؛ اتصال معتبر همیشه در `finally` بسته می‌شود.
- هیچ تلاش مجدد رمز/پورت خودکار وجود ندارد.
- در شکست اتصالِ «مقدار برگشتی»، Raw Code ثبت می‌شود؛ در اتصالِ «استثنا»، هیچ Raw Code/LastError جعلی ثبت نمی‌شود (صفر به‌عنوان No support تفسیر نمی‌شود).

## ۷. وضعیت Build و تست

- Build واقعی net48/x86 با `dotnet build -c Release` (SDK 8.0.428 روی لینوکس + پکیج reference assemblies) انجام و خروجی PE32/x86 تولید شد.
- هارنس تست بدون دستگاه (net8.0، **۱۳۵ تست**، Mock + PE مصنوعی + دو DLL واقعی مرجع فقط برای Parse): کامل سبز است: هیچ DLL Native در Build/تست اجرا نشده و هیچ سوکتی باز نشده. تست‌های دادهٔ واقعی FK623Attend/FKAttend در `_dlls` ریپو به‌عنوان مرجع Parse می‌شوند (160 Ordinal + 1 نام‌دار از FKAttend؛ همهٔ Ordinalها در Export مقصد موجود).
- کد خروج برنامه: `0` موفق کامل · `1` خطای ورودی/رمز یا کنسول Redirect شده · `2` شکست مرحلهٔ DLLها · `3` شکست اتصال · `4` شکست مرحلهٔ خواندن یا قطع اتصال · `5` استثنای پیش‌بینی‌نشده (با catch سراسری کنترل‌شده؛ بدون Stack Trace). خروجی `0` فقط وقتی صادر می‌شود که اتصال واقعاً بسته شده باشد یا از ابتدا هندل معتبری وجود نداشته باشد.
- اگر ورودی کنسول Redirect شده باشد، ابزار به‌جای crash پیام قابل فهم می‌دهد و با کد 1 خارج می‌شود.
- اجرای واقعی SDK و اتصال به دستگاه NP756P فقط دستی روی ویندوز و توسط اپراتور انجام می‌شود.

## ۸. مسیرهای مرجع (برای بازبینی)

- نگاشت مدل: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDAL/TimeAttendanceDAL.Dal.Device/DeviceInfo.cs`
- معرفی UI مدل ۹۳۰۸: `NovinLibraryResearch/decompiled-complete/TimeAttendanceUI/TimeAttendanceUI.Forms.Device/FrmDevice.cs`
- امضاهای SDK: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/FKAttendDLL2.cs`
- الگوی اتصال/پارامترها: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/clsHysoon.cs`
- الگوهای خواندن: `NovinLibraryResearch/decompiled-complete/FK623AttendDllCSSample/FKAttendDllCSSample/FrmCheckDeviceHysoon.cs` و `FrmConnectDeviceHysoon.cs`
- DLLهای باینری: `NovinLibraryResearch/decompiled-complete/_dlls/`
