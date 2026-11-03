namespace PacketParser.Entities;

public enum AccountingPackage : short
{
	[PersianNameAttribute.PersianName("عمومی")]
	General = 0,
	[PersianNameAttribute.PersianName("پخش مواد غذایی")]
	Pakhsh = 2
}
