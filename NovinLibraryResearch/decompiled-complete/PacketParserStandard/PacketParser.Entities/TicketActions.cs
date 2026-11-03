namespace PacketParser.Entities;

public enum TicketActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = 10,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest = 11,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse = 12,
	[PersianNameAttribute.PersianName("درخواست خواندن")]
	ReadRequest = 15,
	[PersianNameAttribute.PersianName("سینک")]
	ReadResponse = 16,
	[PersianNameAttribute.PersianName("ریست")]
	Reset = 17,
	[PersianNameAttribute.PersianName("درخواست دریافت همه")]
	GetAllRequest = 18,
	[PersianNameAttribute.PersianName("پاسخ درخواست دریافت همه")]
	GetAllReponse = 19
}
