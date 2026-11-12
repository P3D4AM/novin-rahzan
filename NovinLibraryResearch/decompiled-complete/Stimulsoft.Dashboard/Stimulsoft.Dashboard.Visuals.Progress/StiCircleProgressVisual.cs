using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Visuals.Progress;

public class StiCircleProgressVisual : StiPieProgressVisual
{
	public override void Draw(StiContext context, RectangleF rectMain)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Expected O, but got Unknown
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		if (base.Iterations.Count == 0)
		{
			return;
		}
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(RectangleD.CreateFromRectangle(rectMain), base.Iterations.Count, base.Element);
		if (locationRectangles.Count < 1)
		{
			return;
		}
		float num = 0.87f;
		RectangleF rectangleF = locationRectangles[0].ToRectangleF();
		float num2 = ((rectangleF.Width < rectangleF.Height) ? rectangleF.Width : rectangleF.Height);
		Font font = base.Element.Font;
		float num3 = GetTitleHeight(num2, font.Size);
		float width = rectangleF.Width;
		List<decimal> targetValues = GetTargetValues();
		List<string> textValues = GetTextValues();
		float? num4 = null;
		Font val = new Font(font.FontFamily.Name, 1f, font.Style, font.Unit);
		try
		{
			foreach (StiProgressIteration item in base.Iterations.Where((StiProgressIteration i) => !string.IsNullOrEmpty(i.Series)))
			{
				float fontSize = StiStringFormatHelper.GetFontSize(context, new RectangleF(0f, 0f, width, num3), item.Series, val);
				if (!num4.HasValue)
				{
					num4 = fontSize;
				}
				else if (num4 > fontSize)
				{
					num4 = fontSize;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (num4.HasValue && num4.GetValueOrDefault() < font.Size)
		{
			num3 = GetTitleHeight(num2, num4.GetValueOrDefault());
		}
		float num5 = (num2 - num3) / 2f * 0.8f;
		float? num6 = null;
		RectangleF rect = new RectangleF(rectangleF.X - num5 * num, rectangleF.Y - num5 * 0.82f, 2f * num5 * num, 2f * num5 * num);
		Font val2 = new Font(base.Element.Font.Name, 1f, (FontStyle)1, (GraphicsUnit)2);
		try
		{
			for (int j = 0; j < base.Iterations.Count; j++)
			{
				float fontSize2 = StiStringFormatHelper.GetFontSize(context, rect, textValues[j], val2);
				if (!num6.HasValue)
				{
					num6 = fontSize2;
				}
				else if (num6 > fontSize2)
				{
					num6 = fontSize2;
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (num4.GetValueOrDefault() < (float)(int)base.MinFontSize)
		{
			num3 = 0f;
			num4 = 0f;
			num5 = num2 / 2f * 0.8f;
		}
		float num7 = 2f * num5;
		float num8 = num5 / 5f;
		float num9 = num5 - num8;
		float num10 = 2f * num9;
		bool flag = false;
		context.PushClip(rectMain);
		for (int k = 0; k < base.Iterations.Count; k++)
		{
			RectangleF rectangleF2 = locationRectangles[k].ToRectangleF();
			if (!flag && (rectangleF2.Right - rectMain.Right > 1f || rectangleF2.Bottom - rectMain.Bottom > 1f))
			{
				flag = true;
			}
			float x = rectangleF2.X + rectangleF2.Width / 2f;
			float y = rectangleF2.Y + num3 + (rectangleF.Height - num3) / 2f;
			PointF centerPie = new PointF(x, y);
			RectangleF rectangleF3 = new RectangleF(centerPie.X - num5, centerPie.Y - num5, num7, num7);
			RectangleF rect2 = new RectangleF(centerPie.X - num9, centerPie.Y - num9, num10, num10);
			Color color = ProcessTrackColor(base.Style.TrackColor, base.Iterations[k]);
			StiPenGeom pen = new StiPenGeom(color, num8);
			context.DrawEllipse(pen, rectangleF3.X + num8 / 2f, rectangleF3.Y + num8 / 2f, rectangleF3.Width - num8, rectangleF3.Height - num8);
			decimal num11 = targetValues[k];
			float num12 = ((num11 != 0m) ? ((float)(360m / num11 * Math.Abs(base.Iterations[k].Value))) : 0f);
			int num13 = -90;
			if (num12 > 360f)
			{
				num12 = 360f;
			}
			List<StiSegmentGeom> list = new List<StiSegmentGeom>();
			list.Add(new StiArcSegmentGeom(rectangleF3, num13, num12));
			list.Add(new StiLineSegmentGeom(GetPoint(centerPie, num5, (float)num13 + num12), GetPoint(centerPie, num9, (float)num13 + num12)));
			list.Add(new StiArcSegmentGeom(rect2, (float)num13 + num12, 0f - num12));
			list.Add(new StiLineSegmentGeom(GetPoint(centerPie, num9, num13), GetPoint(centerPie, num5, num13)));
			Color color2 = ProcessColor(GetColor(k), base.Iterations[k]);
			context.FillPath(color2, list, rectangleF3);
			if (num6.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				RectangleF value = new RectangleF(centerPie.X - num5 * num, centerPie.Y - num5 * 0.82f, 2f * num5 * num, 2f * num5 * num);
				string text = textValues[k];
				Font val3 = new Font(base.Element.Font.Name, num6.GetValueOrDefault(), (FontStyle)1, (GraphicsUnit)2);
				FontStyle val4 = ProcessFontStyle(val3, base.Iterations[k]);
				StiFontGeom font2 = new StiFontGeom(new Font(val3, val4));
				Color color3 = ProcessForeColor(base.Iterations[k]);
				context.DrawRotatedString(text, font2, color3, Rectangle.Round(value), StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
			}
			if (num4.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				RectangleF elementRectText = new RectangleF(centerPie.X - width / 2f, rectangleF3.Top - num3, width, num3);
				Font val5 = new Font(font.FontFamily.Name, num4.GetValueOrDefault(), font.Style, font.Unit);
				string text2 = ProcessFontName(val5.FontFamily.Name, base.Iterations[k]);
				FontStyle val6 = ProcessFontStyle(val5, base.Iterations[k]);
				Font font3 = new Font(text2, val5.Size, val6);
				StiFontGeom font4 = new StiFontGeom(font3);
				StiRotationMode rotationMode = StiRotationMode.CenterCenter;
				PointF point = default(PointF);
				StiStringFormatHelper.MeasureAlignmentParameters(StiHorAlignment.Center, elementRectText, ref rotationMode, ref point);
				Color color4 = ProcessForeColor(base.Iterations[k]);
				context.DrawRotatedString(base.Iterations[k].Series, font4, color4, point, StiStringFormatHelper.GetStringFormatGeom(context), rotationMode, 0f, antialiasing: true);
			}
		}
		if (flag)
		{
			float factor = (float)(StiElementScale.Factor((IStiElement)base.Element) * StiScale.Factor);
			DrawArrowMore(context, base.Element, rectMain, factor);
		}
		context.PopClip();
	}

	private float GetTitleHeight(float elementSide, float size)
	{
		float num = size / 13f;
		if (num > 4f)
		{
			num = 4f;
		}
		return elementSide / 4f * num;
	}

	protected PointF GetPoint(PointF centerPie, float radius, float angle)
	{
		float num = (float)(Math.PI * (double)angle / 180.0);
		return new PointF(centerPie.X + (float)Math.Cos(num) * radius, centerPie.Y + (float)Math.Sin(num) * radius);
	}

	public StiCircleProgressVisual(StiProgressElement element, List<StiProgressIteration> iterations)
		: base(element, iterations)
	{
	}
}
