namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeStartPanelParts : ThemePart
{
	public static ThemeStartPanelParts UserPane = new ThemeStartPanelParts(1);

	public static ThemeStartPanelParts MorePrograms = new ThemeStartPanelParts(2);

	public static ThemeStartPanelParts MoreProgramsArrow = new ThemeStartPanelParts(3);

	public static ThemeStartPanelParts ProgList = new ThemeStartPanelParts(4);

	public static ThemeStartPanelParts ProgListSeparator = new ThemeStartPanelParts(5);

	public static ThemeStartPanelParts PlacesList = new ThemeStartPanelParts(6);

	public static ThemeStartPanelParts PlacesListSeparator = new ThemeStartPanelParts(7);

	public static ThemeStartPanelParts LogOff = new ThemeStartPanelParts(8);

	public static ThemeStartPanelParts LogOffButtons = new ThemeStartPanelParts(9);

	public static ThemeStartPanelParts UserPicture = new ThemeStartPanelParts(10);

	public static ThemeStartPanelParts Preview = new ThemeStartPanelParts(11);

	protected ThemeStartPanelParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
