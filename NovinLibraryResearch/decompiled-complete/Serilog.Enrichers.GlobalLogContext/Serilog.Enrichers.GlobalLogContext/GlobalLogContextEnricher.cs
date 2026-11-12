using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.GlobalLogContext;

internal sealed class GlobalLogContextEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		Serilog.Context.GlobalLogContext.Enrich(logEvent, propertyFactory);
	}
}
