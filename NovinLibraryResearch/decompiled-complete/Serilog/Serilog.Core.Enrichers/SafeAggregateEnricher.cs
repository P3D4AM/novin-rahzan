using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Debugging;
using Serilog.Events;

namespace Serilog.Core.Enrichers;

internal class SafeAggregateEnricher : ILogEventEnricher
{
	private readonly ILogEventEnricher[] _enrichers;

	public SafeAggregateEnricher(IEnumerable<ILogEventEnricher> enrichers)
	{
		Guard.AgainstNull(enrichers, "enrichers");
		_enrichers = enrichers.ToArray();
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		ILogEventEnricher[] enrichers = _enrichers;
		foreach (ILogEventEnricher logEventEnricher in enrichers)
		{
			try
			{
				logEventEnricher.Enrich(logEvent, propertyFactory);
			}
			catch (Exception arg)
			{
				SelfLog.WriteLine("Exception {0} caught while enriching {1} with {2}.", arg, logEvent, logEventEnricher);
			}
		}
	}
}
