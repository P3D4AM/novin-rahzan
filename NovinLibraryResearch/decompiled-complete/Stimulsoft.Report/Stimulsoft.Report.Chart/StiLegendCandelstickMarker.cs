using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendCandelstickMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries serie, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		IStiCandlestickSeries stiCandlestickSeries = serie as IStiCandlestickSeries;
		StiBrush brush = stiCandlestickSeries.Brush;
		Color borderColor = stiCandlestickSeries.BorderColor;
		StiPenGeom pen = new StiPenGeom(borderColor);
		context.PushSmoothingModeToAntiAlias();
		PointF pointF = new PointF(rect.Left + rect.Width / 4f, rect.Top + rect.Height / 4f);
		float width = rect.Width / 2f;
		float height = rect.Height / 2f;
		context.FillRectangle(brush, pointF.X, pointF.Y, width, height, null, index);
		context.DrawRectangle(pen, pointF.X, pointF.Y, width, height);
		context.DrawLine(pen, rect.X + rect.Width / 2f, rect.Y, rect.X + rect.Width / 2f, rect.Top + rect.Height / 4f);
		context.DrawLine(pen, rect.X + rect.Width / 2f, rect.Bottom, rect.X + rect.Width / 2f, rect.Bottom - rect.Height / 4f);
		context.PopSmoothingMode();
	}
}
