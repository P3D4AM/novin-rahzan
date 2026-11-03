using System;

namespace EntityCache.Business.OrderDepot.OrderCouponCalculation;

public class OrderCouponRelationDeltailReportBusiness
{
	public Guid CouponDetailGuid { get; private set; }

	public Guid OrderDetailGuid { get; private set; }

	public decimal Discount { get; private set; }

	public OrderCouponRelationDeltailReportBusiness(Guid couponDetGuid, Guid orderDetGuid, decimal discount)
	{
		CouponDetailGuid = couponDetGuid;
		OrderDetailGuid = orderDetGuid;
		Discount = discount;
	}
}
