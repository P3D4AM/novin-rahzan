namespace PacketParser.Entities;

public enum EnProductType : short
{
	[PersianNameAttribute.PersianName("کالاها")]
	Product = 0,
	[PersianNameAttribute.PersianName("خدمات")]
	Service = 1,
	[PersianNameAttribute.PersianName("Room")]
	Room = 2,
	[PersianNameAttribute.PersianName("None")]
	None = 100
}
