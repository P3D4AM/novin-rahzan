namespace PacketParser.Entities;

public enum TypeHozor : short
{
	[PersianNameAttribute.PersianName("-")]
	None,
	[PersianNameAttribute.PersianName("سطح یک")]
	Level1,
	[PersianNameAttribute.PersianName("سطح دو")]
	Level2,
	[PersianNameAttribute.PersianName("سطح سه")]
	Level3,
	[PersianNameAttribute.PersianName("سطح چهار")]
	Level4,
	[PersianNameAttribute.PersianName("سطح پنج")]
	Level5
}
