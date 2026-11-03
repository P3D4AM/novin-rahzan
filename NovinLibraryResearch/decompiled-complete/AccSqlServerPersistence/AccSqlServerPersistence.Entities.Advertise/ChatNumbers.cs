using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Advertise;

namespace AccSqlServerPersistence.Entities.Advertise;

[Table("Adv_ChatNumbers")]
public class ChatNumbers : IChatNumber, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	[MaxLength(15)]
	public string? Number { get; set; }

	public DateTime DateM { get; set; }

	[MaxLength(100)]
	public string? City { get; set; }

	[MaxLength(200)]
	public string? Category { get; set; }

	public bool IsUsed { get; set; }
}
