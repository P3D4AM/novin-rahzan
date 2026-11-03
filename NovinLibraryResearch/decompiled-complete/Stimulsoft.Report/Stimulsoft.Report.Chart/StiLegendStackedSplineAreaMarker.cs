using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendStackedSplineAreaMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiStackedSplineAreaSeries stiStackedSplineAreaSeries = series as StiStackedSplineAreaSeries;
		IStiMarker marker = stiStackedSplineAreaSeries.Marker;
		StiPenStyle lineStyle = stiStackedSplineAreaSeries.LineStyle;
		IStiLineMarker lineMarker = stiStackedSplineAreaSeries.LineMarker;
		float lineWidth = stiStackedSplineAreaSeries.LineWidth;
		Color lineColor = stiStackedSplineAreaSeries.LineColor;
		bool lighting = stiStackedSplineAreaSeries.Lighting;
		StiBrush brush = stiStackedSplineAreaSeries.Brush;
		List<StiSegmentGeom> splineAreaMarkerPath = StiLegendMarkerHelper.GetSplineAreaMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		PointF[] splineAreaMarkerLinePoints = StiLegendMarkerHelper.GetSplineAreaMarkerLinePoints(rect);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, splineAreaMarkerPath, rect, null, index);
		context.DrawCurve(pen, splineAreaMarkerLinePoints, stiStackedSplineAreaSeries.Tension);
		context.PopSmoothingMode();
	}
}
