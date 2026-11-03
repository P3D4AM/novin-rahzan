using System;

namespace Stimulsoft.Report.Events;

public class StiGetDrillDownReportEventArgs : EventArgs
{
	public StiReport Report { get; set; }

	public virtual bool Cancel { get; set; }
}
