using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendAreaMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		StiAreaSeries stiAreaSeries = series as StiAreaSeries;
		Color lineColor = stiAreaSeries.LineColor;
		StiBrush brush = stiAreaSeries.Brush;
		List<StiSegmentGeom> areaMarkerPath = StiLegendMarkerHelper.GetAreaMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, areaMarkerPath, rect, null, index);
		PointF[] areaMarkerLinePoints = StiLegendMarkerHelper.GetAreaMarkerLinePoints(rect);
		context.DrawLines(pen, areaMarkerLinePoints);
		context.PopSmoothingMode();
	}
}
