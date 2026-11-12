using System;
using System.Reflection;

namespace Microsoft.Extensions.Hosting;

internal class HostFactoryResolver
{
	public static readonly string BuildWebHost = "BuildWebHost";

	public static readonly string CreateWebHostBuilder = "CreateWebHostBuilder";

	public static readonly string CreateHostBuilder = "CreateHostBuilder";

	public static Func<string[], TWebHost> ResolveWebHostFactory<TWebHost>(Assembly assembly)
	{
		return ResolveFactory<TWebHost>(assembly, BuildWebHost);
	}

	public static Func<string[], TWebHostBuilder> ResolveWebHostBuilderFactory<TWebHostBuilder>(Assembly assembly)
	{
		return ResolveFactory<TWebHostBuilder>(assembly, CreateWebHostBuilder);
	}

	public static Func<string[], THostBuilder> ResolveHostBuilderFactory<THostBuilder>(Assembly assembly)
	{
		return ResolveFactory<THostBuilder>(assembly, CreateHostBuilder);
	}

	private static Func<string[], T> ResolveFactory<T>(Assembly assembly, string name)
	{
		Type type = assembly?.EntryPoint?.DeclaringType;
		if (type == null)
		{
			return null;
		}
		MethodInfo factory = type.GetTypeInfo().GetDeclaredMethod(name);
		if (!IsFactory<T>(factory))
		{
			return null;
		}
		return (string[] args) => (T)factory.Invoke(null, new object[1] { args });
	}

	private static bool IsFactory<TReturn>(MethodInfo factory)
	{
		if (factory != null && typeof(TReturn).IsAssignableFrom(factory.ReturnType) && factory.GetParameters().Length == 1)
		{
			return typeof(string[]).Equals(factory.GetParameters()[0].ParameterType);
		}
		return false;
	}

	public static Func<string[], IServiceProvider> ResolveServiceProviderFactory(Assembly assembly)
	{
		Func<string[], object> webHostFactory = ResolveWebHostFactory<object>(assembly);
		if (webHostFactory != null)
		{
			return (string[] args) => GetServiceProvider(webHostFactory(args));
		}
		Func<string[], object> webHostBuilderFactory = ResolveWebHostBuilderFactory<object>(assembly);
		if (webHostBuilderFactory != null)
		{
			return (string[] args) => GetServiceProvider(Build(webHostBuilderFactory(args)));
		}
		Func<string[], object> hostBuilderFactory = ResolveHostBuilderFactory<object>(assembly);
		if (hostBuilderFactory != null)
		{
			return (string[] args) => GetServiceProvider(Build(hostBuilderFactory(args)));
		}
		return null;
	}

	private static object Build(object builder)
	{
		return builder.GetType().GetMethod("Build")?.Invoke(builder, Array.Empty<object>());
	}

	private static IServiceProvider GetServiceProvider(object host)
	{
		if (host == null)
		{
			return null;
		}
		return (IServiceProvider)host.GetType().GetTypeInfo().GetDeclaredProperty("Services")
			.GetValue(host);
	}
}
