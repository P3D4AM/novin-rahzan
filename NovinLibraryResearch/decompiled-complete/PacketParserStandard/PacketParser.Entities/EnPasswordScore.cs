namespace PacketParser.Entities;

public enum EnPasswordScore : short
{
	[PersianNameAttribute.PersianName("")]
	Blank,
	[PersianNameAttribute.PersianName("بسیار ضعیف")]
	VeryWeak,
	[PersianNameAttribute.PersianName("ضعیف")]
	Weak,
	[PersianNameAttribute.PersianName("متوسط")]
	Medium,
	[PersianNameAttribute.PersianName("قوی")]
	Strong,
	[PersianNameAttribute.PersianName("بسیار قوی")]
	VeryStrong
}
