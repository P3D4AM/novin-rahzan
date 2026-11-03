using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_Owner")]
public class Owner : IOwner, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	[Required]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[MaxLength(50)]
	[Required]
	public string Name { get; set; }

	[MaxLength(50)]
	[Required]
	public string Family { get; set; }

	public Guid VisitorGuid { get; set; }

	[Required]
	public bool Status { get; set; }

	public Visitor Visitor { get; set; }
}
