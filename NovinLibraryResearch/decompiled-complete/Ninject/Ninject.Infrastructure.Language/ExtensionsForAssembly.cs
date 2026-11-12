using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

namespace Ninject.Infrastructure.Language;

internal static class ExtensionsForAssembly
{
	public static bool HasNinjectModules(this Assembly assembly)
	{
		if (!assembly.IsDynamic)
		{
			return assembly.ExportedTypes.Any(IsLoadableModule);
		}
		return false;
	}

	public static IEnumerable<INinjectModule> GetNinjectModules(this Assembly assembly)
	{
		if (!assembly.IsDynamic)
		{
			return from type in assembly.ExportedTypes.Where(IsLoadableModule)
				select Activator.CreateInstance(type) as INinjectModule;
		}
		return Enumerable.Empty<INinjectModule>();
	}

	private static bool IsLoadableModule(Type type)
	{
		if (typeof(INinjectModule).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
		{
			return type.GetConstructor(Type.EmptyTypes) != null;
		}
		return false;
	}
}
