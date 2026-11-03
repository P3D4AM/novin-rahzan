namespace PacketParser.Entities;

public enum NotePirority : short
{
	[PersianNameAttribute.PersianName("ضروری")]
	Zarori,
	[PersianNameAttribute.PersianName("مهم")]
	Mohem,
	[PersianNameAttribute.PersianName("معمولی")]
	Mamoli
}
