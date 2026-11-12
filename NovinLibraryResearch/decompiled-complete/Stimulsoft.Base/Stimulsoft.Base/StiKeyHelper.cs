using System;
using System.Text.RegularExpressions;

namespace Stimulsoft.Base;

public static class StiKeyHelper
{
	public static string GenerateKey()
	{
		return Guid.NewGuid().ToString().Replace("-", string.Empty);
	}

	internal static string GenerateShortKey(int max)
	{
		return GenerateKey().Substring(0, max).ToLowerInvariant();
	}

	public static bool IsKey(string key)
	{
		return !IsEmptyKey(key);
	}

	public static bool IsCorrectKey(string key)
	{
		if (IsEmptyKey(key))
		{
			return true;
		}
		if (key.Length != 32)
		{
			return false;
		}
		return Regex.IsMatch(key, "^[a-zA-Z0-9]*$");
	}

	public static bool IsEmptyKey(string key)
	{
		return string.IsNullOrWhiteSpace(key);
	}

	public static bool IsEmptyKey(string key1, string key2)
	{
		if (IsEmptyKey(key1))
		{
			return IsEmptyKey(key2);
		}
		return false;
	}

	public static string SelectKey(string key1, string key2)
	{
		if (!IsEmptyKey(key1))
		{
			return key1;
		}
		return key2;
	}

	public static bool IsEqualKeys(string key1, string key2)
	{
		if (IsEmptyKey(key1) && IsEmptyKey(key2))
		{
			return true;
		}
		key1 = key1.Trim().ToLowerInvariant();
		key2 = key2.Trim().ToLowerInvariant();
		return string.Compare(key1, key2, StringComparison.Ordinal) == 0;
	}

	public static string GetOrGeneratedKey(string key)
	{
		if (!IsEmptyKey(key))
		{
			return key;
		}
		return GenerateKey();
	}

	public static string GetOrGeneratedKey(string key1, string key2)
	{
		return GetOrGeneratedKey(SelectKey(key1, key2));
	}
}
