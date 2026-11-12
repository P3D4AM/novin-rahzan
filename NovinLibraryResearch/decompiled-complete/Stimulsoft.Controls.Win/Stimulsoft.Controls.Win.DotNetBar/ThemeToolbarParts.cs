namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeToolbarParts : ThemePart
{
	public static ThemeToolbarParts Button = new ThemeToolbarParts(1);

	public static ThemeToolbarParts DropDownButton = new ThemeToolbarParts(2);

	public static ThemeToolbarParts SplitButton = new ThemeToolbarParts(3);

	public static ThemeToolbarParts SplitButtonDropDown = new ThemeToolbarParts(4);

	public static ThemeToolbarParts Separator = new ThemeToolbarParts(5);

	public static ThemeToolbarParts SeparatorVert = new ThemeToolbarParts(6);

	protected ThemeToolbarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
