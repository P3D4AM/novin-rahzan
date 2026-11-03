namespace PacketParser.Entities;

public enum EnTelegramType : short
{
	[PersianNameAttribute.PersianName("نام کاربری")]
	UserName,
	[PersianNameAttribute.PersianName("کانال")]
	Chanel,
	[PersianNameAttribute.PersianName("شماره تلفن")]
	Mobile
}
