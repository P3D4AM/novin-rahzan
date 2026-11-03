using System.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiState3Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiStateIndicator indicator = element as StiStateIndicator;
		float x = rect.Left + rect.Width / 2f;
		float y = rect.Top + rect.Height / 2f;
		AddLines(context, indicator, new PointF[4]
		{
			new PointF(rect.Left, y),
			new PointF(x, rect.Top),
			new PointF(rect.Right, y),
			new PointF(x, rect.Bottom)
		}, rect, angle, centerPoint, null, element.Animation);
	}
}
