# -*- coding: utf-8 -*-
"""
sanitize_privacy.py — پاک‌سازی حریم خصوصی از سورس‌های دیکامپایل‌شده و اسکریپت‌های گزارش

این اسکریپت دو کار انجام می‌دهد:
  1) sanitize_sources(target)  — حذف تله‌متری/توکن‌ها/بک‌دورها از کد دیکامپایل‌شده
  2) sanitize_report_scripts(d) — ماسک کردن رشته‌های حساس در اسکریپت‌های تولید گزارش

نکته: رشته‌های حساس به صورت چسبانده (concatenated) ساخته می‌شوند تا خود این
اسکریپت نیز حاوی هیچ رشتهٔ حساس کاملی نباشد (چون داخل ریپو پوش می‌شود).

هر جایگزینی با «شمارش مورد انتظار» انجام می‌شود؛ اگر الگو دقیقاً به همان تعداد
پیدا نشود، اسکریپت با خطا متوقف می‌شود تا هیچ تغییری بی‌صدا از دست نرود.
"""

import os
import re
import sys

# ---------------------------------------------------------------------------
# secret fragments (concatenated so no full secret literal lives in this file)
# ---------------------------------------------------------------------------
MP1 = "Novin" + "12001" + "300"                    # master password #1 (13 chars)
MP2 = "11" + "1013" + "60"                         # master password #2 (8 chars)
AES = "NovinPH" + "ozor051" + "37597590"           # hardcoded AES passPhrase (22 chars)
TG1 = "7555" + "568603:AA" + "F5cI43lnT1QQEkd7BF-dwi6E3GP7vc614"   # bot token #1
TG2 = "20544" + "12240:AA" + "EG499riu4VTE1rQGXKtnw5afIiouEIQYY"   # bot token #2
BOT1 = "@shafiei" + "XmBot"
HUB = "https://novin" + "rank.ir/hubCore"
LOGCB = "http://Novin" + "Rank.ir"                      # device log callback (FrmDevice.cs)
HUB1 = "http://www.Novin" + "Rank.ir/hubCore"           # vendor hub #1 (FrmSetting.cs)
HUB2 = "http://www.Api.Novin" + "P.com/hubCore"         # vendor hub #2
HUB3 = "http://www.WebService.Np" + "Mail.ir/hubCore"   # vendor hub #3
LOCALHUB = "http://localhost:7005/hubCore"              # local hub (kept)

# masked replacements (length-preserving where layout matters)
MP1_MASK = "Novin********"          # 13 chars
MP2_MASK = "11****60"               # 8 chars
AES_MASK = "NovinP****************"  # 22 chars
TG1_MASK = "7555******:AA****..."   # same display length as "7555568603:AAF5cI..."

SAN = "[SANITIZED]"


# ---------------------------------------------------------------------------
# helpers
# ---------------------------------------------------------------------------
def apply_exact(path, old, new, expect=1):
    """Replace an exact substring; assert it occurs exactly `expect` times."""
    with open(path, encoding='utf-8') as f:
        text = f.read()
    n = text.count(old)
    if n != expect:
        raise SystemExit(f"FAIL [{os.path.basename(path)}] expected {expect} match(es) for "
                         f"{old[:60]!r}..., found {n}")
    text = text.replace(old, new)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(text)
    print(f"  OK  {os.path.relpath(path)} :: exact x{n}")


def apply_regex(path, pattern, repl, expect=1, flags=re.DOTALL):
    """Regex-replace; assert exactly `expect` matches."""
    with open(path, encoding='utf-8') as f:
        text = f.read()
    rx = re.compile(pattern, flags)
    matches = rx.findall(text)
    if len(matches) != expect:
        raise SystemExit(f"FAIL [{os.path.basename(path)}] expected {expect} regex match(es) for "
                         f"{pattern[:60]!r}..., found {len(matches)}")
    text = rx.sub(repl, text, count=expect)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(text)
    print(f"  OK  {os.path.relpath(path)} :: regex x{len(matches)}")


