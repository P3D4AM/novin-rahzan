# -*- coding: utf-8 -*-
"""Report content — Part 4: Tele, WebSms, native DLLs, aux libs, System.*, conclusion."""

from report_lib import (code, teacher, warn, info, chapter, section,
                        subsection, table, para, bullets, pill)


def build_tele():
    h = []
    h.append(chapter('هشتم', 'CH-08', 'کتابخانهٔ Tele؛ مودم صوتی و تردد تلفنی', 'Tele.dll — AT Command Voice Modem'))

    h.append(para(
        'جذاب‌ترین کتابخانهٔ کوچک بسته! Tele (۲۳ کیلوبایت) یک <b>مودم صوتی</b> را از طریق پورت سریال کنترل می‌کند تا کارکنانی که '
        'دسترسی به دستگاه ندارند، با <b>تماس تلفنی</b> و واردکردن کد پرسنلی با کلیدهای گوشی، تردد ثبت کنند — کلاس اصلی آن '
        'به‌حق نام <code dir="ltr">NovinPhone</code> دارد. این کتابخانه دلیل حضور فایل System.IO.Ports.dll در بسته است.'
    ))

    h.append(para(
        'زبان مودم‌ها <b>دستورات AT</b> است (AT از کلمهٔ Attention). مودم را با متن سریال_config می‌کنند و پاسخ OK یا ERROR '
        'می‌گیرند. جدول مهم‌ترین دستورات استفاده‌شده در این کتابخانه:'
    ))

    h.append(table(
        ['دستور AT', 'کارکرد در Tele', 'توضیح'],
        [
            ['<code dir="ltr">ATZ</code>', 'ریست مودم', 'بازگشت به تنظیمات کارخانه'],
            ['<code dir="ltr">AT</code>', 'تست سلامت', 'پاسخ OK یعنی مودم زنده است'],
            ['<code dir="ltr">AT+VCID=1</code>', 'فعال‌سازی شناسهٔ تماس‌گیرنده', 'Caller ID برای ثبت شمارهٔ تماس'],
            ['<code dir="ltr">AT S0=3</code>', 'پاسخ خودکار بعد از ۳ زنگ', 'پیش‌فرض autoAnswerCount = 3'],
            ['<code dir="ltr">AT+FCLASS=8</code>', 'ورود به حالت صوت', 'مودم به‌جای دیتا، صوت را مدیریت می‌کند'],
            ['<code dir="ltr">ATD&lt;شماره&gt;;</code>', 'شماره‌گیری', 'تماس خروجی (مثلاً برای اطلاع‌رسانی)'],
            ['<code dir="ltr">AT+VTX</code>', 'ارسال صوت خام', 'شروع استریم WAV به تماس‌گیرنده'],
            ['<code dir="ltr">AT+VSM=9,8000</code>', 'تنظیم فرمت صوت', 'نمونه‌برداری ۸ کیلوهرتز'],
        ],
        caption='جدول ۸-۱: دستورات AT بکاررفته در Tele.dll'
    ))

    h.append(para('اجرای صوت فایل WAV برای شنیدن منوی صوتی، شاید آموزنده‌ترین بخش این کتابخانه باشد:'))

    h.append(code(
'''public void Play_Wave(string path)
{
    try
    {
        if (Status == ENStatus.PlayingVoice ||
            Status == ENStatus.Dissconnectd)
        {
            return;
        }
        StopPlaying = false;
        Status = ENStatus.PlayingVoice;
        VoiceMode();
        ExecCommnad("AT+FCLASS=8");      // حالت صوتی
        ExecCommnad("AT+VSM9,8000");     // فرمت ۸ کیلوهرتز
        ExecCommnad("AT+VSD\\u00128,0");
        ExecCommnad("AT+VTX");           // شروع ارسال صوت
        byte[] array = File.ReadAllBytes(path);
        _serial.DtrEnable = true;
        int num = 44;                    // پرش از روی هدر ۴۴ بایتی WAV!
        int num2 = 1024;                 // تکه‌های ۱ کیلوبایتی
        while (true)
        {
            Thread.Sleep(80);            // ریتم ارسال: هر ۸۰ میلی‌ثانیه
            if (StopPlaying)
            {
                Status = ENStatus.PlayingVoiceFinished;
                StopPlaying = false;
                OnPlayingFinished();
                return;
            }
            _serial.Write(array, num, num2);
            num += num2;
            if (array.Length - num < 800)
            {
                num2 = array.Length - num;
            }
            if (num2 <= 0) break;
            Application.DoEvents();
        }
        ExecCommnad("AT+FCLASS=0");      // بازگشت به حالت دیتا
        OnPlayingFinished();
    }
    catch (Exception ex2) { ... }
}''',
        title='Tele/NovinPhone.cs — پخش فایل WAV روی خط تلفن',
        note='نکتهٔ فنی: ۴۴ بایت اول فایل WAV هدرِ استاندارد RIFF است و مستقیم به مودم فرستاده نمی‌شود؛ فقط دادهٔ خام صوت ارسال می‌شود.'))

    h.append(para(
        'کلاس <code dir="ltr">CLSNumberDetector</code> گوش می‌دهد که تماس‌گیرنده چه اعدادی با کلیدهای گوشی فشرده است (دیتون‌های '
        'DTMF که به‌صورت کاراکتر از مودم می‌رسند) و هرچه تا فشردن * یا # تایپ شد را به‌عنوان کد پرسنلی برمی‌گرداند:'
    ))

    h.append(code(
'''public static string GetInput(NovinPhone phone, short maxSeconds)
{
    DateTime now = DateTime.Now;
    string text = "";
    while (Math.Abs(now.Subtract(DateTime.Now).Seconds) < maxSeconds)
    {
        Thread.Sleep(100);
        string text2 = phone.ReadLine();
        if (!(text2 == "") && !(text2 == "-"))
        {
            if (text2 == "*" || text2 == "#")
            {
                return text;         // پایان ورودی کاربر
            }
            text += ATCommandManager.GetInputNumber(text2);
        }
    }
    return text;                     // پایان مهلت زمانی
}''',
        title='Tele/CLSNumberDetector.cs — دریافت کد پرسنلی از کلیدهای تلفن',
        note='رویدادهای NovinPhone: Ring (زنگ)، ConnectCall، EndCall، Tone (هر کلید)، PlayingFinished و TerminalNewMessage. تمام وضعیت‌ها هم در enum وضعیت (ENStatus) مدیریت می‌شوند: زنگ‌خوردن، در حال تماس، پخش صوت، ضبط صوت...'))

    h.append(info(
        'متد <code dir="ltr">GetInputNumber</code> در ATCommandManager کار پاک‌سازی ورودی را انجام می‌دهد: پسوندهای H0/H1 و '
        'کاراکترهای + = کنار ارقام را حذف می‌کند و فقط رقم‌ها را نگه می‌دارد. همچنین کلاس FileManager هر گفتگوی سریال را در سه '
        'فایل Input.txt، Output.txt و ALL.txt کنار برنامه ثبت می‌کند — برای عیب‌یابی مودم بدون ابزار خاص.',
        'ثبت کامل لاگ ارتباط سریال'
    ))
    return ''.join(h)


