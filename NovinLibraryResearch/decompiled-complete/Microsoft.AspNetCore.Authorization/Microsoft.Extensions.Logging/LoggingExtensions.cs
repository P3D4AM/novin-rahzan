using System;

namespace Microsoft.Extensions.Logging;

internal static class LoggingExtensions
{
	private static Action<ILogger, Exception> _userAuthorizationFailed;

	private static Action<ILogger, Exception> _userAuthorizationSucceeded;

	static LoggingExtensions()
	{
		_userAuthorizationSucceeded = LoggerMessage.Define(LogLevel.Information, 1, "Authorization was successful.");
		_userAuthorizationFailed = LoggerMessage.Define(LogLevel.Information, 2, "Authorization failed.");
	}

	public static void UserAuthorizationSucceeded(this ILogger logger)
	{
		_userAuthorizationSucceeded(logger, null);
	}

	public static void UserAuthorizationFailed(this ILogger logger)
	{
		_userAuthorizationFailed(logger, null);
	}
}
