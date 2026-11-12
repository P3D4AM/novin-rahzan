using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorNumeric : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorNumeric");

	private double left;

	private double right;

	private bool hasEnum;

	public double Left
	{
		get
		{
			return left;
		}
		set
		{
			traceContext.TraceVerbose("Setting Left to: {0}", value);
			left = value;
		}
	}

	public double Right
	{
		get
		{
			return right;
		}
		set
		{
			traceContext.TraceVerbose("Setting Right to: {0}", value);
			right = value;
		}
	}

	internal EvaluatorNumeric()
	{
		left = 0.0;
		right = 0.0;
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
		this.left = Evaluator.ConvertToDouble(left);
		this.right = Evaluator.ConvertToDouble(right);
		hasEnum = left.GetType().IsEnum || right.GetType().IsEnum;
	}

	public override bool Evaluate(OperatorType opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Evaluate");
		methodTraceContext.TraceParameters(opType);
		bool flag = false;
		if (hasEnum && opType != OperatorType.EQ && opType != OperatorType.NE)
		{
			throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), typeof(Enum).ToString()));
		}
		flag = opType switch
		{
			OperatorType.EQ => left == right, 
			OperatorType.NE => left != right, 
			OperatorType.GE => left >= right, 
			OperatorType.GT => left > right, 
			OperatorType.LE => left <= right, 
			OperatorType.LT => left < right, 
			_ => throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Numeric.ToString())), 
		};
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}
}