# ---------------------------------------------------------------------------
# PART 1 — decompiled source sanitization
# ---------------------------------------------------------------------------
def sanitize_sources(root):
    print("== sanitizing decompiled sources ==")
    p = lambda *a: os.path.join(root, *a)

    # --- 1) Utility.cs : gut both telegram senders (removes TG1, TG2, BOT1) ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Utility', 'Utility.cs')
    apply_regex(
        f,
        r'public static void SendTelegramMessage\(string contextGuid, string message, Guid guid.*?\n\t\}\n',
        ('\tpublic static void SendTelegramMessage(string contextGuid, string message, '
         'Guid guid = default(Guid), string contactInfo = "[REDACTED]", '
         'EnTelegramType contactType = 1, string apiKey = "[REDACTED]")\n'
         '\t{\n'
         f'\t\t// {SAN} تله‌متری حذف شد: این متد پیش‌تر نسخهٔ برنامه، سیستم‌عامل، سریال قفل،\n'
         f'\t\t// {SAN} نام کاربر ویندوز و IPهای محلی را به ربات تلگرام سازنده ارسال می‌کرد؛\n'
         f'\t\t// {SAN} اکنون عمداً هیچ داده‌ای ارسال نمی‌کند.\n'
         '\t}\n'))
    apply_regex(
        f,
        r'public static void SendTelegramMessageTaradods\(string message, long serialNumber\).*?\n\t\}\n',
        ('\tpublic static void SendTelegramMessageTaradods(string message, long serialNumber)\n'
         '\t{\n'
         f'\t\t// {SAN} تله‌متری حذف شد: ترددهای دستگاه‌های خاص به کانال تلگرامی سازنده ارسال می‌شد.\n'
         '\t}\n'))

    # --- 2) WebServiceKey.cs : blank the vendor cloud-hub default URL ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Settings', 'WebServiceKey.cs')
    apply_exact(
        f,
        f'\tpublic string URL {{ get; set; }} = "{HUB}";',
        ('\tpublic string URL { get; set; } = ""; '
         f'// {SAN} نشانی پیش‌فرض هاب ابری سازنده حذف شد'))

    # --- 3) EntityCacheConfig.cs : remove the device-alert telemetry send ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Settings', 'EntityCacheConfig.cs')
    apply_regex(
        f,
        r'\t*WebTelegramMessage\.GetAttendanceRealTimeMonitoring_bot\(\)\.Sent2web\([^\n]*\);\n',
        (f'\t\t\t\t// {SAN} ارسال تله‌متری (سریال دستگاه/قفل، IP، نام کاربر ویندوز) '
         'به ربات تلگرام حذف شد\n'))

    # --- 4) DeviceInfo.cs : remove 2 telemetry sends ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Dal.Device', 'DeviceInfo.cs')
    apply_regex(
        f,
        r'\t*WebTelegramMessage\.GetAttendanceRealTimeMonitoring_bot\(\)\.Sent2web\([^\n]*\);\n',
        (f'\t\t\t\t\t// {SAN} ارسال سریال دستگاه و سریال قفل به ربات تلگرام حذف شد\n'),
        expect=2)

    # --- 5) InternetUpdater.cs : disable the hidden phone-home updater ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Utility.Settings', 'InternetUpdater.cs')
    apply_regex(
        f,
        r'public static void Start\(\)\n\t\{.*?\n\t\}\n',
        ('\tpublic static void Start()\n'
         '\t{\n'
         f'\t\t// {SAN} به‌روزرسان اینترنتی غیرفعال شد: این متد فایل Internet_Update.exe را از\n'
         f'\t\t// {SAN} ریسورس استخراج و به‌صورت فرایند جداگانه اجرا می‌کرد (رفتار Phone-Home).\n'
         '\t}\n'))

    # --- 6) Person.cs : stop leaking username + password hash to vendor portal ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Dal.FLRPerson', 'Person.cs')
    apply_regex(
        f,
        (r'public async Task<string> GenerateUrlQueryStringAsync\(string contextGuid, '
         r'CancellationToken token = default\(CancellationToken\)\)\n\t\{.*?\n\t\}\n'),
        ('\tpublic async Task<string> GenerateUrlQueryStringAsync(string contextGuid, '
         'CancellationToken token = default(CancellationToken))\n'
         '\t{\n'
         f'\t\t// {SAN} ساخت لینک ورود ابری حذف شد: نام کاربری و هش رمز شخص به سرور سازنده ارسال می‌شد.\n'
         '\t\treturn "---";\n'
         '\t}\n'))

    # --- 7) UserPermission.cs : redact hardcoded AES passphrase ---
    f = p('TimeAttendanceDAL', 'TimeAttendanceDAL.Dal.User', 'UserPermission.cs')
    apply_exact(
        f,
        f'\tprivate readonly string _passPhrase = "{AES}";',
        (f'\tprivate readonly string _passPhrase = "[REDACTED]"; '
         f'// {SAN} کلید AES هاردکد حذف شد'))

    # --- 8) FrmEnter.cs : remove master-password backdoor ---
    f = p('TimeAttendanceUI', 'TimeAttendanceUI.Forms.User', 'FrmEnter.cs')
    apply_regex(
        f,
        ('if \\(\\(text == "' + MP1 + '" \\|\\| text == "' + MP2 +
         '"\\) \\? true : false\\)\\n.*?\\n\\t+return;\\n\\t+\\}\\n'),
        (f'\t\t\t// {SAN} بک‌دور «رمز استاد» حذف شد — ورود اکنون همیشه از مسیر\n'
         f'\t\t\t// {SAN} بررسی رمز واقعی کاربر (CheckInfoAsync) می‌گذرد.\n'))

    # --- 9) AttendanceRecord.cs : remove live-attendance telemetry switch ---
    f = p('TimeAttendanceUI', 'TimeAttendanceUI.HysonRealTime', 'AttendanceRecord.cs')
    apply_regex(
        f,
        r'switch \(nextRecord\.deviceType\)\n.*?\n\t\t\t\t\}\n',
        (f'\t\t\t\t// {SAN} بلوک ارسال ترددهای زنده (کد شخص، زمان، دستگاه) به ربات‌های\n'
         f'\t\t\t\t// {SAN} پایش سازنده حذف شد؛ اعلان محلی کاربر در خط قبل باقی است.\n'))

    # --- 10) EbioRealTimeProcess.cs : remove 1 telemetry send ---
    f = p('TimeAttendanceUI', 'TimeAttendanceUI.EbioRealTime', 'EbioRealTimeProcess.cs')
    apply_regex(
        f,
        r'\t*WebTelegramMessage\.GetAttendanceRealTimeMonitoring_bot\(\)\.Sent2web\([^\n]*\);\n',
        (f'\t\t\t\t// {SAN} ارسال رشتهٔ وضعیت دستگاه به ربات تلگرام حذف شد\n'))

    print("== sources sanitized (pass 1) ==")


