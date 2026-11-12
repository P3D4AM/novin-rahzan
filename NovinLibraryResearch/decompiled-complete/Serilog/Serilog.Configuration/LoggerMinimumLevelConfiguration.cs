using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Configuration;

public class LoggerMinimumLevelConfiguration
{
	private readonly LoggerConfiguration _loggerConfiguration;

	private readonly Action<LogEventLevel> _setMinimum;

	private readonly Action<LoggingLevelSwitch> _setLevelSwitch;

	private readonly Action<string, LoggingLevelSwitch> _addOverride;

	internal LoggerMinimumLevelConfiguration(LoggerConfiguration loggerConfiguration, Action<LogEventLevel> setMinimum, Action<LoggingLevelSwitch> setLevelSwitch, Action<string, LoggingLevelSwitch> addOverride)
	{
		_loggerConfiguration = Guard.AgainstNull(loggerConfiguration, "loggerConfiguration");
		_setMinimum = Guard.AgainstNull(setMinimum, "setMinimum");
		_setLevelSwitch = setLevelSwitch;
		_addOverride = Guard.AgainstNull(addOverride, "addOverride");
	}

	public LoggerConfiguration Is(LogEventLevel minimumLevel)
	{
		_setMinimum(minimumLevel);
		return _loggerConfiguration;
	}

	public LoggerConfiguration ControlledBy(LoggingLevelSwitch levelSwitch)
	{
		Guard.AgainstNull(levelSwitch, "levelSwitch");
		_setLevelSwitch(levelSwitch);
		return _loggerConfiguration;
	}

	public LoggerConfiguration Verbose()
	{
		return Is(LogEventLevel.Verbose);
	}

	public LoggerConfiguration Debug()
	{
		return Is(LogEventLevel.Debug);
	}

	public LoggerConfiguration Information()
	{
		return Is(LogEventLevel.Information);
	}

	public LoggerConfiguration Warning()
	{
		return Is(LogEventLevel.Warning);
	}

	public LoggerConfiguration Error()
	{
		return Is(LogEventLevel.Error);
	}

	public LoggerConfiguration Fatal()
	{
		return Is(LogEventLevel.Fatal);
	}

	public LoggerConfiguration Override(string source, LoggingLevelSwitch levelSwitch)
	{
		Guard.AgainstNull(source, "source");
		Guard.AgainstNull(levelSwitch, "levelSwitch");
		string text = source.Trim();
		if (text.Length == 0)
		{
			throw new ArgumentException("A source source must be provided.", "source");
		}
		_addOverride(text, levelSwitch);
		return _loggerConfiguration;
	}

	public LoggerConfiguration Override(string source, LogEventLevel minimumLevel)
	{
		return Override(source, new LoggingLevelSwitch(minimumLevel));
	}
}
