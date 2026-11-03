namespace PacketParser.Entities;

internal enum TelegramMessageActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse
}
