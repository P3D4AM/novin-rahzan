namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeHeaderParts : ThemePart
{
	public static ThemeHeaderParts HeaderItem = new ThemeHeaderParts(1);

	public static ThemeHeaderParts HeaderItemLeft = new ThemeHeaderParts(2);

	public static ThemeHeaderParts HeaderItemRight = new ThemeHeaderParts(3);

	public static ThemeHeaderParts HeaderSortArrow = new ThemeHeaderParts(4);

	protected ThemeHeaderParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
