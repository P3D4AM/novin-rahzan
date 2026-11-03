using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_ProductAttribute")]
public class ProductAttribute : IProductAttribute, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	[Required]
	[MaxLength(500)]
	public string Name { get; set; }

	[Required]
	[MaxLength(24)]
	public string Slug { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string? MetaData { get; set; }

	public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; }

	public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
}
