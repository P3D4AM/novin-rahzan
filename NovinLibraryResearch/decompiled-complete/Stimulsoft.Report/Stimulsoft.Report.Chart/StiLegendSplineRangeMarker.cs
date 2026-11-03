using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendSplineRangeMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiSplineRangeSeries stiSplineRangeSeries = series as StiSplineRangeSeries;
		IStiMarker marker = stiSplineRangeSeries.Marker;
		StiPenStyle lineStyle = stiSplineRangeSeries.LineStyle;
		IStiLineMarker lineMarker = stiSplineRangeSeries.LineMarker;
		float lineWidth = stiSplineRangeSeries.LineWidth;
		Color lineColor = stiSplineRangeSeries.LineColor;
		bool lighting = stiSplineRangeSeries.Lighting;
		StiBrush brush = stiSplineRangeSeries.Brush;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 3f));
		PointF[] points = new PointF[4]
		{
			new PointF(rect.X, rect.Y + num2 * 3f),
			new PointF(rect.X + num * 2f, rect.Y),
			new PointF(rect.X + num * 4f, rect.Y + num2 * 2f),
			new PointF(rect.X + num * 5f, rect.Y + num2 * 1f)
		};
		list.Add(new StiCurveSegmentGeom(points, stiSplineRangeSeries.Tension));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 1f, rect.X + num * 5f, rect.Y + num2 * 3f));
		PointF[] points2 = new PointF[4]
		{
			new PointF(rect.X + num * 5f, rect.Y + num2 * 3f),
			new PointF(rect.X + num * 4f, rect.Y + num2 * 4f),
			new PointF(rect.X + num * 2f, rect.Y + num2 * 4f),
			new PointF(rect.X, rect.Y + num2 * 5f)
		};
		list.Add(new StiCurveSegmentGeom(points2, stiSplineRangeSeries.Tension));
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, list, rect, null, index);
		context.DrawCurve(pen, points, stiSplineRangeSeries.Tension);
		context.DrawCurve(pen, points2, stiSplineRangeSeries.Tension);
		context.PopSmoothingMode();
	}
}
