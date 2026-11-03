using System;
using System.Text.Json.Serialization;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting.ProductAttrebutes;

public class ProductAttributeReportBusiness : IProductAttribute, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Slug { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }
}
