using System;
using System.Drawing;
using Microsoft.Win32;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorFunctions
{
	public struct HLSColor
	{
		public double Hue;

		public double Lightness;

		public double Saturation;
	}

	private static Color m_HoverBackColor;

	private static Color m_HoverBackColor2;

	private static Color m_HoverBackColor3;

	private static Color m_CheckBoxBackColor;

	private static Color m_MenuBackColor;

	private static Color m_MenuFocusBorderColor;

	private static Color m_PressedBackColor;

	private static Color m_ToolMenuFocusBackColor;

	private static Color m_SideRecentlyBackColor;

	private static Color m_RecentlyUsedOfficeBackColor;

	private static Bitmap m_PushedBrushBmp;

	internal static bool ColorsLoaded;

	public static void LoadColors()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		RefreshColors();
		if (!ColorsLoaded)
		{
			SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(PreferenceChanged);
		}
		ColorsLoaded = true;
	}

	private static void PreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
	{
		RefreshColors();
		NativeFunctions.RefreshSettings();
		BarFunctions.RefreshScreens();
	}

	private static void RefreshColors()
	{
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		if (NativeFunctions.ColorDepth >= 16)
		{
			int red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.3 + (double)(int)SystemColors.Window.R);
			int green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.3 + (double)(int)SystemColors.Window.G);
			int blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.3 + (double)(int)SystemColors.Window.B);
			m_HoverBackColor = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.45 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.45 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.45 + (double)(int)SystemColors.Window.B);
			m_HoverBackColor2 = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.1 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.1 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.1 + (double)(int)SystemColors.Window.B);
			m_HoverBackColor3 = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.25 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.25 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.25 + (double)(int)SystemColors.Window.B);
			m_CheckBoxBackColor = Color.FromArgb(red, green, blue);
			if (NativeFunctions.ColorDepth <= 16)
			{
				m_MenuBackColor = SystemColors.ControlLightLight;
			}
			else
			{
				red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.2 + (double)(int)Color.White.R);
				green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.2 + (double)(int)Color.White.G);
				blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.2 + (double)(int)Color.White.B);
				m_MenuBackColor = Color.FromArgb(red, green, blue);
			}
			red = (int)((double)(int)SystemColors.ControlDark.R * 0.8);
			green = (int)((double)(int)SystemColors.ControlDark.G * 0.8);
			blue = (int)((double)(int)SystemColors.ControlDark.B * 0.8);
			m_MenuFocusBorderColor = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Highlight.R - SystemColors.Window.R) * 0.25 + (double)(int)SystemColors.Window.R);
			green = (int)((double)(SystemColors.Highlight.G - SystemColors.Window.G) * 0.25 + (double)(int)SystemColors.Window.G);
			blue = (int)((double)(SystemColors.Highlight.B - SystemColors.Window.B) * 0.25 + (double)(int)SystemColors.Window.B);
			m_PressedBackColor = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.8 + (double)(int)Color.White.R);
			green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.8 + (double)(int)Color.White.G);
			blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.8 + (double)(int)Color.White.B);
			m_ToolMenuFocusBackColor = Color.FromArgb(red, green, blue);
			red = (int)((double)(SystemColors.Control.R - Color.White.R) * 0.5 + (double)(int)Color.White.R);
			green = (int)((double)(SystemColors.Control.G - Color.White.G) * 0.5 + (double)(int)Color.White.G);
			blue = (int)((double)(SystemColors.Control.B - Color.White.B) * 0.5 + (double)(int)Color.White.B);
			m_RecentlyUsedOfficeBackColor = Color.FromArgb(red, green, blue);
			m_SideRecentlyBackColor = SystemColors.Control;
		}
		else
		{
			m_HoverBackColor = SystemColors.ControlLightLight;
			m_HoverBackColor2 = m_HoverBackColor;
			m_HoverBackColor3 = m_HoverBackColor;
			m_CheckBoxBackColor = SystemColors.ControlLight;
			m_MenuBackColor = SystemColors.ControlLightLight;
			m_MenuFocusBorderColor = SystemColors.ControlDark;
			m_PressedBackColor = SystemColors.ControlLight;
			m_ToolMenuFocusBackColor = SystemColors.ControlLight;
			m_RecentlyUsedOfficeBackColor = SystemColors.ControlLight;
			m_SideRecentlyBackColor = SystemColors.Control;
		}
		if (m_PushedBrushBmp != null)
		{
			((Image)m_PushedBrushBmp).Dispose();
			m_PushedBrushBmp = new Bitmap(2, 2);
			m_PushedBrushBmp.SetPixel(0, 0, SystemColors.Control);
			m_PushedBrushBmp.SetPixel(1, 0, SystemColors.ControlLightLight);
			m_PushedBrushBmp.SetPixel(0, 1, SystemColors.ControlLightLight);
			m_PushedBrushBmp.SetPixel(1, 1, SystemColors.Control);
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
		double num4 = Maximum(num, num2, num3);
		double num5 = Minimum(num, num2, num3);
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

	private static double Maximum(double rR, double rG, double rB)
	{
		double num = 0.0;
		if (rR > rG)
		{
			if (rR > rB)
			{
				return rR;
			}
			return rB;
		}
		if (rB > rG)
		{
			return rB;
		}
		return rG;
	}

	private static double Minimum(double rR, double rG, double rB)
	{
		double num = 0.0;
		if (rR < rG)
		{
			if (rR < rB)
			{
				return rR;
			}
			return rB;
		}
		if (rB < rG)
		{
			return rB;
		}
		return rG;
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
			double v = 2.0 * Lightness - num;
			red = (int)(255.0 * hue_2_rgb(v, num, Hue + 1.0 / 3.0));
			green = (int)(255.0 * hue_2_rgb(v, num, Hue));
			blue = (int)(255.0 * hue_2_rgb(v, num, Hue - 1.0 / 3.0));
		}
		return Color.FromArgb(red, green, blue);
	}

	private static double hue_2_rgb(double v1, double v2, double vh)
	{
		if (vh < 0.0)
		{
			vh += 1.0;
		}
		if (vh > 1.0)
		{
			vh -= 1.0;
		}
		if (6.0 * vh < 1.0)
		{
			return v1 + (v2 - v1) * 6.0 * vh;
		}
		if (2.0 * vh < 1.0)
		{
			return v2;
		}
		if (3.0 * vh < 2.0)
		{
			return v1 + (v2 - v1) * ((2.0 / 3.0 - vh) * 6.0);
		}
		return v1;
	}

	public static Color HLSToRGB(HLSColor clr)
	{
		return HLSToRGB(clr.Hue, clr.Lightness, clr.Saturation);
	}

	public static Color HoverBackColor()
	{
		return m_HoverBackColor;
	}

	public static Color HoverBackColor2()
	{
		return m_HoverBackColor2;
	}

	public static Color HoverBackColor3()
	{
		return m_HoverBackColor3;
	}

	public static Color HoverBackColor(Graphics g)
	{
		return g.GetNearestColor(HoverBackColor());
	}

	public static Color PressedBackColor()
	{
		return m_PressedBackColor;
	}

	public static Color PressedBackColor(Graphics g)
	{
		return g.GetNearestColor(PressedBackColor());
	}

	public static Color CheckBoxBackColor()
	{
		return m_CheckBoxBackColor;
	}

	public static Color CheckBoxBackColor(Graphics g)
	{
		return g.GetNearestColor(CheckBoxBackColor());
	}

	public static Color ToolMenuFocusBackColor()
	{
		return m_ToolMenuFocusBackColor;
	}

	public static Color RecentlyUsedOfficeBackColor()
	{
		return m_RecentlyUsedOfficeBackColor;
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
		return m_SideRecentlyBackColor;
	}

	public static Color ToolMenuFocusBackColor(Graphics g)
	{
		return g.GetNearestColor(ToolMenuFocusBackColor());
	}

	public static Color MenuFocusBorderColor()
	{
		return m_MenuFocusBorderColor;
	}

	public static Color MenuFocusBorderColor(Graphics g)
	{
		return g.GetNearestColor(MenuFocusBorderColor());
	}

	public static Color MenuBackColor()
	{
		return m_MenuBackColor;
	}

	public static Color MenuBackColor(Graphics g)
	{
		return g.GetNearestColor(MenuBackColor());
	}

	public static Brush GetPushedBrush(BaseItem item)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		if (item.Parent is GenericItemContainer)
		{
			if (!((GenericItemContainer)item.Parent).BackgroundColor.IsEmpty)
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
		if (m_PushedBrushBmp == null)
		{
			m_PushedBrushBmp = new Bitmap(2, 2);
			m_PushedBrushBmp.SetPixel(0, 0, SystemColors.Control);
			m_PushedBrushBmp.SetPixel(1, 0, SystemColors.ControlLightLight);
			m_PushedBrushBmp.SetPixel(0, 1, SystemColors.ControlLightLight);
			m_PushedBrushBmp.SetPixel(1, 1, SystemColors.Control);
		}
		return new TextureBrush((Image)(object)m_PushedBrushBmp);
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
