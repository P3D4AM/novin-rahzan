# -*- coding: utf-8 -*-
"""Assemble the full Persian RTL HTML report + companion CSV."""

import csv
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from report_css import CSS, HTML_SHELL
from report_lib import table, para, en2fa_digits
from report_part1 import build_intro, build_architecture
from report_part2a import build_dal
from report_part2b import build_devices
from report_part3 import build_ui, build_print, build_validator
from report_part4 import (build_tele, build_sms, build_native, build_aux,
                          build_system, build_conclusion)

OUT_DIR = '/home/z/my-project/download'
ASSETS_DIR = '/home/z/my-project/report_assets'

TITLE = 'گزارش آموزشی تحلیل کتابخانه‌های سیستم حضور و غیاب نوین پرداز'


# ---------------------------------------------------------------------------
# Cover
# ---------------------------------------------------------------------------

def build_cover():
    return '''
<div class="cover">
  <div class="deco-grid"></div>
  <div class="deco-circle dc1"></div>
  <div class="deco-circle dc2"></div>
  <div class="deco-circle dc3"></div>

  <div class="cover-tag">گزارش فنی — آموزشی</div>
  <h1 class="cover-title">تحلیل کتابخانه‌های<br><span class="hl">سیستم حضور و غیاب</span><br>نوین پرداز</h1>
  <div class="cover-sub">کالبدشکافی کامل ۵۱ فایل بستهٔ NovinDll&rsquo;s.zip با دکامپایلر ILSpy — آموزش لایه به لایه به همراه کدهای واقعی</div>
  <div class="cover-en">NOVIN PARDAZ TIME &amp; ATTENDANCE — .NET LIBRARY ANALYSIS REPORT</div>

  <div class="cover-summary">
    در این گزارش، ۱۳ کتابخانهٔ اختصاصی نرم‌افزار جامع حضور و غیاب و حقوق و دستمزد «نوین پرداز» (پروژهٔ NPACC)
    را لایه‌به‌لایه بررسی می‌کنیم: از لایهٔ دسترسی داده و بانک اطلاعاتی SQL Server تا درایورهای شش برند دستگاه اثر انگشت،
    رابط کاربری WinForms، چاپ Stimulsoft، مودم صوتی و سرویس پیامک — همیشه با کد واقعیِ دکامپایل‌شده و نگاه معلم‌وار.
  </div>

  <div class="cover-chips">
    <span class="chip">‎.NET Framework 4.8</span>
    <span class="chip">C#‎ / WinForms</span>
    <span class="chip">SQL Server</span>
    <span class="chip">SignalR</span>
    <span class="chip">Stimulsoft</span>
    <span class="chip">‎۵۱ فایل • ۳۴٫۸ MB</span>
    <span class="chip">‎۶ برند دستگاه</span>
  </div>

  <div class="cover-footer">
    <div>
      <div class="strong">نسخهٔ ۱٫۰ — آبان ۱۴۰۵</div>
      <div>سند آموزشی عمومی — تهیه‌شده بر پایهٔ تحلیل ایستا (Static Analysis)</div>
    </div>
    <div class="left">ILSpy Decompiler<br>NovinDll&rsquo;s.zip / file.kiwi</div>
  </div>
</div>
'''


# ---------------------------------------------------------------------------
# Roadmap (visual TOC)
# ---------------------------------------------------------------------------

ROADMAP = [
    ('01', 'مقدمه؛ بسته‌ای که بررسی کردیم', 'دسته‌بندی ۵۱ فایل و روش تحلیل'),
    ('02', 'معماری کلان سیستم', 'دیاگرام لایه‌ها و سفر یک تردد'),
    ('03', 'TimeAttendanceDAL', 'قلب سیستم: بانک، DAL، رمزنگاری، ابر'),
    ('04', 'TimeAttendanceDevices', 'درایورهای شش برند دستگاه اثر انگشت'),
    ('05', 'TimeAttendanceUI', '۱۵۰ فرم، ورود، مانیتورینگ زنده'),
    ('06', 'TimeAttendancePrint', 'چاپ و گزارش‌سازی با Stimulsoft'),
    ('07', 'ValidatorAttribute', 'قواعد اعتبارسنجی موجودیت‌ها'),
    ('08', 'Tele.dll', 'مودم صوتی و تردد تلفنی با AT'),
    ('09', 'WebSmsStandard', 'کلاینت REST سرویس پیامک'),
    ('10', 'ZK و ZDC2911 و zkemkeeper', 'کتابخانه‌های بومی و ابزارها'),
    ('11', 'کتابخانه‌های جانبی', 'اعلان شیشه‌ای و انیمیشن'),
    ('12', 'کتابخانه‌های System.*', 'زیرساخت مایکروسافت (۲۶ فایل)'),
    ('13', 'جمع‌بندی و امنیت', 'درس‌های معماری و یافته‌های امنیتی'),
]


