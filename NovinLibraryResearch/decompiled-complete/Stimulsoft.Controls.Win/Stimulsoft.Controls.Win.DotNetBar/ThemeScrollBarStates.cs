namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeScrollBarStates : ThemeState
{
	public static ThemeScrollBarStates ArrowBtnUpNormal = new ThemeScrollBarStates(1);

	public static ThemeScrollBarStates ArrowBtnUpHot = new ThemeScrollBarStates(2);

	public static ThemeScrollBarStates ArrowBtnUpPressed = new ThemeScrollBarStates(3);

	public static ThemeScrollBarStates ArrowBtnUpDisabled = new ThemeScrollBarStates(4);

	public static ThemeScrollBarStates ArrowBtnDownNormal = new ThemeScrollBarStates(5);

	public static ThemeScrollBarStates ArrowBtnDownHot = new ThemeScrollBarStates(6);

	public static ThemeScrollBarStates ArrowBtnDownPressed = new ThemeScrollBarStates(7);

	public static ThemeScrollBarStates ArrowBtnDownDisabled = new ThemeScrollBarStates(8);

	public static ThemeScrollBarStates ArrowBtnLeftNormal = new ThemeScrollBarStates(9);

	public static ThemeScrollBarStates ArrowBtnLeftHot = new ThemeScrollBarStates(10);

	public static ThemeScrollBarStates ArrowBtnLeftPressed = new ThemeScrollBarStates(11);

	public static ThemeScrollBarStates ArrowBtnLeftDisabled = new ThemeScrollBarStates(12);

	public static ThemeScrollBarStates ArrowBtnRightNormal = new ThemeScrollBarStates(13);

	public static ThemeScrollBarStates ArrowBtnRightHot = new ThemeScrollBarStates(14);

	public static ThemeScrollBarStates ArrowBtnRightPressed = new ThemeScrollBarStates(15);

	public static ThemeScrollBarStates ArrowBtnRightDisabled = new ThemeScrollBarStates(16);

	public static ThemeScrollBarStates GripperHorzNormal = new ThemeScrollBarStates(0);

	public static ThemeScrollBarStates GripperVertNormal = new ThemeScrollBarStates(0);

	public static ThemeScrollBarStates TrackNormal = new ThemeScrollBarStates(1);

	public static ThemeScrollBarStates TrackHot = new ThemeScrollBarStates(2);

	public static ThemeScrollBarStates TrackPressed = new ThemeScrollBarStates(3);

	public static ThemeScrollBarStates TrackDisabled = new ThemeScrollBarStates(4);

	public static ThemeScrollBarStates ThumbNormal = new ThemeScrollBarStates(1);

	public static ThemeScrollBarStates ThumbHot = new ThemeScrollBarStates(2);

	public static ThemeScrollBarStates ThumbPressed = new ThemeScrollBarStates(3);

	public static ThemeScrollBarStates ThumbDisabled = new ThemeScrollBarStates(4);

	public static ThemeScrollBarStates SizeBoxRightAlign = new ThemeScrollBarStates(1);

	public static ThemeScrollBarStates SizeBoxLeftAlign = new ThemeScrollBarStates(2);

	protected ThemeScrollBarStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
