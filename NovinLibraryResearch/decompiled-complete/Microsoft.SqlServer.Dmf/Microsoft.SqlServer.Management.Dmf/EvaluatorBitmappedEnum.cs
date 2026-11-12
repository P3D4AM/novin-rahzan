using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorBitmappedEnum : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorEnum");

	private object left;

	private object right;

	internal EvaluatorBitmappedEnum()
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
		methodTraceContext.Assert(right.GetType().IsBitmappedEnum(), "Incorrect operand types used for bitmapped enum evaluation: leftType = " + left.GetType()?.ToString() + " rightType = " + right.GetType());
		int num = (int)left;
		int num2 = (int)right;
		bool flag = (num == 0 && num2 == 0) || (num & num2) == num2;
		switch (opType)
		{
		case OperatorType.BEQ:
			return flag;
		case OperatorType.BNE:
			return !flag;
		default:
			methodTraceContext.Assert(condition: false, "Incorrect operator used for enum evaluation: " + opType);
			return false;
		}
	}
}
