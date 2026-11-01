# -*- coding: utf-8 -*-
"""Report content — Part 1: intro + architecture (chapters 1-2)."""

from report_lib import (code, teacher, warn, info, chapter, section,
                        subsection, table, para, bullets, pill)


def build_intro():
    h = []
    h.append(chapter('یکم', 'CH-01', 'مقدمه؛ بسته‌ای که بررسی کردیم', 'Introduction'))

    h.append(para(
        'در این گزارش آموزشی، محتوای فایل فشرده <b>NovinDll&rsquo;s.zip</b> (که از طریق سرویس اشتراک‌گذاری file.kiwi دریافت شد) را '
        'موجوده به موشکافی کدهای درونی آن بررسی می‌کنیم. این بسته شامل <b>۵۱ فایل</b> با حجم کل حدود <b>۳۴٫۸ مگابایت</b> است و '
        'در واقع پوشهٔ خروجی (Bin) یک نرم‌افزار تجاری ایرانی به نام <b>«نرم‌افزار جامع حضور و غیاب و حقوق و دستمزد نوین پرداز»</b> '
        'می‌باشد؛ نام پروژهٔ این نرم‌افزار در دل کدها <b>NPACC</b> است و توسعه‌دهندهٔ آن شرکت «نوین پرداز» (مقرر در مشهد، پیش‌شمارهٔ تلفن ۰۵۱) است. '
        'هدف ما این است که مثل یک کلاس درس، لایه به لایه و کتابخانه به کتابخانه، بفهمیم این نرم‌افزار چگونه ساخته شده، '
        'هر قطعه چه وظیفه‌ای دارد و کدهای واقعی داخل آن چه می‌گویند.'
    ))

    h.append(para(
        'همهٔ کدهایی که در این گزارش می‌بینید، از داخل خود فایل‌های DLL با ابزار <b>ILSpy</b> (دیکامپایلر متن‌باز دات‌نت) استخراج شده‌اند؛ '
        'یعنی هیچ کد فرضی یا ساختگی در کار نیست و هر قطعه کد را می‌توانید با مسیر فایل منبعش مقایسه کنید. '
        'بسته‌بندی فنی نرم‌افزار هم از خود فایل‌ها قابل مشاهده است: <b>.NET Framework نسخهٔ 4.8</b>، زبان <b>C#</b>، '
        'رابط کاربری <b>WinForms</b> با کامپوننت‌های DevComponents، و پایگاه‌دادهٔ <b>Microsoft SQL Server</b>.'
    ))

    h.append(teacher(
        'وقتی یک برنامهٔ #C ساخته می‌شود، کد شما به زبان میانی IL (Intermediate Language) ترجمه و داخل فایل DLL قرار می‌گیرد. '
        'دیکامپایلرها مثل ILSpy همین زبان میانی را دوباره به #C برمی‌گردانند. به همین دلیل کدهای این گزارش گاهی شکل «عجیبی» دارند: '
        'پرانتزهای اضافه مثل <code dir="ltr">((DbCommand)(object)cmd)</code>، متغیرهای بی‌استفاده مثل <code dir="ltr">_ = 2;</code> یا '
        'کلمهٔ <code dir="ltr">num</code> به‌جای نام‌های اصلی متغیرها — این‌ها artefact (اثر جانبی) دکامپایل هستند، نه سبک برنامه‌نویسی نویسندهٔ اصلی. '
        'برای خوانایی، کامنت‌های <code dir="ltr">//IL_...</code> را حذف کرده‌ایم اما منطق کد دست‌نخورده است.',
        'کد دکامپایل‌شده یعنی چه؟'
    ))

    h.append(section('آنچه داخل بسته پیدا کردیم'))
    h.append(para(
        'بسته را باز کردیم و فایل‌ها را بر اساس نقش‌شان در چهار گروه دسته‌بندی کردیم. '
        'نکتهٔ جالب اینکه فایل اجرایی اصلی <code dir="ltr">TimeAttendance.exe</code> در این بسته <b>حاضر نیست</b> و فقط فایل‌های '
        '<code dir="ltr">TimeAttendance.exe.config</code> و <code dir="ltr">TimeAttendance.pdb</code> از آن باقی مانده‌اند؛ '
        'یعنی فرستندهٔ بسته فقط کتابخانه‌ها را جمع کرده است. فایل‌های <code dir="ltr">.pdb</code> هم اطلاعات عیب‌یابی (خط و نام فایل‌های سورس اصلی) هستند '
        'و همین فایل‌ها بودند که مسیر پروژهٔ اصلی توسعه‌دهنده را لو دادند: '
        '<code dir="ltr">D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\...</code>'
    ))

    h.append(table(
        ['گروه', 'تعداد', 'فایل‌ها', 'توضیح'],
        [
            ['<b>کتابخانه‌های اختصاصی اصلی</b>', '۷',
             '<code>TimeAttendanceDAL</code><br><code>TimeAttendanceUI</code><br><code>TimeAttendanceDevices</code><br><code>TimeAttendancePrint</code><br><code>TimeAttendance.ValidatorAttribute</code><br><code>WebSmsStandard</code><br><code>Tele</code>',
             'قلب نرم‌افزار؛ لایهٔ دسترسی داده، رابط کاربری، درایور دستگاه‌ها، چاپ، اعتبارسنجی، پیامک و تلفن'],
            ['<b>ابزارها و درایورهای کمکی</b>', '۴',
             '<code>ZK.dll</code>، <code>ZDC2911.dll</code>، <code>zkemkeeper.dll</code>، <code>zkemsdk.dll</code>',
             'درایور/SDK سازندگان دستگاه‌های اثر انگشت (ZKTeco و Realand)'],
            ['<b>کتابخانه‌های متن‌باز جانبی</b>', '۲',
             '<code>Tulpep.NotificationWindow</code>، <code>WinFormAnimation</code>',
             'نمایش اعلان‌های شیشه‌ای و انیمیشن فرم‌ها (از NuGet)'],
            ['<b>کتابخانه‌های مایکروسافتی</b>', '۲۶ + ۱۲',
             '<code>System.*.dll</code> (۲۶ فایل) و فایل‌های <code>.config</code>/<code>.pdb</code> (۱۲ فایل)',
             'زیرساخت استاندارد .NET Framework که در فصل ۱۲ مرور می‌کنیم'],
        ],
        caption='جدول ۱-۱: دسته‌بندی ۵۱ فایل موجود در بسته'
    ))

    h.append('<div class="stat-strip">'
             '<div class="stat"><div class="v">۵۱</div><div class="l">فایل در بسته</div></div>'
             '<div class="stat"><div class="v">۱۳</div><div class="l">کتابخانهٔ اختصاصی و کمکی</div></div>'
             '<div class="stat"><div class="v">۳۴٫۸</div><div class="l">مگابایت حجم کل</div></div>'
             '<div class="stat"><div class="v">۴۵۰+</div><div class="l">کلاس دکامپایل‌شده</div></div>'
             '<div class="stat"><div class="v">۶</div><div class="l">برند دستگاه پشتیبانی‌شده</div></div>'
             '</div>')

    h.append(section('چطور این گزارش را بخوانیم؟'))
    h.append(para(
        'گزارش طوری چیده شده که مثل یک دورهٔ آموزشی از بالا به پایین جلو برود: ابتدا در فصل ۲ تصویر کلان معماری را می‌بینیم، '
        'سپس در فصل‌های ۳ تا ۱۱ وارد جزئیات هر کتابخانه می‌شویم — همیشه ابتدا «این کتابخانه چه مشکلی را حل می‌کند؟» بعد «چطور کار می‌کند؟» '
        'و سپس «کد واقعی‌اش را ببینیم». در فصل ۱۲ کتابخانه‌های استاندارد مایکروسافت را جدولی مرور می‌کنیم و در فصل ۱۳ '
        'درس‌های معماری و یافته‌های امنیتی را جمع‌بندی می‌کنیم. جعبه‌های آبی «نکتهٔ معلم» مفاهیم پایه را توضیح می‌دهند و '
        'جعبه‌های قرمز «هشدار امنیتی» نقاط ضعفی را نشان می‌دهند که از بابت آموزش امنیت، دیدنشان برای شما ارزشمند است.'
    ))
    return ''.join(h)


