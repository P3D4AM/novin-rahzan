using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class WarningsConfiguration
{
	private Dictionary<int, (WarningBehavior? Behavior, LogLevel? Level)> _explicitBehaviors = new Dictionary<int, (WarningBehavior?, LogLevel?)>();

	private WarningBehavior _defaultBehavior;

	private long? _serviceProviderHash;

	public virtual WarningBehavior DefaultBehavior => _defaultBehavior;

	public WarningsConfiguration()
	{
	}

	protected WarningsConfiguration([JetBrains.Annotations.NotNull] WarningsConfiguration copyFrom)
	{
		_defaultBehavior = copyFrom._defaultBehavior;
		_explicitBehaviors = copyFrom._explicitBehaviors;
	}

	protected virtual WarningsConfiguration Clone()
	{
		return new WarningsConfiguration(this);
	}

	public virtual WarningsConfiguration WithDefaultBehavior(WarningBehavior warningBehavior)
	{
		WarningsConfiguration warningsConfiguration = Clone();
		warningsConfiguration._defaultBehavior = warningBehavior;
		return warningsConfiguration;
	}

	public virtual WarningsConfiguration WithExplicit([JetBrains.Annotations.NotNull] IEnumerable<EventId> eventIds, WarningBehavior warningBehavior)
	{
		WarningsConfiguration warningsConfiguration = Clone();
		warningsConfiguration._explicitBehaviors = new Dictionary<int, (WarningBehavior?, LogLevel?)>(_explicitBehaviors);
		foreach (EventId eventId in eventIds)
		{
			(WarningBehavior?, LogLevel?) value = ((!warningsConfiguration._explicitBehaviors.TryGetValue(eventId.Id, out value)) ? (warningBehavior, null) : (warningBehavior, value.Item2));
			warningsConfiguration._explicitBehaviors[eventId.Id] = value;
		}
		return warningsConfiguration;
	}

	public virtual WarningsConfiguration WithExplicit([JetBrains.Annotations.NotNull] IEnumerable<(EventId Id, LogLevel Level)> eventsAndLevels)
	{
		WarningsConfiguration warningsConfiguration = Clone();
		warningsConfiguration._explicitBehaviors = new Dictionary<int, (WarningBehavior?, LogLevel?)>(_explicitBehaviors);
		foreach (var (eventId, value) in eventsAndLevels)
		{
			warningsConfiguration._explicitBehaviors[eventId.Id] = (WarningBehavior.Log, value);
		}
		return warningsConfiguration;
	}

	public virtual WarningBehavior? GetBehavior(EventId eventId)
	{
		if (!_explicitBehaviors.TryGetValue(eventId.Id, out (WarningBehavior?, LogLevel?) value))
		{
			return null;
		}
		return value.Item1;
	}

	public virtual LogLevel? GetLevel(EventId eventId)
	{
		if (!_explicitBehaviors.TryGetValue(eventId.Id, out (WarningBehavior?, LogLevel?) value))
		{
			return null;
		}
		return value.Item2;
	}

	public virtual WarningsConfiguration TryWithExplicit(EventId eventId, WarningBehavior warningBehavior)
	{
		if (!_explicitBehaviors.ContainsKey(eventId.Id))
		{
			return WithExplicit(new EventId[1] { eventId }, warningBehavior);
		}
		return this;
	}

	public virtual long GetServiceProviderHashCode()
	{
		if (!_serviceProviderHash.HasValue)
		{
			long num = _defaultBehavior.GetHashCode();
			if (_explicitBehaviors != null)
			{
				num = _explicitBehaviors.Aggregate(num, (long t, KeyValuePair<int, (WarningBehavior? Behavior, LogLevel? Level)> e) => (t * 397) ^ (((long)e.Value.GetHashCode() * 3163L) ^ e.Key.GetHashCode()));
			}
			_serviceProviderHash = num;
		}
		return _serviceProviderHash.Value;
	}
}
