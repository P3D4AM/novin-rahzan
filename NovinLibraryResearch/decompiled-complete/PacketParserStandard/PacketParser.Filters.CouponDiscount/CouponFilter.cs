using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters.CouponDiscount;

public class CouponFilter
{
	public EnCouponType? Type { get; set; } = EnCouponType.Coupon;


	public string CouponCode { get; set; }

	public bool? Status { get; set; } = true;


	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public List<EnCouponMode> ModeList { get; set; }

	public EnCouponUsageType UsingStatus { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public Guid? CashierGuid { get; set; }
}