def build_sms():
    h = []
    h.append(chapter('نهم', 'CH-09', 'کتابخانهٔ WebSmsStandard؛ ارسال پیامک', 'WebSmsStandard.dll — SMS REST Client'))

    h.append(para(
        'این کتابخانه (۲۶ کیلوبایت) کلاینت REST سرویس پیامکِ خود شرکت است و به آدرس '
        '<code dir="ltr">http://webservice.novinp.ir/sms/Npsms/...</code> متصل می‌شود. کارش ساده اما مهم است: در DAL دیدیم که '
        'موتور پیامک (ClsSms) برای غیبت‌های روزانه یا ترددهای ناقص، پیامک‌های خودکار می‌سازد؛ ارسال واقعیِ آن‌ها از همین‌جا انجام می‌شود.'
    ))

    h.append(para('مدل درخواست، ساده و JSON-محور است:'))

    h.append(code(
'''public class SmsModelV2 : SmsModelBase
{
    public List<string> Numbers { get; set; }    // شماره‌های گیرنده
    public List<string> Messages { get; set; }   // متن‌ها (نسبت ۱ به ۱)
}

// فیلدهای کلاس پایه SmsModelBase:
// UserName, Password, Domain, SenderNumber''',
        title='WebSmsStandard/SmsModelV2.cs — مدل درخواست ارسال'))

    h.append(code(
'''private static async Task<long[]> SendSmsAsync_(
    string Username, string Pass, string Domain, List<string> listMSG,
    List<string> listNum, string SenderNumber, CancellationToken inputToken,
    int tryCount = 10)
{
    ...
    SmsModelV2 smsModelV = new SmsModelV2
    {
        UserName = Username,
        Password = Pass,
        Domain = Domain,
        SenderNumber = SenderNumber,
        Messages = listMSG,
        Numbers = listNum
    };
    try
    {
        HttpResponseMessage response = await HttpHelpers.PostStreamAsync(
            (object)smsModelV,
            "http://webservice.novinp.ir/sms/Npsms/SendSmsV2/", token);
        ...
        if (response != null && response.IsSuccessStatusCode)
        {
            ActiveSendJsonResponse activeSendJsonResponse =
                await HttpHelpers.ReadJsonResponseAsync<ActiveSendJsonResponse>(response);
            if (activeSendJsonResponse != null &&
                activeSendJsonResponse.Result != null)
            {
                return activeSendJsonResponse.Result.ToArray();  // شناسه‌های پیامک
            }
        }
    }
    catch (OperationCanceledException)
    {
        if (tryCount > 0 && !inputToken.IsCancellationRequested)
        {
            await Task.Delay(new Random().Next(30000));   // مکث تصادفی تا ۳۰ ثانیه
            return await SendSmsAsync_(Username, Pass, Domain, listMSG,
                listNum, SenderNumber, inputToken, tryCount - 1);
        }
        return new long[1] { -119L };
    }
    ...
}''',
        title='WebSmsStandard/SmsSender.cs — ارسال با تلاش مجدد (۱۰ بار)',
        note='خروجی آرایه‌ای از long است: شناسهٔ هر پیامک برای پیگیری وضعیت تحویل؛ اعداد منفی هم کدهای خطای داخلی هستند (مثلاً -119 = لغو عملیات، -110 = خطای شبکه).'))

    h.append(para(
        'بخش آموزشیِ این کتابخانه، کلاس <code dir="ltr">RetryClientFactory</code> است که ترفند حرفه‌ای «تزریق هندلر retry در HttpClient» '
        'را نشان می‌دهد — الگویی که در پروژه‌های مدرن هم روز اول استفاده می‌شود:'
    ))

    h.append(code(
'''internal class RetryClientFactory
{
    public class RetryHandler : DelegatingHandler
    {
        private readonly int _maxRetries = 3;
        private readonly int _delay = 2000;

        public RetryHandler(HttpMessageHandler innerHandler,
                            int maxRetries, int delay) : base(innerHandler)
        {
            _maxRetries = maxRetries;
            _delay = delay;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < _maxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                // مکث نمایی با کمی تصادفی‌سازی (Jitter)
                await Task.Delay((int)((new Random().NextDouble() + 0.5) * _delay));
            }
            return response;
        }
    }

    public HttpClient GetClient()
    {
        return new HttpClient(new RetryHandler(new HttpClientHandler(),
            _maxRetries, _delay));
    }
}''',
        title='WebSmsStandard/RetryClientFactory.cs — الگوی Retry در سطح HttpClient'))

    h.append(teacher(
        '<code dir="ltr">DelegatingHandler</code> یک لایهٔ میانی در خط لولهٔ HttpClient است؛ مثل یک پستچی وسطی که هر درخواست را '
        'اول خودش بررسی می‌کند، به مقصد می‌فرستد و در صورت نیاز دوباره تلاش می‌کند. الگوی Retry با «مکث تصادفی» (Jitter) هم از '
        'هم‌زمان‌شدن هزاران تلاشِ مجدد پس از قطعی سرور جلوگیری می‌کند — همان چیزی که در کتابخانه‌های Polly و resilience مدرن هم هست.',
        'DelegatingHandler چیست؟'
    ))

    h.append(para(
        'متد دیگر، <code dir="ltr">GetDeliveryAsync</code> است که وضعیت تحویلِ پیامک‌های ارسال‌شده را با POST فرم-انکد (نه JSON) '
        'می‌پرسد و آرایه‌ای از کدهای وضعیت برمی‌گرداند؛ و <code dir="ltr">GetCreditAsync</code> موجودی پنل پیامک را نشان می‌دهد. '
        'همچنین کلاس OutBox_Other_Info مدل صف خروجی پیامک‌ها را در بانک محلی نگه می‌دارد.'
    ))
    return ''.join(h)


