namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeEditStates : ThemeState
{
	public static ThemeEditStates Normal = new ThemeEditStates(1);

	public static ThemeEditStates Hot = new ThemeEditStates(2);

	public static ThemeEditStates Selected = new ThemeEditStates(3);

	public static ThemeEditStates Disabled = new ThemeEditStates(4);

	public static ThemeEditStates Focused = new ThemeEditStates(5);

	public static ThemeEditStates ReadOnly = new ThemeEditStates(6);

	public static ThemeEditStates Assist = new ThemeEditStates(7);

	protected ThemeEditStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
