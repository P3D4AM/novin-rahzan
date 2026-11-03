using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ProductAttributeRelation")]
public class ProductAttributeRelation : IProductAttributeRelation, IHasMetaData
{
	public Guid ProductGuid { get; set; }

	public Guid AttributeValueGuid { get; set; }

	public Guid AttributeGuid { get; set; }

	public string? MetaData { get; set; }

	public virtual Product Product { get; set; }

	public virtual ProductAttribute ProductAttribute { get; set; }

	public virtual ProductAttributeValue ProductAttributeValue { get; set; }
}
