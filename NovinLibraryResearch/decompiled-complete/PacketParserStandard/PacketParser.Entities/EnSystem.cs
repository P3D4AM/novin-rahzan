namespace PacketParser.Entities;

public enum EnSystem : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("تبادلات")]
	SignalR,
	[PersianNameAttribute.PersianName("رابط کاربری")]
	UserInterface,
	[PersianNameAttribute.PersianName("انتقال")]
	Transfer,
	[PersianNameAttribute.PersianName("Mapper")]
	Mapper,
	[PersianNameAttribute.PersianName("انتقال سال مالی")]
	TransferMaliYear,
	[PersianNameAttribute.PersianName("TaxApi")]
	TaxApi,
	[PersianNameAttribute.PersianName("SmsParser")]
	SmsParser,
	[PersianNameAttribute.PersianName("بازسازی اطلاعات")]
	BazsaziData,
	[PersianNameAttribute.PersianName("سیستم داخلی")]
	System,
	[PersianNameAttribute.PersianName("وردپرس")]
	Wordpress
}
