using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendSteppedRangeMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		StiSteppedRangeSeries stiSteppedRangeSeries = series as StiSteppedRangeSeries;
		Color lineColor = stiSteppedRangeSeries.LineColor;
		StiBrush brush = stiSteppedRangeSeries.Brush;
		List<StiSegmentGeom> steppedMarkerPath = StiLegendMarkerHelper.GetSteppedMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, steppedMarkerPath, rect, null, index);
		context.DrawPath(pen, steppedMarkerPath, rect);
		context.PopSmoothingMode();
	}
}
