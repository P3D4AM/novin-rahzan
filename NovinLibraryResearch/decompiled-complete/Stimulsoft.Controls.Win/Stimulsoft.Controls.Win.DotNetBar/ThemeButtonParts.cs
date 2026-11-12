namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeButtonParts : ThemePart
{
	public static ThemeButtonParts PushButton = new ThemeButtonParts(1);

	public static ThemeButtonParts RadioButton = new ThemeButtonParts(2);

	public static ThemeButtonParts CheckBox = new ThemeButtonParts(3);

	public static ThemeButtonParts GroupBox = new ThemeButtonParts(4);

	public static ThemeButtonParts UserButton = new ThemeButtonParts(5);

	protected ThemeButtonParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
