using System;

namespace PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

public interface ICouponDiscount : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	DateTime? DateFrom { get; set; }

	DateTime? DateTo { get; set; }

	bool SingleUse { get; set; }

	int CountCouponUse { get; set; }

	int CountCouponUseForUser { get; set; }

	int UsedCount { get; set; }

	Guid CashierGuid { get; set; }

	EnCouponType Type { get; set; }

	EnCouponMode Mode { get; set; }

	int DeadLineForUse { get; set; }

	bool IsImmediateUseAllowed { get; set; }

	int Version { get; set; }
}
