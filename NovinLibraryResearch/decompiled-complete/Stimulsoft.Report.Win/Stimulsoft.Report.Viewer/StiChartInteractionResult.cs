using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Viewer;

internal class StiChartInteractionResult
{
	internal string ToolTipString { get; set; }

	internal string HyperlinkString { get; set; }

	internal StiSeriesInteractionData InteractionData { get; set; }
}
