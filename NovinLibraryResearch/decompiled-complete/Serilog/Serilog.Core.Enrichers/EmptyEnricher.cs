using Serilog.Events;

namespace Serilog.Core.Enrichers;

internal class EmptyEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
	}
}
