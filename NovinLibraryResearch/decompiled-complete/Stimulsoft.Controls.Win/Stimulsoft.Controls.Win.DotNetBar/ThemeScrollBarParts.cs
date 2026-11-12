namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeScrollBarParts : ThemePart
{
	public static ThemeScrollBarParts ArrowBtn = new ThemeScrollBarParts(1);

	public static ThemeScrollBarParts ThumbBtnHorz = new ThemeScrollBarParts(2);

	public static ThemeScrollBarParts ThumbBtnVert = new ThemeScrollBarParts(3);

	public static ThemeScrollBarParts LowerTrackHorz = new ThemeScrollBarParts(4);

	public static ThemeScrollBarParts UpperTrackHorz = new ThemeScrollBarParts(5);

	public static ThemeScrollBarParts LowerTrackVert = new ThemeScrollBarParts(6);

	public static ThemeScrollBarParts UpperTrackVert = new ThemeScrollBarParts(7);

	public static ThemeScrollBarParts GripperHorz = new ThemeScrollBarParts(8);

	public static ThemeScrollBarParts GripperVert = new ThemeScrollBarParts(9);

	public static ThemeScrollBarParts SizeBox = new ThemeScrollBarParts(10);

	protected ThemeScrollBarParts(int themepartvalue)
		: base(themepartvalue)
	{
	}
}
