using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class ConfigurationItem
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ConfigurationItem");

	private string property;

	private object desiredValue;

	internal string Property
	{
		get
		{
			return property;
		}
		set
		{
			traceContext.TraceVerbose("Setting Property to: {0}", value);
			property = value;
		}
	}

	internal object DesiredValue
	{
		get
		{
			return desiredValue;
		}
		set
		{
			traceContext.TraceVerbose("Setting DesiredValue to: {0}", value);
			desiredValue = value;
		}
	}

	internal ConfigurationItem(string property, object desiredValue)
	{
		traceContext.TraceMethodEnter("ConfigurationItem");
		traceContext.TraceParameters(property, desiredValue);
		this.property = property;
		this.desiredValue = desiredValue;
		traceContext.TraceMethodExit("ConfigurationItem");
	}
}
