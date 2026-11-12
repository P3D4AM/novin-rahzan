namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTaskBandParts : ThemePart
{
	public static ThemeTaskBandParts GroupCount = new ThemeTaskBandParts(1);

	public static ThemeTaskBandParts FlashButton = new ThemeTaskBandParts(2);

	public static ThemeTaskBandParts FlashButtonGroupMenu = new ThemeTaskBandParts(3);

	protected ThemeTaskBandParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
