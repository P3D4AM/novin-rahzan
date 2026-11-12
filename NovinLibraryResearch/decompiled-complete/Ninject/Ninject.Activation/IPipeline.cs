using System;
using System.Collections.Generic;
using Ninject.Activation.Strategies;
using Ninject.Components;

namespace Ninject.Activation;

public interface IPipeline : INinjectComponent, IDisposable
{
	IList<IActivationStrategy> Strategies { get; }

	void Activate(IContext context, InstanceReference reference);

	void Deactivate(IContext context, InstanceReference reference);
}
