namespace Microsoft.Win32.TaskScheduler;

public abstract class SnapshotItem
{
	public string Path { get; private set; }

	public string Sddl { get; private set; }

	internal SnapshotItem(string path, string sddl)
	{
		Path = path;
		Sddl = sddl;
	}
}
