using System;

namespace PacketParser.Entities;

public enum PacketType
{
	[PersianNameAttribute.PersianName("نامشخص")]
	none = 0,
	[PersianNameAttribute.PersianName("خطا")]
	ErrorHandler = 1,
	[PersianNameAttribute.PersianName("لاگین")]
	Login = 2,
	[PersianNameAttribute.PersianName("کالا")]
	Product = 3,
	[PersianNameAttribute.PersianName("اشخاص")]
	Customer = 4,
	[PersianNameAttribute.PersianName("فاکتور")]
	PishFactor = 5,
	[PersianNameAttribute.PersianName("دریافت")]
	Reception = 6,
	[PersianNameAttribute.PersianName("پرداخت")]
	Pardakht = 7,
	[PersianNameAttribute.PersianName("ErrorHandleSum")]
	ErrorHandleSum = 8,
	[PersianNameAttribute.PersianName("فایل")]
	File = 9,
	[PersianNameAttribute.PersianName("گروه کالا")]
	ProductGroup = 10,
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup = 11,
	[PersianNameAttribute.PersianName("عنوان اشخاص")]
	CustomerTitle = 12,
	[PersianNameAttribute.PersianName("حساب ها")]
	HesabTree = 13,
	[PersianNameAttribute.PersianName("انبار")]
	Depot = 14,
	[PersianNameAttribute.PersianName("جی پی اس")]
	GPS = 15,
	[PersianNameAttribute.PersianName("موجودی کالا")]
	PrdAnbarRelation = 16,
	[PersianNameAttribute.PersianName("تنظیمات")]
	ApplicationSetting = 17,
	[PersianNameAttribute.PersianName("واحد کالا")]
	ProductVahed = 18,
	[PersianNameAttribute.PersianName("سینک")]
	Sync = 19,
	[PersianNameAttribute.PersianName("تماس با ما")]
	ContactUs = 20,
	[PersianNameAttribute.PersianName("اخبار")]
	News = 21,
	[PersianNameAttribute.PersianName("گردش تفصیل")]
	GardeshTafsili = 22,
	[PersianNameAttribute.PersianName("هایپر لینک")]
	HyperLink = 23,
	[PersianNameAttribute.PersianName("ناتیفیکیشن")]
	Notification = 24,
	[PersianNameAttribute.PersianName("Send2Friend")]
	Send2Friend = 25,
	[PersianNameAttribute.PersianName("کاربر حسابداری")]
	UserAcc = 26,
	[PersianNameAttribute.PersianName("گردش حساب")]
	GardeshHesab = 27,
	[PersianNameAttribute.PersianName("دلیوری")]
	Delivery = 28,
	[PersianNameAttribute.PersianName("پیام تلگرامی")]
	TelegramMessage = 29,
	[PersianNameAttribute.PersianName("Script")]
	Script = 30,
	[PersianNameAttribute.PersianName("Packet")]
	Packet = 31,
	[PersianNameAttribute.PersianName("بارکد")]
	Barcode = 32,
	[PersianNameAttribute.PersianName("آدرس مشتری")]
	CustomerAddress = 33,
	[PersianNameAttribute.PersianName("ExecuteQuery")]
	ExecuteQuery = 34,
	[PersianNameAttribute.PersianName("تیکت")]
	Ticket = 35,
	[PersianNameAttribute.PersianName("اقلام تیکت")]
	TicketDet = 36,
	[PersianNameAttribute.PersianName("TicketPart")]
	TicketPart = 37,
	[PersianNameAttribute.PersianName("TicketStatus")]
	TicketStatus = 38,
	[PersianNameAttribute.PersianName("شرح آماده")]
	Desc = 39,
	[PersianNameAttribute.PersianName("AttendanceBank")]
	AttendanceBank = 40,
	[PersianNameAttribute.PersianName("AttendanceBankHesabType")]
	AttendanceBankHesabType = 41,
	[PersianNameAttribute.PersianName("AttendanceBaseYearsSalary")]
	AttendanceBaseYearsSalary = 42,
	[PersianNameAttribute.PersianName("AttendanceGharardad")]
	AttendanceGharardad = 43,
	[PersianNameAttribute.PersianName("AttendanceGroupSanavat")]
	AttendanceGroupSanavat = 44,
	[PersianNameAttribute.PersianName("AttendancePerEisar")]
	AttendancePerEisar = 45,
	[PersianNameAttribute.PersianName("AttendancePerKhedmat")]
	AttendancePerKhedmat = 46,
	[PersianNameAttribute.PersianName("AttendancePerKhedmatPlace")]
	AttendancePerKhedmatPlace = 47,
	[PersianNameAttribute.PersianName("AttendancePerPart")]
	AttendancePerPart = 48,
	[PersianNameAttribute.PersianName("AttendancePerSalaryType")]
	AttendancePerSalaryType = 49,
	[PersianNameAttribute.PersianName("AttendancePerTypeEstekhdam")]
	AttendancePerTypeEstekhdam = 50,
	[PersianNameAttribute.PersianName("AttendancePerWorkTitle")]
	AttendancePerWorkTitle = 51,
	[PersianNameAttribute.PersianName("AttendancePerson")]
	AttendancePerson = 52,
	[PersianNameAttribute.PersianName("AttendancePersonGroup")]
	AttendancePersonGroup = 53,
	[PersianNameAttribute.PersianName("AttendancePersonTitle")]
	AttendancePersonTitle = 54,
	[PersianNameAttribute.PersianName("AttendanceAttSanad")]
	AttendanceAttSanad = 55,
	[PersianNameAttribute.PersianName("AttendanceShiftCodeing")]
	AttendanceShiftCodeing = 56,
	[PersianNameAttribute.PersianName("AttendanceShiftDet")]
	AttendanceShiftDet = 57,
	[PersianNameAttribute.PersianName("AttendanceShiftProgrammableDet")]
	AttendanceShiftProgrammableDet = 58,
	[PersianNameAttribute.PersianName("AttendanceShiftMain")]
	AttendanceShiftMain = 59,
	[PersianNameAttribute.PersianName("AttendanceCompany")]
	AttendanceCompany = 60,
	[PersianNameAttribute.PersianName("AttendanceHoliday")]
	AttendanceHoliday = 61,
	[PersianNameAttribute.PersianName("AttendanceRptGen")]
	AttendanceRptGen = 62,
	[PersianNameAttribute.PersianName("AttendanceRptGenDet")]
	AttendanceRptGenDet = 63,
	[PersianNameAttribute.PersianName("AttendanceAgreedDeduction")]
	AttendanceAgreedDeduction = 64,
	[PersianNameAttribute.PersianName("AttendanceFish")]
	AttendanceFish = 65,
	[PersianNameAttribute.PersianName("AttBenefitsDeductions")]
	AttBenefitsDeductions = 66,
	[PersianNameAttribute.PersianName("AttendanceTaxTable")]
	AttendanceTaxTable = 67,
	[PersianNameAttribute.PersianName("AttendanceAttTaradodDet")]
	AttendanceAttTaradodDet = 68,
	[PersianNameAttribute.PersianName("AttendanceLogOnPersonel")]
	AttendanceLogOnPersonel = 69,
	[PersianNameAttribute.PersianName("AttendanceMorakhasidaily")]
	AttendanceMorakhasidaily = 70,
	[PersianNameAttribute.PersianName("AttendanceMorakhasiCodeing")]
	AttendanceMorakhasiCodeing = 71,
	[PersianNameAttribute.PersianName("AttendanceLogUsers")]
	AttendanceLogUsers = 72,
	[PersianNameAttribute.PersianName("AttendanceUserAttendance")]
	AttendanceUserAttendance = 73,
	[PersianNameAttribute.PersianName("ویزیت")]
	Visit = 80,
	[PersianNameAttribute.PersianName("اقساط اشخاص")]
	AghsatAshkhas = 81,
	[PersianNameAttribute.PersianName("اقساط شخصی")]
	AghsatShakhsi = 82,
	[PersianNameAttribute.PersianName("مودم")]
	Modem = 83,
	[PersianNameAttribute.PersianName("چک اشخاص")]
	ViewCheckM = 85,
	[PersianNameAttribute.PersianName("چک شخصی")]
	ViewCheckSh = 86,
	[PersianNameAttribute.PersianName("سطوح تفصیل")]
	TafsilGroup = 87,
	[PersianNameAttribute.PersianName("تماس ها")]
	CallingLog = 88,
	[PersianNameAttribute.PersianName("آخرین قیمت فاکتور")]
	LastPrice = 89,
	[PersianNameAttribute.PersianName("AttendanceLogOnAdmin")]
	AttendanceLogOnAdmin = 90,
	[PersianNameAttribute.PersianName("AttendanceLogOnAlarm")]
	AttendanceLogOnAlarm = 91,
	[PersianNameAttribute.PersianName("حساب کل")]
	Kol = 92,
	[PersianNameAttribute.PersianName("حساب معین")]
	Moein = 93,
	[PersianNameAttribute.PersianName("حساب تفصیل")]
	Tafsil = 94,
	[Obsolete]
	[PersianNameAttribute.PersianName("WebProductAttribute")]
	WebProductAttribute = 95,
	[PersianNameAttribute.PersianName("لاگ کاربر")]
	UserLog = 96,
	[PersianNameAttribute.PersianName("انبارگردانی")]
	PrdAnbarGardani = 97,
	[PersianNameAttribute.PersianName("سند")]
	Sanad = 98,
	[PersianNameAttribute.PersianName("انتقال کالا بین انبارها")]
	DepEnteghal = 99,
	[PersianNameAttribute.PersianName("قفل های مشتری")]
	LkInfo = 100,
	[PersianNameAttribute.PersianName("عملیات اسناد دریافتنی")]
	CheckMOperation = 101,
	[PersianNameAttribute.PersianName("عملیات اسناد پرداختنی")]
	CheckShOperation = 102,
	[PersianNameAttribute.PersianName("سریال کالاها")]
	PrdSerial = 103,
	[PersianNameAttribute.PersianName("نحوه آشنایی")]
	IntroductionMethod = 104,
	[PersianNameAttribute.PersianName("وضعیت اسناد")]
	SanadStatusReport = 108,
	[PersianNameAttribute.PersianName("SanadPrint")]
	SanadPrint = 109,
	[PersianNameAttribute.PersianName("EntitySync")]
	EntitySync = 110,
	[PersianNameAttribute.PersianName("VisitorArea")]
	VisitorArea = 111,
	[PersianNameAttribute.PersianName("Account")]
	Account = 112,
	[PersianNameAttribute.PersianName("نقش اشخاص")]
	CustomerRole = 113,
	[PersianNameAttribute.PersianName("یادداشت")]
	Note = 114,
	[PersianNameAttribute.PersianName("لاگین سامانه مودیان")]
	TaxLogin = 115,
	[PersianNameAttribute.PersianName("ارسال صورت حساب به سامانه مودیان")]
	TaxSendOrder = 116,
	[PersianNameAttribute.PersianName("پیگیری صورت حساب از سامانه مودیان")]
	TaxTrackingOrder = 117,
	[PersianNameAttribute.PersianName("مرخصی پرسنل")]
	AttPersonLeave = 118,
	[PersianNameAttribute.PersianName("کوپن تخفیف")]
	DiscountCoupon = 119,
	[PersianNameAttribute.PersianName("طلا و ارز")]
	TalaVaArz = 120,
	[PersianNameAttribute.PersianName("دپارتمان های حضور")]
	AttDepartment = 121,
	[PersianNameAttribute.PersianName("مالیات کالا")]
	Tax = 122,
	[PersianNameAttribute.PersianName("گزارش سامانه مودیان")]
	TaxOrderLog = 123,
	[PersianNameAttribute.PersianName("استعلام کد اقتصادی از سامانه مودیان")]
	TaxEconomicCodeState = 124,
	[PersianNameAttribute.PersianName("گروه اقساط")]
	AghsatMGroup = 125,
	[PersianNameAttribute.PersianName("سطح قیمت کالا")]
	ProductPriceLevel = 126,
	[PersianNameAttribute.PersianName("ارتباط با دستگاه")]
	AttendanceDeviceLogOnPersonel = 127,
	[PersianNameAttribute.PersianName("بیومتریک")]
	BioMetric = 999
}
