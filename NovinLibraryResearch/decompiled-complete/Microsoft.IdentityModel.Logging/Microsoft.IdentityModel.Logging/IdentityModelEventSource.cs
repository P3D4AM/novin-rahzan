using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Reflection;

namespace Microsoft.IdentityModel.Logging;

[EventSource(Name = "Microsoft.IdentityModel.EventSource")]
public class IdentityModelEventSource : EventSource
{
	private static string _versionLogMessage;

	private static string _dateLogMessage;

	private static string _piiOffLogMessage;

	private static string _piiOnLogMessage;

	public static IdentityModelEventSource Logger { get; }

	public static bool ShowPII { get; set; }

	public static string HiddenPIIString { get; }

	public static bool HeaderWritten { get; set; }

	public EventLevel LogLevel { get; set; }

	static IdentityModelEventSource()
	{
		ShowPII = false;
		HiddenPIIString = "[PII is hidden. For more details, see https://aka.ms/IdentityModel/PII.]";
		HeaderWritten = false;
		_versionLogMessage = "Library version: {0}.";
		_dateLogMessage = "Date: {0}.";
		_piiOffLogMessage = "PII (personally identifiable information) logging is currently turned off. Set IdentityModelEventSource.ShowPII to 'true' to view the full details of exceptions.";
		_piiOnLogMessage = "PII (personally identifiable information) logging is currently turned on. Set IdentityModelEventSource.ShowPII to 'false' to hide PII from log messages.";
		Logger = new IdentityModelEventSource();
	}

	private IdentityModelEventSource()
	{
		LogLevel = EventLevel.Warning;
	}

	[Event(6, Level = EventLevel.LogAlways)]
	public void WriteAlways(string message)
	{
		if (IsEnabled())
		{
			message = PrepareMessage(EventLevel.LogAlways, message);
			WriteEvent(6, message);
		}
	}

	[NonEvent]
	public void WriteAlways(string message, params object[] args)
	{
		if (IsEnabled())
		{
			if (args != null)
			{
				WriteAlways(LogHelper.FormatInvariant(message, args));
			}
			else
			{
				WriteAlways(message);
			}
		}
	}

	[Event(1, Level = EventLevel.Verbose)]
	public void WriteVerbose(string message)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Verbose)
		{
			message = PrepareMessage(EventLevel.Verbose, message);
			WriteEvent(1, message);
		}
	}

	[NonEvent]
	public void WriteVerbose(string message, params object[] args)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Verbose)
		{
			if (args != null)
			{
				WriteVerbose(LogHelper.FormatInvariant(message, args));
			}
			else
			{
				WriteVerbose(message);
			}
		}
	}

	[Event(2, Level = EventLevel.Informational)]
	public void WriteInformation(string message)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Informational)
		{
			message = PrepareMessage(EventLevel.Informational, message);
			WriteEvent(2, message);
		}
	}

	[NonEvent]
	public void WriteInformation(string message, params object[] args)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Informational)
		{
			if (args != null)
			{
				WriteInformation(LogHelper.FormatInvariant(message, args));
			}
			else
			{
				WriteInformation(message);
			}
		}
	}

	[Event(3, Level = EventLevel.Warning)]
	public void WriteWarning(string message)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Warning)
		{
			message = PrepareMessage(EventLevel.Warning, message);
			WriteEvent(3, message);
		}
	}

	[NonEvent]
	public void WriteWarning(string message, params object[] args)
	{
		if (args != null)
		{
			WriteWarning(LogHelper.FormatInvariant(message, args));
		}
		else
		{
			WriteWarning(message);
		}
	}

	[Event(4, Level = EventLevel.Error)]
	public void WriteError(string message)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Error)
		{
			message = PrepareMessage(EventLevel.Error, message);
			WriteEvent(4, message);
		}
	}

	[NonEvent]
	public void WriteError(string message, params object[] args)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Error)
		{
			if (args != null)
			{
				WriteError(LogHelper.FormatInvariant(message, args));
			}
			else
			{
				WriteError(message);
			}
		}
	}

	[Event(5, Level = EventLevel.Critical)]
	public void WriteCritical(string message)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Critical)
		{
			message = PrepareMessage(EventLevel.Critical, message);
			WriteEvent(5, message);
		}
	}

	[NonEvent]
	public void WriteCritical(string message, params object[] args)
	{
		if (IsEnabled() && LogLevel >= EventLevel.Critical)
		{
			if (args != null)
			{
				WriteCritical(LogHelper.FormatInvariant(message, args));
			}
			else
			{
				WriteCritical(message);
			}
		}
	}

	[NonEvent]
	public void Write(EventLevel level, Exception innerException, string message)
	{
		Write(level, innerException, message, null);
	}

	[NonEvent]
	public void Write(EventLevel level, Exception innerException, string message, params object[] args)
	{
		if (innerException != null)
		{
			message = ((ShowPII || LogHelper.IsCustomException(innerException)) ? string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}", message, innerException.Message) : string.Format(CultureInfo.InvariantCulture, "Message: {0}, InnerException: {1}", message, innerException.GetType()));
		}
		if (!HeaderWritten)
		{
			WriteAlways(string.Format(CultureInfo.InvariantCulture, _versionLogMessage, typeof(IdentityModelEventSource).GetTypeInfo().Assembly.GetName().Version.ToString()));
			WriteAlways(string.Format(CultureInfo.InvariantCulture, _dateLogMessage, DateTime.UtcNow));
			if (ShowPII)
			{
				WriteAlways(_piiOnLogMessage);
			}
			else
			{
				WriteAlways(_piiOffLogMessage);
			}
			HeaderWritten = true;
		}
		switch (level)
		{
		case EventLevel.LogAlways:
			WriteAlways(message, args);
			return;
		case EventLevel.Critical:
			WriteCritical(message, args);
			return;
		case EventLevel.Error:
			WriteError(message, args);
			return;
		case EventLevel.Warning:
			WriteWarning(message, args);
			return;
		case EventLevel.Informational:
			WriteInformation(message, args);
			return;
		case EventLevel.Verbose:
			WriteVerbose(message, args);
			return;
		}
		WriteError(LogHelper.FormatInvariant("MIML10002: Unknown log level: {0}.", level));
		WriteError(message, args);
	}

	private static string PrepareMessage(EventLevel level, string message, params object[] args)
	{
		if (message == null)
		{
			return string.Empty;
		}
		try
		{
			if (args != null && args.Length != 0)
			{
				return string.Format(CultureInfo.InvariantCulture, "[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), LogHelper.FormatInvariant(message, args));
			}
			return string.Format(CultureInfo.InvariantCulture, "[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), message);
		}
		catch
		{
		}
		try
		{
			return LogHelper.FormatInvariant("[{0}]{1} {2}", level.ToString(), DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), message);
		}
		catch (Exception)
		{
			return level.ToString() + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + message;
		}
	}
}
