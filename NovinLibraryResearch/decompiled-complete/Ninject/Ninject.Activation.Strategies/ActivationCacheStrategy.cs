using System;
using Ninject.Activation.Caching;
using Ninject.Components;
using Ninject.Infrastructure;

namespace Ninject.Activation.Strategies;

public class ActivationCacheStrategy : NinjectComponent, IActivationStrategy, INinjectComponent, IDisposable
{
	private readonly IActivationCache activationCache;

	public ActivationCacheStrategy(IActivationCache activationCache)
	{
		Ensure.ArgumentNotNull(activationCache, "activationCache");
		this.activationCache = activationCache;
	}

	public void Activate(IContext context, InstanceReference reference)
	{
		activationCache.AddActivatedInstance(reference.Instance);
	}

	public void Deactivate(IContext context, InstanceReference reference)
	{
		activationCache.AddDeactivatedInstance(reference.Instance);
	}
}
