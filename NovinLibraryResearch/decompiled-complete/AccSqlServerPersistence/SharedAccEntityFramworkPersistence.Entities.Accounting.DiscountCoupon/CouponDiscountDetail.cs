using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Gift;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

[Table("Acc_CouponDiscountDetail")]
public class CouponDiscountDetail : ICouponDiscountDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public string? MetaData { get; set; }

	public decimal AmountCoupon { get; set; }

	public decimal MinBuy { get; set; }

	public decimal MaxBuy { get; set; }

	public decimal MaxDiscountPrice { get; set; }

	public bool Status { get; set; }

	public EnDiscountType EnDiscountType { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public Guid MasterGuid { get; set; }

	public Guid? GiftGuid { get; set; }

	public bool ProductContains { get; set; }

	public bool CustomerContains { get; set; }

	public int GardeshAffectiveDays { get; set; }

	public decimal GardeshStartPrice { get; set; }

	public decimal GardeshEndPrice { get; set; }

	public decimal MinBuyQuantity { get; set; }

	public CouponDiscount Master { get; set; }

	public SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift Gift { get; set; }

	public ICollection<OrderDetailCouponCalculation> OrderDetailCouponCalculation { get; set; }

	public ICollection<CouponDiscountEntityRelation> CouponDiscountEntityRelation { get; set; }

	public ICollection<OrderDetail> OrderDetails { get; set; }
}
