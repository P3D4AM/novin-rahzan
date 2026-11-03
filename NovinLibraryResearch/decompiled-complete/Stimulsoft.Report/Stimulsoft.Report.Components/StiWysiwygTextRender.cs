using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public static class StiWysiwygTextRender
{
	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Invalid comparison between Unknown and I4
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Invalid comparison between Unknown and I4
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		if (!StiDpiHelper.IsWindows)
		{
			StiTypographicTextRender.DrawString(g, rect, textBox.CheckAllowHtmlTags() ? StiTextRenderer.GetPlainTextFromHtmlTags(text) : text, textBox);
			return;
		}
		if (!StiOptions.Engine.UseOldWYSIWYGTextQuality)
		{
			StiDpiHelper.CheckWysiwygScaling();
			double value = ((StiDpiHelper.NeedFontScaling && textBox.IsPrinting) ? 1.0 : (1.0 / StiDpiHelper.GraphicsScale));
			if (StiScale.IsNoScaling && (textBox.IsPrinting || !StiDpiHelper.NeedFontScaling))
			{
				value = 1.0 / StiDpiHelper.GraphicsScale;
			}
			StiTextRenderer.DrawText(g, text, textBox.Font, rect, StiBrush.ToColor(textBox.TextBrush), Color.Transparent, (double)(textBox.LineSpacing * StiOptions.Engine.TextLineSpacingScale), textBox.HorAlignment, textBox.VertAlignment, textBox.WordWrap, textBox.TextOptions.RightToLeft, textBox.Page.Zoom, (double)textBox.Angle, textBox.TextOptions.Trimming, textBox.TextOptions.LineLimit, textBox.CheckAllowHtmlTags(), textBox.TextOptions, (double?)value);
			return;
		}
		if (textBox.Page.Zoom < 0.10000000149011612)
		{
			StiStandardTextRenderer.DrawString(g, rect, text, textBox);
			return;
		}
		try
		{
			Bitmap val = new Bitmap(1, 1);
			Metafile val3;
			try
			{
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				try
				{
					IntPtr hdc = val2.GetHdc();
					val3 = new Metafile(hdc, (EmfType)3);
					val2.ReleaseHdc(hdc);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			float num = (float)(textBox.Page.Zoom * StiDpiHelper.GraphicsScale * StiScale.X);
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			try
			{
				val4.PageUnit = (GraphicsUnit)2;
				RectangleD rect2 = default(RectangleD);
				((RectangleD)(ref rect2))._002Ector(0.0, 0.0, ((RectangleD)(ref rect)).Width / (double)num, ((RectangleD)(ref rect)).Height / (double)num);
				StiStandardTextRenderer.DrawString(val4, rect2, text, textBox, 1f);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			StiHorAlignment val5 = (StiHorAlignment)1;
			if ((int)textBox.HorAlignment == 1)
			{
				val5 = (StiHorAlignment)2;
			}
			if ((int)textBox.HorAlignment == 2)
			{
				val5 = (StiHorAlignment)3;
			}
			GraphicsUnit val6 = (GraphicsUnit)2;
			SizeF size = ((Image)val3).GetBounds(ref val6).Size;
			size.Width += 2f;
			size.Height += 2f;
			SizeD val7 = default(SizeD);
			((SizeD)(ref val7))._002Ector((double)(size.Width * num), (double)(size.Height * num));
			RectangleD val8 = StiRectangleUtils.AlignSizeInRect(rect, val7, val5, textBox.VertAlignment);
			if (((RectangleD)(ref val8)).Width > 1.0 && ((RectangleD)(ref val8)).Height > 1.0)
			{
				RectangleF rectangleF = ((RectangleD)(ref val8)).ToRectangleF();
				rectangleF.Height--;
				rectangleF.Width--;
				RectangleF clipBounds = g.ClipBounds;
				g.SetClip(((RectangleD)(ref rect)).ToRectangleF());
				g.DrawImage((Image)(object)val3, rectangleF);
				g.SetClip(clipBounds);
			}
			((Image)val3).Dispose();
		}
		catch
		{
		}
	}

	public static SizeD MeasureString(double width, Font font, StiText textBox)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Engine.UseNewHtmlEngine && textBox.CheckAllowHtmlTags())
		{
			SizeD result;
			if (textBox.Angle == 90f || textBox.Angle == 270f)
			{
				result = StiHtmlTextRender.MeasureString(textBox);
				double width2 = ((SizeD)(ref result)).Width;
				((SizeD)(ref result)).Width = ((SizeD)(ref result)).Height;
				((SizeD)(ref result)).Height = width2;
			}
			else
			{
				result = StiHtmlTextRender.MeasureString(textBox);
			}
			return result;
		}
		if (!StiDpiHelper.IsWindows)
		{
			if (textBox.CheckAllowHtmlTags())
			{
				string text = textBox.Text;
				textBox.Text = StiTextRenderer.GetPlainTextFromHtmlTags(text);
				SizeD result2 = StiTypographicTextRender.MeasureString(width, font, textBox);
				textBox.Text = text;
				return result2;
			}
			return StiTypographicTextRender.MeasureString(width, font, textBox);
		}
		if (!StiOptions.Engine.UseOldWYSIWYGTextQuality)
		{
			bool flag = textBox.WordWrap || (int)textBox.HorAlignment == 3;
			if (textBox.AutoWidth && !flag)
			{
				width = 10000000.0;
			}
			float num = textBox.Angle % 360f;
			if (num < 0f)
			{
				num = 360f + num;
			}
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(0.0, 0.0, width, 10000000.0);
			if ((num > 45f && num < 135f) || (num > 225f && num < 315f))
			{
				((RectangleD)(ref val))._002Ector(0.0, 0.0, 10000000.0, width);
			}
			SizeD val2 = StiTextRenderer.MeasureText(textBox.GetMeasureGraphics(), (string)textBox.Text, font, val, (double)(textBox.LineSpacing * StiOptions.Engine.TextLineSpacingScale), flag, textBox.TextOptions.RightToLeft, 1.0, (double)num, textBox.TextOptions.Trimming, textBox.TextOptions.LineLimit, textBox.CheckAllowHtmlTags(), textBox.TextOptions, (double?)1.0);
			if (textBox.Report != null && textBox.Report.ReportUnit != StiReportUnitType.HundredthsOfInch)
			{
				StiUnit unit = textBox.Report.Unit;
				double num2 = ((SizeD)(ref val2)).Width + textBox.Margins.Left + textBox.Margins.Right + textBox.Border.Size;
				if (unit.ConvertToHInches(Math.Round(unit.ConvertFromHInches(num2), 2)) < num2)
				{
					((SizeD)(ref val2)).Width = ((SizeD)(ref val2)).Width + unit.ConvertToHInches(0.01);
				}
			}
			if (num == 90f || num == 180f)
			{
				((SizeD)(ref val2)).Width = ((SizeD)(ref val2)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWysiwyg;
			}
			else
			{
				((SizeD)(ref val2)).Height = ((SizeD)(ref val2)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWysiwyg;
			}
			return new SizeD(((SizeD)(ref val2)).Width, ((SizeD)(ref val2)).Height);
		}
		lock (textBox.GetMeasureGraphics())
		{
			SizeD result3 = StiTextDrawing.MeasureString(textBox.GetMeasureGraphics(), (string)textBox.Text, font, width, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, false, 1.0);
			((SizeD)(ref result3)).Height = ((SizeD)(ref result3)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorStandard;
			return result3;
		}
	}
}
