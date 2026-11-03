using System.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiState2Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		StiStateIndicator stiStateIndicator = element as StiStateIndicator;
		context.AddRectangleGaugeGeom(rect, stiStateIndicator.Brush, stiStateIndicator.BorderBrush, stiStateIndicator.BorderWidth);
		if (stiStateIndicator.ShowValue)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			Font font = StiGaugeContextPainter.ChangeFontSize(stiStateIndicator.Font, context.Zoom);
			context.AddTextGaugeGeom(string.Format(stiStateIndicator.Format, stiStateIndicator.ValueObj), font, stiStateIndicator.TextBrush, rect, sf);
		}
	}
}
