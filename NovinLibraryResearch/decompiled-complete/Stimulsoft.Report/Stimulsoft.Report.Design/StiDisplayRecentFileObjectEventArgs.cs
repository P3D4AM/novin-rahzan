using System;

namespace Stimulsoft.Report.Design;

public class StiDisplayRecentFileObjectEventArgs : EventArgs
{
	public string FileName { get; set; }

	public string DisplayName { get; set; }

	public bool Visible { get; set; } = true;


	public StiRecentFile RecentFile { get; }

	public StiDisplayRecentFileObjectEventArgs(StiRecentFile recentFile)
	{
		RecentFile = recentFile;
		FileName = recentFile.FileName;
		DisplayName = recentFile.DisplayName;
	}
}
