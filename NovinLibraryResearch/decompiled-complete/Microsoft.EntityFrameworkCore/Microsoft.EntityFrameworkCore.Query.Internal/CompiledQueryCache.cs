using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class CompiledQueryCache : ICompiledQueryCache
{
	public const string CompiledQueryParameterPrefix = "__";

	private static readonly ConcurrentDictionary<object, object> _querySyncObjects = new ConcurrentDictionary<object, object>();

	private readonly IMemoryCache _memoryCache;

	public CompiledQueryCache([JetBrains.Annotations.NotNull] IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public virtual Func<QueryContext, TResult> GetOrAddQuery<TResult>(object cacheKey, Func<Func<QueryContext, TResult>> compiler)
	{
		return GetOrAddQueryCore(cacheKey, compiler);
	}

	public virtual Func<QueryContext, TResult> GetOrAddAsyncQuery<TResult>(object cacheKey, Func<Func<QueryContext, TResult>> compiler)
	{
		return GetOrAddQueryCore(cacheKey, compiler);
	}

	private Func<QueryContext, TFunc> GetOrAddQueryCore<TFunc>(object cacheKey, Func<Func<QueryContext, TFunc>> compiler)
	{
		Func<QueryContext, TFunc> value;
		while (!_memoryCache.TryGetValue<Func<QueryContext, TFunc>>(cacheKey, out value))
		{
			if (_querySyncObjects.TryAdd(cacheKey, null))
			{
				try
				{
					value = compiler();
					_memoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions
					{
						Size = 10L
					});
				}
				finally
				{
					_querySyncObjects.TryRemove(cacheKey, out var _);
				}
				break;
			}
		}
		return value;
	}
}
