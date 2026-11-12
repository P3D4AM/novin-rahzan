using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class AttributeOperatorPair
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "AttributeOperatorPair");

	private string attribute;

	private OperatorType opType;

	internal string Attribute => attribute;

	internal OperatorType OpType => opType;

	public AttributeOperatorPair(string attribute, OperatorType opType)
	{
		traceContext.TraceMethodEnter("AttributeOperatorPair");
		traceContext.TraceParameters(attribute, opType);
		this.attribute = attribute;
		this.opType = opType;
		traceContext.TraceMethodExit("AttributeOperatorPair");
	}
}
