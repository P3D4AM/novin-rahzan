namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemeButtonStates : ThemeState
{
	public static ThemeButtonStates PushButtonNormal = new ThemeButtonStates(1);

	public static ThemeButtonStates PushButtonHot = new ThemeButtonStates(2);

	public static ThemeButtonStates PushButtonPressed = new ThemeButtonStates(3);

	public static ThemeButtonStates PushButtonDisabled = new ThemeButtonStates(4);

	public static ThemeButtonStates PushButtonDefaulted = new ThemeButtonStates(5);

	public static ThemeButtonStates RadioButtonUncheckedNormal = new ThemeButtonStates(1);

	public static ThemeButtonStates RadioButtonUncheckedHot = new ThemeButtonStates(2);

	public static ThemeButtonStates RadioButtonUncheckedPressed = new ThemeButtonStates(3);

	public static ThemeButtonStates RadioButtonUncheckedDisabled = new ThemeButtonStates(4);

	public static ThemeButtonStates RadioButtonCheckedNormal = new ThemeButtonStates(5);

	public static ThemeButtonStates RadioButtonCheckedHot = new ThemeButtonStates(6);

	public static ThemeButtonStates RadioButtonCheckedPressed = new ThemeButtonStates(7);

	public static ThemeButtonStates RadioButtonCheckedDisabled = new ThemeButtonStates(8);

	public static ThemeButtonStates CheckBoxUncheckedNormal = new ThemeButtonStates(1);

	public static ThemeButtonStates CheckBoxUncheckedHot = new ThemeButtonStates(2);

	public static ThemeButtonStates CheckBoxUncheckedPressed = new ThemeButtonStates(3);

	public static ThemeButtonStates CheckBoxUncheckedDisabled = new ThemeButtonStates(4);

	public static ThemeButtonStates CheckBoxCheckedNormal = new ThemeButtonStates(5);

	public static ThemeButtonStates CheckBoxCheckedHot = new ThemeButtonStates(6);

	public static ThemeButtonStates CheckBoxCheckedPressed = new ThemeButtonStates(7);

	public static ThemeButtonStates CheckBoxCheckedDisabled = new ThemeButtonStates(8);

	public static ThemeButtonStates CheckBoxMixedNormal = new ThemeButtonStates(9);

	public static ThemeButtonStates CheckBoxMixedHot = new ThemeButtonStates(10);

	public static ThemeButtonStates CheckBoxMixedPressed = new ThemeButtonStates(11);

	public static ThemeButtonStates CheckBoxMixedDisabled = new ThemeButtonStates(12);

	public static ThemeButtonStates GroupBoxNormal = new ThemeButtonStates(1);

	public static ThemeButtonStates GroupBoxDisabled = new ThemeButtonStates(2);

	protected ThemeButtonStates(int themestatevalue)
		: base(themestatevalue)
	{
	}
}
