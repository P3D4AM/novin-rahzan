using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Helpers;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

internal class MetroColorTableInitializer
{
	private const double DEGREE = 1.0 / 360.0;

	public static MetroColorTable CreateColorTable(MetroColorGeneratorParameters colorParams)
	{
		return CreateColorTable(colorParams.CanvasColor, colorParams.BaseColor);
	}

	public static MetroColorTable CreateColorTable(Color canvasColor, Color baseColor)
	{
		MetroColorTable metroColorTable = new MetroColorTable();
		Office2007ColorTable office2007ColorTable = new Office2007ColorTable();
		CreateColors(metroColorTable, office2007ColorTable, canvasColor, baseColor);
		if (StyleManager.Style == eStyle.Metro)
		{
			((Office2007Renderer)GlobalManager.Renderer).ColorTable = office2007ColorTable;
		}
		return metroColorTable;
	}

	private static double GetHue(double hue, int addDegrees)
	{
		hue += (double)addDegrees * (1.0 / 360.0);
		if (hue > 1.0)
		{
			hue -= 1.0;
		}
		else if (hue < 0.0)
		{
			hue += 1.0;
		}
		return hue;
	}

	private static double GetComplementSaturation(double saturation)
	{
		if (saturation == 1.0)
		{
			return 1.0;
		}
		if (saturation < 0.81)
		{
			return saturation + 0.1;
		}
		if (saturation < 0.91)
		{
			return saturation + 0.05;
		}
		return saturation + (0.99 - saturation);
	}