def build_native():
    h = []
    h.append(chapter('دهم', 'CH-10', 'کتابخانه‌های ZK، ZDC2911 و zkemkeeper؛ دنیای بومی', 'ZK.dll / ZDC2911.dll / zkemkeeper.dll / zkemsdk.dll'))

    h.append(section('۱-۱۰) ZK.dll؛ ابزار کوچک تست دستگاه'))

    h.append(para(
        'فایل ZK.dll (۱۰ کیلوبایت) یک برنامهٔ کوچک WinForms است (کلاس‌های Program و Form1 را دارد) که کارش خواندن ترددهای '
        'دستگاه ZKTeco است — عملاً نسخهٔ آزمایشی/جیبی‌شدهٔ همان ClsZK که در فصل ۴ دیدیم، به‌علاوهٔ یک کلاس Record با ۱۴ پراپرتی و '
        'پراپرتی محاسباتی <code dir="ltr">DateM</code> که اجزای سال/ماه/روز را به DateTime تبدیل می‌کند. جالب‌ترین بخشش این است که '
        'مستقیماً بعد از اتصال در سازنده، GetLog را اجرا می‌کند؛ یعنی «وصل شو و بلافاصله همه‌چیز را بخوان»!'
    ))

    h.append(code(
'''public ZK(int MachineNum, string IP, int port, string Pass)
{
    _iMachineNumber = MachineNum;
    _IP = IP;
    _Port = port;
    _Pass = Pass;
    _bIsConnected = axCZKEM1.Connect_Net(IP, port);
    GetLog();     // خواندن فوری پس از اتصال!
}''',
        title='ZK/ZK.cs — سازندهٔ کلاس ZK',
        note='پورت پیش‌فرض 5005 است — پورت استاندارد TCP دستگاه‌های ZKTeco.'))

    h.append(section('۲-۱۰) ZDC2911.dll؛ SDK شرکت Realand'))

    h.append(para(
        'این DLL (۱۱۶ کیلوبایت) در واقع SDK نمونهٔ سازندهٔ Realand برای دستگاه‌های ZDC2911 است که تیم نوین پرداز آن را به‌صورت '
        'درون‌سازی (Embed) استفاده کرده. کلاس اصلی <code dir="ltr">Realand</code> یک فرم ویندوزی داخلی (CMForm) را بسته‌بندی می‌کند و '
        'از SDK مدیریت‌شدهٔ <code dir="ltr">Riss.Devices</code> استفاده می‌کند:'
    ))

    h.append(code(
'''public class Realand
{
    private CMForm frm = null;

    public Realand(int DeviceNumber, string Password, string IP, int Port)
    {
        frm = new CMForm(DeviceNumber, Password, IP, Port);
    }

    public bool Connect()  { return frm.Connect(); }
    public bool Disconnect() { return frm.DisConnect(); }

    public List<Records> GetALLGlog()
    {
        List<Record> aLLGlog = frm.GetALLGlog();
        if (aLLGlog == null)
        {
            return null;
        }
        List<Records> list = new List<Records>();
        foreach (Record item in aLLGlog)
        {
            Records records = new Records();
            records.Action = item.Action;
            records.Antipassback = item.Antipassback;
            records.Clock = item.Clock;          // زمان تردد
            records.DIN = item.DIN;              // شمارهٔ کاربر
            records.DN = item.DN;                // شمارهٔ دستگاه
            records.Verify = item.Verify;        // نوع تأیید
            list.Add(records);
        }
        return list;
    }
}''',
        title='ZDC2911/Realand.cs — لایهٔ بسته‌بندی SDK',
        note='نام‌گذاری فیلدهای Record در این SDK (DN/DIN/Clock/Verify/MDIN) دقیقاً همان SDK Riss است؛ این دو خانوادهٔ سخت‌افزاری خویشاوندند! ZDC2911 همچنین ۱۰ فرم UI نمونه (GLogForm، EnrollForm، RealTimeLogForm...) دارد که نشان می‌دهد SDK به‌صورت یک برنامهٔ دمو هم قابل اجراست.'))

    h.append(section('۳-۱۰) zkemkeeper.dll و zkemsdk.dll؛ قلب بومی ZKTeco'))

    h.append(para(
        'این دو فایل با هم ۸۵۸ کیلوبایت و برخلاف بقیهٔ DLL های بسته، <b>دات‌نت نیستند</b> — فایل‌های بومی PE ۳۲بیتی (x86) هستند. '
        '<code dir="ltr">zkemkeeper.dll</code> همان SDK رسمی ZKTeco است که کلاس COM معروف <b>CZKEM</b> را ارائه می‌دهد؛ یعنی همان '
        'چیزی که ClsZK و ZK.dll از طریق Interop با آن کار می‌کنند. <code dir="ltr">zkemsdk.dll</code> هم لایهٔ پایین‌تر همان خانواده '
        'است. با اسکن نمادهای داخلی این دو فایل، فهرست توابع زیر شناسایی شد:'
    ))

    h.append(table(
        ['نمونهٔ تابع بومی', 'کارکرد'],
        [
            ['<code dir="ltr">Z_Connect_NET / Z_Connect_COM / Z_Connect_USB</code>', 'اتصال از راه شبکه / پورت سریال / USB'],
            ['<code dir="ltr">Z_ReadLogEx / Z_GetLog / Z_GetOpLog</code>', 'خواندن ترددها و لاگ عملیات'],
            ['<code dir="ltr">Z_GetUserInfo / Z_SetUserInfo</code>', 'خواندن/نوشتن اطلاعات کاربران'],
            ['<code dir="ltr">Z_GetUserTmp / Z_SetUserTmp</code>', 'انتقال قالب اثر انگشت'],
            ['<code dir="ltr">Z_EnableDevice / Z_DisableDevice</code>', 'قفل/آزادسازی دستگاه هنگام انتقال داده'],
            ['<code dir="ltr">Z_GetDeviceStatus / Z_GetDeviceInfo</code>', 'وضعیت و مشخصات دستگاه'],
            ['<code dir="ltr">Z_SetTime / Z_GetTime</code>', 'همگام‌سازی ساعت دستگاه'],
            ['<code dir="ltr">Z_PowerOffDevice / Z_RebootDevice</code>', 'خاموش/ریست دستگاه از راه دور'],
            ['<code dir="ltr">Z_ClearLogData</code>', 'پاک‌کردن تاریخچهٔ ترددها'],
            ['<code dir="ltr">_SSR_Z_GetUserInfo ...</code>', 'توابط سری SSR (SSR_GetGeneralLogData و…)'],
        ],
        caption='جدول ۱۰-۱: توابع نمونهٔ داخل zkemkeeper.dll و zkemsdk.dll (استخراج با اسکن نمادها)', small=True
    ))

    h.append(teacher(
        'DLL های بومی ۳۲بیتی مثل این‌ها باید قبل از استفاده در ویندوز «ثبت» شوند: دستور <code dir="ltr">regsvr32 zkemkeeper.dll</code> '
        'آن‌ها را در رجیستری معرفی می‌کند تا بقیهٔ برنامه‌ها بتوانند از COM آن‌ها شیء بسازند. دو نکتهٔ عملی: (۱) چون ۳۲بیتی‌اند، '
        'برنامهٔ میزبان هم باید با پلتفرم x86 کامپایل شده باشد وگرنه خطای می‌گیرد؛ (۲) تابع نهایی‌شان معمولاً اسم مدل دستگاه را '
        'لاگ می‌کند — راه‌انداز رسمی همراه با نصب‌کنندهٔ SDK خود ZKTeco روی سیستم باید کپی شود. '
        'برای کار مستقیم با این SDK در پروژهٔ خودتان، بعد از افزودن ارجاع COM به zkemkeeper، ویژوال استودیو Interop آن را می‌سازد.',
        'نصب و استفاده از COM بومی'
    ))
    return ''.join(h)


