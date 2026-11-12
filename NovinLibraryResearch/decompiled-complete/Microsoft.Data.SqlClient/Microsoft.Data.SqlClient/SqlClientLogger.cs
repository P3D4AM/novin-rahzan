using System;

namespace Microsoft.Data.SqlClient;

public class SqlClientLogger
{
	internal enum LogLevel
	{
		Info,
		Warning,
		Error
	}

	public bool IsLoggingEnabled => SqlClientEventSource.Log.IsEnabled();

	public void LogInfo(string type, string method, string message)
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc|{0}|{1}|{2}>{3}", type, method, LogLevel.Info, message);
	}

	public void LogWarning(string type, string method, string message)
	{
		Console.Out.WriteLine(message);
		SqlClientEventSource.Log.TryTraceEvent("<sc|{0}|{1}|{2}>{3}", type, method, LogLevel.Warning, message);
	}

	public void LogError(string type, string method, string message)
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc|{0}|{1}|{2}>{3}", type, method, LogLevel.Error, message);
	}

	public bool LogAssert(bool value, string type, string method, string message)
	{
		if (!value)
		{
			LogError(type, method, message);
		}
		return value;
	}
}
