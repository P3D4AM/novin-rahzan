using System.Collections.Generic;
using System.Linq;
using Ninject.Infrastructure;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;

namespace Ninject.Activation.Strategies;

public class MethodInjectionStrategy : ActivationStrategy
{
	public override void Activate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(reference, "reference");
		foreach (MethodInjectionDirective item in context.Plan.GetAll<MethodInjectionDirective>())
		{
			IEnumerable<object> source = item.Targets.Select((ITarget target) => target.ResolveWithin(context));
			item.Injector(reference.Instance, source.ToArray());
		}
	}
}
