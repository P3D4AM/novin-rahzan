namespace PacketParser.Entities;

public enum MaliYearPolicy : short
{
	[PersianNameAttribute.PersianName("سالانه")]
	Annual,
	[PersianNameAttribute.PersianName("فصلی")]
	Seasonal,
	[PersianNameAttribute.PersianName("تاریخ متغیر")]
	VariableDate
}
