using System;
using System.Drawing;
using System.Drawing.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public static class StiTypographicTextRender
{
	public static void DrawString(Graphics g, RectangleD rect, string text, StiComponent component, Font textFont, StiBrush textBrush, StiTextOptions textOptions, StiTextHorAlignment textHorAlignment, StiVertAlignment textVertAlignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		RectangleD val = rect;
		double num = component.Page.Zoom;
		if (StiScale.IsNoScaling && !component.IsPrinting && StiDpiHelper.NeedDeviceCapsScale)
		{
			num *= StiDpiHelper.DeviceCapsScale;
		}
		float num2 = (float)((double)textFont.Size * num);
		if (component.Report.IsPrinting)
		{
			if ((int)textFont.Unit != 2 && (int)textFont.Unit != 0)
			{
				num2 *= StiOptions.Engine.TypographicTextQualityScale / 100f;
			}
		}
		else if ((int)textFont.Unit != 2 && (int)textFont.Unit != 0)
		{
			num2 *= StiOptions.Engine.StandardTextQualityScale / 96f;
		}
		Font val2 = StiFontUtils.ChangeFontSize(textFont, num2);
		try
		{
			Brush brush = StiBrush.GetBrush(textBrush, ((RectangleD)(ref val)).ToRectangleF());
			try
			{
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				g.TextRenderingHint = (TextRenderingHint)4;
				StiTextDrawing.DrawString(g, text, val2, brush, val, textOptions, textHorAlignment, textVertAlignment, true, (float)component.Page.Zoom, (double)((component as StiText).LineSpacing * StiOptions.Engine.TextLineSpacingScale));
				g.TextRenderingHint = textRenderingHint;
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		DrawString(g, rect, text, textBox, textBox.Font, textBox.TextBrush, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment);
	}

	public static SizeD MeasureString(double width, Font font, StiText textBox)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)((double)font.Size * StiDpiHelper.DeviceCapsScale);
		if ((int)font.Unit != 2 && (int)font.Unit != 0)
		{
			num *= StiOptions.Engine.StandardTextQualityScale / 96f;
		}
		TextRenderingHint textRenderingHint;
		lock (textBox.GetMeasureGraphics())
		{
			textRenderingHint = textBox.GetMeasureGraphics().TextRenderingHint;
			textBox.GetMeasureGraphics().TextRenderingHint = (TextRenderingHint)4;
		}
		Font val = StiFontUtils.ChangeFontSize(font, num);
		SizeD result;
		try
		{
			lock (textBox.GetMeasureGraphics())
			{
				result = StiTextDrawing.MeasureString(textBox.GetMeasureGraphics(), (string)textBox.Text, val, width, textBox.TextOptions, textBox.HorAlignment, textBox.VertAlignment, true, (double)(textBox.LineSpacing * StiOptions.Engine.TextLineSpacingScale));
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		lock (textBox.GetMeasureGraphics())
		{
			textBox.GetMeasureGraphics().TextRenderingHint = textRenderingHint;
		}
		if (textBox.Report != null && textBox.Report.ReportUnit != StiReportUnitType.HundredthsOfInch)
		{
			StiUnit unit = textBox.Report.Unit;
			double num2 = ((SizeD)(ref result)).Width + textBox.Margins.Left + textBox.Margins.Right + textBox.Border.Size;
			if (unit.ConvertToHInches(Math.Round(unit.ConvertFromHInches(num2), 2)) < num2)
			{
				((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width + unit.ConvertToHInches(0.01);
			}
		}
		if (textBox.Angle == 90f || textBox.Angle == 180f)
		{
			((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
		}
		else
		{
			((SizeD)(ref result)).Height = ((SizeD)(ref result)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
		}
		return result;
	}
}
