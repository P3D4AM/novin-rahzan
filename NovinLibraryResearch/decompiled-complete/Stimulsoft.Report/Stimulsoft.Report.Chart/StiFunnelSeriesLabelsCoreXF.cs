using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiFunnelSeriesLabelsCoreXF : StiSeriesLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Funnel;

	public abstract StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, int pointIndex, double value, double valueNext, string argumentText, string tag, int colorIndex, int colorCount, RectangleF rect, float singleValueHeight, float singleValueWidth, float centerAxis, out RectangleF measureRect);

	public StiFunnelSeriesLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
