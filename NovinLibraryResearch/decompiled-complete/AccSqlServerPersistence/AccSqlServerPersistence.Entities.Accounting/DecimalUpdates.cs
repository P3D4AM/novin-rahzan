using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_DecimalUpdates")]
public class DecimalUpdates : IDecimalUpdates, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[NotMapped]
	private DateTime _dateM;

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


	public Guid DecimalGuid { get; set; }

	public DateTime DateM
	{
		get
		{
			return _dateM;
		}
		set
		{
			DateTime dateM = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_dateM = dateM;
		}
	}

	public decimal Price { get; set; }

	public DecimalEntity DecimalEntity { get; set; }
}
