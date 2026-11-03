namespace PacketParser.Entities.Attendance;

public enum ReportFilterEnumType : short
{
	[PersianNameAttribute.PersianName("None")]
	None = 0,
	[PersianNameAttribute.PersianName("  ")]
	RptTaradod = 2,
	[PersianNameAttribute.PersianName("گزارش ورود و خروج پرسنل")]
	RptEnterPerson = 3,
	[PersianNameAttribute.PersianName("گزارش تجمیع تردد")]
	RptPersonsOneMounth = 7,
	[PersianNameAttribute.PersianName("لیست پرسنل")]
	RptPerson = 8,
	[PersianNameAttribute.PersianName("گزارش تردد")]
	RptTaradodFull = 9,
	[PersianNameAttribute.PersianName("صدور فیش حقوقی")]
	SodorFish = 10,
	[PersianNameAttribute.PersianName("گزارش وضعیت پرسنل")]
	RptPersonsOneDay = 11,
	[PersianNameAttribute.PersianName("گزارش مرخصی روزانه")]
	MorakhasiDaily = 12,
	[PersianNameAttribute.PersianName("گزارش مرخصی ساعتی")]
	MorakhasiSaati = 13,
	[PersianNameAttribute.PersianName("لیست فیش های حقوقی")]
	FishList = 14,
	[PersianNameAttribute.PersianName("گزارش مرخصی")]
	RptMorakhasi = 15,
	[PersianNameAttribute.PersianName("لیست قرارداد ها")]
	ListGharardad = 16,
	[PersianNameAttribute.PersianName("گزارش لاگ عملکرد کاربران")]
	ListLogUser = 17,
	[PersianNameAttribute.PersianName("شیفت های قابل برنامه ریزی")]
	ShiftProgrammable = 18,
	[PersianNameAttribute.PersianName("گزارش تردد کلیه کارکنان")]
	RptAllTaradod = 19,
	[PersianNameAttribute.PersianName("لیست اسناد حسابداری")]
	RptSanadList = 20,
	[PersianNameAttribute.PersianName("صدور فیش حقوقی")]
	Issuingpayroll = 21,
	[PersianNameAttribute.PersianName("گزارش تردد شخص در باشگاه")]
	RptPackage = 22,
	[PersianNameAttribute.PersianName("تهیه لیست بیمه")]
	InsuranceList = 23,
	[PersianNameAttribute.PersianName("گزارش پیام های دستگاه حضور و غیاب")]
	RptNotify = 24,
	[PersianNameAttribute.PersianName("گزارش مرخصی ها")]
	RptVacations = 25,
	[PersianNameAttribute.PersianName("کپی قرارداد برای کارکنان از یک قرارداد نمونه")]
	CopyGharardadForPerson = 26,
	[PersianNameAttribute.PersianName("لیست پرسنل جهت تسویه حساب")]
	CheckoutPersonlist = 27,
	[PersianNameAttribute.PersianName("لیست قرارداد ها**")]
	ListGharardadForCalcVacation = 28
}
