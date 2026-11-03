using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendRangeMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		StiRangeSeries stiRangeSeries = series as StiRangeSeries;
		IStiMarker marker = stiRangeSeries.Marker;
		StiPenStyle lineStyle = stiRangeSeries.LineStyle;
		IStiLineMarker lineMarker = stiRangeSeries.LineMarker;
		float lineWidth = stiRangeSeries.LineWidth;
		Color lineColor = stiRangeSeries.LineColor;
		bool lighting = stiRangeSeries.Lighting;
		StiBrush brush = stiRangeSeries.Brush;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		float num = rect.Width / 5f;
		float num2 = rect.Height / 5f;
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 3f, rect.X, rect.Y + num2 * 3f));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 3f, rect.X + num * 2f, rect.Y));
		list.Add(new StiLineSegmentGeom(rect.X + num * 2f, rect.Y, rect.X + num * 4f, rect.Y + num2 * 2f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 4f, rect.Y + num2 * 2f, rect.X + num * 5f, rect.Y + num2 * 1f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 1f, rect.X + num * 5f, rect.Y + num2 * 3f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 5f, rect.Y + num2 * 3f, rect.X + num * 4f, rect.Y + num2 * 4f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 4f, rect.Y + num2 * 4f, rect.X + num * 2f, rect.Y + num2 * 3f));
		list.Add(new StiLineSegmentGeom(rect.X + num * 2f, rect.Y + num2 * 3f, rect.X, rect.Y + num2 * 5f));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Y + num2 * 5f, rect.X, rect.Y + num2 * 3f));
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(brush, list, rect, null, index);
		context.DrawPath(pen, list, rect);
		context.PopSmoothingMode();
	}
}
