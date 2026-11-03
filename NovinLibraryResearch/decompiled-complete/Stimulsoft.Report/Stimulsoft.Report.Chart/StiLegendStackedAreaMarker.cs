using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendStackedAreaMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiStackedAreaSeries stiStackedAreaSeries = series as StiStackedAreaSeries;
		IStiMarker marker = stiStackedAreaSeries.Marker;
		StiPenStyle lineStyle = stiStackedAreaSeries.LineStyle;
		IStiLineMarker lineMarker = stiStackedAreaSeries.LineMarker;
		float lineWidth = stiStackedAreaSeries.LineWidth;
		Color lineColor = stiStackedAreaSeries.LineColor;
		bool lighting = stiStackedAreaSeries.Lighting;
		StiBrush brush = stiStackedAreaSeries.Brush;
		List<StiSegmentGeom> areaMarkerPath = StiLegendMarkerHelper.GetAreaMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, areaMarkerPath, rect, null, index);
		PointF[] areaMarkerLinePoints = StiLegendMarkerHelper.GetAreaMarkerLinePoints(rect);
		context.DrawLines(pen, areaMarkerLinePoints);
		context.PopSmoothingMode();
	}
}
