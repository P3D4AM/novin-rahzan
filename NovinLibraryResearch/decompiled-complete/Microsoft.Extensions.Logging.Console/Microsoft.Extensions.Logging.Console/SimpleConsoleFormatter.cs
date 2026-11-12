using System;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging.Console;

internal sealed class SimpleConsoleFormatter : ConsoleFormatter, IDisposable
{
	private readonly struct ConsoleColors
	{
		public ConsoleColor? Foreground { get; }

		public ConsoleColor? Background { get; }

		public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
		{
			Foreground = foreground;
			Background = background;
		}
	}

	private const string LoglevelPadding = ": ";

	private static readonly string _messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + ": ".Length);

	private static readonly string _newLineWithMessagePadding = Environment.NewLine + _messagePadding;

	private IDisposable _optionsReloadToken;

	internal SimpleConsoleFormatterOptions FormatterOptions { get; set; }

	public SimpleConsoleFormatter(IOptionsMonitor<SimpleConsoleFormatterOptions> options)
		: base("simple")
	{
		ReloadLoggerOptions(options.CurrentValue);
		_optionsReloadToken = options.OnChange(ReloadLoggerOptions);
	}

	private void ReloadLoggerOptions(SimpleConsoleFormatterOptions options)
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
			ConsoleColors logLevelConsoleColors = GetLogLevelConsoleColors(logLevel);
			string logLevelString = GetLogLevelString(logLevel);
			string text2 = null;
			string timestampFormat = FormatterOptions.TimestampFormat;
			if (timestampFormat != null)
			{
				text2 = GetCurrentDateTime().ToString(timestampFormat);
			}
			if (text2 != null)
			{
				textWriter.Write(text2);
			}
			if (logLevelString != null)
			{
				textWriter.WriteColoredMessage(logLevelString, logLevelConsoleColors.Background, logLevelConsoleColors.Foreground);
			}
			CreateDefaultLogMessage(textWriter, in logEntry, text, scopeProvider);
		}
	}

	private void CreateDefaultLogMessage<TState>(TextWriter textWriter, in LogEntry<TState> logEntry, string message, IExternalScopeProvider scopeProvider)
	{
		bool singleLine = FormatterOptions.SingleLine;
		int id = logEntry.EventId.Id;
		Exception exception = logEntry.Exception;
		textWriter.Write(": ");
		textWriter.Write(logEntry.Category);
		textWriter.Write('[');
		textWriter.Write(id.ToString());
		textWriter.Write(']');
		if (!singleLine)
		{
			textWriter.Write(Environment.NewLine);
		}
		WriteScopeInformation(textWriter, scopeProvider, singleLine);
		WriteMessage(textWriter, message, singleLine);
		if (exception != null)
		{
			WriteMessage(textWriter, exception.ToString(), singleLine);
		}
		if (singleLine)
		{
			textWriter.Write(Environment.NewLine);
		}
	}

	private void WriteMessage(TextWriter textWriter, string message, bool singleLine)
	{
		if (!string.IsNullOrEmpty(message))
		{
			if (singleLine)
			{
				textWriter.Write(' ');
				WriteReplacing(textWriter, Environment.NewLine, " ", message);
			}
			else
			{
				textWriter.Write(_messagePadding);
				WriteReplacing(textWriter, Environment.NewLine, _newLineWithMessagePadding, message);
				textWriter.Write(Environment.NewLine);
			}
		}
		static void WriteReplacing(TextWriter writer, string oldValue, string newValue, string message)
		{
			string value = message.Replace(oldValue, newValue);
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

	private static string GetLogLevelString(LogLevel logLevel)
	{
		return logLevel switch
		{
			LogLevel.Trace => "trce", 
			LogLevel.Debug => "dbug", 
			LogLevel.Information => "info", 
			LogLevel.Warning => "warn", 
			LogLevel.Error => "fail", 
			LogLevel.Critical => "crit", 
			_ => throw new ArgumentOutOfRangeException("logLevel"), 
		};
	}

	private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel)
	{
		if (FormatterOptions.ColorBehavior == LoggerColorBehavior.Disabled || (FormatterOptions.ColorBehavior == LoggerColorBehavior.Default && System.Console.IsOutputRedirected))
		{
			return new ConsoleColors(null, null);
		}
		return logLevel switch
		{
			LogLevel.Trace => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black), 
			LogLevel.Debug => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black), 
			LogLevel.Information => new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black), 
			LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black), 
			LogLevel.Error => new ConsoleColors(ConsoleColor.Black, ConsoleColor.DarkRed), 
			LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.DarkRed), 
			_ => new ConsoleColors(null, null), 
		};
	}

	private void WriteScopeInformation(TextWriter textWriter, IExternalScopeProvider scopeProvider, bool singleLine)
	{
		if (!FormatterOptions.IncludeScopes || scopeProvider == null)
		{
			return;
		}
		bool paddingNeeded = !singleLine;
		scopeProvider.ForEachScope(delegate(object scope, TextWriter state)
		{
			if (paddingNeeded)
			{
				paddingNeeded = false;
				state.Write(_messagePadding);
				state.Write("=> ");
			}
			else
			{
				state.Write(" => ");
			}
			state.Write(scope);
		}, textWriter);
		if (!paddingNeeded && !singleLine)
		{
			textWriter.Write(Environment.NewLine);
		}
	}
}
