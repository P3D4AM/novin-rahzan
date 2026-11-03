using System;
using System.Collections.Generic;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace PacketParser.Entities;

public class WebDiscountCouponDetail : ICouponDiscountDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public decimal AmountCoupon { get; set; }

	public EnDiscountType EnDiscountType { get; set; }

	public decimal MinBuy { get; set; }

	public decimal MaxBuy { get; set; }

	public decimal MaxDiscountPrice { get; set; }

	public Guid? GiftGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public bool ProductContains { get; set; }

	public bool CustomerContains { get; set; }

	public int GardeshAffectiveDays { get; set; }

	public decimal GardeshStartPrice { get; set; }

	public decimal GardeshEndPrice { get; set; }

	public decimal MinBuyQuantity { get; set; }

	public List<WebDiscountCouponDetailRelation> CouponEntityRelation { get; set; }

	public WebGift Gift { get; set; }
}
