using System;
using System.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiNeedleIndicator4Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		StiNeedle stiNeedle = element as StiNeedle;
		float num = Math.Min(rect.Width, rect.Height);
		PointF location = new PointF(rect.Location.X + num / 2f - stiNeedle.OffsetNeedle * rect.Width, rect.Location.Y + rect.Height / 2f - num / 2f);
		RectangleF rect2 = new RectangleF(location, new SizeF(rect.Width - num / 2f, num));
		PointF[] array = new PointF[5];
		array[0] = new PointF(rect2.X, rect2.Y + rect2.Height / 2f - rect2.Height * stiNeedle.EndWidth / 2f);
		array[1] = new PointF(rect2.Right, rect2.Y + rect2.Height / 2f - rect2.Height * stiNeedle.StartWidth / 2f);
		array[2] = new PointF(rect2.Right, rect2.Y + rect2.Height / 2f + rect2.Height * stiNeedle.StartWidth / 2f);
		array[3] = new PointF(rect2.X, rect2.Y + rect2.Height / 2f + rect2.Height * stiNeedle.EndWidth / 2f);
		array[4] = array[0];
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect2, array[0], stiNeedle.Brush, stiNeedle.BorderBrush, stiNeedle.BorderWidth);
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
		PointF location2 = new PointF(rect.Location.X, rect.Location.Y + rect.Height / 2f - num / 2f);
		RectangleF rect3 = new RectangleF(location2, new SizeF(num, num));
		context.AddEllipseGaugeGeom(rect3, stiNeedle.CapBrush, stiNeedle.CapBorderBrush, stiNeedle.CapBorderWidth);
		if (flag)
		{
			context.AddPopTranformGaugeGeom();
		}
		float? actualValue = stiNeedle.GetActualValue();
		if (stiNeedle.ShowValue && actualValue.HasValue)
		{
			StringFormat sf = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1,
				FormatFlags = (StringFormatFlags)4096
			};
			string text = string.Format(stiNeedle.Format, actualValue.GetValueOrDefault());
			Font font = StiGaugeContextPainter.ChangeFontSize(stiNeedle.Font, context.Zoom);
			SizeF size = context.MeasureString(text, font);
			context.AddTextGaugeGeom(text, font, stiNeedle.TextBrush, new RectangleF(rect3.Location, size), sf);
		}
	}
}
