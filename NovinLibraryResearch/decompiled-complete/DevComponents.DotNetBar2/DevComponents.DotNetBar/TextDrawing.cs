using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class TextDrawing
{
	public static bool TextDrawingEnabled;

	public static bool UseTextRenderer;

	internal static bool _၃;

	private static Dictionary<eTextFormat, StringFormat> _၄;

	static TextDrawing()
	{
		TextDrawingEnabled = true;
		UseTextRenderer = false;
		_၃ = false;
		_၄ = new Dictionary<eTextFormat, StringFormat>();
		switch (Application.CurrentCulture.TwoLetterISOLanguageName)
		{
		case "ja":
		case "ko":
		case "zh":
		case "ar":
			_၃ = true;
			break;
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, int x, int y, eTextFormat format)
	{
		DrawString(g, text, font, color, new Rectangle(x, y, 0, 0), format);
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, Rectangle bounds, eTextFormat format)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (UseTextRenderer && (format & eTextFormat.Vertical) == 0)
		{
			TextRenderer.DrawText((IDeviceContext)(object)g, text, font, bounds, color, GetTextFormatFlags(format));
		}
		else
		{
			DrawStringLegacy(g, text, font, color, bounds, format);
		}
	}

	public static void DrawStringLegacy(Graphics g, string text, Font font, Color color, Rectangle bounds, eTextFormat format)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		if (color.IsEmpty || !TextDrawingEnabled)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			StringFormat stringFormat = GetStringFormat(format);
			g.DrawString(text, font, (Brush)(object)val, (RectangleF)bounds, stringFormat);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawStringLegacy(Graphics g, string text, Font font, Color color, RectangleF bounds, eTextFormat format)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		if (color.IsEmpty || !TextDrawingEnabled)
		{
			return;
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			StringFormat stringFormat = GetStringFormat(format);
			g.DrawString(text, font, (Brush)(object)val, bounds, stringFormat);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static Size MeasureString(Graphics g, string text, Font font)
	{
		return MeasureString(g, text, font, Size.Empty, eTextFormat.Default);
	}

	public static Size MeasureString(Graphics g, string text, Font font, int proposedWidth, eTextFormat format)
	{
		return MeasureString(g, text, font, new Size(proposedWidth, 0), format);
	}

	public static Size MeasureString(Graphics g, string text, Font font, int proposedWidth)
	{
		return MeasureString(g, text, font, new Size(proposedWidth, 0), eTextFormat.Default);
	}

	public static Size MeasureString(Graphics g, string text, Font font, Size proposedSize, eTextFormat format)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (UseTextRenderer && (format & eTextFormat.Vertical) == 0)
		{
			format &= ~(format & eTextFormat.VerticalCenter);
			format &= ~(format & eTextFormat.Bottom);
			format &= ~(format & eTextFormat.HorizontalCenter);
			format &= ~(format & eTextFormat.Right);
			format &= ~(format & eTextFormat.EndEllipsis);
			return Size.Ceiling(TextRenderer.MeasureText((IDeviceContext)(object)g, text, font, proposedSize, GetTextFormatFlags(format)));
		}
		StringFormat stringFormat = GetStringFormat(format);
		return Size.Ceiling(g.MeasureString(text, font, (SizeF)proposedSize, stringFormat));
	}

	public static Size MeasureStringLegacy(Graphics g, string text, Font font, Size proposedSize, eTextFormat format)
	{
		StringFormat stringFormat = GetStringFormat(format);
		return g.MeasureString(text, font, (SizeF)proposedSize, stringFormat).ToSize();
	}

	public static eTextFormat TranslateHorizontal(StringAlignment align)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)align == 1)
		{
			return eTextFormat.HorizontalCenter;
		}
		if ((int)align == 2)
		{
			return eTextFormat.Right;
		}
		return eTextFormat.Default;
	}

	public static eTextFormat TranslateVertical(StringAlignment align)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)align == 1)
		{
			return eTextFormat.VerticalCenter;
		}
		if ((int)align == 2)
		{
			return eTextFormat.Bottom;
		}
		return eTextFormat.Default;
	}

	public static TextFormatFlags GetTextFormatFlags(eTextFormat format)
	{
		format |= eTextFormat.PreserveGraphicsClipping | eTextFormat.PreserveGraphicsTranslateTransform;
		if ((format & eTextFormat.SingleLine) == eTextFormat.SingleLine && (format & eTextFormat.WordBreak) == eTextFormat.WordBreak)
		{
			format &= ~(format & eTextFormat.SingleLine);
		}
		return (TextFormatFlags)format;
	}

	public static StringFormat GetStringFormat(eTextFormat format)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		if (_၄.TryGetValue(format, out var value))
		{
			return value;
		}
		value = ((!_၃) ? ((StringFormat)StringFormat.GenericTypographic.Clone()) : ((StringFormat)StringFormat.GenericDefault.Clone()));
		if (format == eTextFormat.Default)
		{
			value.HotkeyPrefix = (HotkeyPrefix)1;
			_၄.Add(format, value);
			return value;
		}
		if ((format & eTextFormat.HorizontalCenter) == eTextFormat.HorizontalCenter)
		{
			value.Alignment = (StringAlignment)1;
		}
		else if ((format & eTextFormat.Right) == eTextFormat.Right)
		{
			value.Alignment = (StringAlignment)2;
		}
		if ((format & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
		{
			value.LineAlignment = (StringAlignment)1;
		}
		else if ((format & eTextFormat.Bottom) == eTextFormat.Bottom)
		{
			value.LineAlignment = (StringAlignment)2;
		}
		if ((format & eTextFormat.EndEllipsis) == eTextFormat.EndEllipsis)
		{
			value.Trimming = (StringTrimming)3;
		}
		else
		{
			value.Trimming = (StringTrimming)1;
		}
		if ((format & eTextFormat.HidePrefix) == eTextFormat.HidePrefix)
		{
			value.HotkeyPrefix = (HotkeyPrefix)2;
		}
		else if ((format & eTextFormat.NoPrefix) == eTextFormat.NoPrefix)
		{
			value.HotkeyPrefix = (HotkeyPrefix)0;
		}
		else
		{
			value.HotkeyPrefix = (HotkeyPrefix)1;
		}
		if ((format & eTextFormat.WordBreak) == eTextFormat.WordBreak)
		{
			value.FormatFlags = (StringFormatFlags)(value.FormatFlags & ~(value.FormatFlags & 0x1000));
		}
		else
		{
			StringFormat obj = value;
			obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags | 0x1000);
		}
		if ((format & eTextFormat.LeftAndRightPadding) == eTextFormat.LeftAndRightPadding)
		{
			StringFormat obj2 = value;
			obj2.FormatFlags = (StringFormatFlags)(obj2.FormatFlags | 0x800);
		}
		if ((format & eTextFormat.RightToLeft) == eTextFormat.RightToLeft)
		{
			StringFormat obj3 = value;
			obj3.FormatFlags = (StringFormatFlags)(obj3.FormatFlags | 1);
		}
		if ((format & eTextFormat.Vertical) == eTextFormat.Vertical)
		{
			StringFormat obj4 = value;
			obj4.FormatFlags = (StringFormatFlags)(obj4.FormatFlags | 2);
		}
		if ((format & eTextFormat.NoClipping) == eTextFormat.NoClipping)
		{
			StringFormat obj5 = value;
			obj5.FormatFlags = (StringFormatFlags)(obj5.FormatFlags | 0x4000);
		}
		_၄.Add(format, value);
		return value;
	}

	public static ThemeTextFormat GetTextFormat(eTextFormat format)
	{
		ThemeTextFormat themeTextFormat = ThemeTextFormat.Top;
		if (format == eTextFormat.Default)
		{
			return themeTextFormat;
		}
		if ((format & eTextFormat.HorizontalCenter) == eTextFormat.HorizontalCenter)
		{
			themeTextFormat |= ThemeTextFormat.Center;
		}
		else if ((format & eTextFormat.Right) == eTextFormat.Right)
		{
			themeTextFormat |= ThemeTextFormat.Right;
		}
		if ((format & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
		{
			themeTextFormat |= ThemeTextFormat.VCenter;
		}
		else if ((format & eTextFormat.Bottom) == eTextFormat.Bottom)
		{
			themeTextFormat |= ThemeTextFormat.Bottom;
		}
		if ((format & eTextFormat.EndEllipsis) == eTextFormat.EndEllipsis)
		{
			themeTextFormat |= ThemeTextFormat.EndEllipsis;
		}
		if ((format & eTextFormat.HidePrefix) == eTextFormat.HidePrefix)
		{
			themeTextFormat |= ThemeTextFormat.HidePrefix;
		}
		else if ((format & eTextFormat.NoPrefix) == eTextFormat.NoPrefix)
		{
			themeTextFormat |= ThemeTextFormat.NoPrefix;
		}
		themeTextFormat = (((format & eTextFormat.WordBreak) != eTextFormat.WordBreak) ? (themeTextFormat | ThemeTextFormat.SingleLine) : (themeTextFormat | ThemeTextFormat.WordBreak));
		if ((format & eTextFormat.RightToLeft) == eTextFormat.RightToLeft)
		{
			themeTextFormat |= ThemeTextFormat.RtlReading;
		}
		if ((format & eTextFormat.NoClipping) == eTextFormat.NoClipping)
		{
			themeTextFormat |= ThemeTextFormat.NoClip;
		}
		return themeTextFormat;
	}
}
