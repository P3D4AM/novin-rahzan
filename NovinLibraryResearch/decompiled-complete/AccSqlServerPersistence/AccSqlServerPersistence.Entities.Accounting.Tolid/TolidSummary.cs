using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Summary;

namespace AccSqlServerPersistence.Entities.Accounting.Tolid;

[Table("Acc_TolidSummary")]
public class TolidSummary : ITolidSummary, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
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

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public int Id2 { get; set; }

	[Required]
	public string Description { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Cashier Cashier { get; set; }

	public ICollection<TolidSummaryDetail> TolidSummaryDetail { get; set; }

	public string? MetaData { get; set; }
}
