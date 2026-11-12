namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskFolderSnapshot : SnapshotItem
{
	internal TaskFolderSnapshot(string path, string sddl)
		: base(path, sddl)
	{
	}
}
