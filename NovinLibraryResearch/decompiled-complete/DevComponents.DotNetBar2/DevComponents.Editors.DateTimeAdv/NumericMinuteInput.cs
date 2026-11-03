namespace DevComponents.Editors.DateTimeAdv;

public class NumericMinuteInput : VisualIntegerInput, IDateTimePartInput
{
	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Minute;

	public NumericMinuteInput()
	{
		base.MinValue = 0;
		base.MaxValue = 59;
	}
}
