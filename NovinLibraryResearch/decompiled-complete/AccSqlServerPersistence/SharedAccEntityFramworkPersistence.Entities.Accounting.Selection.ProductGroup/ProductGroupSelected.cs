using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup;

[Table("Acc_PrdGroupSelected")]
public class ProductGroupSelected : IPrdGroupSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[StringLength(50)]
	[Required]
	public string Code { get; set; }

	[Required]
	public string Title { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	[Required]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Cashier CashierItem { get; set; }

	public string? MetaData { get; set; }

	public ICollection<ProductGroupSelectedDetails> ProductGroupSelectedDetails { get; set; }
}
