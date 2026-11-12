namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTrayNotifyParts : ThemePart
{
	public static ThemeTrayNotifyParts Background = new ThemeTrayNotifyParts(1);

	public static ThemeTrayNotifyParts AnimBackground = new ThemeTrayNotifyParts(2);

	protected ThemeTrayNotifyParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
