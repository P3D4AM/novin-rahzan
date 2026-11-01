# -*- coding: utf-8 -*-
"""Report content — Part 3: UI (ch5), Print (ch6), Validator (ch7)."""

from report_lib import (code, teacher, warn, info, chapter, section,
                        subsection, table, para, bullets, pill)


def build_ui():
    h = []
    h.append(chapter('پنجم', 'CH-05', 'کتابخانهٔ TimeAttendanceUI؛ چهرهٔ نرم‌افزار', 'TimeAttendanceUI.dll — WinForms UI Layer'))

    h.append(para(
        'این کتابخانه با ۱۵ مگابایت و ۲۱۱ فایل سورس، بزرگ‌ترین DLL بسته است و شامل حدود <b>۱۵۰ فرم ویندوزی</b> است. '
        'رابط کاربری با WinForms و کامپوننت‌های تجاری <b>DevComponents DevComponents2</b> (منوی Ribbon همانند Office) ساخته شده، '
        'تقویم شمسی از کامپوننت BehComponents است و برای هر برند دستگاه که Real-time دارد، کنترل‌های ActiveX مربوطه هم داخل همین لایه میزبانی می‌شوند.'
    ))

    h.append(table(
        ['دستهٔ فرم‌ها', 'تعداد', 'نمونه‌ها و شرح'],
        [
            ['فرم اصلی و ورود', '۵', 'FrmMain (فرم Ribbon با ۵,۷۵۵ خط کد!)، FrmEnter (ورود)، FrmManagment (دسترسی ویژه)'],
            ['پرسنل و زیرمجموعه‌ها', '۲۵', 'FrmPersonel (۳,۰۸۷ خط)، گروه‌ها، عناوین، نوع استخدام، بخش‌ها، ایمپورت اکسل'],
            ['تردد و مرخصی', '۱۳', 'ویرایش گروهی ترددها، مرخصی روزانه/ساعتی، انواع مرخصی'],
            ['گزارش‌ها', '۲۴', 'FrmRptTaradodFull (۴,۳۰۹ خط) + طراح گزارش پارامتری FrmRPTGen'],
            ['شیفت‌ها', '۱۶', 'ساخت شیفت معمولی/شناور/دوری، کدینگ شیفت'],
            ['حقوق و دستمزد', '۱۳', 'فیش حقوقی، اسناد، کسورات توافقی، جدول مالیات، سرفصل حساب‌ها'],
            ['قراردادها', '۱۵', 'FrmGharardad، کپی گروهی قرارداد، گروه‌های سنوات، حقوق پایهٔ سالانه'],
            ['تسویه‌حساب', '۱۱', 'فرم‌های Checkout با الگوی جادویی ICheckout (ویزاردی)'],
            ['بیمه', '۳', 'شرکت‌های بیمه، لیست بیمهٔ تامین اجتماعی'],
            ['باشگاه (کلوپ)', '۱۱', 'عضویت و قراردادهای باشگاه‌های ورزشی — حسابداری تردد اعضا'],
            ['پیامک', '۴', 'تقویم رویدادهای پیامکی (تولد، تبریک)، لیست ارسال‌شده/نشده'],
            ['دستگاه‌ها', '۱۰', 'FrmDevice (۲,۲۸۱ خط)، تعریف کد سخت‌افزاری، همگام‌سازی اثر انگشت'],
            ['سرویس ابری', '۵', 'DeviceServiceHandler — نوشتن کاربر روی دستگاه از طریق اینترنت!'],
            ['سایر', '~۱۵', 'تنظیمات (۶,۸۲۳ خط!)، درباره، تعطیلات، پشتیبان‌گیری، صفحه‌کلید مجازی'],
        ],
        caption='جدول ۵-۱: نقشهٔ فرم‌های لایهٔ UI', small=True
    ))

    h.append(section('۱-۵) نقطهٔ شروع؛ یک حلقهٔ پیام دست‌ساز'))

    h.append(para(
        'کلاس Program این کتابخانه تعجب‌آور است: بعد از نمایش فرم ورود، به‌جای فرم اصلی، وارد یک حلقهٔ بی‌نهایت می‌شود که هر '
        'ثانیه یک‌بار <code dir="ltr">Application.DoEvents()</code> را صدا می‌زند! یعنی فرم اصلی توسط زیرساخت لایسنس (FormCustom) '
        'نمایش داده می‌شود و این حلقه فقط پروسه را زنده نگه می‌دارد:'
    ))

    h.append(code(
'''internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        if ((int)((Form)new FrmEnter(default(Guid).ToString())).ShowDialog() != 1)
        {
            return;
        }
        while (true)
        {
            Thread.Sleep(1000);
            Application.DoEvents();
        }
    }
}''',
        title='TimeAttendanceUI/Program.cs — ورودی برنامه با حلقهٔ پیام دستی'))

    h.append(section('۲-۵) ورود کاربران؛ و جایی که مشکل بزرگ پنهان است'))

    h.append(para(
        'منطق ورود: کاربر از لیست انتخاب و رمز تایپ می‌شود؛ اگر با هش ذخیره‌شده مطابقت داشت، نام کاربر در رجیستری '
        '(کلید <code dir="ltr">HKCU\\Software\\Novin\\service</code> با نام UId) ذخیره می‌شود تا دفعهٔ بعد انتخاب‌شده باشد. '
        'اما کد زیر را با دقت بخوانید:'
    ))

    h.append(code(
'''private async void BtnEnter_Click(object sender, EventArgs e)
{
    try
    {
        if (SelectedUser == null)
        {
            throw new NullReferenceException("SelectedUser");
        }
        ...
        string text = ((Control)TxtPASS).Text;
        if ((text == "Novin********" || text == "11****60") ? true : false)
        {
            // *** رمز استاد: ورود بدون هیچ بررسیِ دیتابیسی! ***
            UserAttendance.SelectedUser = SelectedUser;
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\service",
                "UId", SelectedUser.ID);
            ((Form)this).DialogResult = (DialogResult)1;
            ((Form)this).Close();
            return;
        }
        if (await UserAttendance.CheckInfoAsync(ContextGuid, SelectedUser.ID,
            Text.FixString(((Control)TxtPASS).Text)))
        {
            ((TextBoxBase)TxtPASS).Clear();
            UserAttendance.SelectedUser = SelectedUser;
            ...
        }
    }
}''',
        title='TimeAttendanceUI.Forms.User/FrmEnter.cs — منطق ورود به برنامه'))

    h.append(warn(
        'در خط ششم کد بالا، دو <b>رمز عبور استاد (Master Password)</b> هاردکد شده‌اند: <code dir="ltr">Novin********</code> و '
        '<code dir="ltr">11****60</code>. هر کسی که این دو رشته را بداند، می‌تواند با انتخاب «هر کاربری» (حتی مدیر کل) بدون '
        'بررسی رمز واقعی وارد شود! این الگو برای پشتیبانیِ شرکت‌های نرم‌افزاری متداول بود، اما امروزه یک در پشتی جدی محسوب می‌شود. '
        'یافتهٔ مشابه: فرم «دسترسی ویژه» (FrmManagment_Login) حتی رمز را هم چک نمی‌کند و مستقیم فرم اجرای دستی روال‌های SQL را باز می‌کند!',
        'رمز استاد هاردکدشده'
    ))

    h.append(section('۳-۵) فرم اصلی؛ مغز عملیاتی نرم‌افزار'))

    h.append(para(
        'FrmMain یک فرم Ribbon کامل است: زبان فارسی/عربی قابل تعویض، آیکن کنار ساعت ویندوز، تقویم شمسی ماهانه با تعطیلات، '
        'نوار وضعیت (نام کاربر جاری، نام بانک)، دو گرید زندهٔ «آخرین ترددها» و «رخدادهای دستگاه‌ها»، چراغ وضعیت سرویس پیامک و '
        'شمارندهٔ انقضای لایسنس. هنگام بارگذاری، همهٔ سرویس‌های پس‌زمینه روشن می‌شوند:'
    ))

    h.append(code(
'''// قسمتی از FRMMain_Load — روشن‌کردن موتورهای پس‌زمینه
await EntityCacheConfig.InitAsync(ContextGuid, ClsConnection.connectionString,
    SystemInfoClass.SystemInfo(), VersionManagment.Version, (EnConnectionType)1);
await new ConnectionManager(ContextGuid).HanldleConnectionsAsync();
UiInvokerInstance.Initialize((CacheBaseForm)(object)this);
LogOnPersonel.InstanceForEvents.OnStatusErorTaradod += ShowEventMessage;
_setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid);
...
Task.Run(() => ucLkExpireDisplay1.SetContextGuidAsync(ContextGuid, ...));
...
Task.Run((Action)CreateBackUp);
Log.Information("نرم افزار حضور باز شد\\r\\n user: " +
    UserAttendance.SelectedUser.Name + "\\r\\n DB: " +
    ClsConnection.GetDbName(...));
Task.Run((Func<Task?>)ReceiveDataDevicePolling);   // پولینگ ۱۰ دقیقه‌ای
...
Task.Run(() => ClsSms.StartSendSmsAsync(ContextGuid));   // موتور پیامک''',
        title='TimeAttendanceUI/FrmMain.cs — راه‌اندازی سرویس‌های پس‌زمینه',
        note='سطح دسترسی هر دکمهٔ منو هم از تنظیمات کاربر خوانده می‌شود؛ مثلاً BtnGharardad فقط اگر UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain فعال باشد در دسترس است.'))

    h.append(para(
        'امنیت فیزیکی فرم هم جالب است: با تابع Win32 به نام <code dir="ltr">GetLastInputInfo</code>، مدت زمان بی‌کاری کاربر اندازه‌گیری '
        'می‌شود و اگر از حد تنظیم‌شده (پیش‌فرض ۱۲۰ ثانیه) بگذرد، برنامه قفل شده و دوباره فرم ورود ظاهر می‌شود. عنوان پنجرهٔ اصلی هم '
        'سطح لایسنس را نشان می‌دهد و در نسخهٔ آزمایشی، کل Ribbon زرد/قرمز می‌شود!'
    ))

    h.append(section('۴-۵) دریافت تردد زنده؛ چهار کانال هم‌زمان'))

    h.append(para(
        'متد <code dir="ltr">OpenPortsForDevicesAsync</code> برای هر برند یک کانال زنده باز می‌کند. برای Hysoon قدیمی، کنترل '
        'ActiveX به نام AxRealSvrOcxTcp برای هر پورت ساخته می‌شود و رویدادهایش مشترک می‌گردد. وقتی کاربر انگشت بزند، '
        'دستگاه یک JSON می‌فرستد، برنامه همان‌جا جوابش را می‌دهد و رکورد را در صف (BufferBlock) می‌گذارد:'
    ))

    h.append(code(
'''public static void AxRealSvrOcxTcp1_OnReceiveGLogTextAndImage(object sender,
    _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent e)
{
    try
    {
        JObject val = JObject.Parse(e.astrLogText);
        string text = ((object)val["io_time"])?.ToString();
        if (text != null)
        {
            DateTime logTime = DateTime.Parse(
                text.Substring(0, 4) + "-" + text.Substring(4, 2) + "-" +
                text.Substring(6, 2) + " " + text.Substring(8, 2) + ":" +
                text.Substring(10, 2) + ":" + text.Substring(12, 2));
            AttendanceRecord attendanceRecord = new AttendanceRecord(ContextGuid)
            {
                transId = long.Parse(((object)val["log_id"])?.ToString()),
                personNo = int.Parse(((object)val["user_id"])?.ToString()),
                deviceNo = int.Parse(((object)val["fk_device_id"])?.ToString()),
                ioMode = ((object)val["io_mode"])?.ToString(),
                logTime = logTime,
                deviceType = "NP587_N",
                deviceIp = e.astrClientIP
            };
            // پاسخ فوری به دستگاه
            JObject val2 = new JObject();
            val2.Add("log_id", attendanceRecord.transId.ToString());
            val2.Add("result", "OK");
            ((AxRealSvrOcxTcp)sender).SendResponse(e.astrClientIP,
                e.anClientPort, val3.ToString());
            // تحویل به صف پردازش
            AttendanceRecord._bufferBlock.Post(attendanceRecord);
        }
    }
}''',
        title='TimeAttendance.HysonRealTime/HysoonRealTimeProcess.cs — رویداد تردد آنی'))

    h.append(para(
        'مصرف‌کنندهٔ این صف، رکورد را ذخیره می‌کند و یک اعلان ویندوزی با متن فارسی نمایش می‌دهد: «سریال قفل: … نسخهٔ نرم‌افزار: … '
        'کد شخص: … زمان: … نوع تردد: …». برای دستگاه‌های Ebio (تشخیص رگ کف دست) هم سرور TCP رویداد TimeLog می‌دهد که وضعیت‌های '
        'Duty On/Off و Overtime و In/Out را به کدهای تردد سیستم ترجمه می‌کند. کانال چهارم، همان HTTP Push دوربین‌های NP761 و '
        'Hysoon جدید است که در فصل قبل دیدیم.'
    ))

    h.append(section('۵-۵) فارسی‌سازی کامل'))

    h.append(para(
        'راست‌چین بودن فرم‌ها با <code dir="ltr">RightToLeft = 1</code> — که در کد دکامپایل‌شده <b>۵۹۴ بار در ۱۵۹ فایل</b> تکرار '
        'شده — فونت‌های فارسی Tahoma و B Titr، تاریخ‌های شمسی با کلاس Calendar و تبدیل <code dir="ltr">MiladiToCalture</code>، '
        'جعبه‌پیام فارسی FMessageBox و حتی تعویض زبان کل رابط به عربی، همه در این لایه پیاده شده‌اند. رشته‌های فارسی به‌صورت '
        '«کلید ترجمه» درج می‌شوند: <code dir="ltr">Utilities.GetTranslate("[اطلاعات با موفقیت ثبت شد]")</code>.'
    ))

    h.append(teacher(
        'چرا ۱۵ مگابایت؟ چون در WinForms همهٔ چیز — حتی یک لیبل کوچک — با کد #C در متد InitializeComponent تولید می‌شود و '
        'هر فرم ده‌ها کنترل دارد. ضمناً تصاویر و آیکن‌های فارسی (ریسورس‌ها) هم داخل DLL جاسازی شده‌اند. برای مقایسه: یک فرم '
        '30 کنترله در WPF یا وب، کسری از این حجم را می‌گیرد.',
        'چرا این‌همه حجم؟'
    ))
    return ''.join(h)


