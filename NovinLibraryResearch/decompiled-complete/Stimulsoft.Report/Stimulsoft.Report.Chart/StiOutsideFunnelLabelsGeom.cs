using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiOutsideFunnelLabelsGeom : StiCenterFunnelLabelsGeom
{
	public PointF StartPointLine { get; }

	public PointF EndPointLine { get; }

	public override void Draw(StiContext context)
	{
		StiPenGeom pen = new StiPenGeom(base.BorderColor);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation)
		{
			StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
			context.DrawAnimationLines(pen, new PointF[2] { StartPointLine, EndPointLine }, animation);
		}
		else
		{
			context.DrawLine(pen, StartPointLine.X, StartPointLine.Y, EndPointLine.X, EndPointLine.Y);
		}
		base.Draw(context);
	}

	public StiOutsideFunnelLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, StiBrush seriesBrush, StiBrush labelBrush, Color borderColor, Color seriesBorderColor, RectangleF labelRect, PointF startPointLine, PointF endPointLine)
		: base(seriesLabels, series, index, value, clientRectangle, text, seriesBrush, labelBrush, borderColor, seriesBorderColor, labelRect, null)
	{
		StartPointLine = startPointLine;
		EndPointLine = endPointLine;
	}
}
