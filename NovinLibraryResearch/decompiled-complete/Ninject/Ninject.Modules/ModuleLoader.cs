using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninject.Components;
using Ninject.Infrastructure;

namespace Ninject.Modules;

public class ModuleLoader : NinjectComponent, IModuleLoader, INinjectComponent, IDisposable
{
	public IKernel Kernel { get; private set; }

	public ModuleLoader(IKernel kernel)
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Kernel = kernel;
	}

	public void LoadModules(IEnumerable<string> patterns)
	{
		IEnumerable<IModuleLoaderPlugin> all = Kernel.Components.GetAll<IModuleLoaderPlugin>();
		foreach (IGrouping<string, string> item in from filename in patterns.SelectMany((string pattern) => GetFilesMatchingPattern(pattern))
			group filename by Path.GetExtension(filename).ToLowerInvariant())
		{
			string extension = item.Key;
			all.Where((IModuleLoaderPlugin p) => p.SupportedExtensions.Contains(extension)).FirstOrDefault()?.LoadModules(item);
		}
	}

	private static IEnumerable<string> GetFilesMatchingPattern(string pattern)
	{
		return NormalizePaths(Path.GetDirectoryName(pattern)).SelectMany((string path) => Directory.GetFiles(path, Path.GetFileName(pattern)));
	}

	private static IEnumerable<string> NormalizePaths(string path)
	{
		if (!Path.IsPathRooted(path))
		{
			return (from baseDirectory in GetBaseDirectories()
				select Path.Combine(baseDirectory, path)).Where(Directory.Exists);
		}
		return new string[1] { Path.GetFullPath(path) };
	}

	private static IEnumerable<string> GetBaseDirectories()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
		if (!string.IsNullOrEmpty(relativeSearchPath))
		{
			return from path in relativeSearchPath.Split(new char[1] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries)
				select Path.Combine(baseDirectory, path);
		}
		return new string[1] { baseDirectory };
	}
}