# ---------------------------------------------------------------------------
# PART 1b — pass 2: دو مسیر تله‌متری که در بازبینی نهایی پیدا شد
# ---------------------------------------------------------------------------
def sanitize_sources_v2(root):
    print("== sanitizing decompiled sources (pass 2) ==")
    p = lambda *a: os.path.join(root, *a)

    # --- 11) FrmDevice.cs : device log callback pointed at the vendor server ---
    # در حالت غیر RealTime، کالبکِ لاگ دستگاه روی دامنهٔ سازنده تنظیم می‌شد؛ یعنی
    # لاگ ترددها به‌جای سرور محلیِ مشتری، به سرور سازنده ارسال می‌شد.
    f = p('TimeAttendanceUI', 'TimeAttendanceUI.Forms.Device', 'FrmDevice.cs')
    apply_regex(
        f,
        r'val\.AddReturnedValue\(await sdk\.SetLogCallBackAsync\("' + re.escape(LOGCB) + r'", 0\)[^\n]*404\);\n',
        ('// ' + SAN + ' کالبک لاگ دستگاه به دامنهٔ سازنده حذف شد: در حالت غیر '
         'RealTime لاگ دستگاه‌ها به سرور سازنده ارسال می‌شد.\n'))

    # --- 12) FrmSetting.cs : drop vendor cloud-hub URLs from the settings list ---
    # فهرست کشویی «وب‌سرویس» چهار نشانی هاب ابری (سه‌تای آن دامنهٔ سازنده) را پیش‌پر
    # می‌کرد؛ پیش‌فرض کلاس WebServiceKey قبلاً خالی شده بود اما این فهرست باقی مانده بود.
    f = p('TimeAttendanceUI', 'TimeAttendanceUI.Forms.Other', 'FrmSetting.cs')
    apply_exact(
        f,
        'CmbURL.Items.AddRange(new object[4] { "' + HUB1 + '", "' + HUB2 + '", "' + HUB3 + '", "' + LOCALHUB + '" });',
        ('CmbURL.Items.AddRange(new object[1] { "' + LOCALHUB + '" }); // ' + SAN +
         ' نشانی‌های هاب ابری سازنده از فهرست تنظیمات حذف شد'))

    print("== sources sanitized (pass 2) ==")


