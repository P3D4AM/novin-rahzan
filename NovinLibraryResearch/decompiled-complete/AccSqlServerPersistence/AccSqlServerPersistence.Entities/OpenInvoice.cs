using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order.OpenInvoices;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_OpenInvoice")]
public class OpenInvoice : IOpenInvoice, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

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

	public DateTime DateM { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public string? Description { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public int PeopleCount { get; set; }

	public int SanadNo { get; set; }

	public decimal Sum { get; set; }

	public DateTime? SettlementDate { get; set; }

	public string? MetaData { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Cashier CashierItem { get; set; }

	public Reception ReceptionItem { get; set; }

	public Pardakht PardakhtItem { get; set; }

	public ICollection<OpenInvoiceOrderRelation> OrderRelation { get; set; }

	public ICollection<OpenInvoicePlacementRelation> PlacementRelation { get; set; }
}
