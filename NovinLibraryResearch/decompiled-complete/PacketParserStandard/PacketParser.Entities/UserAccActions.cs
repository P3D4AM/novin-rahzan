namespace PacketParser.Entities;

public enum UserAccActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse,
	[PersianNameAttribute.PersianName("درخواست خواندن")]
	ReadRequest,
	[PersianNameAttribute.PersianName("سینک")]
	ReadResponse
}
