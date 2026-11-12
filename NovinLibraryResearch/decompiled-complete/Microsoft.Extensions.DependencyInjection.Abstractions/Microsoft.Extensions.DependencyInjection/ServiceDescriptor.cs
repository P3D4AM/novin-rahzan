using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[DebuggerDisplay("{DebuggerToString(),nq}")]
public class ServiceDescriptor
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	private Type _implementationType;

	private object _implementationInstance;

	private object _implementationFactory;

	public ServiceLifetime Lifetime { get; }

	public object? ServiceKey { get; }

	public Type ServiceType { get; }

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public Type? ImplementationType
	{
		get
		{
			if (IsKeyedService)
			{
				ThrowKeyedDescriptor();
			}
			return _implementationType;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	public Type? KeyedImplementationType
	{
		get
		{
			if (!IsKeyedService)
			{
				ThrowNonKeyedDescriptor();
			}
			return _implementationType;
		}
	}

	public object? ImplementationInstance
	{
		get
		{
			if (IsKeyedService)
			{
				ThrowKeyedDescriptor();
			}
			return _implementationInstance;
		}
	}

	public object? KeyedImplementationInstance
	{
		get
		{
			if (!IsKeyedService)
			{
				ThrowNonKeyedDescriptor();
			}
			return _implementationInstance;
		}
	}

	public Func<IServiceProvider, object>? ImplementationFactory
	{
		get
		{
			if (IsKeyedService)
			{
				ThrowKeyedDescriptor();
			}
			return (Func<IServiceProvider, object>)_implementationFactory;
		}
	}

	public Func<IServiceProvider, object?, object>? KeyedImplementationFactory
	{
		get
		{
			if (!IsKeyedService)
			{
				ThrowNonKeyedDescriptor();
			}
			return (Func<IServiceProvider, object, object>)_implementationFactory;
		}
	}

	public bool IsKeyedService => ServiceKey != null;

	public ServiceDescriptor(Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
		: this(serviceType, null, implementationType, lifetime)
	{
	}

	public ServiceDescriptor(Type serviceType, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
		: this(serviceType, serviceKey, lifetime)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
		_implementationType = implementationType;
	}

	public ServiceDescriptor(Type serviceType, object instance)
		: this(serviceType, null, instance)
	{
	}

	public ServiceDescriptor(Type serviceType, object? serviceKey, object instance)
		: this(serviceType, serviceKey, ServiceLifetime.Singleton)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(instance, "instance");
		_implementationInstance = instance;
	}

	public ServiceDescriptor(Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
		: this(serviceType, (object)null, lifetime)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(factory, "factory");
		_implementationFactory = factory;
	}

	public ServiceDescriptor(Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> factory, ServiceLifetime lifetime)
	{
		Func<IServiceProvider, object, object> factory2 = factory;
		this._002Ector(serviceType, serviceKey, lifetime);
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(factory2, "factory");
		if (serviceKey == null)
		{
			Func<IServiceProvider, object> implementationFactory = (IServiceProvider sp) => factory2(sp, null);
			_implementationFactory = implementationFactory;
		}
		else
		{
			_implementationFactory = factory2;
		}
	}

	private ServiceDescriptor(Type serviceType, object serviceKey, ServiceLifetime lifetime)
	{
		Lifetime = lifetime;
		ServiceType = serviceType;
		ServiceKey = serviceKey;
	}

	public override string ToString()
	{
		string text = string.Format("{0}: {1} {2}: {3} ", "ServiceType", ServiceType, "Lifetime", Lifetime);
		if (IsKeyedService)
		{
			text += string.Format("{0}: {1} ", "ServiceKey", ServiceKey);
			if (KeyedImplementationType != null)
			{
				return text + string.Format("{0}: {1}", "KeyedImplementationType", KeyedImplementationType);
			}
			if (KeyedImplementationFactory != null)
			{
				return text + string.Format("{0}: {1}", "KeyedImplementationFactory", KeyedImplementationFactory.Method);
			}
			return text + string.Format("{0}: {1}", "KeyedImplementationInstance", KeyedImplementationInstance);
		}
		if (ImplementationType != null)
		{
			return text + string.Format("{0}: {1}", "ImplementationType", ImplementationType);
		}
		if (ImplementationFactory != null)
		{
			return text + string.Format("{0}: {1}", "ImplementationFactory", ImplementationFactory.Method);
		}
		return text + string.Format("{0}: {1}", "ImplementationInstance", ImplementationInstance);
	}

	internal Type GetImplementationType()
	{
		if (ServiceKey == null)
		{
			if (ImplementationType != null)
			{
				return ImplementationType;
			}
			if (ImplementationInstance != null)
			{
				return ImplementationInstance.GetType();
			}
			if (ImplementationFactory != null)
			{
				Type[] genericTypeArguments = ImplementationFactory.GetType().GenericTypeArguments;
				return genericTypeArguments[1];
			}
		}
		else
		{
			if (KeyedImplementationType != null)
			{
				return KeyedImplementationType;
			}
			if (KeyedImplementationInstance != null)
			{
				return KeyedImplementationInstance.GetType();
			}
			if (KeyedImplementationFactory != null)
			{
				Type[] genericTypeArguments2 = KeyedImplementationFactory.GetType().GenericTypeArguments;
				return genericTypeArguments2[2];
			}
		}
		return null;
	}

	public static ServiceDescriptor Transient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor KeyedTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object? serviceKey) where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor Transient(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
		return Describe(service, implementationType, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor KeyedTransient(Type service, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
		return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor Transient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor KeyedTransient<TService, TImplementation>(object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor KeyedTransient<TService>(object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(service, implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor KeyedTransient(Type service, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(service, serviceKey, implementationFactory, ServiceLifetime.Transient);
	}

	public static ServiceDescriptor Scoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor KeyedScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object? serviceKey) where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor Scoped(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		return Describe(service, implementationType, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor KeyedScoped(Type service, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor Scoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor KeyedScoped<TService, TImplementation>(object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor Scoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor KeyedScoped<TService>(object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor Scoped(Type service, Func<IServiceProvider, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(service, implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor KeyedScoped(Type service, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(service, serviceKey, implementationFactory, ServiceLifetime.Scoped);
	}

	public static ServiceDescriptor Singleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>((object)null, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor KeyedSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object? serviceKey) where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed<TService, TImplementation>(serviceKey, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor Singleton(Type service, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
		return Describe(service, implementationType, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor KeyedSingleton(Type service, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
	{
		System.ThrowHelper.ThrowIfNull(service, "service");
		System.ThrowHelper.ThrowIfNull(implementationType, "implementationType");
		return DescribeKeyed(service, serviceKey, implementationType, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor Singleton<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor KeyedSingleton<TService, TImplementation>(object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor Singleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor KeyedSingleton<TService>(object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(typeof(TService), serviceKey, implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor Singleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return Describe(serviceType, implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor KeyedSingleton(Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(implementationFactory, "implementationFactory");
		return DescribeKeyed(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
	}

	public static ServiceDescriptor Singleton<TService>(TService implementationInstance) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
		return Singleton(typeof(TService), implementationInstance);
	}

	public static ServiceDescriptor KeyedSingleton<TService>(object? serviceKey, TService implementationInstance) where TService : class
	{
		System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
		return KeyedSingleton(typeof(TService), serviceKey, implementationInstance);
	}

	public static ServiceDescriptor Singleton(Type serviceType, object implementationInstance)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
		return new ServiceDescriptor(serviceType, implementationInstance);
	}

	public static ServiceDescriptor KeyedSingleton(Type serviceType, object? serviceKey, object implementationInstance)
	{
		System.ThrowHelper.ThrowIfNull(serviceType, "serviceType");
		System.ThrowHelper.ThrowIfNull(implementationInstance, "implementationInstance");
		return new ServiceDescriptor(serviceType, serviceKey, implementationInstance);
	}

	private static ServiceDescriptor DescribeKeyed<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(object serviceKey, ServiceLifetime lifetime) where TService : class where TImplementation : class, TService
	{
		return DescribeKeyed(typeof(TService), serviceKey, typeof(TImplementation), lifetime);
	}

	public static ServiceDescriptor Describe(Type serviceType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
	{
		return new ServiceDescriptor(serviceType, implementationType, lifetime);
	}

	public static ServiceDescriptor DescribeKeyed(Type serviceType, object? serviceKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, ServiceLifetime lifetime)
	{
		return new ServiceDescriptor(serviceType, serviceKey, implementationType, lifetime);
	}

	public static ServiceDescriptor Describe(Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime)
	{
		return new ServiceDescriptor(serviceType, implementationFactory, lifetime);
	}

	public static ServiceDescriptor DescribeKeyed(Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory, ServiceLifetime lifetime)
	{
		return new ServiceDescriptor(serviceType, serviceKey, implementationFactory, lifetime);
	}

	private string DebuggerToString()
	{
		string text = $"Lifetime = {Lifetime}, ServiceType = \"{ServiceType.FullName}\"";
		if (IsKeyedService)
		{
			text += $", ServiceKey = \"{ServiceKey}\"";
			if (KeyedImplementationType != null)
			{
				return text + ", KeyedImplementationType = \"" + KeyedImplementationType.FullName + "\"";
			}
			if (KeyedImplementationFactory != null)
			{
				return text + $", KeyedImplementationFactory = {KeyedImplementationFactory.Method}";
			}
			return text + $", KeyedImplementationInstance = {KeyedImplementationInstance}";
		}
		if (ImplementationType != null)
		{
			return text + ", ImplementationType = \"" + ImplementationType.FullName + "\"";
		}
		if (ImplementationFactory != null)
		{
			return text + $", ImplementationFactory = {ImplementationFactory.Method}";
		}
		return text + $", ImplementationInstance = {ImplementationInstance}";
	}

	private static void ThrowKeyedDescriptor()
	{
		throw new InvalidOperationException(System.SR.KeyedDescriptorMisuse);
	}

	private static void ThrowNonKeyedDescriptor()
	{
		throw new InvalidOperationException(System.SR.NonKeyedDescriptorMisuse);
	}
}