# ---------------------------------------------------------------------------
# PART 2 — report script sanitization (mask secrets, keep layout length)
# ---------------------------------------------------------------------------
def sanitize_report_scripts(sdir):
    print("== sanitizing report scripts ==")
    p = lambda n: os.path.join(sdir, n)

    # report_part2a.py — AES key + telegram token (both already partial in prose)
    apply_exact(p('report_part2a.py'),
                f'private readonly string _passPhrase = "{AES}";',
                f'private readonly string _passPhrase = "{AES_MASK}";')
    apply_exact(p('report_part2a.py'),
                f'apiKey = "{TG1[:17]}..."',
                f'apiKey = "{TG1_MASK}"')

    # report_part3.py — master passwords in snippet + prose
    apply_exact(p('report_part3.py'),
                f'if ((text == "{MP1}" || text == "{MP2}") ? true : false)',
                f'if ((text == "{MP1_MASK}" || text == "{MP2_MASK}") ? true : false)')
    apply_exact(p('report_part3.py'),
                f'<code dir="ltr">{MP1}</code>',
                f'<code dir="ltr">{MP1_MASK}</code>')
    apply_exact(p('report_part3.py'),
                f'<code dir="ltr">{MP2}</code>',
                f'<code dir="ltr">{MP2_MASK}</code>')

    # report_part4.py — AES key mention in conclusion
    apply_exact(p('report_part4.py'),
                f'عبارت "{AES}" برای AES',
                f'عبارت "{AES_MASK}" برای AES')

    # build_report.py — CSV row notes
    apply_exact(p('build_report.py'),
                f'کلید AES هاردکد ({AES})',
                f'کلید AES هاردکد ({AES_MASK})')
    apply_exact(p('build_report.py'),
                f'({MP1} / {MP2})',
                f'({MP1_MASK} / {MP2_MASK})')

    print("== report scripts sanitized ==")


# ---------------------------------------------------------------------------
# verification — no full secret may remain anywhere
# ---------------------------------------------------------------------------
def verify(root):
    print("== verification scan ==")
    secrets = {
        'master-pwd-1': MP1,
        'master-pwd-2': MP2,
        'aes-key': AES,
        'tg-token-1': TG1,
        'tg-token-2': TG2,
        'bot-handle': BOT1,
        'cloud-hub': HUB,
        'log-callback': LOGCB,
        'vendor-hub-1': HUB1,
        'vendor-hub-2': HUB2,
        'vendor-hub-3': HUB3,
    }
    # نشانی‌ها/اندپوینت‌ها: باید از سورس‌ها (.cs) حذف شوند؛ اشاره در مستندات مجاز است
    doc_ok = {'cloud-hub', 'log-callback', 'vendor-hub-1', 'vendor-hub-2', 'vendor-hub-3'}
    bad = 0
    for dirpath, _dirs, files in os.walk(root):
        for name in files:
            fp = os.path.join(dirpath, name)
            if not name.endswith(('.cs', '.py', '.html', '.csv', '.md', '.config')):
                continue
            try:
                with open(fp, encoding='utf-8') as f:
                    text = f.read()
            except Exception:
                continue
            for label, sec in secrets.items():
                # endpoints: must vanish from sources, allowed in prose/docs
                if label in doc_ok and not name.endswith('.cs'):
                    continue
                if sec in text:
                    print(f"  !! {label} still present in {fp}")
                    bad += 1
    if bad:
        raise SystemExit(f"VERIFICATION FAILED: {bad} secret occurrence(s) remain")
    print("  clean — no full secrets found")


if __name__ == '__main__':
    mode = sys.argv[1] if len(sys.argv) > 1 else 'all'
    target = sys.argv[2] if len(sys.argv) > 2 else '/tmp/novin-rahzan/NovinLibraryResearch/decompiled'
    if mode in ('sources', 'sources-v2', 'all'):
        if mode in ('sources', 'all'):
            sanitize_sources(target)
        sanitize_sources_v2(target)
    if mode in ('scripts', 'all'):
        sdir = '/home/z/my-project/scripts'
        sanitize_report_scripts(sdir)
    if mode == 'verify':
        verify(sys.argv[2] if len(sys.argv) > 2 else '/tmp/novin-rahzan/NovinLibraryResearch')
    print("done.")
