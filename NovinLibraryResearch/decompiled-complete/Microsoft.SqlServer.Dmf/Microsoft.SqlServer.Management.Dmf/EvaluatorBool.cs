using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorBool : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorBool");

	private bool left;

	private bool right;

	internal EvaluatorBool()
	{
		left = false;
		right = false;
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SetEvaluatorObjects");
		methodTraceContext.TraceParameters(left, right);
		if (left == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("left"));
		}
		if (right == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("right"));
		}
		this.left = Evaluator.ConvertToBool(left);
		this.right = Evaluator.ConvertToBool(right);
	}

	public override bool Evaluate(OperatorType opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Evaluate");
		methodTraceContext.TraceParameters(opType);
		bool flag = false;
		flag = opType switch
		{
			OperatorType.AND => left && right, 
			OperatorType.OR => left || right, 
			OperatorType.EQ => left == right, 
			OperatorType.NE => left != right, 
			_ => throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Bool.ToString())), 
		};
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}
}
