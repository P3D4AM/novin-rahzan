using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory;

public static class CacheExtensions
{
	public static object Get(this IMemoryCache cache, object key)
	{
		cache.TryGetValue(key, out var value);
		return value;
	}

	public static TItem Get<TItem>(this IMemoryCache cache, object key)
	{
		return (TItem)(cache.Get(key) ?? ((object)default(TItem)));
	}

	public static bool TryGetValue<TItem>(this IMemoryCache cache, object key, out TItem value)
	{
		if (cache.TryGetValue(key, out var value2))
		{
			if (value2 == null)
			{
				value = default(TItem);
				return true;
			}
			if (value2 is TItem val)
			{
				value = val;
				return true;
			}
		}
		value = default(TItem);
		return false;
	}

	public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value)
	{
		using ICacheEntry cacheEntry = cache.CreateEntry(key);
		cacheEntry.Value = value;
		return value;
	}

	public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, DateTimeOffset absoluteExpiration)
	{
		using ICacheEntry cacheEntry = cache.CreateEntry(key);
		cacheEntry.AbsoluteExpiration = absoluteExpiration;
		cacheEntry.Value = value;
		return value;
	}

	public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
	{
		using ICacheEntry cacheEntry = cache.CreateEntry(key);
		cacheEntry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
		cacheEntry.Value = value;
		return value;
	}

	public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, IChangeToken expirationToken)
	{
		using ICacheEntry cacheEntry = cache.CreateEntry(key);
		cacheEntry.AddExpirationToken(expirationToken);
		cacheEntry.Value = value;
		return value;
	}

	public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value, MemoryCacheEntryOptions options)
	{
		using ICacheEntry cacheEntry = cache.CreateEntry(key);
		if (options != null)
		{
			cacheEntry.SetOptions(options);
		}
		cacheEntry.Value = value;
		return value;
	}

	public static TItem GetOrCreate<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, TItem> factory)
	{
		if (!cache.TryGetValue(key, out var value))
		{
			using ICacheEntry cacheEntry = cache.CreateEntry(key);
			value = (cacheEntry.Value = factory(cacheEntry));
		}
		return (TItem)value;
	}

	public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCache cache, object key, Func<ICacheEntry, Task<TItem>> factory)
	{
		if (!cache.TryGetValue(key, out var value))
		{
			using ICacheEntry entry = cache.CreateEntry(key);
			value = (entry.Value = await factory(entry).ConfigureAwait(continueOnCapturedContext: false));
		}
		return (TItem)value;
	}
}
