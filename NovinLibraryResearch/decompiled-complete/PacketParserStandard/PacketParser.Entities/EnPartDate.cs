namespace PacketParser.Entities;

public enum EnPartDate
{
	[PersianNameAttribute.PersianName("سال")]
	Year = 1,
	[PersianNameAttribute.PersianName("ماه")]
	Mounth,
	[PersianNameAttribute.PersianName("روز")]
	Day
}
