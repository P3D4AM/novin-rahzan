using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker6Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		StiMarkerBase indicator = element as StiMarkerBase;
		float y = rect.Bottom - rect.Height / 3f;
		float x = rect.Left + rect.Width / 2f;
		PointF[] points = new PointF[5]
		{
			new PointF(rect.Left, rect.Top),
			new PointF(rect.Right, rect.Top),
			new PointF(rect.Right, y),
			new PointF(x, rect.Bottom),
			new PointF(rect.Left, y)
		};
		StringFormat sf = new StringFormat
		{
			Alignment = (StringAlignment)1,
			LineAlignment = (StringAlignment)0
		};
		AddLines(context, indicator, points, rect, angle, centerPoint, sf, element.Animation);
	}
}
