namespace PacketParser.Entities;

public enum EnPersianMonth : short
{
	[PersianNameAttribute.PersianName("فروردین")]
	Farvadin,
	[PersianNameAttribute.PersianName("اردیبهشت")]
	Ordibehesht,
	[PersianNameAttribute.PersianName("خرداد")]
	Khordad,
	[PersianNameAttribute.PersianName("تیر")]
	Tir,
	[PersianNameAttribute.PersianName("مرداد")]
	Mordad,
	[PersianNameAttribute.PersianName("شهریور")]
	Shahrivar,
	[PersianNameAttribute.PersianName("مهر")]
	Mehr,
	[PersianNameAttribute.PersianName("آبان")]
	Aban,
	[PersianNameAttribute.PersianName("آذر")]
	Azar,
	[PersianNameAttribute.PersianName("دی")]
	Dey,
	[PersianNameAttribute.PersianName("بهمن")]
	Bahman,
	[PersianNameAttribute.PersianName("اسفند")]
	Esfand
}
