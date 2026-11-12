using System;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

[Flags]
public enum ErrorFlags
{
	None = 0,
	UnresolvedReference = 1,
	UnresolvedDatabaseObjectReference = 3,
	UnsupportedTSql = 4
}
