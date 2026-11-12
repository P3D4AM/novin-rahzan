using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Stimulsoft.Base.Drawing;

public class StiUX
{
	public static bool IsDark { get; set; }

	public static bool AllowUseDark { get; }

	public static Color Accent => SystemColors.MenuHighlight;

	public static Color BackgroundDark
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#202020");
		}
	}

	public static Color Background
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#2b2b2b");
		}
	}

	public static Color BackgroundLight
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb(238, 238, 238);
			}
			return StiColor.Get("#4e4e4e");
		}
	}

	public static Color Separator
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb(198, 198, 198);
			}
			return StiColor.Get("#4a4a4a");
		}
	}

	public static Color Foreground
	{
		get
		{
			if (!IsDark)
			{
				return Color.DimGray;
			}
			return StiColor.Get("#b1b1b1");
		}
	}

	public static Color Hyperlink
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb(255, 43, 86, 154);
			}
			return StiColor.Get("#23a6e8");
		}
	}

	public static Color HyperlinkHover
	{
		get
		{
			if (!IsDark)
			{
				return Color.Red;
			}
			return StiColor.Get("#79cdfe");
		}
	}

	public static Color ErrorForeground
	{
		get
		{
			if (!IsDark)
			{
				return Color.Red;
			}
			return StiColor.Get("#f30031");
		}
	}

	public static Color ErrorBackground
	{
		get
		{
			if (!IsDark)
			{
				return Color.FromArgb(252, 220, 220);
			}
			return StiColor.Get("#49252d");
		}
	}

	public static Color TrackBackground
	{
		get
		{
			if (!IsDark)
			{
				return StiColor.Get("#bbbcbc");
			}
			return StiColor.Get("#474747");
		}
	}

	public static Color TrackBackgroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return StiColor.Get("#e5e5e5");
			}
			return StiColor.Get("#626262");
		}
	}

	public static Color TrackIndicator
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#888888");
		}
	}

	public static Color TrackIndicatorDisabled
	{
		get
		{
			if (!IsDark)
			{
				return StiColor.Get("#d5d5d5");
			}
			return StiColor.Get("#b1b1b1");
		}
	}

	public static Color TrackIndicatorHover
	{
		get
		{
			if (!IsDark)
			{
				return StiColor.Get("#eeeeee");
			}
			return StiColor.Get("#aaaaaa");
		}
	}

	public static Color ButtonBackground
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#373737");
		}
	}

	public static Color ButtonBackgroundHover
	{
		get
		{
			if (!IsDark)
			{
				return Color.LightGray;
			}
			return StiColor.Get("#434343");
		}
	}

	public static Color ButtonBackgroundChecked
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#626262");
		}
	}

	public static Color ButtonBackgroundPressed
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#121212");
		}
	}

	public static Color ButtonBackgroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#626262");
		}
	}

	public static Color ButtonBorder
	{
		get
		{
			if (!IsDark)
			{
				return Color.DarkGray;
			}
			return StiColor.Get("#5a5a5a");
		}
	}

	public static Color ButtonBorderPressed
	{
		get
		{
			if (!IsDark)
			{
				return Color.Black;
			}
			return StiColor.Get("#3a3a3a");
		}
	}

	public static Color ButtonForeground
	{
		get
		{
			if (!IsDark)
			{
				return Color.Black;
			}
			return StiColor.Get("#dddddd");
		}
	}

	public static Color ButtonForegroundHover
	{
		get
		{
			if (!IsDark)
			{
				return Color.Black;
			}
			return StiColor.Get("#eeeeee");
		}
	}

	public static Color ButtonForegroundChecked
	{
		get
		{
			if (!IsDark)
			{
				return Color.Black;
			}
			return StiColor.Get("#eeeeee");
		}
	}

	public static Color ButtonForegroundPressed
	{
		get
		{
			if (!IsDark)
			{
				return Color.Black;
			}
			return StiColor.Get("#dddddd");
		}
	}

	public static Color ButtonForegroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.ControlLight;
			}
			return StiColor.Get("#888888");
		}
	}

	public static Color ItemBackground
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.Window;
			}
			return StiColor.Get("#3a3a3a");
		}
	}

	public static Color ItemBackgroundHover
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.Highlight;
			}
			return StiColor.Get("#595959");
		}
	}

	public static Color ItemBackgroundPressed
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.Highlight;
			}
			return StiColor.Get("#4e4e4e");
		}
	}

	public static Color ItemBackgroundInactive
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.InactiveCaption;
			}
			return StiColor.Get("#555555");
		}
	}

	public static Color ItemBackgroundSelected
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.Highlight;
			}
			return StiColor.Get("#6e6e6e");
		}
	}

	public static Color ItemBackgroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return Color.WhiteSmoke;
			}
			return StiColor.Get("#727272");
		}
	}

	public static Color ItemSeparator
	{
		get
		{
			if (!IsDark)
			{
				return Color.Gray;
			}
			return StiColor.Get("#4a4a4a");
		}
	}

	public static Color ItemForeground
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.WindowText;
			}
			return StiColor.Get("#cccccc");
		}
	}

	public static Color ItemForegroundHover
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.HighlightText;
			}
			return StiColor.Get("#cccccc");
		}
	}

	public static Color ItemForegroundPressed
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.HighlightText;
			}
			return StiColor.Get("#888888");
		}
	}

	public static Color ItemForegroundInactive
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.InactiveCaptionText;
			}
			return StiColor.Get("#aaaaaa");
		}
	}

	public static Color ItemForegroundSelected
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.HighlightText;
			}
			return StiColor.Get("#dddddd");
		}
	}

	public static Color ItemForegroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.ControlText;
			}
			return StiColor.Get("#666666");
		}
	}

	public static Color InputBorder
	{
		get
		{
			if (!IsDark)
			{
				return Color.DarkGray;
			}
			return StiColor.Get("#5a5a5a");
		}
	}

	public static Color InputBackground
	{
		get
		{
			if (!IsDark)
			{
				return Color.White;
			}
			return StiColor.Get("#202020");
		}
	}

	public static Color InputBackgroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return Color.WhiteSmoke;
			}
			return StiColor.Get("#454545");
		}
	}

	public static Color InputBackgroundReadOnly
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.Control;
			}
			return StiColor.Get("#353535");
		}
	}

	public static Color InputForeground
	{
		get
		{
			if (!IsDark)
			{
				return SystemColors.ControlText;
			}
			return StiColor.Get("#cfcfcf");
		}
	}

	public static Color InputForegroundDisabled
	{
		get
		{
			if (!IsDark)
			{
				return Color.Gray;
			}
			return StiColor.Get("#888888");
		}
	}

	public static Color InputForegroundHint
	{
		get
		{
			if (!IsDark)
			{
				return Color.Gray;
			}
			return StiColor.Get("#848484");
		}
	}

	[DllImport("uxtheme.dll", EntryPoint = "#95")]
	private static extern uint GetImmersiveColorFromColorSetEx(uint dwImmersiveColorSet, uint dwImmersiveColorType, bool bIgnoreHighContrast, uint dwHighContrastCacheMode);

	[DllImport("uxtheme.dll", EntryPoint = "#96")]
	private static extern uint GetImmersiveColorTypeFromName(IntPtr pName);

	[DllImport("uxtheme.dll", EntryPoint = "#98")]
	private static extern int GetImmersiveUserColorSetPreference(bool bForceCheckRegistry, bool bSkipCheckOnFail);

	public static Color GetThemeColor(string color)
	{
		uint immersiveColorFromColorSetEx = GetImmersiveColorFromColorSetEx((uint)GetImmersiveUserColorSetPreference(bForceCheckRegistry: false, bSkipCheckOnFail: false), GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni(color)), bIgnoreHighContrast: false, 0u);
		byte red = (byte)(0xFFu & immersiveColorFromColorSetEx);
		byte green = (byte)((0xFF00 & immersiveColorFromColorSetEx) >> 8);
		byte blue = (byte)((0xFF0000 & immersiveColorFromColorSetEx) >> 16);
		byte alpha = (byte)((0xFF000000u & immersiveColorFromColorSetEx) >> 24);
		return Color.FromArgb(alpha, red, green, blue);
	}
}
