namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemePageParts : ThemePart
{
	public static ThemePageParts Up = new ThemePageParts(1);

	public static ThemePageParts Down = new ThemePageParts(2);

	public static ThemePageParts UpHorz = new ThemePageParts(3);

	public static ThemePageParts DownHorz = new ThemePageParts(4);

	protected ThemePageParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
