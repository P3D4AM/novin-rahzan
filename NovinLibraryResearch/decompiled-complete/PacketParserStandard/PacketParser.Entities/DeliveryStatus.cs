namespace PacketParser.Entities;

public enum DeliveryStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknow = 42,
	[PersianNameAttribute.PersianName("خطا")]
	Failed = 40,
	[PersianNameAttribute.PersianName("موفق")]
	Success = 41,
	[PersianNameAttribute.PersianName("ارسال به مخابرات")]
	Mokhaberat = 43,
	[PersianNameAttribute.PersianName("لیست سیاه")]
	Blacklist = 44,
	[PersianNameAttribute.PersianName("انصراف")]
	Cancel = 49,
	[PersianNameAttribute.PersianName("درحال ارسال")]
	Waiting = 47
}
