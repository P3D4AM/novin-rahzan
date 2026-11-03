using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiLegendFontIconMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiFontIcons? icon = ((IStiFontIconsSeries)series).Icon;
			Color? colorLegend = ((StiSeries)series).ColorLegend;
			StiBrush brush = (StiBrush)((!colorLegend.HasValue) ? ((object)series.Core.GetSeriesBrush(colorIndex, colorCount)) : ((object)new StiSolidBrush(colorLegend.GetValueOrDefault())));
			StiFontIconsExHelper.DrawDirectionIcons(context, brush, rect, new SizeF(rect.Height, rect.Height), icon.GetValueOrDefault(), null, verticalDirection: true);
		}
		catch
		{
		}
	}
}
