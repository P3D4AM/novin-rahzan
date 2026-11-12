namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeHeaderStates : ThemeState
{
	public static ThemeHeaderStates ItemNormal = new ThemeHeaderStates(1);

	public static ThemeHeaderStates ItemHot = new ThemeHeaderStates(2);

	public static ThemeHeaderStates ItemPressed = new ThemeHeaderStates(3);

	public static ThemeHeaderStates SortArrowSortedUp = new ThemeHeaderStates(1);

	public static ThemeHeaderStates SortArrowSortedDown = new ThemeHeaderStates(2);

	protected ThemeHeaderStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
