using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiNeedleIndicator3Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		StiNeedle stiNeedle = element as StiNeedle;
		float num = Math.Min(rect.Width, rect.Height);
		float num2 = num * 0.6f;
		RectangleF rect2 = new RectangleF(rect.Left + num / 2f, rect.Top + (rect.Height - num2) / 2f, rect.Width - num / 2f, num2);
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect2, rect2.Location, stiNeedle.Brush, stiNeedle.BorderBrush, stiNeedle.BorderWidth);
		float num3 = rect2.Width * 0.111f;
		float num4 = rect2.Height * 0.15f;
		PointF[] array = new PointF[6];
		array[0] = rect2.Location;
		array[1] = new PointF(rect2.Right - num3, rect2.Top + num4);
		array[2] = new PointF(rect2.Right, rect2.Top + rect2.Height / 2f);
		array[3] = new PointF(rect2.Right - num3, rect2.Bottom - num4);
		array[4] = new PointF(rect2.Left, rect2.Bottom);
		array[5] = array[0];
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		bool flag = angle.HasValue && centerPoint.HasValue;
		if (flag)
		{
			context.AddPushMatrixGaugeGeom(angle.Value, centerPoint.Value);
		}
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
		RectangleF rect3 = new RectangleF(rect.Location, new SizeF(num, num));
		StiGradientBrush background = new StiGradientBrush(Color.FromArgb(230, 229, 229), Color.FromArgb(80, 80, 80), 45.0);
		StiSolidBrush borderBrush = new StiSolidBrush(Color.FromArgb(98, 94, 90));
		context.AddEllipseGaugeGeom(rect3, (StiBrush)(object)background, (StiBrush)(object)borderBrush, 1.5f);
		if (flag)
		{
			context.AddPopTranformGaugeGeom();
		}
		if (element.Animation != null)
		{
			stiGraphicsPathGaugeGeom.Animation = element.Animation;
		}
		if (stiNeedle.ShowValue)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			string text = string.Format(stiNeedle.Format, stiNeedle.ValueObj);
			Font font = StiGaugeContextPainter.ChangeFontSize(stiNeedle.Font, context.Zoom);
			SizeF size = context.MeasureString(text, font);
			context.AddTextGaugeGeom(text, font, stiNeedle.TextBrush, new RectangleF(rect3.Location, size), sf);
		}
	}
}
