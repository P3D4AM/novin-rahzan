namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeStartPanelStates : ThemeState
{
	public static ThemeStartPanelStates UserPaneNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates MoreProgramsNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates MoreProgramsArrowNormal = new ThemeStartPanelStates(1);

	public static ThemeStartPanelStates MoreProgramsArrowHot = new ThemeStartPanelStates(2);

	public static ThemeStartPanelStates MoreProgramsArrowPressed = new ThemeStartPanelStates(3);

	public static ThemeStartPanelStates ProgListNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates ProgListSeparatorNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates PlacesListNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates PlacesListSeparatorNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates LogOffNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates LogOffButtonsNormal = new ThemeStartPanelStates(1);

	public static ThemeStartPanelStates LogOffButtonsHot = new ThemeStartPanelStates(2);

	public static ThemeStartPanelStates LogOffButtonsPressed = new ThemeStartPanelStates(3);

	public static ThemeStartPanelStates UserPictureNormal = new ThemeStartPanelStates(0);

	public static ThemeStartPanelStates PreviewNormal = new ThemeStartPanelStates(0);

	protected ThemeStartPanelStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
