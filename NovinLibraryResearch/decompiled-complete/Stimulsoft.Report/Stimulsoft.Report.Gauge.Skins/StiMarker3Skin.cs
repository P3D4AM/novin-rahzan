using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker3Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		StiMarkerBase indicator = element as StiMarkerBase;
		PointF[] points = new PointF[3]
		{
			new PointF(rect.Left, rect.Bottom),
			new PointF(rect.Left + rect.Width / 2f, rect.Top),
			new PointF(rect.Right, rect.Bottom)
		};
		StringFormat sf = new StringFormat
		{
			Alignment = (StringAlignment)1,
			LineAlignment = (StringAlignment)2
		};
		AddLines(context, indicator, points, rect, angle, centerPoint, sf, element.Animation);
	}
}
