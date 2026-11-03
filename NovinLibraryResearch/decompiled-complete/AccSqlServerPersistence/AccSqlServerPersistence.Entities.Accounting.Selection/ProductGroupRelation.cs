using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace AccSqlServerPersistence.Entities.Accounting.Selection;

[Table("Acc_ProductGroupRelation")]
public class ProductGroupRelation : IProductGroupRelation
{
	public Guid GroupGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public short Ordinal { get; set; }

	public Product Product { get; set; }

	public ProductGroup Group { get; set; }
}
