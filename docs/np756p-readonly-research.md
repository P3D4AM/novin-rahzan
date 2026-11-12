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
| 9301..9305, 9307 | **3 (Hysoon کلاسیک)** |
| 9306 | 5 (Dynamic Hysoon) |
| 9308 | **3 (Hysoon کلاسیک)** ← NP756P |
| 9309 | 6 |
| 9400 | 4 (ZK) |

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

| DLL | معماری | تعداد Export | وابستگی‌های Native غیرسیستمی |
|---|---|---|---|
| FK623Attend.dll | x86 (i386) | 161 | fkattend.dll |
| FKAttend.dll | x86 | 162 | fkviadev.dll · lfwviadev.dll · fkpwdencdec.dll · fpdataconv.dll · facedataconv.dll |
| FKViaDev.dll | x86 | 45 | — |
| LFWViaDev.dll | x86 | 23 | — |
| FKPwdEncDec.dll | x86 | 9 | — |
| FpDataConv.dll | x86 | 8 | — |
| FaceDataConv.dll | x86 | 1 | — |

- وابستگی‌های سیستمی: kernel32, user32, gdi32, gdiplus, ws2_32 (سوکت), advapi32, ole32/oleaut32/oleacc, comctl32, comdlg32, msimg32, imm32, shell32, shlwapi, setupapi (USB), winmm, winspool.drv — همه با ویندوز عرضه می‌شوند.
- هر دو DLL `FK623Attend.dll` و `FKAttend.dll` شش تابع مجاز ابزار را Export می‌کنند (بررسی‌شده با استخراج جدول Export).
- نتیجه: **حداقل مجموعهٔ لازم برای بارگذاری = همان ۷ DLL**؛ هر ۷ فایل x86 هستند پس فرایند اجرا باید x86 باشد.
- توجه: `FK623Attend.dll` تنها `FK_GetLastError` را از `fkattend.dll` به‌صورت Import مستقیم دارد؛ بقیهٔ توابع احتمالاً در زمان اجرا از طریق forwarder/GetProcAddress حل می‌شوند — برای بارگذاری، وجود هر ۷ فایل لازم است.
- DLLهای دیگر `_dlls` (مانند SBXPCDLL/ZK/…) برای این ابزار لازم نیستند و کپی نمی‌شوند.

## ۶. معماری ابزار و محدودیت ایمنی

- net48 · x86 · کنسول · مستقل از اسمبلی‌های Novin.
- فقط ۶ تابع مجاز تعریف شده‌اند؛ توابع نوشتنی حتی DllInvoke هم ندارند.
- `FK_EnableDevice` — هرچند در نمونهٔ فروشنده قبل از برخی عملیات استفاده می‌شود — **عمداً در ابزار اجرا نمی‌شود**؛ اگر خواندن‌ها بدون آن شکست خوردند فقط علت گزارش می‌شود (تصمیم جداگانهٔ بعدی).
- `FK_GetLastError` فقط با هندل معتبر و پس از شکست خواندن صدا زده می‌شود.
- رمز عددی فقط مخفی از کاربر گرفته می‌شود؛ در هیچ خروجی/لاگ/ToString نمی‌آید (تست واحد مخصوص دارد).
- شکست هر مرحله، مراحل وابستهٔ بعدی را SKIPPED می‌کند؛ اتصال معتبر همیشه در `finally` بسته می‌شود.
- هیچ تلاش مجدد رمز/پورت خودکار وجود ندارد.

## ۷. وضعیت Build و تست

- Build واقعی net48/x86 با `dotnet build -c Release` (SDK 8.0.428 روی لینوکس + پکیج reference assemblies) انجام و خروجی PE32/x86 تولید شد.
- هارنس تست بدون دستگاه (net8.0، ۷۲ تست، Mock + PE مصنوعی) کامل سبز است: هیچ DLL Native در Build/تست اجرا نشده و هیچ سوکتی باز نشده.
- اجرای واقعی SDK و اتصال به دستگاه NP756P فقط دستی روی ویندوز و توسط اپراتور انجام می‌شود.

## ۸. مسیرهای مرجع (برای بازبینی)

- نگاشت مدل: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDAL/TimeAttendanceDAL.Dal.Device/DeviceInfo.cs`
- معرفی UI مدل ۹۳۰۸: `NovinLibraryResearch/decompiled-complete/TimeAttendanceUI/TimeAttendanceUI.Forms.Device/FrmDevice.cs`
- امضاهای SDK: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/FKAttendDLL2.cs`
- الگوی اتصال/پارامترها: `NovinLibraryResearch/decompiled-complete/TimeAttendanceDevices/TimeAttendanceDevices.FLRHysoon/clsHysoon.cs`
- الگوهای خواندن: `NovinLibraryResearch/decompiled-complete/FK623AttendDllCSSample/FKAttendDllCSSample/FrmCheckDeviceHysoon.cs` و `FrmConnectDeviceHysoon.cs`
- DLLهای باینری: `NovinLibraryResearch/decompiled-complete/_dlls/`
