using System;
using System.Collections.Generic;
using Ninject.Components;

namespace Ninject.Modules;

public interface IModuleLoaderPlugin : INinjectComponent, IDisposable
{
	IEnumerable<string> SupportedExtensions { get; }

	void LoadModules(IEnumerable<string> filenames);
}
