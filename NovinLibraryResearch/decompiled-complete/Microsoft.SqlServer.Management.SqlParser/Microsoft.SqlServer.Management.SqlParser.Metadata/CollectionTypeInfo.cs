using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class CollectionTypeInfo : TypeInfo
{
	private readonly bool isOrdered;

	private TypeInfo elementType;

	public TypeInfo ElementType => elementType;

	public bool IsOrdered => isOrdered;

	public CollectionTypeInfo(Type type, int id, bool isOrdered)
		: base(type, id)
	{
		this.isOrdered = isOrdered;
	}

	public void SetElementType(TypeInfo elementType)
	{
		this.elementType = elementType;
	}
}
