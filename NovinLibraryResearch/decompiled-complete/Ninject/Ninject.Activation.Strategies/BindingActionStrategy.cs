using System;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Activation.Strategies;

public class BindingActionStrategy : ActivationStrategy
{
	public override void Activate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		context.Binding.ActivationActions.Map(delegate(Action<IContext, object> action)
		{
			action(context, reference.Instance);
		});
	}

	public override void Deactivate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		context.Binding.DeactivationActions.Map(delegate(Action<IContext, object> action)
		{
			action(context, reference.Instance);
		});
	}
}
