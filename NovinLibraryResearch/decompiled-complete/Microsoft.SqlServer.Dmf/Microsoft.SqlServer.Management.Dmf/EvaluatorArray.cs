using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal abstract class EvaluatorArray : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorArray");

	private Array left;

	private Array right;

	private object inObject;

	private Evaluator evaluator;

	protected Array Left
	{
		get
		{
			return left;
		}
		set
		{
			traceContext.TraceVerbose("Setting Left to: {0}", value);
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			left = (Array)value.Clone();
			Array.Sort(left);
		}
	}

	protected Array Right
	{
		get
		{
			return right;
		}
		set
		{
			traceContext.TraceVerbose("Setting Right to: {0}", value);
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			right = (Array)value.Clone();
			Array.Sort(right);
		}
	}

	protected object InObject
	{
		get
		{
			return inObject;
		}
		set
		{
			traceContext.TraceVerbose("Setting InObject to: {0}", value);
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			inObject = value;
		}
	}

	protected Evaluator Evaluator
	{
		get
		{
			return evaluator;
		}
		set
		{
			traceContext.TraceVerbose("Setting Evaluator to: {0}", value);
			evaluator = value;
		}
	}

	protected EvaluatorArray()
	{
		left = null;
		right = null;
		inObject = null;
	}

	public static Evaluator ConstructEvaluator(object arg)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ConstructEvaluator");
		methodTraceContext.TraceParameters(arg);
		if (arg == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("arg"));
		}
		object obj = null;
		Array array = (Array)arg;
		for (int i = 0; i < array.Length; i++)
		{
			obj = array.GetValue(i);
			if (obj != null)
			{
				switch (EvaluationFactory.ClassifyType(obj))
				{
				case TypeClass.Numeric:
					return new EvaluatorArrayNumeric();
				case TypeClass.String:
					return new EvaluatorArrayString();
				default:
					methodTraceContext.TraceParameterOut("returnVal", null);
					return null;
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", null);
		return null;
	}

	protected bool ArraysEqual()
	{
		if (left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			evaluator.SetEvaluatorObjects(left.GetValue(i), right.GetValue(i));
			if (!evaluator.Evaluate(OperatorType.EQ))
			{
				return false;
			}
		}
		return true;
	}

	public override bool Evaluate(OperatorType opType)
	{
		switch (opType)
		{
		case OperatorType.EQ:
		case OperatorType.NE:
			if (Right == null)
			{
				throw traceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Array.ToString()));
			}
			if (ArraysEqual())
			{
				return opType == OperatorType.EQ;
			}
			return opType == OperatorType.NE;
		case OperatorType.IN:
		case OperatorType.NOT_IN:
		{
			if (InObject == null)
			{
				throw traceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Array.ToString()));
			}
			bool flag = false;
			for (int i = 0; i < Right.Length; i++)
			{
				evaluator.SetEvaluatorObjects(InObject, Right.GetValue(i));
				if (evaluator.Evaluate(OperatorType.EQ))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return opType == OperatorType.IN;
			}
			return opType == OperatorType.NOT_IN;
		}
		default:
			throw traceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.Array.ToString()));
		}
	}
}
