using System.Drawing;

namespace Stimulsoft.Base.Drawing;

public sealed class StiFontUtils
{
	public static FontStyle CorrectStyle(string fontName, FontStyle style)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		bool flag = StiFontCollection.IsCustomFont(fontName);
		FontFamily val = null;
		try
		{
			val = (FontFamily)(flag ? ((object)StiFontCollection.GetFontFamily(fontName)) : ((object)new FontFamily(fontName)));
		}
		catch
		{
			return (FontStyle)0;
		}
		try
		{
			if (val.IsStyleAvailable(style))
			{
				return style;
			}
			if (!val.IsStyleAvailable((FontStyle)1) && (style & 1) > 0)
			{
				style = (FontStyle)(style - 1);
			}
			if (!val.IsStyleAvailable((FontStyle)2) && (style & 2) > 0)
			{
				style = (FontStyle)(style - 2);
			}
			if (!val.IsStyleAvailable((FontStyle)8) && (style & 8) > 0)
			{
				style = (FontStyle)(style - 8);
			}
			if (!val.IsStyleAvailable((FontStyle)4) && (style & 4) > 0)
			{
				style = (FontStyle)(style - 4);
			}
			if (!val.IsStyleAvailable(style))
			{
				if (val.IsStyleAvailable((FontStyle)1))
				{
					return (FontStyle)1;
				}
				if (val.IsStyleAvailable((FontStyle)2))
				{
					return (FontStyle)2;
				}
				if (val.IsStyleAvailable((FontStyle)4))
				{
					return (FontStyle)4;
				}
				if (val.IsStyleAvailable((FontStyle)8))
				{
					return (FontStyle)8;
				}
			}
			return style;
		}
		finally
		{
			if (!flag)
			{
				val.Dispose();
			}
		}
	}

	public static Font ChangeFontName(Font font, string newFontName)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (string.IsNullOrEmpty(newFontName))
		{
			return font;
		}
		return new Font(StiFontCollection.GetFontFamily(newFontName), font.Size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontSize(Font font, float newFontSize)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (newFontSize < 1f)
		{
			newFontSize = 1f;
		}
		return new Font(font.FontFamily, newFontSize, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontStyle(Font font, FontStyle style)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		return new Font(font.FontFamily, font.Size, style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontStyle(string fontName, float fontSize, FontStyle style)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		return new Font(fontName, fontSize, style);
	}

	public static Font ChangeFontStyleBold(Font font, bool bold)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (font.Italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (font.Underline)
		{
			val = (FontStyle)(val | 4);
		}
		return new Font(font.FontFamily, font.Size, val, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontStyleItalic(Font font, bool italic)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (font.Bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (font.Underline)
		{
			val = (FontStyle)(val | 4);
		}
		return new Font(font.FontFamily, font.Size, val, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontStyleUnderline(Font font, bool underline)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (font.Bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (font.Italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (underline)
		{
			val = (FontStyle)(val | 4);
		}
		return new Font(font.FontFamily, font.Size, val, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public static Font ChangeFontStyleStrikeout(Font font, bool strikeout)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)0;
		if (font.Bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (font.Italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (font.Underline)
		{
			val = (FontStyle)(val | 4);
		}
		if (strikeout)
		{
			val = (FontStyle)(val | 8);
		}
		return new Font(font.FontFamily, font.Size, val, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}
}
