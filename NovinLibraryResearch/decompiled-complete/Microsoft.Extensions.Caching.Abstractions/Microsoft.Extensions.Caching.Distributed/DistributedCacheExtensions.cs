using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensions
{
	public static void Set(this IDistributedCache cache, string key, byte[] value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		cache.Set(key, value, new DistributedCacheEntryOptions());
	}

	public static Task SetAsync(this IDistributedCache cache, string key, byte[] value, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return cache.SetAsync(key, value, new DistributedCacheEntryOptions(), token);
	}

	public static void SetString(this IDistributedCache cache, string key, string value)
	{
		cache.SetString(key, value, new DistributedCacheEntryOptions());
	}

	public static void SetString(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		cache.Set(key, Encoding.UTF8.GetBytes(value), options);
	}

	public static Task SetStringAsync(this IDistributedCache cache, string key, string value, CancellationToken token = default(CancellationToken))
	{
		return cache.SetStringAsync(key, value, new DistributedCacheEntryOptions(), token);
	}

	public static Task SetStringAsync(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options, token);
	}

	public static string GetString(this IDistributedCache cache, string key)
	{
		byte[] array = cache.Get(key);
		if (array == null)
		{
			return null;
		}
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	public static async Task<string> GetStringAsync(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken))
	{
		byte[] array = await cache.GetAsync(key, token);
		if (array == null)
		{
			return null;
		}
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}
}
