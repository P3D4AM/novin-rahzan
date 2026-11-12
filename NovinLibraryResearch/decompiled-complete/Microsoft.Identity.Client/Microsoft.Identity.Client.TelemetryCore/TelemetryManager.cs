using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.TelemetryCore;

internal class TelemetryManager : IMatsTelemetryManager
{
	internal class EventKey : IEquatable<EventKey>
	{
		public string CorrelationId { get; }

		public string EventId { get; }

		public string EventName { get; }

		public EventKey(EventBase eventBase)
		{
			CorrelationId = eventBase.CorrelationId;
			EventId = eventBase.EventId;
			EventName = eventBase["msal.event_name"];
		}

		public bool Equals(EventKey other)
		{
			if ((object)other == null)
			{
				return false;
			}
			if ((object)this == other)
			{
				return true;
			}
			if (string.Equals(CorrelationId, other.CorrelationId, StringComparison.OrdinalIgnoreCase) && string.Equals(EventId, other.EventId, StringComparison.OrdinalIgnoreCase))
			{
				return string.Equals(EventName, other.EventName, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((0x50C5D1F ^ ((EventId != null) ? EventId.GetHashCode() : 0)) * 16777619) ^ ((CorrelationId != null) ? CorrelationId.GetHashCode() : 0)) * 16777619) ^ ((EventName != null) ? EventName.GetHashCode() : 0);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((EventKey)obj);
		}

		public static bool operator ==(EventKey left, EventKey right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(EventKey left, EventKey right)
		{
			return !object.Equals(left, right);
		}
	}

	private const string MsalCacheEventValuePrefix = "msal.token";

	private const string MsalCacheEventName = "msal.cache_event";

	internal readonly ConcurrentDictionary<string, List<EventBase>> _completedEvents = new ConcurrentDictionary<string, List<EventBase>>();

	internal readonly ConcurrentDictionary<EventKey, EventBase> _eventsInProgress = new ConcurrentDictionary<EventKey, EventBase>();

	internal readonly ConcurrentDictionary<string, ConcurrentDictionary<string, int>> _eventCount = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

	private EventBase _mostRecentStoppedApiEvent;

	private readonly object _mostRecentStoppedApiEventLockObj = new object();

	private readonly bool _onlySendFailureTelemetry;

	private readonly IPlatformProxy _platformProxy;

	private readonly IApplicationConfiguration _applicationConfiguration;

	private bool IsMatsConfigured
	{
		get
		{
			if (Callback == null)
			{
				return _applicationConfiguration.TelemetryConfig != null;
			}
			return true;
		}
	}

	public TelemetryCallback Callback { get; }

	public TelemetryManager(IApplicationConfiguration applicationConfiguration, IPlatformProxy platformProxy, TelemetryCallback telemetryCallback, bool onlySendFailureTelemetry = false)
	{
		_applicationConfiguration = applicationConfiguration;
		_platformProxy = platformProxy;
		Callback = telemetryCallback;
		_onlySendFailureTelemetry = onlySendFailureTelemetry;
	}

	public void StartEvent(EventBase eventToStart)
	{
		if (IsMatsConfigured)
		{
			_eventsInProgress[new EventKey(eventToStart)] = eventToStart;
		}
	}

	public void StopEvent(EventBase eventToStop)
	{
		if (!IsMatsConfigured)
		{
			return;
		}
		EventKey key = new EventKey(eventToStop);
		_eventsInProgress.TryGetValue(key, out var value);
		if (value == null)
		{
			return;
		}
		eventToStop.Stop();
		IncrementEventCount(eventToStop);
		if (_completedEvents.TryGetValue(eventToStop.CorrelationId, out var value2))
		{
			value2.Add(eventToStop);
		}
		else
		{
			_completedEvents.TryAdd(eventToStop.CorrelationId, new List<EventBase> { eventToStop });
		}
		_eventsInProgress.TryRemove(key, out var _);
		if (!eventToStop.TryGetValue("msal.api_id", out var _))
		{
			return;
		}
		lock (_mostRecentStoppedApiEventLockObj)
		{
			_mostRecentStoppedApiEvent = eventToStop;
		}
	}

	public void Flush(string correlationId)
	{
		if (IsMatsConfigured && _completedEvents.ContainsKey(correlationId))
		{
			_completedEvents[correlationId].AddRange(CollateOrphanedEvents(correlationId));
			_completedEvents.TryRemove(correlationId, out var value);
			_eventCount.TryRemove(correlationId, out var value2);
			if (_onlySendFailureTelemetry && value.Any((EventBase ev) => ev is ApiEvent apiEvent && apiEvent.WasSuccessful))
			{
				value.Clear();
			}
			if (value.Any())
			{
				value.Insert(0, new DefaultEvent(_platformProxy, correlationId, _applicationConfiguration.ClientId, value2 ?? new ConcurrentDictionary<string, int>()));
				Callback?.Invoke(value.Cast<Dictionary<string, string>>().ToList());
			}
		}
	}

	private IEnumerable<EventBase> CollateOrphanedEvents(string correlationId)
	{
		List<EventBase> list = new List<EventBase>();
		foreach (EventKey key in _eventsInProgress.Keys)
		{
			if (string.Compare(key.CorrelationId, correlationId, StringComparison.OrdinalIgnoreCase) == 0 && _eventsInProgress.TryRemove(key, out var value))
			{
				IncrementEventCount(value);
				list.Add(value);
			}
		}
		return list;
	}

	private void IncrementEventCount(EventBase eventToIncrement)
	{
		string key2 = ((!(eventToIncrement["msal.event_name"].Substring(0, 10) == "msal.token")) ? eventToIncrement["msal.event_name"] : "msal.cache_event");
		if (!_eventCount.ContainsKey(eventToIncrement.CorrelationId))
		{
			_eventCount[eventToIncrement.CorrelationId] = new ConcurrentDictionary<string, int>();
			_eventCount[eventToIncrement.CorrelationId].TryAdd(key2, 1);
		}
		else
		{
			_eventCount[eventToIncrement.CorrelationId].AddOrUpdate(key2, 1, (string key, int count) => count + 1);
		}
	}
}
