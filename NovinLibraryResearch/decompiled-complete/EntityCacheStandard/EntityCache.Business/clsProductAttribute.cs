using System;

namespace EntityCache.Business;

internal class clsProductAttribute
{
	public string Name { get; private set; }

	public Guid AttributeValueGuid { get; private set; }

	public short Ordinal { get; private set; }

	public clsProductAttribute(string name, Guid guid, short index)
	{
		Name = name;
		AttributeValueGuid = guid;
		Ordinal = index;
	}
}
