using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public abstract class StiMarkerBaseSkin : StiGaugeElementSkin
{
	protected void AddLines(StiGaugeContextPainter context, StiIndicatorBase indicator, PointF[] points, RectangleF rect, float? angle, PointF? centerPoint, StringFormat sf, StiAnimation animation)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect, points[0], indicator.Brush, indicator.BorderBrush, indicator.BorderWidth);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(points);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		if (animation != null)
		{
			stiGraphicsPathGaugeGeom.Animation = animation;
		}
		if (angle.HasValue && centerPoint.HasValue)
		{
			context.AddPushMatrixGaugeGeom(angle.Value, centerPoint.Value);
			context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
			context.AddPopTranformGaugeGeom();
		}
		else
		{
			context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
		}
		if (indicator is IStiGaugeMarker { ShowValue: not false } stiGaugeMarker)
		{
			if (sf == null)
			{
				sf = new StringFormat
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)1,
					FormatFlags = (StringFormatFlags)4096
				};
			}
			Font font = StiGaugeContextPainter.ChangeFontSize(stiGaugeMarker.Font, context.Zoom);
			context.AddTextGaugeGeom(string.Format(stiGaugeMarker.Format, indicator.ValueObj), font, stiGaugeMarker.TextBrush, rect, sf);
		}
	}
}
