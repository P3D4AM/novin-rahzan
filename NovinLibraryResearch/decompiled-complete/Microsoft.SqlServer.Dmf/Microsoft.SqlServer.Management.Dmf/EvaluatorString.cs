using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

internal class EvaluatorString : Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "EvaluatorString");

	private string left;

	private string right;

	private IComparer stringComparer;

	private CompareOptions compareOptions;

	public string Left
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

	public string Right
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

	internal EvaluatorString()
	{
		left = null;
		right = null;
		stringComparer = null;
	}

	internal EvaluatorString(FacetEvaluationContext context)
	{
		traceContext.TraceMethodEnter("EvaluatorString");
		traceContext.TraceParameters(context);
		left = null;
		right = null;
		stringComparer = null;
		if (context != null && context.PhysicalTarget is SqlSmoObject sqlSmoObject)
		{
			stringComparer = sqlSmoObject.StringComparer;
			if (sqlSmoObject.StringComparer != null)
			{
				compareOptions = sqlSmoObject.StringComparer.CompareOptions;
			}
		}
		traceContext.TraceMethodExit("EvaluatorString");
	}

	public override void SetEvaluatorObjects(object left, object right)
	{
		traceContext.TraceMethodEnter("SetEvaluatorObjects");
		traceContext.TraceParameters(left, right);
		Left = Evaluator.ConvertToString(left);
		Right = Evaluator.ConvertToString(right);
		traceContext.TraceMethodExit("SetEvaluatorObjects");
	}

	public override bool Evaluate(OperatorType opType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Evaluate");
		methodTraceContext.TraceParameters(opType);
		bool flag = false;
		flag = opType switch
		{
			OperatorType.EQ => (stringComparer != null) ? (stringComparer.Compare(left, right) == 0) : (left == right), 
			OperatorType.NE => (stringComparer != null) ? (stringComparer.Compare(left, right) != 0) : (left != right), 
			OperatorType.LIKE => EvaluateLike(left, right), 
			OperatorType.NOT_LIKE => !EvaluateLike(left, right), 
			_ => throw methodTraceContext.TraceThrow(new OperatorNotApplicableException(opType.ToString(), TypeClass.String.ToString())), 
		};
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}

	private bool EvaluateLike(string str, string likePattern)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EvaluateLike");
		methodTraceContext.TraceParameters(str, likePattern);
		bool flag = false;
		try
		{
			string pattern = EvaluationFactory.LikeToRegex(likePattern);
			RegexOptions regexOptions = RegexOptions.None;
			if (stringComparer != null && (compareOptions & CompareOptions.IgnoreCase) != 0)
			{
				regexOptions = RegexOptions.IgnoreCase;
			}
			regexOptions |= RegexOptions.CultureInvariant;
			flag = new Regex(pattern, regexOptions).IsMatch(str);
		}
		catch (ArgumentException)
		{
		}
		methodTraceContext.TraceParameterOut("returnVal", flag);
		return flag;
	}
}
