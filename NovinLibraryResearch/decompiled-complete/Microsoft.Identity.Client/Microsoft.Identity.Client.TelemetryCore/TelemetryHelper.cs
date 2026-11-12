using System;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.TelemetryCore;

internal sealed class TelemetryHelper : IDisposable
{
	private readonly EventBase _eventToEnd;

	private readonly IMatsTelemetryManager _telemetryManager;

	private readonly IHttpTelemetryManager _httpTelemetryManager;

	private bool _disposedValue;

	public TelemetryHelper(IMatsTelemetryManager telemetryManager, IHttpTelemetryManager httpTelemetryManager, EventBase eventBase)
	{
		_telemetryManager = telemetryManager;
		_httpTelemetryManager = httpTelemetryManager;
		_eventToEnd = eventBase;
		_telemetryManager?.StartEvent(eventBase);
	}

	private void Dispose(bool disposing)
	{
		if (_disposedValue)
		{
			return;
		}
		if (disposing)
		{
			_telemetryManager?.StopEvent(_eventToEnd);
			if (_eventToEnd is ApiEvent apiEvent)
			{
				_httpTelemetryManager?.RecordStoppedEvent(apiEvent);
			}
		}
		_disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
