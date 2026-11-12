using System;

namespace Microsoft.SqlServer.Diagnostics.STrace;

internal class TraceContext : IDisposable
{
	public static TraceContext GetTraceContext(string componentName, string s)
	{
		return new TraceContext();
	}

	public ActivityContext GetActivityContext(string s)
	{
		return new ActivityContext();
	}

	public MethodContext GetMethodContext(string s)
	{
		return new MethodContext();
	}

	public void TraceActivityStart(string s)
	{
	}

	public void TraceActivityEnd(string s)
	{
	}

	public void TraceCatch(Exception ex)
	{
	}

	public void TraceInformation(string s, params object[] info)
	{
	}

	public void Dispose()
	{
	}
}
