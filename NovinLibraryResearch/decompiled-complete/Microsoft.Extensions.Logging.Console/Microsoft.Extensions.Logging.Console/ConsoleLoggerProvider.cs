using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Console;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("Console")]
public class ConsoleLoggerProvider : ILoggerProvider, IDisposable, ISupportExternalScope
{
	private readonly IOptionsMonitor<ConsoleLoggerOptions> _options;

	private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers;

	private ConcurrentDictionary<string, ConsoleFormatter> _formatters;

	private readonly ConsoleLoggerProcessor _messageQueue;

	private IDisposable _optionsReloadToken;

	private IExternalScopeProvider _scopeProvider = Microsoft.Extensions.Logging.NullExternalScopeProvider.Instance;

	public ConsoleLoggerProvider(IOptionsMonitor<ConsoleLoggerOptions> options)
		: this(options, Array.Empty<ConsoleFormatter>())
	{
	}

	public ConsoleLoggerProvider(IOptionsMonitor<ConsoleLoggerOptions> options, IEnumerable<ConsoleFormatter> formatters)
	{
		_options = options;
		_loggers = new ConcurrentDictionary<string, ConsoleLogger>();
		SetFormatters(formatters);
		ReloadLoggerOptions(options.CurrentValue);
		_optionsReloadToken = _options.OnChange(ReloadLoggerOptions);
		_messageQueue = new ConsoleLoggerProcessor();
		if (DoesConsoleSupportAnsi())
		{
			_messageQueue.Console = new AnsiLogConsole();
			_messageQueue.ErrorConsole = new AnsiLogConsole(stdErr: true);
		}
		else
		{
			_messageQueue.Console = new AnsiParsingLogConsole();
			_messageQueue.ErrorConsole = new AnsiParsingLogConsole(stdErr: true);
		}
	}

	[UnsupportedOSPlatformGuard("windows")]
	private static bool DoesConsoleSupportAnsi()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return true;
		}
		IntPtr stdHandle = global::Interop.Kernel32.GetStdHandle(-11);
		if (!global::Interop.Kernel32.GetConsoleMode(stdHandle, out var mode))
		{
			return false;
		}
		return ((ulong)mode & 4uL) == 4;
	}

	private void SetFormatters(IEnumerable<ConsoleFormatter> formatters = null)
	{
		ConcurrentDictionary<string, ConsoleFormatter> concurrentDictionary = new ConcurrentDictionary<string, ConsoleFormatter>(StringComparer.OrdinalIgnoreCase);
		bool flag = false;
		if (formatters != null)
		{
			foreach (ConsoleFormatter formatter in formatters)
			{
				concurrentDictionary.TryAdd(formatter.Name, formatter);
				flag = true;
			}
		}
		if (!flag)
		{
			concurrentDictionary.TryAdd("simple", new SimpleConsoleFormatter(new FormatterOptionsMonitor<SimpleConsoleFormatterOptions>(new SimpleConsoleFormatterOptions())));
			concurrentDictionary.TryAdd("systemd", new SystemdConsoleFormatter(new FormatterOptionsMonitor<ConsoleFormatterOptions>(new ConsoleFormatterOptions())));
			concurrentDictionary.TryAdd("json", new JsonConsoleFormatter(new FormatterOptionsMonitor<JsonConsoleFormatterOptions>(new JsonConsoleFormatterOptions())));
		}
		_formatters = concurrentDictionary;
	}

	private void ReloadLoggerOptions(ConsoleLoggerOptions options)
	{
		if (options.FormatterName == null || !_formatters.TryGetValue(options.FormatterName, out var value))
		{
			ConsoleLoggerFormat format = options.Format;
			ConsoleFormatter consoleFormatter = ((format != ConsoleLoggerFormat.Systemd) ? _formatters["simple"] : _formatters["systemd"]);
			value = consoleFormatter;
			if (options.FormatterName == null)
			{
				UpdateFormatterOptions(value, options);
			}
		}
		foreach (KeyValuePair<string, ConsoleLogger> logger in _loggers)
		{
			logger.Value.Options = options;
			logger.Value.Formatter = value;
		}
	}

	public ILogger CreateLogger(string name)
	{
		if (_options.CurrentValue.FormatterName == null || !_formatters.TryGetValue(_options.CurrentValue.FormatterName, out var value))
		{
			ConsoleLoggerFormat format = _options.CurrentValue.Format;
			ConsoleFormatter consoleFormatter = ((format != ConsoleLoggerFormat.Systemd) ? _formatters["simple"] : _formatters["systemd"]);
			value = consoleFormatter;
			if (_options.CurrentValue.FormatterName == null)
			{
				UpdateFormatterOptions(value, _options.CurrentValue);
			}
		}
		if (!_loggers.TryGetValue(name, out var value2))
		{
			return _loggers.GetOrAdd(name, new ConsoleLogger(name, _messageQueue)
			{
				Options = _options.CurrentValue,
				ScopeProvider = _scopeProvider,
				Formatter = value
			});
		}
		return value2;
	}

	private void UpdateFormatterOptions(ConsoleFormatter formatter, ConsoleLoggerOptions deprecatedFromOptions)
	{
		if (formatter is SimpleConsoleFormatter simpleConsoleFormatter)
		{
			simpleConsoleFormatter.FormatterOptions = new SimpleConsoleFormatterOptions
			{
				ColorBehavior = ((!deprecatedFromOptions.DisableColors) ? LoggerColorBehavior.Enabled : LoggerColorBehavior.Disabled),
				IncludeScopes = deprecatedFromOptions.IncludeScopes,
				TimestampFormat = deprecatedFromOptions.TimestampFormat,
				UseUtcTimestamp = deprecatedFromOptions.UseUtcTimestamp
			};
		}
		else if (formatter is SystemdConsoleFormatter systemdConsoleFormatter)
		{
			systemdConsoleFormatter.FormatterOptions = new ConsoleFormatterOptions
			{
				IncludeScopes = deprecatedFromOptions.IncludeScopes,
				TimestampFormat = deprecatedFromOptions.TimestampFormat,
				UseUtcTimestamp = deprecatedFromOptions.UseUtcTimestamp
			};
		}
	}

	public void Dispose()
	{
		_optionsReloadToken?.Dispose();
		_messageQueue.Dispose();
	}

	public void SetScopeProvider(IExternalScopeProvider scopeProvider)
	{
		_scopeProvider = scopeProvider;
		foreach (KeyValuePair<string, ConsoleLogger> logger in _loggers)
		{
			logger.Value.ScopeProvider = _scopeProvider;
		}
	}
}
