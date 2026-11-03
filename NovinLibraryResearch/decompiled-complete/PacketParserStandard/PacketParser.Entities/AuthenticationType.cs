namespace PacketParser.Entities;

public enum AuthenticationType : short
{
	[PersianNameAttribute.PersianName("نام کاربری")]
	UserName,
	[PersianNameAttribute.PersianName("شماره موبایل")]
	Mobile,
	[PersianNameAttribute.PersianName("کد اشتراک")]
	Code,
	[PersianNameAttribute.PersianName("کارت")]
	Card
}
