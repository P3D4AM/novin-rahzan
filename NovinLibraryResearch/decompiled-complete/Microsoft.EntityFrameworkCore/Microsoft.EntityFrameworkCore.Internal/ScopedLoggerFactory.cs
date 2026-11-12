using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Internal;

public class ScopedLoggerFactory : ILoggerFactory, IDisposable
{
	private readonly ILoggerFactory _underlyingFactory;

	private readonly bool _dispose;

	public ScopedLoggerFactory([JetBrains.Annotations.NotNull] ILoggerFactory loggerFactory, bool dispose)
	{
		_underlyingFactory = loggerFactory;
		_dispose = dispose;
	}

	public static ScopedLoggerFactory Create([JetBrains.Annotations.NotNull] IServiceProvider internalServiceProvider, [JetBrains.Annotations.CanBeNull] IDbContextOptions contextOptions)
	{
		CoreOptionsExtension coreOptionsExtension = (contextOptions ?? ServiceProviderServiceExtensions.GetService<IDbContextOptions>(internalServiceProvider))?.FindExtension<CoreOptionsExtension>();
		if (coreOptionsExtension != null)
		{
			if (coreOptionsExtension.LoggerFactory != null)
			{
				return new ScopedLoggerFactory(coreOptionsExtension.LoggerFactory, dispose: false);
			}
			IServiceProvider applicationServiceProvider = coreOptionsExtension.ApplicationServiceProvider;
			if (applicationServiceProvider != null && applicationServiceProvider != internalServiceProvider)
			{
				ILoggerFactory service = ServiceProviderServiceExtensions.GetService<ILoggerFactory>(applicationServiceProvider);
				if (service != null)
				{
					return new ScopedLoggerFactory(service, dispose: false);
				}
			}
		}
		return new ScopedLoggerFactory(new LoggerFactory(), dispose: true);
	}

	public virtual void Dispose()
	{
		if (_dispose)
		{
			_underlyingFactory.Dispose();
		}
	}

	public virtual ILogger CreateLogger(string categoryName)
	{
		return _underlyingFactory.CreateLogger(categoryName);
	}

	public virtual void AddProvider(ILoggerProvider provider)
	{
		_underlyingFactory.AddProvider(provider);
	}
}
