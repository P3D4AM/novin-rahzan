namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenuBandParts : ThemePart
{
	public static ThemeMenuBandParts NewAppButton = new ThemeMenuBandParts(1);

	public static ThemeMenuBandParts Separator = new ThemeMenuBandParts(2);

	protected ThemeMenuBandParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
