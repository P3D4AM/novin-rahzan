namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTaskBarParts : ThemePart
{
	public static ThemeTaskBarParts BackgroundBottom = new ThemeTaskBarParts(1);

	public static ThemeTaskBarParts BackgroundRight = new ThemeTaskBarParts(2);

	public static ThemeTaskBarParts BackgroundTop = new ThemeTaskBarParts(3);

	public static ThemeTaskBarParts BackgroundLeft = new ThemeTaskBarParts(4);

	public static ThemeTaskBarParts SizingBarBottom = new ThemeTaskBarParts(5);

	public static ThemeTaskBarParts SizingBarRight = new ThemeTaskBarParts(6);

	public static ThemeTaskBarParts SizingBarTop = new ThemeTaskBarParts(7);

	public static ThemeTaskBarParts SizingBarLeft = new ThemeTaskBarParts(8);

	protected ThemeTaskBarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
