namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeStatusParts : ThemePart
{
	public static ThemeStatusParts Pane = new ThemeStatusParts(1);

	public static ThemeStatusParts GripperPane = new ThemeStatusParts(2);

	public static ThemeStatusParts Gripper = new ThemeStatusParts(3);

	protected ThemeStatusParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
