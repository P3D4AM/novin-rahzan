using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.CouponDiscount;

public class CouponDiscountDetailTypeForDisplay
{
	public EnDiscountType DiscountType { get; set; }

	public string DiscountTypeForDisplay => DiscountType.GetDisplay().GetTranslate();

	public CouponDiscountDetailTypeForDisplay(EnDiscountType discountType)
	{
		DiscountType = discountType;
	}
}
