using System;

namespace Microsoft.Data.SqlClient;

[Serializable]
public enum ApplicationIntent
{
	ReadWrite,
	ReadOnly
}