def build_print():
    h = []
    h.append(chapter('ششم', 'CH-06', 'کتابخانهٔ TimeAttendancePrint؛ چاپ و گزارش‌سازی', 'TimeAttendancePrint.dll — Stimulsoft Reporting'))

    h.append(para(
        'این کتابخانه (۲٫۲ مگابایت) مسئول همهٔ خروجی‌های کاغذی سیستم است و موتور آن <b>Stimulsoft Reports</b> است — یکی از '
        'محبوب‌ترین ابزارهای گزارش‌سازی دات‌نت که قالب گزارش را در فایل‌هایی با پسوند <code dir="ltr">.mrt</code> نگه می‌دارد. '
        'ایدهٔ مرکزی: قالب‌های ازپیش‌طراحی‌شده داخل ریسورس DLL جاسازی شده‌اند و در اولین اجرا در پوشه‌ای فارسی‌نام روی دیسک ریخته می‌شوند.'
    ))

    h.append(code(
'''public StiReport GetSti(StiReport sti, Print.EnPrinterType paper,
                        EnumPrintShow printShowType)
{
    _printShowType = printShowType;
    ReportPath.CreatePath(ReportPath.ReportPath_Renamed);
    ReportPath.CreatePath(ReportPath.ReportPath_Renamed + "\\\\" + FolderName);
    string text = ReportPath.ReportPath_Renamed + "\\\\" + FolderName + "\\\\" +
        Utilities.GetAttReportFileName<EnumPrintShow>(_printShowType) + ".mrt";
    if (paper == Print.EnPrinterType.AllA5Printers)
    {
        text = ...TrimEnd('4') + "5.mrt";   // نسخهٔ A5 قالب
    }
    if (!File.Exists(text))
    {
        switch (paper)
        {
            case Print.EnPrinterType.AllA4Printers:
                switch (printShowType - 1)
                {
                    case 0: File.WriteAllBytes(text, Resources.Taradodkamel_A4); break;
                    case 1: File.WriteAllBytes(text, Resources.Personel_A4); break;
                    case 2: File.WriteAllBytes(text, Resources.listFish_A4); break;
                    case 5: File.WriteAllBytes(text, Resources.Fish_A4); break;
                    case 9: File.WriteAllBytes(text, Resources.Gharardad_A4); break;
                    ...
                }
                break;
        }
    }
    sti.Load(text);
    return sti;
}''',
        title='TimeAttendancePrint.SetSTI/ClsGeneral.cs — استخراج قالب‌های .mrt از ریسورس',
        note='مقدار FolderName برابر «حضور-غیاب و حقوق-دستمزد» است؛ یعنی کنار EXE برنامه پوشه‌ای با این نام فارسی ساخته می‌شود!'))

    h.append(para(
        'بعد از انتخاب قالب، کلاس ClsPrint داده‌ها را به گزارش «تزریق» می‌کند. دقت کنید نام منابع داده هم فارسی است — '
        '«لیست داده‌ها» و «منبع داده‌ها» — که در طراح گزارش Stimulsoft با همین نام‌ها ظاهر می‌شوند:'
    ))

    h.append(code(
'''public async Task Print2PrinterAsync(StiReport st, DataSet ds,
    List<object> list, int sanadId, string extraVar, List<object> list2,
    CancellationToken token)
{
    try
    {
        ((CollectionBase)(object)st.Dictionary.Databases).Clear();
        StiSqlDatabase val = new StiSqlDatabase("Database",
            Extensions.GetSqlConnectionString(ContextGuid));
        st.Dictionary.Databases.Add((StiDatabase)(object)val);
        st.Dictionary.Variables.Add("DateM", (object)DateTime.Now);
        st.Dictionary.Variables.Add("DateCalture",
            (object)Calendar.MiladiToCalture(DateTime.Now));   // تاریخ شمسی
        st.Dictionary.Variables.Add("SanadID", (object)sanadId);
        st.RegData("لیست داده ها", (object)list);          // نام منبع دادهٔ فارسی!
        st.RegData("DataList", (object)list2);
        st.RegData("منبع داده ها", ...);
        if (Print.ShowDesign)
        {
            st.Design();     // باز کردن طراح گزارش
        }
        else
        {
            st.Print(false, (short)1);   // چاپ مستقیم
        }
    }
    ...
}''',
        title='TimeAttendancePrint/ClsPrint.cs — تزریق داده به گزارش Stimulsoft و چاپ',
        note='کلاس ReportGenerator لایهٔ بالاتر است: بر اساس نوع گزارش (EnumPrintShow) و اندازهٔ کاغذ (A4/A5/رسید ۸ سانتی) قالب مناسب را انتخاب می‌کند.'))

    h.append(para(
        'پشتیبانی از سه اندازهٔ کاغذ — A4، A5 و «۸ سانت» (رسیدهای收款 کارت‌خوان‌مانند برای چاپگرهای حرارتی) — به‌علاوهٔ '
        'حالت پیش‌نمایش (FrmPrintPreview) و حالت طراحی زنده، این کتابخانه را به یک زیرسیستم کامل چاپ تبدیل کرده است. '
        'فایل تنظیمات آن هم (که در فصل ۱ دیدیم) حاوی یک کوئری SQL با ستون‌های کاملاً فارسی برای «مشخصات شرکت» است.'
    ))

    h.append(teacher(
        'موتورهای گزارش‌ساز مثل Stimulsoft، Crystal Reports و FastReport همه یک ایده دارند: <b>جداسازی «قالب» از «داده»</b>. '
        'قالب (mrt.) مشخص می‌کند جدول‌ها و متن‌ها کجای صفحه بیایند؛ داده هنگام اجرا با متدهایی مثل RegData تزریق می‌شود. '
        'مزیت: طراح می‌تواند قالب را بدون دست‌زدن به کد برنامه تغییر دهد — و همین الان هم اگر پوشهٔ «حضور-غیاب...» را باز کنید، '
        'می‌توانید mrt ها را با Stimulsoft Designer ویرایش کنید.',
        'موتور گزارش‌ساز چگونه کار می‌کند؟'
    ))
    return ''.join(h)


