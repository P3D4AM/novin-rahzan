using System;

namespace Stimulsoft.Report.Events;

public class StiReportControlUpdateEventArgs : EventArgs
{
	public string PropertyName { get; set; }

	public StiReportControlUpdateEventArgs(string propertyName)
	{
		PropertyName = propertyName;
	}
}
