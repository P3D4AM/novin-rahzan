using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public static class StiStandardTextRenderer
{
	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DrawString(g, rect, text, textBox, (float)textBox.Page.Zoom);
	}

	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox, float zoom)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		if (StiScale.IsNoScaling && !textBox.IsPrinting && StiDpiHelper.NeedDeviceCapsScale)
		{
			zoom = (float)((double)zoom * StiDpiHelper.DeviceCapsScale);
		}
		if (StiOptions.Engine.UseGdi32ForTextRendering && textBox.TextOptions.Angle == 0f)
		{
			DrawStringGdi(g, rect, text, textBox, zoom);
			return;
		}
		RectangleD val = rect;
		float fontSize = textBox.Font.Size * zoom;
		if (textBox.Report.IsPrinting)
		{
			if ((int)textBox.Font.Unit != 2 && (int)textBox.Font.Unit != 0)
			{
				fontSize *= StiOptions.Engine.StandardTextQualityScale / 100f;
			}
		}
		else if ((int)textBox.Font.Unit != 2 && (int)textBox.Font.Unit != 0)
		{
			fontSize *= StiOptions.Engine.StandardTextQualityScale / 96f;
		}
		StiViewerFitTextHelper.StiFitTextInfo stiFitTextInfo = null;
		object obj = null;
		string hashSt = null;
		if (textBox.WordWrap && fontSize >= 3f)
		{
			stiFitTextInfo = StiViewerFitTextHelper.GetReportInfo(textBox.Report);
			if (stiFitTextInfo != null)
			{
				obj = stiFitTextInfo.GetFontSizeObject(textBox, rect, text, ref fontSize, ref hashSt);
			}
		}
		Font val2 = StiFontUtils.ChangeFontSize(textBox.Font, fontSize);
		try
		{
			Font val3 = null;
			if (stiFitTextInfo != null && obj == null)
			{
				SizeD val4 = StiTextDrawing.MeasureString(g, text, val2, ((RectangleD)(ref val)).Width, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, false, 1.0);
				float num = val2.Size * 0.2f;
				if (((SizeD)(ref val4)).Height > ((RectangleD)(ref rect)).Height + (double)num)
				{
					while (((SizeD)(ref val4)).Height > ((RectangleD)(ref rect)).Height + (double)num)
					{
						fontSize -= 0.5f;
						if (val3 != null)
						{
							val3.Dispose();
						}
						val3 = StiFontUtils.ChangeFontSize(val2, fontSize);
						val4 = StiTextDrawing.MeasureString(g, text, val3, ((RectangleD)(ref val)).Width, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, false, 1.0);
					}
					stiFitTextInfo.HashText[hashSt] = fontSize;
					stiFitTextInfo.HashComponent[textBox] = fontSize;
				}
				else
				{
					stiFitTextInfo.HashText[hashSt] = false;
					stiFitTextInfo.HashComponent[textBox] = false;
				}
			}
			Brush brush = StiBrush.GetBrush(textBox.TextBrush, ((RectangleD)(ref val)).ToRectangleF());
			try
			{
				StiTextDrawing.DrawString(g, text, val3 ?? val2, brush, val, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, false, (float)textBox.Page.Zoom, (double)(textBox.LineSpacing * StiOptions.Engine.TextLineSpacingScale));
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
			if (val3 != null)
			{
				val3.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void DrawStringGdi(Graphics g, RectangleD rect, string text, StiText textBox, float zoom)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		RectangleD val = rect;
		float num = 1f;
		float num2 = 1f;
		float num3 = textBox.Font.Size * zoom;
		num *= g.DpiX / 100f;
		num2 *= g.DpiX / 100f;
		num3 *= num;
		Font val2 = StiFontUtils.ChangeFontSize(textBox.Font, num3);
		try
		{
			StringFormat stringFormat = textBox.TextOptions.GetStringFormat();
			try
			{
				stringFormat.Alignment = StiTextDrawing.GetAlignment(textBox.HorAlignment);
				stringFormat.LineAlignment = StiTextDrawing.GetAlignment(textBox.VertAlignment);
				Rectangle rectangle = ((RectangleD)(ref val)).ToRectangle();
				rectangle.X += (int)Math.Round(g.Transform.OffsetX);
				rectangle.Y += (int)Math.Round(g.Transform.OffsetY);
				if (textBox.IsPrinting)
				{
					rectangle.X = (int)(num * (float)rectangle.X);
					rectangle.Y = (int)(num2 * (float)rectangle.Y);
					rectangle.Width = (int)(num * (float)rectangle.Width);
					rectangle.Height = (int)(num2 * (float)rectangle.Height);
				}
				TextFormatFlags textFormatFlagsFromStringFormat = GetTextFormatFlagsFromStringFormat(stringFormat);
				TextRenderer.DrawText((IDeviceContext)(object)g, text, val2, rectangle, StiBrush.ToColor(textBox.TextBrush), textFormatFlagsFromStringFormat);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static SizeD MeasureString(double maxWidth, Font font, StiText textBox)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		Graphics measureGraphics = textBox.GetMeasureGraphics();
		lock (measureGraphics)
		{
			double num = (double)font.Size * StiDpiHelper.DeviceCapsScale;
			if ((int)font.Unit != 2 && (int)font.Unit != 0)
			{
				num *= (double)(StiOptions.Engine.StandardTextQualityScale / 96f);
			}
			Font val = font;
			if (StiDpiHelper.NeedDeviceCapsScale || StiOptions.Engine.StandardTextQualityScale != 96f)
			{
				val = StiFontUtils.ChangeFontSize(font, (float)num);
			}
			SizeD result = StiTextDrawing.MeasureString(measureGraphics, (string)textBox.Text, val, maxWidth, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, false, (double)(textBox.LineSpacing * StiOptions.Engine.TextLineSpacingScale));
			StiReport report = textBox.Report;
			if (report != null && report.ReportUnit != StiReportUnitType.HundredthsOfInch)
			{
				StiUnit unit = report.Unit;
				StiMargins margins = textBox.Margins;
				double num2 = ((SizeD)(ref result)).Width + margins.Left + margins.Right + textBox.Border.Size;
				if (unit.ConvertToHInches(Math.Round(unit.ConvertFromHInches(num2), 2)) < num2)
				{
					((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width + unit.ConvertToHInches(0.01);
				}
			}
			if (textBox.Angle == 90f || textBox.Angle == 180f)
			{
				((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorStandard;
			}
			else
			{
				((SizeD)(ref result)).Height = ((SizeD)(ref result)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorStandard;
			}
			return result;
		}
	}

	private static TextFormatFlags GetTextFormatFlagsFromStringFormat(StringFormat format)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected I4, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		TextFormatFlags val = (TextFormatFlags)0;
		if (format == null)
		{
			return val;
		}
		float num = default(float);
		if (format.GetTabStops(ref num) != null)
		{
			val = (TextFormatFlags)(val | 0x40);
		}
		if ((format.FormatFlags & 1) != 0)
		{
			val = (TextFormatFlags)(val | 0x20000);
		}
		if ((format.FormatFlags & 4) != 0)
		{
			val = (TextFormatFlags)(val | 0x10000000);
		}
		if ((format.FormatFlags & 0x4000) != 0)
		{
			val = (TextFormatFlags)(val | 0x100);
		}
		val = (TextFormatFlags)(val | 0x10);
		if ((format.FormatFlags & 0x2000) != 0)
		{
			val = (TextFormatFlags)(val | 0x2010);
		}
		StringAlignment alignment = format.Alignment;
		val = (TextFormatFlags)(((int)alignment == 1) ? (val | 1) : (((int)alignment != 2) ? (val | 0) : (val | 2)));
		StringAlignment lineAlignment = format.LineAlignment;
		val = (TextFormatFlags)(((int)lineAlignment == 1) ? (val | 4) : (((int)lineAlignment != 2) ? (val | 0) : (val | 8)));
		StringTrimming trimming = format.Trimming;
		switch (trimming - 3)
		{
		case 0:
			val = (TextFormatFlags)(val | 0x8000);
			break;
		case 2:
			val = (TextFormatFlags)(val | 0x4000);
			break;
		case 1:
			val = (TextFormatFlags)(val | 0x40000);
			break;
		}
		HotkeyPrefix hotkeyPrefix = format.HotkeyPrefix;
		if ((int)hotkeyPrefix != 0)
		{
			if ((int)hotkeyPrefix == 2)
			{
				val = (TextFormatFlags)(val | 0x100000);
			}
		}
		else
		{
			val = (TextFormatFlags)(val | 0x800);
		}
		return val;
	}
}
