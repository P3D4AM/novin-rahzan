using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiNeedleIndicator2Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		StiNeedle stiNeedle = element as StiNeedle;
		float num = Math.Min(rect.Width, rect.Height);
		float num2 = num / 3f;
		RectangleF rect2 = new RectangleF(rect.Left + 1f, rect.Top + (rect.Height - num2) / 2f, rect.Width - 1f, num2);
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect2, rect2.Location, stiNeedle.Brush, stiNeedle.BorderBrush, stiNeedle.BorderWidth);
		num2 = num / 7f;
		PointF[] array = new PointF[5];
		array[0] = rect2.Location;
		array[1] = new PointF(rect2.Right, rect2.Top + (rect2.Height - num2) / 2f);
		array[2] = new PointF(rect2.Right, rect2.Bottom - (rect2.Height - num2) / 2f);
		array[3] = new PointF(rect2.Left, rect2.Bottom);
		array[4] = array[0];
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		bool flag = angle.HasValue && centerPoint.HasValue;
		if (flag)
		{
			context.AddPushMatrixGaugeGeom(angle.Value, centerPoint.Value);
		}
		if (element.Animation != null)
		{
			stiGraphicsPathGaugeGeom.Animation = element.Animation;
		}
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
		RectangleF rect3 = new RectangleF(rect.Location, new SizeF(num, num));
		StiSolidBrush background = new StiSolidBrush(Color.FromArgb(175, 175, 175));
		context.AddEllipseGaugeGeom(rect3, (StiBrush)(object)background, stiNeedle.BorderBrush, 1.5f);
		float num3 = num * 0.15f;
		RectangleF rect4 = new RectangleF(rect3.X + num3, rect3.Top + num3, num - num3 * 2f, num - num3 * 2f);
		context.AddEllipseGaugeGeom(rect4, (StiBrush)new StiEmptyBrush(), (StiBrush)new StiSolidBrush(Color.White), 2f);
		if (flag)
		{
			context.AddPopTranformGaugeGeom();
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
			context.AddTextGaugeGeom(string.Format(stiNeedle.Format, stiNeedle.ValueObj), font, stiNeedle.TextBrush, new RectangleF(rect3.Location, size), sf);
		}
	}
}
