using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker13Skin : StiMarkerBaseSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		StiMarkerBase indicator = element as StiMarkerBase;
		float bottom = rect.Bottom;
		float right = rect.Right;
		float x = right - rect.Width / 3f;
		float y = rect.Top + rect.Height / 2f;
		PointF[] points = new PointF[5]
		{
			new PointF(rect.Left, rect.Top),
			new PointF(x, rect.Top),
			new PointF(right, y),
			new PointF(x, bottom),
			new PointF(rect.Left, bottom)
		};
		StringFormat sf = new StringFormat
		{
			Alignment = (StringAlignment)0,
			LineAlignment = (StringAlignment)1
		};
		AddLines(context, indicator, points, rect, angle, centerPoint, sf, element.Animation);
	}
}
