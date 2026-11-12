namespace Microsoft.SqlServer.Management.XEvent;

internal class TraceContext
{
	public static TraceContext GetTraceContext(string name, string component)
	{
		return new TraceContext();
	}

	public void Assert(bool assertion, string message, params object[] args)
	{
	}

	public void TraceCriticalError(params object[] args)
	{
	}

	public void TraceMethodEnter(params object[] args)
	{
	}

	public void TraceMethodExit(params object[] args)
	{
	}

	public void TraceParameterIn(params object[] args)
	{
	}

	public MethodContext GetMethodContext(params object[] args)
	{
		return new MethodContext();
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
}