def build_validator():
    h = []
    h.append(chapter('هفتم', 'CH-07', 'کتابخانهٔ ValidatorAttribute؛ نگهبان داده‌ها', 'TimeAttendance.ValidatorAttribute.dll'))

    h.append(para(
        'این کتابخانهٔ کوچک (۷۷ کیلوبایت، ~۲,۵۰۰ خط) کتابِ قواعد اعتبارسنجی سیستم است: قبل از اینکه هر موجودیتی در بانک ذخیره شود، '
        'ولیداتور مخصوصش اجرا می‌شود و اگر ایرادی باشد، پیام فارسی به کاربر نشان می‌دهد و مانع ذخیره می‌شود. ساختار هر حوزه دو کلاس دارد: '
        '<b>XxxValidator</b> (قواعد) و <b>XxxCheckValidation</b> (اجرای قواعد + نمایش نتیجه).'
    ))

    h.append(table(
        ['حوزه', 'کلاس‌ها', 'نمونهٔ قواعد'],
        [
            ['پرسنل', 'PersonValidator', 'نام خانوادگی اجباری؛ یکتا بودن کد معین/تفصیلی'],
            ['شیفت‌ها', 'ShiftsValidator، ShiftDetValidator', 'همپوشانی نبودن بازه‌های شیفت'],
            ['قرارداد', 'GharardadDetValidator، CopyGharardadValidator', 'اعتبار تاریخ‌ها و تاریخچهٔ حقوق'],
            ['فیش حقوقی', 'PayRollValidator، ListFishValidator', 'قفل بودن فیش قبلی، دسترسی کاربر'],
            ['سند حسابداری', 'AttSanadValidator، AttSanadDetValidator', 'تراز بودن بدهکار/بستانکار'],
            ['بیمه', 'InsuranceValidator', 'اعتبارسنجی لیست بیمه'],
            ['پیامک', 'PayamakValidator', 'اعتبار شماره‌ها و متن'],
            ['مرخصی', 'VacationValidator', 'ماندهٔ مرخصی و بازهٔ مجاز'],
            ['باشگاه', 'PackageValidator', 'اعتبار بستهٔ باشگاه'],
        ],
        caption='جدول ۷-۱: ولیداتورهای سیستم (۱۸ جفت کلاس)', small=True
    ))

    h.append(para('بیایید قواعد اعتبارسنجی «شخص» را از نزدیک ببینیم — این همان کدی است که وقتی دکمهٔ ثبت فرم پرسنل را می‌زنید اجرا می‌شود:'))

    h.append(code(
'''public async Task<ReturnedSaveFuncInfo> ValidateAsync(CancellationToken token)
{
    ReturnedSaveFuncInfo errorsInfo = new ReturnedSaveFuncInfo();
    try
    {
        // ۱) نام خانوادگی اجباری است
        if (((PersonBase)_obj).Family != null &&
            string.IsNullOrEmpty(((PersonBase)_obj).Family))
        {
            errorsInfo.AddReturnedValue((ReturnedState)2,
                "وارد کردن نام خانوادگی الزامی است.", ...);
        }
        // ۲) ترکیب «کد معین + کد تفصیلی» باید یکتا باشد
        if (_obj != null && !(await BeAValidMoeinTafsilAsync(
            ((PersonBase)_obj).Id + ":" +
            ((PersonBase)_obj).CodeMoein + ":" +
            ((PersonBase)_obj).CodeTafsil, token)))
        {
            errorsInfo.AddReturnedValue((ReturnedState)2,
                "کارمند دیگری با این کد معین و تفصیل وجود دارد", ...);
        }
        // ۳) کدهای حسابداری صفر نباشند
        if (((PersonBase)_obj).CodeMoein == 0L)
        {
            errorsInfo.AddReturnedValue((ReturnedState)2,
                "کد معین نامعتبر است", ...);
        }
        if (((PersonBase)_obj).CodeTafsil == 0L)
        {
            errorsInfo.AddReturnedValue((ReturnedState)2,
                "کد تفضیل نامعتبر است", ...);
        }
    }
    catch (Exception ex)
    {
        WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateAsync", ...);
    }
    return errorsInfo;
}

private async Task<bool> BeAValidMoeinTafsilAsync(string postcode,
    CancellationToken token)
{
    string[] array = postcode.Split(':');
    return await Person.GetCountByCodeMAsync(ContextGuid,
        int.Parse(array[0]), array[1], array[2], token) <= 0;
}''',
        title='TimeAttendance.ValidatorAttribute.FLRPerson/PersonValidator.cs'))

    h.append(para(
        'و این هم لایهٔ اجرا که نتیجهٔ اعتبارسنجی را به کاربر نشان می‌دهد و تصمیم ادامه/توقف را برمی‌گرداند:'
    ))

    h.append(code(
'''public static async Task<DialogResult> CheckValidationAsync(
    string contextGuid, Person pObj, bool isShowMessage,
    CancellationToken token)
{
    try
    {
        ReturnedSaveFuncInfo val =
            await new PersonValidator(contextGuid, pObj).ValidateAsync(token);
        DialogResult result;
        if (val.HasError || val.HasInformation || val.HasWarning)
        {
            FrmShowErrorMessage val2 = new FrmShowErrorMessage(
                contextGuid, val, "", (MessageBoxButtons)3, false, ...);
            ((Form)val2).ShowDialog();      // نمایش خطاها به کاربر
            result = ((Form)val2).DialogResult;
        }
        else
        {
            result = (DialogResult)6;       // Yes → ادامهٔ ذخیره‌سازی
        }
        return result;
    }
    ...
}''',
        title='PersonCheckValidation.cs — نمایش نتیجهٔ اعتبارسنجی',
        note='در فرم پرسنل (فصل ۵) دیدیم که BtnAccept_Click اول همین متد را صدا می‌زند و فقط اگر نتیجه Yes بود به SaveAsync می‌رسد — یعنی ولیدیشن واقعاً اجباری است.'))

    h.append(teacher(
        'این معماری با الگوی معروف <b>FluentValidation</b> در دنیای دات‌نت شباهت دارد: قواعد در کلاسی جدا از موجودیت و جدا از UI '
        'تعریف می‌شوند و خروجی یک «کیف نتیجه» (ReturnedSaveFuncInfo) است که هم خطا، هم هشدار و هم اطلاعات می‌تواند داشته باشد. '
        'مزیتش: همان قواعد در ذخیرهٔ دستی، ایمپورت اکسل و سینک ابری هم به کار می‌آیند.',
        'شباهت به FluentValidation'
    ))
    return ''.join(h)
