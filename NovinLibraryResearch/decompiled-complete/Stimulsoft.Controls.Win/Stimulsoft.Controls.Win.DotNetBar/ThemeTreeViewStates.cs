namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTreeViewStates : ThemeState
{
	public static ThemeTreeViewStates BranchNormal = new ThemeTreeViewStates(0);

	public static ThemeTreeViewStates GlyphClosed = new ThemeTreeViewStates(1);

	public static ThemeTreeViewStates GlyphOpen = new ThemeTreeViewStates(2);

	public static ThemeTreeViewStates TreeItemNormal = new ThemeTreeViewStates(1);

	public static ThemeTreeViewStates TreeItemHot = new ThemeTreeViewStates(2);

	public static ThemeTreeViewStates TreeItemSelected = new ThemeTreeViewStates(3);

	public static ThemeTreeViewStates TreeItemDisabled = new ThemeTreeViewStates(4);

	public static ThemeTreeViewStates TreeItemSelectedNotFocus = new ThemeTreeViewStates(5);

	protected ThemeTreeViewStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
