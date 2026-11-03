using System;
using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Events;

public class StiDropFileInViewerEventArgs : EventArgs
{
	public IStiViewerControl Viewer { get; }

	public Point Pos { get; }

	public StiComponent Component { get; }

	public string FileName { get; }

	public StiDropFileInViewerEventArgs(IStiViewerControl viewer, string fileName, Point pos, StiComponent comp)
	{
		Viewer = viewer;
		Pos = pos;
		Component = comp;
		FileName = fileName;
	}
}
