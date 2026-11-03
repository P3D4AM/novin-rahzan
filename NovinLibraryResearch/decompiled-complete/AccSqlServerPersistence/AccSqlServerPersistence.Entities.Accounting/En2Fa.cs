using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_L2F")]
public class En2Fa : IEn2Fa, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Required]
	[MaxLength(150)]
	public string LName { get; set; }

	[MaxLength(150)]
	[Required]
	public string FName { get; set; }

	public bool Status { get; set; }
}
