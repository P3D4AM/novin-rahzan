namespace PacketParser.Entities;

public enum ProductSort
{
	[PersianNameAttribute.PersianName("نام صعودی")]
	NameAsc,
	[PersianNameAttribute.PersianName("نام نزولی")]
	NameDesc,
	[PersianNameAttribute.PersianName("کد صعودی")]
	CodeAsc,
	[PersianNameAttribute.PersianName("کد نزولی")]
	CodeDesc,
	[PersianNameAttribute.PersianName("قیمت صعودی")]
	PriceAsc,
	[PersianNameAttribute.PersianName("قیمت نزولی")]
	PriceDesc,
	[PersianNameAttribute.PersianName("موجودی صعودی")]
	AmontAsc,
	[PersianNameAttribute.PersianName("موجودی نزولی")]
	AmontDesc
}
