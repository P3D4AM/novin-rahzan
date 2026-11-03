using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Coding;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Coding;

[Table("Acc_AccountLog")]
public class AccountLog : IAccountLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime Date { get; set; }

	public decimal NewAccount { get; set; }

	public decimal OldAccount { get; set; }

	[MaxLength(1000)]
	public string DebuggerInfo { get; set; }

	public ENAction Action { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Moein Moein { get; set; }

	public Tafsil Tafsil { get; set; }
}
