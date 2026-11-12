using System;

namespace Microsoft.Identity.Json.Utilities;

internal static class ValidationUtils
{
	public static void ArgumentNotNull(object value, string parameterName)
	{
		if (value == null)
		{
			throw new ArgumentNullException(parameterName);
		}
	}
}
