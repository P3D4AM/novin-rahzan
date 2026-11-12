using System;

namespace Stimulsoft.Base.Wpf;

public static class StiWpfSkinHelper
{
	internal static bool IsWinFormsMode { get; set; }

	public static StiSkinBackground CurrentBackground { get; private set; }

	public static StiSkinForeground CurrentForeground { get; private set; }

	public static event EventHandler SkinChanged;

	static StiWpfSkinHelper()
	{
		StiGuiParameters stiGuiParameters = StiGuiParameters.LoadParameters();
		if (stiGuiParameters != null)
		{
			CurrentBackground = stiGuiParameters.Theme;
			CurrentForeground = stiGuiParameters.Style;
		}
		else
		{
			CurrentBackground = StiSkinBackground.White;
			CurrentForeground = StiSkinForeground.Blue;
			StiGuiParameters.SaveThemeAndStyle(CurrentBackground, CurrentForeground);
		}
	}

	public static void SetNewSkin(StiSkinBackground background, StiSkinForeground foreground)
	{
		CurrentBackground = background;
		CurrentForeground = foreground;
		StiGuiParameters.SaveThemeAndStyle(background, foreground);
		StiWpfSkinHelper.SkinChanged?.Invoke(null, null);
	}
}
