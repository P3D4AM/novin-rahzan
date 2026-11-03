using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_RasGiriDet")]
public class RasGiriDet : IRasGiriDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public decimal Price { get; set; }

	public decimal Enheraf { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid MasterGuid { get; set; }

	public RasGiri Master { get; set; }
}
