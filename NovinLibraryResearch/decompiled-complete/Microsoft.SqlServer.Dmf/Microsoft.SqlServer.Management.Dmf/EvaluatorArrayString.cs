using System;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorArrayString : EvaluatorArray
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorArrayString");

	internal EvaluatorArrayString()
	{
		base.Evaluator = new EvaluatorString();
	}

	internal EvaluatorArrayString(FacetEvaluationContext context)
	{
		traceContext.TraceMethodEnter("EvaluatorArrayString");
		traceContext.TraceParameters(context);
		base.Evaluator = new EvaluatorString(context);
		traceContext.TraceMethodExit("EvaluatorArrayString");
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
			base.InObject = Evaluator.ConvertToString(left);
			base.Right = Evaluator.ConvertToArray(right);
		}
		traceContext.TraceMethodExit("SetEvaluatorObjects");
	}
}
