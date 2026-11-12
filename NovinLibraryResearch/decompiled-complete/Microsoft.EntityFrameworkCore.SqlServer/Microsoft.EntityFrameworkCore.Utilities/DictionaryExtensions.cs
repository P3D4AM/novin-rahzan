using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Utilities;

[DebuggerStepThrough]
internal static class DictionaryExtensions
{
	public static TValue GetOrAddNew<TKey, TValue>([JetBrains.Annotations.NotNull] this IDictionary<TKey, TValue> source, [JetBrains.Annotations.NotNull] TKey key) where TValue : new()
	{
		if (!source.TryGetValue(key, out var value))
		{
			value = new TValue();
			source.Add(key, value);
		}
		return value;
	}

	public static TValue Find<TKey, TValue>([JetBrains.Annotations.NotNull] this IReadOnlyDictionary<TKey, TValue> source, [JetBrains.Annotations.NotNull] TKey key)
	{
		if (source.TryGetValue(key, out var value))
		{
			return value;
		}
		return default(TValue);
	}
}
