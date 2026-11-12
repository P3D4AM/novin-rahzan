using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment;

internal static class CollectionUtil
{
	public static TDictionary UnionWith<TDictionary, TKey, TValue>(this TDictionary dict, IReadOnlyDictionary<TKey, TValue> other) where TDictionary : IDictionary<TKey, TValue>
	{
		foreach (KeyValuePair<TKey, TValue> item in other)
		{
			if (!dict.ContainsKey(item.Key))
			{
				dict.Add(item.Key, item.Value);
			}
		}
		return dict;
	}

	public static TDictionary PrefixKeys<TDictionary>(this string prefix, TDictionary dictionary) where TDictionary : IDictionary<string, object>
	{
		foreach (string item in dictionary.Keys.ToList())
		{
			string key = prefix + "." + item;
			dictionary.Add(key, dictionary[item]);
			dictionary.Remove(item);
		}
		return dictionary;
	}

	public static TDictionary PrefixKeys<TDictionary>(this TDictionary dictionary, string prefix) where TDictionary : IDictionary<string, object>
	{
		return prefix.PrefixKeys(dictionary);
	}

	public static TDictionary Add<TDictionary>(this TDictionary dictionary, KeyValuePair<string, string> pair) where TDictionary : IDictionary<string, object>
	{
		dictionary.Add(pair.Key, pair.Value);
		return dictionary;
	}

	public static HashSet<T> SplitWith<T>(this HashSet<T> left, HashSet<T> right)
	{
		HashSet<T> hashSet = new HashSet<T>(left);
		hashSet.IntersectWith(right);
		left.ExceptWith(right);
		right.ExceptWith(left);
		right.ExceptWith(hashSet);
		return hashSet;
	}
}
