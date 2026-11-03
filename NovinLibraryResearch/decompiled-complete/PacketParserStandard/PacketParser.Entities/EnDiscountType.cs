namespace PacketParser.Entities;

public enum EnDiscountType : short
{
	[PersianNameAttribute.PersianName("ثابت سبد خرید (درصد)")]
	OrderFixedPercent,
	[PersianNameAttribute.PersianName("ثابت سبد خرید (مبلغ)")]
	OrderFixedPrice,
	[PersianNameAttribute.PersianName("ثابت محصول (درصد)")]
	ProductFixedPercent,
	[PersianNameAttribute.PersianName("ثابت محصول (مبلغ)")]
	ProductFixedPrice
}
