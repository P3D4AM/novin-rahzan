namespace PacketParser.Entities;

public enum EnProductChangePriceType : short
{
	[PersianNameAttribute.PersianName("افزایش")]
	Increase = 1,
	[PersianNameAttribute.PersianName("کاهش")]
	Decrease
}
