using JetBrains.Annotations;

namespace Microsoft.Win32.TaskScheduler;

[PublicAPI]
public class TaskCompatibilityEntry
{
	public TaskCompatibility CompatibilityLevel { get; }

	public string Property { get; }

	public string Reason { get; }

	internal TaskCompatibilityEntry(TaskCompatibility comp, string prop, string reason)
	{
		CompatibilityLevel = comp;
		Property = prop;
		Reason = reason;
	}
}
