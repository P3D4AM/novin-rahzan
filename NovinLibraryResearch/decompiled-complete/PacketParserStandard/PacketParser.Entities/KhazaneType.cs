namespace PacketParser.Entities;

public enum KhazaneType : short
{
	[PersianNameAttribute.PersianName("تعیین نشده")]
	None,
	[PersianNameAttribute.PersianName("نقدی")]
	Naghd,
	[PersianNameAttribute.PersianName("حواله")]
	Havale,
	[PersianNameAttribute.PersianName("چک مشتری")]
	ChequeM,
	[PersianNameAttribute.PersianName("چک شخصی")]
	ChequeSh,
	[PersianNameAttribute.PersianName("انتقال بدهکاری")]
	EnteghalBed,
	[PersianNameAttribute.PersianName("انتقال بستانکاری")]
	EnteghalBes,
	[PersianNameAttribute.PersianName("کارتخوان")]
	Poss
}
