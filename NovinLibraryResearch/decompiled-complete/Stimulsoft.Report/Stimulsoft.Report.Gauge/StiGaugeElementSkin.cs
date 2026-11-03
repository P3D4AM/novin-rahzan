using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge;

public abstract class StiGaugeElementSkin
{
	public void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect)
	{
		Draw(context, element, rect, null, null);
	}

	public abstract void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint);
}
