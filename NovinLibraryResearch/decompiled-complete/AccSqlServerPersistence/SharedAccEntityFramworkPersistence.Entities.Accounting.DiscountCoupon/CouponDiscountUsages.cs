using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

[Table("Acc_CouponDiscountUsages")]
public class CouponDiscountUsages : ICouponDiscountUsages
{
	public Guid CouponGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public CouponDiscount Coupon { get; set; }
}
