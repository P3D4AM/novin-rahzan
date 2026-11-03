using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendSteppedAreaMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiSteppedAreaSeries stiSteppedAreaSeries = series as StiSteppedAreaSeries;
		IStiMarker marker = stiSteppedAreaSeries.Marker;
		StiPenStyle lineStyle = stiSteppedAreaSeries.LineStyle;
		IStiLineMarker lineMarker = stiSteppedAreaSeries.LineMarker;
		float lineWidth = stiSteppedAreaSeries.LineWidth;
		Color lineColor = stiSteppedAreaSeries.LineColor;
		bool lighting = stiSteppedAreaSeries.Lighting;
		StiBrush brush = stiSteppedAreaSeries.Brush;
		List<StiSegmentGeom> steppedMarkerPath = StiLegendMarkerHelper.GetSteppedMarkerPath(rect);
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, steppedMarkerPath, rect, null, index);
		context.DrawPath(pen, steppedMarkerPath, rect);
		context.PopSmoothingMode();
	}
}
