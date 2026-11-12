using System;
using System.Drawing;
using System.Drawing.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiAutoSizeTextHelper
{
	public static void Draw(Graphics g, RectangleD rect, string text, Font font, Color color, StiHorAlignment horAlign, StiVertAlignment vertAlign, StiTextSizeMode mode)
	{
		Draw(g, rect, text, font, new StiSolidBrush(color), horAlign, vertAlign, mode);
	}

	public static void Draw(Graphics g, RectangleD rect, string text, Font font, StiBrush brush, StiHorAlignment horAlign, StiVertAlignment vertAlign, StiTextSizeMode mode)
	{
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		SizeD sizeD = Measure(g, text, font);
		StiTextHorAlignment stiTextHorAlignment = ToAlignment(horAlign);
		StiTextOptions defaultTextOptions = StiAutoSizeHtmlTextHelper.GetDefaultTextOptions();
		double num = 1.0;
		defaultTextOptions.WordWrap = mode == StiTextSizeMode.WordWrap;
		defaultTextOptions.Trimming = (StringTrimming)((mode == StiTextSizeMode.Trimming) ? 3 : 0);
		if (mode == StiTextSizeMode.Fit)
		{
			double value = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
			double value2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
			num = Math.Min(Math.Abs(value), Math.Abs(value2));
			double num2 = rect.X;
			double num3 = rect.Y;
			double num4 = rect.Width;
			double num5 = rect.Height;
			if (num < 1.0)
			{
				switch (stiTextHorAlignment)
				{
				case StiTextHorAlignment.Left:
				case StiTextHorAlignment.Width:
					num4 = sizeD.Width * num;
					break;
				case StiTextHorAlignment.Center:
					num2 += (num4 - sizeD.Width * num) / 2.0;
					num4 = sizeD.Width * num;
					break;
				case StiTextHorAlignment.Right:
					num2 += num4 - sizeD.Width * num;
					num4 = sizeD.Width * num;
					break;
				}
				switch (vertAlign)
				{
				case StiVertAlignment.Top:
					num5 = sizeD.Height * num;
					break;
				case StiVertAlignment.Center:
					num3 += (num5 - sizeD.Height * num) / 2.0;
					num5 = sizeD.Height * num;
					break;
				case StiVertAlignment.Bottom:
					num3 += num5 - sizeD.Height * num;
					num5 = sizeD.Height * num;
					break;
				}
			}
			rect = new RectangleD(num2, num3, num4, num5);
		}
		StiBrush brush2 = brush ?? new StiSolidBrush(Color.DimGray);
		Brush brush3 = StiBrush.GetBrush(brush2, rect);
		try
		{
			Font val = StiFontUtils.ChangeFontSize(font, (float)((double)font.Size * num));
			try
			{
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				g.TextRenderingHint = (TextRenderingHint)4;
				StiTextDrawing.DrawString(g, text, val, brush3, rect, defaultTextOptions, stiTextHorAlignment, StiVertAlignment.Center, antialiasing: true, (float)num);
				g.TextRenderingHint = textRenderingHint;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)brush3)?.Dispose();
		}
	}

	internal static StiTextHorAlignment ToAlignment(StiHorAlignment alignment)
	{
		return alignment switch
		{
			StiHorAlignment.Left => StiTextHorAlignment.Left, 
			StiHorAlignment.Center => StiTextHorAlignment.Center, 
			StiHorAlignment.Right => StiTextHorAlignment.Right, 
			_ => throw new StiTypeNotRecognizedException(alignment), 
		};
	}

	internal static SizeD Measure(Graphics g, string text, Font font)
	{
		SizeF sizeF = StiTextDrawing.MeasureString(g, text, font);
		return new SizeD(sizeF.Width, sizeF.Height);
	}

	internal static SizeD Measure(Graphics g, string text, Font font, double width)
	{
		StiTextOptions stiTextOptions = new StiTextOptions();
		stiTextOptions.Angle = 0f;
		stiTextOptions.WordWrap = true;
		return StiTextDrawing.MeasureString(g, text, font, width, stiTextOptions, StiTextHorAlignment.Center, StiVertAlignment.Center, antialiasing: false);
	}

	internal static float MeasureFontSize(Graphics g, string text, RectangleD rect, Font font)
	{
		SizeD sizeD = Measure(g, text, font);
		double value = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
		double value2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
		double num = Math.Min(Math.Abs(value), Math.Abs(value2));
		return (float)((double)font.Size * num);
	}
}
