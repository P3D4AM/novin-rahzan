namespace Stimulsoft.Report;

public class StiStyleConditionComponentNameElement : StiStyleConditionElement
{
	public StiStyleConditionOperation OperationComponentName { get; set; }

	public string ComponentName { get; set; }

	public StiStyleConditionComponentNameElement(string componentName)
		: this(componentName, StiStyleConditionOperation.EqualTo)
	{
	}

	public StiStyleConditionComponentNameElement(string componentName, StiStyleConditionOperation operationComponentName)
	{
		ComponentName = componentName;
		OperationComponentName = operationComponentName;
	}
}
