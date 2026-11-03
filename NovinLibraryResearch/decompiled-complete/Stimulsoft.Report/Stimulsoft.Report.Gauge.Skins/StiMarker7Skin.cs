using System.Drawing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiMarker7Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		StiMarkerBase stiMarkerBase = element as StiMarkerBase;
		if (angle.HasValue && centerPoint.HasValue)
		{
			context.AddPushMatrixGaugeGeom(angle.Value, centerPoint.Value);
			context.AddEllipseGaugeGeom(rect, stiMarkerBase.Brush, stiMarkerBase.BorderBrush, stiMarkerBase.BorderWidth);
			context.AddPopTranformGaugeGeom();
		}
		else
		{
			context.AddEllipseGaugeGeom(rect, stiMarkerBase.Brush, stiMarkerBase.BorderBrush, stiMarkerBase.BorderWidth);
		}
		if (stiMarkerBase.ShowValue)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			Font font = StiGaugeContextPainter.ChangeFontSize(stiMarkerBase.Font, context.Zoom);
			context.AddTextGaugeGeom(string.Format(stiMarkerBase.Format, stiMarkerBase.ValueObj), font, stiMarkerBase.TextBrush, rect, sf);
		}
	}
}
