using System;

namespace PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

public interface ICouponDiscountEntityRelation
{
	Guid CouponDetailGuid { get; set; }

	Guid EntityGuid { get; set; }

	EntityPart EntityType { get; set; }
}
