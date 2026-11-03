using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiAllowClipboardOperationEventArgs : EventArgs
{
	public StiComponentsCollection Components { get; }

	public virtual bool Allow { get; set; } = true;


	public StiAllowClipboardOperationEventArgs(StiComponentsCollection comps)
	{
		Components = comps;
	}
}
