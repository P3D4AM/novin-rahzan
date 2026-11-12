using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.SystemConsole.Platform;
using Serilog.Sinks.SystemConsole.Themes;

namespace Serilog.Sinks.SystemConsole;

internal class ConsoleSink : ILogEventSink
{
	private readonly LogEventLevel? _standardErrorFromLevel;

	private readonly ConsoleTheme _theme;

	private readonly ITextFormatter _formatter;

	private readonly object _syncRoot;

	private const int DefaultWriteBufferCapacity = 256;

	static ConsoleSink()
	{
		WindowsConsole.EnableVirtualTerminalProcessing();
	}

	public ConsoleSink(ConsoleTheme theme, ITextFormatter formatter, LogEventLevel? standardErrorFromLevel, object syncRoot)
	{
		_standardErrorFromLevel = standardErrorFromLevel;
		_theme = theme ?? throw new ArgumentNullException("theme");
		_formatter = formatter;
		_syncRoot = syncRoot ?? throw new ArgumentNullException("syncRoot");
	}

	public void Emit(LogEvent logEvent)
	{
		TextWriter textWriter = SelectOutputStream(logEvent.Level);
		if (_theme.CanBuffer)
		{
			StringWriter stringWriter = new StringWriter(new StringBuilder(256));
			_formatter.Format(logEvent, stringWriter);
			string value = stringWriter.ToString();
			lock (_syncRoot)
			{
				textWriter.Write(value);
				textWriter.Flush();
				return;
			}
		}
		lock (_syncRoot)
		{
			_formatter.Format(logEvent, textWriter);
			textWriter.Flush();
		}
	}

	private TextWriter SelectOutputStream(LogEventLevel logEventLevel)
	{
		LogEventLevel? standardErrorFromLevel = _standardErrorFromLevel;
		if (!standardErrorFromLevel.HasValue)
		{
			return Console.Out;
		}
		if (!(logEventLevel < _standardErrorFromLevel))
		{
			return Console.Error;
		}
		return Console.Out;
	}
}
