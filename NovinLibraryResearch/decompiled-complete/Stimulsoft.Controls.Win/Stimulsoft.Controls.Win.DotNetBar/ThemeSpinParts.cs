namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeSpinParts : ThemePart
{
	public static ThemeSpinParts Up = new ThemeSpinParts(1);

	public static ThemeSpinParts Down = new ThemeSpinParts(2);

	public static ThemeSpinParts UpHorz = new ThemeSpinParts(3);

	public static ThemeSpinParts DownHorz = new ThemeSpinParts(4);

	protected ThemeSpinParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
