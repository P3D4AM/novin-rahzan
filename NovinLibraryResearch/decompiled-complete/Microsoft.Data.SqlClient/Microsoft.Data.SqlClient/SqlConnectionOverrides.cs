using System;

namespace Microsoft.Data.SqlClient;

[Flags]
public enum SqlConnectionOverrides
{
	None = 0,
	OpenWithoutRetry = 1
}
