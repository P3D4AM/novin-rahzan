using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.CouponDiscount;

public class CouponDiscountModeForDisplay
{
	public EnCouponMode CouponMode { get; set; }

	public string DiscountTypeForDisplay => CouponMode.GetDisplay();

	public CouponDiscountModeForDisplay(EnCouponMode couponMode)
	{
		CouponMode = couponMode;
	}
}
