using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TTMS;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject;

[Table("Acc_TTMS")]
public class TTMS : ITTMS, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
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

	public string FilePath { get; set; }

	public EnSeason Priod { get; set; }

	public short Year { get; set; }

	public short State { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public ICollection<TTMSDetail> DetailList { get; set; }
}
