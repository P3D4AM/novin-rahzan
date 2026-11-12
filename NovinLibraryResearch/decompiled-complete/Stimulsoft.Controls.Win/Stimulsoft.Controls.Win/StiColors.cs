using System.Drawing;

namespace Stimulsoft.Controls.Win;

internal sealed class StiColors
{
	private static Color[] customColors;

	private static Color[] systemColors;

	private static Color[] colors;

	private static Color focus;

	private static Color controlStart;

	private static Color controlEnd;

	private static Color content;

	private static Color contentDark;

	private static Color controlText;

	private static Color selected;

	private static Color selectedText;

	private static Color controlStartLight;

	private static Color controlEndLight;

	private static Color controlStartDark;

	private static Color controlEndDark;

	private static Color activeCaptionStart;

	private static Color activeCaptionEnd;

	private static Color inactiveCaptionStart;

	private static Color inactiveCaptionEnd;

	public static Color[] CustomColors => customColors;

	public static Color[] SystemColors => systemColors;

	public static Color[] Colors => colors;

	public static Color Focus
	{
		get
		{
			return focus;
		}
		set
		{
			focus = value;
		}
	}

	public static Color ControlStart
	{
		get
		{
			return controlStart;
		}
		set
		{
			controlStart = value;
		}
	}

	public static Color ControlEnd
	{
		get
		{
			return controlEnd;
		}
		set
		{
			controlEnd = value;
		}
	}

	public static Color Content
	{
		get
		{
			return content;
		}
		set
		{
			content = value;
		}
	}

	public static Color ContentDark
	{
		get
		{
			return contentDark;
		}
		set
		{
			contentDark = value;
		}
	}

	public static Color ControlText
	{
		get
		{
			return controlText;
		}
		set
		{
			controlText = value;
		}
	}

	public static Color Selected
	{
		get
		{
			return selected;
		}
		set
		{
			selected = value;
		}
	}

	public static Color SelectedText
	{
		get
		{
			return selectedText;
		}
		set
		{
			selectedText = value;
		}
	}

	public static Color ControlStartLight
	{
		get
		{
			return controlStartLight;
		}
		set
		{
			controlStartLight = value;
		}
	}

	public static Color ControlEndLight
	{
		get
		{
			return controlEndLight;
		}
		set
		{
			controlEndLight = value;
		}
	}

	public static Color ControlStartDark
	{
		get
		{
			return controlStartDark;
		}
		set
		{
			controlStartDark = value;
		}
	}

	public static Color ControlEndDark
	{
		get
		{
			return controlEndDark;
		}
		set
		{
			controlEndDark = value;
		}
	}

	public static Color ActiveCaptionStart
	{
		get
		{
			return activeCaptionStart;
		}
		set
		{
			activeCaptionStart = value;
		}
	}

	public static Color ActiveCaptionEnd
	{
		get
		{
			return activeCaptionEnd;
		}
		set
		{
			activeCaptionEnd = value;
		}
	}

	public static Color InactiveCaptionStart
	{
		get
		{
			return inactiveCaptionStart;
		}
		set
		{
			inactiveCaptionStart = value;
		}
	}

	public static Color InactiveCaptionEnd
	{
		get
		{
			return inactiveCaptionEnd;
		}
		set
		{
			inactiveCaptionEnd = value;
		}
	}

