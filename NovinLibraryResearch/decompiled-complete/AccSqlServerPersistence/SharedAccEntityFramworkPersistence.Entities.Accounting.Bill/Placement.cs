using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

[Table("Acc_Placement")]
public class Placement : IPlacement, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid? MasterGuid { get; set; }

	public Guid? GroupGuid { get; set; }

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

	public int Number { get; set; }

	public decimal Price { get; set; }

	public decimal ExtraPrice { get; set; }

	public int ItemCount { get; set; }

	[MaxLength(500)]
	public string? Title { get; set; }

	public string? Description { get; set; }

	public Hall Master { get; set; }

	public ProductGroup ProductGroup { get; set; }

	public ICollection<ReceiptDetail> ReciptDetails { get; set; }

	public ICollection<PlacementFacilitiesRelation> PlacementFacilities { get; set; }

	public ICollection<TempBill> TempBills { get; set; }

	public ICollection<BillDetail> BillDetails { get; set; }

	public ICollection<OpenInvoicePlacementRelation> PlacementRelation { get; set; }

	public ICollection<PlacementOrderRelation> PlacementOrderRelation { get; set; }

	public ICollection<ReservationDetail> ReservationDetail { get; set; }
}
