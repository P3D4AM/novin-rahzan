using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class PrimitiveTypeInfo : TypeInfo
{
	public PrimitiveTypeInfo(Type type, int id)
		: base(type, id)
	{
	}
}
