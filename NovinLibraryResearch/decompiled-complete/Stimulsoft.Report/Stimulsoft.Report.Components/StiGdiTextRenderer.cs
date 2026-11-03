using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components;

public static class StiGdiTextRenderer
{
	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)((double)textBox.Font.Size * textBox.Page.Zoom);
		float num2 = g.DpiX / 100f;
		float num3 = g.DpiX / 100f;
		num *= num2;
		Font val = StiFontUtils.ChangeFontSize(textBox.Font, num);
		try
		{
			StringFormat stringFormat = textBox.TextOptions.GetStringFormat();
			try
			{
				stringFormat.Alignment = StiTextDrawing.GetAlignment(textBox.HorAlignment);
				stringFormat.LineAlignment = StiTextDrawing.GetAlignment(textBox.VertAlignment);
				Rectangle rectangle = ((RectangleD)(ref rect)).ToRectangle();
				rectangle.X += (int)Math.Round(g.Transform.OffsetX);
				rectangle.Y += (int)Math.Round(g.Transform.OffsetY);
				if (textBox.IsPrinting)
				{
					rectangle.X = (int)(num2 * (float)rectangle.X);
					rectangle.Y = (int)(num3 * (float)rectangle.Y);
					rectangle.Width = (int)(num2 * (float)rectangle.Width);
					rectangle.Height = (int)(num3 * (float)rectangle.Height);
				}
				TextFormatFlags textFormatFlagsFromStringFormat = GetTextFormatFlagsFromStringFormat(stringFormat);
				TextRenderer.DrawText((IDeviceContext)(object)g, text, val, rectangle, StiBrush.ToColor(textBox.TextBrush), textFormatFlagsFromStringFormat);
			}
			finally
			{
				((IDisposable)stringFormat)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
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
