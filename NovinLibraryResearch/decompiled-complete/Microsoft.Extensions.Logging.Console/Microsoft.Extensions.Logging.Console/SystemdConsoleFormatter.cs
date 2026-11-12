using System;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Console;

internal sealed class SystemdConsoleFormatter : ConsoleFormatter, IDisposable
{
	private IDisposable _optionsReloadToken;

	internal ConsoleFormatterOptions FormatterOptions { get; set; }

	public SystemdConsoleFormatter(IOptionsMonitor<ConsoleFormatterOptions> options)
		: base("systemd")
	{
		ReloadLoggerOptions(options.CurrentValue);
		_optionsReloadToken = options.OnChange(ReloadLoggerOptions);
	}

	private void ReloadLoggerOptions(ConsoleFormatterOptions options)
	{
		FormatterOptions = options;
	}

	public void Dispose()
	{
		_optionsReloadToken?.Dispose();
	}

	public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
	{
		string text = logEntry.Formatter(logEntry.State, logEntry.Exception);
		if (logEntry.Exception != null || text != null)
		{
			LogLevel logLevel = logEntry.LogLevel;
			string category = logEntry.Category;
			int id = logEntry.EventId.Id;
			Exception exception = logEntry.Exception;
			string syslogSeverityString = GetSyslogSeverityString(logLevel);
			textWriter.Write(syslogSeverityString);
			string timestampFormat = FormatterOptions.TimestampFormat;
			if (timestampFormat != null)
			{
				textWriter.Write(GetCurrentDateTime().ToString(timestampFormat));
			}
			textWriter.Write(category);
			textWriter.Write('[');
			textWriter.Write(id);
			textWriter.Write(']');
			WriteScopeInformation(textWriter, scopeProvider);
			if (!string.IsNullOrEmpty(text))
			{
				textWriter.Write(' ');
				WriteReplacingNewLine(textWriter, text);
			}
			if (exception != null)
			{
				textWriter.Write(' ');
				WriteReplacingNewLine(textWriter, exception.ToString());
			}
			textWriter.Write(Environment.NewLine);
		}
		static void WriteReplacingNewLine(TextWriter writer, string message)
		{
			string value = message.Replace(Environment.NewLine, " ");
			writer.Write(value);
		}
	}

	private DateTimeOffset GetCurrentDateTime()
	{
		if (!FormatterOptions.UseUtcTimestamp)
		{
			return DateTimeOffset.Now;
		}
		return DateTimeOffset.UtcNow;
	}

	private static string GetSyslogSeverityString(LogLevel logLevel)
	{
		return logLevel switch
		{
			LogLevel.Trace => "<7>", 
			LogLevel.Debug => "<7>", 
			LogLevel.Information => "<6>", 
			LogLevel.Warning => "<4>", 
			LogLevel.Error => "<3>", 
			LogLevel.Critical => "<2>", 
			_ => throw new ArgumentOutOfRangeException("logLevel"), 
		};
	}

	private void WriteScopeInformation(TextWriter textWriter, IExternalScopeProvider scopeProvider)
	{
		if (FormatterOptions.IncludeScopes)
		{
			scopeProvider?.ForEachScope(delegate(object scope, TextWriter state)
			{
				state.Write(" => ");
				state.Write(scope);
			}, textWriter);
		}
	}
}
