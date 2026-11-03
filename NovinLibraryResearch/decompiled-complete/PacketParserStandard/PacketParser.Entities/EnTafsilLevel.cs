namespace PacketParser.Entities;

public enum EnTafsilLevel : short
{
	[PersianNameAttribute.PersianName("None")]
	None = 0,
	[PersianNameAttribute.PersianName("Level2")]
	Level1 = 1,
	[PersianNameAttribute.PersianName("Level2")]
	Level2 = 2,
	[PersianNameAttribute.PersianName("Level3")]
	Level3 = 4,
	[PersianNameAttribute.PersianName("Level4")]
	Level4 = 8
}
