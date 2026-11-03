using System;

namespace Stimulsoft.Report.Design;

public class StiOpenRecentFileObjectEventArgs : EventArgs
{
	public string FileName => RecentFile.FileName;

	public object Tag => RecentFile.Tag;

	public StiRecentFile RecentFile { get; }

	public bool Processed { get; set; } = true;


	public StiOpenRecentFileObjectEventArgs(StiRecentFile recentFile)
	{
		RecentFile = recentFile;
	}
}
