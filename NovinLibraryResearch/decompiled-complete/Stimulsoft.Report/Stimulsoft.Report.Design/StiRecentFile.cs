namespace Stimulsoft.Report.Design;

public class StiRecentFile
{
	public int Index { get; set; }

	public string FileName { get; set; }

	public string DisplayName { get; set; }

	public object Tag { get; set; }

	public StiRecentFile(string fileName, string displayName, object tag)
	{
		FileName = fileName;
		DisplayName = displayName;
		Tag = tag;
	}
}
