using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

[Table("Acc_CouponDiscountEntityRelation")]
public class CouponDiscountEntityRelation : ICouponDiscountEntityRelation
{
	public Guid CouponDetailGuid { get; set; }

	public Guid EntityGuid { get; set; }

	public EntityPart EntityType { get; set; }

	public CouponDiscountDetail Coupon { get; set; }
}
