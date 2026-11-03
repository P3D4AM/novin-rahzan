using System;
using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Events;

public class StiPasteDataInViewerEventArgs : EventArgs
{
	public IStiViewerControl Viewer { get; }

	public Point Pos { get; }

	public StiComponent Component { get; }

	public StiPasteDataInViewerEventArgs(IStiViewerControl viewer, Point pos, StiComponent comp)
	{
		Viewer = viewer;
		Pos = pos;
		Component = comp;
	}
}
