using System;

namespace Microsoft.Win32.TaskScheduler;

[Flags]
public enum TaskRunFlags
{
	NoFlags = 0,
	AsSelf = 1,
	IgnoreConstraints = 2,
	UseSessionId = 4,
	UserSID = 8
}
