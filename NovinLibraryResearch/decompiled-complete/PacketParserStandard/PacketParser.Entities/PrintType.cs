namespace PacketParser.Entities;

public enum PrintType
{
	[PersianNameAttribute.PersianName("فروش")]
	Sell,
	[PersianNameAttribute.PersianName("خرید")]
	Buy,
	[PersianNameAttribute.PersianName("برگشت از فروش")]
	BackSell,
	[PersianNameAttribute.PersianName("برگشت از خرید")]
	BackBuy,
	[PersianNameAttribute.PersianName("دریافت")]
	Reception,
	[PersianNameAttribute.PersianName("پرداخت")]
	Payement,
	[PersianNameAttribute.PersianName("گردش تفصیلی")]
	GardeshTafsili,
	[PersianNameAttribute.PersianName("گردش تجمیعی")]
	GardeshTajmi,
	[PersianNameAttribute.PersianName("نامشخص")]
	none,
	[PersianNameAttribute.PersianName("پیش فاکتور")]
	PishFactor
}
