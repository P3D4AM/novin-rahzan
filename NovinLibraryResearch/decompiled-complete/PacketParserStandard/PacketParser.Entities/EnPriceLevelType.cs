namespace PacketParser.Entities;

public enum EnPriceLevelType : short
{
	[PersianNameAttribute.PersianName("فروش")]
	Sell,
	[PersianNameAttribute.PersianName("خرید")]
	Buy
}
