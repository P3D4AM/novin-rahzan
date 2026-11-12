using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Internal;

public class SingletonOptionsInitializer : ISingletonOptionsInitializer
{
	private volatile bool _isInitialized;

	private readonly object _lock = new object();

	public virtual void EnsureInitialized(IServiceProvider serviceProvider, IDbContextOptions options)
	{
		if (!_isInitialized)
		{
			lock (_lock)
			{
				if (!_isInitialized)
				{
					foreach (ISingletonOptions item in ServiceProviderServiceExtensions.GetService<IEnumerable<ISingletonOptions>>(serviceProvider))
					{
						item.Initialize(options);
					}
					_isInitialized = true;
				}
			}
		}
		foreach (ISingletonOptions item2 in ServiceProviderServiceExtensions.GetService<IEnumerable<ISingletonOptions>>(serviceProvider))
		{
			item2.Validate(options);
		}
	}
}
