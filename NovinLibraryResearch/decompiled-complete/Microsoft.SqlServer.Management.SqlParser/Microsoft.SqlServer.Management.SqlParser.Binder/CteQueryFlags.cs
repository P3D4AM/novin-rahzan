using System;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

[Flags]
public enum CteQueryFlags : sbyte
{
	None = 0,
	HasUnionAll = 1,
	HasSelfReference = 2
}