def build_aux():
    h = []
    h.append(chapter('یازدهم', 'CH-11', 'کتابخانه‌های جانبی؛ اعلان و انیمیشن', 'Tulpep.NotificationWindow + WinFormAnimation'))

    h.append(para(
        'دو کتابخانهٔ متن‌باز این بسته، از مخزن عمومی NuGet آمده‌اند و کارشان «زیباسازی» تجربهٔ کاربری است — هرچند نقش‌شان در '
        'این سیستم پررنگ است، چون <b>همهٔ اعلان‌های نرم‌افزار</b> (از «تردد جدید دریافت شد» گرفته تا خطاهای ذخیره‌سازی) با این دو نمایش داده می‌شوند.'
    ))

    h.append(section('۱-۱۱) Tulpep.NotificationWindow؛ اعلان شیشه‌ای ویندوز'))

    h.append(para(
        'این کتابخانه (۲۸ کیلوبایت) کامپوننتی به نام <code dir="ltr">PopupNotifier</code> ارائه می‌دهد: پنجرهٔ اعلانی گوشهٔ پایین '
        'صفحه با عنوان، متن، آیکن و دکمهٔ بستن — همان چیزی که همهٔ ما به‌عنوان «نوتیفیکیشن ویندوز» می‌شناسیم. ساختارش آموزشی است: '
        'یک Component با ده‌ها پراپرتی قابل تنظیم (رنگ سربرگ، رنگ متن، فونت، مدت نمایش، سرعت انیمیشن...) و یک فرم داخلی '
        '(PopupNotifierForm) که خودش را بدون فعال‌شدنِ پنجرهٔ اصلی نشان می‌دهد — با تابع Win32 به نام ShowWindow و پرچم '
        '<code dir="ltr">SW_SHOWNOACTIVATE = 4</code>. تایمری هم (tmrAnimation) موقعیت و شفافیت پنجره را قدم‌به‌قدم تغییر می‌دهد تا '
        'اعلان «سُر بخورد» بالا.'
    ))

    h.append(code(
'''[ToolboxBitmap(typeof(PopupNotifier), "Icon.ico")]
[DefaultEvent("Click")]
public class PopupNotifier : Component
{
    private const int SW_SHOWNOACTIVATE = 4;
    private const int HWND_TOPMOST = -1;
    private const uint SWP_NOACTIVATE = 16u;

    private PopupNotifierForm frmPopup;
    private Timer tmrAnimation;
    private Timer tmrWait;
    private bool isAppearing = true;

    [Description("Color of the window header.")]
    [Category("Header")]
    [DefaultValue(typeof(Color), "ControlDark")]
    public Color HeaderColor { get; set; }
    ...
}''',
        title='Tulpep.NotificationWindow/PopupNotifier.cs — اعلان شیشه‌ای با تایمر انیمیشن',
        note='استفادهٔ ویژگی‌های Description/Category/DefaultValue یعنی این کامپوننت برای «جعبه‌ابزار ویژوال استودیو» طراحی شده — در Designer می‌افتد و با موس تنظیم می‌شود.'))

    h.append(section('۲-۱۱) WinFormAnimation؛ انیمیشن سبک‌وزن فرم‌ها'))

    h.append(para(
        'کتابخانهٔ WinFormAnimation (۳۸ کیلوبایت) موتور حرکت است: کلاس‌های <code dir="ltr">Animator</code> (یک‌بعدی)، '
        '<code dir="ltr">Animator2D</code> (دو‌بعدی) و <code dir="ltr">Animator3D</code> به‌همراه <code dir="ltr">Path</code> ها که '
        'مسیر حرکت (مثلاً از نقطهٔ الف تا ب با شتاب اولیه) را توصیف می‌کنند. در این سیستم، جفتِ این دو کتابخانه اعلان‌ها را '
        '«سُر می‌دهد»: PopupNotifier پنجره را می‌سازد و انیمیشنِ بالا آمدن/محو شدن را با همین کتابخانه انجام می‌دهد. تابع‌های '
        'انیمیشن آمادهٔ آن (KnownAnimationFunctions) شامل شتاب‌های خطی، درجه‌دو، Sine و EaseOut است.'
    ))

    h.append(para(
        'در کد دکامپایل‌شده، نقطهٔ اتصال این دو کتابخانه به سیستم اصلی، کلاس‌های <code dir="ltr">ClsNotif</code> و '
        '<code dir="ltr">MyPopupForm</code> در فضای‌نام TimeAttendanceDAL.Utility هستند که همهٔ فرم‌ها از طریق '
        '<code dir="ltr">Utility.MessageBoxShowByNotif(...)</code> با آن صدا می‌زنند — یعنی UI حتی برای نمایش یک پیام ساده هم '
        'به DAL وابسته است! (نکتهٔ معماری: بهتر بود این لایهٔ کمکی جدا بود.)'
    ))

    h.append(teacher(
        'چرا از MessageBox معمولی ویندوز استفاده نکردند؟ چون MessageBox «بلاک‌کننده» است: تا دکمهٔ OK را نزنید نمی‌توانید با برنامه '
        'کار کنید. در سیستم حضور و غیاب که هر چند ثانیه ممکن است تردد جدید برسد، انبوه پیام‌های بلاک‌کننده یعنی قفل‌شدن کار کاربر. '
        'اعلان‌های شیشه‌ای خودمحو (بعد از چند ثانیه محو می‌شوند) راه‌حل درستِ این مسئله‌اند.',
        'چرا اعلان شیشه‌ای به‌جای MessageBox؟'
    ))
    return ''.join(h)


