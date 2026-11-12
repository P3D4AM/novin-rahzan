using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;

namespace Microsoft.IdentityModel.Logging;

public class LogHelper
{
	public static ArgumentNullException LogArgumentNullException(string argument)
	{
		return LogArgumentException<ArgumentNullException>(EventLevel.Error, argument, "IDX10000: The parameter '{0}' cannot be a 'null' or an empty object.", new object[1] { argument });
	}

	public static T LogException<T>(string message) where T : Exception
	{
		return LogException<T>(EventLevel.Error, null, message, null);
	}

	public static T LogArgumentException<T>(string argumentName, string message) where T : ArgumentException
	{
		return LogArgumentException<T>(EventLevel.Error, argumentName, null, message, null);
	}

	public static T LogException<T>(string format, params object[] args) where T : Exception
	{
		return LogException<T>(EventLevel.Error, null, format, args);
	}

	public static T LogArgumentException<T>(string argumentName, string format, params object[] args) where T : ArgumentException
	{
		return LogArgumentException<T>(EventLevel.Error, argumentName, null, format, args);
	}

	public static T LogException<T>(Exception innerException, string message) where T : Exception
	{
		return LogException<T>(EventLevel.Error, innerException, message, null);
	}

	public static T LogArgumentException<T>(string argumentName, Exception innerException, string message) where T : ArgumentException
	{
		return LogArgumentException<T>(EventLevel.Error, argumentName, innerException, message, null);
	}

	public static T LogException<T>(Exception innerException, string format, params object[] args) where T : Exception
	{
		return LogException<T>(EventLevel.Error, innerException, format, args);
	}

	public static T LogArgumentException<T>(string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
	{
		return LogArgumentException<T>(EventLevel.Error, argumentName, innerException, format, args);
	}

	public static T LogException<T>(EventLevel eventLevel, string message) where T : Exception
	{
		return LogException<T>(eventLevel, null, message, null);
	}

	public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, string message) where T : ArgumentException
	{
		return LogArgumentException<T>(eventLevel, argumentName, null, message, null);
	}

	public static T LogException<T>(EventLevel eventLevel, string format, params object[] args) where T : Exception
	{
		return LogException<T>(eventLevel, null, format, args);
	}

	public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, string format, params object[] args) where T : ArgumentException
	{
		return LogArgumentException<T>(eventLevel, argumentName, null, format, args);
	}

	public static T LogException<T>(EventLevel eventLevel, Exception innerException, string message) where T : Exception
	{
		return LogException<T>(eventLevel, innerException, message, null);
	}

	public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, Exception innerException, string message) where T : ArgumentException
	{
		return LogArgumentException<T>(eventLevel, argumentName, innerException, message, null);
	}

	public static T LogException<T>(EventLevel eventLevel, Exception innerException, string format, params object[] args) where T : Exception
	{
		return LogExceptionImpl<T>(eventLevel, null, innerException, format, args);
	}

	public static T LogArgumentException<T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : ArgumentException
	{
		return LogExceptionImpl<T>(eventLevel, argumentName, innerException, format, args);
	}

	public static Exception LogExceptionMessage(Exception exception)
	{
		return LogExceptionMessage(EventLevel.Error, exception);
	}

	public static Exception LogExceptionMessage(EventLevel eventLevel, Exception exception)
	{
		if (exception == null)
		{
			return null;
		}
		if (IdentityModelEventSource.Logger.IsEnabled() && IdentityModelEventSource.Logger.LogLevel >= eventLevel)
		{
			IdentityModelEventSource.Logger.Write(eventLevel, exception.InnerException, exception.Message);
		}
		return exception;
	}

	public static void LogInformation(string message, params object[] args)
	{
		if (IdentityModelEventSource.Logger.IsEnabled())
		{
			IdentityModelEventSource.Logger.WriteInformation(message, args);
		}
	}

	public static void LogVerbose(string message, params object[] args)
	{
		if (IdentityModelEventSource.Logger.IsEnabled())
		{
			IdentityModelEventSource.Logger.WriteVerbose(message, args);
		}
	}

	public static void LogWarning(string message, params object[] args)
	{
		if (IdentityModelEventSource.Logger.IsEnabled())
		{
			IdentityModelEventSource.Logger.WriteWarning(message, args);
		}
	}

	private static T LogExceptionImpl<T>(EventLevel eventLevel, string argumentName, Exception innerException, string format, params object[] args) where T : Exception
	{
		string text = null;
		text = ((args == null) ? format : string.Format(CultureInfo.InvariantCulture, format, args));
		if (IdentityModelEventSource.Logger.IsEnabled() && IdentityModelEventSource.Logger.LogLevel >= eventLevel)
		{
			IdentityModelEventSource.Logger.Write(eventLevel, innerException, text);
		}
		if (innerException != null)
		{
			if (string.IsNullOrEmpty(argumentName))
			{
				return (T)Activator.CreateInstance(typeof(T), text, innerException);
			}
			return (T)Activator.CreateInstance(typeof(T), argumentName, text, innerException);
		}
		if (string.IsNullOrEmpty(argumentName))
		{
			return (T)Activator.CreateInstance(typeof(T), text);
		}
		return (T)Activator.CreateInstance(typeof(T), argumentName, text);
	}

	public static string FormatInvariant(string format, params object[] args)
	{
		if (format == null)
		{
			return string.Empty;
		}
		if (args == null)
		{
			return format;
		}
		if (!IdentityModelEventSource.ShowPII)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] args2 = args.Select(RemovePII).ToArray();
			return string.Format(invariantCulture, format, args2);
		}
		return string.Format(CultureInfo.InvariantCulture, format, args);
	}

	private static string RemovePII(object arg)
	{
		if (arg == null)
		{
			return string.Empty;
		}
		if (arg is Exception ex && ex.GetType().FullName.StartsWith("Microsoft.IdentityModel.", StringComparison.Ordinal))
		{
			return ex.ToString();
		}
		return arg.GetType().ToString();
	}

	internal static bool IsCustomException(Exception ex)
	{
		return ex.GetType().FullName.StartsWith("Microsoft.IdentityModel.", StringComparison.Ordinal);
	}
}
