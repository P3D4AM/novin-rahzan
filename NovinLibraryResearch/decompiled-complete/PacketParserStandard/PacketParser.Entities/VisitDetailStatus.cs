namespace PacketParser.Entities;

public enum VisitDetailStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = 0,
	[PersianNameAttribute.PersianName("شروع نشده")]
	Future = 1,
	[PersianNameAttribute.PersianName("فاکتور زده")]
	Ordered = 2,
	[PersianNameAttribute.PersianName("مراجعه مجدد")]
	ReturnAgain = 3,
	[PersianNameAttribute.PersianName("جابجایی مشتری")]
	ReturnAgainCustomerMoved = 4,
	[PersianNameAttribute.PersianName("تعطیل")]
	ReturnAgainCustomerClosed = 5,
	[PersianNameAttribute.PersianName("موجودی کافی")]
	ReturnAgainInventoryFull = 6,
	[PersianNameAttribute.PersianName("سایر")]
	ReturnAgainOther = 30
}