def build_system():
    h = []
    h.append(chapter('دوازدهم', 'CH-12', 'کتابخانه‌های System.* مایکروسافت؛ زیرساخت', 'Microsoft BCL Libraries (26 files)'))

    h.append(para(
        'بیست‌وشش فایل با پیشوند System. در این بسته، هیچ‌کدام اختصاصی نیستند؛ این‌ها کتابخانه‌های استاندارد مایکروسافت‌اند که '
        'توسعه‌دهنده کنار برنامه کپی کرده تا روی سیستم‌‌های مقصد نصب جداگانه لازم نباشد (روش معروف XCOPY Deploy). '
        'در جدول زیر مهم‌ترین‌شان را با نقششان در «این سیستم خاص» مرور می‌کنیم — چون دیدن این مجموعه خودش درس آموزشی خوبی است: '
        'می‌بینید یک نرم‌افزار ویندوزی واقعی برای کار کردن به کدام قطعات استاندارد نیاز دارد.'
    ))

    h.append(table(
        ['کتابخانه', 'نقش عمومی', 'چرا این سیستم به آن نیاز دارد'],
        [
            ['<code>System.Data.SqlClient</code>', 'کلاینت SQL Server (ADO.NET)', 'همهٔ کارهای بانک اطلاعاتی DAL'],
            ['<code>System.Text.Json</code> + <code>Encodings.Web</code> + <code>Encoding.CodePages</code>', 'سریالایز JSON و صفحه‌کدها', 'تنظیمات JSON، پروتکل Hysoon، خروجی ابری؛ CodePages برای صفحه‌کد فارسی 1256'],
            ['<code>System.Memory</code> + <code>Numaics.Vectors</code> + <code>Runtime.CompilerServices.Unsafe</code> + <code>ValueTuple</code> + <code>Tasks.Extensions</code>', 'پیش‌نیازهای عملکردی جدید در دات‌نت قدیمی', 'Span، تاپل، async در NET Framework 4.8'],
            ['<code>System.IO.Ports</code>', 'پورت سریال (سریال‌پورت)', 'کتابخانهٔ Tele برای مودم صوتی'],
            ['<code>System.Drawing.Common</code>', 'گرافیک GDI+', 'رندر فرم‌ها، تصاویر پرسنل، اسکرین‌شات'],
            ['<code>System.IdentityModel.Tokens.Jwt</code>', 'توکن‌های JWT', 'احراز هویت با سرور ابری نوین پرداز'],
            ['<code>System.Collections.Immutable</code>', 'کالکشن‌های تغییرناپذیر', 'لیست‌های امن در چندتردی'],
            ['<code>System.Threading.Channels</code> + <code>Tasks.Dataflow</code>', 'صف‌های تولیدکننده/مصرف‌کننده', 'صف اعلان‌ها و BufferBlock ترددهای زنده'],
            ['<code>System.IO.Pipelines</code> + <code>Net.WebSockets.WebSocketProtocol</code>', 'I/O و وب‌سوکت مدرن', 'زیرساخت SignalR (ارتباط ابری)'],
            ['<code>System.Configuration.ConfigurationManager</code>', 'خواندن فایل‌های config', 'همان فایل‌های .config کنار DLL ها'],
            ['<code>System.Security.*</code> (۴ فایل)', 'کریپتوگرافی و ACL ویندوز', 'رمزنگاری AES در DAL و دسترسی فایل'],
            ['<code>System.CodeDom</code> + <code>ComponentModel.Annotations</code> + <code>Diagnostics.DiagnosticSource</code>', 'تولید کد، دیتا-انوتیشن، ردیابی', 'فرمول‌های حقوق، اعتبارسنجی، لاگ ردیابی'],
        ],
        caption='جدول ۱۲-۱: مرور کتابخانه‌های مایکروسافتی بسته (خلاصه‌شده بر اساس نقش)', small=True
    ))

    h.append(info(
        'نسخهٔ فایل‌ها از 2018 تا 2024 متغیر است (مثلاً System.Text.Json از سپتامبر 2024 ولی System.Memory از 2022) — نشانهٔ این '
        'که طی سال‌ها با آپدیت بسته‌های NuGet، DLL های تازه‌تر جایگزین شده‌اند بدون اینکه کل پروژه بازنویسی شود. این «زندگی طولانی» '
        'پروژه‌های WinForms صنعتی است: از 2013 (سال کپی‌رایت در کدها) تا امروز!',
        'فسیل‌شناسی نسخه‌ها'
    ))
    return ''.join(h)


