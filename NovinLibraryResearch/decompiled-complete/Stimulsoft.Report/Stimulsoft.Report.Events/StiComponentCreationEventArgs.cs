using System;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Events;

public class StiComponentCreationEventArgs : EventArgs
{
	public IStiDesignerBase Designer { get; }

	public StiComponent Component { get; }

	public StiComponentCreationEventArgs(IStiDesignerBase designer, StiComponent component)
	{
		Designer = designer;
		Component = component;
	}
}
