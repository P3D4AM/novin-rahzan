namespace PacketParser.Entities;

public enum EnAghsatShStatus
{
	[PersianNameAttribute.PersianName("پرداخت شده")]
	Payed = 1,
	[PersianNameAttribute.PersianName("پرداخت نشده")]
	NotPayed,
	[PersianNameAttribute.PersianName("باطل شده")]
	Removed
}
