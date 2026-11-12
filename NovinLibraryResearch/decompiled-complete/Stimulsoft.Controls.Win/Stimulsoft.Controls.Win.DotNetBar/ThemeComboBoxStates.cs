namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeComboBoxStates : ThemeState
{
	public static ThemeComboBoxStates Normal = new ThemeComboBoxStates(1);

	public static ThemeComboBoxStates Hot = new ThemeComboBoxStates(2);

	public static ThemeComboBoxStates Pressed = new ThemeComboBoxStates(3);

	public static ThemeComboBoxStates Disabled = new ThemeComboBoxStates(4);

	protected ThemeComboBoxStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
