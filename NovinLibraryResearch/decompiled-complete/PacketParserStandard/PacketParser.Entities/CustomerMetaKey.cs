namespace PacketParser.Entities;

public enum CustomerMetaKey : short
{
	[PersianNameAttribute.PersianName("نحوه آشنایی")]
	MethodIfIntroduction = 0,
	[PersianNameAttribute.PersianName("شماره حساب 1")]
	HesabNumber1 = 1,
	[PersianNameAttribute.PersianName("شماره حساب 2")]
	HesabNumber2 = 2,
	[PersianNameAttribute.PersianName("شماره کارت 1")]
	CardNumber1 = 3,
	[PersianNameAttribute.PersianName("شماره کارت 2")]
	CardNumber2 = 4,
	[PersianNameAttribute.PersianName("آدرس 1")]
	Address1 = 5,
	[PersianNameAttribute.PersianName("آدرس 2")]
	Address2 = 6,
	[PersianNameAttribute.PersianName("آدرس ذخیره سازی آهنگ زنگ")]
	RingToneAddress = 7,
	[PersianNameAttribute.PersianName("آدرس سایت")]
	SiteUrl = 8,
	[PersianNameAttribute.PersianName("تاریخ تولد")]
	BirthDate = 9,
	[PersianNameAttribute.PersianName("تاریخ اعتبار")]
	CreditDate = 10,
	[PersianNameAttribute.PersianName("تاریخ 3")]
	Date3 = 11,
	[PersianNameAttribute.PersianName("تاریخ 4")]
	Date4 = 12,
	[PersianNameAttribute.PersianName("تاریخ 5")]
	Date5 = 13,
	[PersianNameAttribute.PersianName("دوره ویزیت")]
	VisitPeriod = 14,
	[PersianNameAttribute.PersianName("طول دوره ویزیت")]
	VisitPeriodTerm = 15,
	[PersianNameAttribute.PersianName("اطلاعات بانکی")]
	BankInfo = 16,
	[PersianNameAttribute.PersianName("کدشعبه")]
	BrachCode = 17,
	[PersianNameAttribute.PersianName("مناسبت ها")]
	OccasionCalender = 18,
	[PersianNameAttribute.PersianName("None")]
	None = 100
}
