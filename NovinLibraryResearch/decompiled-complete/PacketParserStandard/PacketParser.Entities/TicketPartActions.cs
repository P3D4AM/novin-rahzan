namespace PacketParser.Entities;

public enum TicketPartActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = 8,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse,
	[PersianNameAttribute.PersianName("درخواست خواندن")]
	ReadRequest,
	[PersianNameAttribute.PersianName("سینک")]
	ReadResponse,
	[PersianNameAttribute.PersianName("درخواست ریست")]
	Reset
}
