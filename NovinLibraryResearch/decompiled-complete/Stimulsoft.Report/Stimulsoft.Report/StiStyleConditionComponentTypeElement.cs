namespace Stimulsoft.Report;

public class StiStyleConditionComponentTypeElement : StiStyleConditionElement
{
	public StiStyleComponentType ComponentType { get; set; }

	public StiStyleConditionOperation OperationComponentType { get; set; }

	public StiStyleConditionComponentTypeElement(StiStyleComponentType componentType)
		: this(componentType, StiStyleConditionOperation.EqualTo)
	{
	}

	public StiStyleConditionComponentTypeElement(StiStyleComponentType componentType, StiStyleConditionOperation operationComponentType)
	{
		ComponentType = componentType;
		OperationComponentType = operationComponentType;
	}
}
