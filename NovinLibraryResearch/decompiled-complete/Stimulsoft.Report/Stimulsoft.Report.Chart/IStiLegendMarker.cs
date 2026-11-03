using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public interface IStiLegendMarker
{
	void Draw(StiContext context, IStiSeries series, RectangleF rect, int colorIndex, int colorCount, int index);
}
