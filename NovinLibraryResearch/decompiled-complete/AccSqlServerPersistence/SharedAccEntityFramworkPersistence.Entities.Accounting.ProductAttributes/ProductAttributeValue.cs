using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.ProductAttrebutes;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;

[Table("Acc_ProductAttributeValue")]
public class ProductAttributeValue : IProductAttributeValue, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	[Required]
	[MaxLength(500)]
	public string Name { get; set; }

	[NotMapped]
	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	[Required]
	[MaxLength(500)]
	public string? Slug { get; set; }

	public virtual ProductAttribute MasterItem { get; set; }

	public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
}
