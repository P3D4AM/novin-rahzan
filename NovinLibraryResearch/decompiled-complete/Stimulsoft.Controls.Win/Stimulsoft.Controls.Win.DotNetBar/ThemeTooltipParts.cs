namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTooltipParts : ThemePart
{
	public static ThemeTooltipParts Standard = new ThemeTooltipParts(1);

	public static ThemeTooltipParts StandardTitle = new ThemeTooltipParts(2);

	public static ThemeTooltipParts Balloon = new ThemeTooltipParts(3);

	public static ThemeTooltipParts BalloonTitle = new ThemeTooltipParts(4);

	public static ThemeTooltipParts Close = new ThemeTooltipParts(5);

	protected ThemeTooltipParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
