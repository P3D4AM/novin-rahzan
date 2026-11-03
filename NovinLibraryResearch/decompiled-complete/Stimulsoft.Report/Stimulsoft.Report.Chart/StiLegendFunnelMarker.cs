using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendFunnelMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries serie, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		IStiFunnelSeries stiFunnelSeries = serie as IStiFunnelSeries;
		StiBrush brush = stiFunnelSeries.Brush;
		if (stiFunnelSeries.AllowApplyBrush)
		{
			brush = stiFunnelSeries.Core.GetSeriesBrush(colorIndex, colorCount);
			brush = stiFunnelSeries.ProcessSeriesBrushes(colorIndex, brush);
		}
		Color color = stiFunnelSeries.BorderColor;
		if (stiFunnelSeries.AllowApplyBorderColor)
		{
			color = (Color)stiFunnelSeries.Core.GetSeriesBorderColor(colorIndex, colorCount);
		}
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		StiPenGeom pen = new StiPenGeom(color);
		context.PushSmoothingModeToAntiAlias();
		PointF[] points = new PointF[7]
		{
			new PointF(rect.X, rect.Y),
			new PointF(rect.Right, rect.Y),
			new PointF(rect.X + rect.Width * 3f / 4f, rect.Top + rect.Height / 2f),
			new PointF(rect.X + rect.Width * 3f / 4f, rect.Bottom),
			new PointF(rect.X + rect.Width * 1f / 4f, rect.Bottom),
			new PointF(rect.X + rect.Width * 1f / 4f, rect.Top + rect.Height / 2f),
			new PointF(rect.X, rect.Y)
		};
		list.Add(new StiLinesSegmentGeom(points));
		list.Add(new StiCloseFigureSegmentGeom());
		context.FillPath(brush, list, rect, null, index);
		context.DrawPath(pen, list, null);
		context.PopSmoothingMode();
	}
}
