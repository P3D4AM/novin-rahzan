namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenuParts : ThemePart
{
	public static ThemeMenuParts MenuItem = new ThemeMenuParts(1);

	public static ThemeMenuParts MenuDropDown = new ThemeMenuParts(2);

	public static ThemeMenuParts MenuBarItem = new ThemeMenuParts(3);

	public static ThemeMenuParts MenuBarDropDown = new ThemeMenuParts(4);

	public static ThemeMenuParts Chevron = new ThemeMenuParts(5);

	public static ThemeMenuParts Separator = new ThemeMenuParts(6);

	protected ThemeMenuParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
