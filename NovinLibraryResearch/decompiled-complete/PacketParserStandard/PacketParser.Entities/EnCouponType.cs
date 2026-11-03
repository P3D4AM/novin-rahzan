namespace PacketParser.Entities;

public enum EnCouponType : short
{
	[PersianNameAttribute.PersianName("کوپن تخفیف")]
	Coupon,
	[PersianNameAttribute.PersianName("پلکان تخفیف")]
	Discount
}
