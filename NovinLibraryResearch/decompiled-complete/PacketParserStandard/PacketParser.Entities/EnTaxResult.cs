namespace PacketParser.Entities;

public enum EnTaxResult : short
{
	[PersianNameAttribute.PersianName("ارسال نشده")]
	None,
	[PersianNameAttribute.PersianName("ارسال شده")]
	Sent,
	[PersianNameAttribute.PersianName("تایید شده")]
	Accepted,
	[PersianNameAttribute.PersianName("رد شده")]
	Failed,
	[PersianNameAttribute.PersianName("یافت نشد")]
	NotFound
}
