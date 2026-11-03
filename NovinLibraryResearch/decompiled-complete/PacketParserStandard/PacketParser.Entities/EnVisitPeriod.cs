namespace PacketParser.Entities;

public enum EnVisitPeriod : short
{
	[PersianNameAttribute.PersianName("None")]
	None,
	[PersianNameAttribute.PersianName("روزانه")]
	Daily,
	[PersianNameAttribute.PersianName("هفتگی")]
	Weekly,
	[PersianNameAttribute.PersianName("ماهانه")]
	Monthly,
	[PersianNameAttribute.PersianName("فصلی")]
	Seasonal,
	[PersianNameAttribute.PersianName("سالانه")]
	Yearly
}
