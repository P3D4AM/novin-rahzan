using Stimulsoft.Base.Context;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Viewer.Elements.Chart;

internal class StiChartInteractionResult
{
	internal string ToolTipString { get; set; }

	internal string HyperlinkString { get; set; }

	internal StiSeriesInteractionData InteractionData { get; set; }

	internal IStiGeomInteraction Geom { get; set; }
}
