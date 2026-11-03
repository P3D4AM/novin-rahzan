using System;

namespace Stimulsoft.Report;

public class StiGetSubReportEventArgs : EventArgs
{
	public StiReport Report { get; set; }

	public string SubReportName { get; }

	public StiGetSubReportEventArgs(string subReportName)
	{
		SubReportName = subReportName;
	}
}
