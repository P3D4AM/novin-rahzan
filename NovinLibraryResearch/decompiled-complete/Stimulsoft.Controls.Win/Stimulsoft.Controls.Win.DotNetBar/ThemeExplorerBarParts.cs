namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeExplorerBarParts : ThemePart
{
	public static ThemeExplorerBarParts HeaderBackground = new ThemeExplorerBarParts(1);

	public static ThemeExplorerBarParts HeaderClose = new ThemeExplorerBarParts(2);

	public static ThemeExplorerBarParts HeaderPin = new ThemeExplorerBarParts(3);

	public static ThemeExplorerBarParts IeBarMenu = new ThemeExplorerBarParts(4);

	public static ThemeExplorerBarParts NormalGroupBackground = new ThemeExplorerBarParts(5);

	public static ThemeExplorerBarParts NormalGroupCollapse = new ThemeExplorerBarParts(6);

	public static ThemeExplorerBarParts NormalGroupExpand = new ThemeExplorerBarParts(7);

	public static ThemeExplorerBarParts NormalGroupHead = new ThemeExplorerBarParts(8);

	public static ThemeExplorerBarParts SpecialGroupBackground = new ThemeExplorerBarParts(9);

	public static ThemeExplorerBarParts SpecialGroupCollapse = new ThemeExplorerBarParts(10);

	public static ThemeExplorerBarParts SpecialGroupExpand = new ThemeExplorerBarParts(11);

	public static ThemeExplorerBarParts SpecialGroupHead = new ThemeExplorerBarParts(12);

	protected ThemeExplorerBarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
