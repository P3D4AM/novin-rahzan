using System;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace Serilog;

public static class SerilogLoggerFactoryExtensions
{
	public static ILoggerFactory AddSerilog(this ILoggerFactory factory, ILogger? logger = null, bool dispose = false)
	{
		if (factory == null)
		{
			throw new ArgumentNullException("factory");
		}
		factory.AddProvider(new SerilogLoggerProvider(logger, dispose));
		return factory;
	}
}
