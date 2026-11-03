using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdSelected")]
public class PrdSelected : IPrdSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	public string Code { get; set; }

	[Required]
	public string Title { get; set; }

	public short? Type { get; set; }

	public Guid UserGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public ICollection<Prd_SelectedDetails> Details { get; set; }

	public Cashier CashierItem { get; set; }

	public string? MetaData { get; set; }
}
