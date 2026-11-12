using System;

namespace Microsoft.Data.SqlClient;

[Serializable]
public enum PoolBlockingPeriod
{
	Auto,
	AlwaysBlock,
	NeverBlock
}
