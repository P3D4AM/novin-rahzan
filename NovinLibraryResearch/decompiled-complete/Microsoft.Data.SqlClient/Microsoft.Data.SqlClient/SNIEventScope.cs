using System;

namespace Microsoft.Data.SqlClient;

internal readonly struct SNIEventScope : IDisposable
{
	private readonly long _scopeId;

	public SNIEventScope(long scopeID)
	{
		_scopeId = scopeID;
	}

	public void Dispose()
	{
		SqlClientEventSource.Log.SNIScopeLeave($"Exit SNI Scope {_scopeId}");
	}

	public static SNIEventScope Create(string message)
	{
		return new SNIEventScope(SqlClientEventSource.Log.SNIScopeEnter(message));
	}
}
