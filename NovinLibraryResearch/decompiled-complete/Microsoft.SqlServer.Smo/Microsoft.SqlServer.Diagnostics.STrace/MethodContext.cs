#define TRACE
using System;
using System.Diagnostics;

namespace Microsoft.SqlServer.Diagnostics.STrace;

internal class MethodContext : IDisposable
{
	public void Dispose()
	{
	}

	public ActivityContext GetActivityContext(string s)
	{
		return new ActivityContext(s);
	}

	public void TraceCatch(Exception ex)
	{
		Trace.TraceError("TraceCatch - {0}", ex);
	}
}
