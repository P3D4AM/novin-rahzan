using System;

namespace Microsoft.SqlServer.Diagnostics.STrace;

internal class MethodContext : IDisposable
{
	public void Dispose()
	{
	}

	public ActivityContext GetActivityContext(string s)
	{
		return new ActivityContext();
	}

	public void TraceCatch(Exception ex)
	{
	}
}
