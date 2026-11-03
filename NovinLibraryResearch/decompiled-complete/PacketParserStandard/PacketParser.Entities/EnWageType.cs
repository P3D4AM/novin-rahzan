namespace PacketParser.Entities;

public enum EnWageType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("کارت به کارت")]
	Card,
	[PersianNameAttribute.PersianName("حواله درون بانکی")]
	InternalHavale,
	[PersianNameAttribute.PersianName("انتقال وجه پایا (شخصی)")]
	Paya_Personal,
	[PersianNameAttribute.PersianName("انتقال وجه پایا (گروهی)")]
	Paya_Group,
	[PersianNameAttribute.PersianName("انتقال وجه ساتنا")]
	Satna,
	[PersianNameAttribute.PersianName("کارتخوان")]
	Poss,
	[PersianNameAttribute.PersianName("حواله پل")]
	Bridge
}