	public static void InitColors()
	{
		activeCaptionStart = StiColorUtils.GetSysColor(Win32.ColorType.COLOR_GRADIENTACTIVECAPTION);
		activeCaptionEnd = System.Drawing.SystemColors.ActiveCaption;
		inactiveCaptionStart = StiColorUtils.GetSysColor(Win32.ColorType.COLOR_GRADIENTINACTIVECAPTION);
		inactiveCaptionEnd = System.Drawing.SystemColors.InactiveCaption;
		focus = CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 70);
		selected = CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 30);
		selectedText = CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 220);
		content = CalcColor(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.Control, 200);
		contentDark = StiColorUtils.Dark(Content, 10);
		controlStart = StiColorUtils.Light(System.Drawing.SystemColors.Control, 30);
		controlEnd = StiColorUtils.Dark(System.Drawing.SystemColors.Control, 10);
		controlText = System.Drawing.SystemColors.ControlText;
		controlStartLight = StiColorUtils.Light(ControlStart, 20);
		controlEndLight = StiColorUtils.Light(ControlEnd, 20);
		controlStartDark = StiColorUtils.Dark(ControlStart, 20);
		controlEndDark = StiColorUtils.Dark(ControlEnd, 20);
	}

	private static Color CalcColor(Color front, Color back, int alpha)
	{
		Color color = Color.FromArgb(255, front);
		Color color2 = Color.FromArgb(255, back);
		float num = (int)color.R;
		float num2 = (int)color.G;
		float num3 = (int)color.B;
		float num4 = (int)color2.R;
		float num5 = (int)color2.G;
		float num6 = (int)color2.B;
		float num7 = num * (float)alpha / 255f + num4 * ((float)(255 - alpha) / 255f);
		byte red = (byte)num7;
		float num8 = num2 * (float)alpha / 255f + num5 * ((float)(255 - alpha) / 255f);
		byte green = (byte)num8;
		float num9 = num3 * (float)alpha / 255f + num6 * ((float)(255 - alpha) / 255f);
		byte blue = (byte)num9;
		return Color.FromArgb(255, red, green, blue);
	}

	static StiColors()
	{
		customColors = new Color[64]
		{
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 192, 192),
			Color.FromArgb(255, 224, 192),
			Color.FromArgb(255, 255, 192),
			Color.FromArgb(192, 255, 192),
			Color.FromArgb(192, 255, 255),
			Color.FromArgb(192, 192, 255),
			Color.FromArgb(255, 192, 255),
			Color.FromArgb(224, 224, 224),
			Color.FromArgb(255, 128, 128),
			Color.FromArgb(255, 192, 128),
			Color.FromArgb(255, 255, 128),
			Color.FromArgb(128, 255, 128),
			Color.FromArgb(128, 255, 255),
			Color.FromArgb(128, 128, 255),
			Color.FromArgb(255, 128, 255),
			Color.FromArgb(192, 192, 192),
			Color.FromArgb(255, 0, 0),
			Color.FromArgb(255, 128, 0),
			Color.FromArgb(255, 255, 0),
			Color.FromArgb(0, 255, 0),
			Color.FromArgb(0, 255, 255),
			Color.FromArgb(0, 0, 255),
			Color.FromArgb(255, 0, 255),
			Color.FromArgb(128, 128, 128),
			Color.FromArgb(192, 0, 0),
			Color.FromArgb(192, 64, 0),
			Color.FromArgb(192, 192, 0),
			Color.FromArgb(0, 192, 0),
			Color.FromArgb(0, 192, 192),
			Color.FromArgb(0, 0, 192),
			Color.FromArgb(192, 0, 192),
			Color.FromArgb(64, 64, 64),
			Color.FromArgb(128, 0, 0),
			Color.FromArgb(128, 64, 0),
			Color.FromArgb(128, 128, 0),
			Color.FromArgb(0, 128, 0),
			Color.FromArgb(0, 128, 128),
			Color.FromArgb(0, 0, 128),
			Color.FromArgb(128, 0, 128),
			Color.FromArgb(0, 0, 0),
			Color.FromArgb(64, 0, 0),
			Color.FromArgb(128, 64, 64),
			Color.FromArgb(64, 64, 0),
			Color.FromArgb(0, 64, 0),
			Color.FromArgb(0, 64, 64),
			Color.FromArgb(0, 0, 64),
			Color.FromArgb(64, 0, 64),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255),
			Color.FromArgb(255, 255, 255)
		};
		systemColors = new Color[26]
		{
			System.Drawing.SystemColors.ActiveBorder,
			System.Drawing.SystemColors.ActiveCaption,
			System.Drawing.SystemColors.ActiveCaptionText,
			System.Drawing.SystemColors.AppWorkspace,
			System.Drawing.SystemColors.Control,
			System.Drawing.SystemColors.ControlDark,
			System.Drawing.SystemColors.ControlDarkDark,
			System.Drawing.SystemColors.ControlLight,
			System.Drawing.SystemColors.ControlLightLight,
			System.Drawing.SystemColors.ControlText,
			System.Drawing.SystemColors.Desktop,
			System.Drawing.SystemColors.GrayText,
			System.Drawing.SystemColors.Highlight,
			System.Drawing.SystemColors.HighlightText,
			System.Drawing.SystemColors.HotTrack,
			System.Drawing.SystemColors.InactiveBorder,
			System.Drawing.SystemColors.InactiveCaption,
			System.Drawing.SystemColors.InactiveCaptionText,
			System.Drawing.SystemColors.Info,
			System.Drawing.SystemColors.InfoText,
			System.Drawing.SystemColors.Menu,
			System.Drawing.SystemColors.MenuText,
			System.Drawing.SystemColors.ScrollBar,
			System.Drawing.SystemColors.Window,
			System.Drawing.SystemColors.WindowFrame,
			System.Drawing.SystemColors.WindowText
		};
		colors = new Color[141]
		{
			Color.Transparent,
			Color.Black,
			Color.DimGray,
			Color.Gray,
			Color.DarkGray,
			Color.Silver,
			Color.LightGray,
			Color.Gainsboro,
			Color.WhiteSmoke,
			Color.White,
			Color.RosyBrown,
			Color.IndianRed,
			Color.Brown,
			Color.Firebrick,
			Color.LightCoral,
			Color.Maroon,
			Color.DarkRed,
			Color.Red,
			Color.Snow,
			Color.MistyRose,
			Color.Salmon,
			Color.Tomato,
			Color.DarkSalmon,
			Color.Coral,
			Color.OrangeRed,
			Color.LightSalmon,
			Color.Sienna,
			Color.SeaShell,
			Color.Chocolate,
			Color.SaddleBrown,
			Color.SandyBrown,
			Color.PeachPuff,
			Color.Peru,
			Color.Linen,
			Color.Bisque,
			Color.DarkOrange,
			Color.BurlyWood,
			Color.Tan,
			Color.AntiqueWhite,
			Color.NavajoWhite,
			Color.BlanchedAlmond,
			Color.PapayaWhip,
			Color.Moccasin,
			Color.Orange,
			Color.Wheat,
			Color.OldLace,
			Color.FloralWhite,
			Color.DarkGoldenrod,
			Color.Goldenrod,
			Color.Cornsilk,
			Color.Gold,
			Color.Khaki,
			Color.LemonChiffon,
			Color.PaleGoldenrod,
			Color.DarkKhaki,
			Color.Beige,
			Color.LightGoldenrodYellow,
			Color.Olive,
			Color.Yellow,
			Color.LightYellow,
			Color.Ivory,
			Color.OliveDrab,
			Color.YellowGreen,
			Color.DarkOliveGreen,
			Color.GreenYellow,
			Color.Chartreuse,
			Color.LawnGreen,
			Color.DarkSeaGreen,
			Color.ForestGreen,
			Color.LimeGreen,
			Color.LightGreen,
			Color.PaleGreen,
			Color.DarkGreen,
			Color.Green,
			Color.Lime,
			Color.Honeydew,
			Color.SeaGreen,
			Color.MediumSeaGreen,
			Color.SpringGreen,
			Color.MintCream,
			Color.MediumSpringGreen,
			Color.MediumAquamarine,
			Color.Aquamarine,
			Color.Turquoise,
			Color.LightSeaGreen,
			Color.MediumTurquoise,
			Color.DarkSlateGray,
			Color.PaleTurquoise,
			Color.Teal,
			Color.DarkCyan,
			Color.Aqua,
			Color.Cyan,
			Color.LightCyan,
			Color.Azure,
			Color.DarkTurquoise,
			Color.CadetBlue,
			Color.PowderBlue,
			Color.LightBlue,
			Color.DeepSkyBlue,
			Color.SkyBlue,
			Color.LightSkyBlue,
			Color.SteelBlue,
			Color.AliceBlue,
			Color.DodgerBlue,
			Color.SlateGray,
			Color.LightSlateGray,
			Color.LightSteelBlue,
			Color.CornflowerBlue,
			Color.RoyalBlue,
			Color.MidnightBlue,
			Color.Lavender,
			Color.Navy,
			Color.DarkBlue,
			Color.MediumBlue,
			Color.Blue,
			Color.GhostWhite,
			Color.SlateBlue,
			Color.DarkSlateBlue,
			Color.MediumSlateBlue,
			Color.MediumPurple,
			Color.BlueViolet,
			Color.Indigo,
			Color.DarkOrchid,
			Color.DarkViolet,
			Color.MediumOrchid,
			Color.Thistle,
			Color.Plum,
			Color.Violet,
			Color.Purple,
			Color.DarkMagenta,
			Color.Magenta,
			Color.Fuchsia,
			Color.Orchid,
			Color.MediumVioletRed,
			Color.DeepPink,
			Color.HotPink,
			Color.LavenderBlush,
			Color.PaleVioletRed,
			Color.Crimson,
			Color.Pink,
			Color.LightPink
		};
		InitColors();
	}

	private StiColors()
	{
	}
}
