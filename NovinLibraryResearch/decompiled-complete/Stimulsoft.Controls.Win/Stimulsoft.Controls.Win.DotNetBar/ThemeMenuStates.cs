namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeMenuStates : ThemeState
{
	public static ThemeMenuStates Normal = new ThemeMenuStates(1);

	public static ThemeMenuStates Selected = new ThemeMenuStates(2);

	public static ThemeMenuStates Demoted = new ThemeMenuStates(3);

	protected ThemeMenuStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
