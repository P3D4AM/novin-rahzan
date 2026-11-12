using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal static class DictionaryUtil
{
	public static TDictionary Add<TDictionary, TKey, TValue>(this TDictionary dictionary, KeyValuePair<TKey, TValue> pair) where TDictionary : IDictionary<TKey, TValue>
	{
		dictionary.Add(pair.Key, pair.Value);
		return dictionary;
	}
}
