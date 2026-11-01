# -*- coding: utf-8 -*-
"""Report content — Part 2a: TimeAttendanceDAL (chapter 3)."""

from report_lib import (code, teacher, warn, info, chapter, section,
                        subsection, table, para, bullets, pill)


def build_dal():
    h = []
    h.append(chapter('سوم', 'CH-03', 'کتابخانهٔ TimeAttendanceDAL؛ قلب تپندهٔ سیستم', 'TimeAttendanceDAL.dll — Data Access Layer'))

    h.append(para(
        'این کتابخانه با حجم <b>۶٫۳ مگابایت</b>، <b>۱۹۳ فایل سورس</b> و بیش از ۳۰ فضای‌نام (Namespace)، بزرگ‌ترین و مهم‌ترین '
        'قطعهٔ نرم‌افزار است. هر عملی که ذخیره‌سازی، خواندن، محاسبه یا ارتباط با دنیای خارج لازم دارد، یا مستقیماً اینجا انجام می‌شود '
        'یا از اینجا فراخوانی می‌شود: از ثبت ترددها و مدیریت پرسنل و قراردادها تا فیش حقوقی، بیمه، مالیات، پیامک‌های خودکار و '
        'همگام‌سازی با سرور ابری شرکت. در کدهای این کتابخانه حتی مشخصات سخت‌افزاری «قفل نرم‌افزار» (لایسنس USB) هم بررسی می‌شود.'
    ))

    h.append(table(
        ['فضای‌نام', 'مسئولیت', 'کلاس‌های شاخص'],
        [
            ['<code dir="ltr">TimeAttendanceDAL</code>', 'مدیریت رشتهٔ اتصال', 'ClsConnection'],
            ['<code dir="ltr">...Database</code>', 'ساخت و بازسازی بانک', 'CLSDatabase، RunScript، Bazsazi'],
            ['<code dir="ltr">...Dal.FLRPerson</code>', 'مدیریت پرسنل', 'Person + ۹ کلاس جدول‌های کمکی'],
            ['<code dir="ltr">...Dal.Taradod</code>', 'ترددها، مرخصی‌ها و محاسبات', 'LogOnPersonel، TaradodFullDet'],
            ['<code dir="ltr">...Dal.FLRGharardad</code>', 'قراردادهای کاری', 'GharardadNew، FormulaClass'],
            ['<code dir="ltr">...Dal.Salary</code>', 'حقوق و دستمزد', 'Fish (۸۵ کیلوبایت کد!)، TaxTable'],
            ['<code dir="ltr">...Dal.FLRShift</code>', 'شیفت‌ها', 'Shifts، ShiftDet'],
            ['<code dir="ltr">...Dal.FLRSanad</code>', 'اسناد حسابداری', 'AttSanad، ATT_SanadDet'],
            ['<code dir="ltr">...Dal.Device</code>', 'دستگاه‌ها و کدهای سخت‌افزاری', 'DeviceInfo، HardwareCodeCus'],
            ['<code dir="ltr">...Dal.User</code>', 'کاربران و سطوح دسترسی', 'UserAttendance، UserPermission'],
            ['<code dir="ltr">...Settings</code>', 'تنظیمات متمرکز (JSON در بانک)', 'TimeAttendanceSettings، Payamak'],
            ['<code dir="ltr">...Utility</code>', 'ابزارهای عمومی', 'Utility، FMessageBox، AttLogger'],
            ['<code dir="ltr">...Mapper</code>', 'تبدیل موجودیت‌ها به مدل ابری', 'PersonMapper و ۱۶ مپر دیگر'],
        ],
        caption='جدول ۳-۱: نقشهٔ فضاهای‌نام اصلی DAL (FLR = Filter/List/Report)', small=True
    ))

    # ---------------------------------------------------------------
    h.append(section('۱-۳) اتصال به پایگاه‌داده؛ سبک کلاسیک ADO.NET'))

    h.append(para(
        'DAL با پایگاه‌داده <b>SQL Server</b> و از طریق کلاس‌های ADO.NET کار می‌کند: <code dir="ltr">SqlConnection</code> برای اتصال، '
        '<code dir="ltr">SqlCommand</code> برای فرمان و <code dir="ltr">SqlDataReader</code> برای خواندن نتیجه. '
        'نکتهٔ معماری این است که <b>هیچ اتصال بلندمدتی نگه داشته نمی‌شود</b>؛ هر متد، اتصال خودش را می‌سازد، کارش را انجام می‌دهد و '
        'در بلوک <code dir="ltr">finally</code> آن را آزاد می‌کند. مدیریت اتصال‌های باز در قالب «استخر اتصال» (Connection Pooling) '
        'خود ADO.NET به‌صورت خودکار انجام می‌شود.'
    ))

    h.append(para(
        'رشتهٔ اتصال در کلاس <code dir="ltr">ClsConnection</code> نگهداری می‌شود. متد جالب آن <code dir="ltr">CheckConnectionString</code> است '
        'که یک اتصال آزمایشی می‌سازد تا از درستی رشتهٔ اتصال مطمئن شود:'
    ))

    h.append(code(
'''public static bool CheckConnectionString(string constr)
{
    try
    {
        SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(constr)
        {
            AsynchronousProcessing = true
        };
        SqlConnection val2 = new SqlConnection();
        ((DbConnection)val2).ConnectionString =
            ((DbConnectionStringBuilder)(object)val).ConnectionString;
        AttLogger.LoggerUi.Information("checkConnectionStrindg() ... ");
        ((DbConnection)val2).Open();
        AttLogger.LoggerUi.Information("checkConnectionStrindg() is ok");
        ((DbConnection)val2).Close();
        connectionString = constr;
        return true;
    }
    catch (Exception ex)
    {
        WebErrorLog.ErrorLogInstance.StartLog(ex.Message, "connstr=" + constr,
            "CheckConnectionString", "...\\ClsConnection.cs", 45);
        return false;
    }
}''',
        title='TimeAttendanceDAL/ClsConnection.cs — اعتبارسنجی رشتهٔ اتصال',
        note='توجه کنید که استثناها (Exception) هرگز باعث کرش برنامه نمی‌شوند؛ همه‌جا با try/catch گرفته شده و در WebErrorLog ثبت می‌شوند — رویه‌ای که در سراسر این نرم‌افزار تکرار می‌شود.'))

    h.append(para(
        'اما چگونه خود بانک ساخته می‌شود؟ متد <code dir="ltr">CreateDbIfNotExist</code> مثل کارگر نصب عمل می‌کند: از نام '
        '<code dir="ltr">Attendance0</code> شروع می‌کند و تا <code dir="ltr">Attendance99</code> جلو می‌رود تا اولین نام آزاد را بیابد:'
    ))

    h.append(code(
'''public static async Task<ReturnedSaveFuncInfo> CreateDbIfNotExist(string contextGuid)
{
    ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
    if (Extensions.GetSqlConnectionString(contextGuid) != "")
    {
        return ret;
    }
    string Database = "Attendance";
    int i = 0;
    try
    {
        for (; i < 100; i++)
        {
            string connectionString = (await BaseDataBaseAction.Instance
                .CreateDataBaseAsync(BackupSettingsRepo.GetInstance(contextGuid),
                                     Database + i)).ConnectionString;
            if (!string.IsNullOrEmpty(connectionString) &&
                ClsConnection.CheckConnectionString(connectionString))
            {
                ClsCache cache = ClsCache.GetCache(contextGuid);
                if (cache != null)
                {
                    cache.Dispose();
                }
                CacheContext context = new CacheContext($"CreateDataBaseGuid{Guid.NewGuid()}");
                await EntityCacheConfig.InitAsync(context.ContextGuid, connectionString,
                    SystemInfoClass.SystemInfo(), VersionManagment.Version,
                    (EnConnectionType)1);
                ClsConnection.ContextGuid = context.ContextGuid;
                return ret;
            }
        }
    }
    catch (Exception ex)
    {
        ret.AddReturnedValue((ReturnedState)2, "ایجاد بانک اطلاعاتی با خطا مواجه شد", ...);
        WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateDbIfNotExist", ...);
    }
    ret.AddReturnedValue((ReturnedState)2, "بانک اطلاعاتی ایجاد نشد", ...);
    return ret;
}''',
        title='TimeAttendanceDAL.Database/CLSDatabase.cs — ساخت خودکار بانک با نام Attendance0 تا 99',
        note='پیام‌های خطا فارسی هستند چون مستقیم به کاربر نهایی نمایش داده می‌شوند. ساختار ReturnedSaveFuncInfo حامل وضعیت (موفق/خطا/هشدار) + پیام فارسی است.'))

    h.append(para(
        'پس از ساخت بانک خالی، کلاس <code dir="ltr">RunScript</code> کل شِمای پایگاه‌داده را از فایل‌های ریسورس داخل خود DLL اجرا می‌کند '
        'و به ترتیب این‌ها را می‌سازد: توابع SQL، جدول‌ها، تریگرها، ویوها، روال‌های ذخیره‌شده و حتی '
        '<b>جدول تقویم (CreateTaghvim)</b> که داده‌های تقویم شمسی — از جمله تعطیلات رسمی ایران — را پر می‌کند. '
        'جالب است بدانید این متد حتی نسخهٔ SQL Server را هم چک می‌کند: اگر سرور قدیمی‌تر از 2012 باشد، خطای نبودِ تابع '
        '<code dir="ltr">FORMAT()</code> را می‌بیند و پیام «Sql server version must be higher than 2012» نمایش می‌دهد. '
        'کلاس <code dir="ltr">Bazsazi</code> («بازسازی») هم همین زنجیره را برای ارتقا/مهاجرت نسخه‌های قدیمی اجرا می‌کند و '
        'در پایان حتی یک پینگ به آدرس 8.8.8.8 می‌زند تا اتصال اینترنت را بسنجد!'
    ))

    h.append(teacher(
        '<b>روال ذخیره‌شده (Stored Procedure)</b> یک برنامهٔ کوچک داخل خود SQL Server است که با دستور CREATE PROCEDURE ساخته می‌شود. '
        'مزیتش سه‌تاست: ۱) سرعت بالاتر چون از قبل کامپایل شده، ۲) کاهش رفت‌وبرگشت شبکه، '
        '۳) امنیت بیشتر در برابر تزریق SQL اگر پارامترها را درست پاس بدهیم. این نرم‌افزار تقریباً ۹۵٪ کارهایش را با SP انجام می‌دهد و '
        'قاعدهٔ نام‌گذاری‌اش کاملاً یکنواخت است: <code dir="ltr">sp_نام‌جدول_عملیات</code> مثلاً '
        '<code dir="ltr">sp_Person_Insert</code>، <code dir="ltr">sp_Person_Update</code>، <code dir="ltr">sp_Person_SelectAll</code>.',
        'روال ذخیره‌شده (SP) چیست؟'
    ))

    # ---------------------------------------------------------------
    h.append(section('۲-۳) کالبدشکافی یک کلاس DAL؛ الگوی Active Record'))

    h.append(para(
        'تقریباً همهٔ کلاس‌های این کتابخانه یک الگوی واحد دارند که در ادبیات مهندسی نرم‌افزار به آن <b>Active Record</b> می‌گویند: '
        '«هر شیء = یک ردیف از جدول». یعنی یک نمونه از کلاس Person یک کارمند واقعی است؛ اگر <code dir="ltr">SaveAsync()</code> '
        'را صدا بزنید و <code dir="ltr">Id</code> صفر باشد <b>درج</b> می‌کند و اگر Id مقدار داشته باشد <b>به‌روزرسانی</b>. '
        'متدهای استاتیک هم نقش «فهرست‌ها و جستجوها» را دارند. بیایید این منطق را در کد واقعی ببینیم:'
    ))

    h.append(code(
'''await ((DbConnection)(object)cn).OpenAsync(token);
if (((PersonBase)this).Id <= 0)
{
    // جدید است → روال درج را صدا بزن
    ((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Person_Insert]";
    object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
    if (obj != null)
    {
        ((PersonBase)this).Id = Convert.ToInt32(obj.ToString());
    }
    await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid,
        "[افزودن اطلاعات یک شخص جدید] : " + ((PersonBase)this).FullName +
        " [با کد تفصیلی  ] : " + ((PersonBase)this).CodeTafsil,
        "Personnel.PersonnelInsert", this, ((PersonBase)this).Id.ToString());
}
else
{
    // موجود است → روال ویرایش را صدا بزن
    ((DbCommand)(object)cmd).CommandText = "[dbo].[sp_Person_Update]";
    cmd.Parameters.AddWithValue("ID", (object)((PersonBase)this).Id);
    await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
    await LogUsers.RegisterNewLogAsync(EntityCacheConfig.CacheInstance.ContextGuid,
        "[ویرایش اطلاعات پرسنلی] : " + ((PersonBase)this).FullName, ...);
}
((DbConnection)(object)cn).Close();
Reset();
return true;''',
        title='TimeAttendanceDAL.Dal.FLRPerson/Person.cs — قلب متد SaveAsync',
        note='بعد از هر تغییر، یک رکورد «حسابرسی» (Audit) هم در جدول LogUsers ثبت می‌شود؛ یعنی چه‌کسی، چه زمانی، چه چیزی را تغییر داده — قابلیتی که برای نرم‌افزارهای منابع انسانی تقریباً الزامی است.'))

    h.append(para(
        'نکتهٔ ظریف این کلاس، وجود یک <b>کش ۶۰ ثانیه‌ای</b> روی فهرست کل اشخاص است. چون تقریباً همهٔ فرم‌ها به لیست پرسنل نیاز دارند، '
        'دوباره‌خوانی آن از SQL Server در هر فرم، پرهزینه است؛ پس نتیجهٔ آخرین کوئری در یک متغیر استاتیک نگه داشته می‌شود و فقط '
        'اگر ۶۰ ثانیه از آخرین بارگذاری گذشته باشد، دوباره از بانک خوانده می‌شود:'
    ))

    h.append(code(
'''public static async Task<List<Person>> GetAllAsync(string contextGuid,
    List<int> listPersonId, bool personStatus, CancellationToken token)
{
    try
    {
        if (LastUpdateTime.AddSeconds(60.0) < DateTime.Now)
        {
            LastUpdateTime = DateTime.Now;
            List<Person> ret = new List<Person>();
            SqlConnection cn = new SqlConnection(
                Extensions.GetSqlConnectionString(contextGuid));
            SqlCommand cmd = new SqlCommand("[sp_Person_SelectAll]", cn);
            ((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("personIDs", (object)"");
            await ((DbConnection)(object)cn).OpenAsync(token);
            SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
            while (await ((DbDataReader)(object)dr).ReadAsync(token))
            {
                token.ThrowIfCancellationRequested();
                ret.Add(LoadData(contextGuid, dr));
            }
            ((DbDataReader)(object)dr).Close();
            Persons = ret;      // کش استاتیک به‌روز شد
        }
        List<Person> list = Persons;
        if (personStatus)
        {
            list = Persons.Where((Person p) =>
                p != null && ((PersonBase)p).St).ToList();
        }
        return list;
    }
    ...
}''',
        title='Person.cs — کش استاتیک ۶۰ ثانیه‌ای جلوی sp_Person_SelectAll'))

    h.append(teacher(
        'متد <code dir="ltr">LoadData(SqlDataReader)</code> که در کد بالا می‌بینید «هیدراتور» (Hydrator) نام دارد: '
        'یک ردیف خوانده‌شده از بانک را می‌گیرد و فیلدهایش را یکی‌یکی در پراپرتی‌های شیء Person می‌ریزد — '
        'مثل خالی‌کردن آب در قالب. الگوی <code dir="ltr">try/finally</code> با <code dir="ltr">Dispose</code> هم تضمین می‌کند '
        'منابع (اتصال، فرمان، خواننده) حتی هنگام خطا آزاد شوند.',
        'LoadData چه می‌کند؟'
    ))

    # ---------------------------------------------------------------
    h.append(section('۳-۳) موجودیت تردد؛ LogOnPersonel'))

    h.append(para(
        'کلاس <code dir="ltr">LogOnPersonel</code> (موجودیت جدول LogonPersonel) پرترافیک‌ترین کلاس سیستم است چون هر کارت‌زدنی '
        'یک ردیف از آن است. ذخیره‌سازی آن دو ویژگی مهم دارد: نخست از یک <b>سمافور (SemaphoreSlim)</b> استفاده می‌شود تا در لحظهٔ '
        'ذخیره‌های موازی، دو تردد هم‌زمان تداخل نکنند؛ دوم پس از ذخیره، شبکهٔ آخرین ترددهای پایش زنده در UI هم به‌روزرسانی می‌شود:'
    ))

    h.append(code(
'''cmd.Parameters.AddWithValue("DateTimeLogM", (object)DateTimeLogM);
cmd.Parameters.AddWithValue("IDTraffic", (object)TrafficId);
cmd.Parameters.AddWithValue("IsVirtual", (object)(short)(int)IsVirtual);
cmd.Parameters.AddWithValue("TrafficType", (object)(short)(int)TypeTaradod);
cmd.Parameters.AddWithValue("TrafficName",
    (Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod) + " / " +
     Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradodVacationOrMission)));
await ((DbConnection)(object)cn).OpenAsync(token);
if (Id <= 0)
{
    cmd.Parameters.AddWithValue("PersonId", (object)PersonId);
    cmd.Parameters.AddWithValue("DeviceId", (object)DeviceId_);
    cmd.Parameters.AddWithValue("DeviceSerial", (object)DeviceSerial_);
    ((DbCommand)(object)cmd).CommandText = "sp_LogonPersonel_Insert";
    SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
    if (await ((DbDataReader)(object)dr).ReadAsync(token) &&
        ((DbDataReader)(object)dr)["id"] != DBNull.Value)
    {
        Id = (int)((DbDataReader)(object)dr)["id"];
    }
}
else
{
    ((DbCommand)(object)cmd).CommandText = "sp_LogonPersonel_Update";
    cmd.Parameters.AddWithValue("Id", (object)Id);
    await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
}
((DbConnection)(object)cn).Close();
TimeAttendanceDAL.Utility.Utility.ReloadDgridLastTaradodsAsync(ContextGuid, token);
return Id;''',
        title='LogOnPersonel.cs — ذخیرهٔ یک تردد با روال ذخیره‌شده (قسمتی از متد SaveAsync)',
        note='خط آخر یعنی: بعد از هر ذخیره، گرید «آخرین ترددها» روی فرم اصلی دوباره پر شود — اتصال بی‌واسطهٔ DAL به UI از طریق یک متد استاتیک.'))
    h.append(para(
        'اعتبارسنجی این کلاس هم کاملاً «ایرانی» است؛ متد <code dir="ltr">Validatetion</code> پیام‌های فارسی تولید می‌کند: '
        'ترددی با زمانِ آینده پذیرفته نمی‌شود، ترددِ متعلق به دستگاه ناشناس رد می‌شود، کد شخص باید شناخته‌شده باشد و '
        'ترددهای قدیمی‌تر از ۹۰ روز هم باید به‌صورت دستی تأیید شوند.'
    ))

    # ---------------------------------------------------------------
    h.append(section('۴-۳) کوئری‌ها؛ پارامتری در برابر الحاقی'))

    h.append(para(
        'همان‌طور که گفتیم اکثر کوئری‌ها از طریق SP و <b>پارامتری</b> اجرا می‌شوند (روش امن). اما جاهایی کدنویس برای کوئری‌های '
        'کوچک، رشته‌ها را مستقیم به هم چسبانده است. یک نمونهٔ سالم و یک نمونهٔ خطرناک:'
    ))

    h.append(code(
'''// نمونهٔ سالم: پارامتری — مقدار خارج از متن کوئری منتقل می‌شود
cmd.Parameters.AddWithValue("@CodeMoein", (object)codeM);
cmd.Parameters.AddWithValue("@CodeTafsil", (object)codeT);
((DbCommand)(object)cmd).CommandText =
    "Select Count(*) from Person Where CodeMoein=@CodeMoein " +
    "and CodeTafsil=@CodeTafsil and ID<>@ID and St='True'";''',
        title='Person.cs — کوئری پارامتری (روش درست)'))

    h.append(code(
'''// نمونهٔ ناامن: الحاق رشته — مستعد تزریق SQL
SqlCommand cmd = new SqlCommand(
    "select Id from [shifts] where [St]='true' and Code='" + code + "'", cn);''',
        title='Shifts.cs — کوئری الحاقی (روش خطرناک)'))

    h.append(warn(
        'در کلاس Shifts متد <code dir="ltr">UniqeCodeAsync</code> مقدار پارامتر <code dir="ltr">code</code> را مستقیم داخل متن SQL '
        'می‌چسباند. اگر مقدار ورودی از کاربر تأمین شود، مهاجم می‌تواند با تزریق عبارتی مثل '
        '<code dir="ltr">\' OR \'1\'=\'1</code> منطق کوئری را دور بزند. نمونه‌های مشابه در '
        '<code dir="ltr">Person.LoadEnrollDataAsync</code> (الحاق Id عددی — کم‌خطر)، <code dir="ltr">FishDet</code> و '
        '<code dir="ltr">ShiftProgrammableDet</code> هم دیده می‌شود. درس آموزشی: <b>همیشه از پارامتر استفاده کنید، حتی برای اعداد.</b>',
        'تزریق SQL (SQL Injection)'
    ))

    # ---------------------------------------------------------------
    h.append(section('۵-۳) رمزنگاری؛ AES با یک کلیدِ داخل کد'))

    h.append(para(
        'سطوح دسترسی و رمز عبور کاربران نرم‌افزار، قبل از ذخیره در بانک با الگوریتم <b>AES/Rijndael</b> رمز می‌شوند. '
        'پیاده‌سازی فنی آن کاملاً حرفه‌ای است: استخراج کلید با PBKDF2 (تابع Rfc2898DeriveBytes با ۱۰۰۰ تکرار)، '
        'مقدار نم (Salt) و بردار آغازین (IV) تصادفی و الحاق آن‌ها به ابتدای خروجی:'
    ))

    h.append(code(
'''public static string Encrypt(string plainText, string passPhrase)
{
    byte[] array = Generate256BitsOfRandomEntropy();   // Salt تصادفی
    byte[] array2 = Generate256BitsOfRandomEntropy();  // IV تصادفی
    byte[] bytes = Encoding.UTF8.GetBytes(plainText);
    using Rfc2898DeriveBytes rfc2898DeriveBytes =
        new Rfc2898DeriveBytes(passPhrase, array, 1000);
    byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);   // کلید ۲۵۶ بیتی
    using RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.BlockSize = 256;
    rijndaelManaged.Mode = CipherMode.CBC;
    rijndaelManaged.Padding = PaddingMode.PKCS7;
    using ICryptoTransform transform =
        rijndaelManaged.CreateEncryptor(bytes2, array2);
    using MemoryStream memoryStream = new MemoryStream();
    using CryptoStream cryptoStream =
        new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
    cryptoStream.Write(bytes, 0, bytes.Length);
    cryptoStream.FlushFinalBlock();
    byte[] inArray = array.Concat(array2).ToArray()
        .Concat(memoryStream.ToArray()).ToArray();
    return Convert.ToBase64String(inArray);
}''',
        title='TimeAttendanceDAL.Classes/Encryption.cs — رمزنگاری AES-256 با Salt تصادفی'))

    h.append(warn(
        'مشکل کجاست؟ در <b>عبور رمز</b>! کلاس UserPermission این متد را با یک عبارت رمز ثابت و هاردکد صدا می‌زند: '
        '<code dir="ltr">private readonly string _passPhrase = "NovinP****************";</code> '
        '— یعنی هرکسی که به همین DLL یا سورس دکامپایل‌شدهٔ آن دسترسی داشته باشد (دقیقاً مثل ما!) می‌تواند محتوای رمزشدهٔ '
        'سطوح دسترسی را باز کند. رمزنگاری قوی با کلید ضعیف، مثل قفل فولادیِ روی درِ پارچه‌ای است. '
        'راه درست: نگهداری کلید در ماژول امن (HSM/TPM) یا حداقل استفاده از DPAPI ویندوز.',
        'کلید هاردکدشده در کد!'
    ))

    # ---------------------------------------------------------------
    h.append(section('۶-۳) پل ارتباطی با ابر؛ SignalR'))

    h.append(para(
        'یکی از جذاب‌ترین بخش‌های DAL، لایهٔ همگام‌سازی ابری است. نرم‌افزار با هاب <b>SignalR</b> سرور نوین پرداز '
        '(آدرس پیش‌فرض: <code dir="ltr">https://novinrank.ir/hubCore</code>) وارد می‌شود و شناسه‌های خود را معرفی می‌کند: '
        'نام کاربری NPACC، شمارهٔ سریال قفل سخت‌افزاری LK، شناسهٔ پردازنده و GUID بانک. بعد از این «ورود»، ترددهایی که در '
        'پنل تحت‌وب ثبت می‌شوند، به‌صورت زنده به مشتری محلی هدایت می‌شوند:'
    ))

    h.append(code(
'''List<ClientConfig> list = new List<ClientConfig>
{
    new ClientConfig
    {
        UserName = usernameSr,
        Password = passwordSr,
        Source = (ENSource)5,
        DbGuid = dbGuid,
        Version = version,
        CompressedMode = (ENCompressedMode)1,
        LkSerial = lkSerial,
        CpuSerial = systemInfo,
        Url = url
    }
};
_cacheInstance.Context.LoginExtention.OnLoginResponse -= Instance_OnLoginResponse;
_cacheInstance.Context.LoginExtention.OnLoginResponse += Instance_OnLoginResponse;
...
ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> val =
    await cacheInstance.ConnectAsync(connectionType, list, token2, true);''',
        title='TimeAttendanceDAL.Settings/EntityCacheConfig.cs — پیکربندی ورود به هاب ابری'))

    h.append(code(
'''private static async Task WebAttendanceLogOnPerson_OnSync(object sender,
    ConnectionEventArgs<WebAttendanceLogOnPerson> e)
{
    try
    {
        Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)9,
            "شروع عملیات سینک", "", 3);
        ReturnedSaveFuncInfoWithValue<(int, List<string>)> val =
            await SaveTaradod(stringBuilder,
                e.Connection.Context.Guid, e.ConnectionData,
                e.Connection.cancellationToken);
        if (val.value.Item1 > 0)
        {
            Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)7,
                string.Format("اتمام عملیات سینک\\r\\n [تعداد ترددهای دریافت شده]: {0}",
                    val.value.Item1), "[اتمام عملیات سینک]", 10);
        }
        ...
    }
}''',
        title='EntityCacheConfig.cs — دریافت دسته‌ای ترددها از سرور ابری',
        note='همچنین یک سرویس ویندوزی به نام NpCommunicationDeviceService (با هاب BioMetricServerCore) روی پورت 5001 نصب می‌شود که رلهٔ ارتباط دستگاه‌های زیستی با این هاب است.'))

    h.append(info(
        'DAL هر ۳۰ دقیقه یکبار اشخاصِ تغییریافتهٔ محلی را به سرور ابری می‌فرستد و تغییرات ثبت‌شده در پنل وب را برمی‌گرداند '
        '(دو جهتِ یک سینک). برای هر کارمند هم یک لینک ورود به «سلف‌سرویس» ساخته می‌شود: '
        '<code dir="ltr">https://nphozor.ir/Login/...</code> با یک هش بی‌اس۶‌۴ از نام‌کاربری + رمز + GUID شخص.',
        'سینک دورهای ۳۰ دقیقه‌ای'
    ))

    # ---------------------------------------------------------------
    h.append(section('۷-۳) الگوهای طراحیِ قابل مشاهده در DAL'))

    h.append(table(
        ['الگو', 'کجا دیده می‌شود', 'توضیح کوتاه'],
        [
            ['Active Record', 'همهٔ کلاس‌های Dal.*', 'هر شیء یک ردیف جدول با متدهای Save/Delete'],
            ['Singleton (برای هر context)', 'EnrollManage، TimeAttendanceSettings', 'یک نمونهٔ یکتا در ConcurrentDictionary با کلید contextGuid'],
            ['Repository', 'DeviceLogOnPersonRepository', 'تنها ریپازیتوری صریح؛ ذخیرهٔ تردد دریافتی از دستگاه'],
            ['Mapper / DTO', 'فضای‌نام Mapper با ۱۷ کلاس', 'تبدیل موجودیت محلی به مدل ابری WebAtt*'],
            ['Observer (رویدادها)', 'OnSaveTaradod، OnStatusErorTaradod', 'اعلان تغییرات به UI بدون وابستگی مستقیم'],
            ['Retry (تلاش مجدد)', 'SaveSmsRefrencedIdsAsync', 'تا ۲۰ بار با فاصلهٔ ۵ ثانیه، به‌صورت بازگشتی!'],
            ['Producer/Consumer', 'BufferBlock در Utility', 'صف اعلان‌ها با TPL Dataflow'],
            ['کش با انقضا', 'Person.Persons + LastUpdateTime', 'کش ۶۰ ثانیه‌ای فهرست اشخاص'],
            ['JSON Blob', 'JsonFishDet، JsonGharardadDet، تنظیمات', 'ذخیرهٔ جزئیات اسکیماپذیر به‌صورت JSON در یک ستون'],
        ],
        caption='جدول ۳-۲: الگوهای طراحی شناسایی‌شده در DAL', small=True
    ))

    h.append(code(
'''public async Task SaveSmsRefrencedIdsAsync(string smsRefrencedIds,
    CancellationToken token, short tryCount = 20)
{
    try
    {
        ...
    }
    catch (Exception ex2)
    {
        if (tryCount <= 0)
        {
            Log.Error(ex2, $"logonpersonel id = {Id}");
            throw;
        }
        await Task.Delay(5000);
        await SaveSmsRefrencedIdsAsync(smsRefrencedIds, token,
            (short)(tryCount - 1));   // تلاش مجدد بازگشتی
    }
}''',
        title='LogOnPersonel.cs — الگوی Retry بازگشتی (۲۰ بار × ۵ ثانیه)',
        note='روش بازگشتی خوانا اما برای تلاش‌های زیاد، مصرف پشته (Stack) دارد؛ نسخهٔ بهتر: حلقهٔ while یا کتابخانهٔ Polly.'))

    # ---------------------------------------------------------------
    h.append(section('۸-۳) نکات ویژهٔ «ایرانیِ» این کتابخانه'))

    h.append(bullets([
        '<b>تقویم شمسی:</b> مقدار <code dir="ltr">MinDateM = 1921/03/21</code> دقیقاً معادل <b>۱۳۰۰/۰۱/۰۱</b> شمسی است و نقش «تاریخ صفر/تهی» سیستم را دارد؛ همهٔ تاریخ‌ها به میلادی در بانک ذخیره و هنگام نمایش با کلاس Calendar به شمسی تبدیل می‌شوند.',
        '<b>مبدل صفحه‌کد داس:</b> کلاس ConvertDosPersianToWindows جدول جابه‌جایی بایت‌های فارسی «ایران‌سیستم» را به یونیکد دارد — برای تولید فایل‌های سازگار با نرم‌افزارهای حسابداری قدیمی!',
        '<b>جدول مالیات حقوق:</b> کلاس TaxTable جدول نرخ‌های مالیات حقوق ایران را از وب‌سرویس شرکت دانلود و در بانک ذخیره می‌کند.',
        '<b>قفل سخت‌افزاری (LK):</b> شناسهٔ قفل USB با متد <code dir="ltr">LK.GetSerialNumber(...)</code> خوانده می‌شود؛ نسخهٔ آزمایشی/رایگان را با پرچم‌های _isTrialVersion و _isFreeVersion از هم جدا می‌کند.',
        '<b>ربات تلگرام داخلی:</b> Utility یک متد SendTelegramMessage دارد که خطاها و حتی ترددهای زنده را برای سازنده به یک ربات تلگرام با توکن هاردکد می‌فرستد!',
        '<b>ورود به سلف‌سرویس:</b> برای هر شخص یک URL یکبارمصرف‌مانند با هش تولید می‌شود (nphozor.ir).',
    ]))

    h.append(warn(
        'متد <code dir="ltr">Utility.SendTelegramMessage</code> یک توکن کامل ربات تلگرام را داخل کد دارد: '
        '<code dir="ltr">apiKey = "7555******:AA****..."</code> و پیام‌ها شامل نسخهٔ برنامه، نام کاربر ویندوز، IP‌های محلی و سریال قفل است. '
        'از دید حریم خصوصی مشتری‌ها، ارسال خودکار مشخصات نصب به سرور توسعه‌دهنده قابل نقد است؛ و توکنِ لو رفته هم یعنی هر کسی می‌تواند '
        'به آن رلاتور پیام بفرستد. اگر روزی خودتان چنین سیستمی ساختید، تنظیم ارسال تله‌متری را شفاف و قابل غیرفعال‌سازی کنید.',
        'توکن تلگرام و تله‌متری خودکار'
    ))

    h.append(para(
        'در جمع‌بندی فصل: DAL یک لایهٔ دادهٔ کامل، خودنصب‌شونده و چندمنظوره است — از ساخت بانک و شِما تا رمزنگاری، سینک ابری، '
        'پیامک خودکار و حتی پایش کاربران. حالا که قلب سیستم را شناختیم، برویم سراغ لایه‌ای که با سخت‌افزار حرف می‌زند.'
    ))
    return ''.join(h)
