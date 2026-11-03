using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PacketParser;

public class NpLoggerProvider : ILoggerProvider, IDisposable
{
	private LogLevel _logLevel;

	private readonly IDisposable _onChangeToken;

	private NpLoggerConfiguration _currentConfig;

	private readonly ConcurrentDictionary<string, NpLogger> _loggers = new ConcurrentDictionary<string, NpLogger>();

	public NpLoggerProvider(LogLevel logLevel)
	{
		_logLevel = logLevel;
	}

	public NpLoggerProvider(IOptionsMonitor<NpLoggerConfiguration> config)
	{
		_currentConfig = config.CurrentValue;
		_onChangeToken = config.OnChange(delegate(NpLoggerConfiguration updatedConfig)
		{
			_currentConfig = updatedConfig;
		});
	}

	public NpLoggerProvider(NpLoggerConfiguration configuration)
	{
		_logLevel = configuration.MinLogLevel;
	}

	public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
	{
		return _loggers.GetOrAdd(categoryName, (string name) => new NpLogger(name, _logLevel));
	}

	public void Dispose()
	{
		_loggers.Clear();
		_onChangeToken.Dispose();
	}
}
