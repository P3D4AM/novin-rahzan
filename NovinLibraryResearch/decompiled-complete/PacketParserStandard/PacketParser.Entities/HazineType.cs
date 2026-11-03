namespace PacketParser.Entities;

public enum HazineType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	UnKnown,
	[PersianNameAttribute.PersianName("دستمزد مستقیم")]
	DirectLabor,
	[PersianNameAttribute.PersianName("دستمزد غیر مستقیم")]
	inDirectLabor,
	[PersianNameAttribute.PersianName("سربار کارخانه")]
	FactoryOverhead,
	[PersianNameAttribute.PersianName("سایر هزینه های تولید")]
	Other
}
