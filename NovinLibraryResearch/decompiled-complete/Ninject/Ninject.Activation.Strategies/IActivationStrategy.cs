using System;
using Ninject.Components;

namespace Ninject.Activation.Strategies;

public interface IActivationStrategy : INinjectComponent, IDisposable
{
	void Activate(IContext context, InstanceReference reference);

	void Deactivate(IContext context, InstanceReference reference);
}
