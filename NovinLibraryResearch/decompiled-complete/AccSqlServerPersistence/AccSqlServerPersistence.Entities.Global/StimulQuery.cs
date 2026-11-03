using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace AccSqlServerPersistence.Entities.Global;

[Table("StimulQuery")]
public class StimulQuery : IStimulQuery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid? Guid { get; set; }

	public int? Id { get; set; }

	public int RefrencedId { get; set; }

	public int Order { get; set; }

	public DateTime Date { get; set; }

	[MaxLength]
	public string? SecondKey { get; set; }

	[NotMapped]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	Guid IHasJustGuid.Guid { get; set; }
}
