namespace Microsoft.SqlServer.Management.XEvent;

internal static class TraceHelper
{
	private const string ComponentName = "SMO XEvent Management";

	private static TraceContext traceContext;

	public static TraceContext TraceContext => traceContext;

	static TraceHelper()
	{
		traceContext = TraceContext.GetTraceContext("SMO XEvent Management", "Microsoft.SqlServer.Management.XEvent");
	}
}
