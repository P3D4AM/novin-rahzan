using System;
using System.Security;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data;

internal static class ExceptionBuilder
{
	private static void TraceException(string trace, Exception e)
	{
		if (e != null)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent(trace, e.Message);
			try
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<comm.ADP.TraceException|ERR|ADV> Environment StackTrace = '{0}'", Environment.StackTrace);
			}
			catch (SecurityException)
			{
			}
		}
	}

	internal static void TraceExceptionAsReturnValue(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|THROW> Message='{0}'", e);
	}

	internal static ArgumentException _Argument(string error)
	{
		ArgumentException ex = new ArgumentException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static Exception InvalidOffsetLength()
	{
		return _Argument(StringsHelper.GetString(Strings.Data_InvalidOffsetLength));
	}
}
