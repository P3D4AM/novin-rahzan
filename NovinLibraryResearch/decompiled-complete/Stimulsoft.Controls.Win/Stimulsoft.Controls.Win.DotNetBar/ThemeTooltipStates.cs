namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTooltipStates : ThemeState
{
	public static ThemeTooltipStates StandardNormal = new ThemeTooltipStates(1);

	public static ThemeTooltipStates StandardLink = new ThemeTooltipStates(2);

	public static ThemeTooltipStates BalloonNormal = new ThemeTooltipStates(1);

	public static ThemeTooltipStates BalloonLink = new ThemeTooltipStates(2);

	public static ThemeTooltipStates CloseNormal = new ThemeTooltipStates(1);

	public static ThemeTooltipStates CloseHot = new ThemeTooltipStates(2);

	public static ThemeTooltipStates ClosePressed = new ThemeTooltipStates(3);

	protected ThemeTooltipStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
