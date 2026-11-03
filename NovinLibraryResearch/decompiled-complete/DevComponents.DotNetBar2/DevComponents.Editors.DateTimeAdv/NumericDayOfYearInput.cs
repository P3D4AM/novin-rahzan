namespace DevComponents.Editors.DateTimeAdv;

public class NumericDayOfYearInput : VisualIntegerInput, IDateTimePartInput
{
	eDateTimePart IDateTimePartInput._2551 => eDateTimePart.DayOfYear;

	public NumericDayOfYearInput()
	{
		base.MinValue = 1;
		base.MaxValue = 366;
	}
}
