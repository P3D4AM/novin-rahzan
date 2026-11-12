namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeSpinStates : ThemeState
{
	public static ThemeSpinStates Normal = new ThemeSpinStates(1);

	public static ThemeSpinStates Hot = new ThemeSpinStates(2);

	public static ThemeSpinStates Pressed = new ThemeSpinStates(3);

	public static ThemeSpinStates Disabled = new ThemeSpinStates(4);

	protected ThemeSpinStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
