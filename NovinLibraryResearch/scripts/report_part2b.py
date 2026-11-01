# -*- coding: utf-8 -*-
"""Report content — Part 2b: TimeAttendanceDevices (chapter 4)."""

from report_lib import (code, teacher, warn, info, chapter, section,
                        subsection, table, para, bullets, pill)


def build_devices():
    h = []
    h.append(chapter('چهارم', 'CH-04', 'کتابخانهٔ TimeAttendanceDevices؛ صحبت با دستگاه‌ها', 'TimeAttendanceDevices.dll — Device Drivers'))

    h.append(para(
        'این کتابخانه (۲۹۴ کیلوبایت، ۴۸ فایل) «مترجمِ» نرم‌افزار و دستگاه‌های حضور و غیاب است. دنیای دستگاه‌های اثر انگشت '
        'شش‌دنگ است: هر سازنده‌ای زبان (پروتکل) خودش را دارد و این کتابخانه برای هر برند یک درایور جداگانه دارد. '
        'برندهای پشتیبانی‌شده: <b>ZKTeco</b>، <b>Hysoon</b> (دو نسل)، <b>Ebio/Grandin</b>، <b>Eizon/Riss</b>، <b>ZDC2911/Realand</b> '
        'و دوربین‌های نورمرئی <b>NP761</b>.'
    ))

    h.append(para(
        'نکتهٔ معماری جالب: برخلاف انتظار، این کتابخانه <b>اینترفیس مشترکی مانند IDevice ندارد!</b> هر درایور کلاسی مستقل است، '
        'اما همهٔ آن‌ها به‌طور قراردادی شکل یکسانی دارند: سازنده‌ای با (شماره دستگاه، رمز، IP، پورت)، پراپرتی '
        '<code dir="ltr">Conected</code> (با غلط املایی ثابت!) و متدی که لیست رکوردها را برمی‌گرداند. واحد‌کنندهٔ واقعی، کلاس '
        '<b>RecordAllDevice</b> است که خروجی هر برند را به موجودیت استاندارد <code dir="ltr">DeviceLogOnPerson</code> تبدیل می‌کند:'
    ))

    h.append(code(
'''[Serializable]
public class RecordAllDevice
{
    public static List<DeviceLogOnPerson> GetAllFromRiss(
        string contextGuid, List<Record> plst)
    {
        List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
        try
        {
            foreach (Record item2 in plst)
            {
                DeviceLogOnPerson item = new DeviceLogOnPerson(contextGuid)
                {
                    DeviceNo = item2.DN,                 // شمارهٔ دستگاه
                    DateTimeLogM = item2.Clock,          // زمان تردد
                    IdTraffic = (short)item2.Verify,     // نوع تأیید (اثرانگشت/کارت...)
                    IoMode = (int)item2.MDIN,            // حالت ورود/خروج
                    PersonNo = (int)item2.DIN            // شمارهٔ کاربر روی دستگاه
                };
                list.Add(item);
            }
        }
        catch (Exception ex)
        {
            WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllFromRiss", ...);
        }
        return list;
    }
}''',
        title='TimeAttendanceDevices/RecordAllDevice.cs — نرمال‌ساز رکوردهای همهٔ برندها',
        note='قرارداد ثابت در کل سیستم: PersonNo = شمارهٔ کاربر، DeviceNo = شمارهٔ دستگاه، DateTimeLogM = زمان میلادی، IdTraffic = نوع تردد.'))

    h.append(table(
        ['برند/نام‌فضایی', 'خانوادهٔ دستگاه', 'روش ارتباط', 'نحوهٔ خواندن تردد', 'Real-time'],
        [
            ['<code dir="ltr">.FLRZk</code>', 'ZKTeco', 'کلاس COM به نام CZKEMClass (zkemkeeper)', 'ReadGeneralLogData + SSR_GetGeneralLogData', 'خیر (Polling)'],
            ['<code dir="ltr">.FLRHysoon</code>', 'Hysoon نسل قدیم (FK)', 'P/Invoke به FK623Attend.dll', 'FK_LoadGeneralLogData + FK_GetGeneralLogData', 'خیر'],
            ['<code dir="ltr">.FLRHysoon (Dynamic)</code>', 'Hysoon نسل جدید', 'سوکت TCP خام + پروتکل JSON', 'فرمان GetLogData صفحه‌بندی‌شده', '<b>بله</b> (HTTP Push)'],
            ['<code dir="ltr">.FLREbio</code>', 'Ebio / Grandin', 'کنترل ActiveX با نام SBXPC', 'ReadAllGLogData + GetAllGLogData', 'خیر'],
            ['<code dir="ltr">.Eizon</code>', 'Riss (ZD/ZDC/U260)', 'SDK مدیریت‌شدهٔ Riss.Devices', 'GetProperty(DeviceProperty.13)', 'خیر'],
            ['<code dir="ltr">.FLRZDC2911</code>', 'Realand ZDC2911', 'SDK مدیریت‌شدهٔ ZDC2911.dll', 'Realand.GetALLGlog()', 'خیر'],
            ['<code dir="ltr">Listener761</code>', 'دوربین نورمرئی NP761', 'سرور HTTP داخلی', 'دریافت POST از خود دستگاه', '<b>بله</b> (Push)'],
        ],
        caption='جدول ۴-۱: هفت مسیر ارتباطی با دستگاه‌ها', small=True
    ))

    # ---------------------------------------------------------------
    h.append(section('۱-۴) ZKTeco؛ از طریق COM'))

    h.append(para(
        'معروف‌ترین سازندهٔ دستگاه‌های اثر انگشت دنیاست و SDK رسمی‌اش (zkemkeeper.dll) یک <b>کامپوننت COM</b> است. '
        'کلاس <code dir="ltr">ClsZK</code> این SDK را بسته‌بندی می‌کند:'
    ))

    h.append(code(
'''public class ClsZK
{
    private readonly int _iMachineNumber = 1;
    private int _Port = 5005;      // پورت پیش‌فرض ZKTeco
    public CZKEMClass AxCZKEM1 { get; set; } = new CZKEMClass();

    public ClsZK(string contextGuid, int DeviceNO, string CommunicationPass,
                 string IP, int TCPPort)
    {
        ContextGuid = contextGuid;
        _iMachineNumber = DeviceNO;
        _IP = IP;
        _Port = TCPPort;
        Conected = AxCZKEM1.Connect_Net(IP, TCPPort);
    }

    public List<ZkRecord> GetLog()
    {
        if (!Conected)
        {
            MessageBox.Show("Please connect the device first");
            return new List<ZkRecord>();
        }
        List<ZkRecord> list = new List<ZkRecord>();
        AxCZKEM1.EnableDevice(_iMachineNumber, false);   // قفل موقت دستگاه
        if (AxCZKEM1.ReadGeneralLogData(_iMachineNumber))
        {
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0, idwInOutMode = 0;
            int idwYear = 0, idwMonth = 0, idwDay = 0;
            int idwHour = 0, idwMinute = 0, idwSecond = 0;
            int idwWorkcode = 0;
            while (AxCZKEM1.SSR_GetGeneralLogData(_iMachineNumber,
                ref sdwEnrollNumber, ref idwVerifyMode, ref idwInOutMode,
                ref idwYear, ref idwMonth, ref idwDay, ref idwHour,
                ref idwMinute, ref idwSecond, ref idwWorkcode))
            {
                list.Add(new ZkRecord(_iMachineNumber, sdwEnrollNumber,
                    idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay,
                    idwHour, idwMinute, idwSecond, idwWorkcode));
            }
        }
        AxCZKEM1.EnableDevice(_iMachineNumber, true);    // آزادسازی دستگاه
        return list;
    }
}''',
        title='TimeAttendanceDevices.FLRZk/ClsZK.cs — اتصال و خواندن ترددهای ZKTeco',
        note='الگوی سه‌مرحله‌ای کلاسیک کار با این دستگاه‌ها: (۱) EnableDevice(false) تا کاربر موقع انتقال داده کارت نزند، (۲) خواندن داده، (۳) EnableDevice(true).'))

    h.append(teacher(
        '<b>COM</b> (Component Object Model) فناوری سال‌های ۱۹۹۰ مایکروسافت برای ساخت اجزای نرم‌افزاری قابل استفادهٔ بین‌زبانی است. '
        'کتابخانهٔ COM مثل zkemkeeper.dll باید ابتدا در ویندوز با دستور regsvr32 «ثبت» شود و بعد هر زبانی — از VB6 تا #C — '
        'می‌تواند از آن شیء بسازد. در #C وقتی به COM ارجاع می‌دهید، ویژوال استودیو یک لایهٔ واسط به نام '
        '<b>Interop</b> می‌سازد (اینجا: Interop.zkemkeeper) که فراخوانی‌های شیءگرا را به فراخوانی COM ترجمه می‌کند. '
        'محدودیت مهم: COMهای ۳۲بیتی مثل این، فقط در پروسهٔ ۳۲بیتی (x86) کار می‌کنند.',
        'COM و Interop یعنی چه؟'
    ))

    # ---------------------------------------------------------------
    h.append(section('۲-۴) Hysoon کلاسیک؛ P/Invoke به کتابخانهٔ بومی'))

    h.append(para(
        'درایور Hysoon قدیمی مستقیماً توابع کتابخانهٔ بومی <code dir="ltr">FK623Attend.dll</code> را با تکنیک <b>P/Invoke</b> '
        'صدا می‌زند و نکتهٔ هشداربرانگیزش این است که هندل اتصال (nCommHandleIndex) یک فیلد <b>استاتیک</b> است؛ یعنی هر پروسه '
        'فقط می‌تواند هم‌زمان به یک دستگاه Hysoon قدیمی وصل باشد!'
    ))

    h.append(code(
'''public clsHysoon(string contextGuid, int deviceNum, int password,
                  string ip, int port, bool getNewTaradodOnly)
{
    ContextGuid = contextGuid;
    _GetNewTaradodOnly = getNewTaradodOnly;
    _vnMachineNumber = deviceNum;
    _vpszNetPassword = password;
    _vpszIPAddress = ip;
    _vpszNetPort = port;
    _vnTimeOut = 5000;
    _vnLicense = 1261;          // شمارهٔ لایسنس ثابت SDK هیسون
    FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectNet(
        _vnMachineNumber, _vpszIPAddress, _vpszNetPort,
        _vnTimeOut, _vnProtocolType, _vpszNetPassword, _vnLicense);
}

public List<DeviceLogOnPerson> GetAllLogOnPublic()
{
    ...
    int num4 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
    num4 = FKAttendDLL.FK_LoadGeneralLogData(FKAttendDLL.nCommHandleIndex,
        _GetNewTaradodOnly ? 1 : 0);
    while (true)
    {
        num4 = FKAttendDLL.FK_GetGeneralLogData(FKAttendDLL.nCommHandleIndex,
            ref personNo, ref num2, ref ioMode, ref dateTimeLogM);
        if (num4 != 1)
        {
            break;   // پایان رکوردها
        }
        list.Add(new DeviceLogOnPerson(ContextGuid)
        {
            PersonNo = personNo,
            DeviceNo = _vnMachineNumber,
            VerifyMode = num2.ToString(),
            IoMode = ioMode,
            DateTimeLogM = dateTimeLogM
        });
    }
    Disconnect();
    ...
}''',
        title='TimeAttendanceDevices.FLRHysoon/clsHysoon.cs — اتصال و خواندن با FK623Attend.dll',
        note='P/Invoke یعنی فراخوانی مستقیم توابع DLL غیرمدیریت‌شده از کد #C؛ اینجا ۷۸ تابع بومی FK* در فایل FKAttendDLL2.cs تعریف شده‌اند.'))

    # ---------------------------------------------------------------
    h.append(section('۳-۴) Hysoon جدید (Dynamic)؛ پروتکل JSON دست‌ساز'))

    h.append(para(
        'نسل جدید دستگاه‌های Hysoon پروتکل جالبی دارند: روی TCP، یک <b>سرآیند ۳۲ بایتی</b> و بعد بدنهٔ JSON می‌فرستند. '
        'کد ساخت بستهٔ ارسالی، مشخصات کامل سرآیند را لو می‌دهد:'
    ))

    h.append(code(
'''public const int PROTOCOLKEY = 404232216;   // == 0x18181818
public byte[] Header = new byte[32];

public byte[] GetSendBuff(byte[] sendBuff)
{
    byte[] array = new byte[32 + sendBuff.Length];
    byte[] arry = new byte[4];
    if (ConvertIntToByteArray(sendBuff.Length, ref arry))
    {
        Buffer.BlockCopy(arry, 0, Header, 0, 4);   // بایت ۰-۳: طول بدنه
    }
    if (ConvertIntToByteArray(404232216, ref arry))
    {
        Buffer.BlockCopy(arry, 0, Header, 4, 4);   // بایت ۴-۷: جادوی 0x18181818
    }
    if (ConvertIntToByteArray(DevicePwd, ref arry))
    {
        Buffer.BlockCopy(arry, 0, Header, 8, 4);   // بایت ۸-۱۱: رمز دستگاه
    }
    Buffer.BlockCopy(Header, 0, array, 0, 32);     // ۲۰ بایت باقی: صفر
    Buffer.BlockCopy(sendBuff, 0, array, 32, sendBuff.Length);
    return array;
}

public static bool ConvertIntToByteArray(int m, ref byte[] arry)
{
    arry[0] = (byte)((uint)m & 0xFFu);
    arry[1] = (byte)((m & 0xFF00) >> 8);
    arry[2] = (byte)((m & 0xFF0000) >> 16);
    arry[3] = (byte)((uint)(m >> 24) & 0xFFu);   // Little-Endian
    return true;
}''',
        title='TimeAttendanceDevices.FLRHysoon/ClsDynamicHysoon.cs — ساخت بستهٔ ۳۲ بایتی + JSON',
        note='عدد جادویی 404232216 همان 0x18181818 است؛ چون هر چهار بایتش برابرند، این شناسه در هر دو حالت Big/Little-Endian یکسان می‌ماند — ترفند هوشمندانه‌ای برای مقاوم‌سازی پروتکل در برابر تفاوت ترتیب بایت!'))

    h.append(table(
        ['آفست', 'طول', 'معنی'],
        [
            ['۰ تا ۳', '۴ بایت', 'طول بدنهٔ JSON (Little-Endian)'],
            ['۴ تا ۷', '۴ بایت', 'عدد جادویی 0x18181818'],
            ['۸ تا ۱۱', '۴ بایت', 'رمز ارتباط دستگاه'],
            ['۱۲ تا ۳۱', '۲۰ بایت', 'پرکنندهٔ صفر'],
            ['۳۲ به بعد', 'n بایت', 'بدنهٔ JSON فرمان (مثلاً GetLogData)'],
        ],
        caption='جدول ۴-۲: ساختار سرآیند پروتکل Hysoon نسل جدید'
    ))

    h.append(para(
        'خواندن پاسخ دستگاه هم به این شکل است — تا وقتی بستهٔ ناقص است ادامه می‌دهیم و پایان پیام را با پیدا شدن '
        '<code dir="ltr">"}\\0"</code> حدس می‌زنیم:'
    ))

    h.append(code(
'''public void Receive(ref StringBuilder jsonStringBuilder)
{
    while (true)
    {
        if (!c_socket.Connected) break;
        byte[] array = new byte[ReceiveBufferSize];
        int num = c_socket.Receive(array);
        if (num == 0) break;
        if (num <= 1 || num == 32) continue;
        string @string = Encoding.UTF8.GetString(array, 0, num);
        jsonStringBuilder.Append(@string);
        @string = @string.Replace("\\n", "");
        int num2 = @string.LastIndexOf("}\\0");
        if (num < ReceiveBufferSize && num2 > 0)
        {
            break;   // پیام کامل رسید
        }
    }
}''',
        title='ClsDynamicHysoon.cs — دریافت پاسخ JSON و تشخیص پایان پیام'))

    h.append(para(
        'خواندن ترددها هم صفحه‌بندی‌شده است؛ فرمان <code dir="ltr">GetLogData</code> با فیلد packageId تا زمانی که دستگاه مقدار '
        'nonzero برگرداند ادامه پیدا می‌کند. و اما بخش هیجان‌انگیز: دستگاه‌های جدید Hysoon می‌توانند تردد را <b>خودشان PUSH کنند</b>. '
        'کتابخانه یک سرور HTTP داخلی بالا می‌آورد و دستگاه به آن POST می‌زند:'
    ))

    h.append(code(
'''public bool Setup(int port, List<Person> persons,
    List<HardwareCodeCus> hardwareCodes, ref string ErrorStr,
    IDeviceLogOnPersonRepository dalRepository)
{
    Persons = persons;
    HardwareCodes = hardwareCodes;
    _dalRepository = dalRepository;
    if (port == 0)
    {
        stop = true;
        httpListener.Close();
        ErrorStr = "شماره پورت نادرست است.";
        return false;
    }
    stop = false;
    httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
    string uriPrefix = $"http://*:{port}/";
    httpListener.Prefixes.Add(uriPrefix);
    httpListener.Start();
    Receive();
    ErrorStr = $"جهت دریافت تردد آنی، پورت {port} باز شد";
    return true;
}

private void EndReciver(IAsyncResult ar)
{
    if (!stop)
    {
        HttpListenerContext context = httpListener.EndGetContext(ar);
        Dispatcher(context);
        Receive();    // گوش دادن به درخواست بعدی
    }
}''',
        title='FLRHysoon.DynamicHysoonRealTime/HttpServerDynamicHysoon.cs — سرور HTTP داخلی برای تردد آنی'))

    h.append(code(
'''public async Task DispatchResources(ExecutingDispatch action, string deviceIp,
    IDeviceLogOnPersonRepository _dalRepository)
{
    try
    {
        string text = request.Headers.Get("request_code");
        string trans_id = request.Headers.Get("trans_id");
        string dev_id = request.Headers.Get("dev_id");
        string deviceModel = request.Headers.Get("dev_model");
        string text2 = new StreamReader(request.InputStream).ReadToEnd();

        if (text.Contains("realtime_glog"))
        {
            DynamicHysoonInfo.MonitoringLogs log2 =
                JsonConvert.DeserializeObject<DynamicHysoonInfo.MonitoringLogs>(text2);
            DeviceLogOnPerson log =
                RecordAllDevice.GetFromDynamicHysoonRealTime(ContextGuid, log2);
            log.DeviceSerial = dev_id;
            log.DeviceModel = deviceModel;
            ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult> val =
                await _dalRepository.SaveAsync(log);
            if (((ReturnedSaveFuncInfo)val).HasError)
            {
                this.RealTimeLogEvent?.Invoke(val.ErrorMessage);
            }
            else
            {
                this.RealTimeLogEvent?.Invoke(
                    $"تردد جدید از دستگاه {dev_id} با کد {log.PersonNo} دریافت شد");
                DeviceEventsEntity.SaveEventMessage(...);
            }
        }
        ...
    }
    catch { action(405); return; }
    action(404);
}''',
        title='RequestHelperDynamicHysoon.cs — دریافت PUSH تردد از دستگاه',
        note='قرارداد سیم: هدرهای request_code: realtime_glog، trans_id، dev_id، dev_model و بدنهٔ JSON مثل {"userId":"123","time":"202401011230","inOut":"in"}. پیام رویداد هم فارسی است و مستقیم به UI می‌رسد.'))

    # ---------------------------------------------------------------
    h.append(section('۴-۴) Ebio و Eizon؛ دو سبک دیگر'))

    h.append(para(
        'درایور <b>Ebio</b> با کنترل ActiveX به نام SBXPC کار می‌کند. چون ActiveX به یک پنجرهٔ ویندوزی نیاز دارد، ترفند جالبی '
        'بکار رفته: یک فرم مخفی (FrmEbio) ساخته می‌شود و کنترل روی آن قرار می‌گیرد! بدتر اینکه چون این کتابخانه ممکن است در '
        'سرویس ویندوز (بدون UI) هم اجرا شود، یک رشتهٔ STA با پمپ پیام دستی راه می‌اندازند:'
    ))

    h.append(code(
'''public List<DeviceLogOnPerson> GetGLogData()
{
    try
    {
        Thread thread = new Thread(STAOparation);
        thread.SetApartmentState(ApartmentState.STA);   // الزام ActiveX
        thread.Start();
        thread.Join();
        while (thread.IsAlive)
        {
            Application.DoEvents();   // پمپ پیام ویندوز، دستی!
        }
        return GLogData;
    }
    ...
}''',
        title='TimeAttendanceDevices.FLREbio/EbioClass.cs — اجرای ActiveX در ترد STA'))

    h.append(para(
        'قالب‌های اثر انگشت Ebio هم رمزآلود است: هر عدد ۳۲بیتی قالب به ۵ بایت تبدیل می‌شود — یک بایت علامت + چهار بایت قدرمطلق '
        'به ترتیب MSB. قالب چهره (اندیس ۱۷) هم ۶,۹۱۷ کلمهٔ ۳۲بیتی در برابر ۳۵۴ کلمه برای اثر انگشت است. برای نوشتن روی دستگاه، '
        'اندیس‌های انگشت ۰ تا ۹ به بازهٔ ۲۰ تا ۲۹ نگاشت می‌شوند! این جزئیات فقط از مهندسی معکوس قابل کشف بود.'
    ))

    h.append(para(
        'درایور <b>Eizon (Riss)</b> به‌جای پروتکل خام، از SDK مدیریت‌شدهٔ Riss.Devices استفاده می‌کند که همه‌چیز را با '
        'GetProperty/SetProperty و «شماره‌های جادویی» انجام می‌دهد؛ مثلاً DeviceProperty.13 یعنی «رکوردهای حضور و غیاب». '
        'تشخیص مدل دستگاه هم با جستجوی رشته در نام محصول است:'
    ))

    h.append(code(
'''private bool IsZd2910(string model)
{
    bool result = false;
    if (!string.IsNullOrEmpty(model) &&
        (model.Contains("ZDC20") || model.Contains("ZDC30") ||
         model.Contains("ZDC50") || model.Contains("ZDC60") ||
         model.Contains("ZDC80") || model.Contains("ZDC90") ||
         model.Contains("2910")))
    {
        result = true;
    }
    return result;
}''',
        title='TimeAttendanceDevices.Eizon/ClsEizon.cs — انگشت‌نگاری مدل دستگاه (۲,۸۶۹ خط!)',
        note='این درایور برای یافتن اولیّن شمارهٔ کاربری خالی، از ۱ تا ۶۵,۵۳۵ را یکی‌یکی روی دستگاه امتحان می‌کند — دامپ کامل یک دستگاه می‌تواند ساعت‌ها طول بکشد!'))

    h.append(para(
        'درایور <b>ZDC2911</b> نازک‌ترین لایه است و فقط کتابخانهٔ جداگانهٔ ZDC2911.dll (که در فصل ۱۰ می‌بینیم) را بسته‌بندی می‌کند. '
        'و در نهایت <b>Listener761</b> برای دوربین‌های نورمرئی، مثل Dynamic Hysoon یک HttpListener است که POST دستگاه را '
        'می‌گیرد و به پارسر مخصوص NP761 می‌سپارد.'
    ))

    h.append(info(
        'درون ریسورس‌های این DLL یک فایل اجرایی ویندوزی جاسازی شده (با نام AccTiny، حدود ۲۰۰ کیلوبایت، PE واقعی با امضای MZ) '
        'و همچنین وضعیت ذخیره‌شدهٔ کنترل ActiveX (bpc.OcxState). جاسازی EXE داخل DLL برای اجرای ابزار جانبی بدون نصب جداگانه است — '
        'اما از دید امنیتی، هر باینری جاسازی‌شده باید بررسی شود.',
        'یک EXE داخل DLL!'
    ))
    return ''.join(h)
