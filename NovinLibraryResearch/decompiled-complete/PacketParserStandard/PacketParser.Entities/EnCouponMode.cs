namespace PacketParser.Entities;

public enum EnCouponMode : short
{
	[PersianNameAttribute.PersianName("عادی")]
	Coupon,
	[PersianNameAttribute.PersianName("تولد")]
	Birthday,
	[PersianNameAttribute.PersianName("نمونه")]
	Sample,
	[PersianNameAttribute.PersianName("تخفیف در خرید بعدی")]
	CashBack,
	[PersianNameAttribute.PersianName("اشانتیون")]
	Gift
}
