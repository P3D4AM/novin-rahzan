namespace Microsoft.Win32.TaskScheduler;

public sealed class TaskSnapshot : SnapshotItem
{
	public bool Enabled { get; internal set; } = true;


	public string TaskDefinitionXml { get; internal set; }

	internal TaskSnapshot(string path, string sddl, bool enabled, string xml = null)
		: base(path, sddl)
	{
		Enabled = enabled;
		TaskDefinitionXml = xml;
	}
}
