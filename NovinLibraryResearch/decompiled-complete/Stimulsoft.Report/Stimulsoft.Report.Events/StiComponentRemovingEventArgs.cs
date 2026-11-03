using System;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Events;

public class StiComponentRemovingEventArgs : EventArgs
{
	public IStiDesignerBase Designer { get; }

	public StiComponent Component { get; }

	public StiComponentRemovingEventArgs(IStiDesignerBase designer, StiComponent component)
	{
		Designer = designer;
		Component = component;
	}
}