def build_roadmap(page_map=None):
    cards = ''
    for num, title, desc in ROADMAP:
        page = ''
        if page_map and num in page_map:
            page = f'<div class="rm-page">صفحهٔ {en2fa_digits(str(page_map[num]))}</div>'
        cards += (f'<div class="rm-card"><div class="rm-num">{num}</div>'
                  f'<div class="rm-title">{title}</div>'
                  f'<div class="rm-desc">{desc}</div>{page}</div>')
    return (
        '<div class="chapter-header" style="margin-top:0">'
        '<div class="ch-marker" dir="ltr">TOC</div>'
        '<div class="ch-tag">نقشهٔ راه</div>'
        '<h2 class="ch-title">در این گزارش چه می‌خوانیم؟</h2>'
        '</div>'
        + para('سیزده فصل پیشِ رو به ترتیبِ «از کلان به جزئی» چیده شده‌اند. اگر تازه‌کار هستید، ترتیب را به هم نزنید؛ '
               'اگر حرفه‌ای هستید، می‌توانید مستقیم به فصل کتابخانهٔ موردنظر بپرید. شمارهٔ صفحهٔ آغازین هر فصل در گوشهٔ کارت‌ها آمده است.')
        + f'<div class="roadmap">{cards}</div>'
    )


# ---------------------------------------------------------------------------
# Ending page
# ---------------------------------------------------------------------------

def build_ending():
    return '''
<div class="ending">
  <div class="deco-circle dc1"></div>
  <div class="deco-circle dc2"></div>
  <div class="ending-big">پایان سفر،<br>آغاز تمرین</div>
  <div class="ending-sub">
    حالا که معماری یک نرم‌افزار صنعتی واقعی را از درون دیده‌اید، بهترین قدم بعدی،
    باز کردن ILSpy و کاوش خودتان است. کد واقعی، بهترین معلم است.
  </div>
  <div class="ending-stats">
    <div class="ending-stat"><div class="v">۱۳</div><div class="l">کتابخانهٔ تحلیل‌شده</div></div>
    <div class="ending-stat"><div class="v">۴۵۰+</div><div class="l">کلاس بررسی‌شده</div></div>
    <div class="ending-stat"><div class="v">۶۰+</div><div class="l">قطعه کد واقعی</div></div>
    <div class="ending-stat"><div class="v">۷</div><div class="l">یافتهٔ امنیتی</div></div>
  </div>
  <div class="ending-foot">گزارش آموزشی تحلیل کتابخانه‌های سیستم حضور و غیاب نوین پرداز — آبان ۱۴۰۵</div>
</div>
'''


# ---------------------------------------------------------------------------
# Assemble
# ---------------------------------------------------------------------------

def build_html(page_map=None):
    body_parts = [
        build_cover(),
        '<div class="main-content">',
        build_roadmap(page_map),
        build_intro(),
        build_architecture(),
        build_dal(),
        build_devices(),
        build_ui(),
        build_print(),
        build_validator(),
        build_tele(),
        build_sms(),
        build_native(),
        build_aux(),
        build_system(),
        build_conclusion(),
        '</div>',
        build_ending(),
    ]
    html = HTML_SHELL.format(title=TITLE, css=CSS, body=''.join(body_parts))
    return html


def write_output(html_text, path):
    os.makedirs(OUT_DIR, exist_ok=True)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(html_text)
    # copy fonts next to the HTML for portable rendering + external fonts.css
    fonts_dir = os.path.join(os.path.dirname(path), 'fonts')
    os.makedirs(fonts_dir, exist_ok=True)
    import shutil
    from report_css import FONTS_CSS
    for fn in os.listdir(ASSETS_DIR + '/fonts'):
        shutil.copy(os.path.join(ASSETS_DIR, 'fonts', fn), fonts_dir)
    with open(os.path.join(fonts_dir, 'fonts.css'), 'w', encoding='utf-8') as f:
        f.write(FONTS_CSS)
    print(f'written: {path} ({len(html_text)/1024:.0f} KB)')


