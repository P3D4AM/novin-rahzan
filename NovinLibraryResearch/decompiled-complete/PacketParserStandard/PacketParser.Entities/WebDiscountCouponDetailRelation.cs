using System;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace PacketParser.Entities;

public class WebDiscountCouponDetailRelation : ICouponDiscountEntityRelation
{
	public Guid CouponDetailGuid { get; set; }

	public Guid EntityGuid { get; set; }

	public EntityPart EntityType { get; set; }
}
