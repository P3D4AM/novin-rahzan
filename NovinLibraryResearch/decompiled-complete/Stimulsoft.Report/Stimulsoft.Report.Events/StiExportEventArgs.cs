using System;

namespace Stimulsoft.Report.Events;

public class StiExportEventArgs : EventArgs
{
	public virtual StiExportFormat ExportFormat { get; set; }

	public StiExportEventArgs(StiExportFormat format)
	{
		ExportFormat = format;
	}
}
