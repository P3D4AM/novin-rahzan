using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendPieMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries serie, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		IStiPieSeries stiPieSeries = serie as IStiPieSeries;
		StiBrush brush = stiPieSeries.Brush;
		if (stiPieSeries.AllowApplyBrush)
		{
			brush = stiPieSeries.Core.GetSeriesBrush(colorIndex, colorCount);
			brush = stiPieSeries.ProcessSeriesBrushes(index, brush);
		}
		Color color = stiPieSeries.BorderColor;
		if (stiPieSeries.AllowApplyBorderColor)
		{
			color = (Color)serie.Core.GetSeriesBorderColor(colorIndex, colorCount);
		}
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		StiPenGeom pen = new StiPenGeom(color);
		context.PushSmoothingModeToAntiAlias();
		list.Add(new StiArcSegmentGeom(new RectangleF(rect.X - rect.Width, rect.Y, rect.Width * 2f, rect.Height * 2f), 270f, 90f));
		list.Add(new StiLineSegmentGeom(rect.Right, rect.Bottom, rect.X, rect.Bottom));
		list.Add(new StiLineSegmentGeom(rect.X, rect.Bottom, rect.X, rect.Y));
		context.FillPath(brush, list, rect, null, index);
		context.DrawPath(pen, list, null);
		context.PopSmoothingMode();
	}
}
