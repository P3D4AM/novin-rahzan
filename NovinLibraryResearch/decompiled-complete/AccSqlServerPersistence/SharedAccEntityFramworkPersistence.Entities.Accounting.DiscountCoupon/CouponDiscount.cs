using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

[Table("Acc_CouponDiscount")]
public class CouponDiscount : ICouponDiscount, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid CashierGuid { get; set; }

	[MaxLength(500)]
	public string? Title { get; set; }

	public string? MetaData { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

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

	public bool SingleUse { get; set; }

	public int CountCouponUse { get; set; }

	public int CountCouponUseForUser { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public EnCouponType Type { get; set; }

	public EnCouponMode Mode { get; set; }

	public int DeadLineForUse { get; set; }

	public bool IsImmediateUseAllowed { get; set; }

	public int Version { get; set; }

	public int UsedCount { get; set; }

	public Cashier Cashier { get; set; }

	public Order Order { get; set; }

	public ICollection<CouponDiscountDetail> CouponDiscountDetails { get; set; }

	public ICollection<CouponDiscountUsages> CouponDiscountUsages { get; set; }
}
