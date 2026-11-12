using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Internal;

public class InternalServiceCollectionMap
{
	private readonly IDictionary<Type, IList<int>> _serviceMap = new Dictionary<Type, IList<int>>();

	public virtual IServiceCollection ServiceCollection { get; }

	public InternalServiceCollectionMap([JetBrains.Annotations.NotNull] IServiceCollection serviceCollection)
	{
		ServiceCollection = serviceCollection;
		int num = 0;
		foreach (ServiceDescriptor item in serviceCollection)
		{
			GetOrCreateDescriptorIndexes(item.ServiceType).Add(num++);
		}
	}

	public virtual IList<int> GetOrCreateDescriptorIndexes([JetBrains.Annotations.NotNull] Type serviceType)
	{
		if (!_serviceMap.TryGetValue(serviceType, out var value))
		{
			value = new List<int>();
			_serviceMap[serviceType] = value;
		}
		return value;
	}

	public virtual void AddNewDescriptor([JetBrains.Annotations.NotNull] IList<int> indexes, [JetBrains.Annotations.NotNull] ServiceDescriptor newDescriptor)
	{
		indexes.Add(ServiceCollection.Count);
		ServiceCollection.Add(newDescriptor);
	}

	public virtual InternalServiceCollectionMap AddDependencySingleton<TDependencies>()
	{
		return AddDependency(typeof(TDependencies), ServiceLifetime.Singleton);
	}

	public virtual InternalServiceCollectionMap AddDependencyScoped<TDependencies>()
	{
		return AddDependency(typeof(TDependencies), ServiceLifetime.Scoped);
	}

	public virtual InternalServiceCollectionMap AddDependency([JetBrains.Annotations.NotNull] Type serviceType, ServiceLifetime lifetime)
	{
		IList<int> orCreateDescriptorIndexes = GetOrCreateDescriptorIndexes(serviceType);
		if (!orCreateDescriptorIndexes.Any())
		{
			AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, serviceType, lifetime));
		}
		else if (orCreateDescriptorIndexes.Count > 1 || ServiceCollection[orCreateDescriptorIndexes[0]].ImplementationType != serviceType)
		{
			throw new InvalidOperationException(CoreStrings.BadDependencyRegistration(serviceType.Name));
		}
		return this;
	}

	public virtual InternalServiceCollectionMap DoPatchInjection<TService>() where TService : class
	{
		if (_serviceMap.TryGetValue(typeof(TService), out var value))
		{
			foreach (int item in value)
			{
				ServiceDescriptor descriptor = ServiceCollection[item];
				ServiceLifetime lifetime = descriptor.Lifetime;
				Type implementationType = descriptor.ImplementationType;
				if (implementationType != null)
				{
					IList<int> orCreateDescriptorIndexes = GetOrCreateDescriptorIndexes(implementationType);
					if (!orCreateDescriptorIndexes.Any())
					{
						AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(implementationType, implementationType, lifetime));
					}
					ServiceDescriptor value2 = new ServiceDescriptor(typeof(TService), (IServiceProvider p) => InjectServices(p, implementationType), lifetime);
					ServiceCollection[item] = value2;
				}
				else if (descriptor.ImplementationFactory != null)
				{
					ServiceDescriptor value3 = new ServiceDescriptor(typeof(TService), (IServiceProvider p) => InjectServices(p, descriptor.ImplementationFactory), lifetime);
					ServiceCollection[item] = value3;
				}
				else
				{
					ServiceDescriptor value4 = new ServiceDescriptor(typeof(TService), (IServiceProvider p) => InjectServices(p, descriptor.ImplementationInstance), lifetime);
					ServiceCollection[item] = value4;
				}
			}
		}
		return this;
	}

	private static object InjectServices(IServiceProvider serviceProvider, Type concreteType)
	{
		object? service = serviceProvider.GetService(concreteType);
		IPatchServiceInjectionSite obj = service as IPatchServiceInjectionSite;
		if (obj != null)
		{
			obj.InjectServices(serviceProvider);
			return service;
		}
		return service;
	}

	private static object InjectServices(IServiceProvider serviceProvider, object service)
	{
		(service as IPatchServiceInjectionSite)?.InjectServices(serviceProvider);
		return service;
	}

	private static object InjectServices(IServiceProvider serviceProvider, Func<IServiceProvider, object> implementationFactory)
	{
		object obj = implementationFactory(serviceProvider);
		IPatchServiceInjectionSite obj2 = obj as IPatchServiceInjectionSite;
		if (obj2 != null)
		{
			obj2.InjectServices(serviceProvider);
			return obj;
		}
		return obj;
	}
}
