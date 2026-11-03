namespace PacketParser.Entities;

public enum ENHesabLevel : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("گروه حساب")]
	GroupHesab,
	[PersianNameAttribute.PersianName("حساب کل")]
	Kol,
	[PersianNameAttribute.PersianName("حساب معین")]
	Moein,
	[PersianNameAttribute.PersianName("حساب تفصیلی")]
	Tafsil,
	[PersianNameAttribute.PersianName("گروه حساب تفصیلی")]
	TafsilGroup
}
