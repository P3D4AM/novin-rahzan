using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdTechnical")]
public class PrdTechnical : IPrdTechnical, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Required]
	public string Code { get; set; }

	[Required]
	public string Name { get; set; }

	public short TechnicalType { get; set; }

	public Guid ProductGuid { get; set; }

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


	public Product Product { get; set; }
}
