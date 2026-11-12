using System;

namespace Microsoft.SqlServer.Management.SqlParser.Common;

[Flags]
internal enum DataTypeErrors
{
	None = 0,
	NationalNotAllowed = 1,
	VaryingNotAllowed = 2
}
