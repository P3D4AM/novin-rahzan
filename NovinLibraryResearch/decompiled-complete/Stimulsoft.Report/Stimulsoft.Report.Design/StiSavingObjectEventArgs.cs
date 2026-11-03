using System;

namespace Stimulsoft.Report.Design;

public class StiSavingObjectEventArgs : EventArgs
{
	public bool SaveAs { get; }

	public StiSaveEventSource EventSource { get; }

	public bool Processed { get; set; } = true;


	public StiSavingObjectEventArgs(bool saveAs, StiSaveEventSource eventSource)
	{
		SaveAs = saveAs;
		EventSource = eventSource;
	}
}
