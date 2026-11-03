using System;

namespace PacketParser.Entities.Interfaces.Accounting.Order;

public interface IOrderDetailCouponCalculation : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ProductGuid { get; set; }

	decimal Quantity { get; set; }

	decimal Price { get; set; }

	decimal Discount { get; set; }

	decimal RowSum { get; set; }

	Guid? DicountCouponDetailGuid { get; set; }

	int RefrencedId { get; set; }
}
