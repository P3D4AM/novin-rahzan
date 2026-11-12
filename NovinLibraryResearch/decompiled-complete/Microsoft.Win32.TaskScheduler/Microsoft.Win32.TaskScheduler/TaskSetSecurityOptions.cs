using System;

namespace Microsoft.Win32.TaskScheduler;

[Flags]
public enum TaskSetSecurityOptions
{
	None = 0,
	DontAddPrincipalAce = 0x10
}
