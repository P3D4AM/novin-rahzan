using System;

namespace Microsoft.Win32.TaskScheduler;

public interface ITriggerDelay
{
	TimeSpan Delay { get; set; }
}
