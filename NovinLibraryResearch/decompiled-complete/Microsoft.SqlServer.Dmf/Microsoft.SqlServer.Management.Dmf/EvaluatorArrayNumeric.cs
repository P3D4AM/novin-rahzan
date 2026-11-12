using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorArrayNumeric : EvaluatorArray
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorArrayNumeric");

	internal EvaluatorArrayNumeric()
	{
		base.Evaluator = new EvaluatorNumeric();
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		traceContext.TraceMethodEnter("SetEvaluatorObjects");
		traceContext.TraceParameters(left, right);
		if (left is Array)
		{
			base.Left = Evaluator.ConvertToArray(left);
			base.Right = Evaluator.ConvertToArray(right);
		}
		else
		{
			base.InObject = Evaluator.ConvertToDouble(left);
			base.Right = Evaluator.ConvertToArray(right);
		}
		traceContext.TraceMethodExit("SetEvaluatorObjects");
	}
}
