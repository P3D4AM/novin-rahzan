namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeRebarStates : ThemeState
{
	public static ThemeRebarStates Normal = new ThemeRebarStates(0);

	public static ThemeRebarStates ChevronNormal = new ThemeRebarStates(1);

	public static ThemeRebarStates ChevronHot = new ThemeRebarStates(2);

	public static ThemeRebarStates ChevronPressed = new ThemeRebarStates(3);

	protected ThemeRebarStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
