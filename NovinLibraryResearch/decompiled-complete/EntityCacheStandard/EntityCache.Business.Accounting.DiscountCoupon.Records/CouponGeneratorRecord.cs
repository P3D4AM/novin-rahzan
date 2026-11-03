using System;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.DiscountCoupon.Records;

public record CouponGeneratorRecord
{
	public CouponDiscountBusiness BaseCoupon { get; private set; }

	public DateTime DateM { get; private set; }

	public Guid TafsilGuid { get; private set; }

	public string Title { get; private set; }

	public decimal DiscountAmount { get; private set; }

	public EnDiscountType DiscountType { get; private set; }

	public CouponGeneratorRecord(CouponDiscountBusiness baseCoupon, DateTime dateM, Guid tafsilGuid, string title, decimal discountAmount, EnDiscountType type)
	{
		BaseCoupon = baseCoupon;
		DateM = dateM;
		TafsilGuid = tafsilGuid;
		Title = title;
		DiscountAmount = discountAmount;
		DiscountType = type;
	}
}
