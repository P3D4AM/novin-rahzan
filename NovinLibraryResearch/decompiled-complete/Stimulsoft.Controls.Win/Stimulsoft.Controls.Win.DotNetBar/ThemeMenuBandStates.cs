namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenuBandStates : ThemeState
{
	public static ThemeMenuBandStates Normal = new ThemeMenuBandStates(1);

	public static ThemeMenuBandStates Hot = new ThemeMenuBandStates(2);

	public static ThemeMenuBandStates Pressed = new ThemeMenuBandStates(3);

	public static ThemeMenuBandStates Disabled = new ThemeMenuBandStates(4);

	public static ThemeMenuBandStates Checked = new ThemeMenuBandStates(5);

	public static ThemeMenuBandStates HotChecked = new ThemeMenuBandStates(6);

	protected ThemeMenuBandStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
