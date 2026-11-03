namespace Stimulsoft.Report;

public class StiStyleConditionLocationElement : StiStyleConditionElement
{
	public StiStyleConditionOperation OperationLocation { get; set; }

	public StiStyleLocation Location { get; set; }

	public StiStyleConditionLocationElement(StiStyleLocation location)
		: this(location, StiStyleConditionOperation.EqualTo)
	{
	}

	public StiStyleConditionLocationElement(StiStyleLocation location, StiStyleConditionOperation operationLocation)
	{
		Location = location;
		OperationLocation = operationLocation;
	}
}
