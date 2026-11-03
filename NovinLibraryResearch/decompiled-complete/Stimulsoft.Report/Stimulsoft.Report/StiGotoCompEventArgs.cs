using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public class StiGotoCompEventArgs : EventArgs
{
	public StiComponent Component { get; set; }

	public StiGotoCompEventArgs(StiComponent component)
	{
		Component = component;
	}
}
