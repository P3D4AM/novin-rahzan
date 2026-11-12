using System;
using Ninject.Components;

namespace Ninject.Activation.Caching;

public interface IActivationCache : INinjectComponent, IDisposable
{
	void Clear();

	void AddActivatedInstance(object instance);

	void AddDeactivatedInstance(object instance);

	bool IsActivated(object instance);

	bool IsDeactivated(object instance);
}
