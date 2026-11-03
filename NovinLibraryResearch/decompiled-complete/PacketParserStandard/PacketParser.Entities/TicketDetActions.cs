namespace PacketParser.Entities;

public enum TicketDetActions
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
	[PersianNameAttribute.PersianName("درخواست ریست")]
	Reset = 17,
	[PersianNameAttribute.PersianName("درخواست Seen")]
	SeenRequest = 18,
	[PersianNameAttribute.PersianName("پاسخ به درخواست Seen")]
	SeenResponse = 19,
	[PersianNameAttribute.PersianName("درخواست تحویل")]
	DeliveryRequest = 20
}
