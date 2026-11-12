using System;

namespace Microsoft.SqlServer.Management.Dmf;

[Flags]
public enum PolicyEffectiveState
{
	None = 0,
	Enabled = 1,
	InFilter = 2,
	InCategory = 4
}
