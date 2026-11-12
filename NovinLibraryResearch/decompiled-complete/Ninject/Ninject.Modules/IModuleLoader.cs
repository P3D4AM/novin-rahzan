using System;
using System.Collections.Generic;
using Ninject.Components;

namespace Ninject.Modules;

public interface IModuleLoader : INinjectComponent, IDisposable
{
	void LoadModules(IEnumerable<string> patterns);
}
