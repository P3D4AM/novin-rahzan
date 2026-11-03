using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_HesabGroup")]
public class HesabGroup : IHesabGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
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

	[Required]
	public string Name { get; set; }

	[Required]
	public int Code { get; set; }

	public Guid TypeGuid { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public HesabTypes HesabTypes { get; set; }

	public ICollection<Kol> Kols { get; set; }
}
