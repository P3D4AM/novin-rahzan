namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeRebarParts : ThemePart
{
	public static ThemeRebarParts Background = new ThemeRebarParts(0);

	public static ThemeRebarParts Gripper = new ThemeRebarParts(1);

	public static ThemeRebarParts GripperVert = new ThemeRebarParts(2);

	public static ThemeRebarParts Band = new ThemeRebarParts(3);

	public static ThemeRebarParts Chevron = new ThemeRebarParts(4);

	public static ThemeRebarParts ChevronVert = new ThemeRebarParts(5);

	protected ThemeRebarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
