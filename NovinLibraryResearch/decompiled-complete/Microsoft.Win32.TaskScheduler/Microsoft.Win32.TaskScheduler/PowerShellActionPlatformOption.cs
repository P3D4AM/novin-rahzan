using System;

namespace Microsoft.Win32.TaskScheduler;

[Flags]
public enum PowerShellActionPlatformOption
{
	Never = 0,
	Version1 = 1,
	Version2 = 2,
	All = 3
}
