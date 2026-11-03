namespace PacketParser.Entities;

public enum Priority : short
{
	[PersianNameAttribute.PersianName("پایین")]
	Low,
	[PersianNameAttribute.PersianName("عادی")]
	Medium,
	[PersianNameAttribute.PersianName("فوری")]
	High
}
