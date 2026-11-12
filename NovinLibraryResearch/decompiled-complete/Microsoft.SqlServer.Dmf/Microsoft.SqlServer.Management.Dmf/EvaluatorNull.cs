using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorNull : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorNull");

	private object left;

	private object right;

	internal EvaluatorNull()
	{
		left = null;
		right = null;
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		traceContext.TraceMethodEnter("SetEvaluatorObjects");
		traceContext.TraceParameters(left, right);
		this.left = left;
		this.right = right;
		traceContext.TraceMethodExit("SetEvaluatorObjects");
	}

	public override bool Evaluate(OperatorType opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Evaluate");
		methodTraceContext.TraceParameters(opType);
		if (opType != OperatorType.EQ && opType != OperatorType.NE)
		{
			methodTraceContext.TraceParameterOut("returnVal", false);
			return false;
		}
		if (left == null && right == null)
		{
			return opType == OperatorType.EQ;
		}
		return opType == OperatorType.NE;
	}
}
