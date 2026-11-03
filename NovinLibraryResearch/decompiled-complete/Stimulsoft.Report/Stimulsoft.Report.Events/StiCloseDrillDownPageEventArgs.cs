using System;

namespace Stimulsoft.Report.Events;

public class StiCloseDrillDownPageEventArgs : EventArgs
{
	public int TabIndex { get; }

	public StiCloseDrillDownPageEventArgs(int tabIndex)
	{
		TabIndex = tabIndex;
	}
}
