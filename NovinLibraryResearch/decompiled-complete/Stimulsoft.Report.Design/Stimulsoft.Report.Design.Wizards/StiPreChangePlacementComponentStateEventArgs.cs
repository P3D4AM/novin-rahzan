using System;

namespace Stimulsoft.Report.Design.Wizards;

public class StiPreChangePlacementComponentStateEventArgs : EventArgs
{
	public StiPlacementComponentDockStyle DockStyle { get; }

	public StiPreChangePlacementComponentStateEventArgs(StiPlacementComponentDockStyle dockStyle)
	{
		DockStyle = dockStyle;
	}
}
