namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTabParts : ThemePart
{
	public static ThemeTabParts TabItem = new ThemeTabParts(1);

	public static ThemeTabParts TabItemLeftEdge = new ThemeTabParts(2);

	public static ThemeTabParts TabItemRightEdge = new ThemeTabParts(3);

	public static ThemeTabParts TabItemBothEdge = new ThemeTabParts(4);

	public static ThemeTabParts TopTabItem = new ThemeTabParts(5);

	public static ThemeTabParts TopTabItemLeftEdge = new ThemeTabParts(6);

	public static ThemeTabParts TopTabItemRightEdge = new ThemeTabParts(7);

	public static ThemeTabParts TopTabItemBothEdge = new ThemeTabParts(8);

	public static ThemeTabParts Pane = new ThemeTabParts(9);

	public static ThemeTabParts Body = new ThemeTabParts(10);

	protected ThemeTabParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
