using System;
using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.Dmf;

internal abstract class Evaluator
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "Evaluator");

	public abstract bool Evaluate(OperatorType opType);

	public abstract void SetEvaluatorObjects(object left, object right);

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static double ConvertToDouble(object number)
	{
		return Convert.ToDouble(number);
	}

	internal static long ConvertToLong(object number)
	{
		return Convert.ToInt64(number);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static string ConvertToString(object str)
	{
		return Convert.ToString(str);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static bool ConvertToBool(object b)
	{
		return Convert.ToBoolean(b);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static DateTime ConvertToDateTime(object dt)
	{
		return Convert.ToDateTime(dt);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static Guid ConvertToGuid(object guid)
	{
		return (Guid)guid;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static Array ConvertToArray(object array)
	{
		return (Array)array;
	}

	internal static object ConvertToAny(object input, TypeClass typeClass)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ConvertToAny");
		methodTraceContext.TraceParameters(input, typeClass);
		switch (typeClass)
		{
		case TypeClass.Numeric:
			return ConvertToDouble(input);
		case TypeClass.String:
			return ConvertToString(input);
		case TypeClass.Bool:
			return ConvertToBool(input);
		case TypeClass.DateTime:
			return ConvertToDateTime(input);
		case TypeClass.Guid:
			return ConvertToGuid(input);
		case TypeClass.Array:
			return ConvertToArray(input);
		default:
			traceContext.DebugAssert(test: false, "Unsupported TypeClass");
			methodTraceContext.TraceParameterOut("returnVal", null);
			return null;
		}
	}
}
