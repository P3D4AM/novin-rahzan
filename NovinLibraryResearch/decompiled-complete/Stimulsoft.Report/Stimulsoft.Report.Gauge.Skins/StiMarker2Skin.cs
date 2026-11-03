using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker2Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiMarkerBase indicator = element as StiMarkerBase;
		float bottom = rect.Bottom;
		float right = rect.Right;
		AddLines(context, indicator, new PointF[4]
		{
			new PointF(rect.Left, rect.Top),
			new PointF(right, rect.Top),
			new PointF(right, bottom),
			new PointF(rect.Left, bottom)
		}, rect, angle, centerPoint, null, element.Animation);
	}
}
