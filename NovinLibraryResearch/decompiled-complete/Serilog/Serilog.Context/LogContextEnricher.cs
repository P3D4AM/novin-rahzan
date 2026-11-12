using Serilog.Core;
using Serilog.Events;

namespace Serilog.Context;

internal sealed class LogContextEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		LogContext.Enrich(logEvent, propertyFactory);
	}
}
