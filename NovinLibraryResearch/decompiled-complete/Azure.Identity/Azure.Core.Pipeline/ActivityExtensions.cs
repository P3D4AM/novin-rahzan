using System;
using System.Diagnostics;
using System.Reflection;

namespace Azure.Core.Pipeline;

internal static class ActivityExtensions
{
	private static readonly MethodInfo? s_setIdFormatMethod = typeof(Activity).GetMethod("SetIdFormat");

	private static readonly MethodInfo? s_getIdFormatMethod = typeof(Activity).GetProperty("IdFormat")?.GetMethod;

	private static readonly MethodInfo? s_getTraceStateStringMethod = typeof(Activity).GetProperty("TraceStateString")?.GetMethod;

	public static bool SetW3CFormat(this Activity activity)
	{
		if (s_setIdFormatMethod == null)
		{
			return false;
		}
		s_setIdFormatMethod.Invoke(activity, new object[1] { 2 });
		return true;
	}

	public static bool IsW3CFormat(this Activity activity)
	{
		if (s_getIdFormatMethod == null)
		{
			return false;
		}
		object obj = s_getIdFormatMethod.Invoke(activity, Array.Empty<object>());
		if (obj != null)
		{
			return (int)obj == 2;
		}
		return false;
	}

	public static bool TryGetTraceState(this Activity activity, out string? traceState)
	{
		traceState = null;
		if (s_getTraceStateStringMethod == null)
		{
			return false;
		}
		traceState = s_getTraceStateStringMethod.Invoke(activity, Array.Empty<object>()) as string;
		return true;
	}
}
