using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMark7Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiTickMarkBase stiTickMarkBase = element as StiTickMarkBase;
		PointF[] array = new PointF[3];
		float x = rect.Left + rect.Width / 2f;
		array[0] = new PointF(rect.Left, rect.Top);
		array[1] = new PointF(rect.Right, rect.Top);
		array[2] = new PointF(x, rect.Bottom);
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, array[0], stiTickMarkBase.Brush, stiTickMarkBase.BorderBrush, stiTickMarkBase.BorderWidth);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}
}
