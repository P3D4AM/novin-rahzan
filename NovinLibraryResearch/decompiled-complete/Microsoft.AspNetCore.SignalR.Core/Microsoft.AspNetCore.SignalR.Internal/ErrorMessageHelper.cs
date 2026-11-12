using System;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal static class ErrorMessageHelper
{
	internal static string BuildErrorMessage(string message, Exception exception, bool includeExceptionDetails)
	{
		if (!includeExceptionDetails)
		{
			return message;
		}
		return message + " " + exception.GetType().Name + ": " + exception.Message;
	}
}
