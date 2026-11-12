namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTreeViewParts : ThemePart
{
	public static ThemeTreeViewParts TreeItem = new ThemeTreeViewParts(1);

	public static ThemeTreeViewParts Glyph = new ThemeTreeViewParts(1);

	public static ThemeTreeViewParts Branch = new ThemeTreeViewParts(1);

	protected ThemeTreeViewParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
