namespace DevComponents.Editors.DateTimeAdv;

public class NumericSecondInput : VisualIntegerInput, IDateTimePartInput
{
	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Second;

	public NumericSecondInput()
	{
		base.MinValue = 0;
		base.MaxValue = 59;
	}
}
