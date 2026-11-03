using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendColumnMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Color? colorLegend = ((StiSeries)series).ColorLegend;
			StiBrush brush = (StiBrush)((!colorLegend.HasValue) ? ((object)series.Core.GetSeriesBrush(colorIndex, colorCount)) : ((object)new StiSolidBrush(colorLegend.GetValueOrDefault())));
			StiPenGeom pen = new StiPenGeom((Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount));
			if (series.Chart != null && series.Chart.Style != null)
			{
				series.Chart.Style.Core.FillColumn(context, rect, brush, null, index);
			}
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		catch
		{
		}
	}
}
