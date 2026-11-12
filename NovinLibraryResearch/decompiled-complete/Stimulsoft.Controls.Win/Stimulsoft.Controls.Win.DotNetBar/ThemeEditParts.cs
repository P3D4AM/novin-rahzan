namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeEditParts : ThemePart
{
	public static ThemeEditParts EditText = new ThemeEditParts(1);

	public static ThemeEditParts Caret = new ThemeEditParts(2);

	protected ThemeEditParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
