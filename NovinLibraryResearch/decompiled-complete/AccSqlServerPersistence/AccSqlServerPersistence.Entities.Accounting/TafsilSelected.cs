using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_TafsilSelected")]
public class TafsilSelected : ITafsilSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[StringLength(50)]
	[Required]
	public string Code { get; set; }

	[Required]
	public string Title { get; set; }

	public short? Type { get; set; }

	public Guid UGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Cashier CashierItem { get; set; }

	public string? MetaData { get; set; }

	public ICollection<TafsilSelectedDetails> TafsilSelectedDetails { get; set; }
}
