using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

[Table("Acc_OrderDetailCouponCalculation")]
public class OrderDetailCouponCalculation : IOrderDetailCouponCalculation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid? DicountCouponDetailGuid { get; set; }

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal RowSum { get; set; }

	public int RefrencedId { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_modified = modified;
		}
	}

	[NotMapped]
	public bool Status { get; set; }

	public virtual Product ProductItem { get; set; }

	public virtual CouponDiscountDetail Coupon { get; set; }
}
