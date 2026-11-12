namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeListViewParts : ThemePart
{
	public static ThemeListViewParts ListItem = new ThemeListViewParts(1);

	public static ThemeListViewParts ListGroup = new ThemeListViewParts(2);

	public static ThemeListViewParts ListDetail = new ThemeListViewParts(3);

	public static ThemeListViewParts ListSortedDetail = new ThemeListViewParts(4);

	public static ThemeListViewParts EmptyText = new ThemeListViewParts(5);

	protected ThemeListViewParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
