using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorDateTime : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorDateTime");

	private DateTime left;

	private DateTime right;

	internal EvaluatorDateTime()
	{
		left = DateTime.MinValue;
		right = DateTime.MinValue;
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		traceContext.TraceMethodEnter("SetEvaluatorObjects");
		traceContext.TraceParameters(left, right);
		this.left = Evaluator.ConvertToDateTime(left);
		this.right = Evaluator.ConvertToDateTime(right);
		traceContext.TraceMethodExit("SetEvaluatorObjects");
	}

	public override bool Evaluate(OperatorType opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Evaluate");
		methodTraceContext.TraceParameters(opType);
		bool flag = false;
		flag = opType switch
		{
			OperatorType.EQ => left == right, 
			OperatorType.NE => left != right, 
			OperatorType.GE => left >= right, 
			OperatorType.GT => left > right, 
			OperatorType.LE => left <= right, 
			OperatorType.LT => left < right, 
			_ => throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.DateTime.ToString())), 
		};
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}
}
