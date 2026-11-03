using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendSplineAreaMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiSplineAreaSeries stiSplineAreaSeries = series as StiSplineAreaSeries;
		IStiMarker marker = stiSplineAreaSeries.Marker;
		StiPenStyle lineStyle = stiSplineAreaSeries.LineStyle;
		IStiLineMarker lineMarker = stiSplineAreaSeries.LineMarker;
		float lineWidth = stiSplineAreaSeries.LineWidth;
		Color lineColor = stiSplineAreaSeries.LineColor;
		bool lighting = stiSplineAreaSeries.Lighting;
		StiBrush brush = stiSplineAreaSeries.Brush;
		List<StiSegmentGeom> splineAreaMarkerPath = StiLegendMarkerHelper.GetSplineAreaMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		PointF[] splineAreaMarkerLinePoints = StiLegendMarkerHelper.GetSplineAreaMarkerLinePoints(rect);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, splineAreaMarkerPath, rect, null, index);
		context.DrawCurve(pen, splineAreaMarkerLinePoints, stiSplineAreaSeries.Tension);
		context.PopSmoothingMode();
	}
}
