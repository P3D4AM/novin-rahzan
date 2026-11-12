using System;

namespace Microsoft.SqlServer.Management.XEvent;

internal class MethodTraceContext : IDisposable
{
	public void TraceParameterIn(params object[] args)
	{
	}

	public MethodContext GetActivityContext(params object[] args)
	{
		return new MethodContext();
	}

	public void TraceVerbose(params object[] args)
	{
	}

	public void TraceCatch(params object[] args)
	{
	}

	public void TraceError(params object[] args)
	{
	}

	public void Assert(params object[] args)
	{
	}

	public void TraceCriticalError(params object[] args)
	{
	}

	public void Dispose()
	{
	}
}
