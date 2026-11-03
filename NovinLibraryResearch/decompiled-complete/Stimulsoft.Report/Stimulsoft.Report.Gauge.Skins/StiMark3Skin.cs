using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMark3Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiTickMarkBase stiTickMarkBase = element as StiTickMarkBase;
		PointF[] array = new PointF[4];
		float x = rect.Left + rect.Width / 2f;
		float y = rect.Top + rect.Height / 2f;
		array[0] = new PointF(rect.Left, y);
		array[1] = new PointF(x, rect.Top);
		array[2] = new PointF(rect.Right, y);
		array[3] = new PointF(x, rect.Bottom);
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, array[0], stiTickMarkBase.Brush, stiTickMarkBase.BorderBrush, stiTickMarkBase.BorderWidth);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}
}
