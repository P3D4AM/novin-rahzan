namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeToolbarStates : ThemeState
{
	public static ThemeToolbarStates Normal = new ThemeToolbarStates(1);

	public static ThemeToolbarStates Hot = new ThemeToolbarStates(2);

	public static ThemeToolbarStates Pressed = new ThemeToolbarStates(3);

	public static ThemeToolbarStates Disabled = new ThemeToolbarStates(4);

	public static ThemeToolbarStates Checked = new ThemeToolbarStates(5);

	public static ThemeToolbarStates HotChecked = new ThemeToolbarStates(6);

	protected ThemeToolbarStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
