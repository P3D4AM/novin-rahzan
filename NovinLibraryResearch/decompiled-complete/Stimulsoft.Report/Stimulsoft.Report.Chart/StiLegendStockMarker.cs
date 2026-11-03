using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiLegendStockMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries serie, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		IStiStockSeries stiStockSeries = serie as IStiStockSeries;
		Color lineColor = stiStockSeries.LineColor;
		StiPenGeom pen = new StiPenGeom(lineColor);
		context.PushSmoothingModeToAntiAlias();
		context.DrawLine(pen, rect.Left + rect.Width / 4f, rect.Top + rect.Height / 4f, rect.Left + rect.Width / 2f, rect.Top + rect.Height / 4f);
		context.DrawLine(pen, rect.Left + rect.Width / 2f, rect.Top, rect.Left + rect.Width / 2f, rect.Bottom);
		context.DrawLine(pen, rect.Left + rect.Width / 2f, rect.Bottom - rect.Height / 4f, rect.Right - rect.Width / 4f, rect.Bottom - rect.Height / 4f);
		context.PopSmoothingMode();
	}
}