def build_architecture():
    h = []
    h.append(chapter('دوم', 'CH-02', 'معماری کلان سیستم؛ نقشهٔ راه فنی', 'System Architecture'))

    h.append(para(
        'قبل از اینکه وارد هر کتابخانه شویم، باید نقشهٔ کلان را ببینیم. این نرم‌افزار یک <b>معماری چندلایه (Multi-Layer / n-Tier)</b> '
        'کلاسیک دارد؛ یعنی مسئولیت‌ها بین لایه‌های مجزا تقسام شده‌اند تا هر لایه فقط کار خودش را انجام دهد و تغییر یک لایه، بقیه را خراب نکند. '
        'اگر هرگز با این مفهوم آشنا نبوده‌اید، نکتهٔ معلم زیر را بخوانید.'
    ))

    h.append(teacher(
        'معماری چندلایه را با یک رستوران مقایسه کنید: <b>سالن و منو</b> (رابط کاربر) سفارش مشتری را می‌گیرد، '
        '<b>گارسن</b> (لایهٔ میانی/اعتبارسنجی) سفارش را بررسی و به آشپزخانه می‌رساند، '
        '<b>آشپزخانه</b> (لایهٔ منطق/دسترسی داده) غذا را آماده می‌کند و <b>انبار</b> (پایگاه‌داده) مواد اولیه را نگه می‌دارد. '
        'اگر مدیر رستوران منوی سالن را عوض کند، نیازی نیست آشپزخانه را بازسازی کند — هر لایه فقط از طریق «سفارش‌های استاندارد» '
        '(در دنیای نرم‌افزار: متدها و اینترفیس‌ها) با هم حرف می‌زنند.',
        'معماری چندلایه یعنی چه؟'
    ))

    h.append(section('نقشهٔ لایه‌ها'))
    h.append(para(
        'دیاگرام زیر را با دقت ببینید؛ در تمام گزارش به همین نقشه برگشت می‌کنیم. جهت خواندن از بالا (کاربر) به پایین (سخت‌افزار) است.'
    ))

    h.append('<div class="arch">'
             '<div class="arch-layer al-1">'
             '<div class="al-title">لایهٔ ۱ — رابط کاربر (Presentation)</div>'
             '<div class="al-desc">فرم‌های ویندوزی، ورود کاربران، گزارش‌ها، مانیتورینگ زندهٔ ترددها</div>'
             '<div class="arch-row"><span class="arch-box">TimeAttendanceUI.dll (۱۵ مگابایت، ~۱۵۰ فرم)</span>'
             '<span class="arch-box">TimeAttendance.exe (در این بسته غایب است)</span></div>'
             '</div>'
             '<div class="arch-arrow">▼</div>'
             '<div class="arch-layer al-2">'
             '<div class="al-title">لایهٔ ۲ — خدمات کنار رابط کاربر</div>'
             '<div class="al-desc">اعتبارسنجی ورودی‌ها، چاپ گزارش‌ها و اعلان‌های شیشه‌ای</div>'
             '<div class="arch-row"><span class="arch-box">TimeAttendance.ValidatorAttribute</span>'
             '<span class="arch-box">TimeAttendancePrint (موتور Stimulsoft)</span>'
             '<span class="arch-box">Tulpep + WinFormAnimation</span></div>'
             '</div>'
             '<div class="arch-arrow">▼</div>'
             '<div class="arch-layer al-3">'
             '<div class="al-title">لایهٔ ۳ — دسترسی به داده و منطق کسب‌وکار (Business + Data)</div>'
             '<div class="al-desc">همهٔ ذخیره/خواندن داده‌ها، محاسبهٔ کارکرد و حقوق، سینک ابری و ارسال پیامک</div>'
             '<div class="arch-row"><span class="arch-box">TimeAttendanceDAL.dll (۶٫۳ مگابایت، قلب سیستم)</span>'
             '<span class="arch-box">WebSmsStandard (پیامک)</span>'
             '<span class="arch-box">Tele (مودم صوتی)</span></div>'
             '</div>'
             '<div class="arch-arrow">▼ ▲ (دو طرفه)</div>'
             '<div class="arch-layer al-4">'
             '<div class="al-title">لایهٔ ۴ — ارتباط با سخت‌افزار (Device I/O)</div>'
             '<div class="al-desc">درایورهای برندهای مختلف دستگاه حضور و غیاب + سرویس ویندوزی رله</div>'
             '<div class="arch-row"><span class="arch-box">TimeAttendanceDevices</span>'
             '<span class="arch-box">zkemkeeper (ZKTeco)</span>'
             '<span class="arch-box">ZDC2911</span>'
             '<span class="arch-box">ZK.dll</span></div>'
             '</div>'
             '<div class="arch-arrow">▼</div>'
             '<div class="arch-layer al-1" style="background:#d7e3e9">'
             '<div class="al-title">لایهٔ ۵ — دنیای خارج</div>'
             '<div class="al-desc">دستگاه‌های اثر انگشت/چهره در شبکه، مودم تلفن، سرور ابری نوین پرداز، سرویس پیامک</div>'
             '<div class="arch-row"><span class="arch-box">دستگاه‌های ZK / Hysoon / Ebio / Eizon / ZDC2911 / NP761</span>'
             '<span class="arch-box">SQL Server (بانک Attendance0…99)</span>'
             '<span class="arch-box">novinrank.ir</span></div>'
             '</div>'
             '</div>')

    h.append(table(
        ['لایه', 'کتابخانه', 'با چه چیزی حرف می‌زند', 'فصل مرتبط'],
        [
            ['رابط کاربر', '<code>TimeAttendanceUI</code>', 'DAL (هرگز مستقیم با SQL حرف نمی‌زند)', 'فصل ۵'],
            ['اعتبارسنجی', '<code>ValidatorAttribute</code>', 'DAL + فرم نمایش خطا', 'فصل ۷'],
            ['چاپ', '<code>TimeAttendancePrint</code>', 'Stimulsoft + DAL', 'فصل ۶'],
            ['دسترسی داده', '<code>TimeAttendanceDAL</code>', 'SQL Server با ADO.NET + سرور ابری با SignalR', 'فصل ۳'],
            ['درایور دستگاه', '<code>TimeAttendanceDevices</code>', 'SDK هر برند (COM / P/Invoke / TCP)', 'فصل ۴'],
            ['پیامک', '<code>WebSmsStandard</code>', 'سرویس REST در webservice.novinp.ir', 'فصل ۹'],
            ['تلفن', '<code>Tele</code>', 'پورت سریال و دستورات AT مودم', 'فصل ۸'],
        ],
        caption='جدول ۲-۱: نقش هر کتابخانه در معماری'
    ))

    h.append(section('سفر یک «تردد» از انگشت تا فیش حقوقی'))
    h.append(para(
        'بهترین روش فهم یک سیستم حضور و غیاب، دنبال کردن مسیر یک رخداد واقعی است. فرض کنید کارمند «رضا» ساعت ۸ صبح '
        'انگشتش را روی دستگاه می‌گذارد؛ اتفاقی که در طول چند ثانیه تا چند هفته می‌افتد این است:'
    ))

    h.append('<div class="flow">'
             '<div class="flow-step"><span class="fs-num">۱</span><b>ثبت در دستگاه</b>دستگاه اثر انگشت، شمارهٔ کاربر + زمان + نوع ورودی/خروجی را در حافظهٔ خود ذخیره می‌کند</div>'
             '<div class="flow-step"><span class="fs-num">۲</span><b>دریافت توسط درایور</b>درایور برند مربوطه در TimeAttendanceDevices رکوردها را از دستگاه می‌خواند</div>'
             '<div class="flow-step"><span class="fs-num">۳</span><b>یکسان‌سازی</b>کلاس RecordAllDevice رکورد هر برند را به موجودیت استاندارد DeviceLogOnPerson تبدیل می‌کند</div>'
             '<div class="flow-step"><span class="fs-num">۴</span><b>ذخیره در بانک</b>DAL آن را با روال ذخیره‌شدهٔ sp_LogonPersonel_Insert در جدول LogonPersonel ثبت می‌کند</div>'
             '<div class="flow-step"><span class="fs-num">۵</span><b>تطبیق هویت</b>کد سخت‌افزاری (HardwareCode) کاربر با جدول اشخاص تطبیق داده می‌شود و PersonId مشخص می‌شود</div>'
             '<div class="flow-step"><span class="fs-num">۶</span><b>محاسبهٔ کارکرد</b>TaradodFullDet ورود/خروج‌ها را جفت می‌کند و کارکرد، اضافه‌کار و کسری‌کار روزانه را می‌سازد</div>'
             '<div class="flow-step"><span class="fs-num">۷</span><b>فیش حقوقی</b>کلاس Fish بر اساس قرارداد و فرمول‌ها، فیش حقوقی ماهانه تولید می‌کند</div>'
             '<div class="flow-step"><span class="fs-num">۸</span><b>خروجی</b>چاپ فیش (Print)، ارسال پیامک به مدیر (WebSms) یا اعلان زندهٔ روی مانیتور (UI)</div>'
             '</div>')

    h.append(teacher(
        'در سیستم‌های حضور و غیاب، به هر رخداد ورود یا خروج یک «تردد» (Traffic) گفته می‌شود و جدول/کلاس آن اینجا '
        '<code dir="ltr">LogOnPersonel</code> نام دارد. عبارت «سخت‌افزار کد» (HardwareCode) هم یعنی شمارهٔ کاربری که روی دستگاه '
        'اثر انگشت برای هر شخص تعریف شده است. چون ممکن است یک شخص در چند دستگاه با شماره‌های مختلف ثبت شده باشد، '
        'سیستم جدولی جداگانه (HardwareCodeCus) برای نگاشت «شمارهٔ دستگاه + کد روی دستگاه ← شخص» دارد.',
        'واژه‌نامهٔ دامنهٔ حضور و غیاب'
    ))

    h.append(section('کلید جادویی: contextGuid'))
    h.append(para(
        'در تمام کدهای این نرم‌افزار مدام با پارامتری به نام <code dir="ltr">contextGuid</code> برخواهید خورد — مثلاً '
        '<code dir="ltr">Person.GetAllAsync(contextGuid, ...)</code>. این یک شناسهٔ یکتا (GUID) است که مشخص می‌کند عملیات '
        'روی «کدام یک از زمینه‌های داده» انجام شود. نرم‌افزار به این ترتیب می‌تواند هم‌زمان چند شرکت/بانک مختلف را مدیریت کند، '
        'هر چند در عمل معمولاً یک بانک فعال است. این کلید در تمام لایه‌ها از UI تا DAL ترد می‌شود و حکم «شناسهٔ جلسهٔ فعال» را دارد؛ '
        'به همین دلیل تقریباً همهٔ کلاس‌ها اینترفیس <code dir="ltr">IHasContext</code> را پیاده‌سازی کرده‌اند.'
    ))

    h.append(info(
        'نام فایل‌های بانک اطلاعاتی این سیستم جالب است: هنگام نصب، DAL دنبال اولین نام خالی از <code dir="ltr">Attendance0</code> '
        'تا <code dir="ltr">Attendance99</code> می‌گردد و بانک را با آن نام می‌سازد! این منطق را در فصل ۳ با کد واقعی خواهیم دید.',
        'نام‌گذاری جالب بانک‌ها'
    ))
    return ''.join(h)
