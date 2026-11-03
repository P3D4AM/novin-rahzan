namespace PacketParser.Entities;

public enum ENSource : short
{
	[TelegramNameAttribute.TelegramName("#None")]
	[PersianNameAttribute.PersianName("همه موارد")]
	None = 0,
	[TelegramNameAttribute.TelegramName("\ud83d\udcb0\ud83d\udcb0#Accounting\ud83d\udcb0\ud83d\udcb0")]
	[PersianNameAttribute.PersianName("حسابداری")]
	Accounting = 1,
	[TelegramNameAttribute.TelegramName("#Android")]
	[PersianNameAttribute.PersianName("اندروید")]
	Android = 2,
	[TelegramNameAttribute.TelegramName("\ud83c\udfea\ud83c\udfe6#Baje\ud83c\udfea\ud83c\udfe6")]
	[PersianNameAttribute.PersianName("حسابداری شعب")]
	Baje = 3,
	[TelegramNameAttribute.TelegramName("#ErrorHandlerProgrammerApp")]
	[PersianNameAttribute.PersianName("مدیریت خطاها")]
	ErrorHandlerProgrammerApp = 4,
	[TelegramNameAttribute.TelegramName("\ud83d\udd70\ud83d\udd70#TimeAttendance\ud83d\udd70\ud83d\udd70")]
	[PersianNameAttribute.PersianName("حضور و غیاب")]
	TimeAttendance = 5,
	[TelegramNameAttribute.TelegramName("\ud83c\udfd3\ud83c\udfd3#WebNP\ud83c\udfd3\ud83c\udfd3")]
	[PersianNameAttribute.PersianName("ارسال داده ها به هاست")]
	WebNP = 6,
	[TelegramNameAttribute.TelegramName("⛑⛑#Department⛑⛑")]
	[PersianNameAttribute.PersianName("مدیریت دپارتمان")]
	Department = 7,
	[TelegramNameAttribute.TelegramName("✍\ufe0f✍\ufe0f#PayamakService✍\ufe0f✍\ufe0f")]
	[PersianNameAttribute.PersianName("سرویس ویندوزی پیامک")]
	PayamakService = 8,
	[TelegramNameAttribute.TelegramName("#PayamakGSM")]
	[PersianNameAttribute.PersianName("پیامک سیم کارتی")]
	PayamakGSM = 9,
	[TelegramNameAttribute.TelegramName("#FarakhaneSoti")]
	[PersianNameAttribute.PersianName("فراخوان صوتی")]
	FarakhaneSoti = 10,
	[TelegramNameAttribute.TelegramName("#ZeroFishNO")]
	[PersianNameAttribute.PersianName("صفر کردن شماره فیش")]
	ZeroFishNO = 11,
	[TelegramNameAttribute.TelegramName("⚖\ufe0f⚖\ufe0f#ScaleDal⚖\ufe0f⚖\ufe0f")]
	[PersianNameAttribute.PersianName("ترازو")]
	ScaleDal = 12,
	[TelegramNameAttribute.TelegramName("#ChekPrint")]
	[PersianNameAttribute.PersianName("چک پرینت")]
	ChekPrint = 13,
	[TelegramNameAttribute.TelegramName("#Other")]
	[PersianNameAttribute.PersianName("متفرقه")]
	Other = 14,
	[TelegramNameAttribute.TelegramName("#ServerTCP")]
	[PersianNameAttribute.PersianName("سرور TCP")]
	ServerTCP = 15,
	[TelegramNameAttribute.TelegramName("#WebScrapper")]
	[PersianNameAttribute.PersianName("اسکرپر")]
	WebScrapper = 16,
	[TelegramNameAttribute.TelegramName("\ud83c\udd71\ufe0f\ud83c\udd71\ufe0f#NPBackupService\ud83c\udd71\ufe0f\ud83c\udd71\ufe0f")]
	[PersianNameAttribute.PersianName("پشتیبانگیری اینترنتی")]
	NPBackupService = 17,
	[TelegramNameAttribute.TelegramName("#ModemCallerId")]
	[PersianNameAttribute.PersianName("مودم کالر آی دی")]
	ModemCallerId = 18,
	[TelegramNameAttribute.TelegramName("\ud83d\udc7b\ud83d\udc7b#WebSite\ud83d\udc7b\ud83d\udc7b")]
	[PersianNameAttribute.PersianName("وبسایت")]
	WebSite = 19,
	[TelegramNameAttribute.TelegramName("\ud83d\udcf2\ud83d\udcf2#Android_HesabYar\ud83d\udcf2\ud83d\udcf2")]
	[PersianNameAttribute.PersianName("حساب یار اندروید")]
	Android_HesabYar = 20,
	[TelegramNameAttribute.TelegramName("\ud83d\udcf1\ud83d\udc68\u200d#Android_SefareshGir\ud83d\udc68\ud83d\udcf1")]
	[PersianNameAttribute.PersianName("سفارشگیر اندروید")]
	Android_SefareshGir = 21,
	[TelegramNameAttribute.TelegramName("#Android_BMS")]
	[PersianNameAttribute.PersianName("BMS")]
	Android_BMS = 22,
	[TelegramNameAttribute.TelegramName("✉\ufe0f\ud83d\udce9#PayamakUI✉\ufe0f\ud83d\udce9")]
	[PersianNameAttribute.PersianName("پیامک")]
	PayamakUI = 23,
	[TelegramNameAttribute.TelegramName("#IOS_HesabYar")]
	[PersianNameAttribute.PersianName("حساب یار ios")]
	IOS_HesabYar = 24,
	[TelegramNameAttribute.TelegramName("#GPSPortal\ud83d\udccd\ud83d\udccc\ud83d\udce1")]
	[PersianNameAttribute.PersianName("پورتال GPS")]
	GpsPortal = 25,
	[TelegramNameAttribute.TelegramName("\ud83d\ude0d\ud83d\ude0d#InternetUpdater\ud83d\ude0d\ud83d\ude0d")]
	[PersianNameAttribute.PersianName("بروزرسانی اینترنتی")]
	InternetUpdater = 26,
	[TelegramNameAttribute.TelegramName("\ud83c\udf10\ud83c\udf10#WebService\ud83c\udf10\ud83c\udf10")]
	[PersianNameAttribute.PersianName("وب سرویس")]
	WebService = 27,
	[TelegramNameAttribute.TelegramName("#Programmer")]
	[PersianNameAttribute.PersianName("برنامه نویس")]
	Programmer = 28,
	[TelegramNameAttribute.TelegramName("#PrintLayout")]
	[PersianNameAttribute.PersianName("قالب پرینت اندروید")]
	PrintLayout = 29,
	[TelegramNameAttribute.TelegramName("#WebService_oldOnNovinP")]
	[PersianNameAttribute.PersianName("وب سرویس قدیمی واقع در novinP.ir/WebService")]
	WebService_oldOnNovinP = 30,
	[TelegramNameAttribute.TelegramName("#LKService")]
	[PersianNameAttribute.PersianName("سرویس مدیریت قفل")]
	LKService = 31,
	[TelegramNameAttribute.TelegramName("#AttendanceMobile")]
	[PersianNameAttribute.PersianName("حضور و غیاب موبایل")]
	AttendanceMobile = 32,
	[TelegramNameAttribute.TelegramName("\ud83d\udc68\u200d\ud83d\udcbb\ud83d\udc68\u200d\ud83d\udcbb#AttendanceWebsite\ud83d\udc68\u200d\ud83d\udcbb\ud83d\udc68\u200d\ud83d\udcbb")]
	[PersianNameAttribute.PersianName("حضور و غیاب وب سایت")]
	AttendanceWebsite = 33,
	[TelegramNameAttribute.TelegramName("\ud83e\udd2f\ud83e\udd2f  #AIService  \ud83e\udd2f\ud83e\udd2f")]
	[PersianNameAttribute.PersianName("سرویس هوش مصنوعی")]
	AIService = 34,
	[TelegramNameAttribute.TelegramName("#Advertise")]
	[PersianNameAttribute.PersianName("ثبت آگهی")]
	Advertise = 35,
	[TelegramNameAttribute.TelegramName("#WebsiteWordPress")]
	[PersianNameAttribute.PersianName("سایت وردپرس")]
	WebsiteWordPress = 36,
	[TelegramNameAttribute.TelegramName("#WordPressDebug")]
	[PersianNameAttribute.PersianName("خطایابی وردپرس")]
	WordPressDebug = 37,
	[TelegramNameAttribute.TelegramName("☎\ufe0f☎\ufe0f#CallerIdWindowsService☎\ufe0f☎\ufe0f")]
	[PersianNameAttribute.PersianName("CallerIdWindowsService")]
	CallerIdWindowsService = 38,
	[TelegramNameAttribute.TelegramName("\ud83d\udce9\ud83d\udce9#NpsmsWebSiteApi\ud83d\udce9\ud83d\udce9")]
	[PersianNameAttribute.PersianName("NpsmsWebSiteApi")]
	NpsmsWebSiteApi = 39,
	[TelegramNameAttribute.TelegramName("#PictureUploader")]
	[PersianNameAttribute.PersianName("PictureUploader")]
	PictureUploader = 41,
	[TelegramNameAttribute.TelegramName("#SendMessageTelegram")]
	[PersianNameAttribute.PersianName("ارسال پیام در تلگرام")]
	SendMessageTelegram = 42,
	[TelegramNameAttribute.TelegramName("#WebSiteCacheCore")]
	[PersianNameAttribute.PersianName("وب سایت کور کش api")]
	WebSiteCacheCore = 43,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#WebServiceCore\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("وب سرویس کور")]
	WebServiceCore = 44,
	[TelegramNameAttribute.TelegramName("\ud83c\udf8a\ud83c\udf8a#CrmPortal\ud83c\udf8a\ud83c\udf8a")]
	[PersianNameAttribute.PersianName("پورتال CRM")]
	CrmPortal = 45,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#RestApi\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("سرویس های وبی")]
	RestApi = 46,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#CustomerDisplay\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("نمایشگر مبلغ")]
	CustomerDisplay = 47,
	[TelegramNameAttribute.TelegramName("#میکروسرویس گزارش خطا")]
	[PersianNameAttribute.PersianName("میکروسرویس گزارش خطا")]
	MicroserviceErrorReporting = 48,
	Microservice_ErrorReporting = 49,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#Activation\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("اکتیویشین")]
	Activation = 50,
	[TelegramNameAttribute.TelegramName("#CallCenterClient")]
	[PersianNameAttribute.PersianName("سرویس ارتباط با مرکز تماس")]
	CallCenterClient = 51,
	[TelegramNameAttribute.TelegramName("#Hotel")]
	[PersianNameAttribute.PersianName("هتلداری")]
	Hotel = 52,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#IssableSever\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("ایزابل سرور")]
	IssableSever = 53,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#IssableUiTest\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("ایزابل تست UI")]
	IssableUiTest = 54,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#TaxService\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("سامانه مودیان")]
	TaxService = 55,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#TaxService\ud83c\udf0d\ud83c\udf0d")]
	[PersianNameAttribute.PersianName("پورتال حسابداری")]
	AccountingPortal = 56,
	[TelegramNameAttribute.TelegramName("\ud83c\udf0d\ud83c\udf0d#BioMetric")]
	[PersianNameAttribute.PersianName("بیومتریک")]
	BioMetric = 57,
	[TelegramNameAttribute.TelegramName("#MauiHesabyar")]
	[PersianNameAttribute.PersianName("حسابیار میو میو")]
	MuiaHesabyar = 58
}
