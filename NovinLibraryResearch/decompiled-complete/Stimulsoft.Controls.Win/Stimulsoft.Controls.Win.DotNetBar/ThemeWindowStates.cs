namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeWindowStates : ThemeState
{
	public static ThemeWindowStates Normal = new ThemeWindowStates(0);

	public static ThemeWindowStates CaptionActive = new ThemeWindowStates(1);

	public static ThemeWindowStates CaptionInactive = new ThemeWindowStates(2);

	public static ThemeWindowStates CaptionDisabled = new ThemeWindowStates(3);

	public static ThemeWindowStates FrameActive = new ThemeWindowStates(1);

	public static ThemeWindowStates FrameInactive = new ThemeWindowStates(2);

	public static ThemeWindowStates ButtonNormal = new ThemeWindowStates(1);

	public static ThemeWindowStates ButtonHot = new ThemeWindowStates(2);

	public static ThemeWindowStates ButtonPushed = new ThemeWindowStates(3);

	public static ThemeWindowStates ButtonDisabled = new ThemeWindowStates(4);

	public static ThemeWindowStates HorzScrollNormal = new ThemeWindowStates(1);

	public static ThemeWindowStates HorzScrollHot = new ThemeWindowStates(2);

	public static ThemeWindowStates HorzScrollPushed = new ThemeWindowStates(3);

	public static ThemeWindowStates HorzScrollDisabled = new ThemeWindowStates(4);

	public static ThemeWindowStates HorzThumbNormal = new ThemeWindowStates(1);

	public static ThemeWindowStates HorzThumbHot = new ThemeWindowStates(2);

	public static ThemeWindowStates HorzThumbPushed = new ThemeWindowStates(3);

	public static ThemeWindowStates HorzThumbDisabled = new ThemeWindowStates(4);

	public static ThemeWindowStates VertScrollNormal = new ThemeWindowStates(1);

	public static ThemeWindowStates VertScrollHot = new ThemeWindowStates(2);

	public static ThemeWindowStates VertScrollPushed = new ThemeWindowStates(3);

	public static ThemeWindowStates VertScrollDisabled = new ThemeWindowStates(4);

	public static ThemeWindowStates VertThumbNormal = new ThemeWindowStates(1);

	public static ThemeWindowStates VertThumbHot = new ThemeWindowStates(2);

	public static ThemeWindowStates VertThumbPushed = new ThemeWindowStates(3);

	public static ThemeWindowStates VertThumbDisabled = new ThemeWindowStates(4);

	protected ThemeWindowStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