	public static MetroPartColors CreateMetroPartColors(Color canvasColor, Color baseColor)
	{
		ColorFunctions.HLSColor hLSColor = ColorFunctions.RGBToHSL(canvasColor);
		ColorFunctions.HLSColor hLSColor2 = ColorFunctions.RGBToHSL(baseColor);
		HSVColor hSVColor = ColorHelpers.ColorToHSV(baseColor);
		HSVColor hSVColor2 = ColorHelpers.ColorToHSV(canvasColor);
		MetroPartColors metroPartColors = new MetroPartColors();
		metroPartColors.CanvasColor = canvasColor;
		metroPartColors.BaseColor = baseColor;
		metroPartColors.TextColor = ((hLSColor.Lightness < 0.4) ? Color.White : Color.Black);
		metroPartColors.TextInactiveColor = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.White) ? (hSVColor2.Value + 0.1) : (hSVColor2.Value - 0.9));
		metroPartColors.TextDisabledColor = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.Black) ? (hSVColor2.Value - 0.33) : (hSVColor2.Value + 0.67));
		metroPartColors.TextLightColor = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.Black) ? (hSVColor2.Value - 0.6) : (hSVColor2.Value + 0.4));
		metroPartColors.CanvasColorDarkShade = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.Black) ? (hSVColor2.Value - 0.33) : (hSVColor2.Value + 0.67));
		metroPartColors.CanvasColorLightShade = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.White) ? (hSVColor2.Value + 0.17) : (hSVColor2.Value - 0.17));
		metroPartColors.CanvasColorLighterShade = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (metroPartColors.TextColor == Color.White) ? (hSVColor2.Value + 0.05) : (hSVColor2.Value - 0.05));
		metroPartColors.BaseTextColor = GetTextColor(baseColor);
		metroPartColors.BaseColorLight = ColorHelpers.HSVToColor(hSVColor.Hue, GetColorMin(0.08, hSVColor.Saturation - 0.41), hSVColor.Value + 0.3);
		metroPartColors.BaseColorLight1 = ColorHelpers.HSVToColor(hSVColor.Hue, GetColorMin(0.05, hSVColor.Saturation - 0.06), hSVColor.Value + 0.11);
		metroPartColors.BaseColorLightText = GetTextColor(metroPartColors.BaseColorLight);
		metroPartColors.BaseColorLightest = ColorHelpers.HSVToColor(hSVColor.Hue, GetColorMin(0.05, hSVColor.Saturation - 0.6), hSVColor.Value + 0.5);
		metroPartColors.BaseColorLighter = ColorHelpers.HSVToColor(hSVColor.Hue, GetColorMin(0.08, hSVColor.Saturation - 0.46), hSVColor.Value + 0.39);
		metroPartColors.BaseColorDark = ColorHelpers.HSVToColor(hSVColor.Hue, hSVColor.Saturation + 0.1, hSVColor.Value - 0.06);
		metroPartColors.BaseColorDarker = ColorFunctions.HLSToRGB(hLSColor2.Hue, hLSColor2.Lightness - 0.2, hLSColor2.Saturation);
		metroPartColors.ComplementColor = ColorHelpers.HSVToColor(GetComplementHue(hSVColor.Hue), hSVColor.Saturation, hSVColor.Value + ((hSVColor.Value > 0.5) ? (-0.35) : 0.0));
		ColorFunctions.HLSColor hLSColor3 = ColorFunctions.RGBToHSL(metroPartColors.ComplementColor);
		HSVColor hSVColor3 = ColorHelpers.ColorToHSV(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLight = ColorHelpers.HSVToColor(hSVColor3.Hue, hSVColor3.Saturation, hSVColor3.Value + 0.2);
		metroPartColors.ComplementColorDark = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.1, hLSColor3.Saturation);
		metroPartColors.ComplementColorDarker = ColorFunctions.HLSToRGB(hLSColor3.Hue, hLSColor3.Lightness - 0.2, hLSColor3.Saturation);
		metroPartColors.ComplementColorText = GetTextColor(metroPartColors.ComplementColor);
		metroPartColors.ComplementColorLightText = GetTextColor(metroPartColors.ComplementColorLight);
		metroPartColors.BaseButtonGradientStart = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (hSVColor2.Value > 0.0) ? (hSVColor2.Value - 0.01) : (hSVColor2.Value + 0.08));
		metroPartColors.BaseButtonGradientEnd = ColorHelpers.HSVToColor(hSVColor2.Hue, hSVColor2.Saturation, (hSVColor2.Value > 0.0) ? (hSVColor2.Value - 0.08) : (hSVColor2.Value + 0.2));
		return metroPartColors;
	}

	public static void CreateColors(MetroColorTable metroTable, Office2007ColorTable officeMetroColorTable, Color canvasColor, Color baseColor)
	{
		MetroPartColors metroPartColors = CreateMetroPartColors(canvasColor, baseColor);
		metroTable.CanvasColor = metroPartColors.CanvasColor;
		metroTable.CanvasColorShadeLight = metroPartColors.CanvasColorLightShade;
		metroTable.CanvasColorShadeLighter = metroPartColors.CanvasColorLighterShade;
		metroTable.ForeColor = metroPartColors.TextColor;
		metroTable.BaseColor = baseColor;
		metroTable.MetroPartColors = metroPartColors;
		metroTable.MetroAppForm.BorderThickness = new Thickness(0.0, 0.0, 1.0, 1.0);
		metroTable.MetroAppForm.BorderColors = new BorderColors(metroPartColors.BaseColor);
		metroTable.MetroAppForm.BorderColorsInactive = new BorderColors(metroPartColors.BaseColorLight);
		metroTable.MetroForm.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
		metroTable.MetroForm.BorderColors = new BorderColors[3]
		{
			new BorderColors(metroPartColors.BaseColorDark),
			new BorderColors(metroPartColors.BaseColor),
			new BorderColors(metroPartColors.BaseColor)
		};
		metroTable.MetroForm.BorderColorsInactive = new BorderColors[4]
		{
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight),
			new BorderColors(metroPartColors.BaseColorLight)
		};
		metroTable.MetroTab.ActiveCaptionText = metroPartColors.TextInactiveColor;
		metroTable.MetroTab.InactiveCaptionText = metroPartColors.TextDisabledColor;
		metroTable.MetroTab.MetroTabItem.Default = GetMetroTabItemStateTable(metroPartColors.TextInactiveColor);
		metroTable.MetroTab.MetroTabItem.Selected = GetMetroTabItemStateTable(metroPartColors.BaseColor);
		metroTable.MetroTab.MetroTabItem.Disabled = GetMetroTabItemStateTable(metroPartColors.TextDisabledColor);
		metroTable.MetroTab.MetroTabItem.MouseOver = GetMetroTabItemStateTable(metroPartColors.TextInactiveColor, metroPartColors.BaseColor);
		metroTable.MetroTab.MetroTabItem.Pressed = null;
		metroTable.MetroTab.TabPanelBackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		metroTable.MetroToolbar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.CanvasColor);
		metroTable.MetroStatusBar.BackgroundStyle = new ElementStyle(metroPartColors.TextColor, metroPartColors.BaseColor);
		metroTable.MetroStatusBar.TopBorders = new Color[0];
		metroTable.MetroStatusBar.BottomBorders = new Color[0];
		metroTable.MetroStatusBar.ResizeMarkerLightColor = Color.FromArgb(92, Color.White);
		metroTable.MetroStatusBar.ResizeMarkerColor = metroPartColors.BaseColorDark;
		MetroOfficeColorTableInitializer.InitializeColorTable(officeMetroColorTable, ColorFactory.Empty, metroPartColors);
	}

	private static double GetColorMin(double minValue, double value)
	{
		if (value < 0.0)
		{
			return minValue;
		}
		return value;
	}

	private static Color GetTextColor(Color color)
	{
		if (!(ColorFunctions.RGBToHSL(color).Lightness < 0.65))
		{
			return Color.Black;
		}
		return Color.White;
	}

	private static double GetComplementHue(double hue)
	{
		if (hue <= 0.37777)
		{
			return Math.Min(1.0, hue + (0.3805555555555556 + 43.0 / 180.0 * (hue / 0.37777)));
		}
		return Math.Max(0.0, hue - (0.3805555555555556 + 43.0 / 180.0 * (hue / 0.6222222)));
	}

	private static MetroTabItemStateColorTable GetMetroTabItemStateTable(Color textColor)
	{
		return GetMetroTabItemStateTable(textColor, Color.Empty);
	}

	private static MetroTabItemStateColorTable GetMetroTabItemStateTable(Color textColor, Color bottomBorderColor)
	{
		ElementStyle elementStyle = new ElementStyle(textColor);
		elementStyle.TextAlignment = eStyleTextAlignment.Center;
		elementStyle.TextLineAlignment = eStyleTextAlignment.Center;
		elementStyle.HideMnemonic = true;
		if (!bottomBorderColor.IsEmpty)
		{
			elementStyle.BorderBottom = eStyleBorderType.Solid;
			elementStyle.BorderBottomWidth = 2;
			elementStyle.BorderBottomColor = bottomBorderColor;
		}
		return new MetroTabItemStateColorTable(elementStyle);
	}
}
