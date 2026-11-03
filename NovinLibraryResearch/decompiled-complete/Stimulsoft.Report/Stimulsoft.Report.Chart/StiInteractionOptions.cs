using System;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiInteractionOptions
{
	public bool SelectOnClick { get; set; } = true;


	public bool UpdateContext { get; set; }

	public bool RecallEvent { get; set; }

	public TimeSpan RecallTime { get; set; } = TimeSpan.Zero;


	public bool IsRecalled { get; set; }

	public PointF MousePoint { get; set; } = PointF.Empty;


	public bool DragEnabled { get; set; }

	public SizeF DragDelta { get; set; } = SizeF.Empty;


	public string InteractionToolTip { get; set; }

	public string InteractionHyperlink { get; set; }

	public StiSeriesInteractionData SeriesInteractionData { get; set; }
}
