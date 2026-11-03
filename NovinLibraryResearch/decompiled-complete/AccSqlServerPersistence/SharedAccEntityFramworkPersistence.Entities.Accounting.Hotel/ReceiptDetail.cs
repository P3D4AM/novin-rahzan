using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

[Table("Hotel_ReceiptDetail")]
public class ReceiptDetail : IReceiptDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public decimal TotalPrice { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal Avarez { get; set; }

	public decimal AvarezPercent { get; set; }

	public Guid PlacementGuid { get; set; }

	public Guid MasterGuid { get; set; }

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

	public bool Status { get; set; }

	public int ExtraCount { get; set; }

	public decimal ExtraPrice { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent { get; set; }

	public string MetaData { get; set; }

	public decimal PricePerNight { get; set; }

	public decimal Penalty { get; set; }

	public Placement placement { get; set; }

	public Receipt Master { get; set; }

	public ICollection<ServiceReceiptDetailRelation> ServiceReceiptDetail { get; set; }
}
