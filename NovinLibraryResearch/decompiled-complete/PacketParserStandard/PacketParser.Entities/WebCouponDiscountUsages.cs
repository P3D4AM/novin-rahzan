using System;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace PacketParser.Entities;

public class WebCouponDiscountUsages : ICouponDiscountUsages
{
	public Guid TafsilGuid { get; set; }

	public Guid CouponGuid { get; set; }
}
