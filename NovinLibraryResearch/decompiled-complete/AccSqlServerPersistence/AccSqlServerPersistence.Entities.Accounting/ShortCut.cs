using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ShortCut")]
public class ShortCut : IShortCut, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	[MaxLength(200)]
	[Required]
	public string FormName { get; set; }

	[MaxLength(200)]
	[Required]
	public string Name { get; set; }

	public int ShortCutKey { get; set; }

	[MaxLength(200)]
	[Required]
	public string Func { get; set; }

	public Guid CashierGuid { get; set; }

	[Required]
	public string Description { get; set; }

	[MaxLength(15)]
	[Required]
	public string BackColor { get; set; }

	[MaxLength(15)]
	[Required]
	public string ForeColor { get; set; }

	public string? ImageSrc { get; set; }

	public bool Enable { get; set; }

	public bool Visable { get; set; }

	public short SubItems { get; set; }

	public Cashier Cashier { get; set; }
}
