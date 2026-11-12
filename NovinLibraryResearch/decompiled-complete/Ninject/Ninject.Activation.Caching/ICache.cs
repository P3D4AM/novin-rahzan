using System;
using Ninject.Components;

namespace Ninject.Activation.Caching;

public interface ICache : INinjectComponent, IDisposable, IPruneable
{
	int Count { get; }

	void Remember(IContext context, InstanceReference reference);

	object TryGet(IContext context);

	bool Release(object instance);

	void Clear(object scope);

	void Clear();
}
