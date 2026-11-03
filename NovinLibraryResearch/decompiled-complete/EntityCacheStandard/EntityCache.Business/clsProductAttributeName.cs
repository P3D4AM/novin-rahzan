using System;
using System.Collections.Generic;

namespace EntityCache.Business;

internal class clsProductAttributeName
{
	public string Name { get; private set; }

	public List<Guid> AttributeValueGuidList { get; private set; }

	public clsProductAttributeName(string name, List<Guid> guidList)
	{
		Name = name;
		AttributeValueGuidList = guidList;
	}
}
