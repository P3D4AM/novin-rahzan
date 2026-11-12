using Serilog.Configuration;
using Serilog.Enrichers.GlobalLogContext;

namespace Serilog;

public static class LoggerEnrichmentConfigurationExtensions
{
	public static LoggerConfiguration FromGlobalLogContext(this LoggerEnrichmentConfiguration enrich)
	{
		return enrich.With<GlobalLogContextEnricher>();
	}
}
