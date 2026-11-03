namespace PacketParser.Entities;

public enum ReadStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 0,
	[PersianNameAttribute.PersianName("ارسال شده")]
	Read = 1,
	[PersianNameAttribute.PersianName("ارسال نشده")]
	UnRead = 2,
	[PersianNameAttribute.PersianName("خطا در تحویل")]
	DeliveryError = 4
}
