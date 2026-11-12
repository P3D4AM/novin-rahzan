using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Components;

namespace Ninject.Modules;

public interface IAssemblyNameRetriever : INinjectComponent, IDisposable
{
	IEnumerable<AssemblyName> GetAssemblyNames(IEnumerable<string> filenames, Predicate<Assembly> filter);
}
