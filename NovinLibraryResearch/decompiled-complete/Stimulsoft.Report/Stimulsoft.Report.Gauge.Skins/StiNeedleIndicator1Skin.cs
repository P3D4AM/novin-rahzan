using System;
using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Gauge.Skins;

public class StiNeedleIndicator1Skin : StiGaugeElementSkin
{
	public override void Draw(StiGaugeContextPainter context, StiGaugeElement element, RectangleF rect, float? angle, PointF? centerPoint)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_010b: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Expected O, but got Unknown
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_02e3: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		StiNeedle stiNeedle = element as StiNeedle;
		float num = Math.Min(rect.Width, rect.Height);
		float num2 = num / 4f;
		float height = rect.Height * 0.6f;
		RectangleF rect2 = new RectangleF(rect.Left + num2, rect.Top + rect.Height * 0.2f, rect.Width - num2, height);
		PointF[] array = new PointF[3]
		{
			rect2.Location,
			new PointF(rect2.Right, rect2.Top + rect2.Height / 2f),
			new PointF(rect2.Left, rect2.Bottom)
		};
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rect2, array[0], (StiBrush)new StiGradientBrush(Color.FromArgb(255, 248, 210), Color.FromArgb(0, 255, 248, 210), 0.0), (StiBrush)new StiEmptyBrush(), 0f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		height = rect.Height * 0.4f;
		float num3 = height * 0.4f;
		RectangleF rect3 = new RectangleF(rect.Left + num2, rect.Top + rect.Height * 0.3f, rect.Width - num2, height);
		PointF[] array2 = new PointF[4]
		{
			rect3.Location,
			new PointF(rect3.Right, rect3.Top + num3),
			new PointF(rect3.Right, rect3.Bottom - num3),
			new PointF(rect3.Left, rect3.Bottom)
		};
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom2 = new StiGraphicsPathGaugeGeom(rect3, array2[0], (StiBrush)new StiGradientBrush(Color.FromArgb(34, 34, 34), Color.FromArgb(255, 210, 90), 0.0), (StiBrush)new StiEmptyBrush(), 0f);
		stiGraphicsPathGaugeGeom2.AddGraphicsPathLinesGaugeGeom(array2);
		stiGraphicsPathGaugeGeom2.AddGraphicsPathCloseFigureGaugeGeom();
		bool flag = angle.HasValue && centerPoint.HasValue;
		RectangleF rect4 = new RectangleF(rect.Location, new SizeF(num, num));
		if (flag)
		{
			context.AddPushMatrixGaugeGeom(angle.Value, centerPoint.Value);
		}
		if (element.Animation != null)
		{
			StiAnimation animation2 = (stiGraphicsPathGaugeGeom2.Animation = element.Animation);
			stiGraphicsPathGaugeGeom.Animation = animation2;
		}
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom2);
		context.AddEllipseGaugeGeom(rect4, (StiBrush)new StiGradientBrush(Color.FromArgb(120, 120, 120), Color.FromArgb(132, 117, 77), 0.0), (StiBrush)new StiSolidBrush(Color.FromArgb(228, 188, 83)), 1f);
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
			context.AddTextGaugeGeom(string.Format(stiNeedle.Format, stiNeedle.ValueObj), font, stiNeedle.TextBrush, new RectangleF(rect4.Location, size), sf);
		}
	}
}
