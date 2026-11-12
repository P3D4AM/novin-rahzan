using System.Reflection;
using Ninject.Infrastructure;
using Ninject.Modules;

namespace Ninject;

public static class ModuleLoadExtensions
{
	public static void Load<TModule>(this IKernel kernel) where TModule : INinjectModule, new()
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Load(kernel, new TModule());
	}

	public static void Load(this IKernel kernel, params INinjectModule[] modules)
	{
		kernel.Load(modules);
	}

	public static void Load(this IKernel kernel, params string[] filePatterns)
	{
		kernel.Load(filePatterns);
	}

	public static void Load(this IKernel kernel, params Assembly[] assemblies)
	{
		kernel.Load(assemblies);
	}
}
