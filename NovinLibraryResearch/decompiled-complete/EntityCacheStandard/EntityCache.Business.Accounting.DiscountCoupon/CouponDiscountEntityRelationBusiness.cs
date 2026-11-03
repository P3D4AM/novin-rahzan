using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace EntityCache.Business.Accounting.DiscountCoupon;

public class CouponDiscountEntityRelationBusiness : Serializable<CouponDiscountEntityRelationBusiness>, ICouponDiscountEntityRelation, IHasContext
{
	public Guid CouponDetailGuid { get; set; }

	public Guid EntityGuid { get; set; }

	public EntityPart EntityType { get; set; }

	public string ContextGuid { get; set; }

	public CouponDiscountEntityRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