def build_conclusion():
    h = []
    h.append(chapter('سیزدهم', 'CH-13', 'جمع‌بندی؛ درس‌ها و یافته‌های امنیتی', 'Lessons & Security Findings'))

    h.append(section('۱-۱۳) آنچه یاد گرفتیم'))

    h.append(para(
        'در این سفر، یک نرم‌افزار صنعتی کامل را کالبدشکافی کردیم: دیدیم چگونه یک معماری چندلایهٔ WinForms + SQL Server با ده‌ها '
        'فرم، شش برند سخت‌افزار، سینک ابری SignalR، پیامک خودکار و حتی مودم تلفنی، در قالب ۱۳ کتابخانهٔ اختصاصی سازمان‌دهی شده است. '
        'الگوهای مهمی که دیدیم می‌تواند چراغ راه پروژه‌های شما باشد:'
    ))

    h.append(bullets([
        '<b>تفکیک مسئولیت:</b> UI هیچ SQL ای اجرا نمی‌کند؛ همه‌چیز از DAL با روال‌های ذخیره‌شده می‌گذرد — دقیقاً همان‌طور که کتاب‌های معماری توصیه می‌کنند.',
        '<b>قرارداد دادهٔ مشترک:</b> با کلاس DeviceLogOnPerson، هفت مسیر سخت‌افزاری متفاوت به یک شکل در می‌آیند.',
        '<b>تحمل خطا:</b> try/catch سراسری + لاگ WebErrorLog + تلاش مجدد بازگشتی؛ نرم‌افزار صنعتی باید «نمرده» بماند.',
        '<b>کش هوشمند:</b> کش ۶۰ ثانیه‌ای پرسنل و Singleton بر اساس contextGuid.',
        '<b>رمزنگاری با کلید مناسب:</b> پیاده‌سازی AES درست بود اما کلیدش داخل کد بود — قفل فولادی روی در پارچه‌ای.',
        '<b>خودکارسازی نصب:</b> از ساخت بانک و اجرای اسکریپت‌های شِما تا پرکردن تقویم شمسی و جدول مالیات، همه خودکار.',
    ]))

    h.append(section('۲-۱۳) کارنامهٔ امنیتی'))

    h.append(table(
        ['یافته', 'محل', 'شدت', 'توضیح'],
        [
            ['رمزهای استاد ورود', 'FrmEnter.cs (UI)', pill('بحرانی', 'err'),
             'دو رمز ثابت که با هر کاربری ورود می‌دهند'],
            ['کلید رمزنگاری هاردکد', 'UserPermission.cs (DAL)', pill('بالا', 'err'),
             'عبارت "NovinP****************" برای AES'],
            ['تزریق SQL', 'Shifts.cs و چند متد دیگر', pill('متوسط', 'warn'),
             'چسباندن رشته به کوئری به‌جای پارامتر'],
            ['توکن ربات تلگرام', 'Utility.cs (DAL)', pill('متوسط', 'warn'),
             'توکن کامل + ارسال خودکار مشخصات نصب'],
            ['کاربر مدیر بدون رمز', 'UserAttendance.cs', pill('متوسط', 'warn'),
             'در اولین اجرا کاربری «مدیر» با رمز خالی ساخته می‌شود'],
            ['فرم دسترسی ویژه بدون رمز', 'FrmManagment_Login.cs', pill('بالا', 'err'),
             'اجرای مستقیم روال‌های SQL بدون احراز هویت'],
            ['اتصال SMS روی HTTP', 'SmsSender.cs', pill('کم', 'warn'),
             'ارسال نام کاربری/رمز بدون رمزگذاری انتقالی (http نه https)'],
        ],
        caption='جدول ۱۳-۱: یافته‌های امنیتی گزارش‌شده از کد دکامپایل‌شده', small=True
    ))

    h.append(para(
        'این یافته‌ها را «برای سوءاستفاده» نبینید؛ برای <b>یادگیری</b> هستند. اگر روزی خودتان سیستمی با این مقیاس ساختید، '
        'همین هفت مورد چک‌لیست بازبینی امنیتی شماست: رمز استاد نگذارید، کلید را در ماژول امن نگه دارید، همیشه کوئری پارامتری بنویسید، '
        'توکن‌ها را بیرون از کد نگه دارید، کاربر پیش‌فرض با رمز قوی یا بدون رمز نسازید، فرم‌های مدیریتی را احراز هویت کنید و ترافیک حساس را روی TLS ببرید.'
    ))

    h.append(section('۳-۱۳) اگر بخواهیم ادامه دهیم...'))

    h.append(bullets([
        'می‌توانید فایل‌های .pdb موجود در بسته را در ILSpy یا dnSpy کنار DLL ها باز کنید تا شماره خطوط سورس اصلی را هم ببینید.',
        'قسمت‌های محاسباتی TaradodFullDet (۴,۰۰۰+ خط) ارزش خواندن تک‌تک را دارد: جفت‌کردن ورود/خروج، شیفت شب، جمعه‌کاری، مرخصی ساعتی...',
        'برای آزمایش امنیتی خودتان: از بستهٔ SQL Server و بانک AttendanceN یک نمونهٔ آزمایشی بسازید و اسکریپت‌های Create_* را اجرا کنید تا شِما را از نزدیک ببینید.',
        'اگر پروژه‌ای مشابه دارید، مطالعهٔ الگوی «کلاس‌های کمکی کوچک» (مثل SmsCalendar) برای شروع عالی است؛ سپس سراغ کلاس‌های غول‌پیکر (Fish با ۸۵KB کد) بروید تا ببینید چطور «نباید» بزرگ شد!',
    ]))

    h.append(section('۴-۱۳) چک‌لیست معلم؛ برای پروژهٔ بعدی شما'))

    h.append(para(
        'اگر خلاصهٔ این گزارش را در ده بند بخواهیم — چک‌لیستی که می‌توانید هنگام طراحی یا بازبینی هر نرم‌افزار سازمانی '
        'کنار دستتان بگذارید:'
    ))

    h.append(bullets([
        'لایه‌ها را جدا نگه دارید: UI فقط نمایش، DAL فقط داده — و بین‌شان قرارداد مشخص (متدهای async با contextGuid).',
        'برای هر جدول، چهار روال استاندارد بسازید (Insert/Update/SelectRow/SelectAll) و قاعدهٔ نام‌گذاری را هرگز نشکنید.',
        'روی داده‌های پرخواننده (مثل فهرست پرسنل) کش با انقضای زمانی بگذارید و آن را زودتر از موعد باطل نکنید.',
        'هر عملیات حساس (ذخیره، حذف، تغییر دسترسی) را با لاگ حسابرسی همراه کنید: چه‌کسی، چه‌زمانی، چه چیزی.',
        'کلید و رمز را از کد جدا کنید؛ هاردکدکردن passphrase یعنی دادن کلید به هر خوانندهٔ دیکامپایلر.',
        'کوئری همیشه پارامتری — حتی برای یک عدد ساده؛ تزریق SQL از همین «موارد استثنای کوچک» وارد می‌شود.',
        'برای ارتباط زنده با سخت‌افزار، از رویداد/Push استفاده کنید و در نبود آن، پولینگ با فاصلهٔ معقول بگذارید.',
        'پیام‌های کاربر را در لایهٔ مناسب و به زبان کاربر تولید کنید؛ فارسی‌نویسی تمیز نیمی از تجربهٔ کاربری است.',
        'خطا را ببلعید اما گم نکنید: try/catch سراسری + لاگ ساخت‌یافته (Serilog) + تلاش مجدد با مکث تصادفی.',
        'پیش از انتشار، یک بار خودتان را جای مهاجم بگذارید و همان هفت سطر جدول ۱۳-۱ را نقض ناپذیر کنید.',
    ]))

    h.append(para(
        'امیدواریم این گزارش، چراغی روشن در مسیر یادگیری معماری نرم‌افزارهای واقعی بوده باشد. کد واقعی همیشه پر از درس‌های '
        'نهان است — چه در نکات درستش و چه در اشتباه‌هایش. موفق باشید!'
    ))
    return ''.join(h)
