using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiPastedEventArgs : EventArgs
{
	public StiComponentsCollection Components { get; }

	public StiPastedEventArgs(StiComponent comp)
	{
		Components = new StiComponentsCollection { comp };
	}

	public StiPastedEventArgs(StiComponentsCollection comps)
	{
		Components = comps;
	}
}
