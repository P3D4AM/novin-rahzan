using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class TargetEvaluation
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "TargetEvaluation");

	private object target;

	private ExpressionNode result;

	public object Target => target;

	public ExpressionNode Result => result;

	internal TargetEvaluation(object target, ExpressionNode result)
	{
		traceContext.TraceMethodEnter("TargetEvaluation");
		traceContext.TraceParameters(target, result);
		this.target = target;
		this.result = result;
		traceContext.TraceMethodExit("TargetEvaluation");
	}
}
