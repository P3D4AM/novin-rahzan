using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Activation.Caching;
using Ninject.Activation.Strategies;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;

namespace Ninject.Activation;

public class Pipeline : NinjectComponent, IPipeline, INinjectComponent, IDisposable
{
	private readonly IActivationCache activationCache;

	public IList<IActivationStrategy> Strategies { get; private set; }

	public Pipeline(IEnumerable<IActivationStrategy> strategies, IActivationCache activationCache)
	{
		Ensure.ArgumentNotNull(strategies, "strategies");
		Ensure.ArgumentNotNull(activationCache, "activationCache");
		Strategies = strategies.ToList();
		this.activationCache = activationCache;
	}

	public void Activate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(reference, "reference");
		if (!activationCache.IsActivated(reference.Instance))
		{
			Strategies.Map(delegate(IActivationStrategy s)
			{
				s.Activate(context, reference);
			});
		}
	}

	public void Deactivate(IContext context, InstanceReference reference)
	{
		Ensure.ArgumentNotNull(context, "context");
		Ensure.ArgumentNotNull(reference, "reference");
		if (!activationCache.IsDeactivated(reference.Instance))
		{
			Strategies.Map(delegate(IActivationStrategy s)
			{
				s.Deactivate(context, reference);
			});
		}
	}
}
