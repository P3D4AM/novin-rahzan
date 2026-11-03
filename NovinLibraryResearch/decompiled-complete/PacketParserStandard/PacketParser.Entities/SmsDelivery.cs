namespace PacketParser.Entities;

public enum SmsDelivery
{
	[PersianNameAttribute.PersianName("وضعیتی دریافت نشده")]
	none = 0,
	[PersianNameAttribute.PersianName("رسیده به گوشی")]
	delivered = 1,
	[PersianNameAttribute.PersianName("نرسیده به گوشی")]
	notDelivered = 2,
	[PersianNameAttribute.PersianName("رسیده به مخابرات")]
	send2operator = 8,
	[PersianNameAttribute.PersianName("نرسیده به مخابرات")]
	delivered2operator = 16,
	[PersianNameAttribute.PersianName("لیست سیاه")]
	blacklist = 5,
	[PersianNameAttribute.PersianName("کنسل شده")]
	canceled = 3,
	[PersianNameAttribute.PersianName("در انتظار تحویل به گوشی")]
	pending = 7
}
