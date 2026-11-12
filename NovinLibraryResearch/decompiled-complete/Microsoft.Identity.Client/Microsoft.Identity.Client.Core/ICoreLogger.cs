using System;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client.Internal.Logger;

namespace Microsoft.Identity.Client.Core;

internal interface ICoreLogger
{
	string ClientName { get; }

	string ClientVersion { get; }

	bool PiiLoggingEnabled { get; }

	void Error(string messageScrubbed);

	void ErrorPii(string messageWithPii, string messageScrubbed);

	void ErrorPii(Exception exWithPii);

	void ErrorPiiWithPrefix(Exception exWithPii, string prefix);

	void Warning(string messageScrubbed);

	void WarningPii(string messageWithPii, string messageScrubbed);

	void WarningPii(Exception exWithPii);

	void WarningPiiWithPrefix(Exception exWithPii, string prefix);

	void Info(string messageScrubbed);

	void InfoPii(string messageWithPii, string messageScrubbed);

	void InfoPii(Exception exWithPii);

	void InfoPiiWithPrefix(Exception exWithPii, string prefix);

	void Verbose(string messageScrubbed);

	void VerbosePii(string messageWithPii, string messageScrubbed);

	void Log(LogLevel logLevel, string messageWithPii, string messageScrubbed);

	bool IsLoggingEnabled(LogLevel logLevel);

	DurationLogHelper LogBlockDuration(string measuredBlockName, LogLevel logLevel = LogLevel.Verbose);

	DurationLogHelper LogMethodDuration(LogLevel logLevel = LogLevel.Verbose, [CallerMemberName] string methodName = null, [CallerFilePath] string filePath = null);
}
