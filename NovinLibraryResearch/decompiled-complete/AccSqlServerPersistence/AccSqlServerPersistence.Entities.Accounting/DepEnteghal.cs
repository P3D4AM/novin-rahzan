using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_DepEnteghal")]
public class DepEnteghal : IDepEnteghal, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
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

	public bool Status { get; set; } = true;


	public Guid UserGuid { get; set; }

	public int No { get; set; }

	[Required]
	public string? Description { get; set; }

	public DateTime DateM { get; set; }

	public DateTime ModifiedM { get; set; }

	[Required]
	public string? MashinNo { get; set; }

	[Required]
	public string? GirandeName { get; set; }

	[Required]
	public string? RanandehName { get; set; }

	public short StatusId { get; set; }

	public string? MetaData { get; set; }

	public Guid? DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public int SanadNo { get; set; }

	public Cashier CashierItem { get; set; }

	public Moein DebitMoeinItem { get; set; }

	public Tafsil DebitTafsilItem { get; set; }

	public Moein CreditMoeinItem { get; set; }

	public Tafsil CreditTafsilItem { get; set; }

	public ICollection<DepEnteghalDet> DepEnteghalDets { get; set; }
}
