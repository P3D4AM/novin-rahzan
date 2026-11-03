using System;

namespace EntityCache.Business;

public class clsProductAttributeRelation
{
	public Guid AttributeGuid { get; private set; }

	public Guid AttributeValueGuid { get; private set; }

	public clsProductAttributeRelation(Guid attributeGuid, Guid attributeValueGuid)
	{
		AttributeGuid = attributeGuid;
		AttributeValueGuid = attributeValueGuid;
	}
}
