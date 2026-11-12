namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTrackbarStates : ThemeState
{
	public static ThemeTrackbarStates TrackNormal = new ThemeTrackbarStates(1);

	public static ThemeTrackbarStates TicsNormal = new ThemeTrackbarStates(1);

	public static ThemeTrackbarStates ThumbNormal = new ThemeTrackbarStates(1);

	public static ThemeTrackbarStates ThumbHot = new ThemeTrackbarStates(2);

	public static ThemeTrackbarStates ThumbPressed = new ThemeTrackbarStates(3);

	public static ThemeTrackbarStates ThumbFocused = new ThemeTrackbarStates(4);

	public static ThemeTrackbarStates ThumbDisabled = new ThemeTrackbarStates(5);

	protected ThemeTrackbarStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
