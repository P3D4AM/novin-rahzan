using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Gift;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Gift;

[Table("Acc_Gift")]
public class Gift : IGift, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData, IDeliverableEntity
{
	[NotMapped]
	public DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public string MetaData { get; set; }

	public string Title { get; set; }

	public string Discription { get; set; }

	public string DetailList { get; set; }

	public EnTypeChose Type { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(20) : value);
			_modified = modified;
		}
	}

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public ICollection<CouponDiscountDetail> CouponDiscountDetail { get; set; }
}
