using System;
using System.Text.RegularExpressions;

namespace Azure.Identity;

internal static class ScopeUtilities
{
	private const string DefaultSuffix = "/.default";

	private const string ScopePattern = "^[0-9a-zA-Z-.:/]+$";

	private const string InvalidScopeMessage = "The specified scope is not in expected format. Only alphanumeric characters, '.', '-', ':', and '/' are allowed";

	private static readonly Regex scopeRegex = new Regex("^[0-9a-zA-Z-.:/]+$");

	public static string ScopesToResource(string[] scopes)
	{
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		if (scopes.Length != 1)
		{
			throw new ArgumentException("To convert to a resource string the specified array must be exactly length 1", "scopes");
		}
		if (!scopes[0].EndsWith("/.default", StringComparison.Ordinal))
		{
			return scopes[0];
		}
		return scopes[0].Remove(scopes[0].LastIndexOf("/.default", StringComparison.Ordinal));
	}

	public static string[] ResourceToScopes(string resource)
	{
		return new string[1] { resource + "/.default" };
	}

	public static void ValidateScope(string scope)
	{
		if (!scopeRegex.IsMatch(scope))
		{
			throw new ArgumentException("The specified scope is not in expected format. Only alphanumeric characters, '.', '-', ':', and '/' are allowed", "scope");
		}
	}
}
