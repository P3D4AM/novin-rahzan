using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMark5Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		StiTickMarkBase stiTickMarkBase = element as StiTickMarkBase;
		PointF[] array = new PointF[3]
		{
			new PointF(rect.Left, rect.Top),
			new PointF(rect.Right, rect.Top + rect.Height / 2f),
			new PointF(rect.Left, rect.Bottom)
		};
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, array[0], stiTickMarkBase.Brush, stiTickMarkBase.BorderBrush, stiTickMarkBase.BorderWidth);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}
}
