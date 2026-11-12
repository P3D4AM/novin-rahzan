using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ServiceCollectionMap : IInfrastructure<InternalServiceCollectionMap>
{
	private readonly InternalServiceCollectionMap _map;

	public virtual IServiceCollection ServiceCollection => _map.ServiceCollection;

	InternalServiceCollectionMap IInfrastructure<InternalServiceCollectionMap>.Instance => _map;

	public ServiceCollectionMap([JetBrains.Annotations.NotNull] IServiceCollection serviceCollection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceCollection, "serviceCollection");
		_map = new InternalServiceCollectionMap(serviceCollection);
	}

	public virtual ServiceCollectionMap TryAddTransient<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddTransient([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAdd(serviceType, implementationType, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScoped([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAdd(serviceType, implementationType, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingleton([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAdd(serviceType, implementationType, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAdd([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType, ServiceLifetime lifetime)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementationType, "implementationType");
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.Count == 0)
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, implementationType, lifetime));
		}
		return this;
	}

	public virtual ServiceCollectionMap TryAddTransient<TService>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TService> factory) where TService : class
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScoped<TService>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TService> factory) where TService : class
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingleton<TService>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TService> factory) where TService : class
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddTransient<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScoped<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingleton<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAdd(typeof(TService), factory, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddTransient([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory)
	{
		return TryAdd(serviceType, factory, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScoped([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory)
	{
		return TryAdd(serviceType, factory, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingleton([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory)
	{
		return TryAdd(serviceType, factory, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAdd([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.Count == 0)
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, factory, lifetime));
		}
		return this;
	}

	public virtual ServiceCollectionMap TryAddSingleton<TService>([JetBrains.Annotations.CanBeNull] TService implementation) where TService : class
	{
		return TryAddSingleton(typeof(TService), implementation);
	}

	public virtual ServiceCollectionMap TryAddSingleton([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.CanBeNull] object implementation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.Count == 0)
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, implementation));
		}
		return this;
	}

	public virtual ServiceCollectionMap TryAddTransientEnumerable<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScopedEnumerable<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingletonEnumerable<TService, TImplementation>() where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddTransientEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAddEnumerable(serviceType, implementationType, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScopedEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAddEnumerable(serviceType, implementationType, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingletonEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType)
	{
		return TryAddEnumerable(serviceType, implementationType, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType, ServiceLifetime lifetime)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementationType, "implementationType");
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.All((int i) => TryGetImplementationType(ServiceCollection[i]) != implementationType))
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, implementationType, lifetime));
		}
		return this;
	}

	public virtual ServiceCollectionMap TryAddTransientEnumerable<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Transient);
	}

	public virtual ServiceCollectionMap TryAddScopedEnumerable<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Scoped);
	}

	public virtual ServiceCollectionMap TryAddSingletonEnumerable<TService, TImplementation>([JetBrains.Annotations.NotNull] Func<IServiceProvider, TImplementation> factory) where TService : class where TImplementation : class, TService
	{
		return TryAddEnumerable(typeof(TService), typeof(TImplementation), factory, ServiceLifetime.Singleton);
	}

	public virtual ServiceCollectionMap TryAddEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] Type implementationType, [JetBrains.Annotations.NotNull] Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementationType, "implementationType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.All((int i) => TryGetImplementationType(ServiceCollection[i]) != implementationType))
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, factory, lifetime));
		}
		return this;
	}

	public virtual ServiceCollectionMap TryAddSingletonEnumerable<TService>([JetBrains.Annotations.NotNull] TService implementation) where TService : class
	{
		return TryAddSingletonEnumerable(typeof(TService), implementation);
	}

	public virtual ServiceCollectionMap TryAddSingletonEnumerable([JetBrains.Annotations.NotNull] Type serviceType, [JetBrains.Annotations.NotNull] object implementation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(serviceType, "serviceType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(implementation, "implementation");
		Type implementationType = implementation.GetType();
		IList<int> orCreateDescriptorIndexes = _map.GetOrCreateDescriptorIndexes(serviceType);
		if (orCreateDescriptorIndexes.All((int i) => TryGetImplementationType(ServiceCollection[i]) != implementationType))
		{
			_map.AddNewDescriptor(orCreateDescriptorIndexes, new ServiceDescriptor(serviceType, implementation));
		}
		return this;
	}

	private static Type TryGetImplementationType(ServiceDescriptor descriptor)
	{
		object obj = descriptor.ImplementationType;
		if (obj == null)
		{
			obj = descriptor.ImplementationInstance?.GetType();
			if (obj == null)
			{
				Func<IServiceProvider, object>? implementationFactory = descriptor.ImplementationFactory;
				if (implementationFactory == null)
				{
					return null;
				}
				obj = implementationFactory.GetType().GetTypeInfo().GenericTypeArguments[1];
			}
		}
		return (Type)obj;
	}
}
