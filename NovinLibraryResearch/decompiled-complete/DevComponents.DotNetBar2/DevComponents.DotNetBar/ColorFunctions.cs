using System;
using System.Drawing;
using Microsoft.Win32;

namespace DevComponents.DotNetBar;

public class ColorFunctions
{
	public struct HLSColor
	{
		public double Hue;

		public double Lightness;

		public double Saturation;
	}

	private static Color _0888;

	private static Color _0889;

	private static Color _088A;

	private static Color _088B;

	private static Color _088C;

	private static Color _088D;

	private static Color _088E;

	private static Color _088F;

	private static Color _0890;

	private static Color _0891;

	private static Bitmap _0892 = null;

	internal static bool _0893 = false;

	public static void LoadColors()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		_0895();
		if (!_0893)
		{
			SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(_0894);
		}
		_0893 = true;
	}

	private static void _0894(object _٠, UserPreferenceChangedEventArgs ؿ)
	{
		_0895();
		_089F.ळ();
		BarFunctions.RefreshScreens();
	}

	private static void _0895()
	{
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		if (_089F.न >= 16)
		{
			int red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.3 + (double)(int)SystemColors.Window.R);
			int green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.3 + (double)(int)SystemColors.Window.G);
			int blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.3 + (double)(int)SystemColors.Window.B);
			_0888 = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.45 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.45 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.45 + (double)(int)SystemColors.Window.B);
			_0889 = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.1 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.1 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.1 + (double)(int)SystemColors.Window.B);
			_088A = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.25 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.25 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.25 + (double)(int)SystemColors.Window.B);
			_088B = Color.FromArgb(red, green, blue);
			if (_089F.न <= 16)
			{
				_088C = SystemColors.ControlLightLight;
			}
			else
			{
				red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.2 + (double)(int)Color.White.R);
				green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.2 + (double)(int)Color.White.G);
				blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.2 + (double)(int)Color.White.B);
				_088C = Color.FromArgb(red, green, blue);
			}
			red = (int)((double)(int)SystemColors.ControlDark.R * 0.8);
			green = (int)((double)(int)SystemColors.ControlDark.G * 0.8);
			blue = (int)((double)(int)SystemColors.ControlDark.B * 0.8);
			_088D = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.25 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.25 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.25 + (double)(int)SystemColors.Window.B);
			_088E = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.8 + (double)(int)Color.White.R);
			green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.8 + (double)(int)Color.White.G);
			blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.8 + (double)(int)Color.White.B);
			_088F = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.5 + (double)(int)Color.White.R);
			green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.5 + (double)(int)Color.White.G);
			blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.5 + (double)(int)Color.White.B);
			_0891 = Color.FromArgb(red, green, blue);
			_0890 = SystemColors.Control;
		}
		else
		{
			_0888 = SystemColors.ControlLightLight;
			_0889 = _0888;
			_088A = _0888;
			_088B = SystemColors.ControlLight;
			_088C = SystemColors.ControlLightLight;
			_088D = SystemColors.ControlDark;
			_088E = SystemColors.ControlLight;
			_088F = SystemColors.ControlLight;
			_0891 = SystemColors.ControlLight;
			_0890 = SystemColors.Control;
		}
		if (_0892 != null)
		{
			((Image)_0892).Dispose();
			_0892 = new Bitmap(2, 2);
			_0892.SetPixel(0, 0, SystemColors.Control);
			_0892.SetPixel(1, 0, SystemColors.ControlLightLight);
			_0892.SetPixel(0, 1, SystemColors.ControlLightLight);
			_0892.SetPixel(1, 1, SystemColors.Control);
		}
	}

	public static Color GetShade(Color color, int shadePercent)
	{
		HLSColor hLSColor = RGBToHSL(color);
		if (hLSColor.Lightness == 0.0)
		{
			return HLSToRGB(hLSColor.Hue, shadePercent / 100, hLSColor.Saturation);
		}
		return HLSToRGB(hLSColor.Hue, Math.Max(0.0, hLSColor.Lightness - (double)(shadePercent / 100)), hLSColor.Saturation);
	}

	public static HLSColor RGBToHSL(int Red, int Green, int Blue)
	{
		HLSColor result = default(HLSColor);
		double num = (double)Red / 255.0;
		double num2 = (double)Green / 255.0;
		double num3 = (double)Blue / 255.0;
		double num4 = _0896(num, num2, num3);
		double num5 = _089A(num, num2, num3);
		double num6 = num4 - num5;
		result.Lightness = (num4 + num5) / 2.0;
		if (num4 == num5)
		{
			result.Saturation = 0.0;
			result.Hue = 0.0;
		}
		else
		{
			if (result.Lightness <= 0.5)
			{
				result.Saturation = (num4 - num5) / (num4 + num5);
			}
			else
			{
				result.Saturation = (num4 - num5) / (2.0 - num4 - num5);
			}
			double num7 = ((num4 - num) / 6.0 + num6 / 2.0) / num6;
			double num8 = ((num4 - num2) / 6.0 + num6 / 2.0) / num6;
			double num9 = ((num4 - num3) / 6.0 + num6 / 2.0) / num6;
			if (num == num4)
			{
				result.Hue = num9 - num8;
			}
			else if (num2 == num4)
			{
				result.Hue = 1.0 / 3.0 + num7 - num9;
			}
			else if (num3 == num4)
			{
				result.Hue = 2.0 / 3.0 + num8 - num7;
			}
			if (result.Hue < 0.0)
			{
				result.Hue += 1.0;
			}
			if (result.Hue > 1.0)
			{
				result.Hue -= 1.0;
			}
		}
		return result;
	}

	public static HLSColor RGBToHSL(Color color)
	{
		return RGBToHSL(color.R, color.G, color.B);
	}

	private static double _0896(double _0897, double _0898, double _0899)
	{
		double num = 0.0;
		if (_0897 > _0898)
		{
			if (_0897 > _0899)
			{
				return _0897;
			}
			return _0899;
		}
		if (_0899 > _0898)
		{
			return _0899;
		}
		return _0898;
	}

	private static double _089A(double _0897, double _0898, double _0899)
	{
		double num = 0.0;
		if (_0897 < _0898)
		{
			if (_0897 < _0899)
			{
				return _0897;
			}
			return _0899;
		}
		if (_0899 < _0898)
		{
			return _0899;
		}
		return _0898;
	}

	public static Color HLSToRGB(double Hue, double Lightness, double Saturation)
	{
		if (Lightness < 0.0)
		{
			Lightness = 0.0;
		}
		else if (Lightness > 1.0)
		{
			Lightness = 1.0;
		}
		if (Saturation < 0.0)
		{
			Saturation = 0.0;
		}
		else if (Saturation > 1.0)
		{
			Saturation = 1.0;
		}
		int red;
		int green;
		int blue;
		if (Saturation == 0.0)
		{
			red = (int)(Lightness * 255.0);
			green = (int)(Lightness * 255.0);
			blue = (int)(Lightness * 255.0);
		}
		else
		{
			double num = ((!(Lightness < 0.5)) ? (Lightness + Saturation - Saturation * Lightness) : (Lightness * (1.0 + Saturation)));
			double num2 = 2.0 * Lightness - num;
			red = (int)(255.0 * _089B(num2, num, Hue + 1.0 / 3.0));
			green = (int)(255.0 * _089B(num2, num, Hue));
			blue = (int)(255.0 * _089B(num2, num, Hue - 1.0 / 3.0));
		}
		return Color.FromArgb(red, green, blue);
	}

	private static double _089B(double _089C, double _089D, double _089E)
	{
		if (_089E < 0.0)
		{
			_089E += 1.0;
		}
		if (_089E > 1.0)
		{
			_089E -= 1.0;
		}
		if (6.0 * _089E < 1.0)
		{
			return _089C + (_089D - _089C) * 6.0 * _089E;
		}
		if (2.0 * _089E < 1.0)
		{
			return _089D;
		}
		if (3.0 * _089E < 2.0)
		{
			return _089C + (_089D - _089C) * ((2.0 / 3.0 - _089E) * 6.0);
		}
		return _089C;
	}

	public static Color HLSToRGB(HLSColor clr)
	{
		return HLSToRGB(clr.Hue, clr.Lightness, clr.Saturation);
	}

	public static Color HoverBackColor()
	{
		return _0888;
	}

	public static Color HoverBackColor2()
	{
		return _0889;
	}

	public static Color HoverBackColor3()
	{
		return _088A;
	}

	public static Color HoverBackColor(Graphics g)
	{
		return g.GetNearestColor(HoverBackColor());
	}

	public static Color PressedBackColor()
	{
		return _088E;
	}

	public static Color PressedBackColor(Graphics g)
	{
		return g.GetNearestColor(PressedBackColor());
	}

	public static Color CheckBoxBackColor()
	{
		return _088B;
	}

	public static Color CheckBoxBackColor(Graphics g)
	{
		return g.GetNearestColor(CheckBoxBackColor());
	}

	public static Color ToolMenuFocusBackColor()
	{
		return _088F;
	}

	public static Color RecentlyUsedOfficeBackColor()
	{
		return _0891;
	}

	public static Color RecentlyUsedOfficeBackColor(Graphics g)
	{
		return g.GetNearestColor(RecentlyUsedOfficeBackColor());
	}

	public static Color SideRecentlyBackColor(Graphics g)
	{
		return g.GetNearestColor(SideRecentlyBackColor());
	}

	public static Color SideRecentlyBackColor()
	{
		return _0890;
	}

	public static Color ToolMenuFocusBackColor(Graphics g)
	{
		return g.GetNearestColor(ToolMenuFocusBackColor());
	}

	public static Color MenuFocusBorderColor()
	{
		return _088D;
	}

	public static Color MenuFocusBorderColor(Graphics g)
	{
		return g.GetNearestColor(MenuFocusBorderColor());
	}

	public static Color MenuBackColor()
	{
		return _088C;
	}

	public static Color MenuBackColor(Graphics g)
	{
		return g.GetNearestColor(MenuBackColor());
	}

	public static Brush GetPushedBrush(BaseItem item)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		if (item.Parent is GenericItemContainer)
		{
			if (!((GenericItemContainer)item.Parent).m_BackgroundColor.IsEmpty)
			{
				return (Brush)new SolidBrush(((GenericItemContainer)item.Parent).m_BackgroundColor);
			}
		}
		else if (item.Parent is SideBarPanelItem)
		{
			if (((SideBarPanelItem)item.Parent).BackgroundStyle.ShouldSerializeBackColor1())
			{
				return (Brush)new SolidBrush(((SideBarPanelItem)item.Parent).BackgroundStyle.BackColor1.Color);
			}
			if (((SideBarPanelItem)item.Parent).BackgroundStyle.ShouldSerializeBackColor1())
			{
				return (Brush)new SolidBrush(((SideBarPanelItem)item.Parent).BackgroundStyle.BackColor1.Color);
			}
		}
		return (Brush)(object)GetPushedBrush();
	}

	public static TextureBrush GetPushedBrush()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (_0892 == null)
		{
			_0892 = new Bitmap(2, 2);
			_0892.SetPixel(0, 0, SystemColors.Control);
			_0892.SetPixel(1, 0, SystemColors.ControlLightLight);
			_0892.SetPixel(0, 1, SystemColors.ControlLightLight);
			_0892.SetPixel(1, 1, SystemColors.Control);
		}
		return new TextureBrush((Image)(object)_0892);
	}

	public static bool IsEqual(Color color1, Color color2)
	{
		if (color1.A == color2.A && color1.R == color2.R && color1.G == color2.G)
		{
			return color1.B == color2.B;
		}
		return false;
	}
}
