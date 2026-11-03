using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;

[Table("Acc_DepotReceipt")]
public class DepotReceipt : IDepotReceipt, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public Guid? TafsilGuid { get; set; }

	public string? Description { get; set; }

	public bool IsTemp { get; set; }

	public Guid CashierGuid { get; set; }

	[MaxLength(500)]
	public string? CarNumber { get; set; }

	[MaxLength(500)]
	public string? DriverName { get; set; }

	public EnDepotReceiptType Type { get; set; }

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

	[NotMapped]
	public bool Status { get; set; }

	public string? MetaData { get; set; }

	[MaxLength(150)]
	public string? DriverPhone { get; set; }

	[MaxLength(150)]
	public string? BarnameNo { get; set; }

	[MaxLength(150)]
	public string? RefrencedNumber { get; set; }

	public Guid? OrderGuid { get; set; }

	public virtual Tafsil TafsilItem { get; set; }

	public virtual Cashier CashierItem { get; set; }

	public virtual Order OrderItem { get; set; }

	public ICollection<DepotReceiptDetail> DepotReceiptDetail { get; set; }
}
