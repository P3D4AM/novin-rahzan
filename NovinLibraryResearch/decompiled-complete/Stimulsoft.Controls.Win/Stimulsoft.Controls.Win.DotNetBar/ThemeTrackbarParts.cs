namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeTrackbarParts : ThemePart
{
	public static ThemeTrackbarParts Track = new ThemeTrackbarParts(1);

	public static ThemeTrackbarParts TrackVert = new ThemeTrackbarParts(2);

	public static ThemeTrackbarParts Thumb = new ThemeTrackbarParts(3);

	public static ThemeTrackbarParts ThumbBottom = new ThemeTrackbarParts(4);

	public static ThemeTrackbarParts ThumbTop = new ThemeTrackbarParts(5);

	public static ThemeTrackbarParts ThumbVert = new ThemeTrackbarParts(6);

	public static ThemeTrackbarParts ThumbLeft = new ThemeTrackbarParts(7);

	public static ThemeTrackbarParts ThumbRight = new ThemeTrackbarParts(8);

	public static ThemeTrackbarParts Tics = new ThemeTrackbarParts(9);

	public static ThemeTrackbarParts TicsVert = new ThemeTrackbarParts(10);

	protected ThemeTrackbarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
