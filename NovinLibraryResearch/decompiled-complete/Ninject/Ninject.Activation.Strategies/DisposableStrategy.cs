using System;

namespace Ninject.Activation.Strategies;

public class DisposableStrategy : ActivationStrategy
{
	public override void Deactivate(IContext context, InstanceReference reference)
	{
		reference.IfInstanceIs(delegate(IDisposable x)
		{
			x.Dispose();
		});
	}
}
