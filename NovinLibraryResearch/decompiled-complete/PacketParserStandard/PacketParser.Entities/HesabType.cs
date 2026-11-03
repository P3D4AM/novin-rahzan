namespace PacketParser.Entities;

public enum HesabType : short
{
	[PersianNameAttribute.PersianName("همه")]
	All,
	[PersianNameAttribute.PersianName("مشتریان")]
	Customers,
	[PersianNameAttribute.PersianName("هزینه")]
	Hazine,
	[PersianNameAttribute.PersianName("برداشت")]
	Bardasht,
	[PersianNameAttribute.PersianName("صندوق")]
	Sandogh,
	[PersianNameAttribute.PersianName("بانک")]
	Bank,
	[PersianNameAttribute.PersianName("")]
	temp6,
	[PersianNameAttribute.PersianName("")]
	temp7,
	[PersianNameAttribute.PersianName("")]
	temp8,
	[PersianNameAttribute.PersianName("")]
	temp9,
	[PersianNameAttribute.PersianName("حساب")]
	Hesab,
	[PersianNameAttribute.PersianName("نامشخص")]
	None
}
