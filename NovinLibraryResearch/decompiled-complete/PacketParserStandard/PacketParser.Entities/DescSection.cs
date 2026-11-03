namespace PacketParser.Entities;

public enum DescSection : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 0,
	[PersianNameAttribute.PersianName("خرید")]
	Buy = 1,
	[PersianNameAttribute.PersianName("برگشت از خرید")]
	BBuy = 2,
	[PersianNameAttribute.PersianName("فروش")]
	Sell = 3,
	[PersianNameAttribute.PersianName("فاکتور برگشت از فروش")]
	BSell = 4,
	[PersianNameAttribute.PersianName("سند دریافت")]
	Reception = 5,
	[PersianNameAttribute.PersianName("سند پرداخت")]
	Pardakht = 6,
	[PersianNameAttribute.PersianName("سند حسابداری یک سطری")]
	SanadSimple = 7,
	[PersianNameAttribute.PersianName("سند حسابداری")]
	Sanad = 8,
	[PersianNameAttribute.PersianName("اقلام فاکتورها")]
	Factor = 9,
	[PersianNameAttribute.PersianName("معایب ذکر شده")]
	ResidProblem = 10,
	[PersianNameAttribute.PersianName("موارد درخواستی")]
	ResidDarkhast = 11,
	[PersianNameAttribute.PersianName("قرارداد")]
	HozorGharadad = 12,
	[PersianNameAttribute.PersianName("فروش")]
	Sell_ = 13,
	[PersianNameAttribute.PersianName("عنوان تیکت")]
	Tickettitle = 50,
	[PersianNameAttribute.PersianName("توضیحات تیکت")]
	TicketDesc = 51,
	[PersianNameAttribute.PersianName("توضیحات جزئیات تیکت")]
	TicketDetDesc = 52,
	[PersianNameAttribute.PersianName("توضیحات ویزیت")]
	VisitDesc = 60,
	[PersianNameAttribute.PersianName("توضیحات مدیر")]
	VisitDetAdminDesc = 61,
	[PersianNameAttribute.PersianName("توضیحات ویزیتور")]
	VisitDetVisitorDesc = 62,
	[PersianNameAttribute.PersianName("مشخصات شرکت")]
	CompanyDesc = 63,
	[PersianNameAttribute.PersianName("یادداشت")]
	Note = 64
}
