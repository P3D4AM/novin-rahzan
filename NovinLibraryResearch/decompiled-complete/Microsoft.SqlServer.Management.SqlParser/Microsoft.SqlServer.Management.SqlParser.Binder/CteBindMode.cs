using System;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

[Flags]
public enum CteBindMode : sbyte
{
	None = 0,
	Anchor = 1,
	Recursive = 2,
	AnchorOrRecursive = 3
}
