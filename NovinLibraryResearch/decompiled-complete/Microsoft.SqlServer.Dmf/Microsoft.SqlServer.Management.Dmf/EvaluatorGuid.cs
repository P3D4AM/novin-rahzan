using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorGuid : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorGuid");

	private Guid left;

	private Guid right;

	internal EvaluatorGuid()
	{
		left = Guid.Empty;
		right = Guid.Empty;
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		traceContext.TraceMethodEnter("SetEvaluatorObjects");
		traceContext.TraceParameters(left, right);
		this.left = (Guid)left;
		this.right = (Guid)right;
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
			_ => throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Guid.ToString())), 
		};
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}
}
