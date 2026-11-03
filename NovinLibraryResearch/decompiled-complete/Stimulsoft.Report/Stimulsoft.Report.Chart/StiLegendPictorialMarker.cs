using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendPictorialMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		context.PushSmoothingModeToAntiAlias();
		context.FillEllipse(seriesBrush, rect, null, index);
		context.PopSmoothingMode();
	}
}