# ---------------------------------------------------------------------------
# CSV companion
# ---------------------------------------------------------------------------

CSV_ROWS = [
    ['Tele.dll', '23', 'اختصاصی', 'مودم صوتی / تردد تلفنی',
     'کنترل مودم روی پورت سریال با دستورات AT؛ پاسخ خودکار تماس، پخش WAV منوی صوتی، دریافت کد پرسنلی با DTMF، شناسه تماس‌گیرنده',
     'NovinPhone; ATCommandManager; CLSNumberDetector; ATReciveDataSpliter; FileManager',
     'Call; HangUp; Play_Wave; ReadLine; ExecCommnad; ActivateCallerID; GetInput',
     'رویدادهای Ring/Connect/Tone/EndCall؛ پرش از هدر ۴۴ بایتی WAV؛ لاگ کامل Input/Output/ALL.txt'],
    ['TimeAttendance.ValidatorAttribute.dll', '77', 'اختصاصی', 'اعتبارسنجی',
     'قواعد اعتبارسنجی موجودیت‌ها قبل از ذخیره (۱۸ جفت کلاس Validator + CheckValidation)',
     'PersonValidator; ShiftsValidator; GharardadDetValidator; PayRollValidator; AttSanadValidator; InsuranceValidator',
     'ValidateAsync; CheckValidationAsync; BeAValidMoeinTafsilAsync',
     'پیام‌های خطای فارسی؛ الگوی مشابه FluentValidation؛ جلوگیری از تکرار کد معین/تفصیلی'],
    ['TimeAttendanceDAL.dll', '6312', 'اختصاصی (قلب سیستم)', 'لایهٔ دسترسی داده',
     'همهٔ ذخیره/خواندن داده‌ها با ADO.NET و روال‌های ذخیره‌شده؛ ساخت خودکار بانک Attendance0-99؛ محاسبات کارکرد و حقوق؛ سینک ابری SignalR با novinrank.ir؛ موتور پیامک خودکار؛ رمزنگاری AES سطوح دسترسی',
     'ClsConnection; CLSDatabase; RunScript; Person; LogOnPersonel; TaradodFullDet; Fish; GharardadNew; Shifts; DeviceInfo; UserAttendance; UserPermission; Encryption; TimeAttendanceSettings; EntityCacheConfig; ClsSms; AttLogger; LogUsers',
     'CreateDbIfNotExist; SaveAsync; GetAllAsync (کش ۶۰ ثانیه); CalcPersonIdAsync; SaveTaradodsNewAsync; ConnectSignalRAsync; SendTelegramMessage; CheckConnectionString',
     'کلید AES هاردکد (NovinP****************)؛ کوئری‌های الحاقی مستعد SQL Injection؛ توکن ربات تلگرام؛ تقویم شمسی 1300/01/01؛ مبدل صفحه‌کد ایران‌سیستم؛ قفل سخت‌افزاری LK'],
    ['TimeAttendanceDevices.dll', '294', 'اختصاصی', 'درایور دستگاه‌ها',
     'ارتباط با ۶ برند دستگاه اثر انگشت/چهره: ZKTeco (COM)، Hysoon قدیم (P/Invoke)، Hysoon جدید (JSON/TCP + HTTP Push)، Ebio (ActiveX SBXPC)، Eizon/Riss (SDK)، ZDC2911؛ نرمال‌سازی رکوردها به DeviceLogOnPerson',
     'RecordAllDevice; ClsZK; clsHysoon; ClsDynamicHysoon; EbioClass; FrmEbio; ClsEizon; ZDC2911Realand; Listener761; HttpServerDynamicHysoon; ClsSbxpc',
     'GetLog; GetAllLogOnPublic; GetSendBuff; Receive; Setup; DispatchResources; GetAllFromRiss; IsZd100',
     'عدد جادویی 0x18181818؛ هندل استاتیک FK (تک‌اتصاله)؛ قالب چهره 6917 کلمه‌ای؛ جستجوی 65535 کاربر در Eizon؛ EXE جاسازی‌شده در ریسورس'],
    ['TimeAttendancePrint.dll', '2242', 'اختصاصی', 'چاپ و گزارش',
     'موتور گزارش‌سازی Stimulsoft؛ ۳۰+ قالب mrt جاسازی‌شده در ریسورس (A4/A5/رسید ۸ سانت)؛ تزریق داده با نام‌های فارسی',
     'ReportGenerator; ClsPrint; ClsGeneral; ClsStimul; ReportPath; STIPrint; FrmPrint; FrmPrintPreview',
     'GetSti; Print2PrinterAsync; PrintPreview; Design; SetSettingsAsync',
     'پوشهٔ خروجی با نام فارسی «حضور-غیاب و حقوق-دستمزد»؛ متغیر تاریخ شمسی DateCalture'],
    ['TimeAttendanceUI.dll', '15157', 'اختصاصی', 'رابط کاربر',
     'حدود ۱۵۰ فرم WinForms با Ribbon DevComponents؛ فرم اصلی 5755 خطی؛ ورود کاربران؛ ۴ کانال تردد زنده؛ تقویم شمسی؛ راست‌چین کامل؛ تعویض زبان فارسی/عربی',
     'Program; FrmMain; FrmEnter; FrmPersonel; FrmDevice; FrmRptTaradodFull; FrmSetting; HysoonRealTimeProcess; AttendanceRecord; EbioRealTimeProcess; DeviceServiceHandler; MarqueeLabel',
     'BtnEnter_Click; FRMMain_Load; OpenPortsForDevicesAsync; AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage; ReceiveDataDevicePolling; InitAccessLevel',
     'دو رمز استاد هاردکد (Novin******** / 11****60)؛ فرم دسترسی ویژه بدون رمز؛ RightToLeft در ۵۹۴ نقطه؛ قفل خودکار با GetLastInputInfo'],
    ['WebSmsStandard.dll', '26', 'اختصاصی', 'پیامک',
     'کلاینت REST سرویس پیامک نوین پرداز (webservice.novinp.ir)؛ ارسال تکی/گروهی، استعلام تحویل و شارژ؛ تلاش مجدد ۱۰ مرحله‌ای',
     'SmsSender; SmsModelV2; Credential; RetryClientFactory; OutBox_Other_Info; ActiveSendJsonResponse; NpsmsGetDeliveryResponse',
     'SendSmsAsync; SendSmsAsync_; GetDeliveryAsync; GetCreditAsync',
     'الگوی DelegatingHandler برای retry؛ کدهای خطای منفی (-119/-110/...)؛ اتصال HTTP رمزنگاری‌نشده'],
    ['Tulpep.NotificationWindow.dll', '28', 'متن‌باز (NuGet)', 'اعلان شیشه‌ای',
     'کامپوننت PopupNotifier: اعلان گوشهٔ صفحه با عنوان/متن/آیکن و انیمیشن؛ نمایش بدون فعال‌سازی پنجره (SW_SHOWNOACTIVATE)',
     'PopupNotifier; PopupNotifierForm',
     'Popup() / Hide() / ShowWithoutActivation',
     'تمام اعلان‌های سیستم از طریق این کتابخانه و wrapper آن (ClsNotif در DAL) نمایش داده می‌شود'],
    ['WinFormAnimation.dll', '38', 'متن‌باز (NuGet)', 'انیمیشن فرم',
     'موتور انیمیشن سبک: Animator/Animator2D/Animator3D + Path های حرکت با توابع شتاب (Linear/Quadratic/Sine/EaseOut)',
     'Animator; Animator2D; Animator3D; Path2D; Path3D; KnownAnimationFunctions; FPSLimiter',
     'Play / PlayAsync (از API عمومی کامپوننت)',
     'همراه Tulpep برای سُر خوردن اعلان‌ها استفاده می‌شود'],
    ['ZDC2911.dll', '116', 'SDK سازنده', 'دستگاه Realand',
     'SDK نمونهٔ Realand برای دستگاه‌های ZDC2911؛ بسته‌بندی SDK مدیریت‌شدهٔ Riss.Devices روی یک فرم داخلی (CMForm)؛ ۱۰ فرم نمونهٔ UI',
     'Realand; Records; DeviceCommEty; CMForm; GLogForm; EnrollForm; RealTimeLogForm',
     'Connect; Disconnect; GetALLGlog',
     'هم‌خانوادهٔ پروتکل Riss (فیلدهای DN/DIN/Clock/Verify)'],
    ['ZK.dll', '10', 'ابزار کمکی', 'تست دستگاه ZKTeco',
     'برنامهٔ کوچک WinForms برای اتصال و خواندن ترددهای ZKTeco با COM به نام CZKEMClass؛ نسخهٔ فشردهٔ ClsZK',
     'ZK; Record; Form1',
     'GetLog; Disconnect',
     'بلافاصله در سازنده GetLog اجرا می‌شود؛ پورت پیش‌فرض 5005'],
    ['zkemkeeper.dll', '652', 'SDK سازنده (بومی x86)', 'ZKTeco COM',
     'SDK رسمی ZKTeco؛ ارائهٔ کلاس COM معروف CZKEM برای اتصال/خواندن/نوشتن روی دستگاه‌ها؛ نیازمند regsvr32 و پروسهٔ ۳۲بیتی',
     '(COM class CZKEM / CZKEMClass)',
     'Z_Connect_NET; Z_ReadLogEx; Z_GetUserInfo; Z_SetUserTmp; Z_EnableDevice; Z_SetTime; Z_ClearLogData; SSR_GetGeneralLogData',
     'فایل بومی PE ۳۲بیتی (نه دات‌نت)؛ مبنای Interop.zkemkeeper که ClsZK و ZK.dll استفاده می‌کنند'],
    ['zkemsdk.dll', '206', 'SDK سازنده (بومی x86)', 'ZKTeco لایهٔ پایین',
     'لایهٔ پایین‌تر خانوادهٔ zkemkeeper؛ توابع SSR_* و Z_* مشابه با تمرکز بر عملیات پایهٔ پروتکل',
     '(توابع بومی SSR_Z_* / Z_*)',
     '_SSR_Z_GetUserInfo; _SSR_Z_SetUserTmp; SSR_Z_SetUserTmpExt; Z_ClearLogData',
     'همراه zkemkeeper.dll باید نصب شود'],
    ['System.Data.SqlClient.dll', '219', 'مایکروسافت', 'زیرساخت',
     'کلاینت ADO.NET برای SQL Server — ستون فقرات همهٔ کوئری‌های DAL', '-', '-', ''],
    ['System.Text.Json.dll', '630', 'مایکروسافت', 'زیرساخت',
     'سریالایز JSON برای تنظیمات، پروتکل Hysoon جدید و مدل‌های ابری', '-', '-', ''],
    ['System.IO.Ports.dll', '34', 'مایکروسافت', 'زیرساخت',
     'پورت سریال — برای کتابخانهٔ Tele (مودم صوتی)', '-', '-', ''],
    ['System.Drawing.Common.dll', '52', 'مایکروسافت', 'زیرساخت',
     'گرافیک GDI+ برای فرم‌ها، تصاویر پرسنل و اسکرین‌شات', '-', '-', ''],
    ['System.IdentityModel.Tokens.Jwt.dll', '80', 'مایکروسافت', 'زیرساخت',
     'توکن‌های JWT — احراز هویت با سرور ابری', '-', '-', ''],
    ['System.Threading.Tasks.Dataflow.dll', '183', 'مایکروسافت', 'زیرساخت',
     'TPL Dataflow — صف BufferBlock اعلان‌ها و ترددهای زنده', '-', '-', ''],
    ['System.Threading.Channels.dll', '51', 'مایکروسافت', 'زیرساخت',
     'کانال‌های تولیدکننده/مصرف‌کننده', '-', '-', ''],
    ['System.Collections.Immutable.dll', '247', 'مایکروسافت', 'زیرساخت',
     'کالکشن‌های تغییرناپذیر برای استفادهٔ امن در چندتردی', '-', '-', ''],
    ['System.Memory.dll', '139', 'مایکروسافت', 'زیرساخت',
     'Span و Memory — پیش‌نیاز عملکردی کتابخانه‌های مدرن روی NET Framework 4.8', '-', '-', ''],
    ['System.Numerics.Vectors.dll', '113', 'مایکروسافت', 'زیرساخت',
     'بردارهای عددی SIMD', '-', '-', ''],
    ['System.Runtime.CompilerServices.Unsafe.dll', '18', 'مایکروسافت', 'زیرساخت',
     'دسترسی مدیریت‌نشده امن — پیش‌نیاز System.Memory', '-', '-', ''],
    ['System.Threading.Tasks.Extensions.dll', '25', 'مایکروسافت', 'زیرساخت',
     'ValueTask و افزونه‌های async — پیش‌نیاز async/await مدرن روی 4.8', '-', '-', ''],
    ['System.ValueTuple.dll', '25', 'مایکروسافت', 'زیرساخت',
     'تاپل‌های مقداری برای NET Framework 4.8', '-', '-', ''],
    ['System.IO.Pipelines.dll', '83', 'مایکروسافت', 'زیرساخت',
     'I/O با کارایی بالا — زیرساخت لایهٔ شبکهٔ SignalR', '-', '-', ''],
    ['System.Net.WebSockets.WebSocketProtocol.dll', '57', 'مایکروسافت', 'زیرساخت',
     'پیاده‌سازی وب‌سوکت — زیرساخت ارتباط زندهٔ SignalR', '-', '-', ''],
    ['System.Text.Encoding.CodePages.dll', '748', 'مایکروسافت', 'زیرساخت',
     'صفحه‌کدهای قدیمی ویندوز (از جمله 1256 فارسی) در دات‌نت جدید', '-', '-', ''],
    ['System.Text.Encodings.Web.dll', '77', 'مایکروسافت', 'زیرساخت',
     'انکود امن متن‌ها برای JSON/HTML', '-', '-', ''],
    ['System.Configuration.ConfigurationManager.dll', '82', 'مایکروسافت', 'زیرساخت',
     'خواندن فایل‌های .config برنامه', '-', '-', ''],
    ['System.Security.AccessControl.dll', '33', 'مایکروسافت', 'زیرساخت',
     'ACL ویندوز — کنترل دسترسی فایل و اشیاء', '-', '-', ''],
    ['System.Security.Cryptography.ProtectedData.dll', '23', 'مایکروسافت', 'زیرساخت',
     'DPAPI — رمزنگاری وابسته به کاربر/ماشین ویندوز', '-', '-', ''],
    ['System.Security.Permissions.dll', '25', 'مایکروسافت', 'زیرساخت',
     'انواع مجوزهای امنیتی دات‌نت', '-', '-', ''],
    ['System.Security.Principal.Windows.dll', '18', 'مایکروسافت', 'زیرساخت',
     'هویت و گروه‌های ویندوزی', '-', '-', ''],
    ['System.CodeDom.dll', '30', 'مایکروسافت', 'زیرساخت',
     'تولید کد در زمان اجرا (مثلاً کامپایل فرمول‌ها)', '-', '-', ''],
    ['System.ComponentModel.Annotations.dll', '42', 'مایکروسافت', 'زیرساخت',
     'دیتا-انوتیشن‌ها برای اعتبارسنجی مدل', '-', '-', ''],
    ['System.Diagnostics.DiagnosticSource.dll', '169', 'مایکروسافت', 'زیرساخت',
     'ردیابی و تله‌متری (زیرساخت JaegerMonitor در کد)', '-', '-', ''],
    ['TimeAttendance.exe.config', '12', 'تنظیمات', 'تنظیمات',
     'Binding Redirect های نسخه اسمبلی‌ها برای اجرای هماهنگ DLL ها', '-', '-', ''],
    ['TimeAttendance.pdb', '160', 'فایل عیب‌یابی', 'عیب‌یابی',
     'نمادهای عیب‌یابی EXE اصلی (خود EXE در بسته نیست)', '-', '-', ''],
    ['فایل‌های .pdb و .config سایر پروژه‌ها', '~5000', 'عیب‌یابی/تنظیمات', 'سایر',
     'PDB برای ۶ پروژهٔ دیگر (DAL، UI، Devices، Print، Validator، WebSms) + فایل‌های config آن‌ها', '-', '-', ''],
]


def write_csv():
    path = os.path.join(OUT_DIR, 'خلاصه_کتابخانه_ها.csv')
    with open(path, 'w', encoding='utf-8-sig', newline='') as f:
        w = csv.writer(f)
        w.writerow(['نام فایل', 'حجم (KB)', 'دسته', 'نقش در سیستم', 'شرح عملکرد',
                    'کلاس‌های کلیدی', 'متدهای مهم', 'نکات قابل توجه'])
        w.writerows(CSV_ROWS)
    print(f'written: {path} ({len(CSV_ROWS)} rows)')


if __name__ == '__main__':
    mode = sys.argv[1] if len(sys.argv) > 1 else 'html'
    if mode == 'csv':
        write_csv()
    else:
        pm = None
        if mode == 'html2' and os.path.exists('/home/z/my-project/scripts/page_map.py'):
            from page_map import PAGE_MAP
            pm = PAGE_MAP
        html_text = build_html(pm)
        write_output(html_text, os.path.join(OUT_DIR, 'گزارش_تحلیل_کتابخانه_ها.html'))
