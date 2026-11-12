using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Utils;

internal static class JsonUtils
{
	public static string GetExistingOrEmptyString(JObject json, string key)
	{
		if (json.TryGetValue(key, out var value))
		{
			return value.ToObject<string>();
		}
		return string.Empty;
	}

	public static string ExtractExistingOrEmptyString(JObject json, string key)
	{
		if (json.TryGetValue(key, out var value))
		{
			string result = value.ToObject<string>();
			json.Remove(key);
			return result;
		}
		return string.Empty;
	}

	public static IDictionary<string, string> ExtractInnerJsonAsDictionary(JObject json, string key)
	{
		if (json.TryGetValue(key, out var value))
		{
			Dictionary<string, string> result = ((JObject)value).ToDictionary((KeyValuePair<string, JToken> pair) => pair.Key, (KeyValuePair<string, JToken> pair) => (string)pair.Value);
			json.Remove(key);
			return result;
		}
		return null;
	}

	public static T ExtractExistingOrDefault<T>(JObject json, string key)
	{
		if (json.TryGetValue(key, out var value))
		{
			T result = value.ToObject<T>();
			json.Remove(key);
			return result;
		}
		return default(T);
	}

	public static long ExtractParsedIntOrZero(JObject json, string key)
	{
		string text = ExtractExistingOrEmptyString(json, key);
		if (!string.IsNullOrWhiteSpace(text) && long.TryParse(text, out var result))
		{
			return result;
		}
		return 0L;
	}
}
