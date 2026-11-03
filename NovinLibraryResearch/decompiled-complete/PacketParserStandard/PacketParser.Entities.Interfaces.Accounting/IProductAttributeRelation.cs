using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IProductAttributeRelation
{
	Guid AttributeValueGuid { get; set; }

	Guid AttributeGuid { get; set; }

	Guid ProductGuid { get; set; }
}
