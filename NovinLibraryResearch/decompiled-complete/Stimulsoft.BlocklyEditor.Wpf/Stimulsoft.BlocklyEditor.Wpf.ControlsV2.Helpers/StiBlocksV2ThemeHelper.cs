using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Stimulsoft.Base.Wpf;

namespace Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;

internal static class StiBlocksV2ThemeHelper
{
	private class StiBlocksV2ResourceDictionary : ResourceDictionary
	{
	}

	private static int count;

	private static bool[] states;

	private static StiBlocksV2ResourceDictionary currentResourceDictionary;

	private static ResourceDictionary styleResourceDictionary;

	static StiBlocksV2ThemeHelper()
	{
		Array values = Enum.GetValues(typeof(StiBlocksV2ControlID));
		states = new bool[values.Length];
		StiWpfSkinHelper.SkinChanged += SkinHelper_SkinChanged;
	}

	private static void SkinHelper_SkinChanged(object sender, EventArgs e)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		Array values = Enum.GetValues(typeof(StiBlocksV2ControlID));
		((ResourceDictionary)currentResourceDictionary).BeginInit();
		((ResourceDictionary)currentResourceDictionary).MergedDictionaries.Clear();
		styleResourceDictionary = null;
		StiSkinForeground currentForeground = StiWpfSkinHelper.CurrentForeground;
		StiSkinBackground currentBackground = StiWpfSkinHelper.CurrentBackground;
		string uriString = $"/Stimulsoft.BlocklyEditor.Wpf;component/Themes/Skins/{currentForeground}{currentBackground}.xaml";
		styleResourceDictionary = new ResourceDictionary
		{
			Source = new Uri(uriString, UriKind.RelativeOrAbsolute)
		};
		foreach (StiBlocksV2ControlID item in values)
		{
			if (states[(int)item])
			{
				ResourceDictionary val = new ResourceDictionary
				{
					Source = new Uri($"/Stimulsoft.BlocklyEditor.Wpf;component/Themes/V2/{item}.xaml", UriKind.RelativeOrAbsolute)
				};
				InitResourcesForOffice2013Theme(val);
				((ResourceDictionary)currentResourceDictionary).MergedDictionaries.Add(val);
			}
		}
		((ResourceDictionary)currentResourceDictionary).EndInit();
	}

	internal static ResourceDictionary LoadControlTheme(IStiBlocksV2Control control)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		StiBlocksV2ControlID v2ID = control.GetV2ID();
		if (states[(int)v2ID])
		{
			return null;
		}
		states[(int)v2ID] = true;
		count++;
		if (currentResourceDictionary == null)
		{
			if (Application.Current != null)
			{
				foreach (ResourceDictionary mergedDictionary in Application.Current.Resources.MergedDictionaries)
				{
					if (mergedDictionary is StiBlocksV2ResourceDictionary)
					{
						currentResourceDictionary = (StiBlocksV2ResourceDictionary)(object)mergedDictionary;
						break;
					}
				}
				if (currentResourceDictionary == null)
				{
					currentResourceDictionary = new StiBlocksV2ResourceDictionary();
					Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)(object)currentResourceDictionary);
				}
			}
			else
			{
				currentResourceDictionary = new StiBlocksV2ResourceDictionary();
			}
		}
		ResourceDictionary val = new ResourceDictionary
		{
			Source = new Uri($"/Stimulsoft.BlocklyEditor.Wpf;component/Themes/V2/{v2ID}.xaml", UriKind.RelativeOrAbsolute)
		};
		InitResourcesForOffice2013Theme(val);
		((ResourceDictionary)currentResourceDictionary).MergedDictionaries.Add(val);
		return val;
	}

	private static void InitResourcesForOffice2013Theme(ResourceDictionary dict1)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		if (styleResourceDictionary == null)
		{
			StiSkinBackground currentBackground = StiWpfSkinHelper.CurrentBackground;
			StiSkinForeground stiSkinForeground = StiWpfSkinHelper.CurrentForeground;
			if (StiWpfSkinHelper.IsWinFormsMode)
			{
				stiSkinForeground = StiSkinForeground.Blue;
			}
			string uriString = $"/Stimulsoft.BlocklyEditor.Wpf;component/Themes/Skins/{stiSkinForeground}{currentBackground}.xaml";
			styleResourceDictionary = new ResourceDictionary
			{
				Source = new Uri(uriString, UriKind.RelativeOrAbsolute)
			};
		}
		foreach (object key in styleResourceDictionary.Keys)
		{
			if (styleResourceDictionary[key] is SolidColorBrush)
			{
				((SolidColorBrush)dict1[key]).Color = ((SolidColorBrush)styleResourceDictionary[key]).Color;
			}
		}
		Color color = (Color)styleResourceDictionary[(object)"office15ColorButtonMOBackground"];
		Color color2 = (Color)styleResourceDictionary[(object)"office15ColorButtonPRBackground"];
		((SolidColorBrush)dict1[(object)"office15ButtonMOBackground"]).Color = color;
		((GradientBrush)(LinearGradientBrush)dict1[(object)"office15TabItemPressedBackground"]).GradientStops[0].Color = color;
		((SolidColorBrush)dict1[(object)"office15ToolBarBtnMOBackground"]).Color = color;
		((SolidColorBrush)dict1[(object)"office15ButtonPRBackground"]).Color = color2;
		((SolidColorBrush)dict1[(object)"office15ToolBarBtnPRBackground"]).Color = color2;
		((SolidColorBrush)dict1[(object)"office15AppColor1"]).Color = (Color)styleResourceDictionary[(object)"office15AppColor1"];
		((SolidColorBrush)dict1[(object)"office15AppColor2"]).Color = (Color)styleResourceDictionary[(object)"office15AppColor2"];
		((SolidColorBrush)dict1[(object)"PageBorderActiveColor"]).Color = (Color)styleResourceDictionary[(object)"PageBorderActiveColor"];
		((SolidColorBrush)dict1[(object)"PageBorderInactiveColor"]).Color = (Color)styleResourceDictionary[(object)"PageBorderInactiveColor"];
	}

	internal static void InitTheme(Control control)
	{
		if (control != null && Application.Current == null)
		{
			if (currentResourceDictionary == null)
			{
				currentResourceDictionary = new StiBlocksV2ResourceDictionary();
			}
			((FrameworkElement)control).Resources.MergedDictionaries.Add((ResourceDictionary)(object)currentResourceDictionary);
		}
	}
}
