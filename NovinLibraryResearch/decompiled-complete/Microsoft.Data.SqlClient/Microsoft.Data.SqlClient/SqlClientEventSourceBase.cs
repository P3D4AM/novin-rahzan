using System.Diagnostics.Tracing;

namespace Microsoft.Data.SqlClient;

internal abstract class SqlClientEventSourceBase : EventSource
{
	protected override void OnEventCommand(EventCommandEventArgs command)
	{
		base.OnEventCommand(command);
		EventCommandMethodCall(command);
	}

	protected virtual void EventCommandMethodCall(EventCommandEventArgs command)
	{
	}

	internal virtual void HardConnectRequest()
	{
	}

	internal virtual void HardDisconnectRequest()
	{
	}

	internal virtual void SoftConnectRequest()
	{
	}

	internal virtual void SoftDisconnectRequest()
	{
	}

	internal virtual void EnterNonPooledConnection()
	{
	}

	internal virtual void ExitNonPooledConnection()
	{
	}

	internal virtual void EnterPooledConnection()
	{
	}

	internal virtual void ExitPooledConnection()
	{
	}

	internal virtual void EnterActiveConnectionPoolGroup()
	{
	}

	internal virtual void ExitActiveConnectionPoolGroup()
	{
	}

	internal virtual void EnterInactiveConnectionPoolGroup()
	{
	}

	internal virtual void ExitInactiveConnectionPoolGroup()
	{
	}

	internal virtual void EnterActiveConnectionPool()
	{
	}

	internal virtual void ExitActiveConnectionPool()
	{
	}

	internal virtual void EnterInactiveConnectionPool()
	{
	}

	internal virtual void ExitInactiveConnectionPool()
	{
	}

	internal virtual void EnterActiveConnection()
	{
	}

	internal virtual void ExitActiveConnection()
	{
	}

	internal virtual void EnterFreeConnection()
	{
	}

	internal virtual void ExitFreeConnection()
	{
	}

	internal virtual void EnterStasisConnection()
	{
	}

	internal virtual void ExitStasisConnection()
	{
	}

	internal virtual void ReclaimedConnectionRequest()
	{
	}
}
