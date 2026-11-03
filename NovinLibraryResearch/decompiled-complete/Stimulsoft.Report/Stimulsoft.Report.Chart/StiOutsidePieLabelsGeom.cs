using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiOutsidePieLabelsGeom : StiCenterPieLabelsGeom
{
	public Color LineColor { get; }

	public PointF LabelPoint { get; }

	public PointF StartPoint { get; }

	public override void Draw(StiContext context)
	{
		base.Draw(context);
		IStiOutsidePieLabels stiOutsidePieLabels = base.SeriesLabels as IStiOutsidePieLabels;
		if (stiOutsidePieLabels.DrawBorder)
		{
			StiPenGeom pen = new StiPenGeom(LineColor);
			StiChart stiChart = base.Series.Chart as StiChart;
			context.PushSmoothingModeToAntiAlias();
			if (stiChart.IsAnimation)
			{
				StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
				context.DrawAnimationLines(pen, new PointF[2] { LabelPoint, StartPoint }, animation);
			}
			else
			{
				context.DrawLine(pen, LabelPoint.X, LabelPoint.Y, StartPoint.X, StartPoint.Y);
			}
			context.PopSmoothingMode();
		}
	}

	public StiOutsidePieLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, StiBrush seriesBrush, StiBrush labelBrush, StiBrush seriesLabelsBrush, Color borderColor, Color seriesBorderColor, StiRotationMode rotationMode, RectangleF labelRect, float angleToUse, Color lineColor, PointF labelPoint, PointF startPoint)
		: base(seriesLabels, series, index, value, clientRectangle, text, seriesBrush, labelBrush, seriesLabelsBrush, borderColor, seriesBorderColor, rotationMode, labelRect, angleToUse, null)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		LineColor = lineColor;
		LabelPoint = labelPoint;
		StartPoint = startPoint;
	}
}
