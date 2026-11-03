using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker14Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiMarkerBase indicator = element as StiMarkerBase;
		float bottom = rect.Bottom;
		float right = rect.Right;
		float num = rect.Height / 3f;
		float x = rect.Left + rect.Width / 5f;
		AddLines(context, indicator, new PointF[7]
		{
			new PointF(x, bottom),
			new PointF(x, bottom - num),
			new PointF(rect.Left, rect.Top + rect.Height / 2f),
			new PointF(x, rect.Top + num),
			new PointF(x, rect.Top),
			new PointF(right, rect.Top),
			new PointF(right, bottom)
		}, rect, angle, centerPoint, null, element.Animation);
	}
}
