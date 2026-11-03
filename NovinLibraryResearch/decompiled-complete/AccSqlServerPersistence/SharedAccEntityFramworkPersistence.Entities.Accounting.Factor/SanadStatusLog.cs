using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Factor;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;

[Table("Acc_SanadStatusLog")]
public class SanadStatusLog : ISanadStatusLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public Guid? CashierGuid { get; set; }

	public int OldStatus { get; set; }

	public int NewStatus { get; set; }

	public DateTime DateM { get; set; }

	public int Ordinal { get; set; }

	[NotMapped]
	public bool Status { get; set; }

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

	public Cashier Cashier { get; set; }

	public SanadStatusReport SanadStatusReportOld { get; set; }

	public SanadStatusReport SanadStatusReportNew { get; set; }
}
