using System;

namespace PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

public interface ICouponDiscountUsages
{
	Guid TafsilGuid { get; set; }

	Guid CouponGuid { get; set; }
}
