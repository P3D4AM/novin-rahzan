namespace PacketParser.Entities;

public enum SearchLocation
{
	[PersianNameAttribute.PersianName("هرکجا")]
	All,
	[PersianNameAttribute.PersianName("ابتدا")]
	Start,
	[PersianNameAttribute.PersianName("انتها")]
	End,
	[PersianNameAttribute.PersianName("همسان")]
	Equal
}
