using System;
using System.Diagnostics.Tracing;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

[EventSource(Name = "Microsoft-AspNetCore-Http-Connections")]
internal class HttpConnectionsEventSource : EventSource
{
	public static readonly HttpConnectionsEventSource Log = new HttpConnectionsEventSource();

	private readonly EventCounter _connectionsStarted;

	private readonly EventCounter _connectionsStopped;

	private readonly EventCounter _connectionsTimedOut;

	private readonly EventCounter _connectionDuration;

	private HttpConnectionsEventSource()
	{
		_connectionsStarted = new EventCounter("ConnectionsStarted", this);
		_connectionsStopped = new EventCounter("ConnectionsStopped", this);
		_connectionsTimedOut = new EventCounter("ConnectionsTimedOut", this);
		_connectionDuration = new EventCounter("ConnectionDuration", this);
	}

	[NonEvent]
	public void ConnectionStop(string connectionId, Microsoft.Extensions.Internal.ValueStopwatch timer)
	{
		if (IsEnabled())
		{
			double num = (timer.IsActive ? timer.GetElapsedTime().TotalMilliseconds : 0.0);
			_connectionDuration.WriteMetric((float)num);
			_connectionsStopped.WriteMetric(1f);
			if (IsEnabled(EventLevel.Informational, EventKeywords.None))
			{
				ConnectionStop(connectionId);
			}
		}
	}

	[NonEvent]
	public void ScannedConnections(TimeSpan duration)
	{
		if (IsEnabled() && IsEnabled(EventLevel.Verbose, EventKeywords.None))
		{
			ScannedConnections(duration.TotalMilliseconds);
		}
	}

	[Event(1, Level = EventLevel.Informational, Message = "Started connection '{0}'.")]
	public Microsoft.Extensions.Internal.ValueStopwatch ConnectionStart(string connectionId)
	{
		if (IsEnabled())
		{
			_connectionsStarted.WriteMetric(1f);
			if (IsEnabled(EventLevel.Informational, EventKeywords.None))
			{
				WriteEvent(1, connectionId);
				return Microsoft.Extensions.Internal.ValueStopwatch.StartNew();
			}
		}
		return default(Microsoft.Extensions.Internal.ValueStopwatch);
	}

	[Event(2, Level = EventLevel.Informational, Message = "Stopped connection '{0}'.")]
	private void ConnectionStop(string connectionId)
	{
		WriteEvent(2, connectionId);
	}

	[Event(3, Level = EventLevel.Informational, Message = "Connection '{0}' timed out.")]
	public void ConnectionTimedOut(string connectionId)
	{
		if (IsEnabled())
		{
			_connectionsTimedOut.WriteMetric(1f);
			if (IsEnabled(EventLevel.Informational, EventKeywords.None))
			{
				WriteEvent(3, connectionId);
			}
		}
	}

	[Event(4, Level = EventLevel.Verbose, Message = "Scanning connections.")]
	public void ScanningConnections()
	{
		if (IsEnabled() && IsEnabled(EventLevel.Verbose, EventKeywords.None))
		{
			WriteEvent(4);
		}
	}

	[Event(5, Level = EventLevel.Verbose, Message = "Finished scanning connections. Duration: {0:0.00}ms.")]
	private void ScannedConnections(double durationInMilliseconds)
	{
		WriteEvent(5, durationInMilliseconds);
	}
}
