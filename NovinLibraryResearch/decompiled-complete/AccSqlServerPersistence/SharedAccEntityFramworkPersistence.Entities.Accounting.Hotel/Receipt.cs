using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

[Table("Hotel_Receipt")]
public class Receipt : IReceipt, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid CustomerGuid { get; set; }

	public Guid OrderGuid { get; set; }

	public Guid? PresenterGuid { get; set; }

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

	public decimal Discount { get; set; }

	public int PepolCount { get; set; }

	public decimal Penalty { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public string MetaData { get; set; }

	public Tafsil Tafsil { get; set; }

	public Tafsil PresenterTafsil { get; set; }

	public Order Order { get; set; }

	public ICollection<ReceiptDetail> ReceiptDetails { get; set; }

	public ICollection<CustomerReceiptRelation> CustomerReceipt { get; set; }
}
