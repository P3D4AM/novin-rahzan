using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Internal;

public class ServiceProviderCache
{
	private readonly ConcurrentDictionary<long, (IServiceProvider ServiceProvider, IDictionary<string, string> DebugInfo)> _configurations = new ConcurrentDictionary<long, (IServiceProvider, IDictionary<string, string>)>();

	public static ServiceProviderCache Instance { get; } = new ServiceProviderCache();


	public virtual IServiceProvider GetOrAdd([JetBrains.Annotations.NotNull] IDbContextOptions options, bool providerRequired)
	{
		CoreOptionsExtension coreOptionsExtension = options.FindExtension<CoreOptionsExtension>();
		IServiceProvider serviceProvider = coreOptionsExtension?.InternalServiceProvider;
		if (serviceProvider != null)
		{
			ValidateOptions(options);
			ISingletonOptionsInitializer service = ServiceProviderServiceExtensions.GetService<ISingletonOptionsInitializer>(serviceProvider);
			if (service == null)
			{
				throw new InvalidOperationException(CoreStrings.NoEfServices);
			}
			if (providerRequired)
			{
				service.EnsureInitialized(serviceProvider, options);
			}
			return serviceProvider;
		}
		CoreOptionsExtension coreOptionsExtension2 = coreOptionsExtension;
		if (coreOptionsExtension2 != null && !coreOptionsExtension2.ServiceProviderCachingEnabled)
		{
			return BuildServiceProvider().ServiceProvider;
		}
		long key = options.Extensions.OrderBy((IDbContextOptionsExtension e) => e.GetType().Name).Aggregate(0L, (long t, IDbContextOptionsExtension e) => (t * 397) ^ ((long)e.GetType().GetHashCode() * 397L) ^ e.Info.GetServiceProviderHashCode());
		return _configurations.GetOrAdd(key, (long k) => BuildServiceProvider()).ServiceProvider;
		(IServiceProvider ServiceProvider, IDictionary<string, string> DebugInfo) BuildServiceProvider()
		{
			ValidateOptions(options);
			Dictionary<string, string> debugInfo = new Dictionary<string, string>();
			foreach (IDbContextOptionsExtension extension in options.Extensions)
			{
				extension.Info.PopulateDebugInfo(debugInfo);
			}
			debugInfo = debugInfo.OrderBy((KeyValuePair<string, string> v) => debugInfo.Keys).ToDictionary((KeyValuePair<string, string> d) => d.Key, (KeyValuePair<string, string> v) => v.Value);
			ServiceCollection serviceCollection = new ServiceCollection();
			bool flag = ApplyServices(options, serviceCollection);
			IReadOnlyDictionary<Type, Type> readOnlyDictionary = coreOptionsExtension?.ReplacedServices;
			if (readOnlyDictionary != null)
			{
				ServiceCollection serviceCollection2 = new ServiceCollection();
				foreach (ServiceDescriptor item in serviceCollection)
				{
					if (readOnlyDictionary.TryGetValue(item.ServiceType, out var value))
					{
						((ICollection<ServiceDescriptor>)serviceCollection2).Add(new ServiceDescriptor(item.ServiceType, value, item.Lifetime));
					}
				}
				ApplyServices(options, serviceCollection2);
				serviceCollection = serviceCollection2;
			}
			ServiceProvider serviceProvider2 = serviceCollection.BuildServiceProvider();
			if (flag)
			{
				ServiceProviderServiceExtensions.GetRequiredService<ISingletonOptionsInitializer>(serviceProvider2).EnsureInitialized(serviceProvider2, options);
			}
			using (IServiceScope serviceScope = ServiceProviderServiceExtensions.CreateScope(serviceProvider2))
			{
				IServiceProvider serviceProvider3 = serviceScope.ServiceProvider;
				LoggingDefinitions service2 = ServiceProviderServiceExtensions.GetService<LoggingDefinitions>(serviceProvider3);
				if (service2 != null)
				{
					DiagnosticsLogger<DbLoggerCategory.Infrastructure> diagnostics = new DiagnosticsLogger<DbLoggerCategory.Infrastructure>(ScopedLoggerFactory.Create(serviceProvider3, options), ServiceProviderServiceExtensions.GetService<ILoggingOptions>(serviceProvider3), ServiceProviderServiceExtensions.GetService<DiagnosticSource>(serviceProvider3), service2);
					if (_configurations.Count == 0)
					{
						diagnostics.ServiceProviderCreated(serviceProvider2);
					}
					else
					{
						diagnostics.ServiceProviderDebugInfo(debugInfo, _configurations.Values.Select(((IServiceProvider ServiceProvider, IDictionary<string, string> DebugInfo) v) => v.DebugInfo).ToList());
						if (_configurations.Count >= 20)
						{
							diagnostics.ManyServiceProvidersCreatedWarning(_configurations.Values.Select(((IServiceProvider ServiceProvider, IDictionary<string, string> DebugInfo) e) => e.ServiceProvider).ToList());
						}
					}
					IServiceProvider obj = options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider;
					if (((obj != null) ? ServiceProviderServiceExtensions.GetService<IRegisteredServices>(obj) : null) != null)
					{
						diagnostics.RedundantAddServicesCallWarning(serviceProvider2);
					}
				}
			}
			return (ServiceProvider: serviceProvider2, DebugInfo: debugInfo);
		}
	}

	private static void ValidateOptions(IDbContextOptions options)
	{
		foreach (IDbContextOptionsExtension extension in options.Extensions)
		{
			extension.Validate(options);
		}
	}

	private static bool ApplyServices(IDbContextOptions options, ServiceCollection services)
	{
		bool flag = false;
		foreach (IDbContextOptionsExtension extension in options.Extensions)
		{
			extension.ApplyServices(services);
			if (extension.Info.IsDatabaseProvider)
			{
				flag = true;
			}
		}
		if (flag)
		{
			return true;
		}
		new EntityFrameworkServicesBuilder(services).TryAddCoreServices();
		return false;
	}
}
