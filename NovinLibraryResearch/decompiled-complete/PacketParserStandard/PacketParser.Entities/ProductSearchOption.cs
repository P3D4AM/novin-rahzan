namespace PacketParser.Entities;

public enum ProductSearchOption
{
	[PersianNameAttribute.PersianName("تمامی موارد")]
	All = 0,
	[PersianNameAttribute.PersianName("کد یا نام")]
	CodeOrName = 1,
	[PersianNameAttribute.PersianName("نام کالا")]
	JustName = 2,
	[PersianNameAttribute.PersianName("کد کالا")]
	JustCode = 3,
	[PersianNameAttribute.PersianName("واحد")]
	JustVahed = 4,
	[PersianNameAttribute.PersianName("هیچکدام")]
	None = 10
}
