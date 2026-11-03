namespace PacketParser.Entities;

public enum EntityPart : short
{
	None = 0,
	[PersianNameAttribute.PersianName("کالا")]
	Product = 1,
	[PersianNameAttribute.PersianName("گروه کالا")]
	ProductGroup = 2,
	[PersianNameAttribute.PersianName("واحد کالا")]
	ProductVahed = 3,
	[PersianNameAttribute.PersianName("اشخاص")]
	Customer = 4,
	[PersianNameAttribute.PersianName("گروه اشخاص")]
	CustomerGroup = 5,
	[PersianNameAttribute.PersianName("عنوان اشخاص")]
	CustomerTitle = 6,
	[PersianNameAttribute.PersianName("انبار")]
	Depot = 7,
	ProductAnbarRelation = 8,
	HesabTree = 9,
	User = 10,
	GardeshTafsili = 11,
	Settings = 12,
	ContactUs = 14,
	HyperLink = 15,
	News = 16,
	FileInformations = 17,
	[PersianNameAttribute.PersianName("دریافت")]
	Reception = 18,
	[PersianNameAttribute.PersianName("پرداخت")]
	Pardakht = 19,
	[PersianNameAttribute.PersianName("فاکتور")]
	Order = 20,
	Delivery = 21,
	Barcode = 26,
	Gps = 28,
	Ticket = 35,
	TicketDet = 36,
	TicketPart = 37,
	TicketStatus = 38,
	Desc = 39,
	AttendanceBank = 40,
	AttendanceBankHesabType = 41,
	AttendanceBaseYearsSalary = 42,
	AttendanceGharardad = 43,
	AttendanceGroupSanavat = 44,
	AttendancePerEisar = 45,
	AttendancePerKhedmat = 46,
	AttendancePerKhedmatPlace = 47,
	AttendancePerPart = 48,
	AttendancePerSalaryType = 49,
	AttendancePerTypeEstekhdam = 50,
	AttendancePerWorkTitle = 51,
	AttendancePerson = 52,
	AttendancePersonGroup = 53,
	AttendancePersonTitle = 54,
	AttendanceAttSanad = 55,
	AttendanceShiftCodeing = 56,
	AttendanceShiftDet = 57,
	AttendanceShiftProgrammableDet = 58,
	AttendanceShiftMain = 59,
	AttendanceCompany = 60,
	AttendanceHoliday = 61,
	AttendanceRptGen = 62,
	AttendanceRptGenDet = 63,
	AttendanceAgreedDeduction = 64,
	AttendanceFish = 65,
	AttBenefitsDeductions = 66,
	AttendanceTaxTable = 67,
	AttendanceAttTaradodDet = 68,
	AttendanceLogOnPersonel = 69,
	AttendanceMorakhasidaily = 70,
	AttendanceMorakhasiCodeing = 71,
	AttendanceLogUsers = 72,
	AttendanceUserAttendance = 73,
	Visit = 80,
	AghsatAshkhas = 81,
	AghsatShakhsi = 82,
	ViewCheckSh = 83,
	ViewCheckM = 84,
	[PersianNameAttribute.PersianName("گروه تفصیلی")]
	TafsilGroup = 85,
	CallingLog = 86,
	LastPrice = 87,
	[PersianNameAttribute.PersianName("حساب تفصیلی")]
	Tafsil = 88,
	[PersianNameAttribute.PersianName("حساب معین")]
	Moein = 89,
	[PersianNameAttribute.PersianName("حساب کل")]
	Kol = 90,
	ProductAttribute = 91,
	UserLog = 92,
	Sanad = 93,
	PrdAnbarGardani = 94,
	DepEnteghal = 95,
	LkInfo = 100,
	ChequeMOperation = 101,
	ChequeShOperation = 102,
	PrdSerial = 103,
	IntroductionMethod = 104,
	State = 105,
	City = 106,
	Area = 107,
	SanadStatusReport = 108,
	SanadPrint = 109,
	EntitySync = 110,
	VisitorArea = 111,
	[PersianNameAttribute.PersianName("مانده حساب ها")]
	Account = 112,
	[PersianNameAttribute.PersianName("نقش اشخاص")]
	CustomerRole = 113,
	Note = 114,
	AttPersonLeave = 118,
	[PersianNameAttribute.PersianName("کوپن تخفیف")]
	DiscountCoupon = 119,
	TaxOrder = 120,
	AttDepartment = 121,
	[PersianNameAttribute.PersianName("مالیات کالا")]
	Tax = 122,
	AghsatMGroup = 123,
	[PersianNameAttribute.PersianName("سطح قیمت کالا")]
	ProductPriceLevel = 124,
	[PersianNameAttribute.PersianName("BioMetricData")]
	BiometricData = 125
}
