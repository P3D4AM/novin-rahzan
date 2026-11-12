using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class MemoryCacheServiceCollectionExtensions
{
	public static IServiceCollection AddMemoryCache(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		services.AddOptions();
		services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
		return services;
	}

	public static IServiceCollection AddMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> setupAction)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (setupAction == null)
		{
			throw new ArgumentNullException("setupAction");
		}
		services.AddMemoryCache();
		services.Configure(setupAction);
		return services;
	}

	public static IServiceCollection AddDistributedMemoryCache(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		services.AddOptions();
		services.TryAdd(ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());
		return services;
	}

	public static IServiceCollection AddDistributedMemoryCache(this IServiceCollection services, Action<MemoryDistributedCacheOptions> setupAction)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		if (setupAction == null)
		{
			throw new ArgumentNullException("setupAction");
		}
		services.AddDistributedMemoryCache();
		services.Configure(setupAction);
		return services;
	}
}
