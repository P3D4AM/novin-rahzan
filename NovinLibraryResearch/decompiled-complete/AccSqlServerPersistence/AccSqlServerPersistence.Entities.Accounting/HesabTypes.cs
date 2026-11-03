using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_HesabType")]
public class HesabTypes : IHesabType, IHasJustGuid, IHasStatus
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Name { get; set; }

	public short Code { get; set; }

	public ICollection<HesabGroup> HesabGroups { get; set; }

	[NotMapped]
	public bool Status { get; set; } = true;

}
