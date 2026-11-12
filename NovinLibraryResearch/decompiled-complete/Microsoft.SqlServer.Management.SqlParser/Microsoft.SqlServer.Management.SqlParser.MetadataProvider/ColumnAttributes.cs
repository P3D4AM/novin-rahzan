using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

[Flags]
public enum ColumnAttributes
{
	None = 0,
	Identity = 1,
	InPrimaryKey = 2,
	Nullable = 4
}
