namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeListViewStates : ThemeState
{
	public static ThemeListViewStates ListGroupNormal = new ThemeListViewStates(1);

	public static ThemeListViewStates ListDetailNormal = new ThemeListViewStates(1);

	public static ThemeListViewStates ListSortedDetailNormal = new ThemeListViewStates(1);

	public static ThemeListViewStates EmptyTextNormal = new ThemeListViewStates(1);

	public static ThemeListViewStates ListItemNormal = new ThemeListViewStates(1);

	public static ThemeListViewStates ListItemHot = new ThemeListViewStates(2);

	public static ThemeListViewStates ListItemSelected = new ThemeListViewStates(3);

	public static ThemeListViewStates ListItemDisabled = new ThemeListViewStates(4);

	public static ThemeListViewStates ListItemSelectedNotFocus = new ThemeListViewStates(5);

	protected ThemeListViewStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
