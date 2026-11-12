using System;
using Microsoft.Extensions.Logging;
using Serilog.Debugging;

namespace Serilog.Extensions.Logging;

public class SerilogLoggerFactory : ILoggerFactory, IDisposable
{
	private readonly LoggerProviderCollection? _providerCollection;

	private readonly SerilogLoggerProvider _provider;

	public SerilogLoggerFactory(ILogger? logger = null, bool dispose = false, LoggerProviderCollection? providerCollection = null)
	{
		_provider = new SerilogLoggerProvider(logger, dispose);
		_providerCollection = providerCollection;
	}

	public void Dispose()
	{
		_provider.Dispose();
	}

	public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
	{
		return _provider.CreateLogger(categoryName);
	}

	public void AddProvider(ILoggerProvider provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (_providerCollection != null)
		{
			_providerCollection.AddProvider(provider);
		}
		else
		{
			SelfLog.WriteLine("Ignoring added logger provider {0}", provider);
		}
	}
}
