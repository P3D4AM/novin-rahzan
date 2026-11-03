namespace PacketParser.Entities;

public enum EnArea : short
{
	[PersianNameAttribute.PersianName("استان")]
	State = 1,
	[PersianNameAttribute.PersianName("شهرستان")]
	City,
	[PersianNameAttribute.PersianName("ناحیه")]
	Area
}
