using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

[Flags]
internal enum SerializationOptions
{
	None = 0,
	ExcludeServerName = 1,
	ExcludeText = 2
}
