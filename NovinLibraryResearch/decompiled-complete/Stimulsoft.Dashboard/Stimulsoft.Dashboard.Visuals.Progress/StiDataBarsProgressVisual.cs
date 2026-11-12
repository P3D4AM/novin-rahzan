using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Visuals.Progress;

public class StiDataBarsProgressVisual : StiProgressVisual
{
	public override void Draw(StiContext context, RectangleF rect)
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		if (base.Iterations.Count == 0)
		{
			return;
		}
		float num = rect.Width * 0.06f;
		if (num < 4f)
		{
			num = 4f;
		}
		RectangleF clipRect = new RectangleF(rect.X + num / 2f, rect.Y + num / 2f, rect.Width - num, rect.Height - num);
		float num2 = clipRect.Height / (float)base.Iterations.Count;
		if (num2 < (float)(int)MinElementSide)
		{
			num2 = (int)MinElementSide;
		}
		float num3 = ((base.Iterations.Count > 1) ? (num2 * 0.95f) : num2);
		float width = clipRect.Width;
		List<string> textValues = GetTextValues();
		List<decimal> targetValues = GetTargetValues();
		Font font = base.Element.Font;
		float? num4 = null;
		Font val = new Font(font.FontFamily.Name, 1f, font.Style, font.Unit);
		try
		{
			for (int i = 0; i < base.Iterations.Count; i++)
			{
				string text = base.Iterations[i].Series + " " + textValues[i];
				RectangleF rect2 = new RectangleF(clipRect.X, clipRect.Y + num2 * (float)i + num3 * 0.07f, width, num3 * 0.86f);
				float fontSize = StiStringFormatHelper.GetFontSize(context, rect2, text, val);
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
		float num5 = base.Element.Font.Size / 18f * num4.GetValueOrDefault();
		if (num5 > num4.GetValueOrDefault())
		{
			num5 = num4.GetValueOrDefault();
		}
		Font val2 = new Font(font.FontFamily.Name, num5, font.Style, font.Unit);
		context.PushClip(clipRect);
		float num6 = ((base.Iterations.Count > 1) ? (num2 * 0.05f / (float)(base.Iterations.Count - 1)) : 0f);
		for (int j = 0; j < base.Iterations.Count; j++)
		{
			string text2 = ProcessFontName(val2.FontFamily.Name, base.Iterations[j]);
			FontStyle val3 = ProcessFontStyle(val2, base.Iterations[j]);
			Font font2 = new Font(text2, val2.Size, val3);
			decimal num7 = targetValues[j];
			float y = clipRect.Y + (num2 + num6) * (float)j;
			RectangleF value = new RectangleF(clipRect.X, y, width, num3);
			Color color = ProcessTrackColor(base.Style.TrackColor, base.Iterations[j]);
			context.FillRectangle(color, Rectangle.Round(value));
			decimal num8 = ((num7 != 0m) ? (Math.Abs(base.Iterations[j].Value) / num7) : 0m);
			string text3 = StiTextFormatHelper.FormatAsPercentage(base.Element?.Report, base.Element.TextFormat, num8);
			Color color2 = ProcessForeColor(base.Iterations[j]);
			if (num4.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				StiFontGeom font3 = new StiFontGeom(font2);
				SizeF sizeF = context.MeasureString(text3, font3);
				context.DrawRotatedString(rect: new RectangleF(value.Right - sizeF.Width, value.Top, sizeF.Width, value.Height), text: text3, font: font3, brush: color2, sf: StiStringFormatHelper.GetStringFormatGeom(context), mode: StiRotationMode.CenterCenter, angle: 0f, antialiasing: true);
			}
			string series = base.Iterations[j].Series;
			if (num4.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				StiFontGeom font4 = new StiFontGeom(font2);
				context.DrawRotatedString(rect: new RectangleF(width: context.MeasureString(series, font4).Width, x: value.Left + (float)StiScale.I10, y: value.Top, height: value.Height), text: series, font: font4, brush: color2, sf: StiStringFormatHelper.GetStringFormatGeom(context), mode: StiRotationMode.CenterCenter, angle: 0f, antialiasing: true);
			}
			float num9 = ((num7 != 0m) ? ((float)((decimal)width / num7 * base.Iterations[j].Value)) : 0f);
			if (num9 > value.Width)
			{
				num9 = value.Width;
			}
			RectangleF rectangleF = new RectangleF(clipRect.X, y, num9, num3);
			context.PushClip(rectangleF);
			Color color3 = ProcessColor(GetColor(j), base.Iterations[j]);
			context.FillRectangle(color3, Rectangle.Round(rectangleF));
			if (num4.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				StiFontGeom font5 = new StiFontGeom(font2);
				SizeF sizeF2 = context.MeasureString(text3, font5);
				RectangleF rect5 = new RectangleF(value.Right - sizeF2.Width, value.Top, sizeF2.Width, value.Height);
				Color value2 = (IsColorLight(color3) ? Color.White : color2);
				Color color4 = ProcessForeColor(base.Iterations[j], value2);
				context.DrawRotatedString(text3, font5, color4, rect5, StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
			}
			if (num4.GetValueOrDefault() >= (float)(int)base.MinFontSize)
			{
				StiFontGeom font6 = new StiFontGeom(font2);
				SizeF sizeF3 = context.MeasureString(series, font6);
				RectangleF rect6 = new RectangleF(value.Left + (float)StiScale.I10, value.Top, sizeF3.Width, value.Height);
				Color value3 = (IsColorLight(color3) ? Color.White : color2);
				Color color5 = ProcessForeColor(base.Iterations[j], value3);
				context.DrawRotatedString(series, font6, color5, rect6, StiStringFormatHelper.GetStringFormatGeom(context), StiRotationMode.CenterCenter, 0f, antialiasing: true);
			}
			context.PopClip();
		}
		context.PopClip();
	}

	private bool IsColorLight(Color color)
	{
		return 600 - color.R - color.G - color.B > 0;
	}

	public StiDataBarsProgressVisual(StiProgressElement element, List<StiProgressIteration> iterations)
		: base(element, iterations)
	{
	}
}
