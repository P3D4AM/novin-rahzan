using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Interfaces.Accounting.Cheque;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting.Cheque;

[Table("Acc_ChequeBook")]
public class ChequeBook : IChequeBook, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified = DateTime.Now;

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

	[NotMapped]
	public bool Status { get; }

	[Required]
	public string SerialNumber { get; set; }

	public string? MetaData { get; set; }

	public Guid BankGuid { get; set; }

	public bool IsSaderShode { get; set; }

	public Tafsil Tafsil { get; set; }
}
