using Microsoft.SqlServer.Diagnostics.STrace;

namespace Microsoft.SqlServer.Management.SqlParser.Diagnostics;

internal static class TraceHelper
{
	private const string ComponentName = "Transact-SQL Language Service";

	private static TraceContext traceContext;

	public static TraceContext TraceContext => traceContext;

	static TraceHelper()
	{
		traceContext = TraceContext.GetTraceContext("Transact-SQL Language Service", "SqlParser");
	}
}
