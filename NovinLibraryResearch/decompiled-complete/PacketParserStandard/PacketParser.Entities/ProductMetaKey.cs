namespace PacketParser.Entities;

public enum ProductMetaKey : short
{
	[PersianNameAttribute.PersianName("None")]
	None = 100,
	[PersianNameAttribute.PersianName("شناسه مالیاتی کالا")]
	TaxCode = 0,
	[PersianNameAttribute.PersianName("نام مالیاتی کالا")]
	TaxName = 1,
	[PersianNameAttribute.PersianName("شناسه تنوع ها")]
	AttributeGuidList = 2,
	[PersianNameAttribute.PersianName("مشخصات سئو")]
	SeoModel = 3,
	[PersianNameAttribute.PersianName("قابل فروش")]
	AvailableForSell = 4,
	[PersianNameAttribute.PersianName("قابل خرید")]
	AvailableForBuy = 5,
	[PersianNameAttribute.PersianName("قابل فروش در فروشگاه اینترنتی")]
	AvailableForSellToOnlineShop = 6,
	[PersianNameAttribute.PersianName("قابل مرجوع")]
	AvailableForReturn = 7
}
