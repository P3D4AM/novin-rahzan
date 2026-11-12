using System.Globalization;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class MatsUtils
{
	public static bool ContainsCharsThatAreEitherAlphaNumericOrDotsOrUnderscore(string name)
	{
		foreach (char c in name)
		{
			if (c != '_' && c != '.' && !char.IsLetterOrDigit(c))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsValidPropertyName(string name, out string errorMessage)
	{
		errorMessage = string.Empty;
		if (string.IsNullOrEmpty(name))
		{
			errorMessage = "Property name is empty";
			return false;
		}
		if (!ContainsCharsThatAreEitherAlphaNumericOrDotsOrUnderscore(name))
		{
			errorMessage = string.Format(CultureInfo.InvariantCulture, "Property Name '{0}' contains invalid characters", name);
			return false;
		}
		return true;
	}

	public static string NormalizeValidPropertyName(string name, out string errorMessage)
	{
		if (!IsValidPropertyName(name, out errorMessage))
		{
			return string.Empty;
		}
		return name.Replace('.', '_');
	}
}
