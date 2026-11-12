using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace Serilog.Extensions.Logging;

public class LoggerProviderCollection : IDisposable
{
	private volatile ILoggerProvider[] _providers = Array.Empty<ILoggerProvider>();

	public IEnumerable<ILoggerProvider> Providers => _providers;

	public void AddProvider(ILoggerProvider provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		ILoggerProvider[] providers;
		ILoggerProvider[] value;
		do
		{
			providers = _providers;
			value = providers.Concat(new ILoggerProvider[1] { provider }).ToArray();
		}
		while (Interlocked.CompareExchange(ref _providers, value, providers) != providers);
	}

	public void Dispose()
	{
		ILoggerProvider[] providers = _providers;
		for (int i = 0; i < providers.Length; i++)
		{
			providers[i].Dispose();
		}
	}
}
