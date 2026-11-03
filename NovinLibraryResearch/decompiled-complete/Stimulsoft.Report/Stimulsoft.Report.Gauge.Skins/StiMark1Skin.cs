using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMark1Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiTickMarkBase stiTickMarkBase = element as StiTickMarkBase;
		context.AddRectangleGaugeGeom(rect, stiTickMarkBase.Brush, stiTickMarkBase.BorderBrush, stiTickMarkBase.BorderWidth);
	}
}
