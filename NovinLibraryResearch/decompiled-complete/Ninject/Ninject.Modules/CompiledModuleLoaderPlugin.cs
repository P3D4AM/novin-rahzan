using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Modules;

public class CompiledModuleLoaderPlugin : NinjectComponent, IModuleLoaderPlugin, INinjectComponent, IDisposable
{
	private static readonly string[] Extensions = new string[1] { ".dll" };

	private readonly IAssemblyNameRetriever assemblyNameRetriever;

	public IKernel Kernel { get; private set; }

	public IEnumerable<string> SupportedExtensions => Extensions;

	public CompiledModuleLoaderPlugin(IKernel kernel, IAssemblyNameRetriever assemblyNameRetriever)
	{
		Ensure.ArgumentNotNull(kernel, "kernel");
		Ensure.ArgumentNotNull(assemblyNameRetriever, "assemblyNameRetriever");
		Kernel = kernel;
		this.assemblyNameRetriever = assemblyNameRetriever;
	}

	public void LoadModules(IEnumerable<string> filenames)
	{
		IEnumerable<AssemblyName> assemblyNames = assemblyNameRetriever.GetAssemblyNames(filenames, (Assembly asm) => asm.HasNinjectModules());
		Kernel.Load(assemblyNames.Select((AssemblyName asm) => Assembly.Load(asm)));
	}
}
