using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.Internal.Logger;

internal class MsalLogger : ICoreLogger
{
	private readonly IPlatformLogger _platformLogger;

	private readonly LogCallback _loggingCallback;

	private readonly LogLevel _minLogLevel;

	private readonly bool _isDefaultPlatformLoggingEnabled;

	private static readonly Lazy<ICoreLogger> s_nullLogger = new Lazy<ICoreLogger>(() => new NullLogger());

	private readonly Guid _correlationId;

	public static ICoreLogger NullLogger => s_nullLogger.Value;

	public bool PiiLoggingEnabled { get; }

	public string ClientName { get; }

	public string ClientVersion { get; }

	internal string ClientInformation { get; }

	internal MsalLogger(Guid correlationId, string clientName, string clientVersion, LogLevel logLevel, bool enablePiiLogging, bool isDefaultPlatformLoggingEnabled, LogCallback loggingCallback)
	{
		_correlationId = correlationId;
		PiiLoggingEnabled = enablePiiLogging;
		_loggingCallback = loggingCallback;
		_minLogLevel = logLevel;
		_isDefaultPlatformLoggingEnabled = isDefaultPlatformLoggingEnabled;
		_platformLogger = PlatformProxyFactory.CreatePlatformProxy(null).PlatformLogger;
		ClientName = clientName ?? string.Empty;
		ClientVersion = clientVersion ?? string.Empty;
		ClientInformation = string.Empty;
		if (!string.IsNullOrEmpty(clientName) && !"UnknownClient".Equals(clientName))
		{
			if (string.IsNullOrEmpty(clientVersion))
			{
				ClientInformation = string.Format(CultureInfo.InvariantCulture, " ({0})", clientName);
			}
			else
			{
				ClientInformation = string.Format(CultureInfo.InvariantCulture, " ({0}: {1})", clientName, clientVersion);
			}
		}
	}

	public static ICoreLogger Create(Guid correlationId, IApplicationConfiguration config, bool isDefaultPlatformLoggingEnabled = false)
	{
		return new MsalLogger(correlationId, config?.ClientName ?? string.Empty, config?.ClientVersion ?? string.Empty, config?.LogLevel ?? LogLevel.Verbose, config?.EnablePiiLogging ?? false, config?.IsDefaultPlatformLoggingEnabled ?? isDefaultPlatformLoggingEnabled, config?.LoggingCallback);
	}

	public void Info(string messageScrubbed)
	{
		Log(LogLevel.Info, string.Empty, messageScrubbed);
	}

	public void InfoPii(string messageWithPii, string messageScrubbed)
	{
		Log(LogLevel.Info, messageWithPii, messageScrubbed);
	}

	public void InfoPii(Exception exWithPii)
	{
		Log(LogLevel.Info, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void InfoPiiWithPrefix(Exception exWithPii, string prefix)
	{
		Log(LogLevel.Info, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void Verbose(string messageScrubbed)
	{
		Log(LogLevel.Verbose, string.Empty, messageScrubbed);
	}

	public void VerbosePii(string messageWithPii, string messageScrubbed)
	{
		Log(LogLevel.Verbose, messageWithPii, messageScrubbed);
	}

	public void Warning(string messageScrubbed)
	{
		Log(LogLevel.Warning, string.Empty, messageScrubbed);
	}

	public void WarningPii(string messageWithPii, string messageScrubbed)
	{
		Log(LogLevel.Warning, messageWithPii, messageScrubbed);
	}

	public void WarningPii(Exception exWithPii)
	{
		Log(LogLevel.Warning, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void WarningPiiWithPrefix(Exception exWithPii, string prefix)
	{
		Log(LogLevel.Warning, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void Error(string messageScrubbed)
	{
		Log(LogLevel.Error, string.Empty, messageScrubbed);
	}

	public void ErrorPiiWithPrefix(Exception exWithPii, string prefix)
	{
		Log(LogLevel.Error, prefix + exWithPii.ToString(), prefix + GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void ErrorPii(string messageWithPii, string messageScrubbed)
	{
		Log(LogLevel.Error, messageWithPii, messageScrubbed);
	}

	public void ErrorPii(Exception exWithPii)
	{
		Log(LogLevel.Error, exWithPii.ToString(), GetPiiScrubbedExceptionDetails(exWithPii));
	}

	public void Log(LogLevel logLevel, string messageWithPii, string messageScrubbed)
	{
		if (!IsLoggingEnabled(logLevel))
		{
			return;
		}
		Guid correlationId = _correlationId;
		string text = (correlationId.Equals(Guid.Empty) ? string.Empty : (" - " + _correlationId));
		IDictionary<string, string> msalIdParameters = MsalIdHelper.GetMsalIdParameters(this);
		string text2 = "N/A";
		if (msalIdParameters.TryGetValue("x-client-OS", out var value))
		{
			text2 = value;
		}
		bool flag = !string.IsNullOrWhiteSpace(messageWithPii) && PiiLoggingEnabled;
		string text3 = (flag ? messageWithPii : messageScrubbed);
		string message = string.Format(CultureInfo.InvariantCulture, "{0} MSAL {1} {2} {3} [{4}{5}]{6} {7}", flag ? "(True)" : "(False)", MsalIdHelper.GetMsalVersion(), msalIdParameters["x-client-SKU"], text2, DateTime.UtcNow, text, ClientInformation, text3);
		if (_isDefaultPlatformLoggingEnabled)
		{
			switch (logLevel)
			{
			case LogLevel.Error:
				_platformLogger.Error(message);
				break;
			case LogLevel.Warning:
				_platformLogger.Warning(message);
				break;
			case LogLevel.Info:
				_platformLogger.Information(message);
				break;
			case LogLevel.Verbose:
				_platformLogger.Verbose(message);
				break;
			}
		}
		_loggingCallback(logLevel, message, flag);
	}

	internal static string GetPiiScrubbedExceptionDetails(Exception ex)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (ex != null)
		{
			stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Exception type: {0}", ex.GetType()));
			if (ex is MsalException ex2)
			{
				stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, ", ErrorCode: {0}", ex2.ErrorCode));
			}
			if (ex is MsalServiceException ex3)
			{
				stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "HTTP StatusCode {0}", ex3.StatusCode));
				stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "CorrelationId {0}", ex3.CorrelationId));
			}
			if (ex.InnerException != null)
			{
				stringBuilder.AppendLine("---> Inner Exception Details");
				stringBuilder.AppendLine(GetPiiScrubbedExceptionDetails(ex.InnerException));
				stringBuilder.AppendLine("=== End of inner exception stack trace ===");
			}
			if (ex.StackTrace != null)
			{
				stringBuilder.Append(Environment.NewLine + ex.StackTrace);
			}
		}
		return stringBuilder.ToString();
	}

	public DurationLogHelper LogBlockDuration(string measuredBlockName, LogLevel logLevel = LogLevel.Verbose)
	{
		return new DurationLogHelper(this, measuredBlockName, logLevel);
	}

	public DurationLogHelper LogMethodDuration(LogLevel logLevel = LogLevel.Verbose, [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null)
	{
		string text = ((!string.IsNullOrEmpty(filePath)) ? Path.GetFileNameWithoutExtension(filePath) : "");
		return LogBlockDuration(text + ":" + methodName, logLevel);
	}

	public bool IsLoggingEnabled(LogLevel logLevel)
	{
		if (_loggingCallback != null)
		{
			return logLevel <= _minLogLevel;
		}
		return false;
	}
}
