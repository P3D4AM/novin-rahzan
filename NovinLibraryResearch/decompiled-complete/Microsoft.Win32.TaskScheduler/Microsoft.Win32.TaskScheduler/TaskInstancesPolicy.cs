using System.ComponentModel;

namespace Microsoft.Win32.TaskScheduler;

[DefaultValue(TaskInstancesPolicy.IgnoreNew)]
public enum TaskInstancesPolicy
{
	Parallel,
	Queue,
	IgnoreNew,
	StopExisting
}
