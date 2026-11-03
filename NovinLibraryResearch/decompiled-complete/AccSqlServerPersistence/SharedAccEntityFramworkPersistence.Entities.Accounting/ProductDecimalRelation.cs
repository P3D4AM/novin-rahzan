using System;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces.Accounting;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting;

[Table("Acc_ProductPrices")]
public class ProductDecimalRelation : IProductDecimalRelation
{
	public Guid ProductGuid { get; set; }

	public Guid DecimalGuid { get; set; }

	public decimal Amount { get; set; }

	public Product Product { get; set; }

	public DecimalEntity Decimal { get; set; }
}
