using System;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebProductAttributeRelation : IProductAttributeRelation
{
	public Guid AttributeValueGuid { get; set; }

	public string AttributeValue { get; set; }

	public Guid AttributeGuid { get; set; }

	public string AttributeTitle { get; set; }

	public string AttributeSlug { get; set; }

	public Guid ProductGuid { get; set; }
}
