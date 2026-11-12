namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemePageStates : ThemeState
{
	public static ThemePageStates Normal = new ThemePageStates(1);

	public static ThemePageStates Hot = new ThemePageStates(2);

	public static ThemePageStates Pressed = new ThemePageStates(3);

	public static ThemePageStates Disabled = new ThemePageStates(4);

	protected ThemePageStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
