using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsServiceCollectionExtensions
{
	public static IServiceCollection AddOptions(this IServiceCollection services)
	{
		System.ThrowHelper.ThrowIfNull(services, "services");
		services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(UnnamedOptionsManager<>)));
		services.TryAdd(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>)));
		services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));
		services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
		services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>)));
		return services;
	}

	public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
	{
		return services.Configure(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
	}

	public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string? name, Action<TOptions> configureOptions) where TOptions : class
	{
		System.ThrowHelper.ThrowIfNull(services, "services");
		System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
		services.AddOptions();
		services.AddSingleton((IConfigureOptions<TOptions>)new ConfigureNamedOptions<TOptions>(name, configureOptions));
		return services;
	}

	public static IServiceCollection ConfigureAll<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
	{
		return services.Configure(null, configureOptions);
	}

	public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
	{
		return services.PostConfigure(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
	}

	public static IServiceCollection PostConfigure<TOptions>(this IServiceCollection services, string? name, Action<TOptions> configureOptions) where TOptions : class
	{
		System.ThrowHelper.ThrowIfNull(services, "services");
		System.ThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
		services.AddOptions();
		services.AddSingleton((IPostConfigureOptions<TOptions>)new PostConfigureOptions<TOptions>(name, configureOptions));
		return services;
	}

	public static IServiceCollection PostConfigureAll<TOptions>(this IServiceCollection services, Action<TOptions> configureOptions) where TOptions : class
	{
		return services.PostConfigure(null, configureOptions);
	}

	public static IServiceCollection ConfigureOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConfigureOptions>(this IServiceCollection services) where TConfigureOptions : class
	{
		return services.ConfigureOptions(typeof(TConfigureOptions));
	}

	private static IEnumerable<Type> FindConfigurationServices(Type type)
	{
		Type[] array = GetInterfacesOnType(type);
		foreach (Type type2 in array)
		{
			if (type2.IsGenericType)
			{
				Type genericTypeDefinition = type2.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(IConfigureOptions<>) || genericTypeDefinition == typeof(IPostConfigureOptions<>) || genericTypeDefinition == typeof(IValidateOptions<>))
				{
					yield return type2;
				}
			}
		}
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2070:UnrecognizedReflectionPattern", Justification = "This method only looks for interfaces referenced in its code. The trimmer will keep the interface and thus all of its implementations in that case. The call to GetInterfaces may return less results in trimmed apps, but it will include the interfaces this method looks for if they should be there.")]
		static Type[] GetInterfacesOnType(Type t)
		{
			return t.GetInterfaces();
		}
	}

	private static void ThrowNoConfigServices(Type type)
	{
		throw new InvalidOperationException((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>)) ? System.SR.Error_NoConfigurationServicesAndAction : System.SR.Error_NoConfigurationServices);
	}

	public static IServiceCollection ConfigureOptions(this IServiceCollection services, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type configureType)
	{
		services.AddOptions();
		bool flag = false;
		foreach (Type item in FindConfigurationServices(configureType))
		{
			services.AddTransient(item, configureType);
			flag = true;
		}
		if (!flag)
		{
			ThrowNoConfigServices(configureType);
		}
		return services;
	}

	public static IServiceCollection ConfigureOptions(this IServiceCollection services, object configureInstance)
	{
		services.AddOptions();
		Type type = configureInstance.GetType();
		bool flag = false;
		foreach (Type item in FindConfigurationServices(type))
		{
			services.AddSingleton(item, configureInstance);
			flag = true;
		}
		if (!flag)
		{
			ThrowNoConfigServices(type);
		}
		return services;
	}

	public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services) where TOptions : class
	{
		return services.AddOptions<TOptions>(Microsoft.Extensions.Options.Options.DefaultName);
	}

	public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services, string? name) where TOptions : class
	{
		System.ThrowHelper.ThrowIfNull(services, "services");
		services.AddOptions();
		return new OptionsBuilder<TOptions>(services, name);
	}
}
