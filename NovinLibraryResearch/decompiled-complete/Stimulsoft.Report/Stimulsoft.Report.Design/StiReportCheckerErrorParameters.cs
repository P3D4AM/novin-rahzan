namespace Stimulsoft.Report.Design;

public class StiReportCheckerErrorParameters
{
	public object Component { get; }

	public string CompName { get; }

	public string PropName { get; }

	public StiReportCheckerErrorParameters(string compName, string propName)
	{
		CompName = compName;
		PropName = propName;
	}

	public StiReportCheckerErrorParameters(string compName, string propName, object component)
	{
		CompName = compName;
		PropName = propName;
		Component = component;
	}
}
