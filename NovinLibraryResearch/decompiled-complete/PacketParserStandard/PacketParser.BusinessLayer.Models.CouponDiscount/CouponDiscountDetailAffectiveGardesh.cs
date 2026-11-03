using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.CouponDiscount;

public class CouponDiscountDetailAffectiveGardesh
{
	public EnCouponGardesh Priod { get; set; }

	public string PriodStr => Priod.GetDisplay().GetTranslate();

	public CouponDiscountDetailAffectiveGardesh(EnCouponGardesh model)
	{
		Priod = model;
	}
}
