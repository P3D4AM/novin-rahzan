namespace DevComponents.Editors.DateTimeAdv;

public class NumericMonthInput : VisualIntegerInput, IDateTimePartInput
{
	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.Month;

	public NumericMonthInput()
	{
		base.MinValue = 1;
		base.MaxValue = 12;
	}
}
