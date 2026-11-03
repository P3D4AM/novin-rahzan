using System;

namespace PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

public interface ICouponDiscountDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	decimal AmountCoupon { get; set; }

	EnDiscountType EnDiscountType { get; set; }

	decimal MinBuy { get; set; }

	decimal MaxBuy { get; set; }

	decimal MaxDiscountPrice { get; set; }

	Guid? GiftGuid { get; set; }

	bool ProductContains { get; set; }

	bool CustomerContains { get; set; }

	int GardeshAffectiveDays { get; set; }

	decimal GardeshStartPrice { get; set; }

	decimal GardeshEndPrice { get; set; }

	decimal MinBuyQuantity { get; set; }
}
