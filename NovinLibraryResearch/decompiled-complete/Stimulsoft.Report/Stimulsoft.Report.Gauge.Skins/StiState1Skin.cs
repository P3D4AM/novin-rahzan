using System.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiState1Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		StiStateIndicator stiStateIndicator = element as StiStateIndicator;
		context.AddEllipseGaugeGeom(rect, stiStateIndicator.Brush, stiStateIndicator.BorderBrush, stiStateIndicator.BorderWidth);
		float? actualValue = stiStateIndicator.GetActualValue();
		if (stiStateIndicator.ShowValue && actualValue.HasValue)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			Font font = StiGaugeContextPainter.ChangeFontSize(stiStateIndicator.Font, context.Zoom);
			context.AddTextGaugeGeom(string.Format(stiStateIndicator.Format, actualValue.GetValueOrDefault()), font, stiStateIndicator.TextBrush, rect, sf);
		}
	}
}
