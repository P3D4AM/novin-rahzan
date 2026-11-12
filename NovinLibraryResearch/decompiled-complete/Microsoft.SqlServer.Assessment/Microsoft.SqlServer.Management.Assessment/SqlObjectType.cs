using System;

namespace Microsoft.SqlServer.Management.Assessment;

[Flags]
public enum SqlObjectType
{
	None = 0,
	Server = 1,
	Database = 2,
	FileGroup = 4,
	AvailabilityGroup = 8
}
