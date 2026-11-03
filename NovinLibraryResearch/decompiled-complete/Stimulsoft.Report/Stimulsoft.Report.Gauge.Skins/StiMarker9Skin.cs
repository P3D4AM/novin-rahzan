using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker9Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiMarkerBase indicator = element as StiMarkerBase;
		float bottom = rect.Bottom;
		float right = rect.Right;
		float y = bottom - rect.Height / 5f;
		float num = rect.Width / 3f;
		AddLines(context, indicator, new PointF[7]
		{
			new PointF(rect.Left, rect.Top),
			new PointF(right, rect.Top),
			new PointF(right, y),
			new PointF(right - num, y),
			new PointF(rect.Left + rect.Width / 2f, bottom),
			new PointF(rect.Left + num, y),
			new PointF(rect.Left, y)
		}, rect, angle, centerPoint, null, element.Animation);
	}
}
