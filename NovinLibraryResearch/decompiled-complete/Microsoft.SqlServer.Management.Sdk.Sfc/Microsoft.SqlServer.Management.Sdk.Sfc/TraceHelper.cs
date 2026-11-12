#define TRACE
using System;
using System.Diagnostics;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public static class TraceHelper
{
	private const uint TraceLevelAssert = 2147483648u;

	private const uint TraceLevelException = 2097152u;

	private static string AddDate(string str)
	{
		return string.Format("{0} - {1}", DateTime.Now.ToString("o"), str);
	}

	public static void Trace(string strComponentName, string strFormat, params object[] args)
	{
		System.Diagnostics.Trace.TraceInformation(AddDate(strFormat), args);
	}

	public static void Trace(string strComponentName, uint traceLevel, string strFormat, params object[] args)
	{
		if (1073741824 == (traceLevel & 0x40000000) || 2097152 == (traceLevel & 0x200000))
		{
			System.Diagnostics.Trace.TraceError(AddDate(strFormat), args);
		}
		else if (int.MinValue != (int)(traceLevel & 0x80000000u))
		{
			if (536870912 == (traceLevel & 0x20000000))
			{
				System.Diagnostics.Trace.TraceWarning(AddDate(strFormat), args);
			}
			else
			{
				System.Diagnostics.Trace.TraceInformation(AddDate(strFormat), args);
			}
		}
	}

	public static void Assert(bool condition)
	{
	}

	public static void Assert(bool condition, string strFormat)
	{
	}

	public static void LogExCatch(Exception ex)
	{
		System.Diagnostics.Trace.TraceError(ex.Message);
	}
}
