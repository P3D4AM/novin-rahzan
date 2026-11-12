using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Caching.Distributed;

public class MemoryDistributedCache : IDistributedCache
{
	private static readonly Task CompletedTask = Task.FromResult<object>(null);

	private readonly IMemoryCache _memCache;

	public MemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor)
		: this(optionsAccessor, NullLoggerFactory.Instance)
	{
	}

	public MemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
	{
		if (optionsAccessor == null)
		{
			throw new ArgumentNullException("optionsAccessor");
		}
		if (loggerFactory == null)
		{
			throw new ArgumentNullException("loggerFactory");
		}
		_memCache = new MemoryCache(optionsAccessor.Value, loggerFactory);
	}

	public byte[] Get(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		return (byte[])_memCache.Get(key);
	}

	public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		return Task.FromResult(Get(key));
	}

	public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
		memoryCacheEntryOptions.AbsoluteExpiration = options.AbsoluteExpiration;
		memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
		memoryCacheEntryOptions.SlidingExpiration = options.SlidingExpiration;
		memoryCacheEntryOptions.Size = value.Length;
		_memCache.Set(key, value, memoryCacheEntryOptions);
	}

	public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		Set(key, value, options);
		return CompletedTask;
	}

	public void Refresh(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_memCache.TryGetValue(key, out var _);
	}

	public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		Refresh(key);
		return CompletedTask;
	}

	public void Remove(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_memCache.Remove(key);
	}

	public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken))
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		Remove(key);
		return CompletedTask;
	}
}
