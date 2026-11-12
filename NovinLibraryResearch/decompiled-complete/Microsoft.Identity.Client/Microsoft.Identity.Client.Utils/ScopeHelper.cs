using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Utils;

internal static class ScopeHelper
{
	public static bool ScopeContains(ISet<string> outerSet, IEnumerable<string> possibleContainedSet)
	{
		foreach (string item in possibleContainedSet)
		{
			if (!outerSet.Contains(item) && !string.IsNullOrEmpty(item))
			{
				return false;
			}
		}
		return true;
	}

	public static HashSet<string> GetMsalScopes(HashSet<string> userScopes)
	{
		return new HashSet<string>(userScopes.Concat(OAuth2Value.ReservedScopes));
	}

	public static HashSet<string> ConvertStringToScopeSet(string singleString)
	{
		if (string.IsNullOrEmpty(singleString))
		{
			return new HashSet<string>();
		}
		return new HashSet<string>(singleString.Split(new char[1] { ' ' }), StringComparer.OrdinalIgnoreCase);
	}

	public static HashSet<string> CreateScopeSet(IEnumerable<string> input)
	{
		if (input == null)
		{
			return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}
		return new HashSet<string>(input, StringComparer.OrdinalIgnoreCase);
	}
}
