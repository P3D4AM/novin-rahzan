#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Identity.Client.Extensions.Msal;

public class TraceSourceLogger
{
	public TraceSource Source { get; }

	public TraceSourceLogger(TraceSource traceSource)
	{
		Source = traceSource;
	}

	public void LogInformation(string message)
	{
		Source.TraceEvent(TraceEventType.Information, 0, FormatLogMessage(message));
	}

	public void LogError(string message)
	{
		Source.TraceEvent(TraceEventType.Error, 0, FormatLogMessage(message));
	}

	public void LogWarning(string message)
	{
		Source.TraceEvent(TraceEventType.Warning, 0, FormatLogMessage(message));
	}

	private static string FormatLogMessage(string message)
	{
		return "[MSAL.Extension][" + DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) + "] " + message;
	}
}
