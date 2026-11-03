namespace PacketParser.Entities;

public enum ENSessionStatus : short
{
	[PersianNameAttribute.PersianName("سشن مشکل دارد")]
	Faild = 1,
	[PersianNameAttribute.PersianName("قادر به ارسال پیام")]
	Ready
}
