namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTabStates : ThemeState
{
	public static ThemeTabStates BodyNormal = new ThemeTabStates(0);

	public static ThemeTabStates PaneNormal = new ThemeTabStates(0);

	public static ThemeTabStates Normal = new ThemeTabStates(1);

	public static ThemeTabStates Hot = new ThemeTabStates(2);

	public static ThemeTabStates Selected = new ThemeTabStates(3);

	public static ThemeTabStates Disabled = new ThemeTabStates(4);

	public static ThemeTabStates Focused = new ThemeTabStates(4);

	protected ThemeTabStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
