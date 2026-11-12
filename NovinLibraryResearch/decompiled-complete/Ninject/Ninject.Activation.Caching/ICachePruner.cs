using System;
using Ninject.Components;

namespace Ninject.Activation.Caching;

public interface ICachePruner : INinjectComponent, IDisposable
{
	void Start(IPruneable cache);

	void Stop();
}
